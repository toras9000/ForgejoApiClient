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

    [TestMethod]
    public async Task Paging()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var owner = $"user-{DateTime.Now.Ticks:X16}";
        var user = await resources.CreateTestUserAsync(owner);
        for (var i = 0; i < 20; i++)
        {
            var name = $"repo-{Guid.NewGuid()}";
            var repo = await resources.CreateTestUserRepoAsync(owner, name);
        }

        // リポジトリ一覧の取得
        var repos1 = await client.Repository.SearchAsync(paging: new(page: 1, limit: 10));
        var repos2 = await client.Repository.SearchAsync(paging: new(page: 2, limit: 10));

        // 取得結果に重複無いことを確認
        Enumerable.Intersect(repos1.data!.Select(r => r.id), repos2.data!.Select(r => r.id)).Should().BeEmpty();
    }

}
