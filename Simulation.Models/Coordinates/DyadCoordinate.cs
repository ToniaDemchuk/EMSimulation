using System.Numerics;

namespace Simulation.Models.Coordinates
{
    /// <summary>
    /// The CartesianCoordinate class.
    /// </summary>
    /// <typeparam name="T">The type of underlying value.</typeparam>
    public class DyadCoordinate<T> where T : struct
    {
        public T xx { get; private set; }
        public T xy { get; private set; }
        public T xz { get; private set; }

        public T yx { get; private set; }
        public T yy { get; private set; }
        public T yz { get; private set; }

        public T zx { get; private set; }
        public T zy { get; private set; }
        public T zz { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianCoordinate" /> class.
        /// </summary>
        /// <param name="xx">The x-x value.</param>
        /// <param name="xy">The x-y value.</param>
        /// <param name="xz">The x-z value.</param>
        /// <param name="yx">The y-x value.</param>
        /// <param name="yy">The y-y value.</param>
        /// <param name="yz">The y-z value.</param>
        /// <param name="zx">The z-x value.</param>
        /// <param name="zy">The z-y value.</param>
        /// <param name="zz">The z-z value.</param>
        public DyadCoordinate(T xx, T xy, T xz, T yx, T yy, T yz, T zx, T zy, T zz)
        {
            this.xx = xx; this.yx = yx; this.zx = zx;
            this.xy = xy; this.yy = yy; this.zy = zy;
            this.xz = xz; this.yz = yz; this.zz = zz;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DyadCoordinate{T}"/> class.
        /// </summary>
        /// <param name="initValue">The value on the main diagonal of identity matrix.</param>
        public DyadCoordinate(T initValue)
        {
            xx = initValue;     yx = default(T);    zx = default(T);
            xy = default(T);    yy = initValue;     zy = default(T);
            xz = default(T);    yz = default(T);    zz = initValue;
        }

        /// <summary>
        /// Implements the operator * (Multiply dyad on the value).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="num">The number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DyadCoordinate<T> operator *(
            DyadCoordinate<T> point1,
            T num)
        {
            return new DyadCoordinate<T>(
                (dynamic)point1.xx * num, (dynamic)point1.yx * num, (dynamic)point1.zx * num,
                (dynamic)point1.xy * num, (dynamic)point1.yy * num, (dynamic)point1.zy * num,
                (dynamic)point1.xz * num, (dynamic)point1.yz * num, (dynamic)point1.zz * num
               );
        }

        /// <summary>
        /// Implements the operator * (Multiply dyad on the value).
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="point1">The point1.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DyadCoordinate<T> operator *(
            T num,
            DyadCoordinate<T> point1)
        {
            return point1 * num;
        }

        /// <summary>
        /// Implements the operator - (Subtract two dyads).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DyadCoordinate<T> operator -(
            DyadCoordinate<T> point1,
            DyadCoordinate<T> point2)
        {
            return new DyadCoordinate<T>(
                (dynamic)point1.xx - point2.xx, (dynamic)point1.yx - point2.yx, (dynamic)point1.zx - point2.zx,
                (dynamic)point1.xy - point2.xy, (dynamic)point1.yy - point2.yy, (dynamic)point1.zy - point2.zy,
                (dynamic)point1.xz - point2.xz, (dynamic)point1.yz - point2.yz, (dynamic)point1.zz - point2.zz
               );
        }

        /// <summary>
        /// Implements the operator - (Subtract two dyads).
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static DyadCoordinate<T> operator +(
            DyadCoordinate<T> point1,
            DyadCoordinate<T> point2)
        {
            return new DyadCoordinate<T>(
                (dynamic)point1.xx + point2.xx, (dynamic)point1.yx + point2.yx, (dynamic)point1.zx + point2.zx,
                (dynamic)point1.xy + point2.xy, (dynamic)point1.yy + point2.yy, (dynamic)point1.zy + point2.zy,
                (dynamic)point1.xz + point2.xz, (dynamic)point1.yz + point2.yz, (dynamic)point1.zz + point2.zz
               );
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DyadCoordinate{T}"/> to <see cref="DyadCoordinate{Complex}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DyadCoordinate<Complex>(DyadCoordinate<T> value)
        {
            return new DyadCoordinate<Complex>(
                (dynamic)value.xx, (dynamic)value.yx, (dynamic)value.zx,
                (dynamic)value.xy, (dynamic)value.yy, (dynamic)value.zy,
                (dynamic)value.xz, (dynamic)value.yz, (dynamic)value.zz
               );
        }
    }
}
