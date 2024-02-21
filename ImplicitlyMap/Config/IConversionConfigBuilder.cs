namespace ImplicitlyMap.Config;

public interface IConversionConfigBuilder
{
    IConversionConfigBuilder WithModelDirectory(string modelDirectory);
    IConversionConfigBuilder AddTargetTypeSuffix(string suffix);
    IConversionConfigBuilder WithUsingNamespace(string usingNamespace);
    ConversionConfig Build();
}