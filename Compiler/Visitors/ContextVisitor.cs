using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Exceptions;
using Compiler.Nodes;

namespace Compiler
{
    public class ContextVisitor : IVisitor
    {
        public static readonly SymbolTable _symbolTable = new SymbolTable();
        private readonly List<SymbolTable.Variable> _parameters = new List<SymbolTable.Variable>();
        private bool _parameterAdd;
        private bool _preVisit = true;

        public object Visit(StartupStucture obj)
        {
            obj.Declarations?.Accept(this);
            obj.Declarations2?.Accept(this);
            obj.Declarations3?.Accept(this);
            _preVisit = false;

            obj.Declarations?.Accept(this);

            _symbolTable.OpenScope();
            obj.Commands?.Accept(this);
            _symbolTable.CloseScope();

            obj.Declarations2?.Accept(this);

            _symbolTable.OpenScope();
            obj.Commands2?.Accept(this);
            _symbolTable.CloseScope();

            obj.Declarations3?.Accept(this);
            return null;
        }

        public object Visit(Assignment obj)
        {
            if (PrefabCheck(GetType(obj.Identifier)) || PrefabCheck(GetType(obj.Value)))
            {
                if (PrefabCheck(GetType(obj.Identifier)) && GetType(obj.Value) == "Prefab")
                {
                }
                else if (PrefabCheck(GetType(obj.Value)) && GetType(obj.Identifier) == "Prefab")
                {
                }
                else if (StandardTypeCheck(GetType(obj.Identifier), GetType(obj.Value)))
                {
                }
                else
                    throw new NotCompatibleTypes(obj.Identifier, obj.Value);
                if (obj.Expression != null)
                    throw new NotUsableWithOperator(obj.Value);
            }
            else if (ExpressionTypeCheck(GetType(obj.Identifier), GetType(obj.Value)))
            {
                if (obj.Expression != null)
                {
                    if (!ExpressionTypeCheck(GetType(obj.Identifier), GetType((string[])obj.Expression.Accept(this))))
                        throw new NotCompatibleTypes(obj.Identifier, (string[])obj.Expression.Accept(this));
                }
            }
            else if (_symbolTable.GetSymbol(obj.Identifier[1], obj.Identifier?[2])?.Type == "Movement")
            {
                string key = obj.Value[1];
                key = key.Substring(key.Length - (key.Length - 1));
                key = key.Remove(key.Length - 1);
                if (!Enum.IsDefined(typeof(SymbolTable.MovementButtons), key))
                    throw new NotCompatibleTypes(obj.Identifier, obj.Value);
                if (obj.Expression != null)
                    throw new NotUsableWithOperator(obj.Identifier);
            }
            else
            {
                throw new NotCompatibleTypes(obj.Identifier, obj.Value);
            }
            obj.NextCommands?.Accept(this);
            return null;
        }

        public object Visit(AssignMethodCall obj)
        {
            SymbolTable.Method method;
            if ((method = _symbolTable.Methods.Find(x => x.Name == obj.Identifier2[1])) != null)
            {
                if (StandardTypeCheck(GetType(obj.Identifier1), method.Type))
                {
                    if (_symbolTable.Methods.Find(x => x.Name == obj.Identifier2[1]).Parameters.Count == 0)
                    {
                        if (obj.Parameters.Count != 0)
                            throw new ParameterDifference(obj.Identifier2);
                    }
                    int i = 0;
                    foreach (SymbolTable.Variable parameter in method.Parameters)
                    {
                        if (!ParameterCheck(obj.Parameters.ElementAt(i)[0], parameter.Type))
                            throw new ParameterDifference(obj.Identifier2);
                        i++;
                    }
                }
                else if (_symbolTable.GetSymbol(obj.Identifier1[1], obj.Identifier1[2]).Type == "Method")
                {
                    if (StandardTypeCheck(method.Type, "void"))
                    {
                        if (method.Parameters.Count > 0)
                            throw new ParameterDifference(obj.Identifier2);
                    }
                    else
                        throw new NotCompatibleTypes(obj.Identifier1, obj.Identifier2);
                }
                else
                    throw new NotCompatibleTypes(obj.Identifier1, obj.Identifier2);
            }
            else
                throw new IdentifierNotFound(obj.Identifier2[1]);
            obj.NextCommands?.Accept(this);
            return null;
        }

        public object Visit(CommandsDeclaration obj)
        {
            _symbolTable.AddToTable(obj.Identifier, obj.Type);
            obj.NextCommands?.Accept(this);
            return null;
        }

        public object Visit(BooleanExpression obj)
        {
            if (StandardTypeCheck(obj.ComparisonOperator1, "touches"))
            {
                if (!(PrefabCheck(GetType(obj.Value1)) && PrefabCheck(GetType(obj.Value2))))
                    throw new NotCompatibleTypes(obj.Value1, obj.Value2);
                if (obj.Expression1 != null || obj.Expression2 != null)
                    throw new NotUsableWithOperator(obj.Value1);
                obj.BooleanExtension?.Accept(this);
                return null;
            }
            if (StandardTypeCheck(obj.ComparisonOperator1, "is=") || StandardTypeCheck(obj.ComparisonOperator1, "is!="))
            {
                if (PrefabCheck(GetType(obj.Value1)) || PrefabCheck(GetType(obj.Value2)) ||
                    StandardTypeCheck(GetType(obj.Value1), "Prefab") || StandardTypeCheck(GetType(obj.Value2), "Prefab"))
                {
                    if (!(PrefabCheck(GetType(obj.Value1)) || StandardTypeCheck(GetType(obj.Value1), "Prefab")))
                        throw new NotCompatibleTypes(obj.Value1, obj.Value2);
                    if (!(PrefabCheck(GetType(obj.Value2)) || StandardTypeCheck(GetType(obj.Value2), "Prefab")))
                        throw new NotCompatibleTypes(obj.Value1, obj.Value2);
                    if (obj.Expression1 != null || obj.Expression2 != null)
                        throw new NotUsableWithOperator(obj.Value1);
                    obj.BooleanExtension?.Accept(this);
                    return null;
                }
            }
            if (PrefabCheck(GetType(obj.Value1)) || PrefabCheck(GetType(obj.Value2)) ||
                StandardTypeCheck(GetType(obj.Value1), "Prefab") || StandardTypeCheck(GetType(obj.Value2), "Prefab"))
                throw new NotCompatibleTypes(obj.Value1, obj.Value2);
            if (ExpressionTypeCheck(GetType(obj.Value1), GetType(obj.Value2)))
            {
                if (obj.Expression1 != null)
                {
                    if (!ExpressionTypeCheck(GetType(obj.Value1), GetType((string[])obj.Expression1.Accept(this))))
                        throw new NotCompatibleTypes(obj.Value1, obj.Value2);
                }
                if (obj.Expression2 != null)
                {
                    if (!ExpressionTypeCheck(GetType(obj.Value1), GetType((string[])obj.Expression2.Accept(this))))
                        throw new NotCompatibleTypes(obj.Value1, obj.Value2);
                }
            }
            else
                throw new NotCompatibleTypes(obj.Value1, obj.Value2);
            obj.BooleanExtension?.Accept(this);
            return null;
        }

        public object Visit(Declaration obj)
        {
            if (_preVisit)
            {
                if (_parameterAdd)
                    _parameters.Add(new SymbolTable.Variable(obj.Identifier, obj.Type));
                else
                    _symbolTable.AddToTable(obj.Identifier, obj.Type);
            }
            obj.NextDeclarations?.Accept(this);
            return null;
        }

        public object Visit(ElseIfStatement obj)
        {
            obj.BooleanExpression.Accept(this);
            obj.Commands1?.Accept(this);
            obj.IfStatementExtend?.Accept(this);
            return null;
        }

        public object Visit(ElseStatement obj)
        {
            obj.Commands1?.Accept(this);
            return null;
        }

        public object Visit(IfStatement obj)
        {
            obj.BooleanExpression.Accept(this);
            obj.Commands1?.Accept(this);
            obj.IfStatementExtend?.Accept(this);
            obj.NextCommands?.Accept(this);
            return null;
        }

        public object Visit(MethodCall obj)
        {
            List<string> callingTypes = new List<string>();
            if (_symbolTable.Methods.Find(x => x.Name == obj.Identifier[1]) != null)
            {
                foreach (string[] parameter in obj.Parameters)
                {
                    callingTypes.Add(GetType(parameter));
                }
                if (_symbolTable.Methods.Find(x => x.Name == obj.Identifier[1]).Parameters.Count == 0)
                {
                    if (obj.Parameters.Count != 0)
                        throw new ParameterDifference(obj.Identifier);
                }
                else if (obj.Parameters.Count == 0)
                    throw new ParameterDifference(obj.Identifier);
                int i = 0;
                foreach (
                    SymbolTable.Variable parameter in
                    _symbolTable.Methods.Find(x => x.Name == obj.Identifier[1]).Parameters)
                {
                    if (!ParameterCheck(callingTypes[i], parameter.Type))
                    {
                        throw new ParameterDifference(obj.Identifier);
                    }
                    i++;
                }
            }
            else
                throw new IdentifierNotFound(obj.Identifier[1]);
            obj.NextCommands?.Accept(this);
            return null;
        }

        public object Visit(Expression obj)
        {
            if (GetType(obj.Value).ToLower() == "boolean")
                throw new NotUsableWithOperator(obj.Value);
            if (obj.Operator != "+" && GetType(obj.Value).ToLower() == "string")
                throw new NotUsableWithOperator(obj.Value);
            foreach (string prefabIdentifiersKey in _symbolTable.PrefabIdentifiers.Keys)
            {
                if (StandardTypeCheck(prefabIdentifiersKey, GetType(obj.Value)))
                    throw new NotUsableWithOperator(obj.Value);
            }
            if (obj.Expression1 != null)
            {
                if (!ExpressionTypeCheck(GetType(obj.Value), GetType((string[])obj.Expression1.Accept(this))))
                    throw new NotCompatibleTypes(obj.Value, (string[])obj.Expression1.Accept(this));
            }
            return obj.Value;
        }

        public object Visit(WhileStatement obj)
        {
            obj.BooleanExpression.Accept(this);
            obj.Commands1?.Accept(this);
            obj.NextCommands?.Accept(this);
            return null;
        }

        public object Visit(Write obj)
        {
            foreach (string[] strings in obj.WriteContext)
            {
                if (strings[0] != "StringValue")
                {
                    if (_symbolTable.GetSymbol(strings[1], strings[2]) == null)
                    {
                        throw new IdentifierNotFound(strings[1]);
                    }
                }
            }
            obj.NextCommands?.Accept(this);
            return null;
        }

        public object Visit(PrefabMethodCall obj)
        {
            List<string> callingTypes = new List<string>();
            foreach (string[] callingParameter in obj.CallingParameters)
            {
                callingTypes.Add(GetType(callingParameter));
            }
            if (_symbolTable.Methods.Find(x => x.Name == obj.PrefabMethod).Parameters.Count == 0)
            {
                if (obj.CallingParameters.Count != 0)
                    throw new ParameterDifference(obj.PrefabMethod);
            }
            int i = 0;
            foreach (SymbolTable.Variable parameter in _symbolTable.PrefabParameters[obj.PrefabMethod])
            {
                if (!ParameterCheck(callingTypes.ElementAt(i), parameter.Type))
                    throw new ParameterDifference(obj.PrefabMethod);
                i++;
            }
            obj.NextCommands?.Accept(this);
            return null;
        }

        public object Visit(MethodDeclaration obj)
        {
            if (_preVisit)
            {
                _parameterAdd = true;
                if (_symbolTable.Methods.Find(x => x.Name == obj.MethodIdentifier[1]) == null)
                {
                    foreach (Declaration parameter in obj.Parameters)
                    {
                        parameter.Accept(this);
                    }
                    _symbolTable.Methods.Add(new SymbolTable.Method(obj.MethodIdentifier[1], obj.MethodType, _parameters));
                    _parameters.Clear();
                }
                else
                {
                    throw new IdentifierAlreadyExists(obj.MethodIdentifier[1]);
                }
                _parameterAdd = false;
            }
            else
            {
                _symbolTable.OpenScope();
                foreach (Declaration declaration in obj.Parameters)
                {
                    declaration.Accept(this);
                }
                obj.Commands?.Accept(this);
                if (obj.ReturnValue == null && StandardTypeCheck(obj.MethodType, "void"))
                {
                    _symbolTable.CloseScope();
                    obj.NextDeclarations?.Accept(this);
                    return null;
                }
                if (!ExpressionTypeCheck(obj.MethodType, GetType(obj.ReturnValue)))
                {
                    throw new NotCompatibleTypes(obj.MethodIdentifier, obj.ReturnValue);
                }
                if (obj.ReturnExpression != null)
                {
                    if (
                        !ExpressionTypeCheck(GetType(obj.ReturnValue),
                            GetType((string[])obj.ReturnExpression.Accept(this))))
                    {
                        throw new NotCompatibleTypes(obj.ReturnValue, (string[])obj.ReturnExpression.Accept(this));
                    }
                }
                _symbolTable.CloseScope();
            }
            obj.NextDeclarations?.Accept(this);
            return null;
        }

        #region AssistingMethods

        private string GetType(string[] value)
        {
            if (value == null)
                return null;
            if (value[0] == "ValueKeyWord")
            {
                return value[2];
            }
            if (value[0] == "Identifier")
            {
                if (_symbolTable.Variables.Find(x => x.Name == value[1]) == null)
                    throw new IdentifierNotFound(value[1]);
                if (value.Length > 2)
                    return _symbolTable.GetSymbol(value[1], value[2])?.Type;
                return _symbolTable.GetSymbol(value[1], null).Type;
            }
            return value[0];
        }

        private bool StandardTypeCheck(string type1, string type2)
        {
            if (string.Equals(type1, type2, StringComparison.CurrentCultureIgnoreCase))
                return true;
            return false;
        }

        private bool ExpressionTypeCheck(string type1, string type2)
        {
            if (StandardTypeCheck(type1, type2))
                return true;
            if (StandardTypeCheck(type1, "time") && StandardTypeCheck(type2, "decimal"))
                return true;
            if (StandardTypeCheck(type2, "time") && StandardTypeCheck(type1, "decimal"))
                return true;
            return false;
        }

        private bool PrefabCheck(string type)
        {
            foreach (string prefabIdentifiersKey in _symbolTable.PrefabIdentifiers.Keys)
            {
                if (StandardTypeCheck(prefabIdentifiersKey, type))
                    return true;
            }
            return false;
        }

        private bool ParameterCheck(string type1, string type2)
        {
            if (ExpressionTypeCheck(type1, type2))
                return true;
            if (type2 == "Prefab")
            {
                if (PrefabCheck(type1))
                    return true;
            }
            return false;
        }

        #endregion
    }
}
