using ImplicitlyMap.Config;

namespace ImplicitlyMap.CodeGeneration;

public interface IAutoImplicitOperatorGenerator
{
    IAutoImplicitOperatorGenerator Configure(Action<ConversionConfigBuilder> configure);
    IAutoImplicitOperatorGenerator GenerateImplicitOperators();
}