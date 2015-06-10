using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Models.ConfigurationParameters
{
    public class FDTDField
    {
        public CartesianCoordinate[,,] D { get; set; }
        public CartesianCoordinate[,,] E { get; set; }
        public CartesianCoordinate[,,] H { get; set; }

        public CartesianCoordinate[,,] ID { get; set; }
        public CartesianCoordinate[,,] IH { get; set; }


    //int IE = numCells.x;
    //int JE = numCells.y;
    //int KE = numCells.z;
    //int ia = pmlLength;
    //int ja = pmlLength;
    //int ka = pmlLength;

    //InitFourier(t_frequency);

        public double[] Gi1 { get; set; }
        public double[] Gi2 { get; set; }
        public double[] Gi3 { get; set; }
        public double[] Gj1 { get; set; }
        public double[] Gj2 { get; set; }
        public double[] Gj3 { get; set; }
        public double[] Gk1 { get; set; }
        public double[] Gk2 { get; set; }
        public double[] Gk3 { get; set; }
        public double[] Fi1 { get; set; }
        public double[] Fi2 { get; set; }
        public double[] Fi3 { get; set; }
        public double[] Fj1 { get; set; }
        public double[] Fj2 { get; set; }
        public double[] Fj3 { get; set; }
        public double[] Fk1 { get; set; }
        public double[] Fk2 { get; set; }
        public double[] Fk3 { get; set; }

    //_boundaryConditionsCalc();


    }
}
