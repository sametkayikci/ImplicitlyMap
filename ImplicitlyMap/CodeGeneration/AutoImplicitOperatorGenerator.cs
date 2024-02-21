using ImplicitlyMap.CodeGeneration.OperatorCreation;
using ImplicitlyMap.CodeGeneration.OperatorValidation;
using ImplicitlyMap.CodeGeneration.PropertyMapping;
using ImplicitlyMap.CodeGeneration.TypeMatching;
using ImplicitlyMap.CodeGeneration.TypeSuffix;
using ImplicitlyMap.Config;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace ImplicitlyMap.CodeGeneration;

public sealed class AutoImplicitOperatorGenerator : IAutoImplicitOperatorGenerator
{
    private ConversionConfig _config = null!;

    private readonly ITypeMatchingService _typeMatchingService;
    private readonly ITypeSuffixService _typeSuffixService;
    private readonly IOperatorValidationService _operatorDetectionService;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IOperatorCreationService _operatorCreationService;

    public AutoImplicitOperatorGenerator(
        ITypeMatchingService typeMatchingService,
        IOperatorValidationService operatorDetectionService,
        IPropertyMappingService propertyMappingService,
        IOperatorCreationService operatorCreationService, ITypeSuffixService typeSuffixService)
    {
        _typeMatchingService = typeMatchingService;
        _operatorDetectionService = operatorDetectionService;
        _propertyMappingService = propertyMappingService;
        _operatorCreationService = operatorCreationService;
        _typeSuffixService = typeSuffixService;
    }

    public IAutoImplicitOperatorGenerator Configure(Action<ConversionConfigBuilder> configure)
    {
        var builder = new ConversionConfigBuilder();
        configure(builder);
        _config = builder.Build();
        return this;
    }

    public IAutoImplicitOperatorGenerator GenerateImplicitOperators()
    {
        var filePaths = Directory.GetFiles(_config.ModelDirectory, "*.cs", SearchOption.AllDirectories);

        foreach (var filePath in filePaths)
        {
            try
            {
                var sourceCode = File.ReadAllText(filePath);
                var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
                var root = syntaxTree.GetRoot();

                var typeDeclarations = root.DescendantNodes().OfType<TypeDeclarationSyntax>();

                foreach (var declaration in typeDeclarations)
                {
                    var typeName = declaration.Identifier.Text;

                    if (!_typeSuffixService.IsTargetTypeSuffixes(typeName, _config) || 
                        _operatorDetectionService.HasImplicitOperator(declaration)) continue;

                    var generator = new ImplicitOperatorGenerator(
                        typeName, typeName, _config, 
                        _typeMatchingService, _operatorDetectionService, _propertyMappingService, _operatorCreationService);
                    
                    var newSyntaxRoot = generator.Visit(syntaxTree.GetRoot());
                    var formattedCode = Formatter.Format(newSyntaxRoot, new AdhocWorkspace());
                    
                    File.WriteAllText(filePath, formattedCode.ToFullString());
                }

                Console.WriteLine($"File processed: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        return this;
    }
}
