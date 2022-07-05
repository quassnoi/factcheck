namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingOnFactWillCreateIt
{
    [/*[|*/Fact/*|]*/]
    public void Test() { }
}
