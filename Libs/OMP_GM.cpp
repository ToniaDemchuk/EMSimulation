//by Igor Kolych Date 15.01.2011

#include "DDA.h"
#include <cmath>
#include <iostream>
#include <omp.h>

using namespace nsModels;

double* OpenMPBuildMatrixA(SystConfig* system, double n_med, std::complex<double> eps, double* k, bool solid)
{
	double eps_m = n_med * n_med;

	double resize = 197.35;
	double w = resize * k[3]; // [eV]
	double wp = 9.2; // [eV] Ag.
	double gamma0 = 0.02; //[eV]
	double vf = 0.915;//2.6;//0.915;	// [eV*nm] Зміна оптичних констант зі зміною радіуса частинки
	double Aparam = 0.75;
	double eps_re_mod, eps_im_mod;

	int size_A = system->size_syst * 3;
	int size_Adbl = system->size_syst * 6;
	double radiation = 2.0 / 3.0 * k[3] * k[3] * k[3];// доданок, що відповідає за релаксаційне випромінювання.

	// Частини матриці А для побудови діагональних елементів
	double* A_1 = new double [9];
	double* A_2 = new double [9];
	double* A = new double [size_Adbl * size_Adbl / 2];

	for (int j = 0, j3 = 0; j3 < size_A; j++ , j3 += 3)
	{
		for (int i = 0, i3 = 0; i3 < size_A; i3 += 3 , i++)
		{
			if (i3 == j3)
			{
				//for(int jj = 0; jj < 6; jj+=2){
				for (int jj = 0; jj < 3; jj++)
				{
					for (int ii = 0; ii < 6; ii += 2)
					{
						if ((2 * i3 + ii) == (2 * j3 + 2 * jj))
						{
							double rad_3 = system->rad[j] * system->rad[j] * system->rad[j];
							if (solid)
							{
								eps_re_mod = eps.real();
								eps_im_mod = eps.imag();
							}
							else
							{
								double gamma = gamma0 + Aparam * vf / system->rad[j];
								eps_re_mod = eps.real() + wp * wp / (w * w + gamma0 * gamma0) - wp * wp / (w * w + gamma * gamma);
								eps_im_mod = eps.imag() - wp * wp * gamma0 / (w * w * w + w * gamma0 * gamma0) + wp * wp * gamma / (w * w * w + w * gamma * gamma);
							}

							double delta_znam = (eps_re_mod - 1.0 * eps_m) * (eps_re_mod - 1.0 * eps_m) + eps_im_mod * eps_im_mod;
							double delta_re = (eps_re_mod - 1.0 * eps_m) * (eps_re_mod + 2.0 * eps_m) + eps_im_mod * eps_im_mod;
							double delta_im = -3.0 * eps_im_mod * eps_m;
							// Дійсна і уявна частини величени оберненої до поляризованості за формулою Клаузіусса-Мосотті.
							double alpha_re_1 = delta_re / delta_znam;
							double alpha_im_1 = delta_im / delta_znam;

							A[size_Adbl * (j3 + jj) + (2 * i3 + ii)] = alpha_re_1 / rad_3;
							//A[size_Adbl*(2*j3+jj+1)+(2*i3+ii+1)] = alpha_re_1 / rad_3;
							A[size_Adbl * (j3 + jj) + (2 * i3 + ii + 1)] = -alpha_im_1 / rad_3 + radiation;
							//A[size_Adbl*(2*j3+jj+1)+(2*i3+ii)] = alpha_im_1 / rad_3 - radiation;
						}
						else
						{
							A[size_Adbl * (j3 + jj) + (2 * i3 + ii)] = 0.0;
							A[size_Adbl * (j3 + jj) + (2 * i3 + ii + 1)] = 0.0;
							//A[size_Adbl*(2*j3+jj+1)+(2*i3+ii)]   = 0.0;
							//A[size_Adbl*(2*j3+jj+1)+(2*i3+ii+1)] = 0.0;
						}
					}
				}
			}
			else
			{
				// Відстані між диполями.
				CartesianCoordinate pointi = system->points[i];
				CartesianCoordinate pointj = system->points[j];
				double rjk = pointj.getDistance(pointi);
					
				double kr = k[3] * rjk;
				double cos_kr = cos(kr);
				double sin_kr = sin(kr);

				double rjk_2 = rjk * rjk;
				double rjk_3 = rjk_2 * rjk;
				double rjk_4 = rjk_3 * rjk;

				A_1[0] = -(pow(pointj.y - pointi.y, 2) +
					pow(pointj.z - pointi.z, 2));
				A_1[1] = (pointj.x - pointi.x) * (pointj.y - pointi.y);
				A_1[2] = (pointj.x - pointi.x) * (pointj.z - pointi.z);

				A_1[3 * (1)] = A_1[1];
				A_1[3 * (1) + 1] = -(pow(pointj.x - pointi.x, 2) +
					pow(pointj.z - pointi.z, 2));
				A_1[3 * (1) + 2] = (pointj.y - pointi.y) * (pointj.z - pointi.z);

				A_1[3 * (2)] = A_1[2];
				A_1[3 * (2) + 1] = A_1[3 * (1) + 2];
				A_1[3 * (2) + 2] = -(pow(pointj.x - pointi.x, 2) +
					pow(pointj.y - pointi.y, 2));

				A_2[0] = -2 * pow(pointj.x - pointi.x, 2) +
					pow(pointj.y - pointi.y, 2) +
					pow(pointj.z - pointi.z, 2);
				A_2[1] = -3 * (pointj.x - pointi.x) * (pointj.y - pointi.y);
				A_2[2] = -3 * (pointj.x - pointi.x) * (pointj.z - pointi.z);

				A_2[3 * (1)] = A_2[1];
				A_2[3 * (1) + 1] = pow(pointj.x - pointi.x, 2) -
					2 * pow(pointj.y - pointi.y, 2) +
					pow(pointj.z - pointi.z, 2);
				A_2[3 * (1) + 2] = -3 * (pointj.y - pointi.y) * (pointj.z - pointi.z);

				A_2[3 * (2)] = A_2[2];
				A_2[3 * (2) + 1] = A_2[3 * (1) + 2];
				A_2[3 * (2) + 2] = pow(pointj.x - pointi.x, 2) +
					pow(pointj.y - pointi.y, 2) -
					2 * pow(pointj.z - pointi.z, 2);

				//				for(int j1 = 0, jj = 0; jj < 6; j1++, jj+=2){
				for (int j1 = 0, jj = 0; jj < 3; j1++ , jj++)
				{
					for (int i1 = 0, ii = 0; ii < 6; i1++ , ii += 2)
					{
						double A_re_temp = 1.0 / rjk_3 * (k[3] * k[3] * A_1[3 * (j1) + (i1)] + A_2[3 * (j1) + (i1)] / rjk_2);
						double A_im_temp = -k[3] / rjk_4 * A_2[3 * (j1) + (i1)];

						A[size_Adbl * (j3 + jj) + (2 * i3 + ii)] = A_re_temp * cos_kr - A_im_temp * sin_kr;
						A[size_Adbl * (j3 + jj) + (2 * i3 + ii + 1)] = - A_re_temp * sin_kr - A_im_temp * cos_kr;
						//A[size_Adbl*(2*j3+jj+1)+(2*i3+ii)] = A_re_temp * sin_kr + A_im_temp * cos_kr;
						//A[size_Adbl*(2*j3+jj+1)+(2*i3+ii+1)] = A_re_temp * cos_kr - A_im_temp * sin_kr;
					}
				}
			}
		}
	}

	/*
	double period, replicaX;
	double kr_kR, cos_kr_kR, sin_kr_kR;

	const int numReplica = 5;
	for( int rX = -numReplica; rX <= numReplica; rX++ ){
	if( !rX ){ rX = 1; }	// if(!rX){ continue; }
	period = 1500.0;
	replicaX = period * rX;

	//CalculationNotDiagonalElementsMatrixA( size_syst, x, y, z, rjk, A_1, A_2, replicaX );

	for(int j = 0, j3 = 0; j3 < size_A; j++, j3+=3){
	for(int i = 0, i3 = 0; i3 < size_A; i3+=3, i++){
	//kr_kR = k[3] * rjk[size_syst*j+i] + k[0] * replicaX;
	//cos_kr_kR = cos(kr_kR);
	//sin_kr_kR = sin(kr_kR);

	//rjk_2 = rjk[size_syst*j+i]*rjk[size_syst*j+i];
	//rjk_3 = rjk_2 * rjk[size_syst*j+i];
	//rjk_4 = rjk_3 * rjk[size_syst*j+i];

	//Calculation non Diagonal Elements of Matrix A
	rjk = sqrt( (x[j]-x[i]+replicaX)*(x[j]-x[i]+replicaX) + (y[j]-y[i])*(y[j]-y[i]) 
	+ (z[j]-z[i])*(z[j]-z[i]) );
	kr = k[3] * rjk;
	cos_kr = cos(kr);
	sin_kr = sin(kr);

	rjk_2 = rjk*rjk;
	rjk_3 = rjk_2 * rjk;
	rjk_4 = rjk_3 * rjk;

	A_1[0] = -((y[j]-y[i])*(y[j]-y[i]) + (z[j]-z[i])*(z[j]-z[i]));
	A_1[1] = (x[j]-x[i])*(y[j]-y[i]);
	A_1[2] = (x[j]-x[i])*(z[j]-z[i]);

	A_1[3*(1)] = A_1[1];
	A_1[3*(1)+1] = -((x[j]-x[i])*(x[j]-x[i]) + (z[j]-z[i])*(z[j]-z[i]));
	A_1[3*(1)+2] = (y[j]-y[i])*(z[j]-z[i]);

	A_1[3*(2)] = A_1[2];
	A_1[3*(2)+1] = A_1[3*(1)+2];
	A_1[3*(2)+2] = -((x[j]-x[i])*(x[j]-x[i]) + (y[j]-y[i])*(y[j]-y[i]));

	A_2[0] = -2*(x[j]-x[i])*(x[j]-x[i]) + (y[j]-y[i])*(y[j]-y[i]) + (z[j]-z[i])*(z[j]-z[i]);
	A_2[1] = -3*(x[j]-x[i])*(y[j]-y[i]);
	A_2[2] = -3*(x[j]-x[i])*(z[j]-z[i]);

	A_2[3*(1)] = A_2[1];
	A_2[3*(1)+1] = (x[j]-x[i])*(x[j]-x[i]) - 2*(y[j]-y[i])*(y[j]-y[i]) + (z[j]-z[i])*(z[j]-z[i]);
	A_2[3*(1)+2] = -3*(y[j]-y[i])*(z[j]-z[i]);

	A_2[3*(2)] = A_2[2];
	A_2[3*(2)+1] = A_2[3*(1)+2];
	A_2[3*(2)+2] = (x[j]-x[i])*(x[j]-x[i]) + (y[j]-y[i])*(y[j]-y[i]) - 2*(z[j]-z[i])*(z[j]-z[i]);


	//for(int j1 = 0, jj = 0; jj < 6; j1++, jj+=2){
	for(int j1 = 0, jj = 0; jj < 3; j1++, jj++){
	for(int i1 = 0, ii = 0; ii < 6; i1++, ii+=2){
	//A_re_temp = 1.0 / rjk_3 * (k[3]*k[3] * A_1[size_A*(j3+j1)+(i3+i1)] + A_2[size_A*(j3+j1)+(i3+i1)]/rjk_2);
	//A_im_temp = -k[3] / rjk_4 * A_2[size_A*(j3+j1)+(i3+i1)];
	A_re_temp = 1.0 / rjk_3 * (k[3]*k[3] * A_1[3*(j1)+(i1)] + A_2[3*(j1)+(i1)]/rjk_2);
	A_im_temp = -k[3] / rjk_4 * A_2[3*(j1)+(i1)];

	A[size_Adbl*(2*j3+jj)+(2*i3+ii)] += A_re_temp * cos_kr_kR - A_im_temp * sin_kr_kR;
	A[size_Adbl*(2*j3+jj)+(2*i3+ii+1)] += - A_re_temp * sin_kr_kR - A_im_temp * cos_kr_kR;
	//A[size_Adbl*(2*j3+jj+1)+(2*i3+ii)] += A_re_temp * sin_kr_kR + A_im_temp * cos_kr_kR;
	//A[size_Adbl*(2*j3+jj+1)+(2*i3+ii+1)] += A_re_temp * cos_kr_kR - A_im_temp * sin_kr_kR;
	}
	}
	}
	}
	}
	//*/

	delete [] A_1;
	delete [] A_2;

	return A;
}

int OpenMPCGMethod(int syst, double* A, double* x, double* b)
{
	omp_set_num_threads(8);
	//	A * x = b;
	
	const int IterationCount = 10;
	const double eps = 1.0e-5;

	//	testing input data
	if (syst < 1)
	{
		std::cout << "***** error in CGMethod: size of matrix < 1 *****" << "\n";

		return 1;
	}

	double sum_b = 0.0;

	for (int i = 0; i < syst; i++)
	{
		sum_b += *(b + i);
	}

	if (!sum_b)
	{
		std::cout << "***** error in CGMethod: sum b equal null (Ax=b) *****" << "\n";

		return 1;
	}
	//	test end

	double* r = new double [syst];
	double* p = new double [syst];
	double* Ax = new double [syst];
	double* Atr = new double [syst];
	double* Ap = new double [syst];
	double* Atr_1 = new double [syst];

	double sum_r = 0;
	/////////////////////////////////////////////////////////////////////////////begin "for"

	for (int cycle = 0; cycle < IterationCount; cycle++)
	{
		//		num = 0;
		int num;
		int systd2 = syst / 2;

#pragma omp parallel for shared(Ax, A, x, r, b, systd2, syst) private(num)
		for (int i = 0; i < syst; i += 2)
		{
			Ax[i] = Ax[i + 1] = 0.0;
			num = systd2 * i;

			for (int j = 0; j < syst; j += 2)
			{
				//				a = A[num];
				//				c = A[num+1];
				Ax[i] += A[num] * x[j] + A[num + 1] * x[j + 1];
				Ax[i + 1] += A[num] * x[j + 1] - A[num + 1] * x[j];
				num += 2;
			}

			r[i] = b[i] - Ax[i];
			r[i + 1] = b[i + 1] - Ax[i + 1];
		}

		//		num = 0;
#pragma omp parallel for shared(Atr, A, x, r, p, systd2, syst) private(num)
		for (int i = 0; i < syst; i += 2)
		{
			Atr[i] = Atr[i + 1] = 0.0;
			num = systd2 * i;

			for (int j = 0; j < syst; j += 2)
			{
				//				a = A[num];
				//				c = A[num+1];
				Atr[i] += A[num] * r[j] - A[num + 1] * r[j + 1];
				Atr[i + 1] += A[num + 1] * r[j] + A[num] * r[j + 1];
				num += 2;
			}

			p[i] = Atr[i];
			p[i + 1] = Atr[i + 1];
		}

		double AtrAtr = 0.0;
		//		Почався один цикл наближеного розв'язку
		for (int iteration = 0; iteration < syst; iteration++)
		{
			/////////// Перевірка на правельність результату
			sum_r = 0.0;

			for (int i = 0; i < syst; i++)
			{
				sum_r += r[i] * r[i];
			}

			sum_r /= (double)syst;
			//			r_k < eps;
			if (sum_r < eps)
			{
				std::cout << "CGMethod returned polarization" << "\n";

				delete [] r;
				delete [] p;
				delete [] Ax;
				delete [] Atr;
				delete [] Ap;
				delete [] Atr_1;

				return 0;
			}
			/////////// Кінеці перевірки
			//			Ap = A * p_k;
			double ApAp = 0.0;
			//			num = 0;
#pragma omp parallel for shared(Ap, A, p, systd2, syst) private(num) reduction(+: ApAp)
			for (int i = 0; i < syst; i += 2)
			{
				Ap[i] = Ap[i + 1] = 0.0;
				num = systd2 * i;

				for (int j = 0; j < syst; j += 2)
				{
					//					a = A[num];
					//					c = A[num+1];
					Ap[i] += A[num] * p[j] + A[num + 1] * p[j + 1];
					Ap[i + 1] += A[num] * p[j + 1] - A[num + 1] * p[j];
					num += 2;
				}

				ApAp += Ap[i] * Ap[i] + Ap[i + 1] * Ap[i + 1];
			}

			//			alpha_k = (Atr_k * Atr_k) / (Ap_k * Ap_k);
			//			При першій ітерації виконується (далі йде переприсвоєння AtrAtr = Atr1Atr1;):
			if (!iteration)
			{
				for (int j = 0; j < syst; j++)
				{
					AtrAtr += Atr[j] * Atr[j];
				}
			}

			//	перевірка ділення на нуль >>>
			if (!(ApAp * AtrAtr))
			{
				std::cout << "***** error in CGMethod: ApAp or AtrAtr = 0.0 *****" << "\n";

				delete [] r;
				delete [] p;
				delete [] Ax;
				delete [] Atr;
				delete [] Ap;
				delete [] Atr_1;

				return 1;
			}
			//	<<<
			double alpha = AtrAtr / ApAp;

			//			x_k+1 = x_k + alpha_k * p_k;
			//			r_k+1 = r_k - alpha_k * Ap_k
			for (int i = 0; i < syst; i++)
			{
				x[i] += alpha * p[i];
				r[i] -= alpha * Ap[i];
			}

			//			beta = (Atr_k+1 * Atr_k+1) / (Atr_k * Atr_k);
			double Atr1Atr1 = 0.0;
			//			num = 0;
#pragma omp parallel for shared(Atr_1, A, r, systd2, syst) private(num) reduction(+: Atr1Atr1)
			for (int i = 0; i < syst; i += 2)
			{
				Atr_1[i] = Atr_1[i + 1] = 0.0;
				num = systd2 * i;

				for (int j = 0; j < syst; j += 2)
				{
					//					a = A[num];
					//					c = A[num+1];
					Atr_1[i] += A[num] * r[j] - A[num + 1] * r[j + 1];
					Atr_1[i + 1] += A[num + 1] * r[j] + A[num] * r[j + 1];
					num += 2;
				}

				Atr1Atr1 += Atr_1[i] * Atr_1[i] + Atr_1[i + 1] * Atr_1[i + 1];
			}

			double beta = Atr1Atr1 / AtrAtr;

			//			p_k+1 = Atr_k+1 + beta_k * p_k;
			for (int i = 0; i < syst; i++)
			{
				p[i] = Atr_1[i] + beta * p[i];
			}

			AtrAtr = Atr1Atr1;
		}
		//		Закінчився один цикл наближеного розв'язку
	}
	/////////////////////////////////////////////////////////////////////////////////end "for";

	delete [] r;
	delete [] p;
	delete [] Ax;
	delete [] Atr;
	delete [] Ap;
	delete [] Atr_1;

	std::cout << "***** warning in CGMethod: ending no good. error is " << (sum_r - eps) / eps * 100.0 << " % *****" << "\n";

	return 1;
}