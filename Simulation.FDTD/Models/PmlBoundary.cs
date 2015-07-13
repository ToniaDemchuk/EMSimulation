using System;

using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

using PmlCoef = System.Tuple<double, double, double>;

namespace Simulation.FDTD.Models
{
    /// <summary>
    ///     The PmlBoundary class.
    /// </summary>
    public class PmlBoundary
    {
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
        /// Gets the electric.
        /// </summary>
        /// <value>
        /// The electric.
        /// </value>
        public PmlCoefficient[,,] Electric { get; private set; }

        /// <summary>
        /// Gets the magnetic.
        /// </summary>
        /// <value>
        /// The magnetic.
        /// </value>
        public PmlCoefficient[,,] Magnetic { get; private set; }

        /// <summary>
        ///     Gets or sets the length.
        /// </summary>
        /// <value>
        ///     The length.
        /// </value>
        public int Length { get; set; }

        private void boundaryConditionsCalc(IndexStore indices)
        {
            PmlCoef[] ei;
            PmlCoef[] mi;

            this.oneDirectionBoundary(indices.ILength, out ei, out mi);

            PmlCoef[] ej;
            PmlCoef[] mj;
            this.oneDirectionBoundary(indices.JLength, out ej, out mj);

            PmlCoef[] ek;
            PmlCoef[] mk;
            this.oneDirectionBoundary(indices.KLength, out ek, out mk);

            this.Electric = indices.CreateArray((i, j, k) => getCoeficient(ei[i], ej[j], ek[k]));
            this.Magnetic = indices.CreateArray((i, j, k) => getCoeficient(mi[i], mj[j], mk[k]));
        }

        private static PmlCoefficient getCoeficient(PmlCoef iCoef, PmlCoef jCoef, PmlCoef kCoef)
        {
            var integralFactor = new CartesianCoordinate(
                iCoef.Item1,
                jCoef.Item1,
                kCoef.Item1);
            var curlFactor = new CartesianCoordinate(
                jCoef.Item2 * kCoef.Item2,
                iCoef.Item2 * kCoef.Item2,
                iCoef.Item2 * jCoef.Item2);
            var fieldFactor = new CartesianCoordinate(
                jCoef.Item3 * kCoef.Item3,
                iCoef.Item3 * kCoef.Item3,
                iCoef.Item3 * jCoef.Item3);
            return new PmlCoefficient(
                fieldFactor,
                curlFactor,
                integralFactor);
        }

        private void oneDirectionBoundary(
            int fullLength,
            out PmlCoef[] g,
            out PmlCoef[] f)
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

            this.setBoundary(fullLength, ref g, ref f);
        }

        private void setBoundary(int fullLength, ref PmlCoef[] g, ref PmlCoef[] f)
        {
            for (int i = 0; i < this.Length; i++)
            {
                double xxn = (this.Length - i) / (double) this.Length;
                double xn = 0.333333 * Math.Pow(xxn, 3.0);
                double xxns = (this.Length - i - 0.5) / this.Length;
                double xns = 0.333333 * Math.Pow(xxns, 3.0);

                f[i] = new PmlCoef(xn, 1.0 / (1.0 + xns), (1.0 - xns) / (1.0 + xns));
                f[fullLength - i - 1] = f[i];
                //f[fullLength - i - 1] = new PmlCoef(xn, f[fullLength - i - 1].Item2, f[fullLength - i - 1].Item3);
                //f[fullLength - i - 2] = new PmlCoef(f[fullLength - i - 2].Item1, 1.0 / (1.0 + xns), (1.0 - xns) / (1.0 + xns));

                g[i] = new PmlCoef(xns, 1.0 / (1.0 + xn), (1.0 - xn) / (1.0 + xn));
                g[fullLength - i - 1] = g[i];

                //g[fullLength - i - 1] = new PmlCoef(g[fullLength - i - 1].Item1, 1.0 / (1.0 + xn), (1.0 - xn) / (1.0 + xn));
                //g[fullLength - i - 2] = new PmlCoef(xns, g[fullLength - i - 2].Item2, g[fullLength - i - 2].Item3);
            }
        }
    }
}