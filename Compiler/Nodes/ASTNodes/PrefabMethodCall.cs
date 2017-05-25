﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Nodes
{
    public class PrefabMethodCall : Statement
    {
        public string PrefabMethod;
        public List<string[]> CallingParameters;
        public PrefabMethodCall(string v)
        {
            PrefabMethod = v;
            CallingParameters = new List<string[]>(CallingParameter.Parameters);
            CallingParameter.Parameters.Clear();
        }
        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
