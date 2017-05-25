using System;
using System.Collections.Generic;

namespace Compiler
{
    public class SymbolTable
    {
        public class Variable
        {
            public List<Variable> ClassSymbolTable = new List<Variable>();
            public string Name { get; }
            public string Type { get; }
            public Variable(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }
        public class Method
        {
            public List<Variable> Parameters;
            public string Name { get; }
            public string Type { get; }

            public Method(string name, string type, List<Variable> parameters)
            {
                Name = name;
                Type = type;
                Parameters = new List<Variable>(parameters);
            }
        }
        public List<Method> Methods = new List<Method>();
        private readonly List<Variable> _globalScope = new List<Variable>();
        private readonly List<Variable> _methodScope = new List<Variable>();
        private static bool _inMethodScope;

        public SymbolTable()
        {
            Methods.Add(new Method("Move", "Void", PrefabParameters["Move"]));
            Methods.Add(new Method("Delete", "Void", PrefabParameters["Delete"]));
        }

        public List<Variable> Variables
        {
            get
            {
                List<Variable> combined = new List<Variable>();
                combined.AddRange(_globalScope);
                combined.AddRange(_methodScope);
                return combined;
            }
        }

        public Variable GetSymbol(string name1, string name2)
        {
            if (Variables.Find(x => x.Name == name1) == null)
                return null;
            if (name2 != null)
                return Variables.Find(x => x.Name == name1).ClassSymbolTable.Find(x => x.Name == name2);
            return Variables.Find(x => x.Name == name1);
        }

        public void AddToTable(string name, string type)
        {
            Variable variable1 = new Variable(name, type);
            if(Variables.Find(x => x.Name == name) != null)
                throw new Exception();
            if (_inMethodScope)
                _methodScope.Add(variable1);
            else
                _globalScope.Add(variable1);
            if (PrefabIdentifiers.ContainsKey(type))
                variable1.ClassSymbolTable = PrefabIdentifiers[type];
        }

        public void OpenScope()
        {
            _inMethodScope = true;
        }

        public void CloseScope()
        {
            _methodScope.Clear();
            _inMethodScope = false;
        }

        public readonly Dictionary<string, List<Variable>> PrefabIdentifiers = new Dictionary<string, List<Variable>>
            {
                {
                    "Character",
                    new List<Variable>
                    {
                        new Variable ("Size", "Point"),
                        new Variable("Location", "Point"),
                        new Variable("Speed", "Decimal"),
                        new Variable("MoveLeftKey", "Movement"),
                        new Variable("MoveRightKey", "Movement"),
                        new Variable("JumpKey", "Movement"),
                        new Variable("Alive", "Boolean"),
                        new Variable("JumpHeight", "Decimal")
                    }
                },
                {
                    "Camera",
                    new List<Variable>
                    {
                        new Variable ("Location", "Point"),
                        new Variable("Target", "Prefab"),
                        new Variable("DistanceToTarget", "Decimal")
                    }
                },
                {
                    "Sprite",
                    new List<Variable>
                    {
                        new Variable ("Size", "Point"),
                        new Variable("Location", "Point"),
                        new Variable("Picture", "String"),
                        new Variable("Speed", "Decimal"),
                        new Variable("Visible", "Boolean")
                    }
                },
                {
                    "Square",
                    new List<Variable>
                    {
                        new Variable ("Size", "Point"),
                        new Variable("Location", "Point"),
                        new Variable("Picture", "String"),
                        new Variable("Visible", "Boolean")
                        // add trigger?, 
                    }
                },
                {
                    "Triangle",
                    new List<Variable>
                    {
                        new Variable ("Size", "Point"),
                        new Variable("Location", "Point"),
                        new Variable("Picture", "String"),
                        new Variable("Visible", "Boolean")
                        // add trigger?, 
                    }
                },
                {
                    "Text",
                    new List<Variable>
                    {
                        new Variable ("TextSize", "Integer"),
                        new Variable("Location", "Point"),
                        new Variable("DisplayText", "String"),
                        new Variable("Visible", "Boolean"),
                        new Variable("TextboxSize", "Point")
                        // add textbox Size?
                    }
                },
                {
                    "Trigger",
                    new List<Variable>
                    {
                        new Variable ("Size", "Point"),
                        new Variable("Location", "Point"),
                        new Variable("Enabled", "Boolean"),
                        new Variable("OnEnter", "Method"),
                        new Variable("OnExit", "Method"),
                        new Variable("OnStay", "Method")
                        // add Damage tick?
                    }
                }
        };

        public readonly Dictionary<string, List<Variable>> PrefabParameters = new Dictionary<string, List<Variable>>
        {
            {
                "Move",
                new List<Variable>
                {
                    new Variable(null, "Prefab"),
                    new Variable(null, "Point"),
                    new Variable(null, "Decimal")

                }

            },
            {
                   "Delete",
                   new List<Variable>
                   {
                       new Variable(null, "Prefab")
                   }
            }
        };
        public enum MovementButtons
        {
            Backspace, Delete, Tab, Clear, Return, Pause, Escape, Space,
            Keypad0, Keypad1, Keypad2, Keypad3, Keypad4, Keypad5, Keypad6, Keypad7, Keypad8, Keypad9,
            KeypadPeriod, KeypadDivide, KeypadMultiply, KeypadMinus, KeypadPlus, KeypadEnter, KeypadEquals,
            UpArrow, DownArrow, RightArrow, LeftArrow, Insert, Home, End, PageUp, PageDown, F1, F2, F3,
            F4, F5, F6, F7, F8, F9, F10, F11, F12, F13, F14, F15, Alpha0, Alpha1, Alpha2, Alpha3,
            Alpha4, Alpha5, Alpha6, Alpha7, Alpha8, Alpha9, Exlaim, DoubleQuote, Hash, Dollar, Ampersand,
            Quote, Leftparen, RightParen, Asterisk, Plus, Comma, Minus, Period, Slash, Colon, Semicolon,
            Less, Equals, Greater, Question, At, LeftBracket, Backslash, RightBracket, Caret, Underscore,
            BackQuote, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
            Numlock, CapsLock, ScrollLock, RightShift, LeftShift, RightControl, LeftControl, RightAlt,
            LeftAlt, LeftCommand, LeftApple, LeftWindows, RightCommand, RightApple, RightWindows, AltGr,
            Help, Print, SysReq, Break, Menu, Mouse0, Mouse1, Mouse2, Mouse3, Mouse4, Mouse5, Mouse6
        }
    }
}
