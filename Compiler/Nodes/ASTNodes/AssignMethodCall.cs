using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class AssignMethodCall : Statement
    {
        public string[] Identifier1;
        public string[] Identifier2;
        public List<string[]> Parameters = new List<string[]>();
        public AssignMethodCall(string[] v1, string[] v2)
        {
            Identifier1 = v1;
            Identifier2 = v2;
            Parameters = new List<string[]>(CallingParameter.Parameters);
            CallingParameter.Parameters.Clear();
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
