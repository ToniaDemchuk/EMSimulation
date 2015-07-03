using Simulation.Medium.Models;
using Simulation.Models.Coordinates;
using Simulation.Models.Enums;
using Simulation.Models.Extensions;
using Simulation.Models.Spectrum;

namespace Simulation.FDTD.Models
{
    /// <summary>
    /// The FDTDField class.
    /// </summary>
    public class FDTDField
    {
        /// <summary>
        /// Gets or sets the displacement field.
        /// </summary>
        /// <value>
        /// The displacement field.
        /// </value>
        public CartesianCoordinate[,,] D { get; set; }

        /// <summary>
        /// Gets or sets the electric field.
        /// </summary>
        /// <value>
        /// The electric field.
        /// </value>
        public CartesianCoordinate[,,] E { get; set; }

        /// <summary>
        /// Gets or sets the magnetizing field.
        /// </summary>
        /// <value>
        /// The magnetizing field.
        /// </value>
        public CartesianCoordinate[,,] H { get; set; }

        /// <summary>
        /// Gets or sets the integral displacement field.
        /// </summary>
        /// <value>
        /// The integral displacement field.
        /// </value>
        public CartesianCoordinate[,,] IntegralD { get; set; }

        /// <summary>
        /// Gets or sets the integral magnetizing field.
        /// </summary>
        /// <value>
        /// The integral magnetizing field.
        /// </value>
        public CartesianCoordinate[,,] IntegralH { get; set; }

        /// <summary>
        /// Gets or sets the fourier series of the field.
        /// </summary>
        /// <value>
        /// The fourier series of the field..
        /// </value>
        public FourierSeries<ComplexCoordinate>[,,] FourierField { get; set; }

        private readonly IndexStore indices;

        private readonly OpticalSpectrum spectrum;

        /// <summary>
        /// Initializes a new instance of the <see cref="FDTDField"/> class.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="spectrum">The spectrum.</param>
        public FDTDField(IndexStore indices, OpticalSpectrum spectrum)
        {
            this.indices = indices;
            this.spectrum = spectrum;
            this.FourierField = indices.CreateArray<FourierSeries<ComplexCoordinate>>();
            this.D = indices.CreateArray<CartesianCoordinate>();
            this.E = indices.CreateArray<CartesianCoordinate>();
            this.H = indices.CreateArray<CartesianCoordinate>();
            this.IntegralD = indices.CreateArray<CartesianCoordinate>();
            this.IntegralH = indices.CreateArray<CartesianCoordinate>();
        }

        /// <summary>
        /// Calculates the fourier series of the field.
        /// </summary>
        /// <param name="time">The time value.</param>
        /// <param name="medium">The medium matrix.</param>
        public void DoFourierField(double time, IMediumSolver[,,] medium)
        {
            foreach (var freq in this.spectrum)
            {
                var angle = freq.ToType(SpectrumUnitType.CycleFrequency) * time;
                this.indices.For(
                    (i, j, k) =>
                    {
                        if (medium[i, j, k].IsBody)
                        {
                            this.FourierField[i, j, k].Add(freq, new ComplexCoordinate(this.E[i, j, k], angle));
                        }
                    });
            }
        }
    }
}