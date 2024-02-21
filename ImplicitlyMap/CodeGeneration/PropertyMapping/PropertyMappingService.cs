using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace ImplicitlyMap.CodeGeneration.PropertyMapping;

public class PropertyMappingService : IPropertyMappingService
{
    public IEnumerable<PropertyDeclarationSyntax> GetProperties(TypeDeclarationSyntax typeDeclaration)
    {
        return typeDeclaration.Members.OfType<PropertyDeclarationSyntax>();
    }

    public IEnumerable<AssignmentExpressionSyntax> CreateAssignments(IEnumerable<PropertyDeclarationSyntax> properties)
    {
        return properties.Select(p =>
            SyntaxFactory.AssignmentExpression(
                SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName(p.Identifier),
                SyntaxFactory.MemberAccessExpression(
                    SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("model"),
                    SyntaxFactory.IdentifierName(p.Identifier))));
    }

    public ObjectCreationExpressionSyntax CreateObjectCreation(IEnumerable<AssignmentExpressionSyntax> assignments,
        string targetTypeName)
    {
        return SyntaxFactory
            .ObjectCreationExpression(SyntaxFactory.ParseTypeName(targetTypeName))
            .WithInitializer(SyntaxFactory.InitializerExpression(ObjectInitializerExpression,
                SyntaxFactory.SeparatedList<ExpressionSyntax>(assignments)));
    }

    public ObjectCreationExpressionSyntax CreateObjectCreationForClass(
        ClassDeclarationSyntax classDeclaration,
        IEnumerable<PropertyDeclarationSyntax> properties,
        string targetTypeName)
    {
        var constructor = classDeclaration.Members
            .OfType<ConstructorDeclarationSyntax>()
            .FirstOrDefault(c => c.ParameterList.Parameters.Count == properties.Count());

        if (constructor is not null)
        {
            var arguments = constructor.ParameterList.Parameters.Select(parameter =>
                SyntaxFactory.Argument(
                    SyntaxFactory.MemberAccessExpression(
                        SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("model"),
                        GetMatchingPropertyIdentifier(properties, parameter.Identifier.ValueText))));

            return SyntaxFactory
                .ObjectCreationExpression(SyntaxFactory.ParseTypeName(targetTypeName))
                .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments)));
        }

        var assignments = CreateAssignments(properties);
        return CreateObjectCreation(assignments, targetTypeName);
    }


    public ObjectCreationExpressionSyntax CreateObjectCreationForRecord(RecordDeclarationSyntax recordDeclaration,
        IEnumerable<PropertyDeclarationSyntax> properties, string targetTypeName)
    {
        var parameterList = recordDeclaration.ParameterList;
        if (parameterList is not null)
        {
            // Parametreli constructor için argüman listesi oluştur
            var arguments = parameterList.Parameters.Select(p =>
                SyntaxFactory.Argument(
                    SyntaxFactory.MemberAccessExpression(
                        SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("model"),
                        SyntaxFactory.IdentifierName(p.Identifier))));

            return SyntaxFactory
                .ObjectCreationExpression(SyntaxFactory.ParseTypeName(targetTypeName))
                .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments)));
        }

        // Parametresiz constructor için assignment'ları kullanarak object creation oluştur
        var assignments = CreateAssignments(properties);
        return CreateObjectCreation(assignments, targetTypeName);
    }

    private static IdentifierNameSyntax GetMatchingPropertyIdentifier(IEnumerable<PropertyDeclarationSyntax> properties,
        string parameterName)
    {
        var property = properties.FirstOrDefault(p =>
            p.Identifier.ValueText.Equals(parameterName, StringComparison.OrdinalIgnoreCase));
        return property != null
            ? SyntaxFactory.IdentifierName(property.Identifier)
            : SyntaxFactory.IdentifierName(parameterName);
    }
}