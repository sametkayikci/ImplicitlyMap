using ImplicitlyMap.CodeGeneration.OperatorCreation;
using ImplicitlyMap.CodeGeneration.OperatorValidation;
using ImplicitlyMap.CodeGeneration;
using ImplicitlyMap.CodeGeneration.PropertyMapping;
using ImplicitlyMap.CodeGeneration.TypeMatching;
using ImplicitlyMap.CodeGeneration.TypeSuffix;

namespace ImplicitlyMap.Factories;

public static class AutoImplicitOperatorGeneratorFactory
{
    public static AutoImplicitOperatorGenerator Create()
    {       
        ITypeMatchingService typeMatchingService = new TypeMatchingService();
        IOperatorValidationService operatorDetectionService = new OperatorValidationService();
        IPropertyMappingService propertyMappingService = new PropertyMappingService();
        IOperatorCreationService operatorCreationService = new OperatorCreationService();
        ITypeSuffixService typeSuffixService = new TypeSuffixService();
        return new AutoImplicitOperatorGenerator(
            typeMatchingService, operatorDetectionService,
            propertyMappingService, operatorCreationService, typeSuffixService);
    }
}