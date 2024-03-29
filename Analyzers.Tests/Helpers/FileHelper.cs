namespace FactCheck.Analyzers.Tests.Helpers;

internal static class FileHelper
{
    private static async Task<string> LoadCode(string path, string file, CancellationToken cancellationToken = default)
    {
        var code = await File.ReadAllTextAsync(Path.Combine(path, file), cancellationToken);
        return code.Replace("/*[|*/", "[|").Replace("/*|]*/", "|]");
    }

    public static Task<string> LoadModule(string path, string module, CancellationToken cancellationToken = default)
        => LoadCode(path, $"{module}.cs", cancellationToken);
}