namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The ICoordinate interface.
    /// </summary>
    /// <typeparam name="T">The type of underlying value.</typeparam>
    public interface ICoordinate<out T> where T : struct
    {
        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        /// <value>
        /// The x coordinate.
        /// </value>
        T X { get; }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value>
        T Y { get; }

        /// <summary>
        /// Gets the z coordinate.
        /// </summary>
        /// <value>
        /// The z coordinate.
        /// </value>
        T Z { get; }

        /// <summary>
        /// Gets the norm of the coordinate.
        /// </summary>
        /// <value>
        /// The norm of the coordinate.
        /// </value>
        double Norm { get; }
    }
}