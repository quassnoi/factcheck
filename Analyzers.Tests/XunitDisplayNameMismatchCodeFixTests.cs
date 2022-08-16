using System.Collections.Immutable;
using FactCheck.Analyzers.Tests.Helpers;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace FactCheck.Analyzers.Tests;

public class XunitDisplayNameMismatchCodeFixTests
{
    private readonly string _xunitMockProjectFixedPath = Path.Combine("MockProjects", "Xunit.MockProject.Fixed");
    private readonly string _xunitMockProjectPath = Path.Combine("MockProjects", "Xunit.MockProject");


    private static CSharpCodeFixTest<XunitDisplayNameMismatchAnalyzer, XunitDisplayNameMismatchCodeFix, XUnitVerifier> TestFactory(
        string testCode,
        string fixedCode,
        params DiagnosticResult[] diagnosticResults)
    {
        var cSharpCodeFixTest = new CSharpCodeFixTest<XunitDisplayNameMismatchAnalyzer, XunitDisplayNameMismatchCodeFix, XUnitVerifier>
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

    [Theory(DisplayName = "XunitDisplayNameMismatchCodeFix, when DisplayName does not match method name, will rename method")]
    [InlineData("lowercase name", "LowercaseName")]
    [InlineData("UPPERCASE NAME", "UPPERCASENAME")]
    public async Task XunitDisplayNameMismatchCodeFixWhenDisplayNameDoesNotMatchMethodNameWillRenameMethod(string displayName, string fixedMethodName)
    {
        var (testTemplate, fixedTemplate) = await (
            FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMismatchCodeFixWhenDisplayNameDoesNotMatchMethodNameWillRenameMethod)),
            FileHelper.LoadModule(_xunitMockProjectFixedPath, nameof(XunitDisplayNameMismatchCodeFixWhenDisplayNameDoesNotMatchMethodNameWillRenameMethod))
        );

        var testCode = testTemplate.Replace("DisplayNameValue", displayName);
        var fixedCode = fixedTemplate
            .Replace("DisplayNameValue", displayName)
            .Replace("TestMethodName", fixedMethodName);

        var codeFixTest = TestFactory(testCode, fixedCode);
        await codeFixTest.RunAsync();
    }
}