using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class Assignment : Statement
    {
        public string[] Identifier { get; private set; }
        public string[] Value { get; private set; }
        public Expression Expression { get; private set; }
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
