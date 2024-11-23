using ForgejoApiClient.Api;

namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientActivityPubTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task GetActorAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // ターゲット呼び出し
        var pub = await client.ActivityPub.GetActorAsync();
        pub.Should().NotBeNull();
    }

    [TestMethod]
    public async Task SendToInboxAsync()
    {
        Assert.Inconclusive("利用方法がわからない。");

        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var userName = $"user-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync(userName);

        // ターゲット呼び出し
        await client.ActivityPub.SendToInboxAsync();
    }

    [TestMethod]
    public async Task GetUserActorAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var userName = $"user-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync(userName);

        // ターゲット呼び出し
        var pub = await client.ActivityPub.GetUserActorAsync(user.id!.Value);
        pub.Should().NotBeNull();
    }

    [TestMethod]
    public async Task SendUserToInboxAsync()
    {
        Assert.Inconclusive("利用方法がわからない。");

        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var userName = $"user-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync(userName);

        // ターゲット呼び出し
        await client.ActivityPub.SendUserToInboxAsync(user.id!.Value);
    }

    [TestMethod]
    public async Task GetRepositoryActorAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // ターゲット呼び出し
        var pub = await client.ActivityPub.GetRepositoryActorAsync(repo.id!.Value);
        pub.Should().NotBeNull();
    }

    [TestMethod]
    public async Task SendRepositoryToInboxAsync()
    {
        Assert.Inconclusive("利用方法がわからない。");

        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var userName = $"user-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync(userName);

        // ターゲット呼び出し
        await client.ActivityPub.SendRepositoryToInboxAsync(user.id!.Value, new());
    }

}
