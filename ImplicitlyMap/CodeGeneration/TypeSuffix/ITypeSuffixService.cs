using ImplicitlyMap.Config;

namespace ImplicitlyMap.CodeGeneration.TypeSuffix;

public interface ITypeSuffixService
{
    bool IsTargetTypeSuffixes(string typeName, ConversionConfig config);
}
