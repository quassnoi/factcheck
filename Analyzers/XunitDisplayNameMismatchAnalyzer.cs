using System.Collections.Immutable;
using FactCheck.Common;
using FactCheck.Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FactCheck;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class XunitDisplayNameMismatchAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
        Diagnostics.FactCheck0002XunitDisplayNameMismatch
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

        foreach (var (methodIdentifier, displayName, methodName) in Mismatches(attributeSyntaxes))
        {
            context.ReportDiagnostic(Diagnostic.Create(Diagnostics.FactCheck0002XunitDisplayNameMismatch,
                methodIdentifier.GetLocation(),
                displayName,
                methodName
            ));
        }
    }

    private static IEnumerable<(SyntaxToken syntaxToken, string displayName, string methodName)> Mismatches(IEnumerable<AttributeSyntax> attributeSyntaxes)
    {
        foreach (var attributeSyntax in attributeSyntaxes)
        {
            if (attributeSyntax.GetDisplayName() is string displayName
                && attributeSyntax.GetMethodIdentifier() is SyntaxToken methodIdentifier
                && !Converters.TextEqualsCode(displayName, methodIdentifier.Text))
            {
                yield return (methodIdentifier, displayName, methodIdentifier.Text);
            }
        }
    }
}