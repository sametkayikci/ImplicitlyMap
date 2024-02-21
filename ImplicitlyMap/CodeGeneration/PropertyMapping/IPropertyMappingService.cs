using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImplicitlyMap.CodeGeneration.PropertyMapping;

public interface IPropertyMappingService
{
    IEnumerable<PropertyDeclarationSyntax> GetProperties(TypeDeclarationSyntax typeDeclaration);
    IEnumerable<AssignmentExpressionSyntax> CreateAssignments(IEnumerable<PropertyDeclarationSyntax> properties);

    ObjectCreationExpressionSyntax CreateObjectCreation(IEnumerable<AssignmentExpressionSyntax> assignments,
        string targetTypeName);

    ObjectCreationExpressionSyntax CreateObjectCreationForClass(
        ClassDeclarationSyntax classDeclaration,
        IEnumerable<PropertyDeclarationSyntax> properties,
        string targetTypeName);

    ObjectCreationExpressionSyntax CreateObjectCreationForRecord(
        RecordDeclarationSyntax recordDeclaration,
        IEnumerable<PropertyDeclarationSyntax> properties,
        string targetTypeName);
}