using System.Runtime.InteropServices;
using System.Text;

namespace ScilabEngine.Helpers
{
    /// <summary>
    /// The ScilabEntryPoint class.
    /// </summary>
    public class ScilabEntryPoint
    {
        /// <summary>
        /// Starts the scilab.
        /// </summary>
        /// <param name="sciPath">The scilab path.</param>
        /// <param name="scilabStartup">The scilab startup.</param>
        /// <param name="stacksize">The stack size.</param>
        /// <returns>The return code.</returns>
        [DllImport(@"C:\Program Files\scilab-5.5.2\bin\call_scilab.dll", EntryPoint = "StartScilab", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartScilab(StringBuilder sciPath, StringBuilder scilabStartup, int stacksize);

        /// <summary>
        /// Terminates the scilab.
        /// </summary>
        /// <param name="scilabQuit">The scilab quit.</param>
        /// <returns>The return code.</returns>
        [DllImport(@"C:\Program Files\scilab-5.5.2\bin\call_scilab.dll", EntryPoint = "TerminateScilab", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TerminateScilab(StringBuilder scilabQuit);

        /// <summary>
        /// Sends the scilab job.
        /// </summary>
        /// <param name="job">The job to execute.</param>
        /// <returns>The return code.</returns>
        [DllImport(@"C:\Program Files\scilab-5.5.2\bin\call_scilab.dll", EntryPoint = "SendScilabJob", ExactSpelling = false,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendScilabJob(string job);
    }
}