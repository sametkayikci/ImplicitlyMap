using ImplicitlyMap.Config;

namespace ImplicitlyMap.CodeGeneration.TypeSuffix;

public class TypeSuffixService : ITypeSuffixService
{
    public bool IsTargetTypeSuffixes(string typeName, ConversionConfig config)
    {
        return config.TargetTypeSuffixes.Any(suffix => typeName.EndsWith(suffix, StringComparison.Ordinal));
    }
}