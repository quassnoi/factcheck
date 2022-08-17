using Microsoft.CodeAnalysis;

namespace FactCheck;

internal readonly record struct CodeFixData(string Title, string EquivalenceKey);

internal static class CodeFixes
{
    public static readonly CodeFixData FactCheck0001XunitDisplayNameMissing = new(
        "Add (DisplayName = \"{0}\")",
        nameof(FactCheck0001XunitDisplayNameMissing));

    public static readonly CodeFixData FactCheck0002XunitDisplayNameMismatch = new(
        "Rename to {0}",
        nameof(FactCheck0002XunitDisplayNameMismatch));
}