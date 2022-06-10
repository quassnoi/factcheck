using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace FactCheck.Analyzers.Tests;
public class XunitDisplayNameMissingCodeFixTests
{
    private readonly string _xunitMockProjectPath = Path.Combine("MockProjects", "Xunit.MockProject");
    private readonly string _xunitMockProjectFixedPath = Path.Combine("MockProjects", "Xunit.MockProject.Fixed");


    private static CSharpCodeFixTest<XunitDisplayNameMissingAnalyzer, XunitDisplayNameMissingCodeFix, XUnitVerifier> TestFactory(
        string testCode,
        string fixedCode,
        params DiagnosticResult[] diagnosticResults)
    {
        var cSharpCodeFixTest = new CSharpCodeFixTest<XunitDisplayNameMissingAnalyzer, XunitDisplayNameMissingCodeFix, XUnitVerifier>()
        {
            ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(
                ImmutableArray.Create(
                    new PackageIdentity("xunit", "2.4.1")
                )),
            TestCode = testCode,
            FixedCode = fixedCode
        };

        cSharpCodeFixTest.ExpectedDiagnostics.AddRange(diagnosticResults);
        return cSharpCodeFixTest;
    }

    [Fact(DisplayName = "XunitDisplayNameMissingCodeFix, when DisplayName attribute is missing, will create it")]
    public async Task XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingWillCreateIt()
    {
        var (testCode, fixedCode) = await (
            Helpers.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingWillCreateIt)),
            Helpers.LoadModule(_xunitMockProjectFixedPath, nameof(XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingWillCreateIt))
        );
        var codeFixTest = TestFactory(testCode, fixedCode);
        await codeFixTest.RunAsync();
    }
}