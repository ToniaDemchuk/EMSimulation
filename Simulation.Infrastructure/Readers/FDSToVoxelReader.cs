using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Simulation.Infrastructure.Models;
using Simulation.Models.Coordinates;

namespace Simulation.Infrastructure.Readers
{
    public class FDSToVoxelReader : IVoxelReader
    {
        public MeshInfo ReadInfo(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var instr = parseList(lines);

            //var meshObj = new ;
            //var box = getBox(meshObj);
            //var resol = getResolution(meshObj);

            return new MeshInfo()
            {

            };
        }

        public IEnumerable<IEnumerable<T>> GetByKey<T>(IEnumerable<IGrouping<T, IEnumerable<T>>> source, T key)
        {
            return source.FirstOrDefault(x => x.Key.Equals(key)).Select(x => x);
        }

        private IEnumerable<IGrouping<string, string[]>> parseList(string[] lines)
        {
            IEnumerable<string> list = lines.Where(x => !(string.IsNullOrWhiteSpace(x) || x.StartsWith("!"))).Select(x => x.TrimStart()).SkipWhile(x=>!x.StartsWith("&"));

            var a = SplitOn(lines, x => x.StartsWith("/")).Select(x=>x.ToArray()).GroupBy(x=>x[0].Substring(1,4));
            return a;
        }



        public IEnumerable<IEnumerable<T>> SplitOn<T>(IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.Aggregate(new List<List<T>> { new List<T>() },
                                    (list, value) =>
                                    {
                                        if (predicate(value))
                                        {
                                            list.Add(new List<T>());
                                        }
                                        else
                                        {
                                            list.Last().Add(value);
                                        }
                                        return list;
                                    });
        }

        private static Tuple<CartesianCoordinate, CartesianCoordinate> getBox(IEnumerable<string> meshObj)
        {
            var meshCoords =
                meshObj.Where(x => x.StartsWith("XB"))
                    .Select(x => x.Substring(0, 3).Split(',')).FirstOrDefault();

            if (meshCoords == null)
            {
                return null;
            }

            var y = meshCoords.Select(double.Parse).ToArray();
            if (y.Length != 6)
            {
                return null;
            }
            return Tuple.Create(
                new CartesianCoordinate(y[0], y[2], y[4]),
                new CartesianCoordinate(y[1], y[3], y[5]));
        }

        private static Voxel getResolution(IEnumerable<string> meshObj)
        {
            var meshCoords =
                meshObj.Where(x => x.StartsWith("IJK"))
                    .Select(x => x.Substring(0, 4).Split(',')).FirstOrDefault();

            if (meshCoords == null)
            {
                return null;
            }

            var y = meshCoords.Select(double.Parse).ToArray();
            if (y.Length != 3)
            {
                return null;
            }
            return new Voxel(new CartesianCoordinate(y[0], y[1], y[2]));
        }

    }
}
