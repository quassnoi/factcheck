namespace FactCheck;

internal readonly record struct CodeFixData(string Title, string EquivalenceKey);

internal static class CodeFixes
{
    public static readonly CodeFixData FactCheck001XunitDisplayNameMissing = new(
        "Generate DisplayName attibute from method name",
        nameof(FactCheck001XunitDisplayNameMissing));

}