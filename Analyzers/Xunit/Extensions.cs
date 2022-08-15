using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FactCheck.Xunit;

internal static class Extensions
{
    public static bool HasDisplayName(this AttributeSyntax attributeSyntax)
        => attributeSyntax
            .ArgumentList?
            .Arguments
            .Any(argument => argument is { NameEquals.Name.Identifier.Text: Constants.PropertyDisplayName }) ?? false;

    private static bool SupportsDisplayName(this AttributeSyntax attributeSyntax, SemanticModel semanticModel)
        => semanticModel.GetSymbolInfo(attributeSyntax) is
        {
            Symbol: IMethodSymbol
                    {
                        MethodKind: MethodKind.Constructor,
                        ContainingType:
                        {
                            ContainingAssembly.Name: Constants.AssemblyXunitCore,
                            ContainingNamespace.Name: Constants.NamespaceAttributes,
                            Name: var containingTypeName
                        }
                    }
        } && Constants.DisplayNameAttributes.Contains(containingTypeName);

    public static IEnumerable<AttributeSyntax> GetAttributesSupportingDisplayName(this SemanticModel semanticModel)
        => semanticModel
            .SyntaxTree
            .GetRoot()
            .DescendantNodes().OfType<AttributeSyntax>()
            .Where(attributeSyntax => attributeSyntax.SupportsDisplayName(semanticModel));

    public static bool HasXunit(this SemanticModel semanticModel)
        => semanticModel
            .Compilation
            .ReferencedAssemblyNames
            .Any(assemblyIdentity => assemblyIdentity.Name == Constants.AssemblyXunitCore);

    public static SyntaxToken? GetMethodIdentifier(this AttributeSyntax attributeSyntax)
        => attributeSyntax is
        {
            Parent.Parent: MethodDeclarationSyntax
                           {
                               Identifier: var identifier
                           }
        }
            ? identifier
            : null;

    public static LiteralExpressionSyntax? GetDisplayNameExpression(this AttributeSyntax attributeSyntax)
        => attributeSyntax
            .ArgumentList?
            .Arguments
            .Where(argument => argument is { NameEquals.Name.Identifier.Text: Constants.PropertyDisplayName })
            .Select(argument => argument.Expression)
            .OfType<LiteralExpressionSyntax>()
            .FirstOrDefault();

    public static string? GetDisplayName(this LiteralExpressionSyntax? literalExpressionSyntax)
        => literalExpressionSyntax is
        {
            Token:
            {
                RawKind: (int)SyntaxKind.StringLiteralToken,
                Value: string displayName
            }
        }
            ? displayName
            : null;

    public static string? GetDisplayName(this AttributeSyntax attributeSyntax)
        => attributeSyntax.GetDisplayNameExpression().GetDisplayName();
}