using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImplicitlyMap.CodeGeneration.TypeMatching;

public class TypeMatchingService : ITypeMatchingService
{
    public bool IsMatchingType(TypeDeclarationSyntax typeDeclaration, string sourceTypeName)
    {
        return string.Equals(typeDeclaration.Identifier.Text, sourceTypeName, StringComparison.Ordinal);
    }
}