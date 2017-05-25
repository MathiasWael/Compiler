using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class ElseIfStatement : IfStatementExtend
    {
        public BooleanExpression BooleanExpression;
        public Commands Commands1;
        public IfStatementExtend IfStatementExtend;

        public ElseIfStatement(BooleanExpression booleanExpression, Commands commands, IfStatementExtend ifStatementExtend)
        {
            BooleanExpression = booleanExpression;
            Commands1 = commands;
            IfStatementExtend = ifStatementExtend;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
