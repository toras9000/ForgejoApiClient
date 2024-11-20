﻿using ForgejoApiClient.Api;

namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientAdminTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task ListCronsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        var crons = await client.Admin.ListCronsAsync();
        crons.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task RunCronTaskAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        await client.Admin.RunCronTaskAsync("repo_health_check");
    }

    [TestMethod]
    public async Task GetActionRunnerRegistrationTokenAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        var result = await client.Admin.GetActionRunnerRegistrationTokenAsync();
        result.token.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task ListEmailsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        var mails = await client.Admin.ListEmailsAsync();
        mails.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task SearchEmailsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        var mails = await client.Admin.SearchEmailsAsync("admin");
        mails.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task DefaultWebhookScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // Webhook作成
        var webhook = await client.Admin.CreateWebhookAsync(
            new(
                config: new Dictionary<string, string>
                {
                    ["content_type"] = "json",
                    ["url"] = "http://localhost:9999",
                },
                type: CreateHookOptionType.Forgejo
            )
        );

        // Webhook取得
        var webhook_get = await client.Admin.GetWebhookAsync(webhook.id!.Value);

        // Webhook更新
        var webhook_updated = await client.Admin.UpdateWebhookAsync(
            webhook.id!.Value,
            new(config: new Dictionary<string, string>
            {
                ["url"] = "http://localhost:9998",
            })
        );

        // Webhook削除
        await client.Admin.DeleteWebhookAsync(webhook.id!.Value);

        // まとめて検証
        webhook.url.Should().Be("http://localhost:9999");
        webhook_get.url.Should().Be("http://localhost:9999");
        webhook_updated.url.Should().Be("http://localhost:9998");
    }

    [TestMethod]
    public async Task SystemWebhookScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // Webhook作成
        var webhook = await client.Admin.CreateWebhookAsync(
            new(
                config: new Dictionary<string, string>
                {
                    ["is_system_webhook"] = "true",
                    ["content_type"] = "json",
                    ["url"] = "http://localhost:9999",
                },
                type: CreateHookOptionType.Forgejo
            )
        );

        // Webhook取得
        var webhook_get = await client.Admin.GetWebhookAsync(webhook.id!.Value);

        // Webhookリスト取得
        var webhook_list = await client.Admin.ListSystemWebhooksAsync();

        // Webhook更新
        var webhook_updated = await client.Admin.UpdateWebhookAsync(
            webhook.id!.Value,
            new(config: new Dictionary<string, string>
            {
                ["url"] = "http://localhost:9998",
            })
        );

        // Webhook削除
        await client.Admin.DeleteWebhookAsync(webhook.id!.Value);

        // まとめて検証
        webhook.url.Should().Be("http://localhost:9999");
        webhook_get.url.Should().Be("http://localhost:9999");
        webhook_list.Should().Contain(k => k.url == "http://localhost:9999");
        webhook_updated.url.Should().Be("http://localhost:9998");
    }

    [TestMethod]
    public async Task ListUnadoptedReposAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // コンテナ内に Unadopted なリポジトリを作成
        var repoOwn = this.TestTokenUser;
        var repoName = $"urepo-{DateTime.Now.Ticks:x16}";   // 小文字でなければならない
        var repoPath = $"/data/git/repositories/{repoOwn}/{repoName}.git";
        await TestContainerHelper.ExecAsync("git", "init", "--bare", repoPath);

        try
        {
            // 対象メソッド実行
            var repos = await client.Admin.ListUnadoptedReposAsync();
            repos.Should().Contain($"{repoOwn}/{repoName}");
        }
        finally
        {
            // 作成したリポジトリのクリーンナップ
            await TestContainerHelper.TryExecAsync("rm", "-rf", repoPath);
        }
    }

    [TestMethod]
    public async Task AdoptUnadoptedRepositoryAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // コンテナ内に Unadopted なリポジトリを作成
        var repoOwner = this.TestTokenUser;
        var repoName = $"urepo-{DateTime.Now.Ticks:x16}";   // 小文字でなければならない
        var repoPath = $"/data/git/repositories/{repoOwner}/{repoName}.git";
        await TestContainerHelper.ExecAsync("git", "init", "--bare", repoPath);

        try
        {
            // 事前に、そのリポジトリ名が無いことを確認
            var repos_before = await client.Repository.SearchAsync(repoName);
            repos_before.data.Should().NotContain(r => r.owner!.login == repoOwner && r.name == repoName);

            // 対象メソッド実行
            await client.Admin.AdoptUnadoptedRepositoryAsync(repoOwner, repoName);

            // 有効なリポジトリとして取り込まれたことを確認
            var repos_after = await client.Repository.SearchAsync(repoName);
            repos_after.data.Should().Contain(r => r.owner!.login == repoOwner && r.name == repoName);

            // 削除しておく
            await client.Repository.DeleteAsync(repoOwner, repoName);
        }
        finally
        {
            // 作成したリポジトリのクリーンナップ
            await TestContainerHelper.TryExecAsync("rm", "-rf", repoPath);
        }
    }

    [TestMethod]
    public async Task DeleteUnadoptedRepositoryAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // コンテナ内に Unadopted なリポジトリを作成
        var repoOwner = this.TestTokenUser;
        var repoName = $"urepo-{DateTime.Now.Ticks:x16}";   // 小文字でなければならない
        var repoPath = $"/data/git/repositories/{repoOwner}/{repoName}.git";
        await TestContainerHelper.ExecAsync("git", "init", "--bare", repoPath);

        // 対象メソッド実行
        await client.Admin.DeleteUnadoptedRepositoryAsync(repoOwner, repoName);
    }

    [TestMethod]
    public async Task CreateUserRepoAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用リポジトリ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // 対象メソッド実行
        var repo = await client.Admin.CreateUserRepoAsync(repoOwner, new(repoName));
        repo.full_name.Should().Be($"{repoOwner}/{repoName}");

        // クリーンナップ
        try { await client.Repository.DeleteAsync(repoOwner, repoName); } catch { }
    }

    [TestMethod]
    public async Task CreateAndListOrganizationScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティの情報
        var orgOwner = this.TestTokenUser;
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // 組織作成
        var org = await client.Admin.CreateOrganizationAsync(orgOwner, new(orgName));

        // 組織リスト取得
        var org_list = await client.Admin.ListOrganizationsAsync();

        // クリーンナップ
        try { await client.Organization.DeleteAsync(orgName); } catch { }

        // 検証
        org.name.Should().Be(orgName);
        org_list.Should().Contain(o => o.name == orgName);

    }

    [TestMethod]
    public async Task UserManageScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティの情報
        var userName = $"user-{DateTime.Now.Ticks:x16}";
        var userMail = $"{userName}@example.com";

        // ユーザ作成
        var user_created = await client.Admin.CreateUserAsync(new(userMail, userName, password: userName));

        // ユーザリスト取得
        var user_list = await client.Admin.ListUsersAsync();

        // ユーザ更新
        var user_updated = await client.Admin.UpdateUserAsync(userName, new(userName, 0, full_name: "edited-user"));

        // ユーザリネーム
        await client.Admin.RenameUserAsync(userName, new($"new-{userName}"));

        // ユーザリスト取得
        var user_list_renamed = await client.Admin.ListUsersAsync();

        // ユーザ削除
        await client.Admin.DeleteUserAsync($"new-{userName}");

        // ユーザリスト取得
        var user_list_deleted = await client.Admin.ListUsersAsync();

        // 検証
        user_created.login.Should().Be(userName);
        user_created.email.Should().Be(userMail);

        user_list.Should().Contain(u => u.login == userName && u.email == $"{userName}@example.com");

        user_updated.login.Should().Be(userName);
        user_updated.full_name.Should().Be("edited-user");

        user_list_renamed.Should().NotContain(u => u.login == userName);
        user_list_renamed.Should().Contain(u => u.login == $"new-{userName}");

        user_list_deleted.Should().NotContain(u => u.login == $"new-{userName}");

    }

    [TestMethod]
    public async Task PublicKeyScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // SSHキー作成
        var key = await client.Admin.AddUserPublicKeyAsync(user.login!, new(TestConstants.TestSshPubKey, "test-pubkey"));

        // SSHキー削除
        await client.Admin.DeleteUserPublicKeyAsync(user.login!, key.id!.Value);

        // まとめて検証
        key.title.Should().Be("test-pubkey");
    }






}