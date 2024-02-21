using ImplicitlyMap.Config;
using ImplicitlyMap.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImplicitlyMap.CodeGeneration.OperatorCreation;

public class OperatorCreationService : IOperatorCreationService
{
    public ConversionOperatorDeclarationSyntax CreateImplicitOperator(
        ExpressionSyntax objectCreation,
        string sourceTypeName,
        string targetTypeName,
        ConversionConfig config)
    {
        var typeName = sourceTypeName.RemoveSuffixes(config.TargetTypeSuffixes.ToArray());

        return SyntaxFactory.ConversionOperatorDeclaration(
                SyntaxFactory.Token(SyntaxKind.ImplicitKeyword),
                SyntaxFactory.ParseTypeName(targetTypeName))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword))
            .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("model"))
                    .WithType(SyntaxFactory.ParseTypeName(typeName)))))
            .WithBody(SyntaxFactory.Block(SyntaxFactory.ReturnStatement(objectCreation)));
    }
}