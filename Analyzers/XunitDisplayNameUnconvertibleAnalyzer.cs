using System.Collections.Immutable;
using FactCheck.Common;
using FactCheck.Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FactCheck;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class XunitDisplayNameUnconvertibleAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
        Diagnostics.FactCheck0003XunitDisplayNameUnconvertible
    );

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSemanticModelAction(Handle);
    }

    private static void Handle(SemanticModelAnalysisContext context)
    {
        var semanticModel = context.SemanticModel;
        if (!semanticModel.HasXunit())
        {
            return;
        }

        var attributeSyntaxes = semanticModel
            .GetAttributesSupportingDisplayName()
            .Where(attributeSyntax => attributeSyntax.HasDisplayName());

        foreach (var (literalExpressionSyntax, displayName) in Unconvertible(attributeSyntaxes))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Diagnostics.FactCheck0003XunitDisplayNameUnconvertible,
                    literalExpressionSyntax.GetLocation(),
                    displayName
                )
            );
        }
    }

    private static IEnumerable<(LiteralExpressionSyntax literalExpressionSyntax, string? displayName)> Unconvertible(IEnumerable<AttributeSyntax> attributeSyntaxes)
    {
        foreach (var attributeSyntax in attributeSyntaxes)
        {
            if (attributeSyntax.GetDisplayNameExpression() is LiteralExpressionSyntax literalExpressionSyntax)
            {
                var displayName = literalExpressionSyntax.GetDisplayName();
                if (displayName == null || Converters.TextToCode(displayName) is null)
                {
                    yield return (literalExpressionSyntax, displayName);
                }
            }
        }
    }
}