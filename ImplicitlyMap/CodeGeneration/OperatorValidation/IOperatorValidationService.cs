using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImplicitlyMap.CodeGeneration.OperatorValidation;

public interface IOperatorValidationService
{
    bool HasImplicitOperator(TypeDeclarationSyntax typeDeclaration);
}