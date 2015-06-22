using System;
using System.Collections.Generic;

namespace Simulation.Models
{
    /// <summary>
    /// The SystemConfig class.
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get; protected set; }

        /// <summary>
        /// Gets or sets the effective radius.
        /// </summary>
        /// <value>
        /// The effective radius.
        /// </value>
        public IList<double> Radius { get; protected set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public IList<CartesianCoordinate> Points { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfig"/> class.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="points">The points.</param>
        public SystemConfig(List<double> radius, List<CartesianCoordinate> points)
        {
            if (radius.Count != points.Count)
            {
                throw new ArgumentException("Lists have different count.");
            }

            this.Size = radius.Count;
            this.Radius = radius.AsReadOnly();
            this.Points = points.AsReadOnly();
        }
    }
}
