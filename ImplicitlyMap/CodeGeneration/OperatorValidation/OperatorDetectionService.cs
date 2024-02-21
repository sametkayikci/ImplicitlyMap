using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImplicitlyMap.CodeGeneration.OperatorValidation;

public class OperatorValidationService : IOperatorValidationService
{
    public bool HasImplicitOperator(TypeDeclarationSyntax typeDeclaration)
    {
        return typeDeclaration.Members
            .OfType<ConversionOperatorDeclarationSyntax>()
            .Any(conversionOperator =>
                conversionOperator.ImplicitOrExplicitKeyword.IsKind(SyntaxKind.ImplicitKeyword));
    }
}