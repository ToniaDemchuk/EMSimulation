using Simulation.Infrastructure.Iterators;
using Simulation.Models.Coordinates;
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
        /// The fourier series of the field.
        /// </value>
        public FourierSeriesCoordinate[,,] FourierE { get; set; }

        /// <summary>
        /// Gets or sets the fourier series of the field.
        /// </summary>
        /// <value>
        /// The fourier series of the field.
        /// </value>
        public FourierSeriesCoordinate[,,] FourierH{ get; set; }

        private readonly IndexStore indices;

        readonly IIterator iterator;

        readonly bool isSpectrumCalculated;

        /// <summary>
        /// Initializes a new instance of the <see cref="FDTDField" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="iterator">The iterator.</param>
        public FDTDField(SimulationParameters parameters, IIterator iterator)
        {
            this.isSpectrumCalculated = parameters.IsSpectrumCalculated;
            this.indices = parameters.Indices;
            this.iterator = iterator;
            if (this.isSpectrumCalculated) {
                this.FourierE = this.indices.CreateArray(
                    (i, j, k) => parameters.Medium[i, j, k].IsBody ? new FourierSeriesCoordinate() : null);
                this.FourierH = this.indices.CreateArray(
                    (i, j, k) => parameters.Medium[i, j, k].IsBody ? new FourierSeriesCoordinate() : null);
            }


            this.D = this.indices.CreateArray(() => CartesianCoordinate.Zero);
            this.E = this.indices.CreateArray(() => CartesianCoordinate.Zero);
            this.H = this.indices.CreateArray(() => CartesianCoordinate.Zero);
            this.IntegralD = this.indices.CreateArray(() => CartesianCoordinate.Zero);
            this.IntegralH = this.indices.CreateArray(() => CartesianCoordinate.Zero);
        }

        /// <summary>
        /// Calculates the fourier series of the field.
        /// </summary>
        public void DoFourierField()
        {
            if (!this.isSpectrumCalculated) {
                return;
            }
            this.iterator.For(this.indices,
                (i, j, k) =>
                {
                    var fourierSeries = this.FourierE[i, j, k];
                    if (fourierSeries != null)
                    {
                        fourierSeries.Add(this.E[i, j, k]);
                    }

                    var fourierSeriesH = this.FourierH[i, j, k];
                    if (fourierSeriesH != null)
                    {
                        fourierSeriesH.Add(this.H[i, j, k]);
                    }
                });
        }
    }
}