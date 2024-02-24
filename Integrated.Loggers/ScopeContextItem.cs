namespace Integrated.Loggers;

public class ScopeContextItem
{
    public string Name { get; init; }

    public string Value { get; init; }

    public ScopeContextItem(string name, string value)
    {
        Name = name;
        Value = value;
    }
}