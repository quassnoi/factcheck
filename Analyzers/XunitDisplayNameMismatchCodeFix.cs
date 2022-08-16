using System.Collections.Immutable;
using FactCheck.Common;
using FactCheck.Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FactCheck;

internal class XunitDisplayNameMismatchCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
        Diagnostics.FactCheck0002XunitDisplayNameMismatch.Id
    );

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostics = context.Diagnostics.Where(diagnostic => diagnostic.Id == Diagnostics.FactCheck0002XunitDisplayNameMismatch.Id).ToImmutableList();

        if (diagnostics.Count == 0)
        {
            return;
        }

        var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        if (syntaxRoot == null)
        {
            return;
        }

        foreach (var diagnostic in diagnostics)
        {
            RegisterCodeFix(context, syntaxRoot, diagnostic);
        }
    }

    private static void RegisterCodeFix(CodeFixContext context, SyntaxNode syntaxRoot, Diagnostic diagnostic)
    {
        var diagnosticNode = syntaxRoot.FindNode(diagnostic.Location.SourceSpan);
        var identifierToken = syntaxRoot.FindToken(diagnostic.Location.SourceSpan.Start);

        if (diagnosticNode is not MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixes.FactCheck0002XunitDisplayNameMismatch.Title,
                _ => CreateFixedDocument(context.Document, syntaxRoot, methodDeclarationSyntax, identifierToken),
                CodeFixes.FactCheck0001XunitDisplayNameMissing.EquivalenceKey),
            diagnostic);
    }

    private static Task<Document> CreateFixedDocument(Document document, SyntaxNode syntaxRoot, MethodDeclarationSyntax methodDeclarationSyntax, SyntaxToken identifierToken)
    {
        var displayNameSyntax = methodDeclarationSyntax
            .AttributeLists
            .SelectMany(attributeList => attributeList.Attributes)
            .FirstOrDefault();
        if (displayNameSyntax == null)
        {
            return Task.FromResult(document);
        }

        var displayName = displayNameSyntax.GetDisplayName();
        if (displayName == null)
        {
            return Task.FromResult(document);
        }

        var newMethodName = Converters.TextToCode(displayName);
        if (newMethodName == null)
        {
            return Task.FromResult(document);
        }

        var newIdentifierToken = SyntaxFactory.Identifier(newMethodName);

        var newSyntaxRoot = syntaxRoot.ReplaceToken(identifierToken, newIdentifierToken);
        return Task.FromResult(document.WithSyntaxRoot(newSyntaxRoot));
    }
}