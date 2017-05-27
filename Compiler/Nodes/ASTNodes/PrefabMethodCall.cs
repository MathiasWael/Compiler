using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class PrefabMethodCall : Statement
    {
        public string PrefabMethod { get; private set; }
        public List<string[]> CallingParameters { get; private set; }
        public PrefabMethodCall(string v)
        {
            PrefabMethod = v;
            CallingParameters = new List<string[]>(CallingParameter.Parameters);
            CallingParameter.Parameters.Clear();
        }
        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
