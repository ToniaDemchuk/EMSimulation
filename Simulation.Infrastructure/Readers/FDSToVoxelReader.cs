using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Simulation.Infrastructure.Models;
using Simulation.Models.Coordinates;
using Simulation.Models.Extensions;

namespace Simulation.Infrastructure.Readers
{
    public class FDSToVoxelReader : IVoxelReader
    {
        public MeshInfo ReadInfo(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var instr = parseObjects(lines);

            var meshObj = instr[FDSTokens.Mesh].FirstOrDefault();
            var box = getBox(meshObj);
            var resol = getResolution(meshObj);
            var res = 0.1;

            List<Voxel> voxels = getVoxels(instr, box, res);

            return new MeshInfo()
            {
                Voxels = voxels,
                Resolution = resol
            };
        }

        private static List<Voxel> getVoxels(ILookup<string, IEnumerable<string>> instr, Tuple<CartesianCoordinate, CartesianCoordinate, string> meshBox, double resolution)
        {
            var voxels = new List<Voxel>();
            var obstructions = instr[FDSTokens.Obstruction];
            foreach (var obstruction in obstructions)
        {
                var obstBox = getBox(obstruction);
                var point1 = new Voxel((obstBox.Item1 - meshBox.Item1) / resolution);
                var point2 = new Voxel((obstBox.Item2 - meshBox.Item1) / resolution);

                for (int i = point1.I; i < point2.I; i++)
        {
                    for (int j = point1.J; j < point2.J; j++)
                                    {
                        for (int k = point1.K; k < point2.K; k++)
                                        {
                            var vox = new Voxel { I = i, J = j, K = k, Material = obstBox.Item3 };
                            voxels.Add(vox);
                                        }
                                        }
        }
            }
            return voxels;
        }

        private ILookup<string, IEnumerable<string>> parseObjects(IEnumerable<string> lines)
        {
            IEnumerable<string> list = lines
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Where(x => !FDSTokens.IsCommentSeparator(x))
                .Select(x => x.TrimStart());

            var objects = list.SplitOn(FDSTokens.IsStartSeparator, FDSTokens.IsEndSeparator);

            return objects.ToLookup(x => x.First().Substring(1, 4)); // e.g. &OBST 
        }

        private static Tuple<CartesianCoordinate, CartesianCoordinate, string> getBox(IEnumerable<string> meshObj)
        {
            var meshCoords = getParams(meshObj, FDSTokens.Box);
            if (meshCoords == null)
            {
                return null;
            }

            var coords = meshCoords.Select(double.Parse).ToArray();
            if (coords.Length != 6)
            {
                return null;
            }

            string materialStr = "";
            var material = getParams(meshObj, FDSTokens.SurfaceId);
            if (material != null && material.Length > 0)
            {
                materialStr = material[0].Trim('\'');
            }

            return Tuple.Create(
                new CartesianCoordinate(coords[0], coords[2], coords[4]),
                new CartesianCoordinate(coords[1], coords[3], coords[5]),
                materialStr);
        }

        private static Voxel getResolution(IEnumerable<string> meshObj)
        {
            var meshCoords = getParams(meshObj, FDSTokens.Resolution);
            if (meshCoords == null)
            {
                return null;
            }

            var coords = meshCoords.Select(double.Parse).ToArray();
            if (coords.Length != 3)
            {
                return null;
            }
            return new Voxel(new CartesianCoordinate(coords[0], coords[1], coords[2]));
        }

        private static string[] getParams(IEnumerable<string> meshObj, string paramName)
        {
            var param = paramName + "=";
            return meshObj.Where(x => x.StartsWith(param))
                .Select(x => x.Substring(param.Length).Split(','))
                .FirstOrDefault();
    }
    }
}
