#pragma warning disable IDE0001
namespace Xunit.MockProject;

public class MissingNameAnalyzerIfQualifiedNameIsUsedShouldNotIssueDiagnostic
{
    [Xunit.Fact(DisplayName = "Test")]
    public void Test() { }
}