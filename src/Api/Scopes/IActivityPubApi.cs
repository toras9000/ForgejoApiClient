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
    public Task<ActivityPub> GetPersonActorAsync(int user_id, CancellationToken cancelToken = default)
        => GetRequest($"activitypub/user-id/{user_id}", cancelToken).JsonResponseAsync<ActivityPub>(cancelToken);

    /// <summary>Send to the inbox</summary>
    /// <param name="user_id">user ID of the user</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/activitypub/user-id/{user-id}/inbox", "Send to the inbox")]
    public Task SendToInboxAsync(int user_id, CancellationToken cancelToken = default)
        => PostRequest($"activitypub/user-id/{user_id}/inbox", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

}
