using Microsoft.CodeAnalysis;

namespace FactCheck;

internal static class Diagnostics
{
    public static readonly DiagnosticDescriptor FactCheck0001XunitDisplayNameMissing = Factory(
        "FACTCHECK0001",
        "DisplayName missing",
        "DisplayName attribute is missing",
        "XUnit",
        DiagnosticSeverity.Info,
        true,
        "[Fact] and [Theory] attributes should have a DisplayName argument."
    );

    public static readonly DiagnosticDescriptor FactCheck0002XunitDisplayNameMismatch = Factory(
        "FACTCHECK0002",
        "DisplayName mismatch",
        "DisplayName \"{0}\" does not match method name {1}",
        "XUnit",
        DiagnosticSeverity.Info,
        true,
        "The test method name does not match the value of DisplayName."
    );

    public static readonly DiagnosticDescriptor FactCheck0003XunitDisplayNameUnconvertible = Factory(
        "FACTCHECK0003",
        "DisplayName unconvertible",
        "DisplayName \"{0}\" cannot be converted to a valid identifier",
        "XUnit",
        DiagnosticSeverity.Info,
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