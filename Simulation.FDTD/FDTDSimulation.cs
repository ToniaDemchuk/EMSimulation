using Simulation.Models;
using Simulation.Models.ConfigurationParameters;
using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

namespace Simulation.FDTD
{
    public class FDTDSimulation
    {
        FDTDField fields;
        FDTDPulse pulse;

        IMedium[,,] medium;
        private int time;

        IndexStore indices = new IndexStore(50, 50, 50);

        private double cellSize;
        void _calcFields()
        {
            var pmlLength = 7;

            // границі розсіяного поля.
            var pulseIndex = indices.ShiftLower(pmlLength + 1).ShiftUpper(pmlLength + 1);
            
            this.calculateDField(pulseIndex);

            // Calculate the E from D field
            indices.ShiftLower(1).ShiftUpper(1)
                   .For((i, j, k) =>
                   {
                       fields.E[i, j, k] = medium[i, j, k].Solve(fields.D[i, j, k]);
                   });

            double timeStep = cellSize * Fundamentals.CourantConst / (Fundamentals.LightVelocity);
            fields.DoFourierField(time * timeStep, this.medium);
            pulse.DoFourierPulse(time * timeStep);
            
            this.calculateHField(pulseIndex);

            time++;
        }

        private void calculateHField(IndexStore pulseIndex)
        {
            // Calculate the Hx field
            this.indices.ShiftUpper(1).For((i, j, k) =>
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
            pulseIndex.ForExceptJ((i, k) =>
            {
                this.fields.H[i, pulseIndex.Lower - 1, k] += new CartesianCoordinate(Fundamentals.CourantConst * this.pulse.E[pulseIndex.Lower], 0, 0);
                this.fields.H[i, pulseIndex.Lower, k] -= new CartesianCoordinate(Fundamentals.CourantConst * this.pulse.E[pulseIndex.Lower], 0, 0);
            });

            // Incident Hy
            pulseIndex.ForExceptI((j, k) =>
            {
                this.fields.H[pulseIndex.Lower - 1, j, k] -= new CartesianCoordinate(0, Fundamentals.CourantConst * this.pulse.E[j], 0);
                this.fields.H[pulseIndex.Lower, j, k] -= new CartesianCoordinate(0, Fundamentals.CourantConst * this.pulse.E[j], 0);
            });
        }

        private void calculateDField(IndexStore pulseIndex)
        {
            // Calculate the Dx field
            this.indices.ShiftLower(1).For((i, j, k) =>
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

            this.pulse.ElectricFieldStepCalc(this.time);

            // Incident Dy
            pulseIndex.ForExceptK((i, j) =>
            {
                this.fields.D[i, j, pulseIndex.Lower] -= new CartesianCoordinate(0, Fundamentals.CourantConst * this.pulse.H[j], 0);
                this.fields.D[i, j, pulseIndex.Lower + 1] -= new CartesianCoordinate(0, Fundamentals.CourantConst * this.pulse.H[j], 0);
            });

            // Incident Dz
            pulseIndex.ForExceptJ((i, k) => { this.fields.D[i, pulseIndex.Lower, k] += new CartesianCoordinate(0, 0, Fundamentals.CourantConst * (this.pulse.H[pulseIndex.Lower - 1] - this.pulse.H[pulseIndex.JLength])); });
        }

        SimulationResultDictionary CalcExtinction(OpticalSpectrum frequency)
        {
            return frequency.ToSimulationResult(this.calculateExtinction);
        }

        private SimulationResult calculateExtinction(SpectrumParameter freq)
        {
            double extinction = 0.0;

            double K = freq.ToType(SpectrumParameterType.CycleFrequency);

            double area = 0.0;

            indices.For((i, j, k) =>
            {
                if (this.medium[i, j, k].IsBody)
                {
                    var eps = this.medium[i, j, k].GetEpsilon(freq);

                    var pulseMultiplier = 1 / this.pulse.FourierPulse[j][freq].Magnitude;
                    double complexPart1 = -eps.Imaginary *
                                          pulseMultiplier * this.fields.FourierField[i, j, k][freq].Norm;

                    extinction += complexPart1;
                }
            });
            indices.ForExceptJ((i, k) =>
            {
                area += this.medium[i, indices.JLength / 2, k].IsBody ? 1 : 0;
            });

            var resu = new SimulationResult();

            extinction *= (K * this.cellSize * this.cellSize * this.cellSize);
            resu.CrossSectionAbsorption = extinction;
            extinction /= (area * this.cellSize * this.cellSize);
            resu.EffectiveCrossSectionAbsorption = extinction;

            return resu;
        }
    }
}
