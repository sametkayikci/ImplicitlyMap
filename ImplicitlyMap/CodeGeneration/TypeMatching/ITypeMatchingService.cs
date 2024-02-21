using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImplicitlyMap.CodeGeneration.TypeMatching;

public interface ITypeMatchingService
{
    bool IsMatchingType(TypeDeclarationSyntax typeDeclaration, string sourceTypeName);
}
