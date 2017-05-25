namespace Compiler.Nodes
{
    public abstract class ASTNode : IVisitable
    {
        public abstract object Accept(IVisitor visitor);
    }
}
