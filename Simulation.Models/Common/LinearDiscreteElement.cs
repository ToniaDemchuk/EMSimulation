using System.Xml.Serialization;
using Simulation.Models;

namespace Simulation.DDA.Console
{
    public class LinearDiscreteElement
    {
        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [XmlAttribute("lower")]
        public double Lower { get; set; }

        /// <summary>
        /// Gets the polar angle.
        /// </summary>
        /// <value>
        /// The polar angle.
        /// </value>
        [XmlAttribute("upper")]
        public double Upper { get; set; }

        /// <summary>
        /// Gets the theta.
        /// </summary>
        /// <value>
        /// The theta.
        /// </value>
        [XmlAttribute("count")]
        public int Count { get; set; }
    }
}
