using Simulation.FDTD.Models;
using Simulation.Models;
using Simulation.Models.ConfigurationParameters;
using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

namespace Simulation.FDTD
{
    public class FDTDSimulation
    {
        private FDTDField fields;
        private FDTDPulse pulse;

        public SimulationResultDictionary Calculate(SimulationParameters parameters)
        {
            this.initParams(parameters);

            int numsteps = 50;
            for (int time = 0; time < numsteps; time++)
            {
                _calcFields(time, parameters);
            }

            return CalcExtinction(parameters);
        }

        private void initParams(SimulationParameters parameters)
        {
            this.fields = new FDTDField(parameters.Indices, parameters.Spectrum);
            this.pulse = new FDTDPulse(parameters.waveFunc, 10, parameters.Spectrum);
        }

        private void _calcFields(int time, SimulationParameters parameters)
        {
            var pmlLength = 7;

            // границі розсіяного поля.
            var pulseIndex = parameters.Indices.ShiftLower(pmlLength + 1).ShiftUpper(pmlLength + 1);

            this.calculateDField(parameters.Indices, pulseIndex, time);

            // Calculate the E from D field
            parameters.Indices.ShiftLower(1).ShiftUpper(1)
                      .For((i, j, k) => { fields.E[i, j, k] = parameters.Medium[i, j, k].Solve(fields.D[i, j, k]); });

            double timeStep = parameters.cellSize * Fundamentals.CourantConst / Fundamentals.LightVelocity;
            this.fields.DoFourierField(time * timeStep, parameters.Medium);
            this.pulse.DoFourierPulse(time * timeStep);

            this.calculateHField(parameters.Indices, pulseIndex);
        }

        private void calculateHField(IndexStore indices, IndexStore pulseIndex)
        {
            // Calculate the Hx field
            indices.ShiftUpper(1).For(
                (i, j, k) =>
                {
                    var rot_e = this.fields.E.Curl(i, j, k, +1);
                    this.fields.IH[i, j, k] = this.fields.IH[i, j, k] + rot_e;

                    var f3 = new CartesianCoordinate(
                        this.fields.Fj3[j] * this.fields.Fk3[k],
                        this.fields.Fi3[i] * this.fields.Fk3[k],
                        this.fields.Fi3[i] * this.fields.Fj3[j]);

                    var f2 = new CartesianCoordinate(
                        this.fields.Fj2[j] * this.fields.Fk2[k],
                        this.fields.Fi2[i] * this.fields.Fk2[k],
                        this.fields.Fi2[i] * this.fields.Fj2[j]);

                    var f1 = new CartesianCoordinate(
                        this.fields.Fi1[i],
                        this.fields.Fj1[j],
                        this.fields.Fk1[k]);

                    this.fields.H[i, j, k] = this.fields.H[i, j, k].ComponentProduct(f3)
                                             + Fundamentals.CourantConst *
                                             (rot_e + this.fields.IH[i, j, k].ComponentProduct(f1)).ComponentProduct(f2);
                });

            this.pulse.MagneticFieldStepCalc();

            // Incident Hx
            pulseIndex.ForExceptJ(
                (i, k) =>
                {
                    this.fields.H[i, pulseIndex.Lower - 1, k] += new CartesianCoordinate(Fundamentals.CourantConst * this.pulse.E[pulseIndex.Lower], 0, 0);
                    this.fields.H[i, pulseIndex.Lower, k] -= new CartesianCoordinate(Fundamentals.CourantConst * this.pulse.E[pulseIndex.Lower], 0, 0);
                });

            // Incident Hy
            pulseIndex.ForExceptI(
                (j, k) =>
                {
                    this.fields.H[pulseIndex.Lower - 1, j, k] -= new CartesianCoordinate(0, Fundamentals.CourantConst * this.pulse.E[j], 0);
                    this.fields.H[pulseIndex.Lower, j, k] -= new CartesianCoordinate(0, Fundamentals.CourantConst * this.pulse.E[j], 0);
                });
        }

        private void calculateDField(IndexStore indices, IndexStore pulseIndex, int time)
        {
            // Calculate the Dx field
            indices.ShiftLower(1).For(
                (i, j, k) =>
                {
                    var rot_h = this.fields.H.Curl(i, j, k, -1);
                    this.fields.ID[i, j, k] += rot_h;

                    var g3 = new CartesianCoordinate(
                        this.fields.Gj3[j] * this.fields.Gk3[k],
                        this.fields.Gi3[i] * this.fields.Gk3[k],
                        this.fields.Gi3[i] * this.fields.Gj3[j]);

                    var g2 = new CartesianCoordinate(
                        this.fields.Gj2[j] * this.fields.Gk2[k],
                        this.fields.Gi2[i] * this.fields.Gk2[k],
                        this.fields.Gi2[i] * this.fields.Gj2[j]);

                    var g1 = new CartesianCoordinate(
                        this.fields.Gi1[i],
                        this.fields.Gj1[j],
                        this.fields.Gk1[k]);

                    this.fields.D[i, j, k] = this.fields.D[i, j, k].ComponentProduct(g3) +
                                             Fundamentals.CourantConst * (rot_h + g1.ComponentProduct(this.fields.ID[i, j, k]))
                                                                             .ComponentProduct(g2);
                });

            this.pulse.ElectricFieldStepCalc(time);

            // Incident Dy
            pulseIndex.ForExceptK(
                (i, j) =>
                {
                    this.fields.D[i, j, pulseIndex.Lower] -= new CartesianCoordinate(0, Fundamentals.CourantConst * this.pulse.H[j], 0);
                    this.fields.D[i, j, pulseIndex.Lower + 1] -= new CartesianCoordinate(0, Fundamentals.CourantConst * this.pulse.H[j], 0);
                });

            // Incident Dz
            pulseIndex.ForExceptJ(
                (i, k) =>
                {
                    this.fields.D[i, pulseIndex.Lower, k] += new CartesianCoordinate(0, 0, Fundamentals.CourantConst * (this.pulse.H[pulseIndex.Lower - 1] - this.pulse.H[pulseIndex.JLength]));
                });
        }

        private SimulationResultDictionary CalcExtinction(SimulationParameters parameters)
        {
            return parameters.Spectrum.ToSimulationResult(x => this.calculateExtinction(x, parameters));
        }

        private SimulationResult calculateExtinction(SpectrumParameter freq, SimulationParameters parameters)
        {
            double extinction = 0.0;

            double K = freq.ToType(SpectrumParameterType.WaveNumber);

            double area = 0.0;

            parameters.Indices.For(
                (i, j, k) =>
                {
                    if (parameters.Medium[i, j, k].IsBody)
                    {
                        var eps = parameters.Medium[i, j, k].GetEpsilon(freq);

                        var pulseMultiplier = 1 / this.pulse.FourierPulse[j][freq].Magnitude;
                        double complexPart1 = -eps.Imaginary *
                                              pulseMultiplier * this.fields.FourierField[i, j, k][freq].Norm;

                        extinction += complexPart1;
                    }
                });
            parameters.Indices.ForExceptJ((i, k) => { area += parameters.Medium[i, parameters.Indices.JLength / 2, k].IsBody ? 1 : 0; });

            var resu = new SimulationResult();

            extinction *= K * parameters.cellSize * parameters.cellSize * parameters.cellSize;
            resu.CrossSectionAbsorption = extinction;
            extinction /= area * parameters.cellSize * parameters.cellSize;
            resu.EffectiveCrossSectionAbsorption = extinction;

            return resu;
        }
    }
}