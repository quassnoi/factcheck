namespace Xunit.MockProject;

public class XunitDisplayNameMissingAnalyzerWhenDisplayNameIsNotProvidedOnTheoryShouldIssueDiagnostic
{
    [Theory]
    [InlineData(1)]
    public void Test(int data)
    {
        Assert.NotEqual(0, data);
    }
}