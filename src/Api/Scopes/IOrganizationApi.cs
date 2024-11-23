namespace ForgejoApiClient.Api.Scopes;

/// <summary>organization スコープのAPIインタフェース</summary>
public interface IOrganizationApi : IApiScope
{
    #region Organization
    /// <summary>Get list of organizations</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OrganizationList</returns>
    [ForgejoEndpoint("GET", "/orgs", "Get list of organizations")]
    public Task<Organization[]> ListAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("orgs".WithQuery(paging), cancelToken).JsonResponseAsync<Organization[]>(cancelToken);

    /// <summary>Get an organization</summary>
    /// <param name="org">name of the organization to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Organization</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}", "Get an organization")]
    public Task<Organization> GetAsync(string org, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}", cancelToken).JsonResponseAsync<Organization>(cancelToken);

    /// <summary>Create an organization</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Organization</returns>
    [ForgejoEndpoint("POST", "/orgs", "Create an organization")]
    public Task<Organization> CreateAsync(CreateOrgOption options, CancellationToken cancelToken = default)
        => PostRequest("orgs", options, cancelToken).JsonResponseAsync<Organization>(cancelToken);

    /// <summary>Edit an organization</summary>
    /// <param name="org">name of the organization to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Organization</returns>
    [ForgejoEndpoint("PATCH", "/orgs/{org}", "Edit an organization")]
    public Task<Organization> UpdateAsync(string org, EditOrgOption options, CancellationToken cancelToken = default)
        => PatchRequest($"orgs/{org}", options, cancelToken).JsonResponseAsync<Organization>(cancelToken);

    /// <summary>Delete an organization</summary>
    /// <param name="org">organization that is to be deleted</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/orgs/{org}", "Delete an organization")]
    public Task DeleteAsync(string org, CancellationToken cancelToken = default)
        => DeleteRequest($"orgs/{org}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List the current user&apos;s organizations</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OrganizationList</returns>
    [ForgejoEndpoint("GET", "/user/orgs", "List the current user's organizations")]
    public Task<Organization[]> ListMyOrgsAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/orgs".WithQuery(paging), cancelToken).JsonResponseAsync<Organization[]>(cancelToken);

    /// <summary>List a user&apos;s organizations</summary>
    /// <param name="username">username of user</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OrganizationList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/orgs", "List a user's organizations")]
    public Task<Organization[]> ListUserOrgsAsync(string username, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/orgs".WithQuery(paging), cancelToken).JsonResponseAsync<Organization[]>(cancelToken);
    #endregion

    #region Org Member
    /// <summary>List an organization&apos;s members</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/members", "List an organization's members")]
    public Task<User[]> ListMembersAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/members".WithQuery(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Check if a user is a member of an organization</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/orgs/{org}/members/{username}", "Check if a user is a member of an organization")]
    public Task<StatusCodeResult> CheckMemberAsync(string org, string username, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/members/{username}", cancelToken).StatusResponseAsync(cancelToken);

    /// <summary>Remove a member from an organization</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/orgs/{org}/members/{username}", "Remove a member from an organization")]
    public Task RemoveMemberAsync(string org, string username, CancellationToken cancelToken = default)
        => DeleteRequest($"orgs/{org}/members/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List an organization&apos;s public members</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/public_members", "List an organization's public members")]
    public Task<User[]> ListPublicMembersAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/public_members".WithQuery(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Check if a user is a public member of an organization</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/orgs/{org}/public_members/{username}", "Check if a user is a public member of an organization")]
    public Task<StatusCodeResult> CheckPublicMemberAsync(string org, string username, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/public_members/{username}", cancelToken).StatusResponseAsync(cancelToken);

    /// <summary>Publicize a user&apos;s membership</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/orgs/{org}/public_members/{username}", "Publicize a user's membership")]
    public Task PublicizeMemberAsync(string org, string username, CancellationToken cancelToken = default)
        => PutRequest($"orgs/{org}/public_members/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Conceal a user&apos;s membership</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/orgs/{org}/public_members/{username}", "Conceal a user's membership")]
    public Task ConcealMemberAsync(string org, string username, CancellationToken cancelToken = default)
        => DeleteRequest($"orgs/{org}/public_members/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Org Repository
    /// <summary>List an organization&apos;s repos</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/repos", "List an organization's repos")]
    public Task<Repository[]> ListRepositoriesAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/repos".WithQuery(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);

    /// <summary>Create a repository in an organization</summary>
    /// <param name="org">name of organization</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/orgs/{org}/repos", "Create a repository in an organization")]
    public Task<Repository> CreateRepositoryAsync(string org, CreateRepoOption options, CancellationToken cancelToken = default)
        => PostRequest($"orgs/{org}/repos", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);
    #endregion

    #region Org Misc
    /// <summary>Get an organization&apos;s actions runner registration token</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/orgs/{org}/actions/runners/registration-token", "Get an organization's actions runner registration token")]
    [ManualEdit("結果値が得られるため独自型を定義して利用")]
    public Task<RegistrationTokenResult> GetActionRunnerRegistrationTokenAsync(string org, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/actions/runners/registration-token", cancelToken).JsonResponseAsync<RegistrationTokenResult>(cancelToken);

    /// <summary>List an organization&apos;s activity feeds</summary>
    /// <param name="org">name of the org</param>
    /// <param name="date">the date of the activities to be found</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActivityFeedsList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/activities/feeds", "List an organization's activity feeds")]
    public Task<Activity[]> ListActivitiesAsync(string org, DateTimeOffset? date = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/activities/feeds".WithQuery(date).Param(paging), cancelToken).JsonResponseAsync<Activity[]>(cancelToken);

    /// <summary>Update Avatar</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/orgs/{org}/avatar", "Update Avatar")]
    public Task UpdateAvatarAsync(string org, UpdateUserAvatarOption options, CancellationToken cancelToken = default)
        => PostRequest($"orgs/{org}/avatar", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete Avatar</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/orgs/{org}/avatar", "Delete Avatar")]
    public Task DeleteAvatarAsync(string org, CancellationToken cancelToken = default)
        => DeleteRequest($"orgs/{org}/avatar", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Action Secret
    /// <summary>List an organization&apos;s actions secrets</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>SecretList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/actions/secrets", "List an organization's actions secrets")]
    public Task<Secret[]> ListActionSecretsAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/actions/secrets".WithQuery(paging), cancelToken).JsonResponseAsync<Secret[]>(cancelToken);

    /// <summary>Create or Update a secret value in an organization</summary>
    /// <param name="org">name of organization</param>
    /// <param name="secretname">name of the secret</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/orgs/{org}/actions/secrets/{secretname}", "Create or Update a secret value in an organization")]
    public Task SetActionSecretAsync(string org, string secretname, CreateOrUpdateSecretOption options, CancellationToken cancelToken = default)
        => PutRequest($"orgs/{org}/actions/secrets/{secretname}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a secret in an organization</summary>
    /// <param name="org">name of organization</param>
    /// <param name="secretname">name of the secret</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/orgs/{org}/actions/secrets/{secretname}", "Delete a secret in an organization")]
    public Task DeleteActionSecretAsync(string org, string secretname, CancellationToken cancelToken = default)
        => DeleteRequest($"orgs/{org}/actions/secrets/{secretname}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Action Variable
    /// <summary>Get an org-level variables list</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>VariableList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/actions/variables", "Get an org-level variables list")]
    public Task<ActionVariable[]> ListActionVariablesAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/actions/variables".WithQuery(paging), cancelToken).JsonResponseAsync<ActionVariable[]>(cancelToken);

    /// <summary>Get an org-level variable</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="variablename">name of the variable</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActionVariable</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/actions/variables/{variablename}", "Get an org-level variable")]
    public Task<ActionVariable> GetActionVariableAsync(string org, string variablename, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/actions/variables/{variablename}", cancelToken).JsonResponseAsync<ActionVariable>(cancelToken);

    /// <summary>Create an org-level variable</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="variablename">name of the variable</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/orgs/{org}/actions/variables/{variablename}", "Create an org-level variable")]
    public Task CreateActionVariableAsync(string org, string variablename, CreateVariableOption options, CancellationToken cancelToken = default)
        => PostRequest($"orgs/{org}/actions/variables/{variablename}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Update an org-level variable</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="variablename">name of the variable</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/orgs/{org}/actions/variables/{variablename}", "Update an org-level variable")]
    public Task UpdateActionVariableAsync(string org, string variablename, UpdateVariableOption options, CancellationToken cancelToken = default)
        => PutRequest($"orgs/{org}/actions/variables/{variablename}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete an org-level variable</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="variablename">name of the variable</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActionVariable</returns>
    [ForgejoEndpoint("DELETE", "/orgs/{org}/actions/variables/{variablename}", "Delete an org-level variable")]
    [ManualEdit("Swaggerの戻り値定義誤りを訂正")]
    public Task DeleteActionVariableAsync(string org, string variablename, CancellationToken cancelToken = default)
        => DeleteRequest($"orgs/{org}/actions/variables/{variablename}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Social
    /// <summary>List the organization&apos;s blocked users</summary>
    /// <param name="org">name of the org</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>BlockedUserList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/list_blocked", "List the organization's blocked users")]
    public Task<BlockedUser[]> ListBlockedUsersAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/list_blocked".WithQuery(paging), cancelToken).JsonResponseAsync<BlockedUser[]>(cancelToken);

    /// <summary>Blocks a user from the organization</summary>
    /// <param name="org">name of the org</param>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/orgs/{org}/block/{username}", "Blocks a user from the organization")]
    public Task BlockUserFromAsync(string org, string username, CancellationToken cancelToken = default)
        => PutRequest($"orgs/{org}/block/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Unblock a user from the organization</summary>
    /// <param name="org">name of the org</param>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/orgs/{org}/unblock/{username}", "Unblock a user from the organization")]
    public Task UnblockUserFromAsync(string org, string username, CancellationToken cancelToken = default)
        => PutRequest($"orgs/{org}/unblock/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Webhook
    /// <summary>List an organization&apos;s webhooks</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>HookList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/hooks", "List an organization's webhooks")]
    public Task<Hook[]> ListWebhooksAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/hooks".WithQuery(paging), cancelToken).JsonResponseAsync<Hook[]>(cancelToken);

    /// <summary>Get a hook</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="id">id of the hook to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/hooks/{id}", "Get a hook")]
    public Task<Hook> GetWebhookAsync(string org, long id, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/hooks/{id}", cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Create a hook</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("POST", "/orgs/{org}/hooks", "Create a hook")]
    public Task<Hook> CreateWebhookAsync(string org, CreateHookOption options, CancellationToken cancelToken = default)
        => PostRequest($"orgs/{org}/hooks", options, cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Update a hook</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="id">id of the hook to update</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("PATCH", "/orgs/{org}/hooks/{id}", "Update a hook")]
    public Task<Hook> UpdateWebhookAsync(string org, long id, EditHookOption options, CancellationToken cancelToken = default)
        => PatchRequest($"orgs/{org}/hooks/{id}", options, cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Delete a hook</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="id">id of the hook to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/orgs/{org}/hooks/{id}", "Delete a hook")]
    public Task DeleteWebhookAsync(string org, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"orgs/{org}/hooks/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Lable
    /// <summary>List an organization&apos;s labels</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LabelList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/labels", "List an organization's labels")]
    public Task<Label[]> ListLabelsAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/labels".WithQuery(paging), cancelToken).JsonResponseAsync<Label[]>(cancelToken);

    /// <summary>Get a single label</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="id">id of the label to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Label</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/labels/{id}", "Get a single label")]
    public Task<Label> GetLabelAsync(string org, long id, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/labels/{id}", cancelToken).JsonResponseAsync<Label>(cancelToken);

    /// <summary>Create a label for an organization</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Label</returns>
    [ForgejoEndpoint("POST", "/orgs/{org}/labels", "Create a label for an organization")]
    public Task<Label> CreateLabelAsync(string org, CreateLabelOption options, CancellationToken cancelToken = default)
        => PostRequest($"orgs/{org}/labels", options, cancelToken).JsonResponseAsync<Label>(cancelToken);

    /// <summary>Update a label</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="id">id of the label to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Label</returns>
    [ForgejoEndpoint("PATCH", "/orgs/{org}/labels/{id}", "Update a label")]
    public Task<Label> UpdateLabelAsync(string org, long id, EditLabelOption options, CancellationToken cancelToken = default)
        => PatchRequest($"orgs/{org}/labels/{id}", options, cancelToken).JsonResponseAsync<Label>(cancelToken);

    /// <summary>Delete a label</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="id">id of the label to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/orgs/{org}/labels/{id}", "Delete a label")]
    public Task DeleteLabelAsync(string org, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"orgs/{org}/labels/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Team
    /// <summary>List an organization&apos;s teams</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TeamList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/teams", "List an organization's teams")]
    public Task<Team[]> ListTeamsAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/teams".WithQuery(paging), cancelToken).JsonResponseAsync<Team[]>(cancelToken);

    /// <summary>Search for teams within an organization</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="q">keywords to search</param>
    /// <param name="include_desc">include search within team description (defaults to true)</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>SearchResults of a successful search</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/teams/search", "Search for teams within an organization")]
    [ManualEdit("結果値に独自定義型を利用")]
    public Task<TeamSearchResults> SearchTeamsAsync(string org, string? q = default, bool? include_desc = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/teams/search".WithQuery(q).Param(include_desc).Param(paging), cancelToken).JsonResponseAsync<TeamSearchResults>(cancelToken);

    /// <summary>Get a team</summary>
    /// <param name="id">id of the team to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Team</returns>
    [ForgejoEndpoint("GET", "/teams/{id}", "Get a team")]
    public Task<Team> GetTeamAsync(long id, CancellationToken cancelToken = default)
        => GetRequest($"teams/{id}", cancelToken).JsonResponseAsync<Team>(cancelToken);

    /// <summary>Create a team</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Team</returns>
    [ForgejoEndpoint("POST", "/orgs/{org}/teams", "Create a team")]
    public Task<Team> CreateTeamAsync(string org, CreateTeamOption options, CancellationToken cancelToken = default)
        => PostRequest($"orgs/{org}/teams", options, cancelToken).JsonResponseAsync<Team>(cancelToken);

    /// <summary>Edit a team</summary>
    /// <param name="id">id of the team to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Team</returns>
    [ForgejoEndpoint("PATCH", "/teams/{id}", "Edit a team")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<Team> UpdateTeamAsync(long id, EditTeamOption options, CancellationToken cancelToken = default)
        => PatchRequest($"teams/{id}", options, cancelToken).JsonResponseAsync<Team>(cancelToken);

    /// <summary>Delete a team</summary>
    /// <param name="id">id of the team to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/teams/{id}", "Delete a team")]
    public Task DeleteTeamAsync(long id, CancellationToken cancelToken = default)
        => DeleteRequest($"teams/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Team Member
    /// <summary>List a team&apos;s members</summary>
    /// <param name="id">id of the team</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/teams/{id}/members", "List a team's members")]
    public Task<User[]> ListTeamMembersAsync(long id, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"teams/{id}/members".WithQuery(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>List a particular member of team</summary>
    /// <param name="id">id of the team</param>
    /// <param name="username">username of the member to list</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>User</returns>
    [ForgejoEndpoint("GET", "/teams/{id}/members/{username}", "List a particular member of team")]
    public Task<User> GetTeamMemberAsync(long id, string username, CancellationToken cancelToken = default)
        => GetRequest($"teams/{id}/members/{username}", cancelToken).JsonResponseAsync<User>(cancelToken);

    /// <summary>Add a team member</summary>
    /// <param name="id">id of the team</param>
    /// <param name="username">username of the user to add</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/teams/{id}/members/{username}", "Add a team member")]
    public Task AddTeamMemberAsync(long id, string username, CancellationToken cancelToken = default)
        => PutRequest($"teams/{id}/members/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Remove a team member</summary>
    /// <param name="id">id of the team</param>
    /// <param name="username">username of the user to remove</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/teams/{id}/members/{username}", "Remove a team member")]
    public Task RemoveTeamMemberAsync(long id, string username, CancellationToken cancelToken = default)
        => DeleteRequest($"teams/{id}/members/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Team Repository
    /// <summary>List a team&apos;s repos</summary>
    /// <param name="id">id of the team</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/teams/{id}/repos", "List a team's repos")]
    public Task<Repository[]> ListTeamRepositoriesAsync(long id, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"teams/{id}/repos".WithQuery(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);

    /// <summary>List a particular repo of team</summary>
    /// <param name="id">id of the team</param>
    /// <param name="org">organization that owns the repo to list</param>
    /// <param name="repo">name of the repo to list</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("GET", "/teams/{id}/repos/{org}/{repo}", "List a particular repo of team")]
    public Task<Repository> GetTeamRepositoryAsync(long id, string org, string repo, CancellationToken cancelToken = default)
        => GetRequest($"teams/{id}/repos/{org}/{repo}", cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Add a repository to a team</summary>
    /// <param name="id">id of the team</param>
    /// <param name="org">organization that owns the repo to add</param>
    /// <param name="repo">name of the repo to add</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/teams/{id}/repos/{org}/{repo}", "Add a repository to a team")]
    public Task AddTeamRepositoryAsync(long id, string org, string repo, CancellationToken cancelToken = default)
        => PutRequest($"teams/{id}/repos/{org}/{repo}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Remove a repository from a team</summary>
    /// <param name="id">id of the team</param>
    /// <param name="org">organization that owns the repo to remove</param>
    /// <param name="repo">name of the repo to remove</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/teams/{id}/repos/{org}/{repo}", "Remove a repository from a team")]
    public Task RemoveTeamRepositoryAsync(long id, string org, string repo, CancellationToken cancelToken = default)
        => DeleteRequest($"teams/{id}/repos/{org}/{repo}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Team Misc
    /// <summary>List a team&apos;s activity feeds</summary>
    /// <param name="id">id of the team</param>
    /// <param name="date">the date of the activities to be found</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActivityFeedsList</returns>
    [ForgejoEndpoint("GET", "/teams/{id}/activities/feeds", "List a team's activity feeds")]
    public Task<Activity[]> ListTeamActivitiesAsync(long id, DateTimeOffset? date = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"teams/{id}/activities/feeds".WithQuery(date).Param(paging), cancelToken).JsonResponseAsync<Activity[]>(cancelToken);

    /// <summary>Get user permissions in organization</summary>
    /// <param name="username">username of user</param>
    /// <param name="org">name of the organization</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OrganizationPermissions</returns>
    [ForgejoEndpoint("GET", "/users/{username}/orgs/{org}/permissions", "Get user permissions in organization")]
    public Task<OrganizationPermissions> GetUserPermissionsAsync(string username, string org, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/orgs/{org}/permissions", cancelToken).JsonResponseAsync<OrganizationPermissions>(cancelToken);
    #endregion

    #region Quota
    /// <summary>List the artifacts affecting the organization&apos;s quota</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaUsedArtifactList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/quota/artifacts", "List the artifacts affecting the organization's quota")]
    public Task<QuotaUsedArtifact[]> ListQuotaArtifactsAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/quota/artifacts".WithQuery(paging), cancelToken).JsonResponseAsync<QuotaUsedArtifact[]>(cancelToken);

    /// <summary>List the attachments affecting the organization&apos;s quota</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaUsedAttachmentList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/quota/attachments", "List the attachments affecting the organization's quota")]
    public Task<QuotaUsedAttachment[]> ListQuotaAttachmentsAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/quota/attachments".WithQuery(paging), cancelToken).JsonResponseAsync<QuotaUsedAttachment[]>(cancelToken);

    /// <summary>List the packages affecting the organization&apos;s quota</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaUsedPackageList</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/quota/packages", "List the packages affecting the organization's quota")]
    public Task<QuotaUsedPackage[]> ListQuotaPackagesAsync(string org, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/quota/packages".WithQuery(paging), cancelToken).JsonResponseAsync<QuotaUsedPackage[]>(cancelToken);

    /// <summary>Get quota information for an organization</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>QuotaInfo</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/quota", "Get quota information for an organization")]
    public Task<QuotaInfo> GetQuotaAsync(string org, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/quota", cancelToken).JsonResponseAsync<QuotaInfo>(cancelToken);

    /// <summary>Check if the organization is over quota for a given subject</summary>
    /// <param name="org">name of the organization</param>
    /// <param name="subject">quota limit subject</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>quota compliance</returns>
    [ForgejoEndpoint("GET", "/orgs/{org}/quota/check", "Check if the organization is over quota for a given subject")]
    [ManualEdit("Swagger定義にsubjectが無かったので追加")]
    [ManualEdit("このAPIはbodyでbooleanを返すように見受けられるのでそれに合わせた戻り値定義")]
    public Task<bool> CheckQuotaOverAsync(string org, string subject, CancellationToken cancelToken = default)
        => GetRequest($"orgs/{org}/quota/check".WithQuery(subject), cancelToken).JsonResponseAsync<bool>(cancelToken);
    #endregion

}
