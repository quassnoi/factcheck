using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<FactCheck.XunitDisplayNameMissingAnalyzer>;

namespace FactCheck.Analyzers.Tests;

public class XunitDisplayNameMissingAnalyzerTests
{
    private readonly string _xunitMockProjectPath = Path.Combine("MockProjects", "Xunit.MockProject");
    private readonly string _classlibMockProjectPath = Path.Combine("MockProjects", "Classlib.MockProject");

    private static CSharpAnalyzerTest<XunitDisplayNameMissingAnalyzer, XUnitVerifier> TestFactory(string code, params DiagnosticResult[] diagnosticResults)
    {
        var cSharpAnalyzerTest = new CSharpAnalyzerTest<XunitDisplayNameMissingAnalyzer, XUnitVerifier>()
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

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when DisplayName is provided, should not issue diagnostic")]
    public async Task MissingNameAnalyzerWhenDisplayNameIsProvidedShouldNotIssueDiagnostic()
    {
        var code = await Helpers.LoadModule(_xunitMockProjectPath, nameof(MissingNameAnalyzerWhenDisplayNameIsProvidedShouldNotIssueDiagnostic));

        var test = TestFactory(code);
        await test.RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, if qualified name is used, should not issue diagnostic")]
    public async Task MissingNameAnalyzerIfQualifiedNameIsUsedShouldNotIssueDiagnostic()
    {
        var code = @"
using System;

namespace Xunit
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class FactAttribute : Attribute
    {
        public string? DisplayName { get; set; }
    }
}

namespace Classlib.MockProject
{
    using Xunit;

    public class XunitDisplayNameMissingAnalyzerWhenXunitIsNotReferencedShouldNotIssueDiagnostic
    {
        [Fact(DisplayName = ""Test"")]
        public void TestWithDislayName() { }

        [Fact]
        public void TestWithoutDislayName()
        { }

    }
}
        ";

        var test = TestFactory(code);
        await test.RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, if attribute string is not trimmed, should not issue diagnostic")]
    public async Task MissingNameAnalyzerIfAttributeStringIsNotTrimmedShouldNotIssueDiagnostic()
    {
        var code = await Helpers.LoadModule(_xunitMockProjectPath, nameof(MissingNameAnalyzerIfAttributeStringIsNotTrimmedShouldNotIssueDiagnostic));

        var test = TestFactory(code);
        await test.RunAsync();
    }


    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when DisplayName is not provided, should issue diagnostic")]
    public async Task MissingNameAnalyzerWhenDisplayNameIsNotProvidedShouldIssueDiagnostic()
    {
        var code = await Helpers.LoadModule(_xunitMockProjectPath, nameof(MissingNameAnalyzerWhenDisplayNameIsNotProvidedShouldIssueDiagnostic));

        await TestFactory(
            code,
            Verify.Diagnostic(Diagnostics.FactCheck001XunitDisplayNameMissing).WithLocation(5, 6))
            .RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when xunit is not referenced, should not issue diagnostic")]
    public async Task XunitDisplayNameMissingAnalyzerWhenXunitIsNotReferencedShouldNotIssueDiagnostic()
    {
        var code = await Helpers.LoadModule(_classlibMockProjectPath, nameof(XunitDisplayNameMissingAnalyzerWhenXunitIsNotReferencedShouldNotIssueDiagnostic));

        var cSharpAnalyzerTest = new CSharpAnalyzerTest<XunitDisplayNameMissingAnalyzer, XUnitVerifier>()
        {
            ReferenceAssemblies = ReferenceAssemblies.Default.AddAssemblies(ImmutableArray.Create("System.Runtime")),
            TestState = {
                Sources =
                {
                    code
                }
            },
        };

        await cSharpAnalyzerTest.RunAsync();
    }
}