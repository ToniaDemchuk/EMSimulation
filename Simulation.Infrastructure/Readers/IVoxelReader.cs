using Simulation.Infrastructure.Models;

namespace Simulation.Infrastructure.Readers
{
    /// <summary>
    /// The voxel reader
    /// </summary>
    public interface IVoxelReader
    {
        /// <summary>
        /// Reads the information.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The mesh info.</returns>
        MeshInfo ReadInfo(string fileName);
    }
}
