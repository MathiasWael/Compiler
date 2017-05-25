using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class ElseStatement : IfStatementExtend
    {
        public Commands Commands1;

        public ElseStatement(Commands commands)
        {
            Commands1 = commands;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}