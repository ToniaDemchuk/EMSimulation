using System;
using System.IO;
using System.Linq;
using System.Text;

using Simulation.Infrastructure.Models;

namespace Simulation.Infrastructure
{
    public static class VoxelReader
    {
        public static MeshInfo ReadInfo(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                var header_size = reader.ReadInt32();
                var num_objects = reader.ReadInt32();
                var object_header_size = reader.ReadInt32();
                var voxel_struct_size = reader.ReadInt32();

                var num_voxels = reader.ReadInt32();
                var voxel_resolution = new int[]
                {
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32()
                };

                var voxel_size = new double[]
                {
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                };
                var model_scale_factor = reader.ReadSingle();
                var model_offset = new double[]
                {
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                };
                var zero_coordinate = new double[]
                {
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                };
                var has_texture = reader.ReadByte();
                string texture_filename = getTextureFileName(reader.ReadChars(260));

                var list =
                    Enumerable.Range(0, voxel_resolution.Aggregate(1, (x, y) => x * y));
                var s = list
                    .TakeWhile(x => reader.BaseStream.Position != reader.BaseStream.Length)
                    .Where(x => reader.ReadChar() == '2')
                    .Select(x=>new
                    {
                        coords = new int[]
                        {
                            reader.ReadInt16(),
                            reader.ReadInt16(),
                            reader.ReadInt16(),
                        },
                        has_texture = Convert.ToBoolean(reader.ReadByte()),
                        textUV = new double[]
                        {
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                        },
                        has_normal = Convert.ToBoolean(reader.ReadByte()),
                        normalarr = new double[]
                        {
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                        },
                        has_distance = Convert.ToBoolean(reader.ReadByte()),
                        distance_to_surface = reader.ReadSingle(),
                        distance_gradient = new double[]
                        {
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                        },
                        num_modifiers = reader.ReadByte(),
                        distance_to_modifier = new double[]
                        {
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                        },
                        modifier_gradient = new double[]
                        {
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                            reader.ReadSingle(),
                        },
                        is_on_border = Convert.ToBoolean(reader.ReadByte())
                    }).ToList();
                return new MeshInfo()
                {
                    Voxels = s.Select(
                        x => new Voxel
                        {
                            I = x.coords[0],
                            J = x.coords[1],
                            K = x.coords[2]
                        }).ToList(),
                    Resolution = new Voxel()
                    {
                        I = voxel_resolution[0],
                        J = voxel_resolution[1],
                        K = voxel_resolution[2]
                    }
                };
            }
        }

        private static string getTextureFileName(char[] chars)
        {
            var texture_filename = new string(chars);

            var ind = texture_filename.IndexOf('\0');
            if (ind > -1)
            {
                texture_filename = texture_filename.Remove(ind);
            }
            return texture_filename;
        }
    }
}
