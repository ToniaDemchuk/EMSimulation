using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.DDA.Console
{
    class Program
    {

        [DllImport("ModernKDDA.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TestMethod1(int syst, double[] A, double[] x, double[] b);

        static void Main(string[] args)
        {

            var list1 = new double[] { 2 };
            var list2 = new double[] { 3 };
            var list3 = new double[] { 4 };

            var res = TestMethod1(6, list1, list2, list3);
            var rea = 1;
        }
    }
}
