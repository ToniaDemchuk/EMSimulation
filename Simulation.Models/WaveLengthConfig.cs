namespace Simulation.Models
{
    public class WaveLengthConfig
    {
        public WaveLengthConfig(double lower, double upper, int count)
        {
            this.Lower = lower;
            this.Upper = upper;
            this.Count = count;
            this.Step = getStep();
        }

        public double Lower { get; private set; }

        public double Upper { get; private set; }

        public int Count { get; private set; }

        public double Step { get; private set; }

        private double getStep()
        {
            return (Upper - Lower) / (Count - 1);
        }

        double GetWaveLength(int i)
        {
            return this.Lower + Step * i;
        }
    }
}
