using System.Xml.Serialization;

namespace Simulation.Models.Common
{
    /// <summary>
    /// The LinearDiscreteElement class.
    /// </summary>
    public class LinearDiscreteElement
    {
        /// <summary>
        /// Gets or sets the lower boundary.
        /// </summary>
        /// <value>
        /// The lower boundary.
        /// </value>
        [XmlAttribute("lower")]
        public double Lower { get; set; }

        /// <summary>
        /// Gets or sets the upper boundary.
        /// </summary>
        /// <value>
        /// The upper boundary.
        /// </value>
        [XmlAttribute("upper")]
        public double Upper { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        [XmlAttribute("count")]
        public int Count { get; set; }
    }
}
