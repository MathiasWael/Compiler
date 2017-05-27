using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class CommandsDeclaration : Commands
    {
        public string Type { get; private set; }
        public string Identifier { get; private set; }
        public CommandsDeclaration(Declaration declaration, Commands commands)
        {
            Type = declaration.Type;
            Identifier = declaration.Identifier;
            NextCommands = commands;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
