using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class MethodCall : Statement
    {
        public string[] Identifier;
        public List<string[]> Parameters;
        public MethodCall(string[] v1)
        {
            Identifier = v1;
            Parameters =  new List<string[]>(CallingParameter.Parameters);
            CallingParameter.Parameters.Clear();
        }
        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
