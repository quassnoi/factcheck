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
    public static readonly DiagnosticDescriptor FactCheck001XunitDisplayNameMissing = Factory(
        "FACTCHECK0001",
        "DisplayName missing",
        "DisplayName missing",
        "XUnit",
        DiagnosticSeverity.Warning,
        true,
        "Fact attribute should have a DisplayName argument."
    );
}