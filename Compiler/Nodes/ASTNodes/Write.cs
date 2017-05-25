using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class Write : Statement
    {
        public List<string[]> WriteContext = new List<string[]>();

        public Write()
        {
            WriteContext = Text.TextContext;
            Text.TextContext.Clear();
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}