using System;
using System.Numerics;

namespace Simulation.Models
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    /// <typeparam name="T">The type of underlying value.</typeparam>
    public abstract class BaseCoordinate<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate" /> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        protected BaseCoordinate(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        /// <value>
        /// The x coordinate.
        /// </value>
        public T X { get; protected set; }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value>
        public T Y { get; protected set; }

        /// <summary>
        /// Gets the z coordinate.
        /// </summary>
        /// <value>
        /// The z coordinate.
        /// </value>
        public T Z { get; protected set; }

        protected Lazy<double> LazyNorm;

        /// <summary>
        /// Gets the norm of the coordinate.
        /// </summary>
        /// <value>
        /// The norm of the coordinate.
        /// </value>
        public double Norm
        {
            get { return this.LazyNorm.Value; }
        }
    }
}
