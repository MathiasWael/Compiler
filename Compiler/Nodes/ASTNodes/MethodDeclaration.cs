using System.Collections.Generic;

namespace Compiler.Nodes
{
    public class MethodDeclaration : Declarations
    {
        public Commands Commands { get; private set; }
        public List<Declaration> Parameters { get; private set; }
        public string MethodType { get; private set; }
        public string[] MethodIdentifier { get; private set; }
        public string[] ReturnValue { get; private set; }
        public Expression ReturnExpression { get; private set; }

        public MethodDeclaration(string v1, string v2, Commands commands, ReturnStatement returnStatement)
        {
            MethodType = v1;
            MethodIdentifier = new []{"Identifier", v2};
            Parameters = new List<Declaration>(DeclaringParameter.Parameters);
            Commands = commands;
            ReturnValue = returnStatement?.Value;
            ReturnExpression = returnStatement?.Expression;
            DeclaringParameter.Parameters.Clear();
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
