using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Simulation.Medium.Models;
using Simulation.Models.Enums;
using Simulation.Models.Spectrum;

namespace Simulation.Medium.Medium
{
    /// <summary>
    /// The DrudeLorentz class.
    /// </summary>
    public class DrudeLorentz : Drude
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrudeLorentz"/> class.
        /// </summary>
        public DrudeLorentz()
        {
            //this.EpsilonInfinity = 1;
            //this.PlasmaTerm.StrengthFactor = 1;
            this.OscillatorTerms = new List<ResonanceTerm>();

            this.OscillatorTerms.Add(new ResonanceTerm
            {
                ResonanceFrequency = new SpectrumUnit(1.24e+15, SpectrumUnitType.CycleFrequency),
                CollisionFrequency = new SpectrumUnit(5.904e+15, SpectrumUnitType.CycleFrequency),
                StrengthFactor = 6.5e-2
            });

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(6.808e+15, SpectrumUnitType.CycleFrequency),
            //    CollisionFrequency = new SpectrumUnit(6.867e+14, SpectrumUnitType.CycleFrequency),
            //    StrengthFactor = 1.24e-1
            //});

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(1.244e+16, SpectrumUnitType.CycleFrequency),
            //    CollisionFrequency = new SpectrumUnit(9.875e+13, SpectrumUnitType.CycleFrequency),
            //    StrengthFactor = 1.1e-2
            //});

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(1.38e+16, SpectrumUnitType.CycleFrequency),
            //    CollisionFrequency = new SpectrumUnit(1.392e+15, SpectrumUnitType.CycleFrequency),
            //    StrengthFactor = 8.4e-1
            //});

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(3.083e+16, SpectrumUnitType.CycleFrequency),
            //    CollisionFrequency = new SpectrumUnit(3.675e+15, SpectrumUnitType.CycleFrequency),
            //    StrengthFactor = 5.646 //todo
            //});

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(0.816, SpectrumUnitType.EVEnergy),
            //    CollisionFrequency = new SpectrumUnit(3.886, SpectrumUnitType.EVEnergy),
            //    StrengthFactor = 0.065
            //});

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(4.481, SpectrumUnitType.EVEnergy),
            //    CollisionFrequency = new SpectrumUnit(0.452, SpectrumUnitType.EVEnergy),
            //    StrengthFactor = 0.124
            //});

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(8.185, SpectrumUnitType.EVEnergy),
            //    CollisionFrequency = new SpectrumUnit(0.065, SpectrumUnitType.EVEnergy),
            //    StrengthFactor = 0.011
            //});

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(9.083, SpectrumUnitType.EVEnergy),
            //    CollisionFrequency = new SpectrumUnit(0.916, SpectrumUnitType.EVEnergy),
            //    StrengthFactor = 0.84
            //});

            //this.OscillatorTerms.Add(new ResonanceTerm
            //{
            //    ResonanceFrequency = new SpectrumUnit(20.29, SpectrumUnitType.EVEnergy),
            //    CollisionFrequency = new SpectrumUnit(2.419, SpectrumUnitType.EVEnergy),
            //    StrengthFactor = 5.646 //todo
            //});
        }

        /// <summary>
        /// Gets or sets the oscillator terms.
        /// </summary>
        /// <value>
        /// The oscillator terms.
        /// </value>
        public List<ResonanceTerm> OscillatorTerms { get; set; }

        /// <summary>
        /// Gets the permittivity at specific frequency.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <returns>
        /// The complex permittivity.
        /// </returns>
        public override Complex GetPermittivity(SpectrumUnit frequency)
        {
            double w = frequency.ToType(SpectrumUnitType.CycleFrequency);

            var drudeEps = base.GetPermittivity(frequency);
            var plasmafreq = this.PlasmaTerm.ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);
            return this.OscillatorTerms.Aggregate(
                drudeEps,
                (eps, oscillator) =>
                {
                    var resonanceFrequency = oscillator.ResonanceFrequency.ToType(SpectrumUnitType.CycleFrequency);
                    var collisionFrequency = oscillator.CollisionFrequency.ToType(SpectrumUnitType.CycleFrequency); 
                    var lorentzEps = oscillator.StrengthFactor * plasmafreq * plasmafreq /
                                      (resonanceFrequency * resonanceFrequency - w * w -
                                       Complex.ImaginaryOne * collisionFrequency * w);

                    eps += lorentzEps;
                    return eps;
                });

        }
    }
}
