using System;
using ScilabEngine.Helpers;

namespace ScilabEngine.Engine
{
    public class ScilabEngine : IDisposable
    {
        public ScilabEngine()
        {
            if (ScilabEntryPoint.StartScilab(null, null, 0) != 1)
            {
                throw new Exception("Error while starting scilab engine");
            }
        }

        public void Execute(string job)
        {
            if (ScilabEntryPoint.SendScilabJob(job) != 0)
            {
                throw new Exception("Error while executing script");
            }
        }

        public void Execute(string format, params object[] objects)
        {
            this.Execute(string.Format(format, objects));
        }

        public void Wait()
        {
            while (true)
            {
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (ScilabEntryPoint.TerminateScilab(null) != 1)
            {
                throw new Exception("Error while closing scilab engine");
            }
        }
    }
}