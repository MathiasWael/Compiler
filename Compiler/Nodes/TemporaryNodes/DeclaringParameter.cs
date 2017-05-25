using System.Collections.Generic;
namespace Compiler.Nodes
{
    public class DeclaringParameter
    {
        public static List<Declaration> Parameters = new List<Declaration>();
        //<DeclaringParameter> ::= ',' <Declaration> <DeclaringParameter>
        public DeclaringParameter(Declaration declaration)
        {
            Parameters.Add(declaration);
        }
    }
}
