namespace Xunit.MockProject;

public class XunitDisplayNameMismatchAnalyzerWhenDisplayNameMatchesMethodNameShouldNotIssueDiagnostic
{
    [Fact(DisplayName = "DisplayNameValue")]
    public void TestMethodName() { }
}