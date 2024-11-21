namespace ForgejoApiClient.Api.Scopes;

/// <summary>issue スコープのAPIインタフェース</summary>
public interface IIssueApi : IApiScope
{
    #region Issue
    /// <summary>Search for issues across the repositories that the user has access to</summary>
    /// <param name="state">whether issue is open or closed</param>
    /// <param name="labels">comma separated list of labels. Fetch only issues that have any of this labels. Non existent labels are discarded</param>
    /// <param name="milestones">comma separated list of milestone names. Fetch only issues that have any of this milestones. Non existent are discarded</param>
    /// <param name="q">search string</param>
    /// <param name="priority_repo_id">repository to prioritize in the results</param>
    /// <param name="type">filter by type (issues / pulls) if set</param>
    /// <param name="since">Only show notifications updated after the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="before">Only show notifications updated before the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="assigned">filter (issues / pulls) assigned to you, default is false</param>
    /// <param name="created">filter (issues / pulls) created by you, default is false</param>
    /// <param name="mentioned">filter (issues / pulls) mentioning you, default is false</param>
    /// <param name="review_requested">filter pulls requesting your review, default is false</param>
    /// <param name="reviewed">filter pulls reviewed by you, default is false</param>
    /// <param name="owner">filter by owner</param>
    /// <param name="team">filter by team (requires organization owner parameter to be provided)</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>IssueList</returns>
    [ForgejoEndpoint("GET", "/repos/issues/search", "Search for issues across the repositories that the user has access to")]
    public Task<Issue[]> SearchAsync(string? state = default, string? labels = default, string? milestones = default, string? q = default, long? priority_repo_id = default, string? type = default, DateTimeOffset? since = default, DateTimeOffset? before = default, bool? assigned = default, bool? created = default, bool? mentioned = default, bool? review_requested = default, bool? reviewed = default, string? owner = default, string? team = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("repos/issues/search".WithQuery(state).Param(labels).Param(milestones).Param(q).Param(priority_repo_id).Param(type).Param(since).Param(before).Param(assigned).Param(created).Param(mentioned).Param(review_requested).Param(reviewed).Param(owner).Param(team).Param(paging), cancelToken).JsonResponseAsync<Issue[]>(cancelToken);

    /// <summary>List a repository&apos;s issues</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="state">whether issue is open or closed</param>
    /// <param name="labels">comma separated list of labels. Fetch only issues that have any of this labels. Non existent labels are discarded</param>
    /// <param name="q">search string</param>
    /// <param name="type">filter by type (issues / pulls) if set</param>
    /// <param name="milestones">comma separated list of milestone names or ids. It uses names and fall back to ids. Fetch only issues that have any of this milestones. Non existent milestones are discarded</param>
    /// <param name="since">Only show items updated after the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="before">Only show items updated before the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="created_by">Only show items which were created by the given user</param>
    /// <param name="assigned_by">Only show items for which the given user is assigned</param>
    /// <param name="mentioned_by">Only show items in which the given user was mentioned</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>IssueList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues", "List a repository's issues")]
    public Task<Issue[]> ListAsync(string owner, string repo, string? state = default, string? labels = default, string? q = default, string? type = default, string? milestones = default, DateTimeOffset? since = default, DateTimeOffset? before = default, string? created_by = default, string? assigned_by = default, string? mentioned_by = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues".WithQuery(state).Param(labels).Param(q).Param(type).Param(milestones).Param(since).Param(before).Param(created_by).Param(assigned_by).Param(mentioned_by).Param(paging), cancelToken).JsonResponseAsync<Issue[]>(cancelToken);

    /// <summary>Get an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Issue</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}", "Get an issue")]
    public Task<Issue> GetAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}", cancelToken).JsonResponseAsync<Issue>(cancelToken);

    /// <summary>Create an issue. If using deadline only the date will be taken into account, and time of day ignored.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Issue</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues", "Create an issue. If using deadline only the date will be taken into account, and time of day ignored.")]
    public Task<Issue> CreateAsync(string owner, string repo, CreateIssueOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues", options, cancelToken).JsonResponseAsync<Issue>(cancelToken);

    /// <summary>Edit an issue. If using deadline only the date will be taken into account, and time of day ignored.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Issue</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/issues/{index}", "Edit an issue. If using deadline only the date will be taken into account, and time of day ignored.")]
    public Task<Issue> UpdateAsync(string owner, string repo, long index, EditIssueOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/issues/{index}", options, cancelToken).JsonResponseAsync<Issue>(cancelToken);

    /// <summary>Delete an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of issue to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}", "Delete an issue")]
    public Task DeleteAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Issue Attachment
    /// <summary>List issue&apos;s attachments</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>AttachmentList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/assets", "List issue's attachments")]
    public Task<Attachment[]> ListAttachmentsAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/assets", cancelToken).JsonResponseAsync<Attachment[]>(cancelToken);

    /// <summary>Get an issue attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="attachment_id">id of the attachment to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}", "Get an issue attachment")]
    public Task<Attachment> GetAttachmentAsync(string owner, string repo, long index, long attachment_id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}", cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Create an issue attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="attachment">attachment to upload</param>
    /// <param name="name">name of the attachment</param>
    /// <param name="updated_at">time of the attachment&apos;s creation. This is a timestamp in RFC 3339 format</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/assets", "Create an issue attachment")]
    public Task<Attachment> CreateAttachmentAsync(string owner, string repo, long index, Stream attachment, string? name = default, DateTimeOffset? updated_at = default, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/assets".WithQuery(name).Param(updated_at), new FormData(attachment), cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Edit an issue attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="attachment_id">id of the attachment to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}", "Edit an issue attachment")]
    public Task<Attachment> UpdateAttachmentAsync(string owner, string repo, long index, long attachment_id, EditAttachmentOptions options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}", options, cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Delete an issue attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="attachment_id">id of the attachment to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}", "Delete an issue attachment")]
    public Task DeleteAttachmentAsync(string owner, string repo, long index, long attachment_id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Issue Reaction
    /// <summary>Get a list reactions of an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ReactionList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/reactions", "Get a list reactions of an issue")]
    public Task<Reaction[]> ListReactionsAsync(string owner, string repo, long index, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/reactions".WithQuery(paging), cancelToken).JsonResponseAsync<Reaction[]>(cancelToken);

    /// <summary>Add a reaction to an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Reaction</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/reactions", "Add a reaction to an issue")]
    public Task<Reaction> AddReactionAsync(string owner, string repo, long index, EditReactionOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/reactions", options, cancelToken).JsonResponseAsync<Reaction>(cancelToken);

    /// <summary>Remove a reaction from an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/reactions", "Remove a reaction from an issue")]
    public Task RemoveReactionAsync(string owner, string repo, long index, EditReactionOption options, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/reactions", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Comment
    /// <summary>List all comments in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="since">if provided, only comments updated since the provided time are returned.</param>
    /// <param name="before">if provided, only comments updated before the provided time are returned.</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CommentList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/comments", "List all comments in a repository")]
    public Task<Comment[]> ListRepositoryCommentsAsync(string owner, string repo, DateTimeOffset? since = default, DateTimeOffset? before = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/comments".WithQuery(since).Param(before).Param(paging), cancelToken).JsonResponseAsync<Comment[]>(cancelToken);

    /// <summary>List all comments on an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="since">if provided, only comments updated since the specified time are returned.</param>
    /// <param name="before">if provided, only comments updated before the provided time are returned.</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CommentList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/comments", "List all comments on an issue")]
    public Task<Comment[]> ListIssueCommentsAsync(string owner, string repo, long index, DateTimeOffset? since = default, DateTimeOffset? before = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/comments".WithQuery(since).Param(before), cancelToken).JsonResponseAsync<Comment[]>(cancelToken);

    /// <summary>Get a comment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Comment</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/comments/{id}", "Get a comment")]
    public Task<Comment> GetCommentAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/comments/{id}", cancelToken).JsonResponseAsync<Comment>(cancelToken);

    /// <summary>Add a comment to an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Comment</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/comments", "Add a comment to an issue")]
    public Task<Comment> CreateCommentAsync(string owner, string repo, long index, CreateIssueCommentOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/comments", options, cancelToken).JsonResponseAsync<Comment>(cancelToken);

    /// <summary>Edit a comment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Comment</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/issues/comments/{id}", "Edit a comment")]
    public Task<Comment> UpdateCommentAsync(string owner, string repo, long id, EditIssueCommentOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/issues/comments/{id}", options, cancelToken).JsonResponseAsync<Comment>(cancelToken);

    /// <summary>Delete a comment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of comment to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/comments/{id}", "Delete a comment")]
    public Task DeleteCommentAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/comments/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Comment Attachment
    /// <summary>List comment&apos;s attachments</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>AttachmentList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/comments/{id}/assets", "List comment's attachments")]
    public Task<Attachment[]> ListCommentAttachmentsAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/comments/{id}/assets", cancelToken).JsonResponseAsync<Attachment[]>(cancelToken);

    /// <summary>Get a comment attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment</param>
    /// <param name="attachment_id">id of the attachment to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}", "Get a comment attachment")]
    public Task<Attachment> GetCommentAttachmentAsync(string owner, string repo, long id, long attachment_id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}", cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Create a comment attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment</param>
    /// <param name="attachment">attachment to upload</param>
    /// <param name="name">name of the attachment</param>
    /// <param name="updated_at">time of the attachment&apos;s creation. This is a timestamp in RFC 3339 format</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/comments/{id}/assets", "Create a comment attachment")]
    public Task<Attachment> CreateCommentAttachmentAsync(string owner, string repo, long id, Stream attachment, string? name = default, DateTimeOffset? updated_at = default, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/comments/{id}/assets".WithQuery(name).Param(updated_at), new FormData(attachment), cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Edit a comment attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment</param>
    /// <param name="attachment_id">id of the attachment to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}", "Edit a comment attachment")]
    public Task<Attachment> UpdateCommentAttachmentAsync(string owner, string repo, long id, long attachment_id, EditAttachmentOptions options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}", options, cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Delete a comment attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment</param>
    /// <param name="attachment_id">id of the attachment to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}", "Delete a comment attachment")]
    public Task DeleteCommentAttachmentAsync(string owner, string repo, long id, long attachment_id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Comment Reaction
    /// <summary>Get a list of reactions from a comment of an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment to edit</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ReactionList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/comments/{id}/reactions", "Get a list of reactions from a comment of an issue")]
    public Task<Reaction[]> ListCommentReactionsAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/comments/{id}/reactions", cancelToken).JsonResponseAsync<Reaction[]>(cancelToken);

    /// <summary>Add a reaction to a comment of an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Reaction</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/comments/{id}/reactions", "Add a reaction to a comment of an issue")]
    public Task<Reaction> AddCommentReactionAsync(string owner, string repo, long id, EditReactionOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/comments/{id}/reactions", options, cancelToken).JsonResponseAsync<Reaction>(cancelToken);

    /// <summary>Remove a reaction from a comment of an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the comment to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/comments/{id}/reactions", "Remove a reaction from a comment of an issue")]
    public Task RemoveCommentReactionAsync(string owner, string repo, long id, EditReactionOption options, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/comments/{id}/reactions", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Block
    /// <summary>List issues that are blocked by this issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>IssueList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/blocks", "List issues that are blocked by this issue")]
    [ManualEdit("index パラメータの型を変更")]
    public Task<Issue[]> ListBlockedAsync(string owner, string repo, long index, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/blocks".WithQuery(paging), cancelToken).JsonResponseAsync<Issue[]>(cancelToken);

    /// <summary>Block the issue given in the body by the issue in path</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Issue</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/blocks", "Block the issue given in the body by the issue in path")]
    [ManualEdit("index パラメータの型を変更")]
    public Task<Issue> BlockAsync(string owner, string repo, long index, IssueMeta options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/blocks", options, cancelToken).JsonResponseAsync<Issue>(cancelToken);

    /// <summary>Unblock the issue given in the body by the issue in path</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Issue</returns>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/blocks", "Unblock the issue given in the body by the issue in path")]
    [ManualEdit("index パラメータの型を変更")]
    public Task<Issue> UnblockAsync(string owner, string repo, long index, IssueMeta options, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/blocks", options, cancelToken).JsonResponseAsync<Issue>(cancelToken);
    #endregion

    #region Dependency
    /// <summary>List an issue&apos;s dependencies, i.e all issues that block this issue.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>IssueList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/dependencies", "List an issue's dependencies, i.e all issues that block this issue.")]
    [ManualEdit("index パラメータの型を変更")]
    public Task<Issue[]> ListDependenciesAsync(string owner, string repo, long index, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/dependencies".WithQuery(paging), cancelToken).JsonResponseAsync<Issue[]>(cancelToken);

    /// <summary>Make the issue in the url depend on the issue in the form.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Issue</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/dependencies", "Make the issue in the url depend on the issue in the form.")]
    [ManualEdit("index パラメータの型を変更")]
    public Task<Issue> MakeDependencyAsync(string owner, string repo, long index, IssueMeta options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/dependencies", options, cancelToken).JsonResponseAsync<Issue>(cancelToken);

    /// <summary>Remove an issue dependency</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Issue</returns>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/dependencies", "Remove an issue dependency")]
    [ManualEdit("index パラメータの型を変更")]
    public Task<Issue> RemoveDependencyAsync(string owner, string repo, long index, IssueMeta options, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/dependencies", options, cancelToken).JsonResponseAsync<Issue>(cancelToken);
    #endregion

    #region Deadline
    /// <summary>Set an issue deadline. If set to null, the deadline is deleted. If using deadline only the date will be taken into account, and time of day ignored.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue to create or update a deadline on</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>IssueDeadline</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/deadline", "Set an issue deadline. If set to null, the deadline is deleted. If using deadline only the date will be taken into account, and time of day ignored.")]
    public Task<IssueDeadline> SetDeadlineAsync(string owner, string repo, long index, EditDeadlineOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/deadline", options, cancelToken).JsonResponseAsync<IssueDeadline>(cancelToken);
    #endregion

    #region Repository Label
    /// <summary>Get all of a repository&apos;s labels</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LabelList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/labels", "Get all of a repository's labels")]
    public Task<Label[]> ListRepositoryLabelsAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/labels".WithQuery(paging), cancelToken).JsonResponseAsync<Label[]>(cancelToken);

    /// <summary>Get a single label</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the label to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Label</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/labels/{id}", "Get a single label")]
    public Task<Label> GetRepositoryLabelAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/labels/{id}", cancelToken).JsonResponseAsync<Label>(cancelToken);

    /// <summary>Create a label</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Label</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/labels", "Create a label")]
    public Task<Label> CreateRepositoryLabelAsync(string owner, string repo, CreateLabelOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/labels", options, cancelToken).JsonResponseAsync<Label>(cancelToken);

    /// <summary>Update a label</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the label to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Label</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/labels/{id}", "Update a label")]
    public Task<Label> UpdateRepositoryLabelAsync(string owner, string repo, long id, EditLabelOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/labels/{id}", options, cancelToken).JsonResponseAsync<Label>(cancelToken);

    /// <summary>Delete a label</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the label to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/labels/{id}", "Delete a label")]
    public Task DeleteRepositoryLabelAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/labels/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Issue Label
    /// <summary>Get an issue&apos;s labels</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LabelList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/labels", "Get an issue's labels")]
    public Task<Label[]> ListIssueLabelsAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/labels", cancelToken).JsonResponseAsync<Label[]>(cancelToken);

    /// <summary>Add a label to an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LabelList</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/labels", "Add a label to an issue")]
    public Task<Label[]> AddIssueLabelAsync(string owner, string repo, long index, IssueLabelsOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/labels", options, cancelToken).JsonResponseAsync<Label[]>(cancelToken);

    /// <summary>Replace an issue&apos;s labels</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LabelList</returns>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/issues/{index}/labels", "Replace an issue's labels")]
    public Task<Label[]> ReplaceIssueLabelsAsync(string owner, string repo, long index, IssueLabelsOption options, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/issues/{index}/labels", options, cancelToken).JsonResponseAsync<Label[]>(cancelToken);

    /// <summary>Remove a label from an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="id">id of the label to remove</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/labels/{id}", "Remove a label from an issue")]
    public Task RemoveIssueLabelAsync(string owner, string repo, long index, long id, DeleteLabelsOption options, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/labels/{id}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Remove all labels from an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/labels", "Remove all labels from an issue")]
    public Task ClearIssueLabelsAsync(string owner, string repo, long index, DeleteLabelsOption options, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/labels", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Issue Pin
    /// <summary>Pin an Issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of issue to pin</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/pin", "Pin an Issue")]
    public Task PinAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/pin", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Unpin an Issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of issue to unpin</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/pin", "Unpin an Issue")]
    public Task UnpinAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/pin", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Moves the Pin to the given Position</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of issue</param>
    /// <param name="position">the new position</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/issues/{index}/pin/{position}", "Moves the Pin to the given Position")]
    public Task MovePinAsync(string owner, string repo, long index, long position, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/issues/{index}/pin/{position}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Stopwatch
    /// <summary>Start stopwatch on an issue.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue to create the stopwatch on</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/stopwatch/start", "Start stopwatch on an issue.")]
    public Task StartStopwatchAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/stopwatch/start", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Stop an issue&apos;s existing stopwatch.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue to stop the stopwatch on</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/stopwatch/stop", "Stop an issue's existing stopwatch.")]
    public Task StopStopwatchAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/stopwatch/stop", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete an issue&apos;s existing stopwatch.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue to stop the stopwatch on</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/stopwatch/delete", "Delete an issue's existing stopwatch.")]
    public Task DeleteStopwatchAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/stopwatch/delete", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Subscription
    /// <summary>Get users who subscribed on an issue.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/subscriptions", "Get users who subscribed on an issue.")]
    public Task<User[]> ListSubscribedUsersAsync(string owner, string repo, long index, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/subscriptions".WithQuery(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Check if user is subscribed to an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>WatchInfo</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/subscriptions/check", "Check if user is subscribed to an issue")]
    public Task<WatchInfo> IsUserSubscribedAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/subscriptions/check", cancelToken).JsonResponseAsync<WatchInfo>(cancelToken);

    /// <summary>Subscribe user to issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="user">user to subscribe</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/issues/{index}/subscriptions/{user}", "Subscribe user to issue")]
    public Task SubscribeUserAsync(string owner, string repo, long index, string user, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/issues/{index}/subscriptions/{user}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Unsubscribe user from issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="user">user witch unsubscribe</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/subscriptions/{user}", "Unsubscribe user from issue")]
    public Task UnsubscribeUserAsync(string owner, string repo, long index, string user, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/subscriptions/{user}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Timeline
    /// <summary>List all comments and events on an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="since">if provided, only comments updated since the specified time are returned.</param>
    /// <param name="before">if provided, only comments updated before the provided time are returned.</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TimelineList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/timeline", "List all comments and events on an issue")]
    public Task<TimelineComment[]> ListTimelineAsync(string owner, string repo, long index, DateTimeOffset? since = default, DateTimeOffset? before = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/timeline".WithQuery(since).Param(before).Param(paging), cancelToken).JsonResponseAsync<TimelineComment[]>(cancelToken);
    #endregion

    #region Tracked Time
    /// <summary>List an issue&apos;s tracked times</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="user">optional filter by user (available for issue managers)</param>
    /// <param name="since">Only show times updated after the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="before">Only show times updated before the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TrackedTimeList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/{index}/times", "List an issue's tracked times")]
    public Task<TrackedTime[]> ListTrackedTimesAsync(string owner, string repo, long index, string? user = default, DateTimeOffset? since = default, DateTimeOffset? before = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/{index}/times".WithQuery(user).Param(since).Param(before).Param(paging), cancelToken).JsonResponseAsync<TrackedTime[]>(cancelToken);

    /// <summary>Add tracked time to a issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TrackedTime</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/issues/{index}/times", "Add tracked time to a issue")]
    public Task<TrackedTime> AddTrackedTimeAsync(string owner, string repo, long index, AddTimeOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/issues/{index}/times", options, cancelToken).JsonResponseAsync<TrackedTime>(cancelToken);

    /// <summary>Reset a tracked time of an issue</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue to add tracked time to</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/times", "Reset a tracked time of an issue")]
    public Task ResetTrackedTimeAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/times", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete specific tracked time</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the issue</param>
    /// <param name="id">id of time to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/issues/{index}/times/{id}", "Delete specific tracked time")]
    public Task DeleteTrackedTimeAsync(string owner, string repo, long index, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/issues/{index}/times/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Milestone
    /// <summary>Get all of a repository&apos;s opened milestones</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="state">Milestone state, Recognized values are open, closed and all. Defaults to &quot;open&quot;</param>
    /// <param name="name">filter by milestone name</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>MilestoneList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/milestones", "Get all of a repository's opened milestones")]
    public Task<Milestone[]> ListMilestonesAsync(string owner, string repo, string? state = default, string? name = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/milestones".WithQuery(state).Param(name).Param(paging), cancelToken).JsonResponseAsync<Milestone[]>(cancelToken);

    /// <summary>Get a milestone</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">the milestone to get, identified by ID and if not available by name</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Milestone</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/milestones/{id}", "Get a milestone")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<Milestone> GetMilestoneAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/milestones/{id}", cancelToken).JsonResponseAsync<Milestone>(cancelToken);

    /// <summary>Create a milestone</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Milestone</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/milestones", "Create a milestone")]
    public Task<Milestone> CreateMilestoneAsync(string owner, string repo, CreateMilestoneOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/milestones", options, cancelToken).JsonResponseAsync<Milestone>(cancelToken);

    /// <summary>Update a milestone</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">the milestone to edit, identified by ID and if not available by name</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Milestone</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/milestones/{id}", "Update a milestone")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<Milestone> UpdateMilestoneAsync(string owner, string repo, long id, EditMilestoneOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/milestones/{id}", options, cancelToken).JsonResponseAsync<Milestone>(cancelToken);

    /// <summary>Delete a milestone</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">the milestone to delete, identified by ID and if not available by name</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/milestones/{id}", "Delete a milestone")]
    [ManualEdit("id パラメータの型を変更")]
    public Task DeleteMilestoneAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/milestones/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

}
