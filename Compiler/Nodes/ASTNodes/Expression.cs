﻿namespace Compiler.Nodes
{
    public class Expression : ASTNode
    {
        public string Operator { get; private set; }
        public string[] Value { get; private set; }
        public Expression Expression1 { get; private set; }
        public Expression(string v1, string[] v2, Expression expression)
        {
            Operator = v1;
            Value = v2;
            Expression1 = expression;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}