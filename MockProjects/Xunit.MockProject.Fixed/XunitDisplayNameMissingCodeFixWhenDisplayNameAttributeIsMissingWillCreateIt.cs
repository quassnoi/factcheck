namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingWillCreateIt
{
    [Fact(DisplayName = "Test")]
    public void Test() { }
}
