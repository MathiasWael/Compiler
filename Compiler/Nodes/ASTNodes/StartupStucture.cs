namespace Compiler.Nodes
{
    public class StartupStucture : ASTNode
    {
        public Declarations Declarations { get; private set; }
        public Commands Commands { get; private set; }
        public Declarations Declarations2 { get; private set; }
        public Commands Commands2 { get; private set; }
        public Declarations Declarations3 { get; private set; }

        public StartupStucture(Declarations declarations, Commands commands, Declarations declarations2, Commands commands2, Declarations declarations3)
        {
            Declarations = declarations;
            Commands = commands;
            Declarations2 = declarations2;
            Commands2 = commands2;
            Declarations3 = declarations3;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}