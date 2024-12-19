using ForgejoApiClient.Api;
using ForgejoApiClient.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientRepositoryTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task MigrateAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo1-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // リポジトリのマイグレーション
        var migrate_addr = $"http://localhost:3000/{repoOwner}/{repoName}";
        var migrate_name = $"repo2-{DateTime.Now.Ticks:X16}";
        var migrate_options = new MigrateRepoOptions(
            clone_addr: migrate_addr,
            repo_owner: repoOwner,
            repo_name: migrate_name,
            auth_token: this.TestToken,
            mirror: true
        );
        var migrated = await client.Repository.MigrateAsync(migrate_options);

        // ミラーの同期
        await client.Repository.SyncMirroredAsync(repoOwner, migrate_name);

        // 後始末
        await client.Repository.DeleteAsync(repoOwner, migrate_name);
    }

    [TestMethod]
    public async Task RepositorySenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // リポジトリ作成
        var repo = await client.Repository.CreateAsync(new(name: repoName));

        // リポジトリ検索
        var repos = await client.Repository.SearchAsync();
        repos.data.Should().Contain(r => r.name == repoName);

        // リポジトリ取得
        var repo_get = await client.Repository.GetAsync(this.TestTokenUser, repoName);
        repo_get.name.Should().Be(repoName);

        // リポジトリ取得(by ID)
        var repo_get_id = await client.Repository.GetAsync(repo_get.id!.Value);
        repo_get_id.name.Should().Be(repoName);

        // リポジトリ更新
        var repo_updated = await client.Repository.UpdateAsync(this.TestTokenUser, repoName, new(description: "updated"));
        repo_updated.description.Should().Be("updated");

        // リポジトリ削除
        await client.Repository.DeleteAsync(this.TestTokenUser, repoName);
    }

    [TestMethod]
    public async Task TemplateRepositorySenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var templateName = $"template-{DateTime.Now.Ticks:X16}";
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // リソースクリーンナップ用。
        await using var resources = new TestForgejoResources(client);

        // テンプレートリポジトリ作成
        var template = await client.Repository.CreateAsync(new(name: templateName, template: true)).WillBeDiscarded(resources);

        // テンプレートからリポジトリ作成
        var repo = await client.Repository.CreateUsingTemplateAsync(ownerName, templateName, new(owner: ownerName, name: repoName, git_content: true)).WillBeDiscarded(resources);
    }

    [TestMethod]
    public async Task BranchScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var branchName = "test";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // リポジトリに適当な内容をpush
        using var repoDir = new TestTempRepo(repo.clone_url!);
        repoDir.Auther = "test-auther";
        repoDir.AutherMail = "test-auther@example.com";
        repoDir.Commit("commit1", dir =>
        {
            dir.RelativeFile("aaa.cs").WriteAllText("using System;");
            dir.RelativeFile("bbb.cs").WriteAllText("using System;");
        });
        repoDir.Push(this.TestTokenUser, this.TestToken);

        // pushしたブランチの情報を取得。少し時間が経たないと情報が得られないようなのでしばらく繰り返して取得する。
        await TestCallHelper.TrySatisfy(
            caller: breaker => client.Repository.GetBranchAsync(ownerName, repoName, repoDir.Repo.Head.FriendlyName, cancelToken: breaker),
            condition: pushed => pushed.name == repoDir.Repo.Head.FriendlyName
        );

        // ブランチ作成
        var branch_created = await client.Repository.CreateBranchAsync(ownerName, repoName, new(new_branch_name: branchName));

        // ブランチ情報取得
        var branch_get = await client.Repository.GetBranchAsync(ownerName, repoName, branchName);

        // ブランチリスト取得
        var branch_list = await client.Repository.ListBranchesAsync(ownerName, repoName);

        // ブランチ削除
        await client.Repository.DeleteBranchAsync(ownerName, repoName, branchName);

        // 検証
        branch_created.name.Should().Be(branchName);
        branch_get.name.Should().Be(branchName);
        branch_list.Select(p => p.name).Should().Contain(branchName);
    }

    [TestMethod]
    public async Task BranchProtectionScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // リポジトリに適当な内容をpush
        using var repoDir = new TestTempRepo(repo.clone_url!);
        repoDir.Auther = "test-auther";
        repoDir.AutherMail = "test-auther@example.com";
        repoDir.Commit("commit1", dir =>
        {
            dir.RelativeFile("aaa.cs").WriteAllText("using System;");
            dir.RelativeFile("bbb.cs").WriteAllText("using System;");
        });
        repoDir.Push(this.TestTokenUser, this.TestToken);

        // ブランチプロテクト
        var protection = await client.Repository.CreateBranchProtectionAsync(ownerName, repoName, new(branch_name: repoDir.Repo.Head.FriendlyName, enable_status_check: false));

        // ブランチプロテクト情報取得
        var protection_get = await client.Repository.GetBranchProtectionAsync(ownerName, repoName, repoDir.Repo.Head.FriendlyName);

        // ブランチプロテクト情報更新
        var protection_updated = await client.Repository.UpdateBranchProtectionAsync(ownerName, repoName, repoDir.Repo.Head.FriendlyName, new(enable_status_check: true));

        // ブランチプロテクトリスト取得
        var protection_list = await client.Repository.ListBranchProtectionsAsync(ownerName, repoName);

        // ブランチプロテクト削除
        await client.Repository.DeleteBranchProtectionAsync(ownerName, repoName, repoDir.Repo.Head.FriendlyName);

        // 検証
        protection.branch_name.Should().Be(repoDir.Repo.Head.FriendlyName);
        protection_get.branch_name.Should().Be(repoDir.Repo.Head.FriendlyName);
        protection_updated.branch_name.Should().Be(repoDir.Repo.Head.FriendlyName);
        protection_list.Select(p => p.branch_name).Should().Contain(repoDir.Repo.Head.FriendlyName);
    }

    [TestMethod]
    public async Task TagScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var tagName = $"tag-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // タグ作成
        var tag = await client.Repository.CreateTagAsync(ownerName, repoName, new(tag_name: tagName));
        tag.name.Should().Be(tagName);

        // タグ取得
        var tag_get = await client.Repository.GetTagAsync(ownerName, repoName, tagName);
        tag_get.name.Should().Be(tagName);

        // タグリスト取得
        var tag_list = await client.Repository.ListTagsAsync(ownerName, repoName);
        tag_list.Should().Contain(t => t.name == tagName);

        // タグ削除
        await client.Repository.DeleteTagAsync(ownerName, repoName, tagName);

        // タグリスト取得
        var tag_deleted = await client.Repository.ListTagsAsync(ownerName, repoName);
        tag_deleted.Should().NotContain(t => t.name == tagName);
    }

    [TestMethod]
    public async Task TagProtectionScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var userName = $"user-{DateTime.Now.Ticks:X16}";
        var tagName = $"tag-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);
        var user = await resources.CreateTestUserAsync(userName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // タグ作成
        var tag = await client.Repository.CreateTagAsync(repoOwner, repoName, new(tag_name: tagName));

        // タグプロテクト
        var protection = await client.Repository.CreateTagProtectionAsync(repoOwner, repoName, new(name_pattern: tagName, whitelist_usernames: [this.TestTokenUser]));
        protection.name_pattern.Should().Be(tagName);

        // タグプロテクト情報取得
        var protection_get = await client.Repository.GetTagProtectionAsync(repoOwner, repoName, protection.id!.Value);
        protection_get.name_pattern.Should().Be(tagName);

        // タグプロテクト情報更新
        var protection_updated = await client.Repository.UpdateTagProtectionAsync(repoOwner, repoName, protection.id!.Value, new(whitelist_usernames: [userName]));
        protection_updated.name_pattern.Should().Be(tagName);

        // タグプロテクトリスト取得
        var protection_list = await client.Repository.ListTagProtectionsAsync(repoOwner, repoName);
        protection_list.Select(p => p.name_pattern).Should().Contain(tagName);

        // タグプロテクト削除
        await client.Repository.DeleteTagProtectionAsync(repoOwner, repoName, protection.id!.Value);
    }

    [TestMethod]
    public async Task CommitScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // リポジトリに適当な内容をpush
        using var repoDir = new TestTempRepo(repo.clone_url!);
        repoDir.Auther = "test-auther";
        repoDir.AutherMail = "test-auther@example.com";
        var commit1 = repoDir.Commit("commit1\naaaa", dir =>
        {
            dir.RelativeFile("aaa.cs").WriteAllText("using System;");
            dir.RelativeFile("bbb.cs").WriteAllText("using System;");
        });
        var commit2 = repoDir.Commit("commit2\nbbb", dir =>
        {
            dir.RelativeFile("aaa.cs").WriteAllText("using System.Text;");
            dir.RelativeFile("bbb.cs").WriteAllText("using System.Linq;");
        });
        repoDir.Push(this.TestTokenUser, this.TestToken);

        // pushしたコミットの情報を取得。少し時間が経たないと情報が得られないようなのでしばらく繰り返して取得する。
        await TestCallHelper.TrySatisfy(
            caller: breaker => client.Repository.GetCommitAsync(ownerName, repoName, repoDir.Repo.Head.Tip.Sha, cancelToken: breaker),
            condition: pushed => pushed.sha == repoDir.Repo.Head.Tip.Sha
        );
        await TestCallHelper.TrySatisfy(
            caller: breaker => client.Repository.ListCommitsAsync(ownerName, repoName, cancelToken: breaker),
            condition: commit_list => commit_list.Any(c => c.sha == repoDir.Repo.Head.Tip.Sha)
        );

        // コミット差分取得
        var commit_diff = await client.Repository.GetCommitDiffAsync(ownerName, repoName, repoDir.Repo.Head.Tip.Sha, "diff");
        commit_diff.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task CommitCompareScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var userName = $"user-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var user = await resources.CreateTestUserAsync(userName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // コミット差分取得
        var commit_compare = await client.Repository.GetCommitCompareAsync(repoOwner, repoName, $"main...other");
        commit_compare.total_commits.Should().BeGreaterThan(0);

    }

    [TestMethod]
    public async Task CommitStatusScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // コミットステータス作成
        var status = await client.Repository.CreateCommitStatusAsync(ownerName, repoName, content.commit!.sha!, new(context: "aaa"));
        status.context.Should().NotBeNullOrWhiteSpace();

        // ステータスリスト取得
        var status_list = await client.Repository.ListCommitStatusesAsync(ownerName, repoName, content.commit!.sha!);
        status_list.Should().Contain(s => s.context == "aaa");

        // ステータスリスト取得
        var statuses_list = await client.Repository.ListCommitsStatusesAsync(ownerName, repoName, content.commit!.sha!);
        statuses_list.Should().Contain(s => s.context == "aaa");

        // combinedステータス取得
        var combine_status = await client.Repository.GetCommitsCombinedStatusAsync(ownerName, repoName, content.commit!.sha!);
        combine_status.statuses.Should().Contain(s => s.context == "aaa");
    }

    [TestMethod]
    public async Task GetContentScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // リポジトリに適当な内容をpush
        var fileContent = "using System;";
        using var repoDir = new TestTempRepo(repo.clone_url!);
        repoDir.Auther = "test-auther";
        repoDir.AutherMail = "test-auther@example.com";
        repoDir.Commit("commit1\naaaa", dir =>
        {
            dir.RelativeFile("aaa.cs").WriteAllText(fileContent);
            dir.RelativeFile("bbb.cs").WriteAllText(fileContent);
        });
        repoDir.Push(this.TestTokenUser, this.TestToken);

        // コンテンツリスト取得
        var content_list = await TestCallHelper.TrySatisfy(
            caller: breaker => client.Repository.ListContentsAsync(ownerName, repoName, cancelToken: breaker),
            condition: contents => 0 < contents.Length
        );
        content_list.Select(c => c.path).Should().Contain(["aaa.cs", "bbb.cs"]);

        // コンテンツ取得
        var content = await client.Repository.GetContentAsync(ownerName, repoName, "aaa.cs");
        content.path.Should().Be("aaa.cs");

        // BLOB取得
        var blob = await client.Repository.GetBlobAsync(ownerName, repoName, content.sha!);
        blob.content.Should().Be(fileContent.EncodeUtf8().EncodeBase64());

        // オブジェクト取得
        using var downloadObject = await client.Repository.GetObjectAsync(ownerName, repoName, "aaa.cs");
        new StreamReader(downloadObject.Result.Stream).ReadToEnd().Should().Be(fileContent);

        // オブジェクト取得
        using var downloadFile = await client.Repository.GetFileAsync(ownerName, repoName, "aaa.cs");
        new StreamReader(downloadFile.Result.Stream).ReadToEnd().Should().Be(fileContent);
    }

    [TestMethod]
    public async Task CreateUpdateDeleteContentScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var targetPath = "aaa.cs";
        var newContent = "ABCDEF".EncodeUtf8Base64();
        var content_created = await client.Repository.CreateFileAsync(ownerName, repoName, targetPath, new(content: newContent));
        content_created.content!.path.Should().Be(targetPath);
        content_created.content!.content.Should().Be(newContent);

        // コンテンツ更新
        var updateContent = "HJIJKL".EncodeUtf8Base64();
        var content_updated = await client.Repository.UpdateFileAsync(ownerName, repoName, targetPath, new(content: updateContent, sha: content_created.content!.sha!));
        content_updated.content!.path.Should().Be(targetPath);
        content_updated.content!.content.Should().Be(updateContent);

        // コンテンツ削除
        var content_deleted = await client.Repository.DeleteFileAsync(ownerName, repoName, targetPath, new(content_updated.content!.sha!));
        content_deleted.content.Should().BeNull();
    }

    [TestMethod]
    public async Task UpdateMultipleContentScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var content2 = await client.Repository.CreateFileAsync(ownerName, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64()));

        // コンテンツ更新
        var updateOptions = new ChangeFilesOptions(files: [
            new(ChangeFileOperationOperation.Update, "aaa.cs",content: "GHI".EncodeUtf8Base64()),
            new(ChangeFileOperationOperation.Create, "ccc.cs",content: "JKL".EncodeUtf8Base64()),
        ]);
        var updated = await client.Repository.UpdateFilesAsync(ownerName, repoName, updateOptions);
        updated.files.Should().Contain(f => f.path == "aaa.cs").And.Contain(f => f.path == "ccc.cs");
    }

    [TestMethod]
    public async Task ListRefsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64(), new_branch: "main"));
        var content2 = await client.Repository.CreateFileAsync(ownerName, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64()));

        // refs取得
        var refs = await client.Repository.ListRefsAsync(ownerName, repoName);
        refs.Should().Contain(r => r.@ref == "refs/heads/main");

        // refs取得
        var refs_from = await client.Repository.ListRefsAsync(ownerName, repoName, "heads/main");
        refs_from.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GitHookScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var userName = $"repo-{DateTime.Now.Ticks:X16}";
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync(userName);
        var repo = await resources.CreateTestUserRepoAsync(userName, repoName);

        // githook を有効にする
        user = await client.Admin.UpdateUserAsync(userName, new(login_name: userName, source_id: 0, allow_git_hook: true));

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(userName);

        // githookリスト取得
        var githooks = await userClient.Repository.ListGitHooksAsync(userName, repoName);
        githooks.Should().NotBeEmpty();

        // githook取得
        var hookname = githooks.First().name!;
        var githook = await userClient.Repository.GetGitHookAsync(userName, repoName, hookname);
        githook.name.Should().Be(hookname);

        // githook更新
        var githook_updated = await userClient.Repository.UpdateGitHookAsync(userName, repoName, hookname, new(content: githook.content!));
        githook_updated.name.Should().Be(hookname);

        // githook削除
        await userClient.Repository.DeleteGitHookAsync(userName, repoName, hookname);
    }

    [TestMethod]
    public async Task GetTreeAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64(), new_branch: "main"));
        var content2 = await client.Repository.CreateFileAsync(ownerName, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64()));

        // タグ取得
        var tree = await client.Repository.GetTreeAsync(ownerName, repoName, content2.commit!.sha!);
        tree.sha.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task GetAnnotatedTagAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // リポジトリに適当な内容をpush
        var fileContent = "using System;";
        using var repoDir = new TestTempRepo(repo.clone_url!);
        repoDir.Auther = "test-auther";
        repoDir.AutherMail = "test-auther@example.com";
        var commit = repoDir.Commit("commit1\naaaa", dir =>
        {
            dir.RelativeFile("aaa.cs").WriteAllText(fileContent);
            dir.RelativeFile("bbb.cs").WriteAllText(fileContent);
        });
        var tag = repoDir.AnnotatedTag("anno-tag", "tekito");
        repoDir.Push(this.TestTokenUser, this.TestToken);
        repoDir.Push(tag.Reference.CanonicalName, this.TestTokenUser, this.TestToken);

        // 認識されるまで待機
        await TestCallHelper.TrySatisfy(
            caller: breaker => client.Repository.GetCommitAsync(ownerName, repoName, repoDir.Repo.Head.Tip.Sha, cancelToken: breaker),
            condition: pushed => pushed.sha == repoDir.Repo.Head.Tip.Sha
        );

        // タグ取得
        var anoTag = await client.Repository.GetAnnotatedTagAsync(ownerName, repoName, tag.Annotation.Sha);
        anoTag.tag.Should().Be("anno-tag");
    }

    [TestMethod]
    public async Task ApplyPatchAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // パッチ内容
        var patch = """
        From d2b5f3676664a86573d2784495fd88f08fe8fdf0 Mon Sep 17 00:00:00 2001
        From: forgejo-admin <forgejo-admin@example.com>
        Date: Thu, 14 Nov 2024 14:58:52 +0000
        Subject: [PATCH] Update aaa.cs

        ---
         aaa.cs | 2 +-
         1 file changed, 1 insertion(+), 1 deletion(-)

        diff --git a/aaa.cs b/aaa.cs
        index 48b83b8..59ecf1e 100644
        --- a/aaa.cs
        +++ b/aaa.cs
        @@ -1 +1 @@
        -ABC
        \ No newline at end of file
        +DEF
        \ No newline at end of file
        """;

        // パッチ適用
        var patched = await client.Repository.ApplyPatchAsync(ownerName, repoName, new(content: patch, content.commit!.sha!));
        patched.commit.Should().NotBeNull();
    }

    [TestMethod]
    public async Task ForkSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // リポジトリフォーク
        var forked = await client.Repository.ForkAsync(ownerName, repoName, new(name: $"forked-{repoName}")).WillBeDiscarded(resources);
        forked.name.Should().Be($"forked-{repoName}");

        // リポジトリフォーク
        var fork_list = await client.Repository.ListForksAsync(ownerName, repoName);
        fork_list.Should().Contain(f => f.name == $"forked-{repoName}");
    }

    [TestMethod]
    public async Task GetActionTasks()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // タスク取得
        var tasks = await client.Repository.GetActionTasks(repoOwner, repoName);
    }

    [TestMethod]
    public async Task DispatchActionWorkflowAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = """
        on: [push]
        jobs:
          test:
            runs-on: docker
            steps:
              - run: echo All Good
        """.ReplaceLineEndings("\n");
        var main1 = await client.Repository.CreateFileAsync(repoOwner, repoName, ".forgejo/workflows/demo.yaml", new(content: content.EncodeUtf8Base64()));

        // ワークフロー実行
        // 適切にセットアップしていないので現状はあまり意味がない。呼び出すだけ。
        await client.Repository.DispatchActionWorkflowAsync(repoOwner, repoName, "demo.yaml", new(@ref: "main"));
    }

    [TestMethod]
    public async Task ActionSecretSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // secret 設定
        await client.Repository.SetActionSecretAsync(this.TestTokenUser, repoName, "secret1", new(data: "AAA"));

        // secret リスト取得
        var secrets = await client.Repository.ListActionSecretsAsync(this.TestTokenUser, repoName);
        secrets.Should().Contain(s => string.Equals(s.name, "secret1", StringComparison.OrdinalIgnoreCase));

        // secret 削除
        await client.Repository.DeleteActionSecretAsync(this.TestTokenUser, repoName, "secret1");
    }

    [TestMethod]
    public async Task ActionVariableScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var varName = $"varname";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // variable作成
        await client.Repository.CreateActionVariableAsync(repoOwner, repoName, varName, new(value: "AAA"));

        // variable取得
        var variable = await client.Repository.GetActionVariableAsync(repoOwner, repoName, varName);
        variable.data.Should().Be("AAA");

        // variable更新
        await client.Repository.UpdateActionVariableAsync(repoOwner, repoName, varName, new(value: "BBB"));

        // リリースリスト取得
        var variable_list = await client.Repository.ListActionVariablesAsync(repoOwner, repoName);
        variable_list.Should().Contain(v => string.Equals(v.name, varName, StringComparison.OrdinalIgnoreCase));

        // リリース情報削除
        await client.Repository.DeleteActionVariableAsync(repoOwner, repoName, varName);

        // 追加検証
        Assert.Inconclusive("ListActionVariablesAsync で変数値が得られない。バグ？");
        variable_list.Should().Contain(v => string.Equals(v.name, varName, StringComparison.OrdinalIgnoreCase) && v.data == "BBB");
    }

    [TestMethod]
    public async Task FlagSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // フラグ追加
        await client.Repository.AddFlagAsync(ownerName, repoName, "flag1");

        // フラグ判定
        var flagged = await client.Repository.IsFlagGivenAsync(ownerName, repoName, "flag1");
        flagged.Should().BeTrue();

        // フラグリスト取得
        var flags1 = await client.Repository.ListFlagsAsync(ownerName, repoName);
        flags1.Should().Contain(["flag1"]);

        // フラグ更新
        await client.Repository.ReplaceFlagsAsync(ownerName, repoName, new(flags: ["flag2", "flag3"]));

        // フラグリスト取得
        var flags2 = await client.Repository.ListFlagsAsync(ownerName, repoName);
        flags2.Should().Contain(["flag2", "flag3"]);

        // フラグ更新
        await client.Repository.RemoveFlagAsync(ownerName, repoName, "flag3");

        // フラグリスト取得
        var flags3 = await client.Repository.ListFlagsAsync(ownerName, repoName);
        flags3.Should().Contain(["flag2"]);

        // フラグクリア
        await client.Repository.ClearFlagsAsync(ownerName, repoName);

        // フラグリスト取得
        var flags4 = await client.Repository.ListFlagsAsync(ownerName, repoName);
        flags4.Should().BeEmpty();
    }

    [TestMethod]
    public async Task WebhookSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // Webhook作成
        var create_config = new Dictionary<string, string> { ["content_type"] = "json", ["url"] = "http://localhost:9999", };
        var webhook = await client.Repository.CreateWebhookAsync(ownerName, repoName, new(create_config, CreateHookOptionType.Forgejo));
        webhook.url.Should().Be("http://localhost:9999");

        // Webhook取得
        var webhook_get = await client.Repository.GetWebhookAsync(ownerName, repoName, webhook.id!.Value);
        webhook_get.url.Should().Be("http://localhost:9999");

        // Webhook更新
        var update_config = new Dictionary<string, string> { ["content_type"] = "json", ["url"] = "http://localhost:8888", };
        var webhook_updated = await client.Repository.UpdateWebhookAsync(ownerName, repoName, webhook.id!.Value, new(config: update_config));
        webhook_updated.url.Should().Be("http://localhost:8888");

        // Webhookリスト取得
        var webhook_list = await client.Repository.ListWebhooksAsync(ownerName, repoName);
        webhook_list.Should().Contain(w => w.url == "http://localhost:8888");

        // Webhook削除
        await client.Repository.DeleteWebhookAsync(ownerName, repoName, webhook_updated.id!.Value);
    }

    [TestMethod]
    public async Task WebhookTestSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // テストWebhook受信サーバ実行
        var receiver = new TaskCompletionSource<string>();
        var servicePort = 9999;
        var servicePath = "/webhook";
        var webapp = WebApplication.Create();
        webapp.Urls.Add($"http://*:{servicePort}");
        webapp.MapPost(servicePath, async (HttpRequest request) => receiver.TrySetResult(await request.Body.ReadAllTextAsync()));

        var server = webapp.StartAsync();
        try
        {
            // Webhook作成
            var testgateway = "testhost-gateway";
            var config = new Dictionary<string, string> { ["content_type"] = "json", ["url"] = $"http://{testgateway}:{servicePort}{servicePath}", };
            var webhook = await client.Repository.CreateWebhookAsync(ownerName, repoName, new(config, CreateHookOptionType.Forgejo, active: true));

            // Webhook削除
            await client.Repository.TestWebhookAsync(ownerName, repoName, webhook.id!.Value);

            // 受信待機
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var notify = await receiver.Task.WaitAsync(timeout.Token);
            notify.Should().NotBeNullOrWhiteSpace();
        }
        finally
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await webapp.StopAsync(timeout.Token);
        }
    }

    [TestMethod]
    public async Task IssueConfigSenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // get config
        var config = await client.Repository.GetIssueConfigAsync(ownerName, repoName);

        // validate config
        var validation = await client.Repository.GetIssueConfigValidationAsync(ownerName, repoName);
    }

    [TestMethod]
    public async Task ListIssueTemplatesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(ownerName, repoName, ".github/issue_template/markdown_template.md", new(content: """
        ---
        name: 'Template Name'
        about: 'This template is for testing!'
        title: '[TEST] '
        ref: 'main'
        labels:
          - bug
          - 'help needed'
        ---

        This is the template!
        """.EncodeUtf8Base64()));

        var content2 = await client.Repository.CreateFileAsync(ownerName, repoName, ".github/issue_template/yaml_template.yml", new(content: """
        name: Bug Report
        about: File a bug report
        title: '[Bug]: '
        body:
          - type: markdown
            attributes:
              value: |
                Thanks for taking the time to fill out this bug report!
          - type: input
            id: contact
            attributes:
              label: Contact Details
              description: How can we get in touch with you if we need more info?
              placeholder: ex. email@example.com
            validations:
              required: false
          - type: textarea
            id: what-happened
            attributes:
              label: What happened?
              description: Also tell us, what did you expect to happen?
              placeholder: Tell us what you see!
              value: 'A bug happened!'
            validations:
              required: true
          - type: dropdown
            id: version
            attributes:
              label: Version
              description: What version of our software are you running?
              options:
                - 1.0.2 (Default)
                - 1.0.3 (Edge)
            validations:
              required: true
          - type: dropdown
            id: browsers
            attributes:
              label: What browsers are you seeing the problem on?
              multiple: true
              options:
                - Firefox
                - Chrome
                - Safari
                - Microsoft Edge
          - type: textarea
            id: logs
            attributes:
              label: Relevant log output
              description: Please copy and paste any relevant log output. This will be automatically formatted into code, so no need for backticks.
              render: shell
          - type: checkboxes
            id: terms
            attributes:
              label: Code of Conduct
              description: By submitting this issue, you agree to follow our [Code of Conduct](https://example.com)
              options:
                - label: I agree to follow this project's Code of Conduct
                  required: true
        """.EncodeUtf8Base64()));

        // Issue template を取得
        var templates = await client.Repository.ListIssueTemplatesAsync(ownerName, repoName);
        templates.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task CreateUpdatePullRequestScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // PR作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // PR取得
        var pr_get = await client.Repository.GetPullRequestAsync(repoOwner, repoName, pr.number!.Value);
        pr_get.title.Should().Be("pull-req");

        // PR取得 for target
        var pr_target = await client.Repository.GetPullRequestAsync(repoOwner, repoName, "main", "other");
        pr_target.title.Should().Be("pull-req");

        // PR更新
        var pr_updated = await client.Repository.UpdatePullRequestAsync(repoOwner, repoName, pr.number!.Value, new(title: "pull-req-upd"));
        pr_updated.title.Should().Be("pull-req-upd");

        // PRリスト取得
        var pr_list = await client.Repository.ListPullRequestsAsync(repoOwner, repoName);
        pr_list.Should().Contain(p => p.title == "pull-req-upd");
    }

    [TestMethod]
    public async Task PullRequestContentScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // PR作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // PR DIFF
        var pr_diff = await client.Repository.GetPullRequestDiffAsync(repoOwner, repoName, pr.number!.Value, "diff");
        pr_diff.Should().NotBeNullOrWhiteSpace();

        // PR コミット取得
        var pr_commits = await client.Repository.ListPullRequestCommitsAsync(repoOwner, repoName, pr.number!.Value);
        pr_commits.Should().NotBeEmpty();

        // PR 変更取得
        var pr_changes = await client.Repository.ListPullRequestChangesAsync(repoOwner, repoName, pr.number!.Value);
        pr_changes.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task PullRequestMergeScenario1()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // PR作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // PR取得 (状態確定待ち)
        var pr_get = await client.Repository.GetPullRequestAsync(repoOwner, repoName, pr.number!.Value);

        // PRマージ済み判定
        var pr_premerge = await client.Repository.IsPullRequestMergeAsync(repoOwner, repoName, pr.number!.Value);
        pr_premerge.Should().BeFalse();

        // PRマージ
        await client.Repository.MergePullRequestAsync(repoOwner, repoName, pr.number!.Value, new(Do: MergePullRequestOptionDo.Merge));

        // PRマージ済み判定。認識されるまで待機
        await TestCallHelper.TrySatisfy(
            caller: breaker => client.Repository.IsPullRequestMergeAsync(repoOwner, repoName, pr.number!.Value, cancelToken: breaker).AsTask(),
            condition: merged => merged
        );

        // マージされたブランチの情報取得
        var merged_branch = await client.Repository.GetBranchAsync(repoOwner, repoName, branch: "main");

        // コミットPR取得
        var pr_commit = await client.Repository.GetCommitPullRequestAsync(repoOwner, repoName, merged_branch.commit!.id!);
        pr_commit.title.Should().Be("pull-req");

    }

    [TestMethod]
    public async Task PullRequestMergeScenario2()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // コンテンツ作成
        var main1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var other1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var other2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));
        var main2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ddd.cs", new(content: "JKL".EncodeUtf8Base64()));

        // PR作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // PR取得 (状態確定待ち)
        var pr_get = await client.Repository.GetPullRequestAsync(repoOwner, repoName, pr.number!.Value);

        // PRマージ済み判定
        var pr_merged = await client.Repository.IsPullRequestMergeAsync(repoOwner, repoName, pr.number!.Value);
        pr_merged.Should().BeFalse();

        // PR逆マージ
        await client.Repository.UpdateMergePullRequestAsync(repoOwner, repoName, pr.number!.Value, "merge");
    }

    [TestMethod]
    public async Task CancelPullRequestAutoMergeAsync()
    {
        Assert.Inconclusive("自動マージがどう働くのかが不明で、そのキャンセルもどう使うのかがわからない");

        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // PR作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // PR取得 (状態確定待ち)
        var pr_get = await client.Repository.GetPullRequestAsync(repoOwner, repoName, pr.number!.Value);

        // PR自動マージキャンセル
        await client.Repository.CancelPullRequestAutoMergeAsync(repoOwner, repoName, pr.number!.Value);
    }

    [TestMethod]
    public async Task PullRequestReviewerScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var userName = $"user-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var user = await resources.CreateTestUserAsync(userName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // PR作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // PR取得 (状態確定待ち)
        var pr_get = await client.Repository.GetPullRequestAsync(repoOwner, repoName, pr.number!.Value);

        // レビューリクエストを作成
        var reviews = await client.Repository.CreatePullRequestReviewRequestsAsync(repoOwner, repoName, pr.number!.Value, new(reviewers: [user.login!]));
        reviews.Should().NotBeEmpty();

        // レビューユーザリスト取得
        var reviewers = await client.Repository.ListReviewRequestedUsersAsync(repoOwner, repoName);
        reviewers.Should().NotBeEmpty();

        // レビューリクエストキャンセル
        await client.Repository.CancelPullRequestReviewRequestsAsync(repoOwner, repoName, pr.number!.Value, new(reviewers: [user.login!]));
    }

    [TestMethod]
    public async Task PullRequestReviewScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var userName = $"user-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var user = await resources.CreateTestUserAsync(userName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // PR作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // PR取得 (状態確定待ち)
        var pr_get = await client.Repository.GetPullRequestAsync(repoOwner, repoName, pr.number!.Value);

        // レビュー作成
        var review = await client.Repository.CreatePullRequestReviewAsync(repoOwner, repoName, pr.number!.Value, new(body: "AAA"));
        review.body.Should().Be("AAA");

        // レビュー取得
        var review_get = await client.Repository.GetPullRequestReviewAsync(repoOwner, repoName, pr.number!.Value, review.id!.Value);
        review_get.body.Should().Be("AAA");

        // レビューリスト取得
        var review_list = await client.Repository.ListPullRequestReviewsAsync(repoOwner, repoName, pr.number!.Value);
        review_list.Should().Contain(r => r.body == "AAA");

        // レビュー削除
        await client.Repository.DeletePullRequestReviewAsync(repoOwner, repoName, pr.number!.Value, review.id!.Value);
    }

    [TestMethod]
    public async Task PullRequestReviewActionScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var userName = $"user-{DateTime.Now.Ticks:X16}";
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync(userName);
        var repo = await resources.CreateTestUserRepoAsync(userName, repoName);

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(userName);

        // コンテンツ作成
        var content1 = await userClient.Repository.CreateFileAsync(userName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await userClient.Repository.CreateBranchAsync(userName, repoName, new("other"));
        var content2 = await userClient.Repository.CreateFileAsync(userName, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await userClient.Repository.CreateFileAsync(userName, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // PR作成
        var pr = await userClient.Repository.CreatePullRequestAsync(userName, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // レビューコメント作成
        var review_options = new CreatePullReviewOptions(
            body: "BODY",
            commit_id: content3.commit!.sha,
            comments: [
                new(path: "ccc.cs", new_position: 1, body: "REVIEW"),
            ]
        );
        var review = await client.Repository.CreatePullRequestReviewAsync(userName, repoName, pr.number!.Value, review_options);

        // レビューの承認
        var submitted = await client.Repository.SubmitPullRequestPendingReviewAsync(userName, repoName, pr.number!.Value, review.id!.Value, new(@event: "APPROVED"));
        submitted.Should().NotBeNull();

        // レビューの却下
        var dismissed = await client.Repository.DismissPullRequestReviewAsync(userName, repoName, pr.number!.Value, review.id!.Value, new(message: "dismiss"));
        dismissed.Should().NotBeNull();

        // レビューの却下取り消し
        var undismissed = await client.Repository.UndismissPullRequestReviewAsync(userName, repoName, pr.number!.Value, review.id!.Value);
        undismissed.Should().NotBeNull();
    }

    [TestMethod]
    public async Task PullRequestReviewCommentScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var userName = $"user-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var user = await resources.CreateTestUserAsync(userName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "other"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: branch.name));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "ccc.cs", new(content: "GHI".EncodeUtf8Base64(), branch: branch.name));

        // PR作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "other"));
        pr.title.Should().Be("pull-req");

        // PR取得 (状態確定待ち)
        var pr_get = await client.Repository.GetPullRequestAsync(repoOwner, repoName, pr.number!.Value);

        // レビュー作成
        var review = await client.Repository.CreatePullRequestReviewAsync(repoOwner, repoName, pr.number!.Value, new(body: "AAA"));

        // レビューコメント追加
        var comment = await client.Repository.AddPullRequestReviewCommentAsync(repoOwner, repoName, pr.number!.Value, review.id!.Value, new(body: "XXXX"));
        comment.body.Should().Be("XXXX");

        // レビューコメント追加
        var comment_get = await client.Repository.GetPullRequestReviewCommentAsync(repoOwner, repoName, pr.number!.Value, review.id!.Value, comment.id!.Value);
        comment_get.body.Should().Be("XXXX");

        // レビューコメントリスト取得
        var comment_list = await client.Repository.ListPullRequestReviewCommentsAsync(repoOwner, repoName, pr.number!.Value, review.id!.Value);
        comment_list.Should().Contain(r => r.body == "XXXX");

        // レビューコメント削除
        await client.Repository.DeletePullRequestReviewCommentAsync(repoOwner, repoName, pr.number!.Value, pr.number!.Value, comment.id!.Value);
    }

    [TestMethod]
    public async Task GetPinsAllowdAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // Issue template を取得
        var pins_allowed = await client.Repository.GetPinsAllowdAsync(ownerName, repoName);
        pins_allowed.Should().NotBeNull();
    }

    [TestMethod]
    public async Task ListPinnedIssuesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var issue = await resources.CreateTestIssueAsync(repoOwner, repoName, issueTitle);

        // Issue を Pin
        await client.Issue.PinAsync(repoOwner, repoName, issue.number!.Value);

        // Pined Issue リスト取得
        var pined_issues = await client.Repository.ListPinnedIssuesAsync(repoOwner, repoName);
        pined_issues.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task ListPinnedPullRequestsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";
        var prTitle = $"pr-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var branch1 = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "branch1"));
        var branch2 = await client.Repository.CreateBranchAsync(repoOwner, repoName, new(new_branch_name: "branch2"));
        var content2 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "DEF".EncodeUtf8Base64(), branch: "branch1"));
        var content3 = await client.Repository.CreateFileAsync(repoOwner, repoName, "bbb.cs", new(content: "GHI".EncodeUtf8Base64(), branch: "branch2"));

        // PullRequestを作成
        var pr = await client.Repository.CreatePullRequestAsync(repoOwner, repoName, new(title: "pull-req", @base: "main", head: "branch1"));

        // Pined PR リスト取得
        // APIでPRにPinする方法がが無い？？取得値の検証ができない。
        await client.Repository.ListPinnedPullRequestsAsync(repoOwner, repoName);
    }

    [TestMethod]
    public async Task ReleaseScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var releaseName = $"release-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // リリース作成
        var release = await client.Repository.CreateReleaseAsync(ownerName, repoName, new(tag_name: releaseName));
        release.tag_name.Should().Be(releaseName);

        // リリース情報取得 ID
        var release_id = await client.Repository.GetReleaseAsync(ownerName, repoName, release.id!.Value);
        release_id.tag_name.Should().Be(releaseName);

        // リリース情報取得 タグ
        var release_tag = await client.Repository.GetReleaseTagAsync(ownerName, repoName, releaseName);
        release_tag.tag_name.Should().Be(releaseName);

        // リリース情報取得 最新
        var release_latest = await client.Repository.GetReleaseLatestAsync(ownerName, repoName);
        release_tag.tag_name.Should().Be(releaseName);

        // リリースリスト取得
        var release_list = await client.Repository.ListReleasesAsync(ownerName, repoName);
        release_list.Should().Contain(r => r.tag_name == releaseName);

        // リリース情報更新
        var release_updated = await client.Repository.UpdateReleaseAsync(ownerName, repoName, release.id!.Value, new(body: "updated"));
        release_updated.tag_name.Should().Be(releaseName);

        // リリース情報削除
        await client.Repository.DeleteReleaseAsync(ownerName, repoName, release.id!.Value);
    }

    [TestMethod]
    public async Task ReleaseTagScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var releaseName = $"release-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // リリース作成
        var release = await client.Repository.CreateReleaseAsync(ownerName, repoName, new(tag_name: releaseName));
        release.tag_name.Should().Be(releaseName);

        // リリース情報取得 タグ
        var release_tag = await client.Repository.GetReleaseTagAsync(ownerName, repoName, releaseName);
        release_tag.tag_name.Should().Be(releaseName);

        // リリース情報更新
        await client.Repository.DeleteReleaseTagAsync(ownerName, repoName, releaseName);
    }

    [TestMethod]
    public async Task ReleaseAttachmentScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var releaseName = $"release-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // リリース作成
        var release = await client.Repository.CreateReleaseAsync(ownerName, repoName, new(tag_name: releaseName));

        // リリース添付ファイル作成
        var attach_file = TestPathHelper.GetProjectDir().RelativeFile("assets/packages/Dummy.0.0.0.nupkg");
        using var attach_content = new MemoryStream(await attach_file.ReadAllBytesAsync());
        var attach = await client.Repository.CreateReleaseAttachmentAsync(ownerName, repoName, release.id!.Value, attach_content, attach_file.Name);
        attach.name.Should().Be(attach_file.Name);

        // リリース添付ファイル情報取得
        var attach_get = await client.Repository.GetReleaseAttachmentAsync(ownerName, repoName, release.id!.Value, attach.id!.Value);
        attach_get.name.Should().Be("Dummy.0.0.0.nupkg");

        // リリース添付ファイル更新
        var attach_updated = await client.Repository.UpdateReleaseAttachmentAsync(ownerName, repoName, release.id!.Value, attach.id!.Value, new(name: "abc.bin"));
        attach_updated.name.Should().Be("abc.bin");

        // リリース添付ファイルリスト取得
        var attach_list = await client.Repository.ListReleaseAttachmentsAsync(ownerName, repoName, release.id!.Value);
        attach_list.Should().Contain(a => a.name == "abc.bin");

        // リリース添付ファイル削除
        await client.Repository.DeleteReleaseAttachmentAsync(ownerName, repoName, release.id!.Value, attach.id!.Value);
    }

    [TestMethod]
    public async Task ReleaseAttachmentExtensionScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var releaseName = $"release-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // リリース作成
        var release = await client.Repository.CreateReleaseAsync(ownerName, repoName, new(tag_name: releaseName));

        // ファイル情報で添付
        var attach_file = TestPathHelper.GetProjectDir().RelativeFile("assets/packages/Dummy.0.0.0.nupkg");
        var attach_by_file = await client.Repository.CreateReleaseFileAttachmentAsync(ownerName, repoName, release.id!.Value, attach_file);
        attach_by_file.name.Should().Be("Dummy.0.0.0.nupkg");

        // バイト列で添付
        var attach_bin = await attach_file.ReadAllBytesAsync();
        var attach_by_bin = await client.Repository.CreateReleaseFileAttachmentAsync(ownerName, repoName, release.id!.Value, attach_bin, name: "test.bin");
        attach_by_bin.name.Should().Be("test.bin");
    }

    [TestMethod]
    public async Task PublicKeyScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // SSHキー作成
        var key = await client.Repository.AddDeployKeyAsync(repoOwner, repoName, new(key: TestConstants.TestSshPubKey, title: "test-pubkey"));
        key.title.Should().Be("test-pubkey");

        // SSHキー取得
        var ket_get = await client.Repository.GetDeployKeyAsync(repoOwner, repoName, key.id!.Value);
        ket_get.title.Should().Be("test-pubkey");

        // SSHキーリスト取得
        var key_list = await client.Repository.ListDeployKeysAsync(repoOwner, repoName);
        key_list.Should().Contain(k => k.title == "test-pubkey");

        // SSHキー削除
        await client.Repository.DeleteDeployKeyAsync(repoOwner, repoName, key.id!.Value);
    }

    [TestMethod]
    public async Task GetSigningKeyGpgAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // GPG署名キー取得
        var key = await client.Repository.GetSigningKeyGpgAsync(repoOwner, repoName);
        key.Should().NotBeNull();
    }

    [TestMethod]
    public async Task PushMirrorScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName1 = $"repo1-{DateTime.Now.Ticks:X16}";
        var repoName2 = $"repo2-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo1 = await resources.CreateTestRepoAsync(repoName1);
        var repo2 = await resources.CreateTestRepoAsync(repoName2);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(repoOwner, repoName1, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // pushミラーを追加
        var mirror_addr = $"http://localhost:3000/{repoOwner}/{repoName2}";
        var mirror_options = new CreatePushMirrorOption(
            interval: "8h",
            remote_address: mirror_addr,
            remote_username: this.TestAdminUser.Username,
            remote_password: this.TestAdminUser.Password
        );
        var mirror = await client.Repository.AddPushMirrorAsync(repoOwner, repoName1, mirror_options);
        mirror.remote_address.Should().NotBeNull();

        // pushミラーリストを取得
        var mirror_list = await client.Repository.ListAllPushMirrorsAsync(repoOwner, repoName1);
        var mirror_info = mirror_list.FirstOrDefault(m => m.remote_address == mirror_addr);
        Assert.IsNotNull(mirror_info);

        // pushミラーを取得
        var mirror_get = await client.Repository.GetPushMirrorAsync(repoOwner, repoName1, mirror_info.remote_name!);
        mirror_get.remote_address.Should().Be(mirror_addr);

        // pushミラー削除
        await client.Repository.DeletePushMirrorAsync(repoOwner, repoName1, mirror_info.remote_name!);
    }

    [TestMethod]
    public async Task PushMirrorSyncScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName1 = $"repo1-{DateTime.Now.Ticks:X16}";
        var repoName2 = $"repo2-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo1 = await resources.CreateTestRepoAsync(repoName1);
        var repo2 = await resources.CreateTestRepoAsync(repoName2);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(repoOwner, repoName1, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // pushミラーを追加
        var mirror_addr = $"http://localhost:3000/{repoOwner}/{repoName2}";
        var mirror_options = new CreatePushMirrorOption(
            interval: "8h",
            remote_address: mirror_addr,
            remote_username: this.TestAdminUser.Username,
            remote_password: this.TestAdminUser.Password
        );
        var mirror = await client.Repository.AddPushMirrorAsync(repoOwner, repoName1, mirror_options);
        mirror.remote_address.Should().NotBeNull();

        // pushミラーへの同期
        await client.Repository.SyncPushMirrorsAsync(repoOwner, repoName1);
    }

    [TestMethod]
    public async Task MirrorSyncScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName1 = $"repo1-{DateTime.Now.Ticks:X16}";
        var repoName2 = $"repo2-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo1 = await resources.CreateTestRepoAsync(repoName1);
        var repo2 = await resources.CreateTestRepoAsync(repoName2);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(repoOwner, repoName1, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // pushミラーを追加
        var mirror_addr = $"http://localhost:3000/{repoOwner}/{repoName2}";
        var mirror_options = new CreatePushMirrorOption(
            interval: "8h",
            remote_address: mirror_addr,
            remote_username: this.TestAdminUser.Username,
            remote_password: this.TestAdminUser.Password
        );
        var mirror = await client.Repository.AddPushMirrorAsync(repoOwner, repoName1, mirror_options);
        mirror.remote_address.Should().NotBeNull();

        // pushミラーへの同期
        await client.Repository.SyncPushMirrorsAsync(repoOwner, repoName1);
    }

    [TestMethod]
    public async Task CollaboratorScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var userName = $"user-{DateTime.Now.Ticks:X16}";
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user = await resources.CreateTestUserAsync(userName);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // 共同作業者追加
        await client.Repository.AddCollaboratorAsync(ownerName, repoName, userName, new(permission: default));

        // 共同作業者判定
        var collaborated = await client.Repository.IsCollaboratorAsync(ownerName, repoName, userName);

        // 共同作業者パーミッション取得
        var permission = await client.Repository.GetCollaboratorPermissionAsync(ownerName, repoName, userName);

        // 共同作業者リスト取得
        var collabo_list = await client.Repository.ListCollaboratorsAsync(ownerName, repoName);

        // 共同作業者削除
        await client.Repository.DeleteCollaboratorAsync(ownerName, repoName, userName);

        // 検証
        collaborated.Should().BeTrue();
        permission.permission.Should().BeEquivalentTo("write");
        collabo_list.Select(c => c.login).Should().Contain(userName);
    }

    [TestMethod]
    public async Task WatchScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // ウォッチ
        var watch = await client.Repository.WatchAsync(repoOwner, repoName);

        // ウォッチ情報取得
        var watch_get = await client.Repository.GetSubscriptionAsync(repoOwner, repoName);

        // ウォッチリスト取得
        var watch_list = await client.Repository.ListSubscribersAsync(repoOwner, repoName);

        // ウォッチ解除
        await client.Repository.UnwatchAsync(repoOwner, repoName);
    }

    [TestMethod]
    public async Task TeamScenario()
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

        // チームにリポジトリアクセスを追加
        await client.Repository.AddTeamAsync(orgName, repoName, teamName);

        // リポジトリのチーム割当を取得
        var team_get = await client.Repository.GetTeamAssignedAsync(orgName, repoName, teamName);

        // リポジトリが割当られているチームリストを取得
        var team_list = await client.Repository.ListTeamsAsync(orgName, repoName);

        // チームへのリポジトリアクセスを解除
        await client.Repository.DeleteTeamAsync(orgName, repoName, teamName);
    }

    [TestMethod]
    public async Task TopicScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        var comparer = StringComparer.OrdinalIgnoreCase;

        // トピック追加
        await client.Repository.AddTopicAsync(repoOwner, repoName, "AAA");

        // トピック取得
        var topic1 = await client.Repository.GetTopicsAsync(repoOwner, repoName);
        topic1.topics.Should().Contain(t => comparer.Equals(t, "AAA"));

        // トピック置換
        await client.Repository.ReplaceTopicsAsync(repoOwner, repoName, new(topics: ["BBB", "CCC"]));

        // トピック取得
        var topic2 = await client.Repository.GetTopicsAsync(repoOwner, repoName);
        topic2.topics.Should().NotContain(t => comparer.Equals(t, "AAA")).And.Contain(t => comparer.Equals(t, "BBB")).And.Contain(t => comparer.Equals(t, "CCC"));

        // トピック検索
        var results = await client.Repository.SearchTopicsAsync("B");
        results.topics.Should().Contain(t => comparer.Equals(t.topic_name, "BBB"));

        // トピック削除
        await client.Repository.DeleteTopicAsync(repoOwner, repoName, "BBB");
    }

    [TestMethod]
    public async Task RepoTransferScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var userName1 = $"user1-{DateTime.Now.Ticks:X16}";
        var userName2 = $"user2-{DateTime.Now.Ticks:X16}";
        var repoName1 = $"repo1-{DateTime.Now.Ticks:X16}";
        var repoName2 = $"repo2-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var user1 = await resources.CreateTestUserAsync(userName1);
        var user2 = await resources.CreateTestUserAsync(userName2);

        // 各ユーザクライアントを作成
        var user1Client = client.Sudo(userName1);
        var user2Client = client.Sudo(userName2);

        // ユーザ1でリポジトリ作成
        var repo1 = await user1Client.Repository.CreateAsync(new(name: repoName1));
        var repo2 = await user1Client.Repository.CreateAsync(new(name: repoName2));

        // 適当なコンテンツを追加
        var content1 = await user1Client.Repository.CreateFileAsync(userName1, repoName1, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var content2 = await user1Client.Repository.CreateFileAsync(userName1, repoName2, "bbb.cs", new(content: "ABC".EncodeUtf8Base64()));

        // リポジトリ転移1
        var transferRepo1 = await user1Client.Repository.TransferOwnerAsync(userName1, repoName1, new(new_owner: userName2));

        // リポジトリ転移を受け付け
        var acceptRepo = await user2Client.Repository.AcceptTransferAsync(userName1, repoName1);

        // 後始末
        await user2Client.Repository.DeleteAsync(userName2, repoName1);

        // リポジトリ転移2
        var transferRepo2 = await user1Client.Repository.TransferOwnerAsync(userName1, repoName2, new(new_owner: userName2));

        // リポジトリ転移を拒否
        var rejectedRepo = await user2Client.Repository.RejectTransferAsync(userName1, repoName2);

        // 後始末
        await user1Client.Repository.DeleteAsync(userName1, repoName2);

        // 検証
        acceptRepo.owner!.login.Should().Be(userName2);
        rejectedRepo.owner!.login.Should().Be(userName1);
    }

    [TestMethod]
    public async Task WikiScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // コンテンツ作成
        var content = await client.Repository.CreateFileAsync(repoOwner, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));

        // Wikiページ追加
        var wiki = await client.Repository.CreateWikiPageAsync(repoOwner, repoName, new(title: "AAA", content_base64: "BBB".EncodeUtf8Base64(), message: "CCC"));
        wiki.title.Should().Be("AAA");

        // Wikiページ情報取得
        var wiki_get = await client.Repository.GetWikiPageAsync(repoOwner, repoName, "AAA");
        wiki_get.title.Should().Be("AAA");

        // Wikiページ更新
        var wiki_updated = await client.Repository.UpdateWikiPageAsync(repoOwner, repoName, "AAA", new(title: "DDD"));
        wiki_updated.title.Should().Be("DDD");

        // Wikiページリスト取得
        var wiki_list = await client.Repository.ListWikiPagesAsync(repoOwner, repoName);
        wiki_list.Should().Contain(w => w.title == "DDD");

        // Wikiページ更新履歴取得
        var wiki_revs = await client.Repository.GetWikiPageRevisionsAsync(repoOwner, repoName, "DDD");
        wiki_revs.count.Should().BeGreaterThanOrEqualTo(1);

        // Wikiページ削除
        await client.Repository.DeleteWikiPageAsync(repoOwner, repoName, "DDD");
    }

    [TestMethod]
    public async Task ListActivitiesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // テスト対象呼び出し
        var activities = await client.Repository.ListActivitiesAsync(this.TestTokenUser, repoName);
        activities.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetArchiveAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // リポジトリに適当な内容をpush
        using var repoDir = new TestTempRepo(repo.clone_url!);
        repoDir.Auther = "test-auther";
        repoDir.AutherMail = "test-auther@example.com";
        repoDir.Commit("commit1", dir =>
        {
            dir.RelativeFile("aaa.cs").WriteAllText("using System;");
            dir.RelativeFile("bbb.cs").WriteAllText("using System;");
        });
        repoDir.Push(this.TestTokenUser, this.TestToken);

        // テスト対象呼び出し
        using var tempDir = new TestTempDir();
        var downloadFile = tempDir.Info.RelativeFile("download.zip");
        using (var archive = await client.Repository.GetArchiveAsync(this.TestTokenUser, repoName, "main.zip"))
        using (var downloadStream = downloadFile.CreateWrite())
        {
            await archive.Result.Stream.CopyToAsync(downloadStream);
        }
        downloadFile.Refresh();
        downloadFile.Length.Should().BeGreaterThan(0);
    }

    [TestMethod]
    public async Task ListAssigneesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // テスト対象呼び出し
        var activities = await client.Repository.ListAssigneesAsync(this.TestTokenUser, repoName);
        activities.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task UpdateDeleteAvatarScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        using var http = new HttpClient();

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // 適当な画像を作ってアバターに設定
        var image = TestResourceGenerator.CreateTextImage("Test User Avator");
        var imageB64 = Convert.ToBase64String(image);
        await client.Repository.UpdateAvatarAsync(ownerName, repoName, new(image: imageB64));

        // アバター画像を検証
        var repo_updated = await client.Repository.GetAsync(ownerName, repoName);
        var avatar1 = await http.GetByteArrayAsync(repo_updated.avatar_url);
        image.Should().Equal(avatar1);

        // アバターを削除
        await client.Repository.DeleteAvatarAsync(ownerName, repoName);

        // アバター画像を検証
        var repo_deleted = await client.Repository.GetAsync(ownerName, repoName);
        repo_deleted.avatar_url.Should().BeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task GetCodeLanguagesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // リポジトリに適当な内容をpush
        using var repoDir = new TestTempRepo(repo.clone_url!);
        repoDir.Auther = "test-auther";
        repoDir.AutherMail = "test-auther@example.com";
        repoDir.Commit("commit1", dir =>
        {
            dir.RelativeFile("aaa.cs").WriteAllText("using System;");
            dir.RelativeFile("bbb.cs").WriteAllText("using System;");
        });
        repoDir.Push(this.TestTokenUser, this.TestToken);

        // 言語情報取得。少し時間が経たないと情報が得られないようなのでしばらく繰り返して取得する。
        var languages = await TestCallHelper.Satisfy(
            caller: breaker => client.Repository.GetCodeLanguagesAsync(this.TestTokenUser, repoName, cancelToken: breaker),
            condition: languages => 0 < languages.Count
        );
    }

    [TestMethod]
    public async Task GetActionRunnerRegistrationTokenAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // テスト対象呼び出し
        var result = await client.Repository.GetActionRunnerRegistrationTokenAsync(ownerName, repoName);
        result.token.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task ListStargazersAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // リポジトリにスターを付ける
        await client.User.StarRepositoryAsync(ownerName, repoName);

        // テスト対象呼び出し
        var stargazers = await client.Repository.ListStargazersAsync(ownerName, repoName);
        stargazers.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task ListTrackedTimesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);
        var issue = await resources.CreateTestIssueAsync(ownerName, repoName, $"issue-{DateTime.Now.Ticks:X16}");

        // イシューでストップウォッチを開始
        await client.Issue.StartStopwatchAsync(repo.owner!.login!, repo.name!, issue.number!.Value);

        // イシューでストップウォッチを停止
        await client.Issue.StopStopwatchAsync(repo.owner!.login!, repo.name!, issue.number!.Value);

        // テスト対象呼び出し
        var times = await client.Repository.ListTrackedTimesAsync(ownerName, repoName);
        times.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetEditorConfigDefinitionAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var ownerName = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestRepoAsync(repoName);

        // コンテンツ作成
        var content1 = await client.Repository.CreateFileAsync(ownerName, repoName, "aaa.cs", new(content: "ABC".EncodeUtf8Base64()));
        var editorconfig = """
        root = false
        [*.xml]
        indent_style = space
        indent_size = 2
        [*.cs]
        indent_style = space
        indent_size = 4
        """;
        var content2 = await client.Repository.CreateFileAsync(ownerName, repoName, ".editorconfig", new(content: editorconfig.EncodeUtf8Base64()));

        // 結果検証
        var definition = await client.Repository.GetEditorConfigDefinitionAsync(ownerName, repoName, "aaa.cs");
        definition.Should().NotBeEmpty();
    }

}
