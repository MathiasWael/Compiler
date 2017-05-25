using System.Collections.Generic;
namespace Compiler.Nodes
{
    public class Text
    {
        public static List<string[]> TextContext = new List<string[]>();
        public Text(string v)
        {
            string[] temp = new string[] { "StringValue", v };
            TextContext.Add(temp);
        }
        public Text(string[] identifier)
        {
            TextContext.Add(identifier);
        }
    }
}
