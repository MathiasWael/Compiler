using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Exceptions
{
    public class ParameterDifference : Exception
    {
        public ParameterDifference(string[] identifier)
        {
            string temp = "";
            foreach (string s in identifier)
            {
                temp += s;
            }
            DelevopmentEnvironment.Formtest.Log("The method: " + temp + " has different parameters than its calls.");
        }
        public ParameterDifference(string identifier)
        {
            DelevopmentEnvironment.Formtest.Log("The method: " + identifier + " has different parameters than its calls.");
        }
    }
}
