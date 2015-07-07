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
        /// Initializes a new instance of the <see cref="FDTDField" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public FDTDField(SimulationParameters parameters)
        {
            this.indices = parameters.Indices;
            this.spectrum = parameters.Spectrum;

            this.FourierField = indices.CreateArray(
                (i, j, k) => parameters.Medium[i, j, k].IsBody ? new FourierSeries<ComplexCoordinate>() : null);

            this.D = indices.CreateArray(() => CartesianCoordinate.Zero);
            this.E = indices.CreateArray(() => CartesianCoordinate.Zero);
            this.H = indices.CreateArray(() => CartesianCoordinate.Zero);
            this.IntegralD = indices.CreateArray(() => CartesianCoordinate.Zero);
            this.IntegralH = indices.CreateArray(() => CartesianCoordinate.Zero);
        }

        /// <summary>
        /// Calculates the fourier series of the field.
        /// </summary>
        /// <param name="time">The time value.</param>
        public void DoFourierField(double time)
        {
            this.indices.For(
                (i, j, k) =>
                {
                    var fourierSeries = this.FourierField[i, j, k];
                    if (fourierSeries != null)
                    {
                        var electricField = this.E[i, j, k];

                        fourierSeries.Aggregate(
                            this.spectrum,
                            freq =>
                            ComplexCoordinate.FromPolarCoordinates(
                                electricField,
                                freq.ToType(SpectrumUnitType.CycleFrequency) * time));
                    }
                });
        }
    }
}