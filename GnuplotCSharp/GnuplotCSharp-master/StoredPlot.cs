namespace AwokeKnowing.GnuplotCSharp
{
    public class StoredPlot
    {
        public string File = null;
        public string Function = null;
        public double[] X;
        public double[] Y;
        public double[] Z;
        public double[,] ZZ;
        public int YSize;
        public string Options;
        public PlotTypes PlotType;
        public bool LabelContours;

        public StoredPlot()
        {
        }
        public StoredPlot(string functionOrfilename, string options = "", PlotTypes plotType = PlotTypes.PlotFileOrFunction)
        {
            if (this.IsFile(functionOrfilename))
                this.File = functionOrfilename;
            else
                this.Function = functionOrfilename;
            this.Options = options;
            this.PlotType = plotType;
        }

        public StoredPlot(double[] y, string options = "")
        {
            this.Y = y;
            this.Options = options;
            this.PlotType = PlotTypes.PlotY;
        }

        public StoredPlot(double[] x, double[] y, string options = "")
        {
            this.X = x;
            this.Y = y;
            this.Options = options;
            this.PlotType = PlotTypes.PlotXY;
        }

        //3D data
        public StoredPlot(int sizeY, double[] z, string options = "", PlotTypes plotType = PlotTypes.SplotZ)
        {
            this.YSize = sizeY;
            this.Z = z;
            this.Options = options;
            this.PlotType = plotType;
        }

        public StoredPlot(double[] x, double[] y, double[] z, string options = "", PlotTypes plotType = PlotTypes.SplotXYZ)
        {
            if (x.Length < 2)
                this.YSize = 1;
            else
                for (this.YSize = 1; this.YSize < x.Length; this.YSize++)
                    if (x[this.YSize] != x[this.YSize - 1])
                        break;
            this.Z = z;
            this.Y = y;
            this.X = x;
            this.Options = options;
            this.PlotType = plotType;
        }

        public StoredPlot(double[,] zz, string options = "", PlotTypes plotType = PlotTypes.SplotZZ)
        {
            this.ZZ = zz;
            this.Options = options;
            this.PlotType = plotType;
        }

        private bool IsFile(string functionOrFilename)
        {
            int dot = functionOrFilename.LastIndexOf(".");
            if (dot < 1) return false;
            if (char.IsLetter(functionOrFilename[dot - 1]) || char.IsLetter(functionOrFilename[dot + 1]))
                return true;
            return false;
        }

    }
}