#pragma warning disable IDE0001
namespace Xunit.MockProject;

public class MissingNameAnalyzerIfAttributeStringIsNotTrimmedShouldNotIssueDiagnostic
{
    [FactAttribute(DisplayName = "Test")]
    public void Test() { }
}