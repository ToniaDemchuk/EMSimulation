using System;

namespace Simulation.Models.Constants
{
    /// <summary>
    /// The Fundamental constants.
    /// </summary>
    public class Fundamentals
    {
        /// <summary>
        /// The speed of light in free space.
        /// </summary>
        public const double SpeedOfLight = 2.99792458e8;

        /// <summary>
        /// The vacuum permeability.
        /// </summary>
        public const double MagneticConst = 4.0 * Math.PI * 1.0e-7;

        /// <summary>
        /// The vacuum permittivity.
        /// </summary>
        public const double ElectricConst = 1.0 / (SpeedOfLight * SpeedOfLight * MagneticConst);

        /// <summary>
        /// The planck constant
        /// </summary>
        public const double PlanckConst = 6.626e-34;

        /// <summary>
        /// The reduced planck constant
        /// </summary>
        public const double ReducedPlanckConst = PlanckConst / (2.0 * Math.PI);

        /// <summary>
        /// The elementary electron charge.
        /// </summary>
        public const double ElementaryCharge = 1.602e-19;

        /// <summary>
        /// The the vacuum impedance.
        /// </summary>
        public readonly double VacuumImpedance = Math.Sqrt(MagneticConst / ElectricConst);
    }
}