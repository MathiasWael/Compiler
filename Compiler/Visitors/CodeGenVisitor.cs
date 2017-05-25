using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Nodes;

namespace Compiler
{
    public class CodeGenVisitor : IVisitor
    {
        private SymbolTable _symbolTable = ContextVisitor._symbolTable;
        private List<string> _instantiateList = new List<string>();
        private bool _preVisit = true;

        public object Visit(Assignment obj)
        {
            string codeString = "";
            codeString += getValueText(obj.Identifier);
            codeString += " = ";
            codeString += getValueText(obj.Value);
            codeString += obj.Expression?.Accept(this);
            codeString += ";";
            codeString += obj.NextCommands?.Accept(this);
            return codeString;
        }

        public object Visit(AssignMethodCall obj)
        {
            string codeString = "";
            codeString += getValueText(obj.Identifier1);
            codeString += " = ";
            codeString += getValueText(obj.Identifier2);

            bool test = false;
            if (obj.Identifier1[2] != null)
            {
                test = _symbolTable.GetSymbol(obj.Identifier1[1], obj.Identifier1[2]).Type == "Method";
            }
            if (!test)
            {
                codeString += "(";
                codeString += getCallingParameters(obj.Parameters);
                codeString += ")";
            }
            codeString += ";";
            codeString += obj.NextCommands?.Accept(this);
            return codeString;
        }

        public object Visit(ASTNode obj)
        {
            throw new NotImplementedException();
        }

        public object Visit(BooleanExpression obj)
        {
            string codeString = "";
            codeString += getValueText(obj.Value1);
            codeString += obj.Expression1?.Accept(this);
            codeString += getComparisonOperator(obj.ComparisonOperator1);
            codeString += getValueText(obj.Value2);
            codeString += obj.Expression2?.Accept(this);
            if (obj.LogicalOperator != null)
            {
                codeString += getLogicalOperator(obj.LogicalOperator);
                codeString += obj.BooleanExtension.Accept(this);
            }
            return codeString;
        }

        public object Visit(Commands obj)
        {
            throw new NotImplementedException();
        }

        public object Visit(CommandsDeclaration obj)
        {
            string codeString = "";
            codeString += getDeclarationText(obj.Identifier, obj.Type);
            _symbolTable.AddToTable(obj.Identifier, obj.Type);
            codeString += ";";
            codeString += obj.NextCommands?.Accept(this);
            return codeString;
        }

        public object Visit(Declaration obj)
        {
            string codeString = "";
            if (_preVisit)
            {
                codeString += getDeclarationText(obj.Identifier, obj.Type) + ";";
            }
            codeString += obj.NextDeclarations?.Accept(this);
            return codeString;
        }

        public object Visit(Declarations obj)
        {
            throw new NotImplementedException();
        }

        public object Visit(ElseIfStatement obj)
        {
            string codeString = "";
            codeString += "else if ( ";
            codeString += obj.BooleanExpression?.Accept(this);
            codeString += " ) {";
            codeString += obj.Commands1?.Accept(this);
            codeString += "}";
            codeString += obj.IfStatementExtend?.Accept(this);
            return codeString;
        }

        public object Visit(ElseStatement obj)
        {
            string codeString = "";
            codeString += "else {";
            codeString += obj.Commands1?.Accept(this);
            codeString += "}";
            return codeString;
        }

        public object Visit(Expression obj)
        {
            string codeString = "";
            codeString += obj.Operator;
            codeString += getValueText(obj.Value);
            codeString += obj.Expression1?.Accept(this);
            return codeString;
        }

        public object Visit(IfStatement obj)
        {
            string codeString = "";
            codeString += "if(";
            codeString += obj.BooleanExpression.Accept(this);
            codeString += ") {";
            codeString += obj.Commands1?.Accept(this);
            codeString += "}";
            codeString += obj.IfStatementExtend?.Accept(this);
            codeString += obj.NextCommands?.Accept(this);
            return codeString;
        }

        public object Visit(IfStatementExtend obj)
        {
            throw new NotImplementedException();
        }

        public object Visit(MethodCall obj)
        {
            string codeString = "";
            codeString += getValueText(obj.Identifier);
            codeString += "(";
            codeString += getCallingParameters(obj.Parameters);
            codeString += ");";
            codeString += obj.NextCommands?.Accept(this);
            return codeString;
        }

        public object Visit(MethodDeclaration obj)
        {
            string codeString = "";
            if (!_preVisit)
            {
                _symbolTable.OpenScope();
                codeString += getMethodType(obj.MethodType);
                codeString += " ";
                codeString += getValueText(obj.MethodIdentifier);
                codeString += "(";
                bool first = true;
                foreach (Declaration declaration in obj.Parameters)
                {
                    if (first)
                    {
                        codeString += getDeclarationText(declaration.Identifier, declaration.Type);
                        first = false;
                    }
                    else
                        codeString += ", " + getDeclarationText(declaration.Identifier, declaration.Type);
                }
                codeString += ") {";
                codeString += obj.Commands?.Accept(this);
                if (obj.ReturnValue != null)
                {
                    codeString += "return ";
                    codeString += getValueText(obj.ReturnValue);
                    codeString += obj.ReturnExpression?.Accept(this);
                    codeString += ";";
                }
                codeString += "}";

                _symbolTable.CloseScope(); ///////////TESTFIX
            }
            obj.NextDeclarations?.Accept(this);
            return codeString;
        }

        public object Visit(PrefabMethodCall obj)
        {
            string codeString = "";
            if (obj.PrefabMethod.ToLower() == "delete")
            {
                codeString += "Destroy(";
                codeString += getCallingParameters(obj.CallingParameters);
                codeString += ");";
            }
            codeString += obj.NextCommands?.Accept(this);
            return codeString;
        }

        public object Visit(StartupStucture obj)
        {
            string codeString = "";

            string[] usingDirectives = { "using System;", "using System.Collections;", "using System.Collections.Generic;",
                                        "using System.Reflection;", "using UnityEngine;", "using UnityEngine.UI;" };

            foreach (string directive in usingDirectives)
                codeString += directive;

            codeString += "public class CompiledScript : MonoBehaviour { ";

            // Awake()
            codeString += obj.Declarations?.Accept(this);
            codeString += obj.Declarations2?.Accept(this);
            codeString += obj.Declarations3?.Accept(this);
            _preVisit = false;
            codeString += "void Awake() {";

            foreach (string type in _instantiateList)
            {
                codeString += type;
            }
            codeString += "}";

            // Start()
            codeString += "void Start(";
            /*            obj.DeclaringParameters?.Accept(this);*/      // Remove?
            codeString += ") {";
            codeString += obj.Commands?.Accept(this);
            _symbolTable.CloseScope();                  //TESTFIX
            codeString += "}";

            // Update()
            codeString += "void Update(";
            /*            obj.DeclaringParameters2?.Accept(this);*/     // REmove?!
            codeString += ") {";
            codeString += obj.Commands2?.Accept(this);
            _symbolTable.CloseScope();                  //TESTFIX
            codeString += "}";

            // Methods
            codeString += obj.Declarations?.Accept(this);
            codeString += obj.Declarations2?.Accept(this);
            codeString += obj.Declarations3?.Accept(this);

            codeString += "}";

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // TODO: Add Better method to fix spaceing in cs file.
            codeString = codeString.Replace(";", ";" + System.Environment.NewLine);
            codeString = codeString.Replace("{", System.Environment.NewLine + "{" + System.Environment.NewLine);
            codeString = codeString.Replace("}", "}" + System.Environment.NewLine);
            Form1.Formtest.TestString = codeString;
            //////////////////////////////////////////////////////////////////////////// This block is only for test.

            // File Setup
            if (File.Exists("C:/BOOTL/BOOTLUnityProject/Assets/Resources/Scripts/CompiledScript.cs"))
                File.WriteAllText("C:/BOOTL/BOOTLUnityProject/Assets/Resources/Scripts/CompiledScript.cs", String.Empty);

            StreamWriter file = new StreamWriter("C:/BOOTL/BOOTLUnityProject/Assets/Resources/Scripts/CompiledScript.cs", true);
            file.Write(codeString);
            file.Close();

            return null;
        }

        public object Visit(Statement obj)
        {
            throw new NotImplementedException();
        }

        public object Visit(WhileStatement obj)
        {
            string codeString = "";
            codeString += "while(";
            codeString += obj.BooleanExpression.Accept(this);
            codeString += ") {";
            codeString += obj.Commands1?.Accept(this);
            codeString += "}";
            codeString += obj.NextCommands?.Accept(this);
            return codeString;
        }

        public object Visit(Write obj)
        {
            string codeString = "";
            codeString += "DebugConsole.Log(";
            bool first = true;
            foreach (string[] textStrings in obj.WriteContext)
            {
                if (first)
                {
                    if (textStrings[0] == "Identifier")
                    {
                        codeString += getValueText(textStrings);
                    }
                    else
                    {
                        codeString += textStrings[1];
                    }
                    first = false;
                }
                else
                {
                    if (textStrings[0] == "Identifier")
                    {
                        codeString += " + " + getValueText(textStrings);
                    }
                    else
                    {
                        codeString += " + " + textStrings[1];
                    }
                }
            }
            codeString += ");";
            codeString += obj.NextCommands?.Accept(this);
            return codeString;
        }

        #region AssistMethods
        private string getValueText(string[] value)
        {
            string codeString = "";
            if (value[0] == "ValueKeyWord")
            {
                if (string.Equals(value[2], "time", StringComparison.CurrentCultureIgnoreCase))
                    codeString += value[1] + "Time.deltaTime";
            }
            else if (value[0] == "Boolean" || value[0] == "String")
            {
                codeString += value[1];
            }
            else if (value[0] == "Identifier")
            {
                codeString += value[1];
                if (value.Length > 2)
                {
                    if (value[2] != null)
                    {
                        string type = _symbolTable.Variables.Find(x => x.Name == value[1]).Type;
                        switch (type)
                        {
                            case "Camera":
                            case "Sprite":
                                type += "Controller";
                                break;
                            case "Text":
                                type = "UI" + type;
                                break;
                        }
                        codeString += ".GetComponent<" + type + ">().";
                        codeString += value[2];
                    }
                }
            }
            else if (value[0] == "Decimal")
            {
                codeString += value[1] + value[2] + "f";
            }
            else if (value[0] == "Integer")
            {
                codeString += value[1] + value[2];
            }
            else if (value[0] == "Point")
            {
                int i = 0;
                codeString += "new Vector2(";
                if (value[1] == "Identifier")
                {
                    codeString += "(float) " + getValueText(new[] { value[1], value[2], value[3] });
                    i = 1;
                }
                else
                {
                    codeString += value[1];
                    codeString += value[2] + "f";
                }
                codeString += ", ";
                if (value[3 + i] == "Identifier")
                {
                    codeString += "(float) " + getValueText(new[] { value[3 + i], value[4 + i], value[5 + i] });
                }
                else
                {
                    codeString += value[3 + i];
                    codeString += value[4 + i] + "f";
                }

                codeString += ")";
            }
            return codeString;
        }

        private string getComparisonOperator(string comparisonOperator)
        {
            string codeString = "";

            switch (comparisonOperator.ToLower())
            {
                case "is=":
                    codeString += " == ";
                    break;
                case "is<=":
                    codeString += " <= ";
                    break;
                case "is>=":
                    codeString += " >= ";
                    break;
                case "is<":
                    codeString += " < ";
                    break;
                case "is>":
                    codeString += " > ";
                    break;
                case "is!=":
                    codeString += " != ";
                    break;
                case "touches":
                    codeString += ".GetComponent<ITouching>().IsTouching == ";
                    break;
            }
            return codeString;
        }

        private string getLogicalOperator(string logicalOperator)
        {
            string codeString = "";

            switch (logicalOperator.ToLower())
            {
                case "or":
                    codeString += " || ";
                    break;
                case "and":
                    codeString += " && ";
                    break;
            }
            return codeString;
        }

        private string getDeclarationText(string identifier, string type)
        {
            string codeString = "";


            switch (type.ToLower())
            {
                case "integer":
                    codeString += "int " + identifier;
                    break;
                case "decimal":
                    codeString += "double " + identifier;
                    break;
                case "string":
                    codeString += "string " + identifier;
                    break;
                case "boolean":
                    codeString += "bool " + identifier;
                    break;
                case "point":
                    codeString += "Vector2 " + identifier;
                    break;
                case "character":
                    codeString += "GameObject " + identifier;
                    if (_preVisit)
                        _instantiateList.Add(identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/CharacterPrefab\"));");
                    else
                        codeString += "; " + identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/CharacterPrefab\"))";
                    break;
                case "camera":
                    codeString += "GameObject " + identifier;
                    if (_preVisit)
                        _instantiateList.Add(identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/CameraPrefab\"));");
                    else
                        codeString += "; " + identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/CameraPrefab\"))";
                    break;
                case "square":
                    codeString += "GameObject " + identifier;
                    if (_preVisit)
                        _instantiateList.Add(identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/SquarePrefab\"));");
                    else
                        codeString += "; " + identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/SquarePrefab\"))";
                    break;
                case "triangle":
                    codeString += "GameObject " + identifier;
                    if (_preVisit)
                        _instantiateList.Add(identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/TrianglePrefab\"));");
                    else
                        codeString += "; " + identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/TrianglePrefab\"))";
                    break;
                case "sprite":
                    codeString += "GameObject " + identifier;
                    if (_preVisit)
                        _instantiateList.Add(identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/SpritePrefab\"));");
                    else
                        codeString += "; " + identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/SpritePrefab\"))";
                    break;
                case "text":
                    codeString += "GameObject " + identifier;
                    if (_preVisit)
                        _instantiateList.Add(identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/UITextPrefab\"));");
                    else
                        codeString += "; " + identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/UITextPrefab\"))";
                    break;
                case "trigger":
                    codeString += "GameObject " + identifier;
                    if (_preVisit)
                        _instantiateList.Add(identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/TriggerPrefab\"));");
                    else
                        codeString += "; " + identifier + " = (GameObject) Instantiate(Resources.Load(\"Prefabs/TriggerPrefab\"))";
                    break;
            }
            return codeString;
        }

        private string getMethodType(string methodType)
        {
            string codeString = "";

            switch (methodType.ToLower())
            {
                case "integer":
                    codeString += "int";
                    break;
                case "decimal":
                    codeString += "double";
                    break;
                case "string":
                    codeString += "string";
                    break;
                case "boolean":
                    codeString += "bool";
                    break;
                case "point":
                    codeString += "Vector2";
                    break;
                case "void":
                    codeString += "void";
                    break;
            }
            return codeString;
        }

        private string getCallingParameters(List<string[]> parameters)
        {
            string codeString = "";
            foreach (string[] parameter in parameters)
            {
                codeString += getValueText(parameter);
            }
            return codeString;
        }
        #endregion
    }
}
