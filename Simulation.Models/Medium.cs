using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Simulation.Models.Enums;
using Simulation.Models.Extensions;

namespace Simulation.Models
{
    public class Valuum : IMedium
    {

        public CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            return displacementField;
        }
    }

    public class Dielectric : IMedium
    {
        public Dielectric(double epsilon)
        {
            Ga = 1 / epsilon;
        }
        public double Ga { get; set; }

        public CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            return Ga * displacementField;
        }
    }

    public class LossyDielectric : Dielectric, IMedium
    {
        public Complex Permitivity { get; set; }

        public CartesianCoordinate IntegralField { get; set; }
        public double Gb { get; set; }

        public new CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            var efield = (displacementField - IntegralField) * Ga;
            IntegralField = IntegralField + efield * Gb;
            return efield;
        }

        public LossyDielectric(double epsilon, double sigma, double timeStep)
            : base(epsilon)
        {
            Ga = 1.0 / (epsilon + (sigma * timeStep / Fundamentals.Eps0));

            Gb = sigma * timeStep / Fundamentals.Eps0;


            Permitivity = new Complex(epsilon, sigma / Fundamentals.Eps0);
        }
    }

    public class Drude : IMedium
    {
        public double OmegaP { get; set; }

        public double EpsInfinity { get; set; }

        public double Ga { get; set; }
        public double Gb { get; set; }
        public double Gc { get; set; }

        public CartesianCoordinate SampledTimeDomain { get; set; }
        public CartesianCoordinate SampledTimeDomain1 { get; set; }
        public CartesianCoordinate SampledTimeDomain2 { get; set; }

        public Drude(double timeStep)
        {

            EpsInfinity = 1;//3.9943;
            this.OmegaP = 1.369e+16;
            var DEps0 = 8.45e-1;
            var Gamma0 = 7.292e+13 / (2 * Math.PI);

            //        epsinf = 1;//3.9943;
            //        omegap = 1.803274e+011;
            //        deps0 = 1;
            //        gamma0 = 2.000000e+010 / (2*PI);

            double epsvc = -Gamma0 * timeStep;
            var exp = Math.Exp(epsvc);
            Ga = (1.0 + exp);
            Gb = exp;
            Gc = ((this.OmegaP * this.OmegaP * DEps0) * timeStep / Gamma0) * (1.0 - exp);
        }
        public CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            var efield = (displacementField - SampledTimeDomain) /
                                        EpsInfinity;

            SampledTimeDomain = Ga * SampledTimeDomain1 -
                                         Gb * SampledTimeDomain2 +
                                         Gc * efield;
            SampledTimeDomain2 = SampledTimeDomain1;
            SampledTimeDomain1 = SampledTimeDomain;

            return efield;

        }

    }

    public class DrudeLorentz : Drude, IMedium
    {
        public DrudeLorentz(double timeStep)
            : base(timeStep)
        {
            var lorentz_order = 5;

            double[] omegak = new double[lorentz_order];
            omegak[0] = 1.24e+15;
            omegak[1] = 6.808e+15;
            omegak[2] = 1.244e+16;
            omegak[3] = 1.38e+16;
            omegak[4] = 3.083e+16;

            double[] depsk = new double[lorentz_order];

            depsk[0] = 6.5e-2;
            depsk[1] = 1.24e-1;
            depsk[2] = 1.1e-2;
            depsk[3] = 8.4e-1;
            depsk[4] = 5.646;

            double[] gammak = new double[lorentz_order];

            gammak[0] = 5.904e+15 / (2 * Math.PI);
            gammak[1] = 6.867e+14 / (2 * Math.PI);
            gammak[2] = 9.875e+13 / (2 * Math.PI);
            gammak[3] = 1.392e+15 / (2 * Math.PI);
            gammak[4] = 3.675e+15 / (2 * Math.PI);

            Gal = new double[lorentz_order];
            Gbl = new double[lorentz_order];
            Gcl = new double[lorentz_order];

            for (int l = 0; l < lorentz_order; l++)
            {
                double alfadt = (gammak[l] / 2.0) * timeStep;
                double sqrtl =
                    Math.Sqrt(Math.Pow(omegak[l], 2.0) - (Math.Pow(gammak[l], 2.0) / 4.0));
                double betadt = sqrtl * timeStep;

                double gammadt = Math.Pow(this.OmegaP, 2.0) / sqrtl * depsk[l] * timeStep;
                double exp = Math.Exp(-alfadt);
                Gal[l] = 2.0 * exp * Math.Cos(betadt);
                Gbl[l] = Math.Exp(-2.0 * alfadt);
                Gcl[l] = exp * Math.Sin(betadt) * gammadt;

            }

        }
        public double[] Gal { get; set; }
        public double[] Gbl { get; set; }
        public double[] Gcl { get; set; }

        public CartesianCoordinate[] SampledLorentzDomain { get; set; }
        public CartesianCoordinate[] SampledLorentzDomain1 { get; set; }
        public CartesianCoordinate[] SampledLorentzDomain2 { get; set; }

        public new CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            var efield = base.Solve(displacementField);

            efield = this.SampledLorentzDomain.Aggregate(efield, (current, domain) => current - domain);


            for (int l = 0; l < SampledLorentzDomain1.Length; l++)
            {
                this.SampledLorentzDomain[l] = Gal[l] * this.SampledLorentzDomain1[l] -
                         Gbl[l] * this.SampledLorentzDomain2[l] +
                         Gcl[l] * efield;
                this.SampledLorentzDomain2[l] = this.SampledLorentzDomain1[l];
                this.SampledLorentzDomain1[l] = this.SampledLorentzDomain[l];
            }


            return efield;

        }

    }
}
