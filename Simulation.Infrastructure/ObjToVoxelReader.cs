using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ObjLoader.Loader.Loaders;

using Simulation.Infrastructure.Models;
using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

namespace Simulation.Infrastructure
{
    public class ObjToVoxelReader
    {
        public static MeshInfo ReadInfo(string fileName)
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create(new MaterialNullStreamProvider());
            var fileStream = new FileStream(fileName, FileMode.Open);
            LoadResult result = objLoader.Load(fileStream);
            var listVoxels = new List<CartesianCoordinate>();
            double resolution = 0;
            foreach (var group in result.Groups)
            {
                var faces = group.Faces.Where(x => x.All(y => y.NormalIndex == 1));

                foreach (var face in faces)
                {

                    var x = face.Min(i => result.Vertices[i.VertexIndex].X);
                    var y = face.Min(i => result.Vertices[i.VertexIndex].Y);
                    var z = face.Min(i => result.Vertices[i.VertexIndex].Z);

                    listVoxels.Add(new CartesianCoordinate(x, y, z));

                    if (resolution <= double.Epsilon)
                    {
                        var sibling1 = result.Vertices[face[0].VertexIndex];
                        var sibling2 = result.Vertices[face[1].VertexIndex];
                        
                        resolution = Math.Max(Math.Max(Math.Abs(sibling1.X-sibling2.X), Math.Abs(sibling1.Y - sibling2.Y)), Math.Abs(sibling1.Z - sibling2.Z));
                    }
                }
            }

            var min = new CartesianCoordinate(listVoxels.Min(x=>x.X), listVoxels.Min(x => x.Y), listVoxels.Min(x => x.Z));

            var transform = listVoxels.Select(x => new Voxel((x - min) / resolution)).ToList();

            return new MeshInfo() {Voxels = transform};
        }
    }
}
