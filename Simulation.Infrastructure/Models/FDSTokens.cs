namespace Simulation.Infrastructure.Models
{
    public class FDSTokens
    {
        public static readonly string StartSeparator = "&";
        public static readonly string EndSeparator = "/";
        public static readonly string CommentSeparator = "!";

        public static readonly string Obstruction = "OBST";
        public static readonly string Mesh = "MESH";
        public static readonly string Box = "XB";
        public static readonly string SurfaceId = "SURF_ID";
        public static readonly string Resolution = "IJK";
        public static readonly string CellSize = "RES";
            
        public static bool IsStartSeparator(string str)
        {
            return str.StartsWith(StartSeparator);
        }

        public static bool IsEndSeparator(string str)
        {
            return str.StartsWith(EndSeparator);
        }

        public static bool IsCommentSeparator(string str)
        {
            return str.StartsWith(CommentSeparator);
        }
    }
}