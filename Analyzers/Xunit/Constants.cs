namespace FactCheck.Xunit;

internal class Constants
{
    public const string AssemblyXunitCore = "xunit.core";
    public const string AttributeFact = "FactAttribute";
    public const string AttributeTheory = "TheoryAttribute";
    public const string NamespaceAttributes = "Xunit";
    public const string PropertyDisplayName = "DisplayName";
    public static readonly HashSet<string> DisplayNameAttributes = new()
    {
        AttributeFact,
        AttributeTheory
    };
}