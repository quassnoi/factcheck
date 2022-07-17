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

    public static bool IsAttributeWithDisplayName(this AttributeSyntax attributeSyntax, SemanticModel semanticModel)
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
        } ? identifier : null;

    public static string? GetMethodName(this AttributeSyntax attributeSyntax)
        => attributeSyntax.GetMethodIdentifier()?.Text;

    public static string? GetDisplayName(this AttributeSyntax attributeSyntax)
        => attributeSyntax
            .ArgumentList?
            .Arguments
            .Where(argument => argument is { NameEquals.Name.Identifier.Text: Constants.PropertyDisplayName })
            .Select(argument => argument is
            {
                Expression: LiteralExpressionSyntax
                {
                    Token:
                    {
                        RawKind: (int)SyntaxKind.StringLiteralToken,
                        Value: string displayName
                    }
                }
            } ? displayName : null
            )
            .FirstOrDefault();

    public static IEnumerable<(string displayName, string methodName)> GetAttributesNames(this IEnumerable<AttributeSyntax> attributeSyntaxes)
    {
        foreach (var attributeSyntax in attributeSyntaxes)
        {
            if (attributeSyntax.GetDisplayName() is string displayName && attributeSyntax.GetMethodName() is string methodName)
            {
                yield return (displayName, methodName);
            }
        }
    }



}