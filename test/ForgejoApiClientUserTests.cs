using System.Globalization;
using System.Text;
using ForgejoApiClient.Api;
using ForgejoApiClient.Api.Extensions;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Utilities;

namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientUserTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task GetMeAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var me = await client.User.GetMeAsync();
        me.login.Should().Be("forgejo-admin");
        me.email.Should().Be("forgejo-admin@example.com");
    }

    [TestMethod]
    public async Task ListEmailsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var me = await client.User.GetMeAsync();
        var emails = await client.User.ListEmailsAsync();
        emails.Select(m => m.email).Should().Contain(me.email);
    }

    [TestMethod]
    public async Task AddDeleteEmailScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var emails = await client.User.ListEmailsAsync();

        var added = await client.User.AddEmailAsync(new(emails: ["aaa@example.com", "bbb@example.com"]));
        await client.User.DeleteEmailAsync(new(emails: ["aaa@example.com", "bbb@example.com"]));
        var deleted = await client.User.ListEmailsAsync();

        added.Select(m => m.email).Should().Contain(["aaa@example.com", "bbb@example.com"]);
        deleted.Select(m => m.email).Should().NotContain(["aaa@example.com", "bbb@example.com"]);
    }

    [TestMethod]
    public async Task UpdateDeleteAvatarScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        using var http = new HttpClient();

        // 適当な画像を作ってアバターに設定、アバター画像を取得
        var image = TestResourceGenerator.CreateTextImage("Test User Avator");
        var imageB64 = Convert.ToBase64String(image);
        await client.User.UpdateAvatarAsync(new(image: imageB64));
        var me1 = await client.User.GetMeAsync();
        var avatar1 = await http.GetByteArrayAsync(me1.avatar_url);

        // アバターを削除し、アバター画像を取得
        await client.User.DeleteAvatarAsync();
        var me2 = await client.User.GetMeAsync();
        var avatar2 = await http.GetByteArrayAsync(me2.avatar_url);

        // 検証
        image.Should().Equal(avatar1);
        image.Should().NotEqual(avatar2);
    }

    [TestMethod]
    public async Task GetSettingsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var settings = await client.User.GetSettingsAsync();
        CultureInfo.GetCultureInfo(settings.language!).Should().NotBeNull();
    }

    [TestMethod]
    public async Task UpdateSettingsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var settings = await client.User.UpdateSettingsAsync(new(language: "en-US"));
        await client.User.UpdateSettingsAsync(new(language: "ja-JP"));
        settings.language.Should().Be("en-US");
    }

    [TestMethod]
    public async Task ListTeamsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のチームを作成してユーザを追加
        await using var resources = new TestForgejoResources(client);
        var org = await resources.CreateTestOrgAsync($"org-{DateTime.Now.Ticks:X16}");
        var team = await resources.CreateTestTeamAsync(org.name!, $"team-{DateTime.Now.Ticks:X16}");
        await client.Organization.AddTeamMemberAsync((int)team.id!.Value, this.TestTokenUser);

        // テスト対象メソッドを実行
        var teams = await client.User.ListTeamsAsync();
        teams.Select(t => t.name).Should().Contain(team.name);
    }

    [TestMethod]
    public async Task ListUserActivitiesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用にリポジトリを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync($"repo-{DateTime.Now.Ticks:X16}");

        // テスト対象メソッドを実行
        var activities = await client.User.ListUserActivitiesAsync(repo.owner!.login!);
        activities.Should().Contain(a => a.op_type == "create_repo");
    }

    [TestMethod]
    public async Task ListUserHeatmapAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用になにか行動を起こす
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync($"repo-{DateTime.Now.Ticks:X16}");

        // テスト対象メソッドを実行
        var heatmaps = await client.User.ListUserHeatmapAsync(this.TestTokenUser);
        heatmaps.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task BlockUnblockAndListScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // ブロックユーザが存在する状態にする
        await client.User.BlockUserAsync(user.login!);

        // ブロックリストを取得
        (await client.User.ListBlockedUsersAsync()).Should().Contain(b => b.block_id == user.id);

        // ブロック解除する
        await client.User.UnblockUserAsync(user.login!);

        // ブロックリストを取得
        (await client.User.ListBlockedUsersAsync()).Should().NotContain(b => b.block_id == user.id);
    }

    [TestMethod]
    public async Task ListFollowerUsersAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // デフォルトコンテキストのユーザをフォロー
        await userClient.User.FollowUserAsync(this.TestTokenUser);

        // フォロワ取得
        var followers = await client.User.ListFollowerUsersAsync();
        followers.Should().Contain(f => f.login == user.login);
    }

    [TestMethod]
    public async Task FollowUnfollowAndListScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // テスト用ユーザをフォロー
        await client.User.FollowUserAsync(user.login!);

        // フォローチェック
        (await client.User.IsFollowingAsync(user.login!)).Should().Be(true);

        // フォローユーザを取得
        (await client.User.ListFollowingUsersAsync()).Should().Contain(f => f.login == user.login);

        // テスト用ユーザをアンフォロー
        await client.User.UnfollowUserAsync(user.login!);

        // 非フォローチェック
        (await client.User.IsFollowingAsync(user.login!)).Should().Be(false);

        // フォローユーザを取得
        (await client.User.ListFollowingUsersAsync()).Should().NotContain(f => f.login == user.login);
    }

    [TestMethod]
    public async Task ListUserFollowersAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // テスト用ユーザをフォロー
        await client.User.FollowUserAsync(user.login!);

        // テスト用ユーザのフォロワーを取得
        (await client.User.ListUserFollowersAsync(user.login!)).Should().Contain(f => f.login == this.TestTokenUser);
    }

    [TestMethod]
    public async Task UserFollowUnfollowAndListScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // デフォルトコンテキストのユーザをフォロー
        await userClient.User.FollowUserAsync(this.TestTokenUser);

        // テスト用ユーザのフォローチェック
        (await client.User.IsUserFollowingAsync(user.login!, this.TestTokenUser)).Should().Be(true);

        // テスト用ユーザのフォローユーザを取得
        (await client.User.ListUserFollowingsAsync(user.login!)).Should().Contain(f => f.login == this.TestTokenUser);

        // デフォルトコンテキストのユーザをアンフォロー
        await userClient.User.UnfollowUserAsync(this.TestTokenUser);

        // テスト用ユーザの非フォローチェック
        (await client.User.IsUserFollowingAsync(user.login!, this.TestTokenUser)).Should().Be(false);

        // テスト用ユーザのフォローユーザを取得
        (await client.User.ListUserFollowingsAsync(user.login!)).Should().NotContain(f => f.login == this.TestTokenUser);
    }

    [TestMethod]
    public async Task StarUnstarAndListScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用にリポジトリを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync($"repo-{DateTime.Now.Ticks:X16}");

        // リポジトリにスターを付ける
        await client.User.StarRepositoryAsync(repo.owner!.login!, repo.name!);

        // スターのあるリポジトリを取得
        (await client.User.ListStarredRepositoriesAsync()).Should().Contain(s => s.full_name == repo.full_name);

        // リポジトリのスターを取得
        var star = await client.User.CheckStarredAsync(repo.owner!.login!, repo.name!);

        // リポジトリのスターを解除する
        await client.User.UnstarRepositoryAsync(repo.owner!.login!, repo.name!);

        // スターのあるリポジトリを取得
        (await client.User.ListStarredRepositoriesAsync()).Should().NotContain(s => s.full_name == repo.full_name);

        // リポジトリのスターを取得
        var star2 = await client.User.CheckStarredAsync(repo.owner!.login!, repo.name!);
    }

    [TestMethod]
    public async Task ListUserStarredAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザとリポジトリを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");
        var repo = await resources.CreateTestRepoAsync($"repo-{DateTime.Now.Ticks:X16}");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // テスト用ユーザでリポジトリにスター
        await userClient.User.StarRepositoryAsync(repo.owner!.login!, repo.name!);

        // テスト用ユーザのスターを付けたリポジトリを取得
        (await client.User.ListUserStarredAsync(user.login!)).Should().Contain(r => r.full_name == repo.full_name);
    }

    [TestMethod]
    public async Task SearchAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成する。
        await using var resources = new TestForgejoResources(client);
        var user1 = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");
        var user2 = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // ユーザ検索
        (await client.User.SearchAsync(user1.login![5..])).data
            .Should().Contain(r => r.login == user1.login)
            .And.NotContain(r => r.login == user2.login);
    }

    [TestMethod]
    public async Task GetAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // ユーザ情報を取得
        (await client.User.GetAsync(user.login!)).login.Should().Be(user.login);
    }

    [TestMethod]
    public async Task ListRepositoriesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のリポジトリを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync($"repo-{DateTime.Now.Ticks:X16}");

        // リポジトリリストを取得
        (await client.User.ListRepositoriesAsync()).Should().Contain(r => r.full_name == repo.full_name);
    }

    [TestMethod]
    public async Task CreateAndListScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // リポジトリ作成
        var repo = await client.User.CreateRepositoryAsync(new(name: $"repo-{DateTime.Now.Ticks:X16}"));
        repo.owner!.login.Should().Be(this.TestTokenUser);

        // リスト取得
        var repos = await client.User.ListRepositoriesAsync();
        repos.Should().ContainEquivalentOf(repo);

        // 後片付け
        await client.Repository.DeleteAsync(repo.owner!.login!, repo.name!);
    }

    [TestMethod]
    public async Task ListUserRepositoriesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");
        var repo = await resources.CreateTestUserRepoAsync(user.login!, $"repo-{DateTime.Now.Ticks:X16}");

        // ウォッチリポジトリリストを取得
        (await client.User.ListUserRepositoriesAsync(user.login!)).Should().Contain(r => r.full_name == repo.full_name);
    }

    [TestMethod]
    public async Task ListSubscriptionsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のリポジトリを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync($"repo-{DateTime.Now.Ticks:X16}");

        // リポジトリをウォッチ
        await client.Repository.WatchAsync(repo.owner!.login!, repo.name!);

        // ウォッチリポジトリリストを取得
        (await client.User.ListSubscriptionsAsync()).Should().Contain(r => r.full_name == repo.full_name);
    }

    [TestMethod]
    public async Task ListUserSubscriptionsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザとリポジトリを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");
        var repo = await resources.CreateTestRepoAsync($"repo-{DateTime.Now.Ticks:X16}");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // テスト用ユーザでリポジトリをウォッチ
        await userClient.Repository.WatchAsync(repo.owner!.login!, repo.name!);

        // ウォッチリポジトリリストを取得
        (await client.User.ListUserSubscriptionsAsync(repo.owner!.login!)).Should().Contain(r => r.full_name == repo.full_name);
    }

    [TestMethod]
    public async Task StopwatcheAndTimesScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のリポジトリとイシューを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync($"repo-{DateTime.Now.Ticks:X16}");
        var issue = await resources.CreateTestIssueAsync(repo.owner!.login!, repo.name!, $"issue-{DateTime.Now.Ticks:X16}");

        // イシューでストップウォッチを開始
        await client.Issue.StartStopwatchAsync(repo.owner!.login!, repo.name!, issue.number!.Value);

        // ストップウォッチリストを取得
        (await client.User.ListStopwatchesAsync()).Should().Contain(w => w.repo_name == repo.name);

        // イシューでストップウォッチを停止
        await client.Issue.StopStopwatchAsync(repo.owner!.login!, repo.name!, issue.number!.Value);

        // 時間の取得
        var times = await client.User.ListTimesAsync();
        times.Should().Contain(t => t.issue!.number == issue.number);
    }

    [TestMethod]
    public async Task GpgKeyTokenVerifyScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // GPGキー作成
        var key = await client.User.CreateGpgKeyAsync(new(armored_public_key: TestConstants.TestGpgPubKey));
        try
        {
            // 検証トークン取得
            var token = await client.User.GetGpgKeyTokenAsync();

            // 署名作成
            using var pubKeyStream = new ArmoredInputStream(new MemoryStream(Strings.ToUtf8ByteArray(TestConstants.TestGpgPubKey), writable: false));
            var pubKeyFactory = new PgpObjectFactory(pubKeyStream);
            var pubKeyRing = pubKeyFactory.FilterPgpObjects<PgpPublicKeyRing>().First();
            var pubKey = pubKeyRing.GetPublicKey();

            using var prvKeyStream = new ArmoredInputStream(new MemoryStream(Strings.ToUtf8ByteArray(TestConstants.TestGpgPrivateKey), writable: false));
            var prvKeyFactory = new PgpObjectFactory(prvKeyStream);
            var prvKeyRing = prvKeyFactory.FilterPgpObjects<PgpSecretKeyRing>().First();
            var prvKey = prvKeyRing.GetSecretKey();

            var sigGen = new PgpSignatureGenerator(pubKey.Algorithm, HashAlgorithmTag.Sha1);
            sigGen.InitSign(PgpSignature.CanonicalTextDocument, prvKey.ExtractPrivateKey(TestConstants.TestGpgPassphrase.ToArray()));
            sigGen.Update(Strings.ToAsciiByteArray(token));

            using var outStream = new MemoryStream();
            using (var armoredStream = new ArmoredOutputStream(outStream))
            using (var bcpgStream = new BcpgOutputStream(armoredStream))
            {
                sigGen.Generate().Encode(bcpgStream);
            }

            var signature = Encoding.UTF8.GetString(outStream.ToArray());

            // 署名検証
            var verify = await client.User.VerifyGpgKeyTokenAsync(new(TestConstants.TestGpgKeyID, signature));
            verify.Should().NotBeNull();
        }
        finally
        {
            try { await client.User.DeleteGpgKeyAsync(key.id!.Value); } catch { }
        }

    }

    [TestMethod]
    public async Task GpgKeyScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // GPGキー作成
        var key = await client.User.CreateGpgKeyAsync(new(armored_public_key: TestConstants.TestGpgPubKey));

        // GPGキー取得
        var ket_get = await client.User.GetGpgKeyAsync(key.id!.Value);

        // GPGキーリスト取得
        var key_list = await client.User.ListGpgKeysAsync();

        // GPGキーリスト取得 (ユーザ名指定)
        var key_list_usr = await client.User.ListUserGpgKeysAsync(this.TestTokenUser);

        // GPGキー削除
        await client.User.DeleteGpgKeyAsync(key.id!.Value);

        // まとめて検証
        key.key_id.Should().Be(TestConstants.TestGpgKeyID);
        ket_get.key_id.Should().Be(TestConstants.TestGpgKeyID);
        key_list.Should().Contain(k => k.key_id == TestConstants.TestGpgKeyID);
        key_list_usr.Should().Contain(k => k.key_id == TestConstants.TestGpgKeyID);
    }

    [TestMethod]
    public async Task PublicKeyScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // SSHキー作成
        var key = await client.User.AddPublicKeyAsync(new(key: TestConstants.TestSshPubKey, title: "test-pubkey"));

        // SSHキー取得
        var ket_get = await client.User.GetPublicKeyAsync(key.id!.Value);

        // SSHキーリスト取得
        var key_list = await client.User.ListPublicKeysAsync();

        // SSHキーリスト取得 (ユーザ名指定)
        var key_list_usr = await client.User.ListUserPublicKeysAsync(this.TestTokenUser);

        // SSHキー削除
        await client.User.DeletePublicKeyAsync(key.id!.Value);

        // まとめて検証
        key.title.Should().Be("test-pubkey");
        ket_get.title.Should().Be("test-pubkey");
        key_list.Should().Contain(k => k.title == "test-pubkey");
        key_list_usr.Should().Contain(k => k.title == "test-pubkey");
    }

    [TestMethod]
    public async Task WebhookScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // Webhook作成
        var webhook = await client.User.CreateWebhookAsync(
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
        var webhook_get = await client.User.GetWebhookAsync(webhook.id!.Value);

        // Webhookリスト取得
        var webhook_list = await client.User.ListWebhooksAsync();

        // Webhook更新
        var webhook_updated = await client.User.UpdateWebhookAsync(
            webhook.id!.Value,
            new(config: new Dictionary<string, string>
            {
                ["url"] = "http://localhost:9998",
            })
        );

        // Webhook削除
        await client.User.DeleteWebhookAsync(webhook.id!.Value);

        // まとめて検証
        webhook.url.Should().Be("http://localhost:9999");
        webhook_get.url.Should().Be("http://localhost:9999");
        webhook_list.Should().Contain(k => k.url == "http://localhost:9999");
        webhook_updated.url.Should().Be("http://localhost:9998");
    }

    [TestMethod]
    public async Task GetActionRunnerRegistrationTokenAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        var result = await client.User.GetActionRunnerRegistrationTokenAsync();
        result.token.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task SetDeleteSecretScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        var secretname = $"secret_{DateTime.Now.Ticks:X16}";
        await client.User.SetActionSecretAsync(secretname, new CreateOrUpdateSecretOption("test-data"));
        await client.User.SetActionSecretAsync(secretname, new CreateOrUpdateSecretOption("test-data-updated"));
        await client.User.DeleteActionSecretAsync(secretname);
    }

    [TestMethod]
    public async Task OAuth2Scenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // アプリケーション管理作成
        var app = await client.User.CreateOAuth2ApplicationAsync(new(name: $"app-{DateTime.Now.Ticks:X16}", redirect_uris: ["http://localhost:9989"]));

        // アプリケーション管理取得
        var app_get = await client.User.GetOAuth2ApplicationAsync(app.id!.Value);

        // アプリケーション管理リスト取得
        var app_list = await client.User.ListOAuth2ApplicationsAsync();

        // アプリケーション管理更新
        var app_updated = await client.User.UpdateOAuth2ApplicationAsync(app.id!.Value, new(name: $"app-{DateTime.Now.Ticks:X16}", redirect_uris: ["http://localhost:9988"]));

        // アプリケーション管理削除
        await client.User.DeleteOAuth2ApplicationAsync(app.id!.Value);

        // まとめて検証
        app.redirect_uris.Should().Contain("http://localhost:9989");
        app_get.redirect_uris.Should().Contain("http://localhost:9989");
        app_list.SelectMany(a => a.redirect_uris!).Should().Contain("http://localhost:9989");
        app_updated.redirect_uris.Should().Contain("http://localhost:9988");
    }

    [TestMethod]
    public async Task AccessTokenScenario()
    {
        // アクセストークン関係のAPIはなぜかBASIC認証かリバースプロキシ認証を要求する。
        // 認証情報を作るに等しいので、別の認証方式で正統性を求めるというような意図だろうか？

        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用のユーザを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // データ
        var username = this.TestTokenUser;
        var tokenname = $"api-{DateTime.Now.Ticks:X16}";
        var scopes = new string[]
        {
            "read:activitypub",
            "read:admin",
            "read:issue",
            "read:misc",
            "read:notification",
            "read:organization",
            "read:package",
        };

        // アクセストークン作成
        var token = await client.User.CreateUserApiTokenAsync(this.TestAdminUser, username, new(name: tokenname, scopes: scopes));

        // アプリケーション管理リスト取得
        var token_list = await client.User.ListUserApiTokensAsync(this.TestAdminUser, username);

        // アプリケーション管理削除
        await client.User.DeleteUserApiTokenAsync(this.TestAdminUser, username, token.name!);

        // まとめて検証
        token.name.Should().Be(tokenname);
        token_list.Should().Contain(t => t.name == tokenname);
    }
}
