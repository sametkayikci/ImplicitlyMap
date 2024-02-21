namespace ImplicitlyMap.Config;

public record ConversionConfig(
    string ModelDirectory,
    HashSet<string> TargetTypeSuffixes,
    string UsingNamespace);

