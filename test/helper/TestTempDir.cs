using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgejoApiClient.Tests.helper;

public sealed class TestTempDir : IDisposable
{
    public TestTempDir(string? prefix = default)
    {
        var tempPrefix = prefix ?? "ForgejoApiClientTests";
        var tempDirName = string.IsNullOrEmpty(tempPrefix) ? $"{Guid.NewGuid()}" : $"{tempPrefix}-{Guid.NewGuid()}";
        var tempDirPath = Path.Combine(Path.GetTempPath(), tempDirName);
        this.Info = new DirectoryInfo(tempDirPath);
        this.Info.Create();
    }

    public DirectoryInfo Info { get; set; }

    public void Dispose()
    {
        foreach (var item in this.Info.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
        {
            if ((item.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                item.Attributes &= ~FileAttributes.ReadOnly;
            }
        }
        this.Info.DeleteRecurse();
    }
}
