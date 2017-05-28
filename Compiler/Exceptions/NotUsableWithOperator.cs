using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions
{
    public class NotUsableWithOperator : Exception
    {
        public NotUsableWithOperator(string[] identifier)
        {
            string temp = "";
            foreach (string s in identifier)
            {
                temp += s;
            }
            DelevopmentEnvironment.Formtest.Log("The variable: " + temp + " cannot be used together with operators");
        }
    }
}
