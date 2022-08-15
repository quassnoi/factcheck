using System.Collections.Immutable;
using FactCheck.Analyzers.Tests.Helpers;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<FactCheck.XunitDisplayNameMissingAnalyzer>;

namespace FactCheck.Analyzers.Tests;

public class XunitDisplayNameMissingAnalyzerTests
{
    private readonly string _xunitMockProjectPath = Path.Combine("MockProjects", "Xunit.MockProject");

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

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when DisplayName is provided on Fact, should not issue diagnostic")]
    public async Task XunitDisplayNameMissingAnalyzerWhenDisplayNameIsProvidedOnFactShouldNotIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingAnalyzerWhenDisplayNameIsProvidedOnFactShouldNotIssueDiagnostic));

        var test = TestFactory(code);
        await test.RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when qualified name is used, should not issue diagnostic")]
    public async Task XunitDisplayNameMissingAnalyzerWhenQualifiedNameIsUsedShouldNotIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingAnalyzerWhenQualifiedNameIsUsedShouldNotIssueDiagnostic));

        var test = TestFactory(code);
        await test.RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when attribute string is not trimmed, should not issue diagnostic")]
    public async Task XunitDisplayNameMissingAnalyzerWhenAttributeStringIsNotTrimmedShouldNotIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingAnalyzerWhenAttributeStringIsNotTrimmedShouldNotIssueDiagnostic));

        var test = TestFactory(code);
        await test.RunAsync();
    }


    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when DisplayName is not provided on Fact, should issue diagnostic")]
    public async Task XunitDisplayNameMissingAnalyzerWhenDisplayNameIsNotProvidedOnFactShouldIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingAnalyzerWhenDisplayNameIsNotProvidedOnFactShouldIssueDiagnostic));

        await TestFactory(
            code,
            Verify.Diagnostic(Diagnostics.FactCheck0001XunitDisplayNameMissing).WithLocation(5, 6))
            .RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when DisplayName is not provided on Theory, should issue diagnostic")]
    public async Task XunitDisplayNameMissingAnalyzerWhenDisplayNameIsNotProvidedOnTheoryShouldIssueDiagnostic()
    {
        var code = await FileHelper.LoadModule(_xunitMockProjectPath, nameof(XunitDisplayNameMissingAnalyzerWhenDisplayNameIsNotProvidedOnTheoryShouldIssueDiagnostic));

        await TestFactory(
            code,
            Verify.Diagnostic(Diagnostics.FactCheck0001XunitDisplayNameMissing).WithLocation(5, 6))
            .RunAsync();
    }

    [Fact(DisplayName = "XunitDisplayNameMissingAnalyzer, when xunit is not referenced, should not issue diagnostic")]
    public async Task XunitDisplayNameMissingAnalyzerWhenXunitIsNotReferencedShouldNotIssueDiagnostic()
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