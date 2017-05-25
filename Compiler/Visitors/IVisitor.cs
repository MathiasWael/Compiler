using Compiler.Nodes;

namespace Compiler
{
    public interface IVisitor
    {
        object Visit(Assignment obj);
        object Visit(AssignMethodCall obj);
        object Visit(ASTNode obj);
        object Visit(BooleanExpression obj);
        object Visit(Commands obj);
        object Visit(CommandsDeclaration obj);
        object Visit(Declaration obj);
        object Visit(Declarations obj);
        object Visit(ElseIfStatement obj);
        object Visit(ElseStatement obj);
        object Visit(Expression obj);
        object Visit(IfStatement obj);
        object Visit(IfStatementExtend obj);
        object Visit(MethodCall obj);
        object Visit(MethodDeclaration obj);
        object Visit(PrefabMethodCall obj);
        object Visit(StartupStucture obj);
        object Visit(Statement obj);
        object Visit(WhileStatement obj);
        object Visit(Write obj);
    }
}
