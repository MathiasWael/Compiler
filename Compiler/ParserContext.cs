using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Compiler.Nodes;
using GoldParser;
using System.Collections;

namespace Compiler
{
    class ParserContext
    {
        private Parser _parser;
        private SymbolTable _symbolTable = new SymbolTable();
        private string[] temp;

        public ParserContext(Parser parser)
        {
            _parser = parser;
        }

        public object CreateNode()
        {
            switch ((RuleConstants)_parser.ReductionRule.Index)
            {
                case RuleConstants.RULE_S_VOID_STARTUP_END_STARTUP_VOID_GAMELOOP_END_GAMELOOP:
                    //<S> ::= <Declarations> void startup <Commands> end startup <Declarations> void GameLoop <Commands> end GameLoop <Declarations>
                    return new StartupStucture(Node<Declarations>(0), Node<Commands>(3), Node<Declarations>(6), Node<Commands>(9), Node<Declarations>(12));

                case RuleConstants.RULE_COMMANDS:
                    //<Commands> ::= <Statement> <Commands>
                    Node<Statement>(0).NextCommands = Node<Commands>(1);
                    return Node<Statement>(0);

                case RuleConstants.RULE_COMMANDS_SEMI:
                    //<Commands> ::= <Declaration> ';' <Commands>
                    return new CommandsDeclaration(Node<Declaration>(0), Node<Commands>(2));

                case RuleConstants.RULE_COMMANDS2:
                    //<Commands> ::= 
                    return null;

                case RuleConstants.RULE_WRITEMETHOD_WRITE_LPAREN_RPAREN_SEMI:
                    //<WriteMethod> ::= write '(' <Text> ')' ';'
                    return new Write();

                case RuleConstants.RULE_METHODCALL_CALL_LPAREN_RPAREN_SEMI:
                    //<MethodCall> ::= Call <Identifiers> '(' <CallingParameters> ')' ';'
                    return new MethodCall(Node<string[]>(1));

                case RuleConstants.RULE_METHODCALL_EQ_CALL_LPAREN_RPAREN_SEMI:
                    //<MethodCall> ::= <Identifiers> '=' Call <Identifiers> '(' <CallingParameters> ')' ';'
                    return new AssignMethodCall(Node<string[]>(0), Node<string[]>(3));

                case RuleConstants.RULE_PREFABMETHODCALL_CALL_LPAREN_RPAREN_SEMI:
                    //<PrefabMethodCall> ::= Call <PrefabMethods> '(' <CallingParameters> ')' ';'
                    return new PrefabMethodCall(Token(1));

                case RuleConstants.RULE_ASSIGNMENT_EQ_SEMI:
                    //<Assignment> ::= <Identifiers> '=' <Value> <Expression> ';'
                    return new Assignment(Node<string[]>(0), Node<string[]>(2), Node<Expression>(3));

                case RuleConstants.RULE_CONTROLSTATEMENTS_IF_LPAREN_RPAREN_END_IF:
                    //<ControlStatements> ::= if '(' <BooleanExpression> ')' <Commands> <ElseIfStatementExtend> end if
                    return new IfStatement(Node<BooleanExpression>(2), Node<Commands>(4), Node<IfStatementExtend>(5));

                case RuleConstants.RULE_CONTROLSTATEMENTS_WHILE_LPAREN_RPAREN_END_WHILE:
                    //<ControlStatements> ::= while '(' <BooleanExpression> ')' <Commands> end while
                    return new WhileStatement(Node<BooleanExpression>(2), Node<Commands>(4));

                case RuleConstants.RULE_ELSEIFSTATEMENTEXTEND_ELSEIF_LPAREN_RPAREN:
                    //<ElseIfStatementExtend> ::= 'else if' '(' <BooleanExpression> ')' <Commands> <ElseIfStatementExtend>
                    return new ElseIfStatement(Node<BooleanExpression>(2), Node<Commands>(4), Node<IfStatementExtend>(5));

                case RuleConstants.RULE_ELSESTATEMENTEXTEND_ELSE:
                    //<ElseStatementExtend> ::= else <Commands>
                    return new ElseStatement(Node<Commands>(1));

                case RuleConstants.RULE_ELSESTATEMENTEXTEND:
                    //<ElseStatementExtend> ::= 
                    return null;

                case RuleConstants.RULE_DECLARATION_IDENTIFIER:
                    //<Declaration> ::= <Type> Identifier
                    return new Declaration(Token(0), Token(1));

                case RuleConstants.RULE_DECLARATIONS_SEMI:
                    //<Declarations> ::= <Declaration> ';' <Declarations>
                    Node<Declaration>(0).NextDeclarations = Node<Declarations>(2);
                    return Node<Declaration>(0);

                case RuleConstants.RULE_DECLARATIONS:
                    //<Declarations> ::= <MethodDeclaration> <Declarations>
                    Node<MethodDeclaration>(0).NextDeclarations = Node<Declarations>(1);
                    return Node<MethodDeclaration>(0);

                case RuleConstants.RULE_DECLARATIONS2:
                    //<Declarations> ::= 
                    return null;

                case RuleConstants.RULE_METHODDECLARATION_METHOD_IDENTIFIER_LPAREN_RPAREN_END_METHOD:
                    //<MethodDeclaration> ::= method <Methodtype> Identifier '(' <DeclaringParameters> ')' <Commands> <ReturnStatement> end method
                    return new MethodDeclaration(Token(1), Token(2), Node<Commands>(6), Node<ReturnStatement>(7));

                case RuleConstants.RULE_RETURNSTATEMENT_RETURN_SEMI:
                    //<ReturnStatement> ::= return <Value> <Expression> ';'
                    return new ReturnStatement(Node<string[]>(1), Node<Expression>(2));

                case RuleConstants.RULE_RETURNSTATEMENT:
                    //<ReturnStatement> ::= 
                    return null;

                case RuleConstants.RULE_CALLINGPARAMETERS:
                    //<CallingParameters> ::= <Value> <CallingParameter>
                    return new CallingParameter(Node<string[]>(0));

                case RuleConstants.RULE_CALLINGPARAMETERS2:
                    //<CallingParameters> ::= 
                    return null;

                case RuleConstants.RULE_CALLINGPARAMETER_COMMA:
                    //<CallingParameter> ::= ',' <Value> <CallingParameter>
                    return new CallingParameter(Node<string[]>(1));

                case RuleConstants.RULE_CALLINGPARAMETER:
                    //<CallingParameter> ::= 
                    return null;

                case RuleConstants.RULE_DECLARINGPARAMETERS:
                    //<DeclaringParameters> ::= <Declaration> <DeclaringParameter>
                    return new DeclaringParameter(Node<Declaration>(0));

                case RuleConstants.RULE_DECLARINGPARAMETERS2:
                    //<DeclaringParameters> ::= 
                    return null;

                case RuleConstants.RULE_DECLARINGPARAMETER_COMMA:
                    //<DeclaringParameter> ::= ',' <Declaration> <DeclaringParameter>
                    return new DeclaringParameter(Node<Declaration>(1));

                case RuleConstants.RULE_DECLARINGPARAMETER:
                    //<DeclaringParameter> ::= 
                    return null;

                case RuleConstants.RULE_EXPRESSION:
                    //<Expression> ::= <operator> <Value> <Expression>
                    return new Expression(Token(0), Node<string[]>(1), Node<Expression>(2));

                case RuleConstants.RULE_EXPRESSION2:
                    //<Expression> ::= 
                    return null;

                case RuleConstants.RULE_BOOLEANEXPRESSION:
                    //<BooleanExpression> ::= <Value> <Expression> <comparisonoperator> <Value> <Expression>
                    return new BooleanExpression(Node<string[]>(0), Node<Expression>(1), Token(2), Node<string[]>(3), Node<Expression>(4));

                case RuleConstants.RULE_BOOLEANEXPRESSION2:
                    //<BooleanExpression> ::= <Value> <Expression> <comparisonoperator> <Value> <Expression> <logicaloperator> <Value> <Expression> <comparisonoperator> <Value> <Expression>
                    return new BooleanExpression(Node<string[]>(0), Node<Expression>(1), Token(2), Node<string[]>(3), Node<Expression>(4), Token(5), Node<string[]>(6), Node<Expression>(7), Token(8), Node<string[]>(9), Node<Expression>(10));

                case RuleConstants.RULE_LOGICALOPERATOR_OR:
                    //<logicaloperator> ::= or
                    return Token(0);

                case RuleConstants.RULE_LOGICALOPERATOR_AND:
                    //<logicaloperator> ::= and
                    return Token(0);

                case RuleConstants.RULE_OPERATOR_TIMES:
                    //<operator> ::= '*'
                    return Token(0);

                case RuleConstants.RULE_OPERATOR_PLUS:
                    //<operator> ::= '+'
                    return Token(0);

                case RuleConstants.RULE_OPERATOR_DIV:
                    //<operator> ::= '/'
                    return Token(0);

                case RuleConstants.RULE_OPERATOR_MINUS:
                    //<operator> ::= '-'
                    return Token(0);

                case RuleConstants.RULE_COMPARISONOPERATOR_ISEQ:
                    //<comparisonoperator> ::= 'is='
                    return Token(0);

                case RuleConstants.RULE_COMPARISONOPERATOR_ISLTEQ:
                    //<comparisonoperator> ::= 'is<='
                    return Token(0);

                case RuleConstants.RULE_COMPARISONOPERATOR_ISGTEQ:
                    //<comparisonoperator> ::= 'is>='
                    return Token(0);

                case RuleConstants.RULE_COMPARISONOPERATOR_ISLT:
                    //<comparisonoperator> ::= 'is<'
                    return Token(0);

                case RuleConstants.RULE_COMPARISONOPERATOR_ISGT:
                    //<comparisonoperator> ::= 'is>'
                    return Token(0);

                case RuleConstants.RULE_COMPARISONOPERATOR_ISEXCLAMEQ:
                    //<comparisonoperator> ::= 'is!='
                    return Token(0);

                case RuleConstants.RULE_COMPARISONOPERATOR_TOUCHES:
                    //<comparisonoperator> ::= touches
                    return Token(0);

                case RuleConstants.RULE_TEXT_STRINGVALUE:
                    //<Text> ::= StringValue <TextPrime>
                    return new Text(Token(0));

                case RuleConstants.RULE_TEXT:
                    //<Text> ::= <Identifiers> <TextPrime>
                    return new Text(Node<string[]>(0));

                case RuleConstants.RULE_TEXT2:
                    //<Text> ::= 
                    return null;

                case RuleConstants.RULE_TEXTPRIME_PLUS:
                    //<TextPrime> ::= '+' <Identifiers> <TextPrime>
                    return new Text(Node<string[]>(1));

                case RuleConstants.RULE_TEXTPRIME_PLUS_STRINGVALUE:
                    //<TextPrime> ::= '+' StringValue <TextPrime>
                    return new Text(Token(1));

                case RuleConstants.RULE_TEXTPRIME:
                    //<TextPrime> ::= 
                    return null;

                case RuleConstants.RULE_TYPE_INTEGER:
                    //<Type> ::= Integer
                    return Token(0);

                case RuleConstants.RULE_TYPE_DECIMAL:
                    //<Type> ::= Decimal
                    return Token(0);

                case RuleConstants.RULE_TYPE_STRING:
                    //<Type> ::= String
                    return Token(0);

                case RuleConstants.RULE_TYPE_BOOLEAN:
                    //<Type> ::= Boolean
                    return Token(0);

                case RuleConstants.RULE_TYPE_POINT:
                    //<Type> ::= Point
                    return Token(0);

                case RuleConstants.RULE_METHODTYPE_VOID:
                    //<Methodtype> ::= void
                    return Token(0);

                case RuleConstants.RULE_VALUE_INTEGERVALUE:
                    //<Value> ::= <Prefix> IntegerValue
                    temp = new []{"Integer", Token(0), Token(1)};
                    return temp;

                case RuleConstants.RULE_VALUE_DECIMALVALUE:
                    //<Value> ::= <Prefix> DecimalValue
                    temp = new[] { "Decimal", Token(0), Token(1) };
                    return temp;

                case RuleConstants.RULE_VALUE_STRINGVALUE:
                    //<Value> ::= StringValue
                    temp = new[] { "String", Token(0)};
                    return temp;

                case RuleConstants.RULE_VALUE_LPAREN_COMMA_RPAREN:
                    //<Value> ::= '(' <PointValue> ',' <PointValue> ')'
                    if (Node<string[]>(1).Length > 2 || Node<string[]>(3).Length > 2)
                    {
                        if (Node<string[]>(1).Length > 2 && Node<string[]>(3).Length > 2)
                        {
                            temp = new[] { "Point", Node<string[]>(1)[0], Node<string[]>(1)[1], Node<string[]>(1)[2], Node<string[]>(3)[0], Node<string[]>(3)[1], Node<string[]>(3)[2] };
                        }
                        else if (Node<string[]>(1).Length > 2)
                        {
                            temp = new[] { "Point", Node<string[]>(1)[0], Node<string[]>(1)[1], Node<string[]>(1)[2], Node<string[]>(3)[0], Node<string[]>(3)[1] };
                        }
                        else if (Node<string[]>(3).Length > 2)
                        {
                            temp = new[] { "Point", Node<string[]>(1)[0], Node<string[]>(1)[1], Node<string[]>(3)[0], Node<string[]>(3)[1], Node<string[]>(3)[2] };
                        }
                        return temp;
                    }
                    temp = new[] { "Point", Node<string[]>(1)[0], Node<string[]>(1)[1], Node<string[]>(3)[0], Node<string[]>(3)[1] };
                    return temp;

                case RuleConstants.RULE_POINTVALUE_DECIMALVALUE:
                    //<PointValue> ::= <Prefix> DecimalValue
                    temp = new[] { Token(0), Token(1) };
                    return temp;

                case RuleConstants.RULE_VALUEKEYWORDS_TIME:
                    //<ValueKeywords> ::= <Prefix> Time
                    temp = new[] { "ValueKeyWord", Token(0), Token(1) };
                    return temp;

                case RuleConstants.RULE_PREFIX_MINUS:
                    //<Prefix> ::= '-'
                    return Token(0);

                case RuleConstants.RULE_PREFIX:
                    //<Prefix> ::= 
                    return null;

                case RuleConstants.RULE_BOOLEANVALUE_TRUE:
                    //<BooleanValue> ::= true
                    temp = new[] { "Boolean", Token(0)};
                    return temp;

                case RuleConstants.RULE_BOOLEANVALUE_FALSE:
                    //<BooleanValue> ::= false
                    temp = new[] { "Boolean", Token(0) };
                    return temp;

                case RuleConstants.RULE_IDENTIFIERS_IDENTIFIER:
                    //<Identifiers> ::= Identifier
                    temp = new[] {"Identifier", Token(0), null};
                    return temp;

                case RuleConstants.RULE_IDENTIFIERS_IDENTIFIER_DOT_IDENTIFIER:
                    //<Identifiers> ::= Identifier '.' Identifier
                    temp = new[] {"Identifier", Token(0), Token(2) };
                    return temp;

                case RuleConstants.RULE_PREFABCLASSES_CHARACTER:
                    //<PrefabClasses> ::= Character
                    return Token(0);

                case RuleConstants.RULE_PREFABCLASSES_CAMERA:
                    //<PrefabClasses> ::= Camera
                    return Token(0);

                case RuleConstants.RULE_PREFABCLASSES_SQUARE:
                    //<PrefabClasses> ::= Square
                    return Token(0);

                case RuleConstants.RULE_PREFABCLASSES_TRIANGLE:
                    //<PrefabClasses> ::= Triangle
                    return Token(0);

                case RuleConstants.RULE_PREFABCLASSES_SPRITE:
                    //<PrefabClasses> ::= Sprite
                    return Token(0);

                case RuleConstants.RULE_PREFABCLASSES_TEXT:
                    //<PrefabClasses> ::= Text
                    return Token(0);

                case RuleConstants.RULE_PREFABCLASSES_TRIGGER:
                    //<PrefabClasses> ::= Trigger
                    return Token(0);

                case RuleConstants.RULE_PREFABMETHODS_DELETE:
                    //<PrefabMethods> ::= Delete
                    return Token(0);

                default:
                    throw new RuleException("Unknown rule: Does your CGT Match your Code Revision?");
            }
        }

        #region Rules
        private T Node<T>(int index)
        {
            return (T)_parser.GetReductionSyntaxNode(index);
        }
        private string Token(int index)
        {
            return (string)_parser.GetReductionSyntaxNode(index);
        }
        [Serializable]
        public class SymbolException : Exception
        {
            public SymbolException(string message) : base(message)
            {
            }


            public SymbolException(string message,
                Exception inner) : base(message, inner)
            {
            }

            protected SymbolException(SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }

        }
        [Serializable]
        public class RuleException : Exception
        {

            public RuleException(string message) : base(message)
            {
            }

            public RuleException(string message,
                                 Exception inner) : base(message, inner)
            {
            }

            protected RuleException(SerializationInfo info,
                                    StreamingContext context) : base(info, context)
            {
            }

        }
        public string GetTokenText()
        {
            switch (_parser.TokenSymbol.Index)
            {
                case (int)SymbolConstants.SYMBOL_EOF:
                    //(EOF)
                    //Token Kind: 3
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ERROR:
                    //(Error)
                    //Token Kind: 7
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE:
                    //Whitespace
                    //Token Kind: 2
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_MINUS:
                    //'-'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                case (int)SymbolConstants.SYMBOL_LPAREN:
                    //'('
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_RPAREN:
                    //')'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TIMES:
                    //'*'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_COMMA:
                    //','
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DOT:
                    //'.'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DIV:
                    //'/'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_SEMI:
                    //';'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_PLUS:
                    //'+'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_EQ:
                    //'='
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_AND:
                    //and
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_BOOLEAN:
                    //Boolean
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_CALL:
                    //Call
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_CAMERA:
                    //Camera
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_CHARACTER:
                    //Character
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DECIMAL:
                    //Decimal
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DECIMALVALUE:
                    //DecimalValue
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DELETE:
                    //Delete
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ELSE:
                    //else
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ELSEIF:
                    //'else if'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_END:
                    //end
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_FALSE:
                    //false
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_GAMELOOP:
                    //GameLoop
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER:
                    //Identifier
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_IF:
                    //if
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_INTEGER:
                    //Integer
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_INTEGERVALUE:
                    //IntegerValue
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ISEXCLAMEQ:
                    //'is!='
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ISLT:
                    //'is<'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ISLTEQ:
                    //'is<='
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ISEQ:
                    //'is='
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ISGT:
                    //'is>'
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ISGTEQ:
                    //'is>='
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_METHOD:
                    //method
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_OR:
                    //or
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_POINT:
                    //Point
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_RETURN:
                    //return
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_SPRITE:
                    //Sprite
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_SQUARE:
                    //Square
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_STARTUP:
                    //startup
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_STRING:
                    //String
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_STRINGVALUE:
                    //StringValue
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TEXT:
                    //Text
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TIME:
                    //Time
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TOUCHES:
                    //touches
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TRIANGLE:
                    //Triangle
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TRIGGER:
                    //Trigger
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TRUE:
                    //true
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_VOID:
                    //void
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_WHILE:
                    //while
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_WRITE:
                    //write
                    return _parser.TokenString;
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ASSIGNMENT:
                    //<Assignment>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_BOOLEANEXPRESSION:
                    //<BooleanExpression>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_BOOLEANVALUE:
                    //<BooleanValue>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_CALLINGPARAMETER:
                    //<CallingParameter>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_CALLINGPARAMETERS:
                    //<CallingParameters>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_COMMANDS:
                    //<Commands>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_COMPARISONOPERATOR:
                    //<comparisonoperator>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_CONTROLSTATEMENTS:
                    //<ControlStatements>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DECLARATION:
                    //<Declaration>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DECLARATIONS:
                    //<Declarations>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DECLARINGPARAMETER:
                    //<DeclaringParameter>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_DECLARINGPARAMETERS:
                    //<DeclaringParameters>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ELSEIFSTATEMENTEXTEND:
                    //<ElseIfStatementExtend>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_ELSESTATEMENTEXTEND:
                    //<ElseStatementExtend>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSION:
                    //<Expression>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIERS:
                    //<Identifiers>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_LOGICALOPERATOR:
                    //<logicaloperator>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_METHODCALL:
                    //<MethodCall>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_METHODDECLARATION:
                    //<MethodDeclaration>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_METHODTYPE:
                    //<Methodtype>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_OPERATOR:
                    //<operator>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_POINTVALUE:
                    //<PointValue>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_PREFABCLASSES:
                    //<PrefabClasses>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_PREFABMETHODCALL:
                    //<PrefabMethodCall>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_PREFABMETHODS:
                    //<PrefabMethods>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_PREFIX:
                    //<Prefix>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_RETURNSTATEMENT:
                    //<ReturnStatement>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_S:
                    //<S>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_STATEMENT:
                    //<Statement>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TEXT2:
                    //<Text>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TEXTPRIME:
                    //<TextPrime>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_TYPE:
                    //<Type>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_VALUE:
                    //<Value>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_VALUEKEYWORDS:
                    //<ValueKeywords>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                case (int)SymbolConstants.SYMBOL_WRITEMETHOD:
                    //<WriteMethod>
                    //Token Kind: 0
                    //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                    // return m_parser.TokenString;
                    return null;

                default:
                    throw new SymbolException("You don't want the text of a non-terminal symbol");
            }

        }
        enum SymbolConstants
        {
            SYMBOL_EOF = 0, // (EOF)
            SYMBOL_ERROR = 1, // (Error)
            SYMBOL_WHITESPACE = 2, // Whitespace
            SYMBOL_MINUS = 3, // '-'
            SYMBOL_LPAREN = 4, // '('
            SYMBOL_RPAREN = 5, // ')'
            SYMBOL_TIMES = 6, // '*'
            SYMBOL_COMMA = 7, // ','
            SYMBOL_DOT = 8, // '.'
            SYMBOL_DIV = 9, // '/'
            SYMBOL_SEMI = 10, // ';'
            SYMBOL_PLUS = 11, // '+'
            SYMBOL_EQ = 12, // '='
            SYMBOL_AND = 13, // and
            SYMBOL_BOOLEAN = 14, // Boolean
            SYMBOL_CALL = 15, // Call
            SYMBOL_CAMERA = 16, // Camera
            SYMBOL_CHARACTER = 17, // Character
            SYMBOL_DECIMAL = 18, // Decimal
            SYMBOL_DECIMALVALUE = 19, // DecimalValue
            SYMBOL_DELETE = 20, // Delete
            SYMBOL_ELSE = 21, // else
            SYMBOL_ELSEIF = 22, // 'else if'
            SYMBOL_END = 23, // end
            SYMBOL_FALSE = 24, // false
            SYMBOL_GAMELOOP = 25, // GameLoop
            SYMBOL_IDENTIFIER = 26, // Identifier
            SYMBOL_IF = 27, // if
            SYMBOL_INTEGER = 28, // Integer
            SYMBOL_INTEGERVALUE = 29, // IntegerValue
            SYMBOL_ISEXCLAMEQ = 30, // 'is!='
            SYMBOL_ISLT = 31, // 'is<'
            SYMBOL_ISLTEQ = 32, // 'is<='
            SYMBOL_ISEQ = 33, // 'is='
            SYMBOL_ISGT = 34, // 'is>'
            SYMBOL_ISGTEQ = 35, // 'is>='
            SYMBOL_METHOD = 36, // method
            SYMBOL_OR = 37, // or
            SYMBOL_POINT = 38, // Point
            SYMBOL_RETURN = 39, // return
            SYMBOL_SPRITE = 40, // Sprite
            SYMBOL_SQUARE = 41, // Square
            SYMBOL_STARTUP = 42, // startup
            SYMBOL_STRING = 43, // String
            SYMBOL_STRINGVALUE = 44, // StringValue
            SYMBOL_TEXT = 45, // Text
            SYMBOL_TIME = 46, // Time
            SYMBOL_TOUCHES = 47, // touches
            SYMBOL_TRIANGLE = 48, // Triangle
            SYMBOL_TRIGGER = 49, // Trigger
            SYMBOL_TRUE = 50, // true
            SYMBOL_VOID = 51, // void
            SYMBOL_WHILE = 52, // while
            SYMBOL_WRITE = 53, // write
            SYMBOL_ASSIGNMENT = 54, // <Assignment>
            SYMBOL_BOOLEANEXPRESSION = 55, // <BooleanExpression>
            SYMBOL_BOOLEANVALUE = 56, // <BooleanValue>
            SYMBOL_CALLINGPARAMETER = 57, // <CallingParameter>
            SYMBOL_CALLINGPARAMETERS = 58, // <CallingParameters>
            SYMBOL_COMMANDS = 59, // <Commands>
            SYMBOL_COMPARISONOPERATOR = 60, // <comparisonoperator>
            SYMBOL_CONTROLSTATEMENTS = 61, // <ControlStatements>
            SYMBOL_DECLARATION = 62, // <Declaration>
            SYMBOL_DECLARATIONS = 63, // <Declarations>
            SYMBOL_DECLARINGPARAMETER = 64, // <DeclaringParameter>
            SYMBOL_DECLARINGPARAMETERS = 65, // <DeclaringParameters>
            SYMBOL_ELSEIFSTATEMENTEXTEND = 66, // <ElseIfStatementExtend>
            SYMBOL_ELSESTATEMENTEXTEND = 67, // <ElseStatementExtend>
            SYMBOL_EXPRESSION = 68, // <Expression>
            SYMBOL_IDENTIFIERS = 69, // <Identifiers>
            SYMBOL_LOGICALOPERATOR = 70, // <logicaloperator>
            SYMBOL_METHODCALL = 71, // <MethodCall>
            SYMBOL_METHODDECLARATION = 72, // <MethodDeclaration>
            SYMBOL_METHODTYPE = 73, // <Methodtype>
            SYMBOL_OPERATOR = 74, // <operator>
            SYMBOL_POINTVALUE = 75, // <PointValue>
            SYMBOL_PREFABCLASSES = 76, // <PrefabClasses>
            SYMBOL_PREFABMETHODCALL = 77, // <PrefabMethodCall>
            SYMBOL_PREFABMETHODS = 78, // <PrefabMethods>
            SYMBOL_PREFIX = 79, // <Prefix>
            SYMBOL_RETURNSTATEMENT = 80, // <ReturnStatement>
            SYMBOL_S = 81, // <S>
            SYMBOL_STATEMENT = 82, // <Statement>
            SYMBOL_TEXT2 = 83, // <Text>
            SYMBOL_TEXTPRIME = 84, // <TextPrime>
            SYMBOL_TYPE = 85, // <Type>
            SYMBOL_VALUE = 86, // <Value>
            SYMBOL_VALUEKEYWORDS = 87, // <ValueKeywords>
            SYMBOL_WRITEMETHOD = 88  // <WriteMethod>
        }
        enum RuleConstants
        {
            RULE_S_VOID_STARTUP_END_STARTUP_VOID_GAMELOOP_END_GAMELOOP = 0, // <S> ::= <Declarations> void startup <Commands> end startup <Declarations> void GameLoop <Commands> end GameLoop <Declarations>
            RULE_COMMANDS = 1, // <Commands> ::= <Statement> <Commands>
            RULE_COMMANDS_SEMI = 2, // <Commands> ::= <Declaration> ';' <Commands>
            RULE_COMMANDS2 = 3, // <Commands> ::= 
            RULE_STATEMENT = 4, // <Statement> ::= <WriteMethod>
            RULE_STATEMENT2 = 5, // <Statement> ::= <Assignment>
            RULE_STATEMENT3 = 6, // <Statement> ::= <ControlStatements>
            RULE_STATEMENT4 = 7, // <Statement> ::= <MethodCall>
            RULE_STATEMENT5 = 8, // <Statement> ::= <PrefabMethodCall>
            RULE_WRITEMETHOD_WRITE_LPAREN_RPAREN_SEMI = 9, // <WriteMethod> ::= write '(' <Text> ')' ';'
            RULE_METHODCALL_CALL_LPAREN_RPAREN_SEMI = 10, // <MethodCall> ::= Call <Identifiers> '(' <CallingParameters> ')' ';'
            RULE_METHODCALL_EQ_CALL_LPAREN_RPAREN_SEMI = 11, // <MethodCall> ::= <Identifiers> '=' Call <Identifiers> '(' <CallingParameters> ')' ';'
            RULE_PREFABMETHODCALL_CALL_LPAREN_RPAREN_SEMI = 12, // <PrefabMethodCall> ::= Call <PrefabMethods> '(' <CallingParameters> ')' ';'
            RULE_ASSIGNMENT_EQ_SEMI = 13, // <Assignment> ::= <Identifiers> '=' <Value> <Expression> ';'
            RULE_CONTROLSTATEMENTS_IF_LPAREN_RPAREN_END_IF = 14, // <ControlStatements> ::= if '(' <BooleanExpression> ')' <Commands> <ElseIfStatementExtend> end if
            RULE_CONTROLSTATEMENTS_WHILE_LPAREN_RPAREN_END_WHILE = 15, // <ControlStatements> ::= while '(' <BooleanExpression> ')' <Commands> end while
            RULE_ELSEIFSTATEMENTEXTEND_ELSEIF_LPAREN_RPAREN = 16, // <ElseIfStatementExtend> ::= 'else if' '(' <BooleanExpression> ')' <Commands> <ElseIfStatementExtend>
            RULE_ELSEIFSTATEMENTEXTEND = 17, // <ElseIfStatementExtend> ::= <ElseStatementExtend>
            RULE_ELSESTATEMENTEXTEND_ELSE = 18, // <ElseStatementExtend> ::= else <Commands>
            RULE_ELSESTATEMENTEXTEND = 19, // <ElseStatementExtend> ::= 
            RULE_DECLARATION_IDENTIFIER = 20, // <Declaration> ::= <Type> Identifier
            RULE_DECLARATIONS_SEMI = 21, // <Declarations> ::= <Declaration> ';' <Declarations>
            RULE_DECLARATIONS = 22, // <Declarations> ::= <MethodDeclaration> <Declarations>
            RULE_DECLARATIONS2 = 23, // <Declarations> ::= 
            RULE_METHODDECLARATION_METHOD_IDENTIFIER_LPAREN_RPAREN_END_METHOD = 24, // <MethodDeclaration> ::= method <Methodtype> Identifier '(' <DeclaringParameters> ')' <Commands> <ReturnStatement> end method
            RULE_RETURNSTATEMENT_RETURN_SEMI = 25, // <ReturnStatement> ::= return <Value> <Expression> ';'
            RULE_RETURNSTATEMENT = 26, // <ReturnStatement> ::= 
            RULE_CALLINGPARAMETERS = 27, // <CallingParameters> ::= <Value> <CallingParameter>
            RULE_CALLINGPARAMETERS2 = 28, // <CallingParameters> ::= 
            RULE_CALLINGPARAMETER_COMMA = 29, // <CallingParameter> ::= ',' <Value> <CallingParameter>
            RULE_CALLINGPARAMETER = 30, // <CallingParameter> ::= 
            RULE_DECLARINGPARAMETERS = 31, // <DeclaringParameters> ::= <Declaration> <DeclaringParameter>
            RULE_DECLARINGPARAMETERS2 = 32, // <DeclaringParameters> ::= 
            RULE_DECLARINGPARAMETER_COMMA = 33, // <DeclaringParameter> ::= ',' <Declaration> <DeclaringParameter>
            RULE_DECLARINGPARAMETER = 34, // <DeclaringParameter> ::= 
            RULE_EXPRESSION = 35, // <Expression> ::= <operator> <Value> <Expression>
            RULE_EXPRESSION2 = 36, // <Expression> ::= 
            RULE_BOOLEANEXPRESSION = 37, // <BooleanExpression> ::= <Value> <Expression> <comparisonoperator> <Value> <Expression>
            RULE_BOOLEANEXPRESSION2 = 38, // <BooleanExpression> ::= <Value> <Expression> <comparisonoperator> <Value> <Expression> <logicaloperator> <Value> <Expression> <comparisonoperator> <Value> <Expression>
            RULE_LOGICALOPERATOR_OR = 39, // <logicaloperator> ::= or
            RULE_LOGICALOPERATOR_AND = 40, // <logicaloperator> ::= and
            RULE_OPERATOR_TIMES = 41, // <operator> ::= '*'
            RULE_OPERATOR_PLUS = 42, // <operator> ::= '+'
            RULE_OPERATOR_DIV = 43, // <operator> ::= '/'
            RULE_OPERATOR_MINUS = 44, // <operator> ::= '-'
            RULE_COMPARISONOPERATOR_ISEQ = 45, // <comparisonoperator> ::= 'is='
            RULE_COMPARISONOPERATOR_ISLTEQ = 46, // <comparisonoperator> ::= 'is<='
            RULE_COMPARISONOPERATOR_ISGTEQ = 47, // <comparisonoperator> ::= 'is>='
            RULE_COMPARISONOPERATOR_ISLT = 48, // <comparisonoperator> ::= 'is<'
            RULE_COMPARISONOPERATOR_ISGT = 49, // <comparisonoperator> ::= 'is>'
            RULE_COMPARISONOPERATOR_ISEXCLAMEQ = 50, // <comparisonoperator> ::= 'is!='
            RULE_COMPARISONOPERATOR_TOUCHES = 51, // <comparisonoperator> ::= touches
            RULE_TEXT_STRINGVALUE = 52, // <Text> ::= StringValue <TextPrime>
            RULE_TEXT = 53, // <Text> ::= <Identifiers> <TextPrime>
            RULE_TEXT2 = 54, // <Text> ::= 
            RULE_TEXTPRIME_PLUS = 55, // <TextPrime> ::= '+' <Identifiers> <TextPrime>
            RULE_TEXTPRIME_PLUS_STRINGVALUE = 56, // <TextPrime> ::= '+' StringValue <TextPrime>
            RULE_TEXTPRIME = 57, // <TextPrime> ::= 
            RULE_TYPE_INTEGER = 58, // <Type> ::= Integer
            RULE_TYPE_DECIMAL = 59, // <Type> ::= Decimal
            RULE_TYPE_STRING = 60, // <Type> ::= String
            RULE_TYPE_BOOLEAN = 61, // <Type> ::= Boolean
            RULE_TYPE_POINT = 62, // <Type> ::= Point
            RULE_TYPE = 63, // <Type> ::= <PrefabClasses>
            RULE_METHODTYPE = 64, // <Methodtype> ::= <Type>
            RULE_METHODTYPE_VOID = 65, // <Methodtype> ::= void
            RULE_VALUE = 66, // <Value> ::= <Identifiers>
            RULE_VALUE_INTEGERVALUE = 67, // <Value> ::= <Prefix> IntegerValue
            RULE_VALUE_DECIMALVALUE = 68, // <Value> ::= <Prefix> DecimalValue
            RULE_VALUE_STRINGVALUE = 69, // <Value> ::= StringValue
            RULE_VALUE2 = 70, // <Value> ::= <BooleanValue>
            RULE_VALUE_LPAREN_COMMA_RPAREN = 71, // <Value> ::= '(' <PointValue> ',' <PointValue> ')'
            RULE_VALUE3 = 72, // <Value> ::= <ValueKeywords>
            RULE_POINTVALUE_DECIMALVALUE = 73, // <PointValue> ::= <Prefix> DecimalValue
            RULE_POINTVALUE = 74, // <PointValue> ::= <Identifiers>
            RULE_VALUEKEYWORDS_TIME = 75, // <ValueKeywords> ::= <Prefix> Time
            RULE_PREFIX_MINUS = 76, // <Prefix> ::= '-'
            RULE_PREFIX = 77, // <Prefix> ::= 
            RULE_BOOLEANVALUE_TRUE = 78, // <BooleanValue> ::= true
            RULE_BOOLEANVALUE_FALSE = 79, // <BooleanValue> ::= false
            RULE_IDENTIFIERS_IDENTIFIER = 80, // <Identifiers> ::= Identifier
            RULE_IDENTIFIERS_IDENTIFIER_DOT_IDENTIFIER = 81, // <Identifiers> ::= Identifier '.' Identifier
            RULE_PREFABCLASSES_CHARACTER = 82, // <PrefabClasses> ::= Character
            RULE_PREFABCLASSES_CAMERA = 83, // <PrefabClasses> ::= Camera
            RULE_PREFABCLASSES_SQUARE = 84, // <PrefabClasses> ::= Square
            RULE_PREFABCLASSES_TRIANGLE = 85, // <PrefabClasses> ::= Triangle
            RULE_PREFABCLASSES_SPRITE = 86, // <PrefabClasses> ::= Sprite
            RULE_PREFABCLASSES_TEXT = 87, // <PrefabClasses> ::= Text
            RULE_PREFABCLASSES_TRIGGER = 88, // <PrefabClasses> ::= Trigger
            RULE_PREFABMETHODS_DELETE = 89  // <PrefabMethods> ::= Delete
        }
        #endregion
    }
}