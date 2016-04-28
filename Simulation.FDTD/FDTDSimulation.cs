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
using Simulation.Infrastructure.Iterators;
using GnuplotCSharp;
using System;
using Simulation.Infrastructure;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Simulation.FDTD.EventArgs;

namespace Simulation.FDTD
{
    /// <summary>
    ///     The FDTDSimulation class.
    /// </summary>
    public class FDTDSimulation
    {
        private readonly IIterator iterator;
        
        public FDTDSimulation(IIterator iterator)
        {
            this.iterator = iterator;
        }
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
                this.OnTimeStepCalculated(new TimeStepCalculatedEventArgs() {Parameters = parameters,Fields = this.fields, Pulse = this.pulse});
            }
            
            var result = this.CalcExtinction(parameters);
           
            return result;
        }

        /// <summary>
        /// Calculates the extinction.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The simulation results.</returns>
        public SimulationResultDictionary CalcExtinction(SimulationParameters parameters)
        {
            if (parameters.IsSpectrumCalculated)
            {
            return parameters.Spectrum.ToSimulationResult(x => this.calculateExtinction(x, parameters));
        }
            return new SimulationResultDictionary();
        }

        private void initParams(SimulationParameters parameters)
        {
            this.fields = new FDTDField(parameters, iterator);
            this.pulse = new FDTDPulse(parameters);

            this.pml = new PmlBoundary(parameters.PmlLength, parameters.Indices);
        }

        private void calcFields(int time, SimulationParameters parameters)
        {
            // границі розсіяного поля.
            var pulseShift = this.pml.Length + 2;
            IndexStore pulseIndex = parameters.Indices.ShiftLower(pulseShift).ShiftUpper(pulseShift);

            this.calculateDField(parameters, pulseIndex, time);

            // Calculate the E from D field
            iterator.For(parameters.Indices.ShiftLower(1).ShiftUpper(1),
                (i, j, k) => { this.fields.E[i, j, k] = parameters.Medium[i, j, k].Solve(this.fields.D[i, j, k]); });

            this.fields.DoFourierField();
            this.pulse.DoFourierPulse();

            this.calculateHField(parameters, pulseIndex);
        }

        private void calculateHField(SimulationParameters param, IndexStore pulseIndex)
        {
            iterator.For(param.Indices.ShiftUpper(1),
                (i, j, k) =>
                {
                    CartesianCoordinate curlE = this.fields.E.Curl(i, j, k, +1);
                    this.fields.IntegralH[i, j, k] = this.fields.IntegralH[i, j, k] + curlE;

                    var coefs = this.pml.Magnetic(i, j, k);
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
            this.iterator.ForExceptI(
                pulseIndex,
                (j, k) =>
                {
                    var cartesian = CartesianCoordinate.YOrth * (courantNumber * this.pulse.E[j]);
                    this.fields.H[pulseIndex.Lower - 1, j, k] -= cartesian;
                    this.fields.H[pulseIndex.ILength, j, k] += cartesian;
                });
        }

        private void addPulseToH1(IndexStore pulseIndex, double courantNumber)
        {
            var cartesianCoordinate = CartesianCoordinate.XOrth *
                                      (courantNumber * this.pulse.E[pulseIndex.Lower]);
            var cartesianCoordinate2 = CartesianCoordinate.XOrth *
                          (courantNumber * this.pulse.E[pulseIndex.JLength]);
            this.iterator.ForExceptJ(
                pulseIndex,
                (i, k) =>
                {
                    this.fields.H[i, pulseIndex.Lower - 1, k] += cartesianCoordinate;
                    this.fields.H[i, pulseIndex.JLength, k] -= cartesianCoordinate2;
                });
        }

        private void calculateDField(SimulationParameters param, IndexStore pulseIndex, int time)
        {
            iterator.For(param.Indices.ShiftLower(1),
                (i, j, k) =>
                {
                    CartesianCoordinate curlH = this.fields.H.Curl(i, j, k, -1);
                    this.fields.IntegralD[i, j, k] += curlH;

                    var pmlCoefs = this.pml.Electric(i, j, k);
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
            iterator.ForExceptK(
                pulseIndex,
                (i, j) =>
                {
                    var cartesianCoordinate = CartesianCoordinate.YOrth * 
                        (courantNumber * this.pulse.H[j]);
                    this.fields.D[i, j, pulseIndex.Lower] -= cartesianCoordinate;
                    this.fields.D[i, j, pulseIndex.KLength + 1] += cartesianCoordinate;
                });
        }

        private void addPulseToD2(IndexStore pulseIndex, double courantNumber)
        {
            var cart1 = CartesianCoordinate.ZOrth * 
                (courantNumber * this.pulse.H[pulseIndex.Lower - 1]);
            var cart2 = CartesianCoordinate.ZOrth * 
                (courantNumber * this.pulse.H[pulseIndex.JLength]);
            iterator.ForExceptJ(
                pulseIndex,
                (i, k) => {
                    this.fields.D[i, pulseIndex.Lower, k] += cart1;
                    this.fields.D[i, pulseIndex.JLength, k] -= cart2;
                });
        }

        private SimulationResult calculateExtinction(SpectrumUnit freq, SimulationParameters parameters)
        {
            double timeStep = parameters.CellSize * parameters.CourantNumber / Fundamentals.SpeedOfLight;

            var pulseFourier = this.pulse.FourierPulse.Select(x => x.Transform(freq, timeStep).Magnitude).ToArray();
            double extinction = iterator.Sum(parameters.Indices,
                (i, j, k) =>
                {
                    var medium = parameters.Medium[i, j, k];
                    if (!medium.IsBody)
                    {
                        return 0;
                    }
                    Complex eps = medium.Permittivity.GetPermittivity(freq);
                    
                    double pulseMultiplier = 1 / pulseFourier[j];
                    var complex = eps.Imaginary *
                                  pulseMultiplier * this.fields.FourierField[i, j, k].Transform(freq, timeStep).Norm;
                    return complex;
                });

            double area = this.calculateArea(parameters);

            var resu = new SimulationResult();

            double waveNumber = freq.ToType(SpectrumUnitType.WaveNumber);

            extinction = extinction * parameters.CellSize * waveNumber;
            double areamult = 1 / area;
            resu.EffectiveCrossSectionAbsorption = (extinction * areamult);

            resu.CrossSectionAbsorption = extinction * Math.Pow(parameters.CellSize, 2);

            return resu;
        }

        private double? crossSectionArea;
        private double calculateArea(SimulationParameters parameters)
        {
            if (crossSectionArea.HasValue)
            {
                return crossSectionArea.Value;
            }
            int area = 0;

            this.iterator.ForExceptJ(
                parameters.Indices,            
                (i, k) =>
                {
                    Interlocked.Add(
                        ref area,
                        parameters.Medium[i, parameters.Indices.GetCenter().JLength, k].IsBody ? 1 : 0);
                });

            this.crossSectionArea = area;
            return area;
        }


        #region Events

        public event EventHandler<TimeStepCalculatedEventArgs> TimeStepCalculated;

        protected virtual void OnTimeStepCalculated(TimeStepCalculatedEventArgs e)
        {
            EventHandler<TimeStepCalculatedEventArgs> handler = this.TimeStepCalculated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}