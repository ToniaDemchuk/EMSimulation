using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;

namespace AwokeKnowing.GnuplotCSharp
{
    public class GnuPlot : IDisposable
    {
        private Process ExtPro;
        private StreamWriter GnupStWr;
        private List<StoredPlot> PlotBuffer;
        private List<StoredPlot> SPlotBuffer;
        private bool ReplotWithSplot;

        private StreamReader StrRead;
        public bool Hold { get; private set; }

        public GnuPlot()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = getExePath(),
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            ExtPro = Process.Start(processStartInfo);
            GnupStWr = ExtPro.StandardInput;
            ExtPro.OutputDataReceived += this.extProOutputDataReceived;
            ExtPro.BeginOutputReadLine();

            PlotBuffer = new List<StoredPlot>();
            SPlotBuffer = new List<StoredPlot>();
            Hold = false;

            this.initOptions();
        }

        private void initOptions()
        {
            this.Set("terminal windows");
            this.Set(@"print ""-"";");
            this.Set(@"bind all ""Button1"" 'print MOUSE_X, MOUSE_Y;'");
        }

        private void extProOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var split = e.Data.Split(' ');
            double mx;
            double.TryParse(split[0], out mx);

            double my;
            double.TryParse(split[1], out my);

            var plotXY = this.PlotBuffer.Where(x => x.PlotType == PlotTypes.PlotXY)
                .SelectMany(plot => plot.X.Zip(plot.Y, (x, y) => new Tuple<double, double>(x, y)));
            var plotY = this.PlotBuffer.Where(x => x.PlotType == PlotTypes.PlotY)
                .SelectMany(p => p.Y.Select((y, i) => new Tuple<double, double>(i, y)));

            var min = plotXY.Concat(plotY)
                .Aggregate(
                    (a, b) =>
                    {
                        var distancea = Math.Pow(a.Item1 - mx, 2) + Math.Pow(a.Item2 - my, 2);
                        var distanceb = Math.Pow(b.Item1 - mx, 2) + Math.Pow(b.Item2 - my, 2);

                        return distancea < distanceb ? a : b;
                    });

            this.setCursor(min);

        }

        private void setCursor(Tuple<double, double> point)
        {
            this.Set(
                string.Format(
                    @"label 1 ""{0}, {1}"" at {0},{1} left point lt {2} pt {3} ps {4}",
                    point.Item1,
                    point.Item2,
                    1,
                    16,
                    (int)PointStyles.Star));
            this.Replot();
        }

        public void Wait()
        {
            ExtPro.WaitForExit();
        }

        private static string getExePath()
        {
            var enviromentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);

            var paths = enviromentPath.Split(';');
            var exePath = paths
                .Select(x => Path.Combine(x, "gnuplot.exe"))
                .FirstOrDefault(File.Exists);
            return exePath;
        }

        public void WriteLine(string gnuplotcommands)
        {

            GnupStWr.WriteLine(gnuplotcommands);
            GnupStWr.Flush();
        }

        public void Write(string gnuplotcommands)
        {
            GnupStWr.Write(gnuplotcommands);
            GnupStWr.Flush();
        }

        public void Set(params string[] options)
        {
            for (int i = 0; i < options.Length; i++)
                GnupStWr.WriteLine("set " + options[i]);

        }

        public void Unset(params string[] options)
        {
            for (int i = 0; i < options.Length; i++)
                GnupStWr.WriteLine("unset " + options[i]);
        }

        #region SaveData

        public bool SaveData(double[] Y, string filename)
        {
            StreamWriter dataStream = new StreamWriter(filename, false);
            WriteData(Y, dataStream);
            dataStream.Close();

            return true;
        }

        public bool SaveData(double[] X, double[] Y, string filename)
        {
            StreamWriter dataStream = new StreamWriter(filename, false);
            WriteData(X, Y, dataStream);
            dataStream.Close();

            return true;
        }

        public bool SaveData(double[] X, double[] Y, double[] Z, string filename)
        {
            StreamWriter dataStream = new StreamWriter(filename, false);
            WriteData(X, Y, Z, dataStream);
            dataStream.Close();

            return true;
        }

        public bool SaveData(int sizeY, double[] Z, string filename)
        {
            StreamWriter dataStream = new StreamWriter(filename, false);
            WriteData(sizeY, Z, dataStream);
            dataStream.Close();

            return true;
        }

        public bool SaveData(double[,] Z, string filename)
        {
            StreamWriter dataStream = new StreamWriter(filename, false);
            WriteData(Z, dataStream);
            dataStream.Close();

            return true;
        }

        #endregion


        public void Replot()
        {
            if (ReplotWithSplot)
                SPlot(SPlotBuffer);
            else
                Plot(PlotBuffer);
        }

        #region Plot

        public void Plot(string filenameOrFunction, string options = "")
        {
            if (!Hold) PlotBuffer.Clear();
            PlotBuffer.Add(new StoredPlot(filenameOrFunction, options));
            Plot(PlotBuffer);
        }

        public void Plot(double[] y, string options = "")
        {
            if (!Hold) PlotBuffer.Clear();
            PlotBuffer.Add(new StoredPlot(y, options));
            Plot(PlotBuffer);
        }

        public void Plot(double[] x, double[] y, string options = "")
        {
            if (!Hold) PlotBuffer.Clear();
            PlotBuffer.Add(new StoredPlot(x, y, options));
            Plot(PlotBuffer);
        }

        #endregion

        #region Contour

        public void Contour(
            string filenameOrFunction,
            string options = "",
            bool labelContours = true)
        {
            if (!Hold) PlotBuffer.Clear();
            var p = new StoredPlot(
                filenameOrFunction,
                options,
                PlotTypes.ContourFileOrFunction);
            p.LabelContours = labelContours;
            PlotBuffer.Add(p);
            Plot(PlotBuffer);
        }

        public void Contour(
            int sizeY,
            double[] z,
            string options = "",
            bool labelContours = true)
        {
            if (!Hold) PlotBuffer.Clear();
            var p = new StoredPlot(sizeY, z, options, PlotTypes.ContourZ);
            p.LabelContours = labelContours;
            PlotBuffer.Add(p);
            Plot(PlotBuffer);
        }

        public void Contour(
            double[] x,
            double[] y,
            double[] z,
            string options = "",
            bool labelContours = true)
        {
            if (!Hold) PlotBuffer.Clear();
            var p = new StoredPlot(x, y, z, options, PlotTypes.ContourXYZ);
            p.LabelContours = labelContours;
            PlotBuffer.Add(p);
            Plot(PlotBuffer);
        }

        public void Contour(double[,] zz, string options = "", bool labelContours = true)
        {
            if (!Hold) PlotBuffer.Clear();
            var p = new StoredPlot(zz, options, PlotTypes.ContourZZ);
            p.LabelContours = labelContours;
            PlotBuffer.Add(p);
            Plot(PlotBuffer);
        }

        #endregion

        #region HeatMap

        public void HeatMap(string filenameOrFunction, string options = "")
        {
            if (!Hold) PlotBuffer.Clear();
            PlotBuffer.Add(
                new StoredPlot(
                    filenameOrFunction,
                    options,
                    PlotTypes.ColorMapFileOrFunction));
            Plot(PlotBuffer);
        }

        public void HeatMap(int sizeY, double[] intensity, string options = "")
        {
            if (!Hold) PlotBuffer.Clear();
            PlotBuffer.Add(new StoredPlot(sizeY, intensity, options, PlotTypes.ColorMapZ));
            Plot(PlotBuffer);
        }

        public void HeatMap(
            double[] x,
            double[] y,
            double[] intensity,
            string options = "")
        {
            if (!Hold) PlotBuffer.Clear();
            PlotBuffer.Add(
                new StoredPlot(x, y, intensity, options, PlotTypes.ColorMapXYZ));
            Plot(PlotBuffer);
        }

        public void HeatMap(double[,] intensityGrid, string options = "")
        {
            if (!Hold) PlotBuffer.Clear();
            PlotBuffer.Add(new StoredPlot(intensityGrid, options, PlotTypes.ColorMapZZ));
            Plot(PlotBuffer);
        }

        #endregion

        #region SPlot

        public void SPlot(string filenameOrFunction, string options = "")
        {
            if (!Hold) SPlotBuffer.Clear();
            SPlotBuffer.Add(
                new StoredPlot(filenameOrFunction, options, PlotTypes.SplotFileOrFunction));
            SPlot(SPlotBuffer);
        }

        public void SPlot(int sizeY, double[] z, string options = "")
        {
            if (!Hold) SPlotBuffer.Clear();
            SPlotBuffer.Add(new StoredPlot(sizeY, z, options));
            SPlot(SPlotBuffer);
        }

        public void SPlot(double[] x, double[] y, double[] z, string options = "")
        {
            if (!Hold) SPlotBuffer.Clear();
            SPlotBuffer.Add(new StoredPlot(x, y, z, options));
            SPlot(SPlotBuffer);
        }

        public void SPlot(double[,] zz, string options = "")
        {
            if (!Hold) SPlotBuffer.Clear();
            SPlotBuffer.Add(new StoredPlot(zz, options));
            SPlot(SPlotBuffer);
        }

        #endregion



        public void Plot(List<StoredPlot> storedPlots)
        {
            ReplotWithSplot = false;
            string plot = "plot ";
            string plotstring = "";
            string contfile;
            string defcntopts;
            removeContourLabels();
            for (int i = 0; i < storedPlots.Count; i++)
            {
                var p = storedPlots[i];
                defcntopts = (p.Options.Length > 0 && (p.Options.Contains(" w") || p.Options[0] == 'w')) ? " " : " with lines ";
                switch (p.PlotType)
                {
                    case PlotTypes.PlotFileOrFunction:
                        if (p.File != null)
                            plotstring += (plot + plotPath(p.File) + " " + p.Options);
                        else
                            plotstring += (plot + p.Function + " " + p.Options);
                        break;
                    case PlotTypes.PlotXY:
                    case PlotTypes.PlotY:
                        plotstring += (plot + @"""-"" " + p.Options);
                        break;
                    case PlotTypes.ContourFileOrFunction:
                        contfile = Path.GetTempPath() + "_cntrtempdata" + i + ".dat";
                        makeContourFile((p.File != null ? plotPath(p.File) : p.Function), contfile);
                        if (p.LabelContours) setContourLabels(contfile);
                        plotstring += (plot + plotPath(contfile) + defcntopts + p.Options);
                        break;
                    case PlotTypes.ContourXYZ:
                        contfile = Path.GetTempPath() + "_cntrtempdata" + i + ".dat";
                        makeContourFile(p.X, p.Y, p.Z, contfile);
                        if (p.LabelContours) setContourLabels(contfile);
                        plotstring += (plot + plotPath(contfile) + defcntopts + p.Options);
                        break;
                    case PlotTypes.ContourZZ:
                        contfile = Path.GetTempPath() + "_cntrtempdata" + i + ".dat";
                        makeContourFile(p.ZZ, contfile);
                        if (p.LabelContours) setContourLabels(contfile);
                        plotstring += (plot + plotPath(contfile) + defcntopts + p.Options);
                        break;
                    case PlotTypes.ContourZ:
                        contfile = Path.GetTempPath() + "_cntrtempdata" + i + ".dat";
                        makeContourFile(p.YSize, p.Z, contfile);
                        if (p.LabelContours) setContourLabels(contfile);
                        plotstring += (plot + plotPath(contfile) + defcntopts + p.Options);
                        break;


                    case PlotTypes.ColorMapFileOrFunction:
                        if (p.File != null)
                            plotstring += (plot + plotPath(p.File) + " with image " + p.Options);
                        else
                            plotstring += (plot + p.Function + " with image " + p.Options);
                        break;
                    case PlotTypes.ColorMapXYZ:
                    case PlotTypes.ColorMapZ:
                        plotstring += (plot + @"""-"" " + " with image " + p.Options);
                        break;
                    case PlotTypes.ColorMapZZ:
                        plotstring += (plot + @"""-"" " + "matrix with image " + p.Options);
                        break;
                }
                if (i == 0) plot = ", ";
            }
            GnupStWr.WriteLine(plotstring);

            for (int i = 0; i < storedPlots.Count; i++)
            {
                var p = storedPlots[i];
                switch (p.PlotType)
                {
                    case PlotTypes.PlotXY:
                        WriteData(p.X, p.Y, GnupStWr, false);
                        GnupStWr.WriteLine("e");
                        break;
                    case PlotTypes.PlotY:
                        WriteData(p.Y, GnupStWr, false);
                        GnupStWr.WriteLine("e");
                        break;
                    case PlotTypes.ColorMapXYZ:
                        WriteData(p.X, p.Y, p.Z, GnupStWr, false);
                        GnupStWr.WriteLine("e");
                        break;
                    case PlotTypes.ColorMapZ:
                        WriteData(p.YSize, p.Z, GnupStWr, false);
                        GnupStWr.WriteLine("e");
                        break;
                    case PlotTypes.ColorMapZZ:
                        WriteData(p.ZZ, GnupStWr, false);
                        GnupStWr.WriteLine("e");
                        GnupStWr.WriteLine("e");
                        break;

                }
            }
            GnupStWr.Flush();
        }

        public void SPlot(List<StoredPlot> storedPlots)
        {
            ReplotWithSplot = true;
            var splot = "splot ";
            string plotstring = "";
            string defopts = "";
            removeContourLabels();
            for (int i = 0; i < storedPlots.Count; i++)
            {
                var p = storedPlots[i];
                defopts = (p.Options.Length > 0 && (p.Options.Contains(" w") || p.Options[0] == 'w')) ? " " : " with lines ";
                switch (p.PlotType)
                {
                    case PlotTypes.SplotFileOrFunction:
                        if (p.File != null)
                            plotstring += (splot + plotPath(p.File) + defopts + p.Options);
                        else
                            plotstring += (splot + p.Function + defopts + p.Options);
                        break;
                    case PlotTypes.SplotXYZ:
                    case PlotTypes.SplotZ:
                        plotstring += (splot + @"""-"" " + defopts + p.Options);
                        break;
                    case PlotTypes.SplotZZ:
                        plotstring += (splot + @"""-"" matrix " + defopts + p.Options);
                        break;
                }
                if (i == 0) splot = ", ";
            }
            GnupStWr.WriteLine(plotstring);

            for (int i = 0; i < storedPlots.Count; i++)
            {
                var p = storedPlots[i];
                switch (p.PlotType)
                {
                    case PlotTypes.SplotXYZ:
                        WriteData(p.X, p.Y, p.Z, GnupStWr, false);
                        GnupStWr.WriteLine("e");
                        break;
                    case PlotTypes.SplotZZ:
                        WriteData(p.ZZ, GnupStWr, false);
                        GnupStWr.WriteLine("e");
                        GnupStWr.WriteLine("e");
                        break;
                    case PlotTypes.SplotZ:
                        WriteData(p.YSize, p.Z, GnupStWr, false);
                        GnupStWr.WriteLine("e");
                        break;
                }
            }
            GnupStWr.Flush();
        }

        #region WriteData

        public void WriteData(double[] y, StreamWriter stream, bool flush = true)
        {
            for (int i = 0; i < y.Length; i++) stream.WriteLine(y[i].ToString());

            if (flush) stream.Flush();
        }

        public void WriteData(
            double[] x,
            double[] y,
            StreamWriter stream,
            bool flush = true)
        {
            for (int i = 0; i < y.Length; i++) stream.WriteLine(x[i].ToString() + " " + y[i].ToString());

            if (flush) stream.Flush();
        }

        public void WriteData(
            int ySize,
            double[] z,
            StreamWriter stream,
            bool flush = true)
        {
            for (int i = 0; i < z.Length; i++)
            {
                if (i > 0 && i % ySize == 0) stream.WriteLine();
                stream.WriteLine(z[i].ToString());
            }

            if (flush) stream.Flush();
        }

        public void WriteData(double[,] zz, StreamWriter stream, bool flush = true)
        {
            int m = zz.GetLength(0);
            int n = zz.GetLength(1);
            string line;
            for (int i = 0; i < m; i++)
            {
                line = "";
                for (int j = 0; j < n; j++) line += zz[i, j].ToString() + " ";
                stream.WriteLine(line.TrimEnd());
            }

            if (flush) stream.Flush();
        }

        public void WriteData(
            double[] x,
            double[] y,
            double[] z,
            StreamWriter stream,
            bool flush = true)
        {
            int m = Math.Min(x.Length, y.Length);
            m = Math.Min(m, z.Length);
            for (int i = 0; i < m; i++)
            {
                if (i > 0 && x[i] != x[i - 1]) stream.WriteLine("");
                stream.WriteLine(x[i] + " " + y[i] + " " + z[i]);
            }

            if (flush) stream.Flush();
        }

        #endregion


        string plotPath(string path)
        {
            return "\"" + path.Replace(@"\", @"\\") + "\"";
        }

        public void SaveSetState(string filename = null)
        {
            if (filename == null)
                filename = Path.GetTempPath() + "setstate.tmp";
            GnupStWr.WriteLine("save set " + plotPath(filename));
            GnupStWr.Flush();
            waitForFile(filename);
        }
        public void LoadSetState(string filename = null)
        {
            if (filename == null)
                filename = Path.GetTempPath() + "setstate.tmp";
            GnupStWr.WriteLine("load " + plotPath(filename));
            GnupStWr.Flush();
        }

        //these makecontourFile functions should probably be merged into one function and use a StoredPlot parameter
        void makeContourFile(string fileOrFunction, string outputFile)//if it's a file, fileOrFunction needs quotes and escaped backslashes
        {
            this.prepareContour(outputFile);
            GnupStWr.WriteLine(@"splot " + fileOrFunction);
            this.postContour(outputFile);
        }



        void makeContourFile(double[] x, double[] y, double[] z, string outputFile)
        {
            this.prepareContour(outputFile);
            GnupStWr.WriteLine(@"splot ""-""");
            WriteData(x, y, z, GnupStWr);
            GnupStWr.WriteLine("e");
            this.postContour(outputFile);
        }

        void makeContourFile(double[,] zz, string outputFile)
        {
            this.prepareContour(outputFile);
            GnupStWr.WriteLine(@"splot ""-"" matrix");
            WriteData(zz, GnupStWr);
            GnupStWr.WriteLine("e");
            GnupStWr.WriteLine("e");
            this.postContour(outputFile);
        }

        void makeContourFile(int sizeY, double[] z, string outputFile)
        {
            this.prepareContour(outputFile);
            GnupStWr.WriteLine(@"splot ""-""");
            WriteData(sizeY, z, GnupStWr);
            GnupStWr.WriteLine("e");
            this.postContour(outputFile);
        }


        private void prepareContour(string outputFile)
        {
            this.SaveSetState();
            this.Set("table " + this.plotPath(outputFile));
            this.Set("contour base");
            this.Unset("surface");
        }
        private void postContour(string outputFile)
        {
            this.Unset("table");
            this.GnupStWr.Flush();
            this.LoadSetState();
            this.waitForFile(outputFile);
        }

        int contourLabelCount = 50000;
        void setContourLabels(string contourFile)
        {
            var file = new StreamReader(contourFile);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("label:"))
                {
                    string[] c = file.ReadLine().Trim().Replace("   ", " ").Replace("  ", " ").Split(' ');
                    GnupStWr.WriteLine("set object " + ++contourLabelCount + " rectangle center " + c[0] + "," + c[1] + " size char " + (c[2].ToString().Length + 1) + ",char 1 fs transparent solid .7 noborder fc rgb \"white\"  front");
                    GnupStWr.WriteLine("set label " + contourLabelCount + " \"" + c[2] + "\" at " + c[0] + "," + c[1] + " front center");
                }
            }
            file.Close();
        }
        void removeContourLabels()
        {
            while (contourLabelCount > 50000)
                GnupStWr.WriteLine("unset object " + contourLabelCount + ";unset label " + contourLabelCount--);
        }

        bool waitForFile(string filename, int timeout = 10000)
        {
            Thread.Sleep(20);
            int attempts = timeout / 100;
            StreamReader file = null;
            while (file == null)
            {
                try { file = new StreamReader(filename); }
                catch
                {
                    if (attempts-- > 0)
                        Thread.Sleep(100);
                    else
                        return false;
                }
            }
            file.Close();
            return true;
        }

        public void HoldOn()
        {
            Hold = true;
            PlotBuffer.Clear();
            SPlotBuffer.Clear();
        }

        public void HoldOff()
        {
            Hold = false;
            PlotBuffer.Clear();
            SPlotBuffer.Clear();
        }

        public void Close()
        {
            this.ExtPro.CloseMainWindow();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
