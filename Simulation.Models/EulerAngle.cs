namespace Simulation.Models
{
    public class EulerAngle
    {
        public EulerAngle(double polyar, double theta, double psi)
        {
            this.Polyar = polyar;
            this.Theta = theta;
            this.Psi = psi;
        }

        public double Polyar { get; private set; }

        public double Theta { get; private set; }

        public double Psi { get; private set; }
    }
}
