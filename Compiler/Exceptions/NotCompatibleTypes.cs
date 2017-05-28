using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions
{
    public class NotCompatibleTypes : Exception
    {
        public NotCompatibleTypes(string[] identifier, string[] identifier2)
        {
            string temp1 = "";
            string temp2 = "";
            foreach (string s in identifier)
            {
                temp1 += s;
            }
            foreach (string s in identifier2)
            {
                temp2 += s;
            }
            DelevopmentEnvironment.Formtest.Log("The variables: " + temp1 + " and " + temp2 + " are not compatible.");
        }
    }
}
