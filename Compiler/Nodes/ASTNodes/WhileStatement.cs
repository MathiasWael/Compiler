using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class WhileStatement : Statement
    {
        public BooleanExpression BooleanExpression { get; private set; }
        public Commands Commands1 { get; private set; }
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
