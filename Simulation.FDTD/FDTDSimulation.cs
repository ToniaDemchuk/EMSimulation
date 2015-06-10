using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Simulation.Models;
using Simulation.Models.ConfigurationParameters;
using Simulation.Models.Extensions;

namespace Simulation.FDTD
{
    public class FDTDSimulation
    {
        FDTDField fields;
        FDTDPulse pulse;

        IMedium[,,] medium;

        void _calcFields()
        {
            int IE = 50;
            int JE = 50;
            int KE = 50;
            var pmlLength = 7;
            //int IE = fields->numCells.x;
            //int JE = fields->numCells.y;
            //int KE = fields->numCells.z;

            // границі розсіяного поля.
            int iaPulse = pmlLength + 1;
            int ibPulse = IE - pmlLength - 2;
            int jaPulse = pmlLength + 1;
            int jbPulse = JE - pmlLength - 2;
            int kaPulse = pmlLength + 1;
            int kbPulse = KE - pmlLength - 2;

            // Calculate the Dx field
            for (int i = 1; i < IE; i++)
            {
                for (int j = 1; j < JE; j++)
                {
                    for (int k = 1; k < KE; k++)
                    {
                        var rot_h = fields.H.Curl(i, j, k, -1);
                        fields.ID[i, j, k]= fields.ID[i, j, k] + rot_h;

                        var g3 = new CartesianCoordinate(
                            fields.Gj3[j] * fields.Gk3[k],
                            fields.Gi3[i] * fields.Gk3[k],
                            fields.Gi3[i] * fields.Gj3[j]);

                        var g2 = new CartesianCoordinate(
                            fields.Gj2[j] * fields.Gk2[k],
                            fields.Gi2[i] * fields.Gk2[k],
                            fields.Gi2[i] * fields.Gj2[j]);

                        var g1 = new CartesianCoordinate(
                            fields.Gi1[i],
                            fields.Gj1[j],
                            fields.Gk1[k]);

                        fields.D[i, j, k] = fields.D[i, j, k].ComponentProduct(g3) +
                                            Fundamentals.CourantConst * (rot_h + g1.ComponentProduct(fields.ID[i, j, k]))
                                                .ComponentProduct(g2);

                    }
                }
            }
            
            pulse->ElectricFieldStepCalc(time);

            // Incident Dy
            for (int i = iaPulse; i <= ibPulse; i++)
            {
                for (int j = jaPulse; j <= jbPulse - 1; j++)
                {
                    fields->dy(i, j, kaPulse) = fields->dy(i, j, kaPulse) -
                            Fundamentals.CourantConst * pulse->h(j);
                    fields->dy(i, j, kbPulse + 1) = fields->dy(i, j, kbPulse + 1) +
                            Fundamentals.CourantConst * pulse->h(j);
                }
            }
            // Incident Dz
            for (int i = iaPulse; i <= ibPulse; i++)
            {
                for (int k = kaPulse; k <= kbPulse; k++)
                {
                    fields->dz(i, jaPulse, k) = fields->dz(i, jaPulse, k) +
                            Fundamentals.CourantConst * pulse->h(jaPulse - 1);
                    fields->dz(i, jbPulse, k) = fields->dz(i, jbPulse, k) -
                            Fundamentals.CourantConst * pulse->h(jbPulse);
                }
            }

            // Calculate the E from D field

            for (int i = 1; i < IE - 1; i++)
            {
                for (int j = 1; j < JE - 1; j++)
                {
                    for (int k = 1; k < KE - 1; k++)
                    {

                        fields.E[i, j, k] = medium[i, j, k].Solve(fields.D[i, j, k]);

                    }
                }
            }
            double timeStep = cellSize * Fundamentals.CourantConst / (Fundamentals.LightVelocity);
            fields->DoFourierField(time * timeStep, &(medium->mediumPhase));
            pulse->DoFourierPulse(time * timeStep);


            // Calculate the Hx field
            for (int i = 0; i < IE - 1; i++)
            {
                for (int j = 0; j < JE - 1; j++)
                {
                    for (int k = 0; k < KE - 1; k++)
                    {

                        var rot_e = fields.E.Curl(i, j, k, +1);
                        fields.IH[i, j, k] = fields.IH[i, j, k] + rot_e;


                        var f3 = new CartesianCoordinate(
                            fields.Fj3[j] * fields.Fk3[k],
                            fields.Fi3[i] * fields.Fk3[k],
                            fields.Fi3[i] * fields.Fj3[j]);

                        var f2 = new CartesianCoordinate(
                            fields.Fj2[j] * fields.Fk2[k],
                            fields.Fi2[i] * fields.Fk2[k],
                            fields.Fi2[i] * fields.Fj2[j]);

                        var f1 = new CartesianCoordinate(
                            fields.Fi1[i],
                            fields.Fj1[j],
                            fields.Fk1[k]);

                        fields.H[i, j, k] = fields.H[i, j, k].ComponentProduct(f3)
                                 + Fundamentals.CourantConst *
                                (rot_e + fields.IH[i, j, k].ComponentProduct(f1)).ComponentProduct(f2);
                    }
                }
            }
            
            pulse->MagneticFieldStepCalc();

            // Incident Hx
            for (int i = iaPulse; i <= ibPulse; i++)
            {
                for (int k = kaPulse; k <= kbPulse; k++)
                {
                    fields->hx(i, jaPulse - 1, k) = fields->hx(i, jaPulse - 1, k) +
                            Fundamentals.CourantConst * pulse->e(jaPulse);
                    fields->hx(i, jbPulse, k) = fields->hx(i, jbPulse, k) -
                            Fundamentals.CourantConst * pulse->e(jbPulse);
                }
            }
            // Incident Hy
            for (int j = jaPulse; j <= jbPulse; j++)
            {
                for (int k = kaPulse; k <= kbPulse; k++)
                {
                    fields->hy(iaPulse - 1, j, k) = fields->hy(iaPulse - 1, j, k) -
                            Fundamentals.CourantConst * pulse->e(j);
                    fields->hy(ibPulse, j, k) = fields->hy(ibPulse, j, k) +
                            Fundamentals.CourantConst * pulse->e(j);
                }
            }



            time++;
        }

    }
}
