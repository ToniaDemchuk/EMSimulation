using System;
using System.Collections.Generic;
using System.Linq;
using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

namespace Simulation.DDA.Models
{
    /// <summary>
    /// The SystemConfig class.
    /// </summary>
    public class SystemConfig
    {
        private const double NanoMeters = 1e-9; //position is set in relative units, need to convert in nm.

        /// <summary>
        /// Gets or sets the size of system.
        /// </summary>
        /// <value>
        /// The size of system.
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
        /// Gets the cross section area.
        /// </summary>
        /// <value>
        /// The cross section area.
        /// </value>
        public double CrossSectionArea {
            get
            {
                if (!this.crossSectionArea.HasValue)
                {
                    this.crossSectionArea = this.Radius.Distinct().Sum(radius => Math.PI * radius * radius); // todo: not valid calculation
                }
                return this.crossSectionArea.Value;
            }
        }

        private double? crossSectionArea;

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        protected IList<CartesianCoordinate> Points { get; set; }

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
            
            this.Radius = radius.Select(rad => rad * NanoMeters).ToList().AsReadOnly();
            this.Points = points.AsReadOnly();
        }

        public CartesianCoordinate GetPoint(int index)
        {
            return this.Points[index] * NanoMeters;
        }

		public CartesianCoordinate GetImagePoint(int index)
		{
			return this.Points[index].ComponentProduct(new CartesianCoordinate(1, 1, -1)) * NanoMeters;
		}

		public CartesianCoordinate GetDistanceUniform(int i, int j)
        {
            return this.Points[i] - this.Points[j];
        }

        public CartesianCoordinate GetDistance(int i, int j)
        {
            return (this.Points[i] - this.Points[j]) * NanoMeters;
        }
	}
}