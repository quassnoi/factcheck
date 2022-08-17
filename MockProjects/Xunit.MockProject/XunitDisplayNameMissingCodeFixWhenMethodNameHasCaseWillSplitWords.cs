namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenMethodNameHasCaseWillSplitWords
{
    [/*[|*/Fact/*|]*/]
    public void TestMethodName() { }
}
