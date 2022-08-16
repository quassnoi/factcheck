#pragma warning disable IDE0001
namespace Xunit.MockProject;

public class XunitDisplayNameUnconvertibleAnalyzerWhenDisplayNameConvertsToValidIdentifierShouldNotIssueDiagnostic
{
    [FactAttribute(DisplayName = "DisplayNameValue")]
    public void Test() { }
}