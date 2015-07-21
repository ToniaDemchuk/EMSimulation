using System;
using System.Linq;

using Simulation.Medium.Factors;
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
        /// Initializes a new instance of the <see cref="DrudeLorentzSolver" /> class.
        /// </summary>
        /// <param name="medium">The medium.</param>
        /// <param name="param">The parameter.</param>
        public DrudeLorentzSolver(DrudeLorentz medium, DrudeLorentzFactor param)
            : base(medium, param)
        {
            this.param = param;

            this.SampledLorentzDomain = new CartesianCoordinate[medium.OscillatorTerms.Count];
            this.SampledLorentzDomain1 = new CartesianCoordinate[medium.OscillatorTerms.Count];
            this.SampledLorentzDomain2 = new CartesianCoordinate[medium.OscillatorTerms.Count];

        }

        private readonly DrudeLorentzFactor param;

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
                this.SampledLorentzDomain[l] = this.param.SampledLorentzShift1[l] * this.SampledLorentzDomain1[l] - 
                    this.param.SampledLorentzShift2[l] * this.SampledLorentzDomain2[l] + 
                    this.param.ElectricLorentz[l] * efield;

                this.SampledLorentzDomain2[l] = this.SampledLorentzDomain1[l];
                this.SampledLorentzDomain1[l] = this.SampledLorentzDomain[l];
            }

            return efield;
        }
    }
}