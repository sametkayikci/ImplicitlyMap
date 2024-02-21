using ImplicitlyMap.Config;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImplicitlyMap.CodeGeneration.OperatorCreation;

public interface IOperatorCreationService
{
    ConversionOperatorDeclarationSyntax CreateImplicitOperator(
        ExpressionSyntax objectCreation,
        string sourceTypeName,
        string targetTypeName,
        ConversionConfig config);
}