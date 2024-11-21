namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientNotificationTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task CheckListMarkScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // 一旦すべて既読にしておく
        await client.Notification.MarkAsync(all: true, to_status: "read");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // テストユーザでIssue作成
        var userIssue = await userClient.Issue.CreateAsync(repoOwner, repoName, new(title: $"issue-{DateTime.Now.Ticks:X16}"));

        // 通知数取得。イベントから少し時間が経たないと通知が出ないようなのでしばらく繰り返して取得する。
        await TestCallHelper.TrySatisfy(
            caller: breaker => client.Notification.CheckNewAsync(breaker),
            condition: notifyCount => 0 < notifyCount.@new
        );

        // 未読情報リスト取得
        var notifications = await client.Notification.ListAsync(status_types: ["unread"]);
        notifications.Should().NotBeEmpty();

        // すべて既読にする
        var marked = await client.Notification.MarkAsync(all: true, to_status: "read");
        marked.Select(m => m.unread).Should().NotContain(true);

        // 未読情報リスト再取得
        var unreaded = await client.Notification.ListAsync(status_types: ["unread"]);
        unreaded.Should().BeEmpty();
    }

    [TestMethod]
    public async Task NotificationThreadScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // 一旦すべて既読にしておく
        await client.Notification.MarkAsync(all: true, to_status: "read");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // テストユーザでIssue作成
        await userClient.Issue.CreateAsync(repoOwner, repoName, new(title: $"issue1-{DateTime.Now.Ticks:X16}"));
        await userClient.Issue.CreateAsync(repoOwner, repoName, new(title: $"issue2-{DateTime.Now.Ticks:X16}"));
        await userClient.Issue.CreateAsync(repoOwner, repoName, new(title: $"issue3-{DateTime.Now.Ticks:X16}"));

        // 通知数取得。イベントから少し時間が経たないと通知が出ないようなのでしばらく繰り返して取得する。
        await TestCallHelper.TrySatisfy(
            caller: breaker => client.Notification.CheckNewAsync(breaker),
            condition: notifyCount => 3 <= notifyCount.@new
        );

        // 未読情報リスト取得
        var notifications = await client.Notification.ListAsync(status_types: ["unread"]);
        notifications.Should().HaveCount(3);

        // 個別に情報取得
        var noti1 = await client.Notification.GetThreadAsync(notifications[0].id!.Value);
        var noti2 = await client.Notification.GetThreadAsync(notifications[1].id!.Value);
        var noti3 = await client.Notification.GetThreadAsync(notifications[2].id!.Value);
        noti1.unread.Should().BeTrue();
        noti2.unread.Should().BeTrue();
        noti3.unread.Should().BeTrue();

        // 個別にマーク
        var marked1 = await client.Notification.MarkThreadAsync(noti1.id!.Value, to_status: "read");
        var marked2 = await client.Notification.MarkThreadAsync(noti2.id!.Value, to_status: "read");
        var marked3 = await client.Notification.MarkThreadAsync(noti3.id!.Value, to_status: "read");
        marked1.unread.Should().BeFalse();
        marked2.unread.Should().BeFalse();
        marked3.unread.Should().BeFalse();
    }

    [TestMethod]
    public async Task RepoNotificationScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // 一旦すべて既読にしておく
        await client.Notification.MarkAsync(all: true, to_status: "read");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // テストユーザでIssue作成
        await userClient.Issue.CreateAsync(repoOwner, repoName, new(title: $"issue1-{DateTime.Now.Ticks:X16}"));
        await userClient.Issue.CreateAsync(repoOwner, repoName, new(title: $"issue2-{DateTime.Now.Ticks:X16}"));
        await userClient.Issue.CreateAsync(repoOwner, repoName, new(title: $"issue3-{DateTime.Now.Ticks:X16}"));

        // 通知数取得。イベントから少し時間が経たないと通知が出ないようなのでしばらく繰り返して取得する。
        await TestCallHelper.TrySatisfy(
            caller: breaker => client.Notification.CheckNewAsync(breaker),
            condition: notifyCount => 3 <= notifyCount.@new
        );

        // 未読情報リスト取得
        var notifications = await client.Notification.ListRepositoryThreadsAsync(repoOwner, repoName, status_types: ["unread"]);
        notifications.Should().HaveCount(3);

        // すべて既読にする
        var marked = await client.Notification.MarkRepositoryThreadsAsync(repoOwner, repoName, all: true, to_status: "read");
        marked.Select(m => m.unread).Should().NotContain(true);

        // 未読情報リスト再取得
        var unreaded = await client.Notification.ListRepositoryThreadsAsync(repoOwner, repoName, status_types: ["unread"]);
        unreaded.Should().BeEmpty();
    }

}
