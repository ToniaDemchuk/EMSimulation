using Simulation.FDTD.Models;
using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

namespace Simulation.Models.ConfigurationParameters
{
    public class FDTDField
    {
        public CartesianCoordinate[,,] D { get; set; }

        public CartesianCoordinate[,,] E { get; set; }

        public CartesianCoordinate[,,] H { get; set; }

        public CartesianCoordinate[,,] ID { get; set; }

        public CartesianCoordinate[,,] IH { get; set; }

        public FourierSeries<ComplexCoordinate>[,,] FourierField { get; set; }

        private IndexStore indices;

        private OpticalSpectrum spectrum;

        public double[] Gi1 { get; set; }

        public double[] Gi2 { get; set; }

        public double[] Gi3 { get; set; }

        public double[] Gj1 { get; set; }

        public double[] Gj2 { get; set; }

        public double[] Gj3 { get; set; }

        public double[] Gk1 { get; set; }

        public double[] Gk2 { get; set; }

        public double[] Gk3 { get; set; }

        public double[] Fi1 { get; set; }

        public double[] Fi2 { get; set; }

        public double[] Fi3 { get; set; }

        public double[] Fj1 { get; set; }

        public double[] Fj2 { get; set; }

        public double[] Fj3 { get; set; }

        public double[] Fk1 { get; set; }

        public double[] Fk2 { get; set; }

        public double[] Fk3 { get; set; }

        public FDTDField(IndexStore indices, OpticalSpectrum spectrum)
        {
            this.indices = indices;
            this.spectrum = spectrum;
            this.FourierField = indices.CreateArray<FourierSeries<ComplexCoordinate>>();
            this.D = indices.CreateArray<CartesianCoordinate>();
            this.E = indices.CreateArray<CartesianCoordinate>();
            this.H = indices.CreateArray<CartesianCoordinate>();
            this.ID = indices.CreateArray<CartesianCoordinate>();
            this.IH = indices.CreateArray<CartesianCoordinate>();
        }

        // _boundaryConditionsCalc();

        public void DoFourierField(double time, IMediumSolver[,,] medium)
        {
            // Fourier transform of EZ

            foreach (var freq in this.spectrum)
            {
                var angle = freq.ToType(SpectrumParameterType.CycleFrequency) * time;
                indices.For(
                    (i, j, k) =>
                    {
                        if (medium[i, j, k].IsBody)
                        {
                            FourierField[i, j, k].Add(freq, new ComplexCoordinate(E[i, j, k], angle));
                        }
                    });
            }
        }
    }
}