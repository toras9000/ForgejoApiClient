
using ForgejoApiClient.Api;

namespace ForgejoApiClient.Tests.helper;

public sealed class TestForgejoResources : IAsyncDisposable
{
    public TestForgejoResources(ForgejoClient client)
    {
        this.Client = client;
        this.resources = new();
    }

    public ForgejoClient Client { get; }

    /// <summary>テスト用ユーザを作成</summary>
    public Task<User> CreateTestUserAsync(string name, string? mail = default, string? password = default)
        => CreateTestUserAsync(new CreateUserOption(mail ?? $"{name}@example.com", name, password: password ?? "password", must_change_password: false));

    /// <summary>テスト用ユーザを作成</summary>
    public Task<User> CreateTestUserAsync(CreateUserOption options)
        => this.Client.Admin.CreateUserAsync(options).WillBeDiscarded(this);

    /// <summary>テスト用組織を作成</summary>
    public Task<Organization> CreateTestOrgAsync(string name)
        => CreateTestOrgAsync(new CreateOrgOption(name));

    /// <summary>テスト用組織を作成</summary>
    public Task<Organization> CreateTestOrgAsync(CreateOrgOption options)
        => this.Client.Organization.CreateAsync(options).WillBeDiscarded(this);

    /// <summary>テスト用チームを作成</summary>
    public Task<Team> CreateTestTeamAsync(string org, string team)
        => CreateTestTeamAsync(org, new CreateTeamOption(team, permission: CreateTeamOptionPermission.Read, units: ["activitypub", "admin", "issue", "misc", "notification", "organization", "package", "repository", "user"]));

    /// <summary>テスト用チームを作成</summary>
    public Task<Team> CreateTestTeamAsync(string org, CreateTeamOption options)
        => this.Client.Organization.CreateTeamAsync(org, options).WillBeDiscarded(this);

    /// <summary>テスト用リポジトリを作成</summary>
    public Task<Repository> CreateTestRepoAsync(string reponame)
        => CreateTestRepoAsync(new CreateRepoOption(reponame));

    /// <summary>テスト用リポジトリを作成</summary>
    public Task<Repository> CreateTestRepoAsync(CreateRepoOption options)
        => this.Client.Repository.CreateAsync(options).WillBeDiscarded(this);

    /// <summary>テスト用ユーザリポジトリを作成</summary>
    public Task<Repository> CreateTestUserRepoAsync(string username, string reponame)
        => CreateTestUserRepoAsync(username, new CreateRepoOption(reponame));

    /// <summary>テスト用ユーザリポジトリを作成</summary>
    public Task<Repository> CreateTestUserRepoAsync(string username, CreateRepoOption options)
        => this.Client.Admin.CreateUserRepoAsync(username, options).WillBeDiscarded(this);

    /// <summary>テスト用組織リポジトリを作成</summary>
    public Task<Repository> CreateTestOrgRepoAsync(string username, string reponame)
        => CreateTestOrgRepoAsync(username, new CreateRepoOption(reponame));

    /// <summary>テスト用組織リポジトリを作成</summary>
    public Task<Repository> CreateTestOrgRepoAsync(string username, CreateRepoOption options)
        => this.Client.Organization.CreateRepositoryAsync(username, options).WillBeDiscarded(this);

    /// <summary>テスト用イシューを作成</summary>
    public Task<Issue> CreateTestIssueAsync(string owner, string repo, string title)
        => CreateTestIssueAsync(owner, repo, new CreateIssueOption(title));

    /// <summary>テスト用イシューを作成</summary>
    public Task<Issue> CreateTestIssueAsync(string owner, string repo, CreateIssueOption options)
        => this.Client.Issue.CreateAsync(owner, repo, options).WillBeDiscarded(this);

    /// <summary>テスト用クォータルールを作成</summary>
    public Task<QuotaRuleInfo> CreateTestQuotaRuleAsync(string name, long limit, ICollection<string> subjects)
        => CreateTestQuotaRuleAsync(new CreateQuotaRuleOptions(name: name, limit: limit, subjects: subjects));

    /// <summary>テスト用クォータルールを作成</summary>
    public Task<QuotaRuleInfo> CreateTestQuotaRuleAsync(CreateQuotaRuleOptions options)
        => this.Client.Admin.CreateQuotaRuleAsync(options).WillBeDiscarded(this);

    /// <summary>テスト用クォータグループを作成</summary>
    public Task<QuotaGroup> CreateTestQuotaGroupAsync(string name, ICollection<CreateQuotaRuleOptions>? rules = default)
        => CreateTestQuotaGroupAsync(new CreateQuotaGroupOptions(name: name, rules: rules));

    /// <summary>テスト用クォータグループを作成</summary>
    public Task<QuotaGroup> CreateTestQuotaGroupAsync(CreateQuotaGroupOptions options)
        => this.Client.Admin.CreateQuotaGroupAsync(options).WillBeDiscarded(this);


    public User ToBeDiscarded(User user)
    {
        this.resources.Add(new(() => this.Client.Admin.DeleteUserAsync(user.login!)));
        return user;
    }

    public Organization ToBeDiscarded(Organization org)
    {
        this.resources.Add(new(() => this.Client.Organization.DeleteAsync(org.name!)));
        return org;
    }

    public Team ToBeDiscarded(Team team)
    {
        this.resources.Add(new(() => this.Client.Organization.DeleteTeamAsync(team.id!.Value)));
        return team;
    }

    public Repository ToBeDiscarded(Repository repo)
    {
        this.resources.Add(new(() => this.Client.Repository.DeleteAsync(repo.owner!.login!, repo.name!)));
        return repo;
    }

    public Issue ToBeDiscarded(Issue issue)
    {
        this.resources.Add(new(() => this.Client.Issue.DeleteAsync(issue.repository!.owner!, issue.repository!.name!, issue.number!.Value)));
        return issue;
    }

    public QuotaRuleInfo ToBeDiscarded(QuotaRuleInfo rule)
    {
        this.resources.Add(new(() => this.Client.Admin.DeleteQuotaRuleAsync(rule.name!)));
        return rule;
    }

    public QuotaGroup ToBeDiscarded(QuotaGroup group)
    {
        this.resources.Add(new(() => this.Client.Admin.DeleteQuotaGroupAsync(group.name!)));
        return group;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var res in this.resources.AsEnumerable().Reverse())
        {
            try { await res.DisposeAsync().ConfigureAwait(false); } catch { }
        }
        this.resources.Clear();
    }

    private class TestResource : IAsyncDisposable
    {
        public TestResource(Func<Task> disposer)
        {
            this.disposer = disposer;
        }

        public async ValueTask DisposeAsync()
        {
            await this.disposer().ConfigureAwait(false);
        }

        private readonly Func<Task> disposer;
    }

    private readonly List<TestResource> resources;
}


public static class TestResourceContainerExtensions
{
    public static async Task<User> WillBeDiscarded(this Task<User> self, TestForgejoResources resources)
        => resources.ToBeDiscarded(await self.ConfigureAwait(false));

    public static async Task<Organization> WillBeDiscarded(this Task<Organization> self, TestForgejoResources resources)
        => resources.ToBeDiscarded(await self.ConfigureAwait(false));

    public static async Task<Team> WillBeDiscarded(this Task<Team> self, TestForgejoResources resources)
        => resources.ToBeDiscarded(await self.ConfigureAwait(false));

    public static async Task<Repository> WillBeDiscarded(this Task<Repository> self, TestForgejoResources resources)
        => resources.ToBeDiscarded(await self.ConfigureAwait(false));

    public static async Task<Issue> WillBeDiscarded(this Task<Issue> self, TestForgejoResources resources)
        => resources.ToBeDiscarded(await self.ConfigureAwait(false));

    public static async Task<QuotaRuleInfo> WillBeDiscarded(this Task<QuotaRuleInfo> self, TestForgejoResources resources)
        => resources.ToBeDiscarded(await self.ConfigureAwait(false));

    public static async Task<QuotaGroup> WillBeDiscarded(this Task<QuotaGroup> self, TestForgejoResources resources)
        => resources.ToBeDiscarded(await self.ConfigureAwait(false));

}
