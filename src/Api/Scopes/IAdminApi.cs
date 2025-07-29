namespace ForgejoApiClient.Api.Scopes;

/// <summary>admin スコープのAPIインタフェース</summary>
public interface IAdminApi : IApiScope
{
    #region Action
    /// <summary>List cron tasks</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CronList</returns>
    [ForgejoEndpoint("GET", "/admin/cron", "List cron tasks")]
    public Task<Cron[]> ListCronsAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/cron".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Cron[]>(cancelToken);

    /// <summary>Run cron task</summary>
    /// <param name="task">task to run</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/admin/cron/{task}", "Run cron task")]
    public Task RunCronTaskAsync(string task, CancellationToken cancelToken = default)
        => PostRequest($"admin/cron/{task}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Get an global actions runner registration token</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/admin/runners/registration-token", "Get an global actions runner registration token")]
    [ManualEdit("結果値が得られるため独自型を定義して利用")]
    public Task<RegistrationTokenResult> GetActionRunnerRegistrationTokenAsync(CancellationToken cancelToken = default)
        => GetRequest("admin/runners/registration-token", cancelToken).JsonResponseAsync<RegistrationTokenResult>(cancelToken);

    /// <summary>Search action jobs according filter conditions</summary>
    /// <param name="labels">a comma separated list of run job labels to search for</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RunJobList is a list of action run jobs</returns>
    [ForgejoEndpoint("GET", "/admin/runners/jobs", "Search action jobs according filter conditions")]
    [ManualEdit("戻り値を nullable に変更")]
    public Task<ActionRunJob[]?> GetActionJobsAsync(string? labels = default, CancellationToken cancelToken = default)
        => GetRequest("admin/runners/jobs".WithQuery().Param(labels), cancelToken).JsonResponseAsync<ActionRunJob[]?>(cancelToken);
    #endregion

    #region Profile
    /// <summary>List all emails</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>EmailList</returns>
    [ForgejoEndpoint("GET", "/admin/emails", "List all emails")]
    public Task<Email[]> ListEmailsAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/emails".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Email[]>(cancelToken);

    /// <summary>Search all emails</summary>
    /// <param name="q">keyword</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>EmailList</returns>
    [ForgejoEndpoint("GET", "/admin/emails/search", "Search all emails")]
    public Task<Email[]> SearchEmailsAsync(string? q = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/emails/search".WithQuery().Param(q).Param(paging), cancelToken).JsonResponseAsync<Email[]>(cancelToken);
    #endregion

    #region Webhook
    /// <summary>List system&apos;s webhooks</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>HookList</returns>
    [ForgejoEndpoint("GET", "/admin/hooks", "List system's webhooks")]
    public Task<Hook[]> ListSystemWebhooksAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/hooks".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Hook[]>(cancelToken);

    /// <summary>Get a hook</summary>
    /// <param name="id">id of the hook to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("GET", "/admin/hooks/{id}", "Get a hook")]
    public Task<Hook> GetWebhookAsync(long id, CancellationToken cancelToken = default)
        => GetRequest($"admin/hooks/{id}", cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Create a hook</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("POST", "/admin/hooks", "Create a hook")]
    public Task<Hook> CreateWebhookAsync(CreateHookOption options, CancellationToken cancelToken = default)
        => PostRequest("admin/hooks", options, cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Update a hook</summary>
    /// <param name="id">id of the hook to update</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("PATCH", "/admin/hooks/{id}", "Update a hook")]
    public Task<Hook> UpdateWebhookAsync(long id, EditHookOption options, CancellationToken cancelToken = default)
        => PatchRequest($"admin/hooks/{id}", options, cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Delete a hook</summary>
    /// <param name="id">id of the hook to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/admin/hooks/{id}", "Delete a hook")]
    public Task DeleteWebhookAsync(long id, CancellationToken cancelToken = default)
        => DeleteRequest($"admin/hooks/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Repository
    /// <summary>List unadopted repositories</summary>
    /// <param name="pattern">pattern of repositories to search for</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>StringSlice</returns>
    [ForgejoEndpoint("GET", "/admin/unadopted", "List unadopted repositories")]
    public Task<string[]> ListUnadoptedReposAsync(string? pattern = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/unadopted".WithQuery().Param(pattern).Param(paging), cancelToken).JsonResponseAsync<string[]>(cancelToken);

    /// <summary>Adopt unadopted files as a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/admin/unadopted/{owner}/{repo}", "Adopt unadopted files as a repository")]
    public Task AdoptUnadoptedRepositoryAsync(string owner, string repo, CancellationToken cancelToken = default)
        => PostRequest($"admin/unadopted/{owner}/{repo}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete unadopted files</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/admin/unadopted/{owner}/{repo}", "Delete unadopted files")]
    public Task DeleteUnadoptedRepositoryAsync(string owner, string repo, CancellationToken cancelToken = default)
        => DeleteRequest($"admin/unadopted/{owner}/{repo}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Create a repository on behalf of a user</summary>
    /// <param name="username">username of the user. This user will own the created repository</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/admin/users/{username}/repos", "Create a repository on behalf of a user")]
    public Task<Repository> CreateUserRepoAsync(string username, CreateRepoOption options, CancellationToken cancelToken = default)
        => PostRequest($"admin/users/{username}/repos", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);
    #endregion

    #region Organization
    /// <summary>List all organizations</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OrganizationList</returns>
    [ForgejoEndpoint("GET", "/admin/orgs", "List all organizations")]
    public Task<Organization[]> ListOrganizationsAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/orgs".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Organization[]>(cancelToken);

    /// <summary>Create an organization</summary>
    /// <param name="username">username of the user that will own the created organization</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Organization</returns>
    [ForgejoEndpoint("POST", "/admin/users/{username}/orgs", "Create an organization")]
    public Task<Organization> CreateOrganizationAsync(string username, CreateOrgOption options, CancellationToken cancelToken = default)
        => PostRequest($"admin/users/{username}/orgs", options, cancelToken).JsonResponseAsync<Organization>(cancelToken);
    #endregion

    #region UserManage
    /// <summary>Search users according filter conditions</summary>
    /// <param name="source_id">ID of the user&apos;s login source to search for</param>
    /// <param name="login_name">user&apos;s login name to search for</param>
    /// <param name="sort">sort order of results</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/admin/users", "Search users according filter conditions")]
    public Task<User[]> ListUsersAsync(long? source_id = default, string? login_name = default, string? sort = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/users".WithQuery().Param(source_id).Param(login_name).Param(sort).Param(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Create a user</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>User</returns>
    [ForgejoEndpoint("POST", "/admin/users", "Create a user")]
    public Task<User> CreateUserAsync(CreateUserOption options, CancellationToken cancelToken = default)
        => PostRequest("admin/users", options, cancelToken).JsonResponseAsync<User>(cancelToken);

    /// <summary>Edit an existing user</summary>
    /// <param name="username">username of user to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>User</returns>
    [ForgejoEndpoint("PATCH", "/admin/users/{username}", "Edit an existing user")]
    public Task<User> UpdateUserAsync(string username, EditUserOption options, CancellationToken cancelToken = default)
        => PatchRequest($"admin/users/{username}", options, cancelToken).JsonResponseAsync<User>(cancelToken);

    /// <summary>Rename a user</summary>
    /// <param name="username">existing username of user</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/admin/users/{username}/rename", "Rename a user")]
    public Task RenameUserAsync(string username, RenameUserOption options, CancellationToken cancelToken = default)
        => PostRequest($"admin/users/{username}/rename", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a user</summary>
    /// <param name="username">username of user to delete</param>
    /// <param name="purge">purge the user from the system completely</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/admin/users/{username}", "Delete a user")]
    public Task DeleteUserAsync(string username, bool? purge = default, CancellationToken cancelToken = default)
        => DeleteRequest($"admin/users/{username}".WithQuery().Param(purge), cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Key
    /// <summary>Add a public key on behalf of a user</summary>
    /// <param name="username">username of the user</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PublicKey</returns>
    [ForgejoEndpoint("POST", "/admin/users/{username}/keys", "Add a public key on behalf of a user")]
    public Task<PublicKey> AddUserPublicKeyAsync(string username, CreateKeyOption options, CancellationToken cancelToken = default)
        => PostRequest($"admin/users/{username}/keys", options, cancelToken).JsonResponseAsync<PublicKey>(cancelToken);

    /// <summary>Delete a user&apos;s public key</summary>
    /// <param name="username">username of user</param>
    /// <param name="id">id of the key to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/admin/users/{username}/keys/{id}", "Delete a user's public key")]
    public Task DeleteUserPublicKeyAsync(string username, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"admin/users/{username}/keys/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Quota
    /// <summary>List the available quota rules</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaRuleInfoList</returns>
    [ForgejoEndpoint("GET", "/admin/quota/rules", "List the available quota rules")]
    public Task<QuotaRuleInfo[]> ListQuotaRulesAsync(CancellationToken cancelToken = default)
        => GetRequest("admin/quota/rules", cancelToken).JsonResponseAsync<QuotaRuleInfo[]>(cancelToken);

    /// <summary>Get information about a quota rule</summary>
    /// <param name="quotarule">quota rule to query</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaRuleInfo</returns>
    [ForgejoEndpoint("GET", "/admin/quota/rules/{quotarule}", "Get information about a quota rule")]
    public Task<QuotaRuleInfo> GetQuotaRuleAsync(string quotarule, CancellationToken cancelToken = default)
        => GetRequest($"admin/quota/rules/{quotarule}", cancelToken).JsonResponseAsync<QuotaRuleInfo>(cancelToken);

    /// <summary>Create a new quota rule</summary>
    /// <param name="options">Definition of the quota rule</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaRuleInfo</returns>
    [ForgejoEndpoint("POST", "/admin/quota/rules", "Create a new quota rule")]
    public Task<QuotaRuleInfo> CreateQuotaRuleAsync(CreateQuotaRuleOptions options, CancellationToken cancelToken = default)
        => PostRequest("admin/quota/rules", options, cancelToken).JsonResponseAsync<QuotaRuleInfo>(cancelToken);

    /// <summary>Change an existing quota rule</summary>
    /// <param name="quotarule">Quota rule to change</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaRuleInfo</returns>
    [ForgejoEndpoint("PATCH", "/admin/quota/rules/{quotarule}", "Change an existing quota rule")]
    public Task<QuotaRuleInfo> UpdateQuotaRuleAsync(string quotarule, EditQuotaRuleOptions options, CancellationToken cancelToken = default)
        => PatchRequest($"admin/quota/rules/{quotarule}", options, cancelToken).JsonResponseAsync<QuotaRuleInfo>(cancelToken);

    /// <summary>Deletes a quota rule</summary>
    /// <param name="quotarule">quota rule to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/admin/quota/rules/{quotarule}", "Deletes a quota rule")]
    public Task DeleteQuotaRuleAsync(string quotarule, CancellationToken cancelToken = default)
        => DeleteRequest($"admin/quota/rules/{quotarule}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Get the user&apos;s quota info</summary>
    /// <param name="username">username of user to query</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaInfo</returns>
    [ForgejoEndpoint("GET", "/admin/users/{username}/quota", "Get the user's quota info")]
    public Task<QuotaInfo> GetUserQuotaRuleAsync(string username, CancellationToken cancelToken = default)
        => GetRequest($"admin/users/{username}/quota", cancelToken).JsonResponseAsync<QuotaInfo>(cancelToken);

    /// <summary>Set the user&apos;s quota groups to a given list.</summary>
    /// <param name="username">username of the user to modify the quota groups from</param>
    /// <param name="options">list of groups that the user should be a member of</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/admin/users/{username}/quota/groups", "Set the user's quota groups to a given list.")]
    public Task SetUserQuotaGroupAsync(string username, SetUserQuotaGroupsOptions options, CancellationToken cancelToken = default)
        => PostRequest($"admin/users/{username}/quota/groups", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List the available quota groups</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaGroupList</returns>
    [ForgejoEndpoint("GET", "/admin/quota/groups", "List the available quota groups")]
    public Task<QuotaGroup[]> ListQuotaGroupsAsync(CancellationToken cancelToken = default)
        => GetRequest("admin/quota/groups", cancelToken).JsonResponseAsync<QuotaGroup[]>(cancelToken);

    /// <summary>Get information about the quota group</summary>
    /// <param name="quotagroup">quota group to query</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaGroup</returns>
    [ForgejoEndpoint("GET", "/admin/quota/groups/{quotagroup}", "Get information about the quota group")]
    public Task<QuotaGroup> GetQuotaGroupAsync(string quotagroup, CancellationToken cancelToken = default)
        => GetRequest($"admin/quota/groups/{quotagroup}", cancelToken).JsonResponseAsync<QuotaGroup>(cancelToken);

    /// <summary>Create a new quota group</summary>
    /// <param name="options">Definition of the quota group</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaGroup</returns>
    [ForgejoEndpoint("POST", "/admin/quota/groups", "Create a new quota group")]
    public Task<QuotaGroup> CreateQuotaGroupAsync(CreateQuotaGroupOptions options, CancellationToken cancelToken = default)
        => PostRequest("admin/quota/groups", options, cancelToken).JsonResponseAsync<QuotaGroup>(cancelToken);

    /// <summary>Delete a quota group</summary>
    /// <param name="quotagroup">quota group to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/admin/quota/groups/{quotagroup}", "Delete a quota group")]
    public Task DeleteQuotaGroupAsync(string quotagroup, CancellationToken cancelToken = default)
        => DeleteRequest($"admin/quota/groups/{quotagroup}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Adds a rule to a quota group</summary>
    /// <param name="quotagroup">quota group to add a rule to</param>
    /// <param name="quotarule">the name of the quota rule to add to the group</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/admin/quota/groups/{quotagroup}/rules/{quotarule}", "Adds a rule to a quota group")]
    public Task AddQuotaGroupRuleAsync(string quotagroup, string quotarule, CancellationToken cancelToken = default)
        => PutRequest($"admin/quota/groups/{quotagroup}/rules/{quotarule}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Removes a rule from a quota group</summary>
    /// <param name="quotagroup">quota group to remove a rule from</param>
    /// <param name="quotarule">the name of the quota rule to remove from the group</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/admin/quota/groups/{quotagroup}/rules/{quotarule}", "Removes a rule from a quota group")]
    public Task RemoveQuotaGroupRuleAsync(string quotagroup, string quotarule, CancellationToken cancelToken = default)
        => DeleteRequest($"admin/quota/groups/{quotagroup}/rules/{quotarule}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List users in a quota group</summary>
    /// <param name="quotagroup">quota group to list members of</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/admin/quota/groups/{quotagroup}/users", "List users in a quota group")]
    public Task<User[]> ListQuotaGroupUsersAsync(string quotagroup, CancellationToken cancelToken = default)
        => GetRequest($"admin/quota/groups/{quotagroup}/users", cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Add a user to a quota group</summary>
    /// <param name="quotagroup">quota group to add the user to</param>
    /// <param name="username">username of the user to add to the quota group</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/admin/quota/groups/{quotagroup}/users/{username}", "Add a user to a quota group")]
    public Task AddQuotaGroupUserAsync(string quotagroup, string username, CancellationToken cancelToken = default)
        => PutRequest($"admin/quota/groups/{quotagroup}/users/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Remove a user from a quota group</summary>
    /// <param name="quotagroup">quota group to remove a user from</param>
    /// <param name="username">username of the user to remove from the quota group</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/admin/quota/groups/{quotagroup}/users/{username}", "Remove a user from a quota group")]
    public Task RemoveQuotaGroupUserAsync(string quotagroup, string username, CancellationToken cancelToken = default)
        => DeleteRequest($"admin/quota/groups/{quotagroup}/users/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

}
