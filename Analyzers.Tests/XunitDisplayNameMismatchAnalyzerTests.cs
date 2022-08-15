using System.Collections.Immutable;
using FactCheck.Analyzers.Tests.Helpers;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<FactCheck.XunitDisplayNameMismatchAnalyzer>;

namespace FactCheck.Analyzers.Tests;

public class XunitDisplayNameMismatchAnalyzerTests
{
    private readonly string _xunitMockProjectPath = Path.Combine("MockProjects", "Xunit.MockProject");

    private static CSharpAnalyzerTest<XunitDisplayNameMismatchAnalyzer, XUnitVerifier> TestFactory(string code, params DiagnosticResult[] diagnosticResults)
    {
        var cSharpAnalyzerTest = new CSharpAnalyzerTest<XunitDisplayNameMismatchAnalyzer, XUnitVerifier>()
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

    [Fact(DisplayName = "XunitDisplayNameMismatchAnalyzer, when DisplayName matches method name, should not issue diagnostic")]
    public async Task XunitDisplayNameMismatchAnalyzerWhenDisplayNameMatchesMethodNameShouldNotIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMismatchAnalyzerWhenDisplayNameMatchesMethodNameShouldNotIssueDiagnostic));

        await TestFactory(code).RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMismatchAnalyzer, when DisplayName does not match method name, should issue diagnostic")]
    public async Task XunitDisplayNameMismatchAnalyzerWhenDisplayNameDoesNotMatchMethodNameShouldIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMismatchAnalyzerWhenDisplayNameDoesNotMatchMethodNameShouldIssueDiagnostic));

        await TestFactory(
            code,
            Verify.Diagnostic(Diagnostics.FactCheck0002XunitDisplayNameMismatch).WithLocation(6, 17))
            .RunAsync();
    }
}