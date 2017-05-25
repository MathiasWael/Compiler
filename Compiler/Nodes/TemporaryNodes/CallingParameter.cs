using System.Collections.Generic;
namespace Compiler.Nodes
{
    public class CallingParameter
    {
        public static List<string[]> Parameters = new List<string[]>();
        public CallingParameter(string[] v)
        {
            Parameters.Add(v);
        }
    }
}
