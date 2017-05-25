using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class Assignment : Statement
    {
        public string[] Identifier;
        public string[] Value;
        public Expression Expression;
        public Assignment(string[] v1, string[] v2, Expression expression)
        {
            Identifier = v1;
            Value = v2;
            Expression = expression;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
