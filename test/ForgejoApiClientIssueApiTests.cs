using System.Text;
using ForgejoApiClient.Api.Extensions;

namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientIssueApiTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task IssueScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // Issue作成
        var issue_created = await client.Issue.CreateAsync(repoOwner, repoName, new(title: issueTitle));

        // Issue取得
        var issue_get = await client.Issue.GetAsync(repoOwner, repoName, issue_created.number!.Value);

        // Issue検索
        var issue_created_search = await client.Issue.SearchAsync();

        // Issueリスト取得
        var issue_created_list = await client.Issue.ListAsync(repoOwner, repoName);

        // Issue更新
        var issue_update = await client.Issue.UpdateAsync(repoOwner, repoName, issue_created.number!.Value, new(title: $"new-{issue_created.title}"));

        // Issueリスト取得
        var issue_update_list = await client.Issue.ListAsync(repoOwner, repoName);

        // Issue削除
        await client.Issue.DeleteAsync(repoOwner, repoName, issue_created.number!.Value);

        // Issueリスト取得
        var issue_deleted_list = await client.Issue.ListAsync(repoOwner, repoName);

        // 検証
        issue_created.title.Should().Be(issueTitle);
        issue_get.title.Should().Be(issueTitle);
        issue_created_search.Should().Contain(i => i.title == issueTitle);
        issue_created_list.Should().Contain(i => i.title == issueTitle);
        issue_update.title.Should().Be($"new-{issue_created.title}");
        issue_update_list.Should().Contain(i => i.title == $"new-{issue_created.title}");
        issue_deleted_list.Should().NotContain(i => i.title == $"new-{issue_created.title}");
    }

    [TestMethod]
    public async Task IssueAttachmentScenario()
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

        // Attachment作成
        using var attachContent = new MemoryStream(Encoding.UTF8.GetBytes("file-content"));
        var attach_time = DateTimeOffset.Now;
        var attach_created = await client.Issue.CreateAttachmentAsync(repoOwner, repoName, issue.number!.Value, attachContent, "first.txt", attach_time);
        attach_created.name.Should().Be("first.txt");

        // Attachment取得
        var attach_get = await client.Issue.GetAttachmentAsync(repoOwner, repoName, issue.number!.Value, attach_created.id!.Value);
        attach_get.name.Should().Be("first.txt");

        // Attachmentリスト取得
        var attach_list = await client.Issue.ListAttachmentsAsync(repoOwner, repoName, issue.number!.Value);
        attach_list.Should().Contain(i => i.name == "first.txt");

        // Attachment更新
        var attach_updated = await client.Issue.UpdateAttachmentAsync(repoOwner, repoName, issue.number!.Value, attach_created.id!.Value, new(name: "second.txt"));
        attach_updated.name.Should().Be("second.txt");

        // Attachmentリスト取得
        var attach_updated_list = await client.Issue.ListAttachmentsAsync(repoOwner, repoName, issue.number!.Value);
        attach_updated_list.Should().Contain(i => i.name == "second.txt");

        // Attachment削除
        await client.Issue.DeleteAttachmentAsync(repoOwner, repoName, issue.number!.Value, attach_created.id!.Value);

        // Attachmentリスト取得
        var attach_deleted_list = await client.Issue.ListAttachmentsAsync(repoOwner, repoName, issue.number!.Value);
        attach_deleted_list.Should().NotContain(i => i.name == "second.txt");
    }

    [TestMethod]
    public async Task IssueAttachmentExtensionScenario()
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

        // ファイル情報で添付
        var attach_file = TestPathHelper.GetProjectDir().RelativeFile($"assets/texts/DummyText.txt");
        var attach_by_file = await client.Issue.CreateFileAttachmentAsync(repoOwner, repoName, issue.number!.Value, attach_file);
        attach_by_file.name.Should().Be(attach_file.Name);

        // バイト列で添付
        var attach_bin = await attach_file.ReadAllBytesAsync();
        var attach_by_bin = await client.Issue.CreateFileAttachmentAsync(repoOwner, repoName, issue.number!.Value, attach_bin, "test.txt");
        attach_by_bin.name.Should().Be("test.txt");
    }

    [TestMethod]
    public async Task IssueReactionScenario()
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

        // Reaction作成
        var reaction_1 = await client.Issue.AddReactionAsync(repoOwner, repoName, issue.number!.Value, new(content: "+1"));

        // Reaction作成
        var reaction_2 = await client.Issue.AddReactionAsync(repoOwner, repoName, issue.number!.Value, new(content: "heart"));

        // Reactionリスト取得
        var reaction_list = await client.Issue.ListReactionsAsync(repoOwner, repoName, issue.number!.Value);

        // Reaction削除
        await client.Issue.RemoveReactionAsync(repoOwner, repoName, issue.number!.Value, new(content: "heart"));

        // Reactionリスト取得
        var reaction_deleted_list = await client.Issue.ListReactionsAsync(repoOwner, repoName, issue.number!.Value);

        // 検証
        reaction_1.content.Should().Be("+1");
        reaction_2.content.Should().Be("heart");
        reaction_list.Select(r => r.content).Should().Contain("+1", "heart");
        reaction_deleted_list.Select(r => r.content).Should().Contain("+1").And.NotContain("heart");
    }

    [TestMethod]
    public async Task IssueCommentScenario()
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

        // コメント作成
        var comment_created = await client.Issue.CreateCommentAsync(repoOwner, repoName, issue.number!.Value, new(body: "comment"));

        // コメント取得
        var comment_created_get = await client.Issue.GetCommentAsync(repoOwner, repoName, comment_created.id!.Value);

        // コメントリスト取得 (issue)
        var comment_created_list = await client.Issue.ListIssueCommentsAsync(repoOwner, repoName, issue.number!.Value);

        // コメントリスト取得 (repo)
        var comment_created_list_repo = await client.Issue.ListRepositoryCommentsAsync(repoOwner, repoName);

        // コメント更新
        var comment_updated = await client.Issue.UpdateCommentAsync(repoOwner, repoName, comment_created.id!.Value, new(body: "comment-updated"));

        // コメントリスト取得 
        var comment_updated_list = await client.Issue.ListIssueCommentsAsync(repoOwner, repoName, issue.number!.Value);

        // コメント削除
        await client.Issue.DeleteCommentAsync(repoOwner, repoName, comment_created.id!.Value);

        // コメントリスト取得 
        var comment_deleted_list = await client.Issue.ListIssueCommentsAsync(repoOwner, repoName, issue.number!.Value);

        // 検証
        comment_created.body.Should().Be("comment");
        comment_created_get.Should().NotBeNull();
        comment_created_get.body.Should().Be("comment");
        comment_created_list.Should().Contain(i => i.body == "comment");
        comment_created_list_repo.Should().Contain(i => i.body == "comment");

        comment_updated.Should().NotBeNull();
        comment_updated.body.Should().Be("comment-updated");
        comment_updated_list.Should().Contain(i => i.body == "comment-updated");

        comment_deleted_list.Should().NotContain(i => i.body == "comment-updated");
    }

    [TestMethod]
    public async Task IssueCommentAttachmentScenario()
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

        // コメント作成
        var comment = await client.Issue.CreateCommentAsync(repoOwner, repoName, issue.number!.Value, new(body: "comment"));

        // Attachment作成
        using var content = new MemoryStream(Encoding.UTF8.GetBytes("file-content"), writable: false);
        var attach_time = DateTimeOffset.Now;
        var attach_created = await client.Issue.CreateCommentAttachmentAsync(repoOwner, repoName, comment.id!.Value, content, "first.txt", attach_time);

        // Attachment取得
        var attach_get = await client.Issue.GetCommentAttachmentAsync(repoOwner, repoName, comment.id!.Value, attach_created.id!.Value);

        // Attachmentリスト取得
        var attach_list = await client.Issue.ListCommentAttachmentsAsync(repoOwner, repoName, comment.id!.Value);

        // Attachment更新
        var attach_updated = await client.Issue.UpdateCommentAttachmentAsync(repoOwner, repoName, comment.id!.Value, attach_created.id!.Value, new(name: "second.txt"));

        // Attachmentリスト取得
        var attach_updated_list = await client.Issue.ListCommentAttachmentsAsync(repoOwner, repoName, comment.id!.Value);

        // Attachment削除
        await client.Issue.DeleteCommentAttachmentAsync(repoOwner, repoName, comment.id!.Value, attach_created.id!.Value);

        // Attachmentリスト取得
        var attach_deleted_list = await client.Issue.ListCommentAttachmentsAsync(repoOwner, repoName, comment.id!.Value);

        // 検証
        attach_created.name.Should().Be("first.txt");
        attach_get.name.Should().Be("first.txt");
        attach_list.Should().Contain(i => i.name == "first.txt");
        attach_updated.name.Should().Be("second.txt");
        attach_updated_list.Should().Contain(i => i.name == "second.txt");
        attach_deleted_list.Should().NotContain(i => i.name == "second.txt");
    }

    [TestMethod]
    public async Task IssueCommentAttachmentExtensionScenario()
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

        // コメント作成
        var comment = await client.Issue.CreateCommentAsync(repoOwner, repoName, issue.number!.Value, new(body: "comment"));

        // ファイル情報で添付
        var attach_file = TestPathHelper.GetProjectDir().RelativeFile($"assets/texts/DummyText.txt");
        var attach_by_file = await client.Issue.CreateCommentFileAttachmentAsync(repoOwner, repoName, comment.id!.Value, attach_file);
        attach_by_file.name.Should().Be(attach_file.Name);

        // バイト列で添付
        var attach_bin = await attach_file.ReadAllBytesAsync();
        var attach_by_bin = await client.Issue.CreateCommentFileAttachmentAsync(repoOwner, repoName, comment.id!.Value, attach_bin, "test.txt");
        attach_by_bin.name.Should().Be("test.txt");
    }

    [TestMethod]
    public async Task IssueCommentReactionScenario()
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

        // コメント作成
        var comment = await client.Issue.CreateCommentAsync(repoOwner, repoName, issue.number!.Value, new(body: "comment"));

        // Reaction作成
        var reaction_1 = await client.Issue.AddCommentReactionAsync(repoOwner, repoName, comment.id!.Value, new(content: "+1"));

        // Reaction作成
        var reaction_2 = await client.Issue.AddCommentReactionAsync(repoOwner, repoName, comment.id!.Value, new(content: "heart"));

        // Reactionリスト取得
        var reaction_list = await client.Issue.ListCommentReactionsAsync(repoOwner, repoName, comment.id!.Value);

        // Reaction削除
        await client.Issue.RemoveCommentReactionAsync(repoOwner, repoName, comment.id!.Value, new(content: "heart"));

        // Reactionリスト取得
        var reaction_deleted_list = await client.Issue.ListCommentReactionsAsync(repoOwner, repoName, comment.id!.Value);

        // 検証
        reaction_1.content.Should().Be("+1");
        reaction_2.content.Should().Be("heart");
        reaction_list.Select(r => r.content).Should().Contain("+1", "heart");
        reaction_deleted_list.Select(r => r.content).Should().Contain("+1").And.NotContain("heart");
    }

    [TestMethod]
    public async Task IssueBlockScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var issue1 = await resources.CreateTestIssueAsync(repoOwner, repoName, "issue1");
        var issue2 = await resources.CreateTestIssueAsync(repoOwner, repoName, "issue2");

        // ブロック
        var block = await client.Issue.BlockAsync(repoOwner, repoName, issue1.number!.Value, new(index: issue2.number!.Value, owner: repoOwner, repo: repoName));

        // ブロックリスト取得
        var blocked_list = await client.Issue.ListBlockedAsync(repoOwner, repoName, issue1.number!.Value);

        // ブロック解除
        var unblock = await client.Issue.UnblockAsync(repoOwner, repoName, issue1.number!.Value, new(index: issue2.number!.Value, owner: repoOwner, repo: repoName));

        // ブロックリスト取得
        var unblocked_list = await client.Issue.ListBlockedAsync(repoOwner, repoName, issue1.number!.Value);

        // 検証
        block.number.Should().Be(issue1.number);
        blocked_list.Should().Contain(b => b.number == issue2.number);
        unblock.number.Should().Be(issue1.number);
        unblocked_list.Should().NotContain(b => b.number == issue2.number);
    }

    [TestMethod]
    public async Task IssueDependencyScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var issue1 = await resources.CreateTestIssueAsync(repoOwner, repoName, "issue1");
        var issue2 = await resources.CreateTestIssueAsync(repoOwner, repoName, "issue2");

        // ブロック
        var dependency = await client.Issue.MakeDependencyAsync(repoOwner, repoName, issue1.number!.Value, new(index: issue2.number!.Value, owner: repoOwner, repo: repoName));

        // ブロックリスト取得
        var dependency_list = await client.Issue.ListDependenciesAsync(repoOwner, repoName, issue1.number!.Value);

        // ブロック解除
        var undependency = await client.Issue.RemoveDependencyAsync(repoOwner, repoName, issue1.number!.Value, new(index: issue2.number!.Value, owner: repoOwner, repo: repoName));

        // ブロックリスト取得
        var undependency_list = await client.Issue.ListDependenciesAsync(repoOwner, repoName, issue1.number!.Value);

        // 検証
        dependency.number.Should().Be(issue1.number);
        dependency_list.Should().Contain(b => b.number == issue2.number);
        undependency.number.Should().Be(issue1.number);
        undependency_list.Should().NotContain(b => b.number == issue2.number);
    }

    [TestMethod]
    public async Task SetDeadlineAsync()
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

        // 期限設定
        var now = DateTimeOffset.Now;
        var deadline = await client.Issue.SetDeadlineAsync(repoOwner, repoName, issue.number!.Value, new(due_date: now));

        // 検証
        deadline.due_date.Should().BeSameDateAs(now);
    }

    [TestMethod]
    public async Task RepositoryLabelScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";
        var issueTitle = $"issue-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // ラベル作成
        var label_created = await client.Issue.CreateRepositoryLabelAsync(repoOwner, repoName, new(color: "#00aabb", name: "test"));

        // ラベル取得
        var label_get = await client.Issue.GetRepositoryLabelAsync(repoOwner, repoName, label_created.id!.Value);

        // ラベルリスト取得
        var label_list = await client.Issue.ListRepositoryLabelsAsync(repoOwner, repoName);

        // ラベル更新
        var label_updated = await client.Issue.UpdateRepositoryLabelAsync(repoOwner, repoName, label_created.id!.Value, new(color: "#aabb00"));

        // ラベルリスト取得
        var label_updated_list = await client.Issue.ListRepositoryLabelsAsync(repoOwner, repoName);

        // ラベル削除
        await client.Issue.DeleteRepositoryLabelAsync(repoOwner, repoName, label_created.id!.Value);

        // ラベルリスト取得
        var label_deleted_list = await client.Issue.ListRepositoryLabelsAsync(repoOwner, repoName);

        // 検証
        label_created.name.Should().Be("test");
        label_created.color.Should().Be("00aabb");
        label_get.name.Should().Be("test");
        label_get.color.Should().Be("00aabb");
        label_list.Should().Contain(b => b.name == "test" && b.color == "00aabb");
        label_updated.name.Should().Be("test");
        label_updated.color.Should().Be("aabb00");
        label_updated_list.Should().Contain(b => b.name == "test" && b.color == "aabb00");
        label_deleted_list.Should().NotContain(b => b.name == "test");
    }

    [TestMethod]
    public async Task IssueLabelScenario()
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

        // ラベル作成
        var label1 = await client.Issue.CreateRepositoryLabelAsync(repoOwner, repoName, new(name: "label1", color: "#ff0000"));
        var label2 = await client.Issue.CreateRepositoryLabelAsync(repoOwner, repoName, new(name: "label2", color: "#00ff00"));
        var label3 = await client.Issue.CreateRepositoryLabelAsync(repoOwner, repoName, new(name: "label3", color: "#0000ff"));

        // ラベル付与
        var label_added1 = await client.Issue.AddIssueLabelAsync(repoOwner, repoName, issue.number!.Value, new(labels: [label2.id!.Value]));
        var label_added2 = await client.Issue.AddIssueLabelAsync(repoOwner, repoName, issue.number!.Value, new(labels: [label3.id!.Value]));

        // ラベル作成
        var label_list = await client.Issue.ListIssueLabelsAsync(repoOwner, repoName, issue.number!.Value);

        // ラベル置換
        var lavel_replaced = await client.Issue.ReplaceIssueLabelsAsync(repoOwner, repoName, issue.number!.Value, new(labels: [label1.id!.Value]));

        // ラベル除去
        await client.Issue.RemoveIssueLabelAsync(repoOwner, repoName, issue.number!.Value, $"{label1.id}", new());

        // ラベル作成
        var label_removed = await client.Issue.ListIssueLabelsAsync(repoOwner, repoName, issue.number!.Value);

        // ラベル付与してクリア
        await client.Issue.AddIssueLabelAsync(repoOwner, repoName, issue.number!.Value, new(labels: [label1.id!.Value, label2.id!.Value, label3.id!.Value]));
        await client.Issue.ClearIssueLabelsAsync(repoOwner, repoName, issue.number!.Value, new());
        var label_cleared = await client.Issue.ListIssueLabelsAsync(repoOwner, repoName, issue.number!.Value);

        // 検証
        label_added1.Should().Contain(l => l.name == "label2");
        label_added2.Should().Contain(l => l.name == "label3");
        label_list.Should().Contain(l => l.name == "label2").And.Contain(l => l.name == "label3");
        lavel_replaced.Should().OnlyContain(l => l.name == "label1");
        label_removed.Should().BeEmpty();
        label_cleared.Should().BeEmpty();
    }

    [TestMethod]
    public async Task IssuePinScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);
        var issue1 = await resources.CreateTestIssueAsync(repoOwner, repoName, "issue1");
        var issue2 = await resources.CreateTestIssueAsync(repoOwner, repoName, "issue2");

        // イシュー取得
        var issue_ini = await client.Issue.GetAsync(repoOwner, repoName, issue1.number!.Value);

        // ピン
        await client.Issue.PinAsync(repoOwner, repoName, issue1.number!.Value);
        await client.Issue.PinAsync(repoOwner, repoName, issue2.number!.Value);

        // イシュー取得
        var issue_pinned1 = await client.Issue.GetAsync(repoOwner, repoName, issue1.number!.Value);
        var issue_pinned2 = await client.Issue.GetAsync(repoOwner, repoName, issue2.number!.Value);

        // ピン移動
        await client.Issue.MovePinAsync(repoOwner, repoName, issue2.number!.Value, 1);

        // イシュー取得
        var issue_pinned_moved1 = await client.Issue.GetAsync(repoOwner, repoName, issue1.number!.Value);
        var issue_pinned_moved2 = await client.Issue.GetAsync(repoOwner, repoName, issue2.number!.Value);

        // アンピン
        await client.Issue.UnpinAsync(repoOwner, repoName, issue2.number!.Value);

        // イシュー取得
        var issue_unpined2 = await client.Issue.GetAsync(repoOwner, repoName, issue2.number!.Value);

        // 検証
        issue_ini.pin_order.Should().Be(0);
        issue_pinned1.pin_order.Should().BeGreaterThan(0);
        issue_pinned2.pin_order.Should().BeGreaterThan(0);
        issue_pinned_moved1.pin_order.Should().BeGreaterThan(0);
        issue_pinned_moved2.pin_order.Should().BeGreaterThan(0);
        issue_pinned_moved2.pin_order.Should().NotBe(issue_pinned2.pin_order);
        issue_unpined2.pin_order.Should().Be(0);
    }

    [TestMethod]
    public async Task StopwatcheScenario()
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

        // イシューでストップウォッチを開始
        await client.Issue.StartStopwatchAsync(repoOwner, repoName, issue.number!.Value);

        // ストップウォッチリストを取得
        var sw_list_started = await client.User.ListStopwatchesAsync();

        // イシューでストップウォッチを停止
        await client.Issue.StopStopwatchAsync(repoOwner, repoName, issue.number!.Value);

        // ストップウォッチリストを取得
        var sw_list_stopped = await client.User.ListStopwatchesAsync();

        // 検証
        sw_list_started.Should().Contain(w => w.issue_index == issue.number);
        sw_list_stopped.Should().BeEmpty();
    }

    [TestMethod]
    public async Task StopwatcheDeleteScenario()
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

        // イシューでストップウォッチを開始
        await client.Issue.StartStopwatchAsync(repoOwner, repoName, issue.number!.Value);

        // ストップウォッチリストを取得
        var sw_list_started = await client.User.ListStopwatchesAsync();

        // イシューでストップウォッチを削除
        await client.Issue.DeleteStopwatchAsync(repoOwner, repoName, issue.number!.Value);

        // ストップウォッチリストを取得
        var sw_list_deleted = await client.User.ListStopwatchesAsync();

        // 検証
        sw_list_started.Should().Contain(w => w.issue_index == issue.number);
        sw_list_deleted.Should().BeEmpty();
    }

    [TestMethod]
    public async Task SubscribeUnsubscribeScenario()
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
        var user = await resources.CreateTestUserAsync($"user-{DateTime.Now.Ticks:X16}");

        // テスト用ユーザコンテキストのクライアントを作成
        var userClient = client.Sudo(user.login!);

        // ユーザがIssueを購読
        await client.Issue.SubscribeUserAsync(repoOwner, repoName, issue.number!.Value, user.login!);

        // ユーザがIssueを購読しているかチェック
        var watch_subscribed = await userClient.Issue.IsUserSubscribedAsync(repoOwner, repoName, issue.number!.Value);

        // ユーザがIssueを購読解除
        await client.Issue.UnsubscribeUserAsync(repoOwner, repoName, issue.number!.Value, user.login!);

        // ユーザがIssueを購読しているかチェック
        var watch_unsubscribed = await userClient.Issue.IsUserSubscribedAsync(repoOwner, repoName, issue.number!.Value);

        // 検証
        watch_subscribed.subscribed.Should().BeTrue();
        watch_unsubscribed.subscribed.Should().BeFalse();
    }

    [TestMethod]
    public async Task SubscribeListScenario()
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
        var user1 = await resources.CreateTestUserAsync($"user1-{DateTime.Now.Ticks:X16}");
        var user2 = await resources.CreateTestUserAsync($"user2-{DateTime.Now.Ticks:X16}");

        // ユーザ1がIssueを購読
        await client.Issue.SubscribeUserAsync(repoOwner, repoName, issue.number!.Value, user1.login!);

        // ユーザ2がIssueを購読
        await client.Issue.SubscribeUserAsync(repoOwner, repoName, issue.number!.Value, user2.login!);

        // Issue購読ユーザを取得
        var users = await client.Issue.ListSubscribedUsersAsync(repoOwner, repoName, issue.number!.Value);

        // 検証
        users.Select(u => u.login).Should().Contain(user1.login, user2.login);
    }

    [TestMethod]
    public async Task ListTimelineAsync()
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

        // Issueタイムライン取得。
        var timeline_new = await client.Issue.ListTimelineAsync(repoOwner, repoName, issue.number!.Value);

        // 作成直後のIssueではnullが返される。
        timeline_new.Should().BeNull();

        // コメントを付ける
        var comment = await client.Issue.CreateCommentAsync(repoOwner, repoName, issue.number!.Value, new(body: "comment"));

        // Issueタイムライン取得。
        var timeline = await client.Issue.ListTimelineAsync(repoOwner, repoName, issue.number!.Value);

        // なんらか得られる
        timeline.Should().NotBeNull();

    }

    [TestMethod]
    public async Task TrackedTimeScenario()
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

        // Time追加
        var time_added1 = await client.Issue.AddTrackedTimeAsync(repoOwner, repoName, issue.number!.Value, new(time: 100));

        // Time追加
        var time_added2 = await client.Issue.AddTrackedTimeAsync(repoOwner, repoName, issue.number!.Value, new(time: 200));

        // Timeリスト取得
        var time_list_added = await client.Issue.ListTrackedTimesAsync(repoOwner, repoName, issue.number!.Value);

        // Time削除
        await client.Issue.DeleteTrackedTimeAsync(repoOwner, repoName, issue.number!.Value, time_added1.id!.Value);

        // Timeリスト取得
        var time_list_deleted = await client.Issue.ListTrackedTimesAsync(repoOwner, repoName, issue.number!.Value);

        // Timeリセット
        await client.Issue.ResetTrackedTimeAsync(repoOwner, repoName, issue.number!.Value);

        // Timeリスト取得
        var time_list_reset = await client.Issue.ListTrackedTimesAsync(repoOwner, repoName, issue.number!.Value);

        // 検証
        time_added1.time.Should().Be(100);
        time_added2.time.Should().Be(200);
        time_list_added.Select(t => t.time).Should().Contain([100, 200]);
        time_list_deleted.Select(t => t.time).Should().Contain(200).And.NotContain(100);
        time_list_reset.Should().BeEmpty();
    }

    [TestMethod]
    public async Task MilestoneScenario()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);

        // テスト用エンティティ情報
        var repoOwner = this.TestTokenUser;
        var repoName = $"repo-{DateTime.Now.Ticks:X16}";

        // テスト用のエンティティを作成する。
        await using var resources = new TestForgejoResources(client);
        var repo = await resources.CreateTestUserRepoAsync(repoOwner, repoName);

        // Milestone作成
        var milestone_created = await client.Issue.CreateMilestoneAsync(repoOwner, repoName, new(title: "milestone1"));

        // Milestone取得
        var milestone_get = await client.Issue.GetMilestoneAsync(repoOwner, repoName, milestone_created.id!.Value);

        // Milestoneリスト取得
        var milestone_list_created = await client.Issue.ListMilestonesAsync(repoOwner, repoName);

        // Milestone更新
        var milestone_updated = await client.Issue.UpdateMilestoneAsync(repoOwner, repoName, milestone_created.id!.Value, new(title: "milestone2"));

        // Milestoneリスト取得
        var milestone_list_updated = await client.Issue.ListMilestonesAsync(repoOwner, repoName);

        // Milestone削除
        await client.Issue.DeleteMilestoneAsync(repoOwner, repoName, milestone_created.id!.Value);

        // Milestoneリスト取得
        var milestone_list_deteled = await client.Issue.ListMilestonesAsync(repoOwner, repoName);

        // 検証
        milestone_created.title.Should().Be("milestone1");
        milestone_get.title.Should().Be("milestone1");
        milestone_list_created.Select(t => t.title).Should().Contain("milestone1");
        milestone_updated.title.Should().Be("milestone2");
        milestone_list_updated.Select(t => t.title).Should().Contain("milestone2");
        milestone_list_deteled.Should().BeEmpty();
    }

}
