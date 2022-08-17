namespace Xunit.MockProject;

public class XunitDisplayNameMismatchAnalyzerWhenDisplayNameOnlyDiffersInCaseAndSeparatorsShouldNotIssueDiagnostic
{
    [Fact(DisplayName = "DisplayNameValue")]
    public void TestMethodName() { }
}