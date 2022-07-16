namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenMethodNameIsCamelCaseWillSplitWords
{
    [/*[|*/Fact/*|]*/]
    public void TestMethodName() { }
}
