using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Compiler.Exceptions;
using Compiler.Nodes;
using GoldParser;

namespace Compiler
{
    public partial class DelevopmentEnvironment : Form
    {
        private Grammar _grammar;
        private StartupStucture _rootAstNode;
        private ParserContext _parserContext;
        public static DelevopmentEnvironment Formtest;

        public DelevopmentEnvironment()
        {
            InitializeComponent();
            using (Stream stream = File.OpenRead("EndeligeGrammar.cgt"))
            {
                BinaryReader reader = new BinaryReader(stream);
                _grammar = new Grammar(reader);
            }
            Formtest = this;
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new DelevopmentEnvironment());
        }

        private bool Parse()
        {
            StringReader reader = new StringReader(InputProgram.Text);
            Parser parser = new Parser(reader, _grammar);
            parser.TrimReductions = true;
            _parserContext = new ParserContext(parser);
            while (true)
            {
                switch (parser.Parse())
                {
                    case ParseMessage.LexicalError:
                        Log("LEXICAL ERROR. Line " + parser.LineNumber + ". Cannot recognize token: " + parser.TokenText);
                        return false;

                    case ParseMessage.SyntaxError:
                        StringBuilder text = new StringBuilder();
                        foreach (Symbol tokenSymbol in parser.GetExpectedTokens())
                        {
                            text.Append(' ');
                            text.Append(tokenSymbol);
                        }
                        Log("SYNTAX ERROR. Line " + parser.LineNumber + ". Expecting:" + text);
                        return false;

                    case ParseMessage.Reduction:
                        parser.TokenSyntaxNode = _parserContext.CreateNode();
                        break;

                    case ParseMessage.Accept:
                        _rootAstNode = parser.TokenSyntaxNode as StartupStucture;
                        return true;

                    case ParseMessage.TokenRead:
                        parser.TokenSyntaxNode = _parserContext.GetTokenText();
                        break;

                    case ParseMessage.InternalError:
                        Log("Something is horribly wrong");
                        return false;

                    case ParseMessage.NotLoadedError:
                        Log("Grammar Table is not loaded.");
                        return false;

                    case ParseMessage.CommentError:
                        Log("Unexpected end of input.");
                        return false;

                    case ParseMessage.CommentBlockRead:
                    case ParseMessage.CommentLineRead:
                        break;
                }
            }
        }

        public void Log(string text)
        {
            ErrorDisplay.Items.Add(text);
        }

        private void Run_Click(object sender, EventArgs e)
        {
            ErrorDisplay.Items.Clear();
            if (Parse())
            {
                try
                {
                    _rootAstNode.Accept(new ContextVisitor());
                    ErrorDisplay.Items.Add("Type/scope check completed");
                    string codeString = (string)_rootAstNode.Accept(new CodeGenVisitor());

                    System.Windows.Forms.MessageBox.Show("Select The UnityProject Folder.");

                    using (var fbd = new FolderBrowserDialog())
                    {
                        fbd.ShowDialog();
                        projectpath = fbd.SelectedPath;
                    }

                    // File Setup
                    if (File.Exists(projectpath + "\\Assets\\Resources\\Scripts\\CompiledScript.cs"))
                        File.WriteAllText(projectpath + "\\Assets\\Resources\\Scripts\\CompiledScript.cs", String.Empty);

                    StreamWriter file = new StreamWriter(projectpath + "\\Assets\\Resources\\Scripts\\CompiledScript.cs", true);
                    file.Write(codeString);
                    file.Close();

                    ErrorDisplay.Items.Add("CodeGen completed");
                    BuildButton.Enabled = true;
                }
                catch (Exception ex) when (ex is IdentifierNotFound || 
                                            ex is IdentifierAlreadyExists || 
                                            ex is NotCompatibleTypes || 
                                            ex is NotUsableWithOperator || 
                                            ex is ParameterDifference)
                {

                }
            }
        }

        static string path;
        static string projectpath;

        private void BuildButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Select Unity Installation Folder");
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                path = fbd.SelectedPath;
            }

            string cmdCommand = "\"" + path + "\\Editor\\Unity.exe" + "\" -batchmode -quit -projectPath \"" +
                                projectpath + "\" -executeMethod EditorScript.PerformBuild";

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(cmdCommand);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());

            System.Windows.Forms.MessageBox.Show("Build Finish");
        }

        private void InputProgram_TextChanged(object sender, EventArgs e)
        {
            BuildButton.Enabled = false;
        }
    }
}
