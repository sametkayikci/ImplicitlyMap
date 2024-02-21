namespace ImplicitlyMap.Config;

public record ConversionConfigBuilder
{
    private string _usingNamespace = null!;
    private string _modelDirectory = null!;
    private readonly HashSet<string> _targetTypeSuffixes = [];

    public ConversionConfigBuilder WithModelDirectory(string modelDirectory)
    {
        _modelDirectory = modelDirectory;
        return this;
    }

    public ConversionConfigBuilder AddTargetTypeSuffix(string suffix)
    {
        _targetTypeSuffixes.Add(suffix);
        return this;
    }

    public ConversionConfigBuilder WithUsingNamespace(string usingNamespace)
    {
        _usingNamespace = usingNamespace;
        return this;
    }

    public ConversionConfig Build()
    {
        if (string.IsNullOrWhiteSpace(_modelDirectory))
            throw new InvalidOperationException("ModelDirectory is required.");

        if (_targetTypeSuffixes is not { Count: not 0 })
            throw new InvalidOperationException("At least one TargetTypeSuffix is required.");

        if (string.IsNullOrWhiteSpace(_usingNamespace))
            throw new InvalidOperationException("UsingNamespace is required.");

        return new ConversionConfig(_modelDirectory, _targetTypeSuffixes, _usingNamespace);
    }
}