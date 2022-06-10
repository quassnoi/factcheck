using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FactCheck;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class XunitDisplayNameMissingAnalyzer : DiagnosticAnalyzer
{
    private const string _assemblyXunitCore = "xunit.core";
    private const string _attributeFact = "FactAttribute";
    private const string _namespaceAttributeFact = "Xunit";
    private const string _propertyDisplayName = "DisplayName";
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
        Diagnostics.FactCheck001XunitDisplayNameMissing
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
        if (!semanticModel.Compilation.ReferencedAssemblyNames.Any(assemblyIdentity => assemblyIdentity.Name == _assemblyXunitCore))
        {
            return;
        }
        var attributeSyntaxes = semanticModel
            .SyntaxTree
            .GetRoot()
            .DescendantNodes().OfType<AttributeSyntax>()
            .Where(attributeSyntax => IsXunitFactAttribute(attributeSyntax, semanticModel))
            .Where(attributeSyntax => !HasDisplayName(attributeSyntax));

        foreach (var attributeSyntax in attributeSyntaxes)
        {
            context.ReportDiagnostic(Diagnostic.Create(Diagnostics.FactCheck001XunitDisplayNameMissing, attributeSyntax.GetLocation()));
        }
    }

    private static bool HasDisplayName(AttributeSyntax attributeSyntax)
    => attributeSyntax
        .ArgumentList?
        .Arguments
        .Select(argument => argument.NameEquals).OfType<NameEqualsSyntax>()
        .Where(nameEqualsSyntax => nameEqualsSyntax.Name.Identifier.Text == _propertyDisplayName)
        .Any() ?? false;

    private static bool IsXunitFactAttribute(AttributeSyntax attributeSyntax, SemanticModel semanticModel)
    {
        if (semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol methodSymbol)
        {
            return false;
        }
        var containingType = methodSymbol.ContainingType;
        return containingType is not null
            && methodSymbol.MethodKind == MethodKind.Constructor
            && containingType.ContainingAssembly.Name == _assemblyXunitCore
            && containingType.Name == _attributeFact
            && containingType.ContainingNamespace.Name == _namespaceAttributeFact;
    }
}