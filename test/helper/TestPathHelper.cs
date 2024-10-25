using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ForgejoApiClient.Tests.helper;

internal static class TestPathHelper
{
    public static DirectoryInfo GetProjectDir()
    {
        var thisFile = new FileInfo(getThisFilePath());
        var scanDir = thisFile.Directory;
        while (scanDir != null)
        {
            if (string.Equals(scanDir.Name, "test", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            scanDir = scanDir.Parent;
        }
        if (scanDir == null) throw new Exception("Solution directory not found.");

        return scanDir;
    }

    public static DirectoryInfo GetDockerDir()
        => GetProjectDir().RelativeDirectory("../service/docker");

    private static string getThisFilePath([CallerFilePath] string filePath = "") => filePath;

}
