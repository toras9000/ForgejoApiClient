using System.Text;
using ForgejoApiClient.Api;
using ForgejoApiClient.Api.Extensions;

namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientOrganizationTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task OrganizationSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // 組織作成
        var org_created = await client.Organization.CreateAsync(new(username: orgName));

        // 組織取得
        var org_get = await client.Organization.GetAsync(orgName);

        // 組織更新
        var org_updated = await client.Organization.UpdateAsync(org_created.name!, new(description: "updated-org"));

        // 組織リスト取得
        var org_list = await client.Organization.ListAsync();

        // 組織リスト取得
        var org_mylist = await client.Organization.ListMyOrgsAsync();

        // 組織更新
        await client.Organization.DeleteAsync(org_created.name!);

        // 検証
        org_created.name.Should().Be(orgName);
        org_get.name.Should().Be(orgName);
        org_updated.description.Should().Be("updated-org");
        org_list.Select(o => o.description).Should().Contain("updated-org");
        org_mylist.Select(o => o.description).Should().Contain("updated-org");
    }

    [TestMethod]
    public async Task ListUserOrgsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // 組織作成
        var org1 = await userClient.Organization.CreateAsync(new(username: $"org1-{DateTime.Now.Ticks:X16}"));
        var org2 = await userClient.Organization.CreateAsync(new(username: $"org2-{DateTime.Now.Ticks:X16}"));

        // 組織リスト取得
        var orgs = await client.Organization.ListUserOrgsAsync(user.login!);

        // 組織削除
        await userClient.Organization.DeleteAsync(org1.name!);
        await userClient.Organization.DeleteAsync(org2.name!);

        // 検証
        orgs.Should().HaveCount(2);
    }

    [TestMethod]
    public async Task OrgMemberSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var team = await resources.CreateTestTeamAsync(org.name!, $"team-{DateTime.Now.Ticks:X16}");
        var user1 = await resources.CreateTestUserAsync($"user1-{DateTime.Now.Ticks:X16}");
        var user2 = await resources.CreateTestUserAsync($"user2-{DateTime.Now.Ticks:X16}");

        // チームにユーザを追加
        await client.Organization.AddTeamMemberAsync(team.id!.Value, user1.login!);

        // ユーザの所属状態チェック
        var user1_ismember = await client.Organization.IsMemberAsync(org.name!, user1.login!);
        var user2_ismember = await client.Organization.IsMemberAsync(org.name!, user2.login!);
        user1_ismember.Should().BeTrue();
        user2_ismember.Should().BeFalse();

        // 組織メンバ取得
        var members = await client.Organization.ListMembersAsync(orgName);
        members.Should().Contain(m => m.login == user1.login).And.NotContain(m => m.login == user2.login);

        // チームにユーザを追加
        await client.Organization.RemoveMemberAsync(orgName, user1.login!);

        // 組織メンバ取得
        var removed = await client.Organization.ListMembersAsync(orgName);
        removed.Should().NotContain(m => m.login == user1.login).And.NotContain(m => m.login == user2.login);
    }

    [TestMethod]
    public async Task OrgPublicizeSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var team = await resources.CreateTestTeamAsync(org.name!, $"team-{DateTime.Now.Ticks:X16}");
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // チームにユーザを追加
        await client.Organization.AddTeamMemberAsync(team.id!.Value, user.login!);

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // 組織でユーザを公開。WebUIと異なり、管理者でも他者を公開できないらしい。
        await userClient.Organization.PublicizeMemberAsync(orgName, user.login!);

        // ユーザが公開されているかを取得
        var userPublic = await client.Organization.IsPublicMemberAsync(orgName, user.login!);
        userPublic.Should().BeTrue();

        // ユーザが公開されているかを取得
        var users = await client.Organization.ListPublicMembersAsync(orgName);
        users.Should().Contain(m => m.login == user.login);

        // 組織でユーザを非公開
        await userClient.Organization.ConcealMemberAsync(orgName, user.login!);

        // 後始末
        await client.Organization.RemoveTeamMemberAsync(team.id!.Value, user.login!);
    }

    [TestMethod]
    public async Task OrgRepoSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // 組織にリポジトリを作成
        var repo = await client.Organization.CreateRepositoryAsync(orgName, new(name: $"repo-{DateTime.Now.Ticks:X16}")).WillBeDiscarded(resources);

        // 組織のリポジトリリストを取得
        var repos = await client.Organization.ListRepositoriesAsync(orgName);
        repos.Should().Contain(r => r.name == repo.name);
    }

    [TestMethod]
    public async Task GetActionRunnerRegistrationTokenAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // テスト対象呼び出し
        var result = await client.Organization.GetActionRunnerRegistrationTokenAsync(orgName);
        result.token.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task GetActionJobsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // テスト対象呼び出し
        var jobs = await client.Organization.GetActionJobsAsync(orgName, "node");
    }

    [TestMethod]
    public async Task ListActivitiesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var repo = await resources.CreateTestOrgRepoAsync(orgName, $"repo-{DateTime.Now.Ticks:X16}");

        // テスト対象呼び出し
        var activities = await client.Organization.ListActivitiesAsync(orgName);
        activities.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task UpdateDeleteAvatarScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        using var http = new HttpClient();

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // 適当な画像を作ってアバターに設定
        var image = TestResourceGenerator.CreateTextImage("Test User Avator");
        var imageB64 = Convert.ToBase64String(image);
        await client.Organization.UpdateAvatarAsync(orgName, new(image: imageB64));

        // アバター画像を検証
        var org_updated = await client.Organization.GetAsync(orgName);
        var avatar1 = await http.GetByteArrayAsync(org_updated.avatar_url);
        image.Should().Equal(avatar1);

        // アバターを削除
        await client.Organization.DeleteAvatarAsync(orgName);

        // アバター画像を検証
        var org_deleted = await client.Organization.GetAsync(orgName);
        var avatar2 = await http.GetByteArrayAsync(org_deleted.avatar_url);
        image.Should().NotEqual(avatar2);
    }

    [TestMethod]
    public async Task ActionSecretScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";
        var secret1Name = $"secret1";
        var secret2Name = $"secret2";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // secretの設定
        await client.Organization.SetActionSecretAsync(orgName, secret1Name, new(data: "AAA"));
        await client.Organization.SetActionSecretAsync(orgName, secret2Name, new(data: "BBB"));

        // secretの取得
        var secrets1 = await client.Organization.ListActionSecretsAsync(orgName);
        secrets1.Select(s => s.name).Should().BeEquivalentTo([secret1Name, secret2Name], config: c => c.Using(StringComparer.OrdinalIgnoreCase));

        // secretの更新
        await client.Organization.SetActionSecretAsync(orgName, secret1Name, new(data: "CCC"));
        await client.Organization.SetActionSecretAsync(orgName, secret2Name, new(data: "DDD"));

        // secretの取得
        var secrets2 = await client.Organization.ListActionSecretsAsync(orgName);
        secrets2.Select(s => s.name).Should().BeEquivalentTo([secret1Name, secret2Name], config: c => c.Using(StringComparer.OrdinalIgnoreCase));

        // secretの削除
        await client.Organization.DeleteActionSecretAsync(orgName, secret1Name);
        await client.Organization.DeleteActionSecretAsync(orgName, secret2Name);

        // secretの取得
        var secrets3 = await client.Organization.ListActionSecretsAsync(orgName);
        secrets3.Should().BeEmpty();
    }

    [TestMethod]
    public async Task ActionVariableScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";
        var varName = $"varname";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // variable作成
        await client.Organization.CreateActionVariableAsync(orgName, varName, new(value: "AAA"));

        // variable取得
        var variable = await client.Organization.GetActionVariableAsync(orgName, varName);
        variable.data.Should().Be("AAA");

        // variable更新
        await client.Organization.UpdateActionVariableAsync(orgName, varName, new(value: "BBB"));

        // リリースリスト取得
        var variable_list = await client.Organization.ListActionVariablesAsync(orgName);
        variable_list.Should().Contain(v => string.Equals(v.name, varName, StringComparison.OrdinalIgnoreCase) && v.data == "BBB");

        // リリース情報削除
        await client.Organization.DeleteActionVariableAsync(orgName, varName);
    }

    [TestMethod]
    public async Task BlockUnblockSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // ユーザをブロック
        await client.Organization.BlockUserFromAsync(orgName, user.login!);

        // ブロックリストを取得
        var blocked = await client.Organization.ListBlockedUsersAsync(orgName);
        blocked.Should().Contain(b => b.block_id == user.id);

        // ユーザをブロック解除
        await client.Organization.UnblockUserFromAsync(orgName, user.login!);
    }

    [TestMethod]
    public async Task WebhookSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // Webhook作成
        var create_config = new Dictionary<string, string> { ["content_type"] = "json", ["url"] = "http://localhost:9999", };
        var webhook = await client.Organization.CreateWebhookAsync(orgName, new(type: CreateHookOptionType.Forgejo, config: create_config));

        // Webhook取得
        var webhook_get = await client.Organization.GetWebhookAsync(orgName, webhook.id!.Value);

        // Webhook更新
        var update_config = new Dictionary<string, string> { ["content_type"] = "json", ["url"] = "http://localhost:8888", };
        var webhook_updated = await client.Organization.UpdateWebhookAsync(orgName, webhook_get.id!.Value, new(config: update_config));

        // Webhookリスト取得
        var webhook_list = await client.Organization.ListWebhooksAsync(orgName);

        // 組織更新
        await client.Organization.DeleteWebhookAsync(orgName, webhook_updated.id!.Value);

        // 検証
        webhook.url.Should().Be("http://localhost:9999");
        webhook_get.url.Should().Be("http://localhost:9999");
        webhook_updated.url.Should().Be("http://localhost:8888");
        webhook_list.Should().Contain(k => k.url == "http://localhost:8888");
    }

    [TestMethod]
    public async Task LabelSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // ラベル作成
        var label = await client.Organization.CreateLabelAsync(orgName, new(name: $"labe-{DateTime.Now.Ticks:X16}", color: "#ff0000"));

        // ラベル取得
        var label_get = await client.Organization.GetLabelAsync(orgName, label.id!.Value);

        // ラベル更新
        var label_updated = await client.Organization.UpdateLabelAsync(orgName, label_get.id!.Value, new(color: "#00ff00"));

        // ラベルリスト取得
        var label_list = await client.Organization.ListLabelsAsync(orgName);

        // 組織更新
        await client.Organization.DeleteLabelAsync(orgName, label_updated.id!.Value);

        // 検証
        label.color.Should().Be("ff0000");
        label_get.color.Should().Be("ff0000");
        label_updated.color.Should().Be("00ff00");
        label_list.Should().Contain(k => k.color == "00ff00");
    }

    [TestMethod]
    public async Task TeamSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";
        var teamName = $"team-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);

        // チーム作成
        var create_units = new Dictionary<string, string> { [""] = "", };
        var team = await client.Organization.CreateTeamAsync(orgName, new(name: teamName, description: "created", units_map: create_units));

        // チーム取得
        var team_get = await client.Organization.GetTeamAsync(team.id!.Value);

        // チーム更新
        var team_updated = await client.Organization.UpdateTeamAsync(team.id!.Value, new(name: teamName, description: "updated"));

        // チームリスト取得
        var team_list = await client.Organization.ListTeamsAsync(orgName);

        // チーム検索
        var team_search = await client.Organization.SearchTeamsAsync(orgName);

        // チーム更新
        await client.Organization.DeleteTeamAsync(team.id!.Value);

        // 検証
        team.name.Should().Be(teamName);
        team.description.Should().Be("created");
        team_get.name.Should().Be(teamName);
        team_get.description.Should().Be("created");
        team_updated.name.Should().Be(teamName);
        team_updated.description.Should().Be("updated");
        team_list.Should().Contain(t => t.name == teamName && t.description == "updated");
        team_search.data.Should().Contain(t => t.name == teamName && t.description == "updated");
    }

    [TestMethod]
    public async Task TeamMemberSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";
        var teamName = $"team-{DateTime.Now.Ticks:X16}";
        var userName = $"user-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var team = await resources.CreateTestTeamAsync(orgName, teamName);
        var user = await resources.CreateTestUserAsync(userName);

        // チームにユーザを追加
        await client.Organization.AddTeamMemberAsync(team.id!.Value, userName);

        // チームメンバ情報取得
        var member = await client.Organization.GetTeamMemberAsync(team.id!.Value, userName);
        member.login.Should().Be(userName);

        // チームメンバリスト取得
        var members = await client.Organization.ListTeamMembersAsync(team.id!.Value);
        members.Should().Contain(m => m.login == userName);

        // チームメンバ削除
        await client.Organization.RemoveTeamMemberAsync(team.id!.Value, userName);
    }

    [TestMethod]
    public async Task TeamRepoSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";
        var teamName = $"team-{DateTime.Now.Ticks:X16}";
        var userName = $"user-{DateTime.Now.Ticks:X16}";
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var team = await resources.CreateTestTeamAsync(orgName, teamName);
        var user = await resources.CreateTestUserAsync(userName);
        var repo = await resources.CreateTestOrgRepoAsync(orgName, repoName);

        // チームにリポジトリアクセスを追加
        await client.Organization.AddTeamRepositoryAsync(team.id!.Value, orgName, repoName);

        // チームにリポジトリ情報取得
        var team_repo = await client.Organization.GetTeamRepositoryAsync(team.id!.Value, orgName, repoName);
        team_repo.name.Should().Be(repoName);

        // チームリポジトリリスト取得
        var team_repos = await client.Organization.ListTeamRepositoriesAsync(team.id!.Value);
        team_repos.Should().Contain(m => m.name == repoName);

        // チームリポジトリ削除
        await client.Organization.RemoveTeamRepositoryAsync(team.id!.Value, orgName, repoName);
    }

    [TestMethod]
    public async Task ListTeamActivitiesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";
        var teamName = $"team-{DateTime.Now.Ticks:X16}";
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var team = await resources.CreateTestTeamAsync(orgName, teamName);
        var repo = await resources.CreateTestOrgRepoAsync(orgName, repoName);

        // リポジトリをチームに追加
        await client.Organization.AddTeamRepositoryAsync(team.id!.Value, orgName, repoName);

        // テスト対象呼び出し
        var activities = await client.Organization.ListTeamActivitiesAsync(team.id!.Value);
        activities.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetUserPermissionsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";
        var teamName = $"team-{DateTime.Now.Ticks:X16}";
        var userName = $"uese-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var team = await resources.CreateTestTeamAsync(orgName, teamName);
        var user = await resources.CreateTestUserAsync(userName);

        // テスト対象呼び出し
        var permissions = await client.Organization.GetUserPermissionsAsync(userName, orgName);
    }

    [TestMethod]
    public async Task QuotaSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var orgName = $"org-{DateTime.Now.Ticks:X16}";
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var pkgName = "Dummy";
        var pkgVer = "0.0.0";
        var pkgFile = TestPathHelper.GetProjectDir().RelativeFile($"assets/packages/{pkgName}.{pkgVer}.nupkg");
        var txtFile = TestPathHelper.GetProjectDir().RelativeFile($"assets/texts/DummyText.txt");

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync(orgName);
        var repo = await resources.CreateTestOrgRepoAsync(orgName, repoName);
        var issue = await resources.CreateTestIssueAsync(orgName, repoName, issueTitle);
        var rule = await resources.CreateTestQuotaRuleAsync($"rule-{DateTime.Now.Ticks:X16}", 100 * 1024 * 1024, ["size:assets:attachments:all"]);
        var quotaGroup = await resources.CreateTestQuotaGroupAsync($"qg-{DateTime.Now.Ticks:X16}", [new(name: rule.name),]);

        // テスト用のパッケージをアップロード
        await this.UploadPackageAsync(orgName, this.TestToken, pkgFile);

        // パッケージ情報取得
        var package = await client.Package.GetAsync(orgName, "nuget", pkgName, pkgVer);

        // Attachment作成
        var attach = await client.Issue.CreateFileAttachmentAsync(orgName, repoName, issue.number!.Value, txtFile);

        // artifacts クォータ使用リスト取得
        var quota_artifacts = await client.Organization.ListQuotaArtifactsAsync(orgName);

        // attachments クォータ使用リスト取得
        var quota_attachments = await client.Organization.ListQuotaAttachmentsAsync(orgName);

        // packages クォータ使用リスト取得
        var quota_packages = await client.Organization.ListQuotaPackagesAsync(orgName);
        quota_packages.Should().NotBeEmpty();

        // ユーザにクォータグループを設定
        await client.Admin.SetUserQuotaGroupAsync(orgName, new([quotaGroup.name!]));

        // クォータ情報取得
        var quota = await client.Organization.GetQuotaAsync(orgName);
        quota.used.Should().NotBeNull();

        // クォータ超過取得
        var quota_within = await client.Organization.CheckQuotaOverAsync(orgName, "size:git:all");
        quota_within.Should().BeFalse();

        // パッケージ削除
        await client.Package.DeleteAsync(orgName, "nuget", pkgName, pkgVer);
    }

}
