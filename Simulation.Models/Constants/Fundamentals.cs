using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models
{
    public class Fundamentals
    {
        public const double LightVelocity = 2.99792458e8;	//light velocity

        public const double Mu0 = 4.0 * Math.PI * 1.0e-7;

        public const double Eps0 = 1.0 / (LightVelocity * LightVelocity * Mu0);

        public readonly double Eta0 = Math.Sqrt(Mu0 / Eps0);

        public const double PlanckConst = 6.626e-34;

        public const double ReducedPlanckConst = PlanckConst / (2.0 * Math.PI);

        public const double QElektron = 1.602e-19;

        public const double CourantConst = 0.5; // = 1.0/sqrt(MU_0*EPS_0) * dt/dx
        //(defines stability of simulations)
    }
}
