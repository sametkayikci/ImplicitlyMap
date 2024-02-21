using ImplicitlyMap.CodeGeneration.OperatorCreation;
using ImplicitlyMap.CodeGeneration.OperatorValidation;
using ImplicitlyMap.CodeGeneration.PropertyMapping;
using ImplicitlyMap.CodeGeneration.TypeMatching;
using ImplicitlyMap.Config;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ImplicitlyMap.CodeGeneration;

public sealed class ImplicitOperatorGenerator(
    string sourceTypeName,
    string targetTypeName,
    ConversionConfig config,
    ITypeMatchingService typeMatchingService,
    IOperatorValidationService operatorDetectionService,
    IPropertyMappingService propertyMappingService,
    IOperatorCreationService operatorCreationService)
    : CSharpSyntaxRewriter
{
    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        if (!typeMatchingService.IsMatchingType(node, sourceTypeName)) return base.VisitClassDeclaration(node);
        if (operatorDetectionService.HasImplicitOperator(node)) return base.VisitClassDeclaration(node);

        var properties = propertyMappingService.GetProperties(node);
        var objectCreation = propertyMappingService.CreateObjectCreationForClass(node, properties, targetTypeName);
        var implicitOperator =
            operatorCreationService.CreateImplicitOperator(objectCreation, sourceTypeName, targetTypeName, config);

        var updatedNode = node.AddMembers(implicitOperator)
            .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
            .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken))
            .NormalizeWhitespace();

        return updatedNode;
    }

    public override SyntaxNode? VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        if (!typeMatchingService.IsMatchingType(node, sourceTypeName)) return base.VisitRecordDeclaration(node);
        if (operatorDetectionService.HasImplicitOperator(node)) return base.VisitRecordDeclaration(node);

        var properties = propertyMappingService.GetProperties(node);
        var objectCreation = propertyMappingService.CreateObjectCreationForRecord(node, properties, targetTypeName);
        var implicitOperator =
            operatorCreationService.CreateImplicitOperator(objectCreation, sourceTypeName, targetTypeName, config);

        var updatedNode = node.AddMembers(implicitOperator)
            .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
            .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken))
            .NormalizeWhitespace();

        return updatedNode;
    }

    public override SyntaxNode? VisitCompilationUnit(CompilationUnitSyntax node)
    {
        var updatedNode = base.VisitCompilationUnit(node);

        if (string.IsNullOrWhiteSpace(config.UsingNamespace)) return updatedNode;

        var usingDirective = UsingDirective(ParseName(config.UsingNamespace))
            .NormalizeWhitespace();
        updatedNode = (updatedNode as CompilationUnitSyntax)?.AddUsings(usingDirective);

        return updatedNode;
    }
}