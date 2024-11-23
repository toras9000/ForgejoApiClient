using System.Text.Json;
using ForgejoApiClient.Api;

namespace ForgejoApiClient.Tests;

record TestServiceInfo(string Url, BasicAuthCredential Admin, string Token);

public abstract class ForgejoApiClientTestsBase
{
    public ForgejoApiClientTestsBase()
    {
        // テスト用のサービスとAPIキー情報を読み取り。これはテスト用のコンテナ初期化時に作られているはずのもの。
        var keyFile = TestPathHelper.GetProjectDir().RelativeFile("test-forgejo-instance.json");
        using var keyStream = keyFile.OpenRead();
        var svcInfo = JsonSerializer.Deserialize<TestServiceInfo>(keyStream) ?? throw new Exception("Cannot load test token");
        if (svcInfo.Url == null) throw new Exception("Cannot load test token");
        if (svcInfo.Admin?.Username == null) throw new Exception("Cannot load test token");
        if (svcInfo.Admin?.Password == null) throw new Exception("Cannot load test token");
        if (svcInfo.Token == null) throw new Exception("Cannot load test token");
        this.TestService = new Uri(svcInfo.Url);
        this.TestToken = svcInfo.Token;
        this.TestTokenUser = svcInfo.Admin.Username;
        this.TestAdminUser = svcInfo.Admin;
    }

    public Uri TestService { get; }
    public string TestToken { get; }
    public string TestTokenUser { get; }
    public BasicAuthCredential TestAdminUser { get; }
}
