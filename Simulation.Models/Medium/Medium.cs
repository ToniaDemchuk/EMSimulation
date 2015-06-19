using System;
using System.Linq;
using System.Numerics;
using Simulation.Models.Extensions;

namespace Simulation.Models
{
    public class Vacuum : IMedium
    {
        public bool IsBody { get; set; }

        public CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            return displacementField;
        }

        public Complex GetEpsilon(SpectrumParameter frequency)
        {
            return Complex.One; //todo: check
        }
    }

    public class Dielectric : IMedium
    {
        protected double _epsilon;

        public Dielectric(double epsilon)
        {
            this._epsilon = epsilon;
            Ga = 1 / epsilon;
        }

        public double Ga { get; set; }

        public bool IsBody { get; set; }

        public CartesianCoordinate Solve(
            CartesianCoordinate displacementField)
        {
            return Ga * displacementField;
        }

        public Complex GetEpsilon(SpectrumParameter frequency)
        {
            return _epsilon;
        }
    }

    public class LossyDielectric : Dielectric, IMedium
    {
        private double sigma;
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

            this.sigma = sigma;
        }

        public new Complex GetEpsilon(SpectrumParameter frequency)
        {
            double W = frequency.ToType(SpectrumParameterType.CycleFrequency);
            return _epsilon - Complex.ImaginaryOne * sigma / Fundamentals.Eps0 / W;
        }
    }

    public class Drude : IMedium
    {
        private double Gamma0;
        private double DEps0;
        protected double OmegaP { get; set; }

        protected double EpsInfinity { get; set; }

        public double Ga { get; set; }
        public double Gb { get; set; }
        public double Gc { get; set; }

        public CartesianCoordinate SampledTimeDomain { get; set; }
        public CartesianCoordinate SampledTimeDomain1 { get; set; }
        public CartesianCoordinate SampledTimeDomain2 { get; set; }

        public Drude(double timeStep)
        {
            this.EpsInfinity = 1;//3.9943;
            this.OmegaP = 1.369e+16;
            this.DEps0 = 8.45e-1;
            this.Gamma0 = 7.292e+13;

            //        epsinf = 1;//3.9943;
            //        omegap = 1.803274e+011;
            //        deps0 = 1;
            //        gamma0 = 2.000000e+010 / (2*PI);

            double epsvc = -(this.Gamma0 / (2 * Math.PI)) * timeStep;
            var exp = Math.Exp(epsvc);
            Ga = (1.0 + exp);
            Gb = exp;
            Gc = ((this.OmegaP * this.OmegaP * this.DEps0) * timeStep / (this.Gamma0 / (2 * Math.PI))) * (1.0 - exp);
        }

        public bool IsBody { get; set; }

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

        public Complex GetEpsilon(SpectrumParameter frequency)
        {

            double W = frequency.ToType(SpectrumParameterType.CycleFrequency);
            Complex compl = EpsInfinity -
                        OmegaP * OmegaP /
                        (W * W -
                         Complex.ImaginaryOne * Gamma0 * W);
            return compl;
        }
    }

    public class DrudeLorentz : Drude, IMedium
    {
        public int LorentzOrder;
        public double[] Omegak;
        public double[] Depsk;
        public double[] Gammak;

        public DrudeLorentz(double timeStep)
            : base(timeStep)
        {
            this.LorentzOrder = 5;

            this.Omegak = new double[this.LorentzOrder];
            this.Omegak[0] = 1.24e+15;
            this.Omegak[1] = 6.808e+15;
            this.Omegak[2] = 1.244e+16;
            this.Omegak[3] = 1.38e+16;
            this.Omegak[4] = 3.083e+16;

            this.Depsk = new double[this.LorentzOrder];

            this.Depsk[0] = 6.5e-2;
            this.Depsk[1] = 1.24e-1;
            this.Depsk[2] = 1.1e-2;
            this.Depsk[3] = 8.4e-1;
            this.Depsk[4] = 5.646;

            this.Gammak = new double[this.LorentzOrder];

            this.Gammak[0] = 5.904e+15 / (2 * Math.PI);
            this.Gammak[1] = 6.867e+14 / (2 * Math.PI);
            this.Gammak[2] = 9.875e+13 / (2 * Math.PI);
            this.Gammak[3] = 1.392e+15 / (2 * Math.PI);
            this.Gammak[4] = 3.675e+15 / (2 * Math.PI);

            Gal = new double[this.LorentzOrder];
            Gbl = new double[this.LorentzOrder];
            Gcl = new double[this.LorentzOrder];

            for (int l = 0; l < this.LorentzOrder; l++)
            {
                double alfadt = (this.Gammak[l] / 2.0) * timeStep;
                double sqrtl =
                    Math.Sqrt(Math.Pow(this.Omegak[l], 2.0) - (Math.Pow(this.Gammak[l], 2.0) / 4.0));
                double betadt = sqrtl * timeStep;

                double gammadt = Math.Pow(this.OmegaP, 2.0) / sqrtl * this.Depsk[l] * timeStep;
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

        public new Complex GetEpsilon(SpectrumParameter frequency)
        {
            double W = frequency.ToType(SpectrumParameterType.CycleFrequency);


            var compl = base.GetEpsilon(frequency);

            for (int l = 0; l < LorentzOrder; l++)
            {
                var complorentz =
                    Depsk[l] * Omegak[l] * Omegak[l] /
                    (Omegak[l] * Omegak[l] + W * W - Complex.ImaginaryOne * Gammak[l] * W);

                compl += complorentz;

            }
            return compl;
        }

    }
}
