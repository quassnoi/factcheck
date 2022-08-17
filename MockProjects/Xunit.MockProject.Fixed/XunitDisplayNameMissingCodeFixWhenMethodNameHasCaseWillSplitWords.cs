namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenMethodNameHasCaseWillSplitWords
{
    [Fact(DisplayName = "DisplayNameValue")]
    public void TestMethodName() { }
}
