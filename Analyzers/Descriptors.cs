using Microsoft.CodeAnalysis;

namespace FactCheck;

internal static class Diagnostics
{
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
    public static readonly DiagnosticDescriptor FactCheck0001XunitDisplayNameMissing = Factory(
        "FACTCHECK0001",
        "DisplayName missing",
        "DisplayName missing",
        "XUnit",
        DiagnosticSeverity.Warning,
        true,
        "[Fact] and [Theory] attributes should have a DisplayName argument."
    );

    public static readonly DiagnosticDescriptor FactCheck0002XunitDisplayNameMismatch = Factory(
        "FACTCHECK0002",
        "The method name does not match DisplayName",
        "The method name does not match DisplayName",
        "XUnit",
        DiagnosticSeverity.Warning,
        true,
        "The test method name does not match the DisplayName argument."
    );
}