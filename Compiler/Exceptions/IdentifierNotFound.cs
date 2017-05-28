using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler;

namespace Compiler.Exceptions
{
    class IdentifierNotFound : Exception
    {
        public IdentifierNotFound(string identifier)
        {
            DelevopmentEnvironment.Formtest.Log("The variable: " + identifier + " was not found.");
        }
    }
}
