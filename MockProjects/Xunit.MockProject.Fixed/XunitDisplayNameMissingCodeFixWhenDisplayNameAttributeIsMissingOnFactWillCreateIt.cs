namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnFactWillCreateIt
{
    [Fact(DisplayName = "Test")]
    public void Test() { }
}
