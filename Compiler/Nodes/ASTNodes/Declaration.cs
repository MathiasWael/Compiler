namespace Compiler.Nodes
{
    public class Declaration : Declarations
    {
        public string Type { get; private set; }
        public string Identifier { get; private set; }

        public Declaration(string v1, string v2)
        {
            Type = v1;
            Identifier = v2;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
