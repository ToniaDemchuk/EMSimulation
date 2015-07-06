using System;
using System.Linq;

using Simulation.Medium.Medium;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;

namespace Simulation.Medium.MediumSolver
{
    /// <summary>
    ///     The DrudeLorentzSolver class.
    /// </summary>
    public class DrudeLorentzSolver : DrudeSolver
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DrudeLorentzSolver" /> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="timeStep">The time step.</param>
        public DrudeLorentzSolver(DrudeLorentz medium, double timeStep)
            : base(medium, timeStep)
        {
            this.SampledLorentzFactor1 = new double[medium.OscillatorTerms.Count];
            this.SampledLorentzFactor2 = new double[medium.OscillatorTerms.Count];
            this.ElectricLorentzFactor = new double[medium.OscillatorTerms.Count];
            this.SampledLorentzDomain = new CartesianCoordinate[medium.OscillatorTerms.Count];
            this.SampledLorentzDomain1 = new CartesianCoordinate[medium.OscillatorTerms.Count];
            this.SampledLorentzDomain2 = new CartesianCoordinate[medium.OscillatorTerms.Count];

            double plasmaFreq = medium.PlasmaTerm.ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);

            for (int l = 0; l < medium.OscillatorTerms.Count; l++)
            {
                double collisionFreq =
                    medium.OscillatorTerms[l].CollisionFrequency.ToType(SpectrumUnitType.Frequency);
                double resonatorFreq =
                    medium.OscillatorTerms[l].ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);
                double alfadt = (collisionFreq / 2.0) * timeStep;
                double sqrtl =
                    Math.Sqrt(Math.Pow(resonatorFreq, 2.0) - (Math.Pow(collisionFreq, 2.0) / 4.0));
                double betadt = sqrtl * timeStep;

                double gammadt = Math.Pow(plasmaFreq, 2.0) / sqrtl * medium.OscillatorTerms[l].StrengthFactor * timeStep;
                double exp = Math.Exp(alfadt);
                this.SampledLorentzFactor1[l] = 2.0 * exp * Math.Cos(betadt);
                this.SampledLorentzFactor2[l] = Math.Exp(2.0 * alfadt);
                this.ElectricLorentzFactor[l] = exp * Math.Sin(betadt) * gammadt;

                this.SampledLorentzDomain[l] = CartesianCoordinate.Zero;
                this.SampledLorentzDomain1[l] = CartesianCoordinate.Zero;
                this.SampledLorentzDomain2[l] = CartesianCoordinate.Zero;
            }
        }

        /// <summary>
        ///     Gets or sets the sampled lorentz factor1.
        /// </summary>
        /// <value>
        ///     The sampled lorentz factor1.
        /// </value>
        public double[] SampledLorentzFactor1 { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled lorentz factor2.
        /// </summary>
        /// <value>
        ///     The sampled lorentz factor2.
        /// </value>
        public double[] SampledLorentzFactor2 { get; protected set; }

        /// <summary>
        ///     Gets or sets the electric lorentz factor.
        /// </summary>
        /// <value>
        ///     The electric lorentz factor.
        /// </value>
        public double[] ElectricLorentzFactor { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled lorentz domain.
        /// </summary>
        /// <value>
        ///     The sampled lorentz domain.
        /// </value>
        public CartesianCoordinate[] SampledLorentzDomain { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled lorentz domain1.
        /// </summary>
        /// <value>
        ///     The sampled lorentz domain1.
        /// </value>
        public CartesianCoordinate[] SampledLorentzDomain1 { get; protected set; }

        /// <summary>
        ///     Gets or sets the sampled lorentz domain2.
        /// </summary>
        /// <value>
        ///     The sampled lorentz domain2.
        /// </value>
        public CartesianCoordinate[] SampledLorentzDomain2 { get; protected set; }

        /// <summary>
        ///     Solves the electric field using specified displacement field.
        /// </summary>
        /// <param name="displacementField">The displacement field.</param>
        /// <returns>
        ///     The electric field.
        /// </returns>
        public override CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            CartesianCoordinate efield = base.Solve(displacementField);

            efield = this.SampledLorentzDomain.Aggregate(efield, (current, domain) => current - domain);

            for (int l = 0; l < this.SampledLorentzDomain1.Length; l++)
            {
                this.SampledLorentzDomain[l] = this.SampledLorentzFactor1[l] * this.SampledLorentzDomain1[l] -
                                               this.SampledLorentzFactor2[l] * this.SampledLorentzDomain2[l] +
                                               this.ElectricLorentzFactor[l] * efield;

                this.SampledLorentzDomain2[l] = this.SampledLorentzDomain1[l];
                this.SampledLorentzDomain1[l] = this.SampledLorentzDomain[l];
            }

            return efield;
        }
    }
}