using System.Collections.Immutable;
using FactCheck.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FactCheck;

[ExportCodeFixProvider(LanguageNames.CSharp)]
public class XunitDisplayNameMissingCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
        Diagnostics.FactCheck0001XunitDisplayNameMissing.Id
    );

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostics = context.Diagnostics.Where(diagnostic => diagnostic.Id == Diagnostics.FactCheck0001XunitDisplayNameMissing.Id).ToImmutableList();

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

        if (diagnosticNode is not AttributeSyntax
            {
                Parent.Parent: MethodDeclarationSyntax
                {
                    Identifier.Text: string methodName
                }
            } attributeSyntax)
        {
            return;
        }

        var newDisplayName = Converters.CodeToText(methodName);

        context.RegisterCodeFix(
            CodeAction.Create(
                $"Add (DisplayName = \"{newDisplayName}\")",
                _ => CreateFixedDocument(context.Document, syntaxRoot, attributeSyntax, newDisplayName),
                nameof(XunitDisplayNameMissingCodeFix)),
            diagnostic);
    }

    private static Task<Document> CreateFixedDocument(Document document, SyntaxNode syntaxRoot, AttributeSyntax attributeSyntax, string newDisplayName)
    {
        var newAttributeSyntax = attributeSyntax.AddArgumentListArguments(
            SyntaxFactory.AttributeArgument(
                SyntaxFactory.NameEquals(
                    SyntaxFactory.IdentifierName("DisplayName"),
                    SyntaxFactory.Token(SyntaxKind.EqualsToken)
                ),
                null,
                SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal(newDisplayName)
                )
            )
        );
        var newSyntaxRoot = syntaxRoot.ReplaceNode(attributeSyntax, newAttributeSyntax);
        return Task.FromResult(document.WithSyntaxRoot(newSyntaxRoot));
    }
}