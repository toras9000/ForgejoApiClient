namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientPackageTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task PackageScenario()
    {
        var ownerName = this.TestTokenUser;
        var ownerKey = this.TestToken;
        var pkgName = "Dummy";
        var pkgVer = "0.0.0";
        var pkgFile = $"{pkgName}.{pkgVer}.nupkg";

        // テスト用のパッケージをアップロード
        await this.UploadPackageAsync(ownerName, ownerKey, pkgFile);

        // クライアント生成
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // パッケージ情報取得
        var package = await client.Package.GetAsync(ownerName, "nuget", pkgName, pkgVer);

        // パッケージリスト取得
        var pkg_list = await client.Package.ListAsync(ownerName);

        // パッケージファイルリスト取得
        var pkg_files = await client.Package.GetFilesAsync(ownerName, "nuget", pkgName, pkgVer);

        // パッケージ削除
        await client.Package.DeleteAsync(ownerName, "nuget", pkgName, pkgVer);

        // 検証
        package.name.Should().Be(pkgName);
        pkg_list.Should().Contain(p => p.name == pkgName);
        pkg_files.Select(s => s.name).Should().ContainEquivalentOf(pkgFile, config: c => c.Using(StringComparer.OrdinalIgnoreCase));
    }



}
