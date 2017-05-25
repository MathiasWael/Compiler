using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class WhileStatement : Statement
    {
        public BooleanExpression BooleanExpression;
        public Commands Commands1;
        public WhileStatement(BooleanExpression booleanExpression, Commands commands)
        {
            BooleanExpression = booleanExpression;
            Commands1 = commands;
        }
        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
