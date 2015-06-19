using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models
{
    public class FDTDPulse
    {
        public double[] E { get; set; }
        public double[] H { get; set; }

        private readonly int medLength; // length of 1D medium

        private readonly OpticalSpectrum spectrum;
        readonly Func<double, double> pulseFunc;

        double eMH1;
        double eMH2;
        double eML1;
        double eML2;

        double signalExistingCondition; // defines existing of pulse

        public FourierSeries<Complex>[] FourierPulse;

        public FDTDPulse(Func<double, double> waveFunc, int length, OpticalSpectrum spectrum)
        {
            medLength = 2 * length; //
            E = new double[medLength];// electric field
            H = new double[medLength];// magnetic field

            eMH2 = eMH1 = 0.0;
            eML2 = eML1 = 0.0;


            pulseFunc = waveFunc;
            this.spectrum = spectrum;

            signalExistingCondition = 1.0;

            InitFourier();
        }

        public void ElectricFieldStepCalc(int t)
        {
            // E-field calculation
            for (int i = 1; i < medLength; i++)
            {
                E[i] += Fundamentals.CourantConst * (H[i - 1] - H[i]);
            }


            double pulse = pulseFunc(t);

            E[2] = pulse;

            /*граничні умови*/
            E[medLength - 1] = eMH2;
            eMH2 = eMH1;
            eMH1 = E[medLength - 2];

            E[0] = eML2;
            eML2 = eML1;
            eML1 = E[1];

            // Подавлення випадкового шуму при обрахунку експоненти після проходження більшої
            // частини сигналу через досліджуванну область
            if (t >= 300)
            {

                eMH2 = 0.0;
                eMH1 = 0.0;
                eML2 = 0.0;
                eML1 = 0.0;

                for (int i = medLength / 2; i < medLength; i++)
                {
                    E[i] = 0.0;
                    H[i] = 0.0;
                }
            }
        }

        public void MagneticFieldStepCalc()
        {
            // H-field calculation
            for (int i = 0; i < medLength - 1; i++)
            {
                H[i] += Fundamentals.CourantConst * (E[i] - E[i + 1]);
            }

        }

        private void InitFourier()
        {
            FourierPulse = new FourierSeries<Complex>[medLength];
            FourierPulse.Initialize();
        }

        public void DoFourierPulse(double time)
        {
            // Fourier transform
            foreach (var cycleFreq in this.spectrum)
            {
                var angle = cycleFreq.ToType(SpectrumParameterType.CycleFrequency) * time;
                for (int m = 0; m < medLength; m++)
                {

                    FourierPulse[m].Add(cycleFreq, Complex.FromPolarCoordinates(E[m], angle));
                }
            }
        }


    }
}
