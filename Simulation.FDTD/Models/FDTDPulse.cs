using System;

using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;
using Simulation.Models.Coordinates;
using System.Numerics;

namespace Simulation.FDTD.Models
{
    /// <summary>
    /// The FDTDPulse class.
    /// </summary>
    public class FDTDPulse
    {
        private readonly double courantNumber;

        private readonly int medLength; // length of 1D medium

        private readonly Func<int, CartesianCoordinate> pulseFunc;

        private CartesianCoordinate eMh1;

        private CartesianCoordinate eMh2;

        private CartesianCoordinate eMl1;

        private CartesianCoordinate eMl2;

        readonly bool isSpectrumCalculated;
        /// <summary>
        /// Initializes a new instance of the <see cref="FDTDPulse" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public FDTDPulse(SimulationParameters parameters)
        {
            this.isSpectrumCalculated = parameters.IsSpectrumCalculated;
            this.courantNumber = parameters.CourantNumber;
            this.medLength = 2 * parameters.Indices.JLength;
            this.E = new CartesianCoordinate[this.medLength]; // electric field
            this.H = new CartesianCoordinate[this.medLength]; // magnetic field

            this.eMh2 = this.eMh1 = CartesianCoordinate.Zero;
            this.eMl2 = this.eMl1 = CartesianCoordinate.Zero;

            this.pulseFunc = parameters.WaveFunc;

            if (this.isSpectrumCalculated)
            {
                this.FourierE = new FourierSeriesCoordinate[this.medLength];
                this.FourierE.For(i => new FourierSeriesCoordinate());
                this.FourierH = new FourierSeriesCoordinate[this.medLength];
                this.FourierH.For(i => new FourierSeriesCoordinate());
            }

        }

        /// <summary>
        /// Gets or sets the electric component of pulse.
        /// </summary>
        /// <value>
        /// The electric component of pulse.
        /// </value>
        public CartesianCoordinate[] E { get; set; }

        /// <summary>
        /// Gets or sets the magnetized component of pulse.
        /// </summary>
        /// <value>
        /// The magnetized component of pulse.
        /// </value>
        public CartesianCoordinate[] H { get; set; }

        /// <summary>
        /// Gets or sets the fourier series of pulse.
        /// </summary>
        /// <value>
        /// The fourier series of pulse.
        /// </value>
        public FourierSeriesCoordinate[] FourierE { get; set; }

        /// <summary>
        /// Gets or sets the fourier series of pulse.
        /// </summary>
        /// <value>
        /// The fourier series of pulse.
        /// </value>
        public FourierSeriesCoordinate[] FourierH { get; set; }

        /// <summary>
        /// Calculate the electric field step.
        /// </summary>
        /// <param name="time">The time value.</param>
        public void ElectricFieldStepCalc(int time)
        {
            // E-field calculation
            for (int i = 1; i < this.medLength; i++)
            {
                this.E[i] += CartesianCoordinate.ZOrth * this.courantNumber * (this.H[i - 1] - this.H[i]).X;
                this.E[i] += CartesianCoordinate.XOrth * this.courantNumber * (this.H[i] - this.H[i-1]).Z;
            }

            CartesianCoordinate pulse = this.pulseFunc(time);

            this.E[3] = pulse;

            /*граничні умови*/
            this.E[this.medLength - 1] = this.eMh2;
            this.eMh2 = this.eMh1;
            this.eMh1 = this.E[this.medLength - 2];

            this.E[0] = this.eMl2;
            this.eMl2 = this.eMl1;
            this.eMl1 = this.E[1];

            //// Подавлення випадкового шуму при обрахунку експоненти після проходження більшої
            //// частини сигналу через досліджуванну область
            //if (time >= 300)
            //{
            //    this.eMh2 = 0.0;
            //    this.eMh1 = 0.0;
            //    this.eMl2 = 0.0;
            //    this.eMl1 = 0.0;

            //    for (int i = this.medLength / 2; i < this.medLength; i++)
            //    {
            //        this.E[i] = 0.0;
            //        this.H[i] = 0.0;
            //    }
            //}

            DoFourierPulseE();
        }

        /// <summary>
        /// Magnetics the field step calculate.
        /// </summary>
        public void MagneticFieldStepCalc()
        {
            // H-field calculation
            for (int i = 0; i < this.medLength - 1; i++)
            {
                this.H[i] += CartesianCoordinate.XOrth * this.courantNumber * (this.E[i] - this.E[i + 1]).Z;
                this.H[i] += CartesianCoordinate.ZOrth * this.courantNumber * (this.E[i + 1] - this.E[i]).X;
            }

            DoFourierPulseH();
        }

        /// <summary>
        /// Does the fourier pulse.
        /// </summary>
        public void DoFourierPulseE()
        {
            if (!this.isSpectrumCalculated) {
                return;
            }
            for (int m = 0; m < this.medLength; m++)
            {
                this.FourierE[m].Add(this.E[m]);
            }
        }

        /// <summary>
        /// Does the fourier pulse.
        /// </summary>
        public void DoFourierPulseH()
        {
            if (!this.isSpectrumCalculated)
            {
                return;
            }
            for (int m = 0; m < this.medLength; m++)
            {
                this.FourierH[m].Add(this.H[m]);
            }
        }
    }
}