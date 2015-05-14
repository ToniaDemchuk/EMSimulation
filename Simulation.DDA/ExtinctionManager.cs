using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation.Models;

namespace Simulation.DDA
{
    public class ExtinctionManager
    {
        //        std::complex< double > SelectOpticalConst( double WaveLength, OptConst *optConst)
        //{
        //    for(int k = 0; k < optConst->size_lamda; k++){
        //        if( (WaveLength >= optConst->lamda_file[k]) && (WaveLength < optConst->lamda_file[k+1]) ){
        //            double eps_re = (WaveLength-optConst->lamda_file[k]) * 
        //                (optConst->eps_re_file[k+1]-optConst->eps_re_file[k]) / 
        //                (optConst->lamda_file[k+1]-optConst->lamda_file[k]) + optConst->eps_re_file[k];
        //            double eps_im = (WaveLength-optConst->lamda_file[k]) * 
        //                (optConst->eps_im_file[k+1]-optConst->eps_im_file[k]) / 
        //                (optConst->lamda_file[k+1]-optConst->lamda_file[k]) + optConst->eps_im_file[k];
        //            std::complex< double > epsilon(eps_re, eps_im);
        //            return epsilon;
        //        }
        //    }

        //    //eps_re = 3.9943 - (13.29e+15*13.29e+15)/((4.0*Pi*Pi*3.0e8*3e8/WaveLength/WaveLength*1e18)+(0.1128e+15*0.1128e+15));
        //    //eps_im = (13.29e+15*13.29e+15*0.1128e+15)/((4.0*Pi*Pi*3e8*3e8/WaveLength/WaveLength*1e18)+(0.1128e+15*0.1128e+15))/(2.0*Pi*3e8/WaveLength*1e9);


        //    return 0;
        //}

        //double GetMediumCoeficient(bool solid, double WaveLength)
        //{
        //    double n_med;

        //    //		n_med = sqrt( 1.0 + 2.121*WaveLength*WaveLength / (WaveLength*WaveLength-263.3*263.3) );	// CdBr2 (300K)
        //    //		n_med = 0.7746 + 0.14147*exp(-(WaveLength-59.0737)/122.56035)
        //    //			 + 0.99808*exp(-(WaveLength-59.0737)/42.78953) + 0.56471*exp(-(WaveLength-59.0737)/36297.71849);	// Waher
        //    n_med = 1; //air

        //    //n_med = 1.39; //water

        //    //		n_med = 1.51; // Glass 1.51

        //    //		n_med = 1.65; // Casein has a refractive index of 1.51-1.65

        //    if(solid){ n_med = 1.0; }
        //    else{ n_med = n_med; }

        //    return n_med;
        //}



        //double * CalculateCrossExtinction(Params* params, WaveConfig* waveConfig, bool solid)
        //{
        //    int size_Adbl = params->systConfig->size_syst * 6;  // розмірність матриці А, Р, Е представлена в дійсних числах

        //    double* P = CreateEmpty(size_Adbl);

        //    double *extinctions = new double[waveConfig->WaveLength_count];

        //    for( int i = 0; i < waveConfig->WaveLength_count; i++ ){
        //        double WaveLength = waveConfig->getWaveLength(i);

        //        double CrossExtinction = CalculationCrossExtinction(P, WaveLength, waveConfig->angles, params, solid);

        //        extinctions[i] = CrossExtinction;
        //    }

        //    delete [] P;

        //    return extinctions;
        //}

        //double CalculationCrossExtinction(double *P, //iterative value of polarization
        //                                  double WaveLength, 
        //                                  EulerAngles angle,	
        //                                  Params *params,
        //                                  bool solid )
        //{
        //    double *Exyz; // Компоненти напруженості поля на осї x, y, z
        //    double *k; // Компоненти k-вектора на осї x, y, z
        //    double *E; // Напруженість поля (стовбець вільних членів)

        //    std::complex<double> eps = SelectOpticalConst( WaveLength, params->optConst);

        //    double n_med = GetMediumCoeficient(solid, WaveLength);

        //    PropertysIncidentField( WaveLength, n_med, params->systConfig, angle, &Exyz, &k, &E );

        //    double *A = OpenMPBuildMatrixA( params->systConfig, n_med, eps, k, solid );

        //    //	PrintMatrix( size_Adbl, A );

        //    int size_Adbl = params->systConfig->size_syst * 6;  // розмірність матриці А, Р, Е представлена в дійсних числах

        //    OpenMPCGMethod( size_Adbl, A, P, E );

        //    double CrossExtinction = BuildExtinction( params->systConfig, n_med, Exyz, k, P);

        //    delete [] Exyz;
        //    delete [] k;
        //    delete [] E;
        //    delete [] A;

        //    return CrossExtinction;
        //}



        //int PropertysIncidentField(double WaveLength, double n_med, SystConfig *system, EulerAngles angle, double **tExyz, double **tk, double **tE )
        //{
        //    int size_Adbl = system->size_syst * 6;  // розмірність матриці А, Р, Е представлена в дійсних числах

        //    // Кути повороту системи поля, вказані в радіанах
        //    double theta = angle.theta * Pi / 180.0;
        //    double psi = angle.psi * Pi / 180.0;
        //    double polyar = angle.polyar * Pi / 180.0;

        //    double *Exyz = new double [4];

        //    Exyz[0] = 1.0;
        //    Exyz[1] = cos(polyar);
        //    Exyz[2] = sin(polyar);

        //    double yx = Exyz[1] * sin(psi) * cos(theta);
        //    double yy = Exyz[1] * cos(psi);
        //    double yz = Exyz[1] * sin(psi) * sin(theta);

        //    double zx = Exyz[2] * cos(psi) * sin(theta);
        //    double zy = Exyz[2] * sin(psi) * sin(theta);
        //    double zz = Exyz[2] * cos(theta);

        //    Exyz[0] = yx + zx;	// Ex 1
        //    Exyz[1] = yy + zy;	// Ey 0
        //    Exyz[2] = yz + zz;	// Ez 0
        //    Exyz[3] = 1.0;		// E_modul

        //    double *k = new double [4];

        //    k[3] = 2.0 * Pi * n_med / WaveLength;	// k_mod
        //    k[0] = k[3] * cos(psi) * cos(theta);	// kx 0
        //    k[1] = k[3] * sin(psi) * cos(theta);	// ky 0
        //    k[2] = k[3] * sin(theta);				// kz 1

        //    double *E = new double [size_Adbl];
        //    double eps_m = n_med*n_med;

        //    for(int j = 0, j6 = 0; j6 < size_Adbl; j++, j6+=6){
        //        CartesianCoordinate point = system->points[j];
        //        double kr = k[0]*point.x + k[1]*point.y + k[2]*point.z;
        //        double cos_kr = cos(kr);
        //        double sin_kr = sin(kr);

        //        E[j6]   = Exyz[0] * cos_kr * eps_m;
        //        E[j6+1] = Exyz[0] * sin_kr * eps_m;
        //        E[j6+2] = Exyz[1] * cos_kr * eps_m;
        //        E[j6+3] = Exyz[1] * sin_kr * eps_m;
        //        E[j6+4] = Exyz[2] * cos_kr * eps_m;
        //        E[j6+5] = Exyz[2] * sin_kr * eps_m;
        //    }

        //    *tExyz = Exyz;
        //    *tk = k;
        //    *tE = E;

        //    return 0;
        //}

        double CalculateCrossSectionExtinction(SystemConfig system, double refractiveIndex, double[] Exyz, double[] k, double[] P)
        {
            int size_Adbl = system.Size * 6;  // розмірність матриці А, Р, Е представлена в дійсних числах

            double kr;
            double cos_kr, sin_kr;
            double eps_m = refractiveIndex * refractiveIndex;

            double const_Cext = 4.0 * Math.PI * k[3] / (Exyz[3] * Exyz[3] * eps_m);
            double Cext = 0.0;
            var waveNumber = new CartesianCoordinate(k[0], k[1], k[2]);
            for (int j = 0, j6 = 0; j6 < size_Adbl; j++, j6 += 6)
            {
                CartesianCoordinate point = system.Points[j];
                kr = -point * waveNumber;
                cos_kr = Math.Cos(kr);
                sin_kr = Math.Sin(kr);

                Cext += Exyz[0] * (cos_kr * P[j6 + 1] + sin_kr * P[j6]);
                Cext += Exyz[1] * (cos_kr * P[j6 + 3] + sin_kr * P[j6 + 2]);
                Cext += Exyz[2] * (cos_kr * P[j6 + 5] + sin_kr * P[j6 + 4]);
            }

            return Cext * const_Cext;
        }
    }
}
