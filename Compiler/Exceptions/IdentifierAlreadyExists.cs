using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions
{
    public class IdentifierAlreadyExists : Exception
    {
        public IdentifierAlreadyExists(string identifier)
        {
            DelevopmentEnvironment.Formtest.Log("The name: " + identifier + " is already taken.");
        }
    }
}
