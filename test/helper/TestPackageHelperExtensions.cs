using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgejoApiClient.Api;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace ForgejoApiClient.Tests.helper;

public static class TestPackageHelperExtensions
{
    public static Task UploadPackageAsync(this ForgejoApiClientTestsBase self, string fileName, CancellationToken token = default)
        => self.UploadPackageAsync(self.TestTokenUser, self.TestToken, TestPathHelper.GetProjectDir().RelativeFile($"assets/packages/{fileName}"), token);

    public static Task UploadPackageAsync(this ForgejoApiClientTestsBase self, string owner, string key, string fileName, CancellationToken token = default)
        => self.UploadPackageAsync(owner, key, TestPathHelper.GetProjectDir().RelativeFile($"assets/packages/{fileName}"), token);

    public static Task UploadPackageAsync(this ForgejoApiClientTestsBase self, FileInfo pkgFile, CancellationToken token = default)
        => self.UploadPackageAsync(self.TestTokenUser, self.TestToken, pkgFile, token);

    public static async Task UploadPackageAsync(this ForgejoApiClientTestsBase self, string owner, string key, FileInfo pkgFile, CancellationToken token = default)
    {
        var nugetFeed = new Uri(self.TestService, $"/api/packages/{owner}/nuget/index.json");
        var nugetRepo = NuGet.Protocol.Core.Types.Repository.Factory.GetCoreV3(nugetFeed.AbsoluteUri);
        var nugetUpdater = await nugetRepo.GetResourceAsync<PackageUpdateResource>();
        await nugetUpdater.Push(
            packagePaths: [pkgFile.FullName],
            symbolSource: default,
            timeoutInSecond: 60,
            disableBuffering: false,
            getApiKey: _ => self.TestToken,
            getSymbolApiKey: _ => self.TestToken,
            noServiceEndpoint: false,
            skipDuplicate: true,
            symbolPackageUpdateResource: default,
            log: NullLogger.Instance
        );


    }
}
