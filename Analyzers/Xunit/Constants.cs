namespace FactCheck.Xunit;

internal static class Constants
{
    public const string AssemblyXunitCore = "xunit.core";
    private const string AttributeFact = "FactAttribute";
    private const string AttributeTheory = "TheoryAttribute";
    public const string NamespaceAttributes = "Xunit";
    public const string PropertyDisplayName = "DisplayName";
    public static readonly HashSet<string> DisplayNameAttributes = new()
    {
        AttributeFact,
        AttributeTheory
    };
}