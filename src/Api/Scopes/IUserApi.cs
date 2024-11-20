namespace ForgejoApiClient.Api.Scopes;

/// <summary>user スコープのAPIインタフェース</summary>
public interface IUserApi : IApiScope
{
    #region Profile
    /// <summary>Get the authenticated user</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>User</returns>
    [ForgejoEndpoint("GET", "/user", "Get the authenticated user")]
    public Task<User> GetMeAsync(CancellationToken cancelToken = default)
        => GetRequest("user", cancelToken).JsonResponseAsync<User>(cancelToken);

    /// <summary>List the authenticated user&apos;s email addresses</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>EmailList</returns>
    [ForgejoEndpoint("GET", "/user/emails", "List the authenticated user's email addresses")]
    public Task<Email[]> ListEmailsAsync(CancellationToken cancelToken = default)
        => GetRequest("user/emails", cancelToken).JsonResponseAsync<Email[]>(cancelToken);

    /// <summary>Add email addresses</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>EmailList</returns>
    [ForgejoEndpoint("POST", "/user/emails", "Add email addresses")]
    public Task<Email[]> AddEmailAsync(CreateEmailOption options, CancellationToken cancelToken = default)
        => PostRequest("user/emails", options, cancelToken).JsonResponseAsync<Email[]>(cancelToken);

    /// <summary>Delete email addresses</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/emails", "Delete email addresses")]
    public Task DeleteEmailAsync(DeleteEmailOption options, CancellationToken cancelToken = default)
        => DeleteRequest("user/emails", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Update Avatar</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/user/avatar", "Update Avatar")]
    public Task UpdateAvatarAsync(UpdateUserAvatarOption options, CancellationToken cancelToken = default)
        => PostRequest("user/avatar", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete Avatar</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/avatar", "Delete Avatar")]
    public Task DeleteAvatarAsync(CancellationToken cancelToken = default)
        => DeleteRequest("user/avatar", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Get user settings</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserSettings</returns>
    [ForgejoEndpoint("GET", "/user/settings", "Get user settings")]
    public Task<UserSettings> GetSettingsAsync(CancellationToken cancelToken = default)
        => GetRequest("user/settings", cancelToken).JsonResponseAsync<UserSettings>(cancelToken);

    /// <summary>Update user settings</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserSettings</returns>
    [ForgejoEndpoint("PATCH", "/user/settings", "Update user settings")]
    public Task<UserSettings> UpdateSettingsAsync(UserSettingsOptions options, CancellationToken cancelToken = default)
        => PatchRequest("user/settings", options, cancelToken).JsonResponseAsync<UserSettings>(cancelToken);

    /// <summary>List all the teams a user belongs to</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TeamList</returns>
    [ForgejoEndpoint("GET", "/user/teams", "List all the teams a user belongs to")]
    public Task<Team[]> ListTeamsAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/teams".WithQuery(paging), cancelToken).JsonResponseAsync<Team[]>(cancelToken);

    /// <summary>List a user&apos;s activity feeds</summary>
    /// <param name="username">username of user</param>
    /// <param name="only_performed_by">if true, only show actions performed by the requested user</param>
    /// <param name="date">the date of the activities to be found</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActivityFeedsList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/activities/feeds", "List a user's activity feeds")]
    public Task<Activity[]> ListUserActivitiesAsync(string username, bool? only_performed_by = default, DateTimeOffset? date = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/activities/feeds".WithQuery(only_performed_by).Param(date).Param(paging), cancelToken).JsonResponseAsync<Activity[]>(cancelToken);

    /// <summary>Get a user&apos;s heatmap</summary>
    /// <param name="username">username of user to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserHeatmapData</returns>
    [ForgejoEndpoint("GET", "/users/{username}/heatmap", "Get a user's heatmap")]
    public Task<UserHeatmapData[]> ListUserHeatmapAsync(string username, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/heatmap", cancelToken).JsonResponseAsync<UserHeatmapData[]>(cancelToken);
    #endregion

    #region Social
    /// <summary>List the authenticated user&apos;s blocked users</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>BlockedUserList</returns>
    [ForgejoEndpoint("GET", "/user/list_blocked", "List the authenticated user's blocked users")]
    public Task<BlockedUser[]> ListBlockedUsersAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/list_blocked".WithQuery(paging), cancelToken).JsonResponseAsync<BlockedUser[]>(cancelToken);

    /// <summary>Blocks a user from the doer.</summary>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/user/block/{username}", "Blocks a user from the doer.")]
    public Task BlockUserAsync(string username, CancellationToken cancelToken = default)
        => PutRequest($"user/block/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Unblocks a user from the doer.</summary>
    /// <param name="username">username of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/user/unblock/{username}", "Unblocks a user from the doer.")]
    public Task UnblockUserAsync(string username, CancellationToken cancelToken = default)
        => PutRequest($"user/unblock/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List the authenticated user&apos;s followers</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/user/followers", "List the authenticated user's followers")]
    public Task<User[]> ListFollowerUsersAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/followers".WithQuery(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>List the users that the authenticated user is following</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/user/following", "List the users that the authenticated user is following")]
    public Task<User[]> ListFollowingUsersAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/following".WithQuery(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Check whether a user is followed by the authenticated user</summary>
    /// <param name="username">username of followed user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/user/following/{username}", "Check whether a user is followed by the authenticated user")]
    public Task<StatusCodeResult> CheckFollowingAsync(string username, CancellationToken cancelToken = default)
        => GetRequest($"user/following/{username}", cancelToken).StatusResponseAsync(cancelToken);

    /// <summary>Follow a user</summary>
    /// <param name="username">username of user to follow</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/user/following/{username}", "Follow a user")]
    public Task FollowUserAsync(string username, CancellationToken cancelToken = default)
        => PutRequest($"user/following/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Unfollow a user</summary>
    /// <param name="username">username of user to unfollow</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/following/{username}", "Unfollow a user")]
    public Task UnfollowUserAsync(string username, CancellationToken cancelToken = default)
        => DeleteRequest($"user/following/{username}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List the given user&apos;s followers</summary>
    /// <param name="username">username of user</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/followers", "List the given user's followers")]
    public Task<User[]> ListUserFollowersAsync(string username, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/followers".WithQuery(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>List the users that the given user is following</summary>
    /// <param name="username">username of user</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/following", "List the users that the given user is following")]
    public Task<User[]> ListUserFollowingsAsync(string username, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/following".WithQuery(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Check if one user is following another user</summary>
    /// <param name="username">username of following user</param>
    /// <param name="target">username of followed user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/users/{username}/following/{target}", "Check if one user is following another user")]
    public Task<StatusCodeResult> CheckUserFollowingAsync(string username, string target, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/following/{target}", cancelToken).StatusResponseAsync(cancelToken);

    /// <summary>The repos that the authenticated user has starred</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/user/starred", "The repos that the authenticated user has starred")]
    public Task<Repository[]> ListStarredRepositoriesAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/starred".WithQuery(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);

    /// <summary>Whether the authenticated is starring the repo</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/user/starred/{owner}/{repo}", "Whether the authenticated is starring the repo")]
    public Task<StatusCodeResult> CheckStarredAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"user/starred/{owner}/{repo}", cancelToken).StatusResponseAsync(cancelToken);

    /// <summary>Star the given repo</summary>
    /// <param name="owner">owner of the repo to star</param>
    /// <param name="repo">name of the repo to star</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/user/starred/{owner}/{repo}", "Star the given repo")]
    public Task StarRepositoryAsync(string owner, string repo, CancellationToken cancelToken = default)
        => PutRequest($"user/starred/{owner}/{repo}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Unstar the given repo</summary>
    /// <param name="owner">owner of the repo to unstar</param>
    /// <param name="repo">name of the repo to unstar</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/starred/{owner}/{repo}", "Unstar the given repo")]
    public Task UnstarRepositoryAsync(string owner, string repo, CancellationToken cancelToken = default)
        => DeleteRequest($"user/starred/{owner}/{repo}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>The repos that the given user has starred</summary>
    /// <param name="username">username of user</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/starred", "The repos that the given user has starred")]
    public Task<Repository[]> ListUserStarredAsync(string username, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/starred".WithQuery(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);
    #endregion

    #region User
    /// <summary>Search for users</summary>
    /// <param name="q">keyword</param>
    /// <param name="uid">ID of the user to search for</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>SearchResults of a successful search</returns>
    [ForgejoEndpoint("GET", "/users/search", "Search for users")]
    [ManualEdit("結果値に独自定義型を利用")]
    public Task<UserSearchResults> SearchAsync(string? q = default, long? uid = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("users/search".WithQuery(q).Param(uid).Param(paging), cancelToken).JsonResponseAsync<UserSearchResults>(cancelToken);

    /// <summary>Get a user</summary>
    /// <param name="username">username of user to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>User</returns>
    [ForgejoEndpoint("GET", "/users/{username}", "Get a user")]
    public Task<User> GetAsync(string username, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}", cancelToken).JsonResponseAsync<User>(cancelToken);
    #endregion

    #region Repository
    /// <summary>List the repos that the authenticated user owns</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/user/repos", "List the repos that the authenticated user owns")]
    public Task<Repository[]> ListRepositoriesAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/repos".WithQuery(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);

    /// <summary>Create a repository</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/user/repos", "Create a repository")]
    public Task<Repository> CreateRepositoryAsync(CreateRepoOption options, CancellationToken cancelToken = default)
        => PostRequest("user/repos", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>List the repos owned by the given user</summary>
    /// <param name="username">username of user</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/repos", "List the repos owned by the given user")]
    public Task<Repository[]> ListUserRepositoriesAsync(string username, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/repos".WithQuery(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);
    #endregion

    #region Subscription
    /// <summary>List repositories watched by the authenticated user</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/user/subscriptions", "List repositories watched by the authenticated user")]
    public Task<Repository[]> ListSubscriptionsAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/subscriptions".WithQuery(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);

    /// <summary>List the repositories watched by a user</summary>
    /// <param name="username">username of the user</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/subscriptions", "List the repositories watched by a user")]
    public Task<Repository[]> ListUserSubscriptionsAsync(string username, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/subscriptions".WithQuery(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);
    #endregion

    #region Issue
    /// <summary>Get list of all existing stopwatches</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>StopWatchList</returns>
    [ForgejoEndpoint("GET", "/user/stopwatches", "Get list of all existing stopwatches")]
    public Task<StopWatch[]> ListStopwatchesAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/stopwatches".WithQuery(paging), cancelToken).JsonResponseAsync<StopWatch[]>(cancelToken);

    /// <summary>List the current user&apos;s tracked times</summary>
    /// <param name="since">Only show times updated after the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="before">Only show times updated before the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TrackedTimeList</returns>
    [ForgejoEndpoint("GET", "/user/times", "List the current user's tracked times")]
    public Task<TrackedTime[]> ListTimesAsync(DateTimeOffset? since = default, DateTimeOffset? before = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/times".WithQuery(since).Param(before).Param(paging), cancelToken).JsonResponseAsync<TrackedTime[]>(cancelToken);
    #endregion

    #region Key
    /// <summary>Get a Token to verify</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>APIString is a string response</returns>
    [ForgejoEndpoint("GET", "/user/gpg_key_token", "Get a Token to verify")]
    public Task<string> GetGpgKeyTokenAsync(CancellationToken cancelToken = default)
        => GetRequest("user/gpg_key_token", cancelToken).TextResponseAsync(cancelToken);

    /// <summary>Verify a GPG key</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GPGKey</returns>
    [ForgejoEndpoint("POST", "/user/gpg_key_verify", "Verify a GPG key")]
    [ManualEdit("Swagger定義に options が無かったので追加")]
    public Task<GPGKey> VerifyGpgKeyTokenAsync(VerifyGPGKeyOption options, CancellationToken cancelToken = default)
        => PostRequest("user/gpg_key_verify", options, cancelToken).JsonResponseAsync<GPGKey>(cancelToken);

    /// <summary>List the authenticated user&apos;s GPG keys</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GPGKeyList</returns>
    [ForgejoEndpoint("GET", "/user/gpg_keys", "List the authenticated user's GPG keys")]
    public Task<GPGKey[]> ListGpgKeysAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/gpg_keys".WithQuery(paging), cancelToken).JsonResponseAsync<GPGKey[]>(cancelToken);

    /// <summary>Get a GPG key</summary>
    /// <param name="id">id of key to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GPGKey</returns>
    [ForgejoEndpoint("GET", "/user/gpg_keys/{id}", "Get a GPG key")]
    public Task<GPGKey> GetGpgKeyAsync(long id, CancellationToken cancelToken = default)
        => GetRequest($"user/gpg_keys/{id}", cancelToken).JsonResponseAsync<GPGKey>(cancelToken);

    /// <summary>Create a GPG key</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GPGKey</returns>
    [ForgejoEndpoint("POST", "/user/gpg_keys", "Create a GPG key")]
    public Task<GPGKey> CreateGpgKeyAsync(CreateGPGKeyOption options, CancellationToken cancelToken = default)
        => PostRequest("user/gpg_keys", options, cancelToken).JsonResponseAsync<GPGKey>(cancelToken);

    /// <summary>Remove a GPG key</summary>
    /// <param name="id">id of key to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/gpg_keys/{id}", "Remove a GPG key")]
    public Task DeleteGpgKeyAsync(long id, CancellationToken cancelToken = default)
        => DeleteRequest($"user/gpg_keys/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List the given user&apos;s GPG keys</summary>
    /// <param name="username">username of user</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GPGKeyList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/gpg_keys", "List the given user's GPG keys")]
    public Task<GPGKey[]> ListUserGpgKeysAsync(string username, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/gpg_keys".WithQuery(paging), cancelToken).JsonResponseAsync<GPGKey[]>(cancelToken);

    /// <summary>List the authenticated user&apos;s public keys</summary>
    /// <param name="fingerprint">fingerprint of the key</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PublicKeyList</returns>
    [ForgejoEndpoint("GET", "/user/keys", "List the authenticated user's public keys")]
    public Task<PublicKey[]> ListPublicKeysAsync(string? fingerprint = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/keys".WithQuery(fingerprint).Param(paging), cancelToken).JsonResponseAsync<PublicKey[]>(cancelToken);

    /// <summary>Get a public key</summary>
    /// <param name="id">id of key to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PublicKey</returns>
    [ForgejoEndpoint("GET", "/user/keys/{id}", "Get a public key")]
    public Task<PublicKey> GetPublicKeyAsync(long id, CancellationToken cancelToken = default)
        => GetRequest($"user/keys/{id}", cancelToken).JsonResponseAsync<PublicKey>(cancelToken);

    /// <summary>Create a public key</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PublicKey</returns>
    [ForgejoEndpoint("POST", "/user/keys", "Create a public key")]
    public Task<PublicKey> AddPublicKeyAsync(CreateKeyOption options, CancellationToken cancelToken = default)
        => PostRequest("user/keys", options, cancelToken).JsonResponseAsync<PublicKey>(cancelToken);

    /// <summary>Delete a public key</summary>
    /// <param name="id">id of key to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/keys/{id}", "Delete a public key")]
    public Task DeletePublicKeyAsync(long id, CancellationToken cancelToken = default)
        => DeleteRequest($"user/keys/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List the given user&apos;s public keys</summary>
    /// <param name="username">username of user</param>
    /// <param name="fingerprint">fingerprint of the key</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PublicKeyList</returns>
    [ForgejoEndpoint("GET", "/users/{username}/keys", "List the given user's public keys")]
    public Task<PublicKey[]> ListUserPublicKeysAsync(string username, string? fingerprint = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"users/{username}/keys".WithQuery(fingerprint).Param(paging), cancelToken).JsonResponseAsync<PublicKey[]>(cancelToken);
    #endregion

    #region Webhook
    /// <summary>List the authenticated user&apos;s webhooks</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>HookList</returns>
    [ForgejoEndpoint("GET", "/user/hooks", "List the authenticated user's webhooks")]
    public Task<Hook[]> ListWebhooksAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/hooks".WithQuery(paging), cancelToken).JsonResponseAsync<Hook[]>(cancelToken);

    /// <summary>Get a hook</summary>
    /// <param name="id">id of the hook to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("GET", "/user/hooks/{id}", "Get a hook")]
    public Task<Hook> GetWebhookAsync(long id, CancellationToken cancelToken = default)
        => GetRequest($"user/hooks/{id}", cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Create a hook</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("POST", "/user/hooks", "Create a hook")]
    public Task<Hook> CreateWebhookAsync(CreateHookOption options, CancellationToken cancelToken = default)
        => PostRequest("user/hooks", options, cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Update a hook</summary>
    /// <param name="id">id of the hook to update</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("PATCH", "/user/hooks/{id}", "Update a hook")]
    public Task<Hook> UpdateWebhookAsync(long id, EditHookOption options, CancellationToken cancelToken = default)
        => PatchRequest($"user/hooks/{id}", options, cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Delete a hook</summary>
    /// <param name="id">id of the hook to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/hooks/{id}", "Delete a hook")]
    public Task DeleteWebhookAsync(long id, CancellationToken cancelToken = default)
        => DeleteRequest($"user/hooks/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Action
    /// <summary>Get an user&apos;s actions runner registration token</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/user/actions/runners/registration-token", "Get an user's actions runner registration token")]
    [ManualEdit("結果値が得られるため独自型を定義して利用")]
    public Task<RegistrationTokenResult> GetActionRunnerRegistrationTokenAsync(CancellationToken cancelToken = default)
        => GetRequest("user/actions/runners/registration-token", cancelToken).JsonResponseAsync<RegistrationTokenResult>(cancelToken);

    /// <summary>Create or Update a secret value in a user scope</summary>
    /// <param name="secretname">name of the secret</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/user/actions/secrets/{secretname}", "Create or Update a secret value in a user scope")]
    public Task SetActionSecretAsync(string secretname, CreateOrUpdateSecretOption options, CancellationToken cancelToken = default)
        => PutRequest($"user/actions/secrets/{secretname}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a secret in a user scope</summary>
    /// <param name="secretname">name of the secret</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/actions/secrets/{secretname}", "Delete a secret in a user scope")]
    public Task DeleteActionSecretAsync(string secretname, CancellationToken cancelToken = default)
        => DeleteRequest($"user/actions/secrets/{secretname}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Application
    /// <summary>List the authenticated user&apos;s oauth2 applications</summary>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OAuth2ApplicationList represents a list of OAuth2 applications.</returns>
    [ForgejoEndpoint("GET", "/user/applications/oauth2", "List the authenticated user's oauth2 applications")]
    public Task<OAuth2Application[]> ListOAuth2ApplicationsAsync(PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("user/applications/oauth2".WithQuery(paging), cancelToken).JsonResponseAsync<OAuth2Application[]>(cancelToken);

    /// <summary>creates a new OAuth2 application</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OAuth2Application</returns>
    [ForgejoEndpoint("POST", "/user/applications/oauth2", "creates a new OAuth2 application")]
    public Task<OAuth2Application> CreateOAuth2ApplicationAsync(CreateOAuth2ApplicationOptions options, CancellationToken cancelToken = default)
        => PostRequest("user/applications/oauth2", options, cancelToken).JsonResponseAsync<OAuth2Application>(cancelToken);

    /// <summary>get an OAuth2 Application</summary>
    /// <param name="id">Application ID to be found</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OAuth2Application</returns>
    [ForgejoEndpoint("GET", "/user/applications/oauth2/{id}", "get an OAuth2 Application")]
    public Task<OAuth2Application> GetOAuth2ApplicationAsync(long id, CancellationToken cancelToken = default)
        => GetRequest($"user/applications/oauth2/{id}", cancelToken).JsonResponseAsync<OAuth2Application>(cancelToken);

    /// <summary>update an OAuth2 Application, this includes regenerating the client secret</summary>
    /// <param name="id">application to be updated</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>OAuth2Application</returns>
    [ForgejoEndpoint("PATCH", "/user/applications/oauth2/{id}", "update an OAuth2 Application, this includes regenerating the client secret")]
    public Task<OAuth2Application> UpdateOAuth2ApplicationAsync(long id, CreateOAuth2ApplicationOptions options, CancellationToken cancelToken = default)
        => PatchRequest($"user/applications/oauth2/{id}", options, cancelToken).JsonResponseAsync<OAuth2Application>(cancelToken);

    /// <summary>delete an OAuth2 Application</summary>
    /// <param name="id">token to be deleted</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/user/applications/oauth2/{id}", "delete an OAuth2 Application")]
    public Task DeleteOAuth2ApplicationAsync(long id, CancellationToken cancelToken = default)
        => DeleteRequest($"user/applications/oauth2/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Token
    /// <summary>List the authenticated user&apos;s access tokens</summary>
    /// <param name="username">username of user</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>AccessTokenList represents a list of API access token.</returns>
    [ForgejoEndpoint("GET", "/users/{username}/tokens", "List the authenticated user's access tokens")]
    [ManualEdit("このAPIはトークン認証では通らない。Basic認証情報を利用。")]
    public Task<AccessToken[]> ListUserApiTokensAsync(BasicAuthCredential auth, string username, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest(auth, $"users/{username}/tokens".WithQuery(paging), cancelToken).JsonResponseAsync<AccessToken[]>(cancelToken);

    /// <summary>Create an access token</summary>
    /// <param name="username">username of user</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>AccessToken represents an API access token.</returns>
    [ForgejoEndpoint("POST", "/users/{username}/tokens", "Create an access token")]
    [ManualEdit("このAPIはトークン認証では通らない。Basic認証情報を利用。")]
    public Task<AccessToken> CreateUserApiTokenAsync(BasicAuthCredential auth, string username, CreateAccessTokenOption options, CancellationToken cancelToken = default)
        => PostRequest(auth, $"users/{username}/tokens", options, cancelToken).JsonResponseAsync<AccessToken>(cancelToken);

    /// <summary>delete an access token</summary>
    /// <param name="username">username of user</param>
    /// <param name="token">token to be deleted, identified by ID and if not available by name</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/users/{username}/tokens/{token}", "delete an access token")]
    [ManualEdit("このAPIはトークン認証では通らない。Basic認証情報を利用。")]
    public Task DeleteUserApiTokenAsync(BasicAuthCredential auth, string username, string token, CancellationToken cancelToken = default)
        => DeleteRequest(auth, $"users/{username}/tokens/{token}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

}
