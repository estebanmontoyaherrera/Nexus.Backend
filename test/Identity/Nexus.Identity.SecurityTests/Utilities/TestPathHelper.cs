using System.Runtime.CompilerServices;

namespace Nexus.Identity.SecurityTests.Utilities;

public static class TestPathHelper
{
    public static string RepoRoot([CallerFilePath] string callerPath = "")
    {
        var dir = Path.GetDirectoryName(callerPath)!;

        while (dir != null)
        {
            if (Directory.Exists(Path.Combine(dir, "src")) &&
                Directory.Exists(Path.Combine(dir, "test")))
            {
                return dir;
            }

            dir = Directory.GetParent(dir)?.FullName;
        }

        throw new DirectoryNotFoundException(
            "Unable to locate Nexus.Backend repository root from test context.");
    }
}