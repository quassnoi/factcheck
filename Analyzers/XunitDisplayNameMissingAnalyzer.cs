using System.Collections.Immutable;
using FactCheck.Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FactCheck;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class XunitDisplayNameMissingAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
        Diagnostics.FactCheck0001XunitDisplayNameMissing
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
            .SyntaxTree
            .GetRoot()
            .DescendantNodes().OfType<AttributeSyntax>()
            .Where(attributeSyntax => attributeSyntax.IsAttributeWithDisplayName(semanticModel))
            .Where(attributeSyntax => !attributeSyntax.HasDisplayName());

        foreach (var attributeSyntax in attributeSyntaxes)
        {
            context.ReportDiagnostic(Diagnostic.Create(Diagnostics.FactCheck0001XunitDisplayNameMissing, attributeSyntax.GetLocation()));
        }
    }

}