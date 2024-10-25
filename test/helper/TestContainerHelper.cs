using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lestaly.Cx;

namespace ForgejoApiClient.Tests.helper;

internal static class TestContainerHelper
{
    public static async Task<string> ExecAsync(params string[] commands)
    {
        var composeFile = TestPathHelper.GetDockerDir().RelativeFile("compose.yml");
        return await "docker".args(["compose", "--file", composeFile.FullName, "exec", "--user", "1000", "app", .. commands]).silent().result().success().output();
    }

    public static async Task<string?> TryExecAsync(params string[] commands)
    {
        try
        {
            var composeFile = TestPathHelper.GetDockerDir().RelativeFile("compose.yml");
            return await "docker".args(["compose", "--file", composeFile.FullName, "exec", "--user", "1000", "app", .. commands]).silent().result().output();
        }
        catch
        {
            return null;
        }
    }
}
