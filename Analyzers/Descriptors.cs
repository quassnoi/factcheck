using Microsoft.CodeAnalysis;

namespace FactCheck;

internal static class Diagnostics
{
    public static readonly DiagnosticDescriptor FactCheck0001XunitDisplayNameMissing = Factory(
        "FACTCHECK0001",
        "DisplayName missing",
        "DisplayName attribute is missing",
        "XUnit",
        DiagnosticSeverity.Warning,
        true,
        "[Fact] and [Theory] attributes should have a DisplayName argument."
    );

    public static readonly DiagnosticDescriptor FactCheck0002XunitDisplayNameMismatch = Factory(
        "FACTCHECK0002",
        "DisplayName mismatch",
        "DisplayName \"{0}\" does not match method name {1}",
        "XUnit",
        DiagnosticSeverity.Warning,
        true,
        "The test method name does not match the DisplayName argument."
    );

    public static readonly DiagnosticDescriptor FactCheck0003XunitDisplayNameUnconvertible = Factory(
        "FACTCHECK0003",
        "DisplayName cannot be converted to a method name",
        "DisplayName",
        "XUnit",
        DiagnosticSeverity.Warning,
        true,
        "DisplayName contains characters that cannot be converted to a valid C# method name identifier."
    );

    private static DiagnosticDescriptor Factory(
        string id,
        string title,
        string messageFormat,
        string category,
        DiagnosticSeverity defaultSeverity,
        bool isEnabledByDefault,
        string description)
        => new(
            id,
            title,
            messageFormat,
            category,
            defaultSeverity,
            isEnabledByDefault,
            description);
}