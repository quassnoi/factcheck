namespace Xunit.MockProject;

public class XunitDisplayNameMismatchAnalyzerWhenDisplayNameMatchesMethodNameShouldNotIssueDiagnostic
{
    [Fact(DisplayName = "System, when condition, should behave")]
    public void SystemWhenConditionShouldBehave() { }
}