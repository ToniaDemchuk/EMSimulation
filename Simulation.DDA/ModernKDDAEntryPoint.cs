using System.Runtime.InteropServices;

namespace Simulation.DDA
{
    /// <summary>
    /// The ModernKDDAEntryPoint class.
    /// </summary>
    public class ModernKDDAEntryPoint
    {
        /// <summary>
        /// The OpenMP conjugate gradient method.
        /// </summary>
        /// <param name="systLength">The system length.</param>
        /// <param name="a">The matrix of coefficients of system.</param>
        /// <param name="x">The unknown vector.</param>
        /// <param name="b">The constant terms.</param>
        /// <returns>if 0 then method is success.</returns>
        [DllImport("ModernKDDA.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int OpenMPCGMethod(int systLength, double[] a, double[] x, double[] b);
    }
}