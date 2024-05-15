namespace ForgejoApiClient.Api.Scopes;

/// <summary>notification スコープのAPIインタフェース</summary>
public interface INotificationApi : IApiScope
{
    /// <summary>List users&apos;s notification threads</summary>
    /// <param name="all">If true, show notifications marked as read. Default value is false</param>
    /// <param name="status_types">Show notifications with the provided status types. Options are: unread, read and/or pinned. Defaults to unread &amp; pinned.</param>
    /// <param name="subject_type">filter notifications by subject type</param>
    /// <param name="since">Only show notifications updated after the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="before">Only show notifications updated before the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>NotificationThreadList</returns>
    [ForgejoEndpoint("GET", "/notifications", "List users's notification threads")]
    public Task<NotificationThread[]> ListAsync(bool? all = default, ICollection<string>? status_types = default, string[]? subject_type = default, DateTimeOffset? since = default, DateTimeOffset? before = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("notifications".WithQuery(all).Param(status_types).Param(subject_type).Param(since).Param(before).Param(paging), cancelToken).JsonResponseAsync<NotificationThread[]>(cancelToken);

    /// <summary>Check if unread notifications exist</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Number of unread notifications</returns>
    [ForgejoEndpoint("GET", "/notifications/new", "Check if unread notifications exist")]
    public Task<NotificationCount> CheckNewAsync(CancellationToken cancelToken = default)
        => GetRequest("notifications/new", cancelToken).JsonResponseAsync<NotificationCount>(cancelToken);

    /// <summary>Mark notification threads as read, pinned or unread</summary>
    /// <param name="last_read_at">Describes the last point that notifications were checked. Anything updated since this time will not be updated.</param>
    /// <param name="all">If true, mark all notifications on this repo. Default value is false</param>
    /// <param name="status_types">Mark notifications with the provided status types. Options are: unread, read and/or pinned. Defaults to unread.</param>
    /// <param name="to_status">Status to mark notifications as, Defaults to read.</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>NotificationThreadList</returns>
    [ForgejoEndpoint("PUT", "/notifications", "Mark notification threads as read, pinned or unread")]
    [ManualEdit("all パラメータの型を変更")]
    public Task<NotificationThread[]> MarkAsync(DateTimeOffset? last_read_at = default, bool? all = default, ICollection<string>? status_types = default, string? to_status = default, CancellationToken cancelToken = default)
        => PutRequest("notifications".WithQuery(last_read_at).Param(all).Param(status_types).Param(to_status), cancelToken).JsonResponseAsync<NotificationThread[]>(cancelToken);

    /// <summary>Get notification thread by ID</summary>
    /// <param name="id">id of notification thread</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>NotificationThread</returns>
    [ForgejoEndpoint("GET", "/notifications/threads/{id}", "Get notification thread by ID")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<NotificationThread> GetThreadAsync(long id, CancellationToken cancelToken = default)
        => GetRequest($"notifications/threads/{id}", cancelToken).JsonResponseAsync<NotificationThread>(cancelToken);

    /// <summary>Mark notification thread as read by ID</summary>
    /// <param name="id">id of notification thread</param>
    /// <param name="to_status">Status to mark notifications as</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>NotificationThread</returns>
    [ForgejoEndpoint("PATCH", "/notifications/threads/{id}", "Mark notification thread as read by ID")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<NotificationThread> MarkThreadAsync(long id, string? to_status = default, CancellationToken cancelToken = default)
        => PatchRequest($"notifications/threads/{id}".WithQuery(to_status), cancelToken).JsonResponseAsync<NotificationThread>(cancelToken);

    /// <summary>List users&apos;s notification threads on a specific repo</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="all">If true, show notifications marked as read. Default value is false</param>
    /// <param name="status_types">Show notifications with the provided status types. Options are: unread, read and/or pinned. Defaults to unread &amp; pinned</param>
    /// <param name="subject_type">filter notifications by subject type</param>
    /// <param name="since">Only show notifications updated after the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="before">Only show notifications updated before the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>NotificationThreadList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/notifications", "List users's notification threads on a specific repo")]
    public Task<NotificationThread[]> ListRepositoryThreadsAsync(string owner, string repo, bool? all = default, ICollection<string>? status_types = default, string[]? subject_type = default, DateTimeOffset? since = default, DateTimeOffset? before = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/notifications".WithQuery(all).Param(status_types).Param(subject_type).Param(since).Param(before).Param(paging), cancelToken).JsonResponseAsync<NotificationThread[]>(cancelToken);

    /// <summary>Mark notification threads as read, pinned or unread on a specific repo</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="all">If true, mark all notifications on this repo. Default value is false</param>
    /// <param name="status_types">Mark notifications with the provided status types. Options are: unread, read and/or pinned. Defaults to unread.</param>
    /// <param name="to_status">Status to mark notifications as. Defaults to read.</param>
    /// <param name="last_read_at">Describes the last point that notifications were checked. Anything updated since this time will not be updated.</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>NotificationThreadList</returns>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/notifications", "Mark notification threads as read, pinned or unread on a specific repo")]
    [ManualEdit("all パラメータの型を変更")]
    public Task<NotificationThread[]> MarkRepositoryThreadsAsync(string owner, string repo, bool? all = default, ICollection<string>? status_types = default, string? to_status = default, DateTimeOffset? last_read_at = default, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/notifications".WithQuery(all).Param(status_types).Param(to_status).Param(last_read_at), cancelToken).JsonResponseAsync<NotificationThread[]>(cancelToken);

}
