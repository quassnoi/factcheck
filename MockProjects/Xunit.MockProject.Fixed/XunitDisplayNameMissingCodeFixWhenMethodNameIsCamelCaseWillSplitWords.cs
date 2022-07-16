namespace Xunit.MockProject;

public class XunitDisplayNameMissingCodeFixWhenMethodNameIsCamelCaseWillSplitWords
{
    [Fact(DisplayName = "DisplayNameValue")]
    public void TestMethodName() { }
}
