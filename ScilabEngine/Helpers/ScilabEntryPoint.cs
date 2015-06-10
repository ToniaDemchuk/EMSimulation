using System.Runtime.InteropServices;
using System.Text;

namespace ScilabEngine.Helpers
{
    public class ScilabEntryPoint
    {
        [DllImport(@"C:\Program Files\scilab-5.5.2\bin\call_scilab.dll", EntryPoint = "StartScilab", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartScilab(StringBuilder SCIpath, StringBuilder ScilabStartup, int Stacksize);

        [DllImport(@"C:\Program Files\scilab-5.5.2\bin\call_scilab.dll", EntryPoint = "TerminateScilab", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TerminateScilab(StringBuilder ScilabQuit);

        [DllImport(@"C:\Program Files\scilab-5.5.2\bin\call_scilab.dll", EntryPoint = "SendScilabJob", ExactSpelling = false,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendScilabJob(string job);
    }
}