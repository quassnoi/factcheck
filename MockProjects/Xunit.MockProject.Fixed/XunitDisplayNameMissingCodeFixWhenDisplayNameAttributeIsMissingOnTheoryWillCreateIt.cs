namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnTheoryWillCreateIt
{
    [Theory(DisplayName = "Test")]
    [InlineData(1)]
    public void Test(int data)
    {
        Assert.NotEqual(0, data);
    }
}
