using Simulation.Medium.Factors;
using Simulation.Medium.Medium;
using Simulation.Medium.MediumSolver;
using Simulation.Medium.Models;
using Simulation.Models.Common;

namespace Simulation.Medium.Factories
{
    /// <summary>
    /// The factory for medium solvers.
    /// </summary>
    public class MediumSolverFactory
    {
        private readonly FuncDictionary mediumSolvers;

        private readonly MemoDictionary<string, BaseMediumFactor> mediumFactors;

        /// <summary>
        /// Gets or sets the time step.
        /// </summary>
        /// <value>
        /// The time step.
        /// </value>
        private readonly double timeStep;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediumSolverFactory"/> class.
        /// </summary>
        /// <param name="timeStep">The time step.</param>
        public MediumSolverFactory(double timeStep)
        {
            this.timeStep = timeStep;
            this.mediumFactors = new MemoDictionary<string, BaseMediumFactor>();
            this.mediumSolvers = new FuncDictionary();

            this.initSolvers();

            this.initFactors();
        }

        private void initFactors()
        {
            this.mediumFactors.Add(
                "silver",
                () =>
                {
                    var silver = new DrudeLorentz();
                    return new DrudeLorentzFactor(silver, this.timeStep);
                });
        }

        private void initSolvers()
        {
            this.mediumSolvers.Add((DrudeLorentzFactor x) => new DrudeLorentzSolver(x));
            this.mediumSolvers.Add((DrudeFactor x) => new DrudeSolver(x));
            this.mediumSolvers.Add((LossyDielectricFactor x) => new LossyDielectricSolver(x));
            this.mediumSolvers.Add((DielectricFactor x) => new DielectricSolver(x));
            this.mediumSolvers.Add((object x) => VacuumSolver.Default);
        }

        public IMediumSolver GetMediumSolver(string type, bool isBody = false)
        {
            var materialType = (string.IsNullOrWhiteSpace(type) ? "silver" : type).ToLowerInvariant();

            BaseMediumFactor mediumFactor = this.mediumFactors.Get(materialType);
            
            var medium = this.mediumSolvers.GetResult<IMediumSolver>(mediumFactor);
            medium.IsBody = isBody;
            return medium;
        }
    }
}
