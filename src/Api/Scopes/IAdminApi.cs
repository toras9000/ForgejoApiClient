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
        => GetRequest("admin/cron".WithQuery(paging), cancelToken).JsonResponseAsync<Cron[]>(cancelToken);

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
    #endregion

    #region Profile
    /// <summary>List all emails</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>EmailList</returns>
    [ForgejoEndpoint("GET", "/admin/emails", "List all emails")]
    public Task<Email[]> ListEmailsAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/emails".WithQuery(paging), cancelToken).JsonResponseAsync<Email[]>(cancelToken);

    /// <summary>Search all emails</summary>
    /// <param name="q">keyword</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>EmailList</returns>
    [ForgejoEndpoint("GET", "/admin/emails/search", "Search all emails")]
    public Task<Email[]> SearchEmailsAsync(string? q = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/emails/search".WithQuery(q).Param(paging), cancelToken).JsonResponseAsync<Email[]>(cancelToken);
    #endregion

    #region Webhook
    /// <summary>List system&apos;s webhooks</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>HookList</returns>
    [ForgejoEndpoint("GET", "/admin/hooks", "List system's webhooks")]
    public Task<Hook[]> ListSystemWebhooksAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/hooks".WithQuery(paging), cancelToken).JsonResponseAsync<Hook[]>(cancelToken);

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
        => GetRequest("admin/unadopted".WithQuery(pattern).Param(paging), cancelToken).JsonResponseAsync<string[]>(cancelToken);

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
        => GetRequest("admin/orgs".WithQuery(paging), cancelToken).JsonResponseAsync<Organization[]>(cancelToken);

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
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/admin/users", "Search users according filter conditions")]
    public Task<User[]> ListUsersAsync(long? source_id = default, string? login_name = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("admin/users".WithQuery(source_id).Param(login_name).Param(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

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
        => DeleteRequest($"admin/users/{username}".WithQuery(purge), cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
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

}
