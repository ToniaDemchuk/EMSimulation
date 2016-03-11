using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Simulation.Infrastructure
{
    public static class VoxelReader
    {
        public static void ReadInfo(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                var header_size = reader.ReadInt32();
                var num_objects = reader.ReadInt32();
                var object_header_size = reader.ReadInt32();
                var voxel_struct_size = reader.ReadInt32();

                var num_voxels = reader.ReadInt32();
                var voxel_resolution = new double[]
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
                var converter = new UTF8Encoding();
                var texture_filename = new string(reader.ReadChars(260));

                var ind = texture_filename.IndexOf('\0');
                if (ind > -1)
                {
                    texture_filename = texture_filename.Remove(ind);
                }

                var list =
                    Enumerable.Range(0, (int) Math.Round(voxel_resolution.Aggregate(1.0, (x, y) => x * y)));
                var s = list
                    .TakeWhile(x => reader.BaseStream.Position != reader.BaseStream.Length)
                    .Where(x=> reader.ReadChar() == '2')
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

            }
        }
    }
}
