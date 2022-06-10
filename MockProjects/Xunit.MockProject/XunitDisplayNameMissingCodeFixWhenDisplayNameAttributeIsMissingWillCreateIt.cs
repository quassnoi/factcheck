namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenDisplayNameAttributeIsMissingWillCreateIt
{
    [/*[|*/Fact/*|]*/]
    public void Test() { }
}
