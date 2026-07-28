using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using ForgejoApiClient.Api;

namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientMiscellaneousTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task ListGitignoreTemplatesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var templates = await client.Miscellaneous.ListGitignoreTemplatesAsync();
        templates.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetGitignoreTemplateAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var template = await client.Miscellaneous.GetGitignoreTemplateAsync("C");
        template.name.Should().Be("C");
    }

    [TestMethod]
    public async Task ListLabelTemplatesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var templates = await client.Miscellaneous.ListLabelTemplatesAsync();
        templates.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task ListTemplateLabelsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var labels = await client.Miscellaneous.ListTemplateLabelsAsync("Default");
        labels.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task ListLicenseTemplatesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var templates = await client.Miscellaneous.ListLicenseTemplatesAsync();
        templates.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetLicenseTemplateAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var license = await client.Miscellaneous.GetLicenseTemplateAsync("MIT");
        license.name.Should().Be("MIT");
    }

    [TestMethod]
    public async Task RenderMarkdownAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var doc = await client.Miscellaneous.RenderMarkdownAsync(new(Text: "- aaa"));
        doc.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task RenderMarkdownRawAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var doc = await client.Miscellaneous.RenderMarkdownRawAsync("- aaa");
        doc.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task RenderMarkupAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var doc = await client.Miscellaneous.RenderMarkupAsync(new(Mode: "gfm", Text: "- aaa"));
        doc.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetNodeInfoAsync()
    {
        Assert.Inconclusive("このAPIはフェデレーション機能が有効な場合に利用できるものらしい？");

        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var info = await client.Miscellaneous.GetNodeInfoAsync();
        info.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetSigningKeyGpgAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var key = await client.Miscellaneous.GetSigningKeyGpgAsync();
        key.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetSigningKeySshAsync()
    {
        Assert.Inconclusive("このAPIは構成で [repository.signing] FORMAT=ssh の時に機能する模様");

        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var key = await client.Miscellaneous.GetSigningKeySshAsync();
        key.Should().NotBeNull();

    }

    [TestMethod]
    public async Task GetVersionAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var version = await client.Miscellaneous.GetVersionAsync();
        version.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetActionsRun()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // 自身のIP
        var selfIp = NetworkInterface.GetAllNetworkInterfaces()
            .Where(i => i.Supports(NetworkInterfaceComponent.IPv4))
            .Where(i => i.OperationalStatus == OperationalStatus.Up)
            .Where(i => i.NetworkInterfaceType is not NetworkInterfaceType.Loopback and not NetworkInterfaceType.Tunnel)
            .SelectMany(i => i.GetIPProperties().UnicastAddresses.Select(a => a.Address))
            .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
            .First();

        // UDP受信開始
        using var udp = new Socket(SocketType.Dgram, ProtocolType.Udp);
        udp.Bind(new IPEndPoint(IPAddress.Any, 0));
        var port = ((IPEndPoint)udp.LocalEndPoint!).Port;
        var buffer = new byte[1024];
        var recvTask = udp.ReceiveAsync(buffer);

        // コンテンツ作成
        // テスト実行ホストに対してアクショントークンを送信する。受信待機タイムアウトを 10 秒とし、少しだけ待ち状態になるようにしている。
        var content = $$$"""
        on: 
          workflow_dispatch:
        jobs:
          test:
            runs-on: busybox
            steps:
              - run: echo ${{ forgejo.token }} | nc -w 10 -u "{{{selfIp}}}" {{{port}}}
        """.ReplaceLineEndings("\n");
        var main1 = await client.Repository.CreateFileAsync(repoOwner, repoName, ".forgejo/workflows/demo.yaml", new(content: content.EncodeUtf8Base64()));

        // ワークフロー実行
        var dispatched = await client.Repository.DispatchActionsWorkflowAsync(repoOwner, repoName, "demo.yaml", new(@ref: "main", return_run_info: true));

        // 受信を完了する
        var recvLen = await recvTask.WaitAsync(TimeSpan.FromMinutes(2));

        // 受信したトークンをデコード
        var actionToken = Encoding.UTF8.GetString(buffer.AsSpan(0, recvLen)).Trim();

        // ワークフローラン取得
        var auth = new ActionTokenCredential(actionToken);
        var run = await client.Miscellaneous.GetActionsRun(auth);

        run.Should().NotBeNull();
    }



}
