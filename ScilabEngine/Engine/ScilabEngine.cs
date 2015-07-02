using System;
using ScilabEngine.Helpers;

namespace ScilabEngine.Engine
{
    /// <summary>
    /// The ScilabEngine class.
    /// </summary>
    public class ScilabEngine : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScilabEngine"/> class.
        /// </summary>
        /// <exception cref="System.Exception">Error while starting scilab engine</exception>
        public ScilabEngine()
        {
            if (ScilabEntryPoint.StartScilab(null, null, 0) != 1)
            {
                throw new Exception("Error while starting scilab engine");
            }
        }

        /// <summary>
        /// Executes the specified job.
        /// </summary>
        /// <param name="job">The job to execute.</param>
        /// <exception cref="System.Exception">Error while executing script</exception>
        public void Execute(string job)
        {
            if (ScilabEntryPoint.SendScilabJob(job) != 0)
            {
                throw new Exception("Error while executing script");
            }
        }

        /// <summary>
        /// Executes the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="objects">The objects.</param>
        public void Execute(string format, params object[] objects)
        {
            this.Execute(string.Format(format, objects));
        }

        /// <summary>
        /// Waits this instance.
        /// </summary>
        public void Wait()
        {
            while (true)
            {
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.Exception">Error while closing scilab engine</exception>
        public void Dispose()
        {
            if (ScilabEntryPoint.TerminateScilab(null) != 1)
            {
                throw new Exception("Error while closing scilab engine");
            }
        }
    }
}