namespace Xunit.MockProject;

public class XunitDisplayNameMismatchAnalyzerWhenDisplayNameDoesNotMatchMethodNameShouldIssueDiagnostic
{
    [Fact(DisplayName = "System, when condition, should behave")]
    public void Test() { }
}