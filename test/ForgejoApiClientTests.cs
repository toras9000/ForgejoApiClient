namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task Sudo()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var userName = $"user-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync(userName);

        // sudo クライアント作成
        var userClient = client.Sudo(userName);
        userClient.SudoUser.Should().Be(userName);
    }

    [TestMethod]
    public void IsDisposed()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        client.Dispose();
        client.IsDisposed.Should().BeTrue();
    }

    [TestMethod]
    public async Task ErrorResponseException()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // 存在しないエンティティ情報
        var repoOwnder = $"not-exist-{DateTime.Now.Ticks:X16}";
        var repoName = $"not-exist-{DateTime.Now.Ticks:X16}";

        // sudo クライアント作成
        await FluentActions.Awaiting(() => client.Repository.GetAsync(repoOwnder, repoName)).Should().ThrowAsync<ErrorResponseException>();
    }
}
