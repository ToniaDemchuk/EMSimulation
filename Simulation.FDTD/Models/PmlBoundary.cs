using System;

using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

using PmlCoef = System.Tuple<double, double, double>;
using PmlCoordCoef = System.Tuple<Simulation.Models.Coordinates.CartesianCoordinate,
    Simulation.Models.Coordinates.CartesianCoordinate,
    Simulation.Models.Coordinates.CartesianCoordinate>;

namespace Simulation.FDTD.Models
{
    /// <summary>
    ///     The PmlBoundary class.
    /// </summary>
    public class PmlBoundary
    {
        private PmlCoef[] fi;

        private PmlCoef[] fj;

        private PmlCoef[] fk;

        private PmlCoef[] gi;

        private PmlCoef[] gj;

        private PmlCoef[] gk;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PmlBoundary" /> class.
        /// </summary>
        /// <param name="pmlLength">Length of the PML.</param>
        /// <param name="store">The store.</param>
        public PmlBoundary(int pmlLength, IndexStore store)
        {
            this.Length = pmlLength;
            this.boundaryConditionsCalc(store);
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; set; }

        /// <summary>
        /// Get magnetics coefficients.
        /// </summary>
        /// <param name="i">The i index.</param>
        /// <param name="j">The j index.</param>
        /// <param name="k">The k index.</param>
        /// <returns>The pml coefficients for specific point.</returns>
        public PmlCoordCoef Magnetic(int i, int j, int k)
        {
            return getCoeficient(this.fi[i], this.fj[j], this.fk[k]);
        }

        /// <summary>
        /// Get electric coefficients.
        /// </summary>
        /// <param name="i">The i index.</param>
        /// <param name="j">The j index.</param>
        /// <param name="k">The k index.</param>
        /// <returns>The pml coefficients for specific point.</returns>
        public PmlCoordCoef Electric(int i, int j, int k)
        {
            return getCoeficient(this.gi[i], this.gj[j], this.gk[k]);
        }

        private static PmlCoordCoef getCoeficient(PmlCoef iCoef, PmlCoef jCoef, PmlCoef kCoef)
        {
            var item1 = new CartesianCoordinate(
                iCoef.Item1,
                jCoef.Item1,
                kCoef.Item1);
            var item2 = new CartesianCoordinate(
                jCoef.Item2 * kCoef.Item2,
                iCoef.Item2 * kCoef.Item2,
                iCoef.Item2 * jCoef.Item2);
            var item3 = new CartesianCoordinate(
                jCoef.Item3 * kCoef.Item3,
                iCoef.Item3 * kCoef.Item3,
                iCoef.Item3 * jCoef.Item3);
            return new PmlCoordCoef(
                item1,
                item2,
                item3);
        }

        private void boundaryConditionsCalc(IndexStore indices)
        {
            this.oneDirectionBoundary(indices.ILength, ref this.gi, ref this.fi);

            this.oneDirectionBoundary(indices.JLength, ref this.gj, ref this.fj);

            this.oneDirectionBoundary(indices.KLength, ref this.gk, ref this.fk);
        }

        private void oneDirectionBoundary(
            int fullLength,
            ref PmlCoef[] g,
            ref PmlCoef[] f)
        {
            const double DefaultItem1 = 0.0;
            const double DefaultItem2 = 1.0;
            const double DefaultItem3 = 1.0;

            g = new PmlCoef[fullLength];
            f = new PmlCoef[fullLength];
            for (int i = 0; i < fullLength; i++)
            {
                g[i] = new PmlCoef(DefaultItem1, DefaultItem2, DefaultItem3);
                f[i] = new PmlCoef(DefaultItem1, DefaultItem2, DefaultItem3);
            }

            for (int i = 0; i < this.Length; i++)
            {
                double xxn = (this.Length - i) / (double) this.Length;
                double xn = 0.333333 * Math.Pow(xxn, 3.0);
                double xxns = (this.Length - i - 0.5) / this.Length;
                double xns = 0.333333 * Math.Pow(xxns, 3.0);

                f[i] = new PmlCoef(xn, 1.0 / (1.0 + xns), (1.0 - xns) / (1.0 + xns));
                f[fullLength - i - 1] = new PmlCoef(xn, DefaultItem2, DefaultItem3);
                f[fullLength - i - 2] = new PmlCoef(DefaultItem1, 1.0 / (1.0 + xns), (1.0 - xns) / (1.0 + xns));

                g[i] = new PmlCoef(xns, 1.0 / (1.0 + xn), (1.0 - xn) / (1.0 + xn));
                g[fullLength - i - 1] = new PmlCoef(DefaultItem1, 1.0 / (1.0 + xn), (1.0 - xn) / (1.0 + xn));
                g[fullLength - i - 2] = new PmlCoef(xns, DefaultItem2, DefaultItem3);
            }
        }
    }
}