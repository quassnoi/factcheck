namespace Xunit.MockProject;

public class XunitDisplayNameMismatchCodeFixWhenDisplayNameDoesNotMatchMethodNameWillRenameMethod
{
    [Fact(DisplayName = "DisplayNameValue")]
    public void TestMethodName() { }
}
