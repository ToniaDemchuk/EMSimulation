using System.Linq;
using System.Numerics;
using System.Threading;

using AwokeKnowing.GnuplotCSharp;

using Simulation.FDTD.Models;
using Simulation.Models.Constants;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;

namespace Simulation.FDTD
{
    /// <summary>
    ///     The FDTDSimulation class.
    /// </summary>
    public class FDTDSimulation
    {
        private FDTDField fields;

        private PmlBoundary pml;

        private FDTDPulse pulse;

        /// <summary>
        ///     Calculates using the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The simulation result.</returns>
        public SimulationResultDictionary Calculate(SimulationParameters parameters)
        {
            this.initParams(parameters);
            var gp = new GnuPlot();
            for (var time = 0; time < parameters.NumSteps; time++)
            {
                this.calcFields(time, parameters);

                //var surf = new double[parameters.Indices.ILength, parameters.Indices.JLength];

                //parameters.Indices.ForExceptK(
                //    (i, j) =>
                //    {
                //        surf[i, j] = this.fields.E[i, j, parameters.Indices.GetCenter().KLength].Z;
                //    });
                //gp.SPlot(surf);

                //gp.Plot(pulse.E);
            }

            var result = this.CalcExtinction(parameters);
            //gp.Plot(result.Select(x=>x.Value.EffectiveCrossSectionAbsorption));
            //gp.Wait();
            return result;
        }

        /// <summary>
        /// Calculates the extinction.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The simulation results.</returns>
        public SimulationResultDictionary CalcExtinction(SimulationParameters parameters)
        {
            return parameters.Spectrum.ToSimulationResult(x => this.calculateExtinction(x, parameters));
        }

        private void initParams(SimulationParameters parameters)
        {
            this.fields = new FDTDField(parameters);
            this.pulse = new FDTDPulse(parameters);

            this.pml = new PmlBoundary(parameters.PmlLength, parameters.Indices);
        }

        private void calcFields(int time, SimulationParameters parameters)
        {
            // границі розсіяного поля.
            IndexStore pulseIndex = parameters.Indices.ShiftLower(this.pml.Length + 1).ShiftUpper(this.pml.Length + 1);

            this.calculateDField(parameters, pulseIndex, time);

            // Calculate the E from D field
            parameters.Indices.ShiftLower(1).ShiftUpper(1).ParallelLinqFor(
                (i, j, k) => { this.fields.E[i, j, k] = parameters.Medium[i, j, k].Solve(this.fields.D[i, j, k]); });

            this.fields.DoFourierField();
            this.pulse.DoFourierPulse();

            this.calculateHField(parameters, pulseIndex);
        }

        private void calculateHField(SimulationParameters param, IndexStore pulseIndex)
        {
            param.Indices.ShiftUpper(1).ParallelLinqFor(
                (i, j, k) =>
                {
                    CartesianCoordinate curlE = this.fields.E.Curl(i, j, k, +1);
                    this.fields.IntegralH[i, j, k] = this.fields.IntegralH[i, j, k] + curlE;

                    var coefs = this.pml.Magnetic[i, j, k];
                    this.fields.H[i, j, k] =
                        coefs.FieldFactor(this.fields.H[i, j, k])
                        + param.CourantNumber * coefs.CurlFactor(
                            curlE + coefs.IntegralFactor(this.fields.IntegralH[i, j, k]));
                });

            this.pulse.MagneticFieldStepCalc();

            this.addPulseToH1(pulseIndex, param.CourantNumber);

            this.addPulseToH2(pulseIndex, param.CourantNumber);
        }

        private void addPulseToH2(IndexStore pulseIndex, double courantNumber)
        {
            pulseIndex.ParallelForExceptI(
                (j, k) =>
                {
                    var cartesian = CartesianCoordinate.YOrth * (courantNumber * this.pulse.E[j]);
                    this.fields.H[pulseIndex.Lower - 1, j, k] -= cartesian;
                    this.fields.H[pulseIndex.Lower, j, k] -= cartesian;
                });
        }

        private void addPulseToH1(IndexStore pulseIndex, double courantNumber)
        {
            var cartesianCoordinate = CartesianCoordinate.XOrth *
                                      (courantNumber * this.pulse.E[pulseIndex.Lower]);
            pulseIndex.ParallelForExceptJ(
                (i, k) =>
                {
                    this.fields.H[i, pulseIndex.Lower - 1, k] += cartesianCoordinate;
                    this.fields.H[i, pulseIndex.Lower, k] -= cartesianCoordinate;
                });
        }

        private void calculateDField(SimulationParameters param, IndexStore pulseIndex, int time)
        {
            param.Indices.ShiftLower(1).ParallelLinqFor(
                (i, j, k) =>
                {
                    CartesianCoordinate curlH = this.fields.H.Curl(i, j, k, -1);
                    this.fields.IntegralD[i, j, k] += curlH;

                    var pmlCoefs = this.pml.Electric[i, j, k];
                    this.fields.D[i, j, k] =
                        pmlCoefs.FieldFactor(this.fields.D[i, j, k]) +
                        param.CourantNumber * pmlCoefs.CurlFactor(
                            curlH + pmlCoefs.IntegralFactor(this.fields.IntegralD[i, j, k]));
                });

            this.pulse.ElectricFieldStepCalc(time);

            this.addPulseToD1(pulseIndex, param.CourantNumber);

            this.addPulseToD2(pulseIndex, param.CourantNumber);
        }

        private void addPulseToD1(IndexStore pulseIndex, double courantNumber)
        {
            pulseIndex.ParallelForExceptK(
                (i, j) =>
                {
                    var cartesianCoordinate = CartesianCoordinate.YOrth * (courantNumber * this.pulse.H[j]);
                    this.fields.D[i, j, pulseIndex.Lower] -= cartesianCoordinate;
                    this.fields.D[i, j, pulseIndex.Lower + 1] -= cartesianCoordinate;
                });
        }

        private void addPulseToD2(IndexStore pulseIndex, double courantNumber)
        {
            var cart = CartesianCoordinate.ZOrth * (courantNumber *
                                                    (this.pulse.H[pulseIndex.Lower - 1] -
                                                     this.pulse.H[pulseIndex.JLength]));
            pulseIndex.ParallelForExceptJ(
                (i, k) => { this.fields.D[i, pulseIndex.Lower, k] += cart; });
        }

        private SimulationResult calculateExtinction(SpectrumUnit freq, SimulationParameters parameters)
        {
            double timeStep = parameters.CellSize * parameters.CourantNumber / Fundamentals.SpeedOfLight;
            var pulseFourier = this.pulse.FourierPulse.Select(x => x.Transform(freq, timeStep).Magnitude).ToArray();
            double extinction = 0;
            object obj = new object();
            parameters.Indices.ParallelLinqFor(
                (i, j, k) =>
                {
                    if (!parameters.Medium[i, j, k].IsBody)
                    {
                        return;
                    }
                    Complex eps = parameters.Medium[i, j, k].Permittivity.GetPermittivity(freq);

                    double pulseMultiplier = 1 / pulseFourier[j];
                    var complex = eps.Imaginary *
                                  pulseMultiplier * this.fields.FourierField[i, j, k].Transform(freq, timeStep).Norm;

                    lock (obj)
                    {
                        extinction += complex;
                    }
                });

            double area = calculateArea(parameters);

            var resu = new SimulationResult();

            double waveNumber = freq.ToType(SpectrumUnitType.WaveNumber);

            extinction *= waveNumber * parameters.CellSize * parameters.CellSize * parameters.CellSize;
            resu.CrossSectionAbsorption = extinction;

            extinction /= area * parameters.CellSize * parameters.CellSize;
            resu.EffectiveCrossSectionAbsorption = extinction;

            return resu;
        }

        private static double calculateArea(SimulationParameters parameters)
        {
            double area = 0.0;
            parameters.Indices.ForExceptJ(
                (i, k) => area += parameters.Medium[i, parameters.Indices.GetCenter().JLength, k].IsBody ? 1 : 0);
            return area;
        }
    }
}