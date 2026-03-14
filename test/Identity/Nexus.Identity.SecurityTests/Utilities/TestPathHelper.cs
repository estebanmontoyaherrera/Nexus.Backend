using System.Runtime.CompilerServices;

namespace Nexus.Identity.SecurityTests.Utilities;

public static class TestPathHelper
{
    public static string RepoRoot([CallerFilePath] string callerPath = "")
    {
        var dir = new DirectoryInfo(Path.GetDirectoryName(callerPath)!);
        while (dir is not null && dir.Name != "Nexus.Backend")
        {
            dir = dir.Parent;
        }

        if (dir is null)
        {
            throw new DirectoryNotFoundException("Unable to locate Nexus.Backend repository root from test context.");
        }

        return dir.FullName;
    }
}
