namespace Compiler.Nodes
{
    public class ReturnStatement
    {
        public string[] Value;
        public Expression Expression;
        public ReturnStatement(string[] v, Expression expression)
        {
            Value = v;
            Expression = expression;
        }
    }
}
