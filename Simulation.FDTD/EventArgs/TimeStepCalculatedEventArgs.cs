using Simulation.FDTD.Models;

namespace Simulation.FDTD.EventArgs
{
    public class TimeStepCalculatedEventArgs : System.EventArgs
    {
        public FDTDField Fields { get; set; }

        public FDTDPulse Pulse { get; set; }
        public SimulationParameters Parameters { get; set; }
    }
}
