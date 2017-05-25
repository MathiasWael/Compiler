namespace Compiler
{
    interface IVisitable
    {
        object Accept(IVisitor visitor);
    }
}
