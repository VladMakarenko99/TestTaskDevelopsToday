using System.Text.RegularExpressions;

namespace TestTaskDevelopsToday.Utilities;

public static class PathUtility
{
    public static string RemoveUnwantedSegments(string baseDirectory)
    {
        var regex = new Regex(@"[/\\]bin[/\\]Debug[/\\]net\d+\.\d+[/\\]?", RegexOptions.IgnoreCase);
        
        var projectRoot = regex.Replace(baseDirectory, string.Empty);

        return projectRoot;
    }
}