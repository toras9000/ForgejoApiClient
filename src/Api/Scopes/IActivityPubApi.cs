namespace ForgejoApiClient.Api.Scopes;

/// <summary>activitypub スコープのAPIインタフェース</summary>
public interface IActivityPubApi : IApiScope
{
    #region ActivityPub
    /// <summary>Returns the Person actor for a user</summary>
    /// <param name="user_id">user ID of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActivityPub</returns>
    [ForgejoEndpoint("GET", "/activitypub/user-id/{user-id}", "Returns the Person actor for a user")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<ActivityPub> GetUserActorAsync(long user_id, CancellationToken cancelToken = default)
        => GetRequest($"activitypub/user-id/{user_id}", cancelToken).JsonResponseAsync<ActivityPub>(cancelToken);

    /// <summary>Send to the inbox</summary>
    /// <param name="user_id">user ID of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/activitypub/user-id/{user-id}/inbox", "Send to the inbox")]
    [ManualEdit("id パラメータの型を変更")]
    public Task SendUserToInboxAsync(long user_id, CancellationToken cancelToken = default)
        => PostRequest($"activitypub/user-id/{user_id}/inbox", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Returns the Repository actor for a repo</summary>
    /// <param name="repository_id">repository ID of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActivityPub</returns>
    [ForgejoEndpoint("GET", "/activitypub/repository-id/{repository-id}", "Returns the Repository actor for a repo")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<ActivityPub> GetRepositoryActorAsync(long repository_id, CancellationToken cancelToken = default)
        => GetRequest($"activitypub/repository-id/{repository_id}", cancelToken).JsonResponseAsync<ActivityPub>(cancelToken);

    /// <summary>Send to the inbox</summary>
    /// <param name="repository_id">repository ID of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/activitypub/repository-id/{repository-id}/inbox", "Send to the inbox")]
    [ManualEdit("id パラメータの型を変更")]
    public Task SendRepositoryToInboxAsync(long repository_id, ForgeLike options, CancellationToken cancelToken = default)
        => PostRequest($"activitypub/repository-id/{repository_id}/inbox", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

}
