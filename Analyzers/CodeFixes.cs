namespace FactCheck;

internal readonly record struct CodeFixData(string Title, string EquivalenceKey);

internal static class CodeFixes
{
    public static readonly CodeFixData FactCheck0001XunitDisplayNameMissing = new(
        "Generate DisplayName attibute from method name",
        nameof(FactCheck0001XunitDisplayNameMissing));

    public static readonly CodeFixData FactCheck0002XunitDisplayNameMismatch = new(
        "Fix method name to match DisplayName attribute",
        nameof(FactCheck0002XunitDisplayNameMismatch));
}