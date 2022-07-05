namespace Xunit.MockProject;

public class MissingNameAnalyzerWhenDisplayNameIsProvidedShouldNotIssueDiagnostic
{
    [Fact(DisplayName = "Test")]
    public void Test() { }
}