namespace Simulation.DDA
{
    /// <summary>
    /// The ModernKDDAEntryPoint class.
    /// </summary>
    public class ModernKDDAEntryPoint
    {
        /// <summary>
        /// The OpenMP conjugate gradient method.
        /// </summary>
        /// <param name="systLength">The system length.</param>
        /// <param name="a">The matrix of coefficients of system.</param>
        /// <param name="x">The unknown vector.</param>
        /// <param name="b">The constant terms.</param>
        /// <returns>if 0 then method is success.</returns>
        public static int OpenMPCGMethod(int syst, double[] A, double[] x, double[] b) {
            //    A * x = b;

            const int IterationCount = 10;
            const double eps = 1.0e-5;

            //    testing input data
            if (syst < 1)
            {
                return 1;
            }

            double sum_b = 0.0;

            for (int i = 0; i < syst; i++)
            {
                sum_b += b[i];
            }

            if (sum_b==0)
            {

                return 1;
            }
            //    test end

            double[] r = new double[syst];
            double[] p = new double[syst];
            double[] Ax = new double[syst];
            double[] Atr = new double[syst];
            double[] Ap = new double[syst];
            double[] Atr_1 = new double[syst];

            double sum_r = 0;
            /////////////////////////////////////////////////////////////////////////////begin "for"

            for (int cycle = 0; cycle < IterationCount; cycle++)
            {
                //        num = 0;
                int systd2 = syst / 2;

                for (int i = 0; i < syst; i += 2)
                {
                    Ax[i] = Ax[i + 1] = 0.0;
                    var num = systd2 * i;

                    for (int j = 0; j < syst; j += 2)
                    {
                        //                a = A[num];
                        //                c = A[num+1];
                        Ax[i] += A[num] * x[j] + A[num + 1] * x[j + 1];
                        Ax[i + 1] += A[num] * x[j + 1] - A[num + 1] * x[j];
                        num += 2;
                    }

                    r[i] = b[i] - Ax[i];
                    r[i + 1] = b[i + 1] - Ax[i + 1];
                }

                //        num = 0;
                for (int i = 0; i < syst; i += 2)
                {
                    Atr[i] = Atr[i + 1] = 0.0;
                    var num = systd2 * i;

                    for (int j = 0; j < syst; j += 2)
                    {
                        //                a = A[num];
                        //                c = A[num+1];
                        Atr[i] += A[num] * r[j] - A[num + 1] * r[j + 1];
                        Atr[i + 1] += A[num + 1] * r[j] + A[num] * r[j + 1];
                        num += 2;
                    }

                    p[i] = Atr[i];
                    p[i + 1] = Atr[i + 1];
                }

                double AtrAtr = 0.0;
                //        Почався один цикл наближеного розв'язку
                for (int iteration = 0; iteration < syst; iteration++)
                {
                    /////////// Перевірка на правельність результату
                    sum_r = 0.0;

                    for (int i = 0; i < syst; i++)
                    {
                        sum_r += (r[i] * r[i]) / (double)syst;
                    }

                    //            r_k < eps;
                    if (sum_r < eps)
                    {
                        return 0;
                    }
                    /////////// Кінеці перевірки
                    //            Ap = A * p_k;
                    double ApAp = 0.0;
                    //            num = 0;
                    for (int i = 0; i < syst; i += 2)
                    {
                        Ap[i] = Ap[i + 1] = 0.0;
                        var num = systd2 * i;

                        for (int j = 0; j < syst; j += 2)
                        {
                            //                    a = A[num];
                            //                    c = A[num+1];
                            Ap[i] += A[num] * p[j] + A[num + 1] * p[j + 1];
                            Ap[i + 1] += A[num] * p[j + 1] - A[num + 1] * p[j];
                            num += 2;
                        }

                        ApAp += Ap[i] * Ap[i] + Ap[i + 1] * Ap[i + 1];
                    }

                    //            alpha_k = (Atr_k * Atr_k) / (Ap_k * Ap_k);
                    //            При першій ітерації виконується (далі йде переприсвоєння AtrAtr = Atr1Atr1;):
                    if (iteration==0)
                    {
                        for (int j = 0; j < syst; j++)
                        {
                            AtrAtr += Atr[j] * Atr[j];
                        }
                    }

                    //    перевірка ділення на нуль >>>
                    if ((ApAp * AtrAtr)==0)
                    {
                        return 1;
                    }
                    //    <<<
                    double alpha = AtrAtr / ApAp;

                    //            x_k+1 = x_k + alpha_k * p_k;
                    //            r_k+1 = r_k - alpha_k * Ap_k
                    for (int i = 0; i < syst; i++)
                    {
                        x[i] += alpha * p[i];
                        r[i] -= alpha * Ap[i];
                    }

                    //            beta = (Atr_k+1 * Atr_k+1) / (Atr_k * Atr_k);
                    double Atr1Atr1 = 0.0;
                    //            num = 0;
                    for (int i = 0; i < syst; i += 2)
                    {
                        Atr_1[i] = Atr_1[i + 1] = 0.0;
                        var num = systd2 * i;

                        for (int j = 0; j < syst; j += 2)
                        {
                            //                    a = A[num];
                            //                    c = A[num+1];
                            Atr_1[i] += A[num] * r[j] - A[num + 1] * r[j + 1];
                            Atr_1[i + 1] += A[num + 1] * r[j] + A[num] * r[j + 1];
                            num += 2;
                        }

                        Atr1Atr1 += Atr_1[i] * Atr_1[i] + Atr_1[i + 1] * Atr_1[i + 1];
                    }

                    double beta = Atr1Atr1 / AtrAtr;

                    //            p_k+1 = Atr_k+1 + beta_k * p_k;
                    for (int i = 0; i < syst; i++)
                    {
                        p[i] = Atr_1[i] + beta * p[i];
                    }

                    AtrAtr = Atr1Atr1;
                }
                //        Закінчився один цикл наближеного розв'язку
            }
            /////////////////////////////////////////////////////////////////////////////////end "for";


            return 1;
        }
    }
}