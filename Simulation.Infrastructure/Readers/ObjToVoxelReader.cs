using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ObjLoader.Loader.Loaders;

using Simulation.Infrastructure.Models;
using Simulation.Models.Coordinates;

namespace Simulation.Infrastructure.Readers
{
    public class ObjToVoxelReader:IVoxelReader
    {
        private readonly ObjLoaderFactory loaderFactory;

        private readonly MaterialNullStreamProvider materialStreamProvider;

        public ObjToVoxelReader()
        {
            this.loaderFactory = new ObjLoaderFactory();
            this.materialStreamProvider = new MaterialNullStreamProvider();
        }

        public MeshInfo ReadInfo(string fileName)
        {
            var objLoader = this.loaderFactory.Create(this.materialStreamProvider);
            LoadResult result;
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                result = objLoader.Load(fileStream);
            }
            
            List<Voxel> transform = transformVerticesToVoxels(result);

            var mesh = new MeshInfo() { Voxels = transform };
            return mesh;
        }

        private static List<Voxel> transformVerticesToVoxels(LoadResult result)
        {
            var listVoxels = new List<CartesianCoordinate>();
            double resolution = 0;
            foreach (var group in result.Groups)
            {
                var faces = @group.Faces.Where(x => x.All(y => y.NormalIndex == 1));

                foreach (var face in faces)
                {
                    var x = face.Min(i => result.Vertices[i.VertexIndex - 1].X);
                    var y = face.Min(i => result.Vertices[i.VertexIndex - 1].Y);
                    var z = face.Min(i => result.Vertices[i.VertexIndex - 1].Z);

                    listVoxels.Add(new CartesianCoordinate(x, y, z));

                    if (resolution <= double.Epsilon)
                    {
                        var sibling1 = result.Vertices[face[0].VertexIndex - 1];
                        var sibling2 = result.Vertices[face[1].VertexIndex - 1];

                        resolution = Math.Max(
                            Math.Max(Math.Abs(sibling1.X - sibling2.X), Math.Abs(sibling1.Y - sibling2.Y)),
                            Math.Abs(sibling1.Z - sibling2.Z));
                    }
                }
            }

            var min = new CartesianCoordinate(listVoxels.Min(x => x.X), listVoxels.Min(x => x.Y), listVoxels.Min(x => x.Z));

            var transform = listVoxels.Select(x => new Voxel((x - min) / resolution)).ToList();
            return transform;
        }
    }
}
