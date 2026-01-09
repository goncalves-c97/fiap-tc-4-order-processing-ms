using System;

namespace Test.Helpers;

internal sealed class EnvVarScope : IDisposable
{
    private readonly string _name;
    private readonly string? _oldValue;

    public EnvVarScope(string name, string? value)
    {
        _name = name;
        _oldValue = Environment.GetEnvironmentVariable(name);
        Environment.SetEnvironmentVariable(name, value);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable(_name, _oldValue);
    }
}
