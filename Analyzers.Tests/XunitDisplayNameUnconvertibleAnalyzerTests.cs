using System.Collections.Immutable;
using FactCheck.Analyzers.Tests.Helpers;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<FactCheck.XunitDisplayNameUnconvertibleAnalyzer>;

namespace FactCheck.Analyzers.Tests;

public class XunitDisplayNameUnconvertibleAnalyzerTests
{
    private readonly string _xunitMockProjectPath = Path.Combine("MockProjects", "Xunit.MockProject");
    private static CSharpAnalyzerTest<XunitDisplayNameUnconvertibleAnalyzer, XUnitVerifier> TestFactory(string code, params DiagnosticResult[] diagnosticResults)
    {
        var cSharpAnalyzerTest = new CSharpAnalyzerTest<XunitDisplayNameUnconvertibleAnalyzer, XUnitVerifier>()
        {
            ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(
                ImmutableArray.Create(
                    new PackageIdentity("xunit", "2.4.1")
                )),
            TestState = {
                Sources =
                {
                    code
                }
            },
        };

        cSharpAnalyzerTest.ExpectedDiagnostics.AddRange(diagnosticResults);
        return cSharpAnalyzerTest;
    }

    [Fact(DisplayName = "XunitDisplayNameUnconvertibleAnalyzer, when DisplayName converts to valid identifier, should not issue diagnostic")]
    public async Task XunitDisplayNameUnconvertibleAnalyzerWhenDisplayNameConvertsToValidIdentifierShouldNotIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameUnconvertibleAnalyzerWhenDisplayNameConvertsToValidIdentifierShouldNotIssueDiagnostic));
        await TestFactory(code).RunAsync();
    }

    [Theory(DisplayName = "XunitDisplayNameUnconvertibleAnalyzer, when DisplayName does not convert to valid identifier, should issue diagnostic")]
    [InlineData("12345")]
    [InlineData("___")]
    [InlineData("")]
    public async Task XunitDisplayNameUnconvertibleAnalyzerWhenDisplayNameDoesNotConvertToValidIdentifierShouldIssueDiagnostic(string displayName)
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameUnconvertibleAnalyzerWhenDisplayNameDoesNotConvertToValidIdentifierShouldIssueDiagnostic));
        var replacedCode = code.Replace("DisplayNameValue", displayName);
        await TestFactory(
            replacedCode,
            Verify.Diagnostic(Diagnostics.FactCheck0003XunitDisplayNameUnconvertible).WithLocation(6, 34))
            .RunAsync();
    }

}