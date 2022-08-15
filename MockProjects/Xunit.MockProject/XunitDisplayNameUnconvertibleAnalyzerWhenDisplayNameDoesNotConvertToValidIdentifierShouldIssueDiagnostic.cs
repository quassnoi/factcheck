#pragma warning disable IDE0001
namespace Xunit.MockProject;

public class XunitDisplayNameUnconvertibleAnalyzerWhenDisplayNameDoesNotConvertToValidIdentifierShouldIssueDiagnostic
{
    [FactAttribute(DisplayName = "DisplayNameValue")]
    public void Test() { }
}