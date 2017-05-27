namespace Compiler.Nodes
{
    public class BooleanExpression : ASTNode
    {
        public string[] Value1 { get; private set; }
        public Expression Expression1 { get; private set; }
        public string ComparisonOperator1 { get; private set; }
        public string[] Value2 { get; private set; }
        public Expression Expression2 { get; private set; }
        public string LogicalOperator { get; private set; }
        public BooleanExpression BooleanExtension { get; private set; }

        public BooleanExpression(string[] v1, Expression expression1, string v2, string[] v3, Expression expression2)
        {
            Value1 = v1;
            Expression1 = expression1;
            ComparisonOperator1 = v2;
            Value2 = v3;
            Expression2 = expression2;
        }

        public BooleanExpression(string[] v1, Expression expression1, string v2, string[] v3, Expression expression2, string v4, string[] v5, Expression expression3, string v6, string[] v7, Expression expression4)
        {
            Value1 = v1;
            Expression1 = expression1;
            ComparisonOperator1 = v2;
            Value2 = v3;
            Expression2 = expression2;
            LogicalOperator = v4;
            BooleanExtension = new BooleanExpression(v5, expression3, v6, v7, expression4);
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
