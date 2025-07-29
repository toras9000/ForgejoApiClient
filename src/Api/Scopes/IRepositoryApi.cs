namespace ForgejoApiClient.Api.Scopes;

/// <summary>repository スコープのAPIインタフェース</summary>
public interface IRepositoryApi : IApiScope
{
    #region Migration
    /// <summary>Migrate a remote git repository</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/repos/migrate", "Migrate a remote git repository")]
    public Task<Repository> MigrateAsync(MigrateRepoOptions options, CancellationToken cancelToken = default)
        => PostRequest("repos/migrate", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Sync a mirrored repository</summary>
    /// <param name="owner">owner of the repo to sync</param>
    /// <param name="repo">name of the repo to sync</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/mirror-sync", "Sync a mirrored repository")]
    public Task SyncMirroredAsync(string owner, string repo, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/mirror-sync", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Repository
    /// <summary>Search for repositories</summary>
    /// <param name="q">keyword</param>
    /// <param name="topic">Limit search to repositories with keyword as topic</param>
    /// <param name="includeDesc">include search of keyword within repository description</param>
    /// <param name="uid">search only for repos that the user with the given id owns or contributes to</param>
    /// <param name="priority_owner_id">repo owner to prioritize in the results</param>
    /// <param name="team_id">search only for repos that belong to the given team id</param>
    /// <param name="starredBy">search only for repos that the user with the given id has starred</param>
    /// <param name="private">include private repositories this user has access to (defaults to true)</param>
    /// <param name="is_private">show only public, private or all repositories (defaults to all)</param>
    /// <param name="template">include template repositories this user has access to (defaults to true)</param>
    /// <param name="archived">show only archived, non-archived or all repositories (defaults to all)</param>
    /// <param name="mode">type of repository to search for. Supported values are &quot;fork&quot;, &quot;source&quot;, &quot;mirror&quot; and &quot;collaborative&quot;</param>
    /// <param name="exclusive">if `uid` is given, search only for repos that the user owns</param>
    /// <param name="sort">sort repos by attribute. Supported values are &quot;alpha&quot;, &quot;created&quot;, &quot;updated&quot;, &quot;size&quot;, &quot;git_size&quot;, &quot;lfs_size&quot;, &quot;stars&quot;, &quot;forks&quot; and &quot;id&quot;. Default is &quot;alpha&quot;</param>
    /// <param name="order">sort order, either &quot;asc&quot; (ascending) or &quot;desc&quot; (descending). Default is &quot;asc&quot;, ignored if &quot;sort&quot; is not specified.</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>SearchResults</returns>
    [ForgejoEndpoint("GET", "/repos/search", "Search for repositories")]
    public Task<SearchResults> SearchAsync(string? q = default, bool? topic = default, bool? includeDesc = default, long? uid = default, long? priority_owner_id = default, long? team_id = default, long? starredBy = default, bool? @private = default, bool? is_private = default, bool? template = default, bool? archived = default, string? mode = default, bool? exclusive = default, string? sort = default, string? order = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("repos/search".WithQuery().Param(q).Param(topic).Param(includeDesc).Param(uid).Param(priority_owner_id).Param(team_id).Param(starredBy).Param(@private).Param(is_private).Param(template).Param(archived).Param(mode).Param(exclusive).Param(sort).Param(order).Param(paging), cancelToken).JsonResponseAsync<SearchResults>(cancelToken);

    /// <summary>Get a repository by id</summary>
    /// <param name="id">id of the repo to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("GET", "/repositories/{id}", "Get a repository by id")]
    public Task<Repository> GetAsync(long id, CancellationToken cancelToken = default)
        => GetRequest($"repositories/{id}", cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Get a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}", "Get a repository")]
    public Task<Repository> GetAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}", cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Create a repository</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/user/repos", "Create a repository")]
    public Task<Repository> CreateAsync(CreateRepoOption options, CancellationToken cancelToken = default)
        => PostRequest("user/repos", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Create a repository using a template</summary>
    /// <param name="template_owner">name of the template repository owner</param>
    /// <param name="template_repo">name of the template repository</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/repos/{template_owner}/{template_repo}/generate", "Create a repository using a template")]
    public Task<Repository> CreateUsingTemplateAsync(string template_owner, string template_repo, GenerateRepoOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{template_owner}/{template_repo}/generate", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Edit a repository&apos;s properties. Only fields that are set will be changed.</summary>
    /// <param name="owner">owner of the repo to edit</param>
    /// <param name="repo">name of the repo to edit</param>
    /// <param name="options">Properties of a repo that you can edit</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}", "Edit a repository's properties. Only fields that are set will be changed.")]
    public Task<Repository> UpdateAsync(string owner, string repo, EditRepoOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Delete a repository</summary>
    /// <param name="owner">owner of the repo to delete</param>
    /// <param name="repo">name of the repo to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}", "Delete a repository")]
    public Task DeleteAsync(string owner, string repo, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Branch
    /// <summary>List a repository&apos;s branches</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>BranchList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/branches", "List a repository's branches")]
    public Task<Branch[]> ListBranchesAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/branches".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Branch[]>(cancelToken);

    /// <summary>Retrieve a specific branch from a repository, including its effective branch protection</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="branch">branch to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Branch</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/branches/{branch}", "Retrieve a specific branch from a repository, including its effective branch protection")]
    public Task<Branch> GetBranchAsync(string owner, string repo, string branch, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/branches/{branch}", cancelToken).JsonResponseAsync<Branch>(cancelToken);

    /// <summary>Create a branch</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Branch</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/branches", "Create a branch")]
    public Task<Branch> CreateBranchAsync(string owner, string repo, CreateBranchRepoOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/branches", options, cancelToken).JsonResponseAsync<Branch>(cancelToken);

    /// <summary>Update a branch</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="branch">name of the branch</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/branches/{branch}", "Update a branch")]
    public Task UpdateBranchAsync(string owner, string repo, string branch, UpdateBranchRepoOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/branches/{branch}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a specific branch from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="branch">branch to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/branches/{branch}", "Delete a specific branch from a repository")]
    public Task DeleteBranchAsync(string owner, string repo, string branch, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/branches/{branch}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region BranchProtection
    /// <summary>List branch protections for a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>BranchProtectionList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/branch_protections", "List branch protections for a repository")]
    public Task<BranchProtection[]> ListBranchProtectionsAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/branch_protections", cancelToken).JsonResponseAsync<BranchProtection[]>(cancelToken);

    /// <summary>Get a specific branch protection for the repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="name">name of protected branch</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>BranchProtection</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/branch_protections/{name}", "Get a specific branch protection for the repository")]
    public Task<BranchProtection> GetBranchProtectionAsync(string owner, string repo, string name, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/branch_protections/{name}", cancelToken).JsonResponseAsync<BranchProtection>(cancelToken);

    /// <summary>Create a branch protections for a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>BranchProtection</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/branch_protections", "Create a branch protections for a repository")]
    public Task<BranchProtection> CreateBranchProtectionAsync(string owner, string repo, CreateBranchProtectionOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/branch_protections", options, cancelToken).JsonResponseAsync<BranchProtection>(cancelToken);

    /// <summary>Edit a branch protections for a repository. Only fields that are set will be changed</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="name">name of protected branch</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>BranchProtection</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/branch_protections/{name}", "Edit a branch protections for a repository. Only fields that are set will be changed")]
    public Task<BranchProtection> UpdateBranchProtectionAsync(string owner, string repo, string name, EditBranchProtectionOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/branch_protections/{name}", options, cancelToken).JsonResponseAsync<BranchProtection>(cancelToken);

    /// <summary>Delete a specific branch protection for the repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="name">name of protected branch</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/branch_protections/{name}", "Delete a specific branch protection for the repository")]
    public Task DeleteBranchProtectionAsync(string owner, string repo, string name, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/branch_protections/{name}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Tag
    /// <summary>List a repository&apos;s tags</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TagList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/tags", "List a repository's tags")]
    public Task<Tag[]> ListTagsAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/tags".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Tag[]>(cancelToken);

    /// <summary>Get the tag of a repository by tag name</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="tag">name of tag</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Tag</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/tags/{tag}", "Get the tag of a repository by tag name")]
    public Task<Tag> GetTagAsync(string owner, string repo, string tag, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/tags/{tag}", cancelToken).JsonResponseAsync<Tag>(cancelToken);

    /// <summary>Create a new git tag in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Tag</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/tags", "Create a new git tag in a repository")]
    public Task<Tag> CreateTagAsync(string owner, string repo, CreateTagOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/tags", options, cancelToken).JsonResponseAsync<Tag>(cancelToken);

    /// <summary>Delete a repository&apos;s tag by name</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="tag">name of tag to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/tags/{tag}", "Delete a repository's tag by name")]
    public Task DeleteTagAsync(string owner, string repo, string tag, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/tags/{tag}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region TagProtection
    /// <summary>List tag protections for a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TagProtectionList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/tag_protections", "List tag protections for a repository")]
    public Task<TagProtection[]> ListTagProtectionsAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/tag_protections", cancelToken).JsonResponseAsync<TagProtection[]>(cancelToken);

    /// <summary>Get a specific tag protection for the repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the tag protect to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TagProtection</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/tag_protections/{id}", "Get a specific tag protection for the repository")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<TagProtection> GetTagProtectionAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/tag_protections/{id}", cancelToken).JsonResponseAsync<TagProtection>(cancelToken);

    /// <summary>Create a tag protections for a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TagProtection</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/tag_protections", "Create a tag protections for a repository")]
    public Task<TagProtection> CreateTagProtectionAsync(string owner, string repo, CreateTagProtectionOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/tag_protections", options, cancelToken).JsonResponseAsync<TagProtection>(cancelToken);

    /// <summary>Edit a tag protections for a repository. Only fields that are set will be changed</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of protected tag</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TagProtection</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/tag_protections/{id}", "Edit a tag protections for a repository. Only fields that are set will be changed")]
    [ManualEdit("id パラメータの型を変更")]
    public Task<TagProtection> UpdateTagProtectionAsync(string owner, string repo, long id, EditTagProtectionOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/tag_protections/{id}", options, cancelToken).JsonResponseAsync<TagProtection>(cancelToken);

    /// <summary>Delete a specific tag protection for the repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of protected tag</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/tag_protections/{id}", "Delete a specific tag protection for the repository")]
    [ManualEdit("id パラメータの型を変更")]
    public Task DeleteTagProtectionAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/tag_protections/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Commit
    /// <summary>Get a list of all commits from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">SHA or branch to start listing commits from (usually &apos;master&apos;)</param>
    /// <param name="path">filepath of a file/dir</param>
    /// <param name="stat">include diff stats for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="verification">include verification for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="files">include a list of affected files for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="not">commits that match the given specifier will not be listed.</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CommitList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/commits", "Get a list of all commits from a repository")]
    public Task<Commit[]> ListCommitsAsync(string owner, string repo, string? sha = default, string? path = default, bool? stat = default, bool? verification = default, bool? files = default, string? not = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/commits".WithQuery().Param(sha).Param(path).Param(stat).Param(verification).Param(files).Param(not).Param(paging), cancelToken).JsonResponseAsync<Commit[]>(cancelToken);

    /// <summary>Get a single commit from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">a git ref or commit sha</param>
    /// <param name="stat">include diff stats for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="verification">include verification for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="files">include a list of affected files for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Commit</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/git/commits/{sha}", "Get a single commit from a repository")]
    public Task<Commit> GetCommitAsync(string owner, string repo, string sha, bool? stat = default, bool? verification = default, bool? files = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/git/commits/{sha}".WithQuery().Param(stat).Param(verification).Param(files), cancelToken).JsonResponseAsync<Commit>(cancelToken);

    /// <summary>Get a commit&apos;s diff or patch</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">SHA of the commit to get</param>
    /// <param name="diffType">whether the output is diff or patch</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>APIString is a string response</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/git/commits/{sha}.{diffType}", "Get a commit's diff or patch")]
    public Task<string> GetCommitDiffAsync(string owner, string repo, string sha, string diffType, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/git/commits/{sha}.{diffType}", cancelToken).TextResponseAsync(cancelToken);

    /// <summary>Get commit comparison information</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="basehead">compare two branches or commits</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns></returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/compare/{basehead}", "Get commit comparison information")]
    public Task<Compare> GetCommitCompareAsync(string owner, string repo, string basehead, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/compare/{basehead}", cancelToken).JsonResponseAsync<Compare>(cancelToken);

    /// <summary>Get the pull request of the commit</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">SHA of the commit to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullRequest</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/commits/{sha}/pull", "Get the pull request of the commit")]
    public Task<PullRequest> GetCommitPullRequestAsync(string owner, string repo, string sha, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/commits/{sha}/pull", cancelToken).JsonResponseAsync<PullRequest>(cancelToken);
    #endregion

    #region CommitNote
    /// <summary>Get a note corresponding to a single commit from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">a git ref or commit sha</param>
    /// <param name="verification">include verification for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="files">include a list of affected files for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Note</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/git/notes/{sha}", "Get a note corresponding to a single commit from a repository")]
    public Task<Note> GetCommitNoteAsync(string owner, string repo, string sha, bool? verification = default, bool? files = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/git/notes/{sha}".WithQuery().Param(verification).Param(files), cancelToken).JsonResponseAsync<Note>(cancelToken);

    /// <summary>Set a note corresponding to a single commit from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">a git ref or commit sha</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Note</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/git/notes/{sha}", "Set a note corresponding to a single commit from a repository")]
    public Task<Note> SetCommitNoteAsync(string owner, string repo, string sha, NoteOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/git/notes/{sha}", options, cancelToken).JsonResponseAsync<Note>(cancelToken);

    /// <summary>Removes a note corresponding to a single commit from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">a git ref or commit sha</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/git/notes/{sha}", "Removes a note corresponding to a single commit from a repository")]
    public Task DeleteCommitNoteAsync(string owner, string repo, string sha, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/git/notes/{sha}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region CommitStatus
    /// <summary>Get a commit&apos;s statuses, by branch/tag/commit reference</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="ref">name of branch/tag/commit</param>
    /// <param name="sort">type of sort</param>
    /// <param name="state">type of state</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CommitStatusList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/commits/{ref}/statuses", "Get a commit's statuses, by branch/tag/commit reference")]
    public Task<CommitStatus[]> ListCommitsStatusesAsync(string owner, string repo, string @ref, string? sort = default, string? state = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/commits/{@ref}/statuses".WithQuery().Param(sort).Param(state).Param(paging), cancelToken).JsonResponseAsync<CommitStatus[]>(cancelToken);

    /// <summary>Get a commit&apos;s combined status, by branch/tag/commit reference</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="ref">name of branch/tag/commit</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CombinedStatus</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/commits/{ref}/status", "Get a commit's combined status, by branch/tag/commit reference")]
    public Task<CombinedStatus> GetCommitsCombinedStatusAsync(string owner, string repo, string @ref, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/commits/{@ref}/status".WithQuery().Param(paging), cancelToken).JsonResponseAsync<CombinedStatus>(cancelToken);

    /// <summary>Get a commit&apos;s statuses</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">sha of the commit</param>
    /// <param name="sort">type of sort</param>
    /// <param name="state">type of state</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CommitStatusList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/statuses/{sha}", "Get a commit's statuses")]
    public Task<CommitStatus[]> ListCommitStatusesAsync(string owner, string repo, string sha, string? sort = default, string? state = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/statuses/{sha}".WithQuery().Param(sort).Param(state).Param(paging), cancelToken).JsonResponseAsync<CommitStatus[]>(cancelToken);

    /// <summary>Create a commit status</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">sha of the commit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CommitStatus</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/statuses/{sha}", "Create a commit status")]
    public Task<CommitStatus> CreateCommitStatusAsync(string owner, string repo, string sha, CreateStatusOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/statuses/{sha}", options, cancelToken).JsonResponseAsync<CommitStatus>(cancelToken);
    #endregion

    #region Git
    /// <summary>Gets the tree of a repository.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">sha of the commit</param>
    /// <param name="recursive">show all directories and files</param>
    /// <param name="page">page number; the &apos;truncated&apos; field in the response will be true if there are still more items after this page, false if the last page</param>
    /// <param name="per_page">number of items per page</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GitTreeResponse</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/git/trees/{sha}", "Gets the tree of a repository.")]
    public Task<GitTreeResponse> GetTreeAsync(string owner, string repo, string sha, bool? recursive = default, int? page = default, int? per_page = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/git/trees/{sha}".WithQuery().Param(recursive).Param(page).Param(per_page), cancelToken).JsonResponseAsync<GitTreeResponse>(cancelToken);

    /// <summary>Get specified ref or filtered repository&apos;s refs</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="ref">part or full name of the ref</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ReferenceList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/git/refs/{ref}", "Get specified ref or filtered repository's refs")]
    public Task<Reference[]> ListRefsAsync(string owner, string repo, string @ref, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/git/refs/{@ref}", cancelToken).JsonResponseAsync<Reference[]>(cancelToken);

    /// <summary>Gets the metadata of all the entries of the root dir</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="ref">The name of the commit/branch/tag. Default the repository’s default branch (usually master)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ContentsListResponse</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/contents", "Gets the metadata of all the entries of the root dir")]
    public Task<ContentsResponse[]> ListContentsAsync(string owner, string repo, string? @ref = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/contents".WithQuery().Param(@ref), cancelToken).JsonResponseAsync<ContentsResponse[]>(cancelToken);

    /// <summary>Gets the metadata and contents (if a file) of an entry in a repository, or a list of entries if a dir</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="filepath">path of the dir, file, symlink or submodule in the repo</param>
    /// <param name="ref">The name of the commit/branch/tag. Default the repository’s default branch (usually master)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ContentsResponse</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/contents/{filepath}", "Gets the metadata and contents (if a file) of an entry in a repository, or a list of entries if a dir")]
    public Task<ContentsResponse> GetContentAsync(string owner, string repo, string filepath, string? @ref = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/contents/{filepath}".WithQuery().Param(@ref), cancelToken).JsonResponseAsync<ContentsResponse>(cancelToken);

    /// <summary>Gets the blob of a repository.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">sha of the commit</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GitBlobResponse</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/git/blobs/{sha}", "Gets the blob of a repository.")]
    public Task<GitBlobResponse> GetBlobAsync(string owner, string repo, string sha, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/git/blobs/{sha}", cancelToken).JsonResponseAsync<GitBlobResponse>(cancelToken);

    /// <summary>Get a file or it&apos;s LFS object from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="filepath">filepath of the file to get</param>
    /// <param name="ref">The name of the commit/branch/tag. Default the repository’s default branch (usually master)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Returns raw file content.</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/media/{filepath}", "Get a file or it's LFS object from a repository")]
    [ManualEdit("応答本文のデータを利用するため独自定義の結果型を使用")]
    public Task<ResponseResult<DownloadResult>> GetObjectAsync(string owner, string repo, string filepath, string? @ref = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/media/{filepath}".WithQuery().Param(@ref), cancelToken).DownloadResponseAsync(cancelToken);

    /// <summary>Get a file from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="filepath">filepath of the file to get</param>
    /// <param name="ref">The name of the commit/branch/tag. Default the repository’s default branch (usually master)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Returns raw file content.</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/raw/{filepath}", "Get a file from a repository")]
    [ManualEdit("応答本文のデータを利用するため独自定義の結果型を使用")]
    public Task<ResponseResult<DownloadResult>> GetFileAsync(string owner, string repo, string filepath, string? @ref = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/raw/{filepath}".WithQuery().Param(@ref), cancelToken).DownloadResponseAsync(cancelToken);

    /// <summary>Create a file in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="filepath">path of the file to create</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>FileResponse</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/contents/{filepath}", "Create a file in a repository")]
    public Task<FileResponse> CreateFileAsync(string owner, string repo, string filepath, CreateFileOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/contents/{filepath}", options, cancelToken).JsonResponseAsync<FileResponse>(cancelToken);

    /// <summary>Update a file in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="filepath">path of the file to update</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>FileResponse</returns>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/contents/{filepath}", "Update a file in a repository")]
    public Task<FileResponse> UpdateFileAsync(string owner, string repo, string filepath, UpdateFileOptions options, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/contents/{filepath}", options, cancelToken).JsonResponseAsync<FileResponse>(cancelToken);

    /// <summary>Modify multiple files in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>FilesResponse</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/contents", "Modify multiple files in a repository")]
    public Task<FilesResponse> UpdateFilesAsync(string owner, string repo, ChangeFilesOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/contents", options, cancelToken).JsonResponseAsync<FilesResponse>(cancelToken);

    /// <summary>Delete a file in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="filepath">path of the file to delete</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>FileDeleteResponse</returns>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/contents/{filepath}", "Delete a file in a repository")]
    public Task<FileDeleteResponse> DeleteFileAsync(string owner, string repo, string filepath, DeleteFileOptions options, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/contents/{filepath}", options, cancelToken).JsonResponseAsync<FileDeleteResponse>(cancelToken);

    /// <summary>Apply diff patch to repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>FileResponse</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/diffpatch", "Apply diff patch to repository")]
    public Task<FileResponse> ApplyPatchAsync(string owner, string repo, UpdateFileOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/diffpatch", options, cancelToken).JsonResponseAsync<FileResponse>(cancelToken);

    /// <summary>Get specified ref or filtered repository&apos;s refs</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ReferenceList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/git/refs", "Get specified ref or filtered repository's refs")]
    public Task<Reference[]> ListRefsAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/git/refs", cancelToken).JsonResponseAsync<Reference[]>(cancelToken);

    /// <summary>Gets the tag object of an annotated tag (not lightweight tags)</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="sha">sha of the tag. The Git tags API only supports annotated tag objects, not lightweight tags.</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>AnnotatedTag</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/git/tags/{sha}", "Gets the tag object of an annotated tag (not lightweight tags)")]
    public Task<AnnotatedTag> GetAnnotatedTagAsync(string owner, string repo, string sha, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/git/tags/{sha}", cancelToken).JsonResponseAsync<AnnotatedTag>(cancelToken);
    #endregion

    #region GitHook
    /// <summary>List the Git hooks in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GitHookList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/hooks/git", "List the Git hooks in a repository")]
    public Task<GitHook[]> ListGitHooksAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/hooks/git", cancelToken).JsonResponseAsync<GitHook[]>(cancelToken);

    /// <summary>Get a Git hook</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the hook to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GitHook</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/hooks/git/{id}", "Get a Git hook")]
    public Task<GitHook> GetGitHookAsync(string owner, string repo, string id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/hooks/git/{id}", cancelToken).JsonResponseAsync<GitHook>(cancelToken);

    /// <summary>Edit a Git hook in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the hook to get</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GitHook</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/hooks/git/{id}", "Edit a Git hook in a repository")]
    public Task<GitHook> UpdateGitHookAsync(string owner, string repo, string id, EditGitHookOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/hooks/git/{id}", options, cancelToken).JsonResponseAsync<GitHook>(cancelToken);

    /// <summary>Delete a Git hook in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the hook to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/hooks/git/{id}", "Delete a Git hook in a repository")]
    public Task DeleteGitHookAsync(string owner, string repo, string id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/hooks/git/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Fork
    /// <summary>List a repository&apos;s forks</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepositoryList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/forks", "List a repository's forks")]
    public Task<Repository[]> ListForksAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/forks".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Repository[]>(cancelToken);

    /// <summary>Fork a repository</summary>
    /// <param name="owner">owner of the repo to fork</param>
    /// <param name="repo">name of the repo to fork</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/forks", "Fork a repository")]
    public Task<Repository> ForkAsync(string owner, string repo, CreateForkOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/forks", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);
    #endregion

    #region Actions
    /// <summary>Get a repository&apos;s actions runner registration token</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/actions/runners/registration-token", "Get a repository's actions runner registration token")]
    [ManualEdit("結果値が得られるため独自型を定義して利用")]
    public Task<RegistrationTokenResult> GetActionsRunnerRegistrationTokenAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/actions/runners/registration-token", cancelToken).JsonResponseAsync<RegistrationTokenResult>(cancelToken);

    /// <summary>List a repository&apos;s action tasks</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TasksList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/actions/tasks", "List a repository's action tasks")]
    public Task<ActionTaskResponse> ListActionsTasksAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/actions/tasks".WithQuery().Param(paging), cancelToken).JsonResponseAsync<ActionTaskResponse>(cancelToken);

    /// <summary>Dispatches a workflow</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="workflowname">name of the workflow</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/actions/workflows/{workflowname}/dispatches", "Dispatches a workflow")]
    [ManualEdit("swagger 定義の戻り値が得られる状況が不明のため、戻り値無しにしている")]
    public Task DispatchActionsWorkflowAsync(string owner, string repo, string workflowname, DispatchWorkflowOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/actions/workflows/{workflowname}/dispatches", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Search for repository&apos;s action jobs according filter conditions</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="labels">a comma separated list of run job labels to search for</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RunJobList is a list of action run jobs</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/actions/runners/jobs", "Search for repository's action jobs according filter conditions")]
    [ManualEdit("戻り値を nullable に変更")]
    public Task<ActionRunJob[]?> ListActionsJobsAsync(string owner, string repo, string? labels = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/actions/runners/jobs".WithQuery().Param(labels), cancelToken).JsonResponseAsync<ActionRunJob[]?>(cancelToken);
    #endregion

    #region Actions Secret
    /// <summary>List an repo&apos;s actions secrets</summary>
    /// <param name="owner">owner of the repository</param>
    /// <param name="repo">name of the repository</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>SecretList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/actions/secrets", "List an repo's actions secrets")]
    public Task<Secret[]> ListActionsSecretsAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/actions/secrets".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Secret[]>(cancelToken);

    /// <summary>Create or Update a secret value in a repository</summary>
    /// <param name="owner">owner of the repository</param>
    /// <param name="repo">name of the repository</param>
    /// <param name="secretname">name of the secret</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/actions/secrets/{secretname}", "Create or Update a secret value in a repository")]
    public Task SetActionsSecretAsync(string owner, string repo, string secretname, CreateOrUpdateSecretOption options, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/actions/secrets/{secretname}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a secret in a repository</summary>
    /// <param name="owner">owner of the repository</param>
    /// <param name="repo">name of the repository</param>
    /// <param name="secretname">name of the secret</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/actions/secrets/{secretname}", "Delete a secret in a repository")]
    public Task DeleteActionsSecretAsync(string owner, string repo, string secretname, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/actions/secrets/{secretname}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Actions Variable
    /// <summary>Get repo-level variables list</summary>
    /// <param name="owner">name of the owner</param>
    /// <param name="repo">name of the repository</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>VariableList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/actions/variables", "Get repo-level variables list")]
    public Task<ActionVariable[]> ListActionsVariablesAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/actions/variables".WithQuery().Param(paging), cancelToken).JsonResponseAsync<ActionVariable[]>(cancelToken);

    /// <summary>Get a repo-level variable</summary>
    /// <param name="owner">name of the owner</param>
    /// <param name="repo">name of the repository</param>
    /// <param name="variablename">name of the variable</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActionVariable</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/actions/variables/{variablename}", "Get a repo-level variable")]
    public Task<ActionVariable> GetActionsVariableAsync(string owner, string repo, string variablename, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/actions/variables/{variablename}", cancelToken).JsonResponseAsync<ActionVariable>(cancelToken);

    /// <summary>Create a repo-level variable</summary>
    /// <param name="owner">name of the owner</param>
    /// <param name="repo">name of the repository</param>
    /// <param name="variablename">name of the variable</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/actions/variables/{variablename}", "Create a repo-level variable")]
    public Task CreateActionsVariableAsync(string owner, string repo, string variablename, CreateVariableOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/actions/variables/{variablename}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Update a repo-level variable</summary>
    /// <param name="owner">name of the owner</param>
    /// <param name="repo">name of the repository</param>
    /// <param name="variablename">name of the variable</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/actions/variables/{variablename}", "Update a repo-level variable")]
    public Task UpdateActionsVariableAsync(string owner, string repo, string variablename, UpdateVariableOption options, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/actions/variables/{variablename}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a repo-level variable</summary>
    /// <param name="owner">name of the owner</param>
    /// <param name="repo">name of the repository</param>
    /// <param name="variablename">name of the variable</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActionVariable</returns>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/actions/variables/{variablename}", "Delete a repo-level variable")]
    [ManualEdit("Swaggerの戻り値定義誤りを訂正")]
    public Task DeleteActionsVariableAsync(string owner, string repo, string variablename, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/actions/variables/{variablename}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    #endregion

    #region Flag
    /// <summary>List a repository&apos;s flags</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>StringSlice</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/flags", "List a repository's flags")]
    public Task<string[]> ListFlagsAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/flags", cancelToken).JsonResponseAsync<string[]>(cancelToken);

    /// <summary>Check if a repository has a given flag</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="flag">name of the flag</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/flags/{flag}", "Check if a repository has a given flag")]
    public Task<StatusCodeResult> CheckFlagGivenAsync(string owner, string repo, string flag, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/flags/{flag}", cancelToken).StatusResponseAsync(cancelToken);

    /// <summary>Add a flag to a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="flag">name of the flag</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/flags/{flag}", "Add a flag to a repository")]
    public Task AddFlagAsync(string owner, string repo, string flag, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/flags/{flag}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Replace all flags of a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/flags", "Replace all flags of a repository")]
    public Task ReplaceFlagsAsync(string owner, string repo, ReplaceFlagsOption options, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/flags", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Remove a flag from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="flag">name of the flag</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/flags/{flag}", "Remove a flag from a repository")]
    public Task RemoveFlagAsync(string owner, string repo, string flag, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/flags/{flag}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Remove all flags from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/flags", "Remove all flags from a repository")]
    public Task ClearFlagsAsync(string owner, string repo, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/flags", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Webhook
    /// <summary>List the hooks in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>HookList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/hooks", "List the hooks in a repository")]
    public Task<Hook[]> ListWebhooksAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/hooks".WithQuery().Param(paging), cancelToken).JsonResponseAsync<Hook[]>(cancelToken);

    /// <summary>Get a hook</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the hook to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/hooks/{id}", "Get a hook")]
    public Task<Hook> GetWebhookAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/hooks/{id}", cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Create a hook</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/hooks", "Create a hook")]
    public Task<Hook> CreateWebhookAsync(string owner, string repo, CreateHookOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/hooks", options, cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Edit a hook in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">index of the hook</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Hook</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/hooks/{id}", "Edit a hook in a repository")]
    public Task<Hook> UpdateWebhookAsync(string owner, string repo, long id, EditHookOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/hooks/{id}", options, cancelToken).JsonResponseAsync<Hook>(cancelToken);

    /// <summary>Delete a hook in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the hook to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/hooks/{id}", "Delete a hook in a repository")]
    public Task DeleteWebhookAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/hooks/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Test a push webhook</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the hook to test</param>
    /// <param name="ref">The name of the commit/branch/tag, indicates which commit will be loaded to the webhook payload.</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/hooks/{id}/tests", "Test a push webhook")]
    public Task TestWebhookAsync(string owner, string repo, long id, string? @ref = default, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/hooks/{id}/tests".WithQuery().Param(@ref), cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Issue
    /// <summary>Returns the issue config for a repo</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepoIssueConfig</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issue_config", "Returns the issue config for a repo")]
    public Task<IssueConfig> GetIssueConfigAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issue_config", cancelToken).JsonResponseAsync<IssueConfig>(cancelToken);

    /// <summary>Returns the validation information for a issue config</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepoIssueConfigValidation</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issue_config/validate", "Returns the validation information for a issue config")]
    public Task<IssueConfigValidation> GetIssueConfigValidationAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issue_config/validate", cancelToken).JsonResponseAsync<IssueConfigValidation>(cancelToken);

    /// <summary>Get available issue templates for a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>IssueTemplates</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issue_templates", "Get available issue templates for a repository")]
    public Task<IssueTemplate[]> ListIssueTemplatesAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issue_templates", cancelToken).JsonResponseAsync<IssueTemplate[]>(cancelToken);
    #endregion

    #region PullRequest
    /// <summary>List a repo&apos;s pull requests</summary>
    /// <param name="owner">Owner of the repo</param>
    /// <param name="repo">Name of the repo</param>
    /// <param name="state">State of pull request</param>
    /// <param name="sort">Type of sort</param>
    /// <param name="milestone">ID of the milestone</param>
    /// <param name="labels">Label IDs</param>
    /// <param name="poster">Filter by pull request author</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullRequestList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls", "List a repo's pull requests")]
    public Task<PullRequest[]> ListPullRequestsAsync(string owner, string repo, string? state = default, string? sort = default, long? milestone = default, ICollection<long>? labels = default, string? poster = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls".WithQuery().Param(state).Param(sort).Param(milestone).Param(labels).Param(poster).Param(paging), cancelToken).JsonResponseAsync<PullRequest[]>(cancelToken);

    /// <summary>Create a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullRequest</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls", "Create a pull request")]
    public Task<PullRequest> CreatePullRequestAsync(string owner, string repo, CreatePullRequestOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls", options, cancelToken).JsonResponseAsync<PullRequest>(cancelToken);

    /// <summary>Update a pull request. If using deadline only the date will be taken into account, and time of day ignored.</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullRequest</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/pulls/{index}", "Update a pull request. If using deadline only the date will be taken into account, and time of day ignored.")]
    public Task<PullRequest> UpdatePullRequestAsync(string owner, string repo, long index, EditPullRequestOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/pulls/{index}", options, cancelToken).JsonResponseAsync<PullRequest>(cancelToken);

    /// <summary>Get a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullRequest</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}", "Get a pull request")]
    public Task<PullRequest> GetPullRequestAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}", cancelToken).JsonResponseAsync<PullRequest>(cancelToken);

    /// <summary>Get a pull request by base and head</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="base">base of the pull request to get</param>
    /// <param name="head">head of the pull request to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullRequest</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{base}/{head}", "Get a pull request by base and head")]
    public Task<PullRequest> GetPullRequestAsync(string owner, string repo, string @base, string head, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{@base}/{head}", cancelToken).JsonResponseAsync<PullRequest>(cancelToken);

    /// <summary>Get a pull request diff or patch</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request to get</param>
    /// <param name="diffType">whether the output is diff or patch</param>
    /// <param name="binary">whether to include binary file changes. if true, the diff is applicable with `git apply`</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>APIString is a string response</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}.{diffType}", "Get a pull request diff or patch")]
    public Task<string> GetPullRequestDiffAsync(string owner, string repo, long index, string diffType, bool? binary = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}.{diffType}".WithQuery().Param(binary), cancelToken).TextResponseAsync(cancelToken);

    /// <summary>Get commits for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request to get</param>
    /// <param name="verification">include verification for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="files">include a list of affected files for every commit (disable for speedup, default &apos;true&apos;)</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>CommitList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}/commits", "Get commits for a pull request")]
    public Task<Commit[]> ListPullRequestCommitsAsync(string owner, string repo, long index, bool? verification = default, bool? files = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}/commits".WithQuery().Param(verification).Param(files).Param(paging), cancelToken).JsonResponseAsync<Commit[]>(cancelToken);

    /// <summary>Get changed files for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request to get</param>
    /// <param name="skip_to">skip to given file</param>
    /// <param name="whitespace">whitespace behavior</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ChangedFileList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}/files", "Get changed files for a pull request")]
    public Task<ChangedFile[]> ListPullRequestChangesAsync(string owner, string repo, long index, string? skip_to = default, string? whitespace = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}/files".WithQuery().Param(skip_to).Param(whitespace).Param(paging), cancelToken).JsonResponseAsync<ChangedFile[]>(cancelToken);

    /// <summary>Check if a pull request has been merged</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}/merge", "Check if a pull request has been merged")]
    public Task<StatusCodeResult> CheckPullRequestMergeAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}/merge", cancelToken).StatusResponseAsync(cancelToken);

    /// <summary>Merge a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request to merge</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls/{index}/merge", "Merge a pull request")]
    public Task MergePullRequestAsync(string owner, string repo, long index, MergePullRequestOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls/{index}/merge", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Merge PR&apos;s baseBranch into headBranch</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request to get</param>
    /// <param name="style">how to update pull request</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls/{index}/update", "Merge PR's baseBranch into headBranch")]
    public Task UpdateMergePullRequestAsync(string owner, string repo, long index, string? style = default, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls/{index}/update".WithQuery().Param(style), cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Cancel the scheduled auto merge for the given pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request to merge</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/pulls/{index}/merge", "Cancel the scheduled auto merge for the given pull request")]
    public Task CancelPullRequestAutoMergeAsync(string owner, string repo, long index, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/pulls/{index}/merge", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region PullRequest - review
    /// <summary>Return all users that can be requested to review in this repo</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/reviewers", "Return all users that can be requested to review in this repo")]
    public Task<User[]> ListReviewRequestedUsersAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/reviewers", cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>create review requests for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullReviewList</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls/{index}/requested_reviewers", "create review requests for a pull request")]
    public Task<PullReview[]> CreatePullRequestReviewRequestsAsync(string owner, string repo, long index, PullReviewRequestOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls/{index}/requested_reviewers", options, cancelToken).JsonResponseAsync<PullReview[]>(cancelToken);

    /// <summary>cancel review requests for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/pulls/{index}/requested_reviewers", "cancel review requests for a pull request")]
    public Task CancelPullRequestReviewRequestsAsync(string owner, string repo, long index, PullReviewRequestOptions options, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/pulls/{index}/requested_reviewers", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List all reviews for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullReviewList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}/reviews", "List all reviews for a pull request")]
    public Task<PullReview[]> ListPullRequestReviewsAsync(string owner, string repo, long index, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}/reviews".WithQuery().Param(paging), cancelToken).JsonResponseAsync<PullReview[]>(cancelToken);

    /// <summary>Get a specific review for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullReview</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}", "Get a specific review for a pull request")]
    public Task<PullReview> GetPullRequestReviewAsync(string owner, string repo, long index, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}", cancelToken).JsonResponseAsync<PullReview>(cancelToken);

    /// <summary>Create a review to an pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullReview</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls/{index}/reviews", "Create a review to an pull request")]
    public Task<PullReview> CreatePullRequestReviewAsync(string owner, string repo, long index, CreatePullReviewOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls/{index}/reviews", options, cancelToken).JsonResponseAsync<PullReview>(cancelToken);

    /// <summary>Delete a specific review from a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}", "Delete a specific review from a pull request")]
    public Task DeletePullRequestReviewAsync(string owner, string repo, long index, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Submit a pending review to an pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullReview</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}", "Submit a pending review to an pull request")]
    public Task<PullReview> SubmitPullRequestPendingReviewAsync(string owner, string repo, long index, long id, SubmitPullReviewOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}", options, cancelToken).JsonResponseAsync<PullReview>(cancelToken);

    /// <summary>Dismiss a review for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullReview</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}/dismissals", "Dismiss a review for a pull request")]
    public Task<PullReview> DismissPullRequestReviewAsync(string owner, string repo, long index, long id, DismissPullReviewOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}/dismissals", options, cancelToken).JsonResponseAsync<PullReview>(cancelToken);

    /// <summary>Cancel to dismiss a review for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullReview</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}/undismissals", "Cancel to dismiss a review for a pull request")]
    public Task<PullReview> UndismissPullRequestReviewAsync(string owner, string repo, long index, long id, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}/undismissals", cancelToken).JsonResponseAsync<PullReview>(cancelToken);

    /// <summary>Get a specific review for a pull request</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullCommentList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments", "Get a specific review for a pull request")]
    public Task<PullReviewComment[]> ListPullRequestReviewCommentsAsync(string owner, string repo, long index, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments", cancelToken).JsonResponseAsync<PullReviewComment[]>(cancelToken);

    /// <summary>Get a pull review comment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="comment">id of the comment</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullComment</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments/{comment}", "Get a pull review comment")]
    public Task<PullReviewComment> GetPullRequestReviewCommentAsync(string owner, string repo, long index, long id, long comment, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments/{comment}", cancelToken).JsonResponseAsync<PullReviewComment>(cancelToken);

    /// <summary>Add a new comment to a pull request review</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullComment</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments", "Add a new comment to a pull request review")]
    public Task<PullReviewComment> AddPullRequestReviewCommentAsync(string owner, string repo, long index, long id, CreatePullReviewComment options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments", options, cancelToken).JsonResponseAsync<PullReviewComment>(cancelToken);

    /// <summary>Delete a pull review comment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="index">index of the pull request</param>
    /// <param name="id">id of the review</param>
    /// <param name="comment">id of the comment</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments/{comment}", "Delete a pull review comment")]
    public Task DeletePullRequestReviewCommentAsync(string owner, string repo, long index, long id, long comment, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments/{comment}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Pin
    /// <summary>Returns if new Issue Pins are allowed</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepoNewIssuePinsAllowed</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/new_pin_allowed", "Returns if new Issue Pins are allowed")]
    public Task<NewIssuePinsAllowed> GetPinsAllowdAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/new_pin_allowed", cancelToken).JsonResponseAsync<NewIssuePinsAllowed>(cancelToken);

    /// <summary>List a repo&apos;s pinned issues</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>IssueList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/issues/pinned", "List a repo's pinned issues")]
    public Task<Issue[]> ListPinnedIssuesAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/issues/pinned", cancelToken).JsonResponseAsync<Issue[]>(cancelToken);

    /// <summary>List a repo&apos;s pinned pull requests</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PullRequestList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/pulls/pinned", "List a repo's pinned pull requests")]
    public Task<PullRequest[]> ListPinnedPullRequestsAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/pulls/pinned", cancelToken).JsonResponseAsync<PullRequest[]>(cancelToken);
    #endregion

    #region Release
    /// <summary>List a repo&apos;s releases</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="draft">filter (exclude / include) drafts, if you dont have repo write access none will show</param>
    /// <param name="pre_release">filter (exclude / include) pre-releases</param>
    /// <param name="q">Search string</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ReleaseList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/releases", "List a repo's releases")]
    public Task<Release[]> ListReleasesAsync(string owner, string repo, bool? draft = default, bool? pre_release = default, string? q = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/releases".WithQuery().Param(draft).Param(pre_release).Param(q).Param(paging), cancelToken).JsonResponseAsync<Release[]>(cancelToken);

    /// <summary>Get a release</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the release to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Release</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/releases/{id}", "Get a release")]
    public Task<Release> GetReleaseAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/releases/{id}", cancelToken).JsonResponseAsync<Release>(cancelToken);

    /// <summary>Get a release by tag name</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="tag">tag name of the release to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Release</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/releases/tags/{tag}", "Get a release by tag name")]
    public Task<Release> GetReleaseTagAsync(string owner, string repo, string tag, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/releases/tags/{tag}", cancelToken).JsonResponseAsync<Release>(cancelToken);

    /// <summary>Gets the most recent non-prerelease, non-draft release of a repository, sorted by created_at</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Release</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/releases/latest", "Gets the most recent non-prerelease, non-draft release of a repository, sorted by created_at")]
    public Task<Release> GetReleaseLatestAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/releases/latest", cancelToken).JsonResponseAsync<Release>(cancelToken);

    /// <summary>Create a release</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Release</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/releases", "Create a release")]
    public Task<Release> CreateReleaseAsync(string owner, string repo, CreateReleaseOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/releases", options, cancelToken).JsonResponseAsync<Release>(cancelToken);

    /// <summary>Update a release</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the release to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Release</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/releases/{id}", "Update a release")]
    public Task<Release> UpdateReleaseAsync(string owner, string repo, long id, EditReleaseOption options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/releases/{id}", options, cancelToken).JsonResponseAsync<Release>(cancelToken);

    /// <summary>Delete a release</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the release to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/releases/{id}", "Delete a release")]
    public Task DeleteReleaseAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/releases/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a release by tag name</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="tag">tag name of the release to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/releases/tags/{tag}", "Delete a release by tag name")]
    public Task DeleteReleaseTagAsync(string owner, string repo, string tag, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/releases/tags/{tag}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>List release&apos;s attachments</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the release</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>AttachmentList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/releases/{id}/assets", "List release's attachments")]
    public Task<Attachment[]> ListReleaseAttachmentsAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/releases/{id}/assets", cancelToken).JsonResponseAsync<Attachment[]>(cancelToken);

    /// <summary>Get a release attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the release</param>
    /// <param name="attachment_id">id of the attachment to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}", "Get a release attachment")]
    public Task<Attachment> GetReleaseAttachmentAsync(string owner, string repo, long id, long attachment_id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}", cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Create a release attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the release</param>
    /// <param name="attachment">attachment to upload (this parameter is incompatible with `external_url`)</param>
    /// <param name="external_url">url to external asset (this parameter is incompatible with `attachment`)</param>
    /// <param name="name">name of the attachment</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/releases/{id}/assets", "Create a release attachment")]
    public Task<Attachment> CreateReleaseAttachmentAsync(string owner, string repo, long id, Stream? attachment = default, string? external_url = default, string? name = default, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/releases/{id}/assets".WithQuery().Param(name), new FormData().File(attachment).Scalar(external_url).AsContent(), cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Edit a release attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the release</param>
    /// <param name="attachment_id">id of the attachment to edit</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Attachment</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}", "Edit a release attachment")]
    public Task<Attachment> UpdateReleaseAttachmentAsync(string owner, string repo, long id, long attachment_id, EditAttachmentOptions options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}", options, cancelToken).JsonResponseAsync<Attachment>(cancelToken);

    /// <summary>Delete a release attachment</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the release</param>
    /// <param name="attachment_id">id of the attachment to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}", "Delete a release attachment")]
    public Task DeleteReleaseAttachmentAsync(string owner, string repo, long id, long attachment_id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Key
    /// <summary>List a repository&apos;s keys</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="key_id">the key_id to search for</param>
    /// <param name="fingerprint">fingerprint of the key</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>DeployKeyList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/keys", "List a repository's keys")]
    public Task<DeployKey[]> ListDeployKeysAsync(string owner, string repo, int? key_id = default, string? fingerprint = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/keys".WithQuery().Param(key_id).Param(fingerprint).Param(paging), cancelToken).JsonResponseAsync<DeployKey[]>(cancelToken);

    /// <summary>Get a repository&apos;s key by id</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the key to get</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>DeployKey</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/keys/{id}", "Get a repository's key by id")]
    public Task<DeployKey> GetDeployKeyAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/keys/{id}", cancelToken).JsonResponseAsync<DeployKey>(cancelToken);

    /// <summary>Add a key to a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>DeployKey</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/keys", "Add a key to a repository")]
    public Task<DeployKey> AddDeployKeyAsync(string owner, string repo, CreateKeyOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/keys", options, cancelToken).JsonResponseAsync<DeployKey>(cancelToken);

    /// <summary>Delete a key from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="id">id of the key to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/keys/{id}", "Delete a key from a repository")]
    public Task DeleteDeployKeyAsync(string owner, string repo, long id, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/keys/{id}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Get signing-key.gpg for given repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GPG armored public key</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/signing-key.gpg", "Get signing-key.gpg for given repository")]
    public Task<string> GetSigningKeyGpgAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/signing-key.gpg", cancelToken).TextResponseAsync(cancelToken);
    #endregion

    #region PushMirror
    /// <summary>Get all push mirrors of the repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PushMirrorList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/push_mirrors", "Get all push mirrors of the repository")]
    public Task<PushMirror[]> ListAllPushMirrorsAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/push_mirrors".WithQuery().Param(paging), cancelToken).JsonResponseAsync<PushMirror[]>(cancelToken);

    /// <summary>Get push mirror of the repository by remoteName</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="name">remote name of push mirror</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PushMirror</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/push_mirrors/{name}", "Get push mirror of the repository by remoteName")]
    public Task<PushMirror> GetPushMirrorAsync(string owner, string repo, string name, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/push_mirrors/{name}", cancelToken).JsonResponseAsync<PushMirror>(cancelToken);

    /// <summary>add a push mirror to the repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PushMirror</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/push_mirrors", "add a push mirror to the repository")]
    public Task<PushMirror> AddPushMirrorAsync(string owner, string repo, CreatePushMirrorOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/push_mirrors", options, cancelToken).JsonResponseAsync<PushMirror>(cancelToken);

    /// <summary>deletes a push mirror from a repository by remoteName</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="name">remote name of the pushMirror</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/push_mirrors/{name}", "deletes a push mirror from a repository by remoteName")]
    public Task DeletePushMirrorAsync(string owner, string repo, string name, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/push_mirrors/{name}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Sync all push mirrored repository</summary>
    /// <param name="owner">owner of the repo to sync</param>
    /// <param name="repo">name of the repo to sync</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/push_mirrors-sync", "Sync all push mirrored repository")]
    public Task SyncPushMirrorsAsync(string owner, string repo, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/push_mirrors-sync", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Collaborator
    /// <summary>List a repository&apos;s collaborators</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/collaborators", "List a repository's collaborators")]
    public Task<User[]> ListCollaboratorsAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/collaborators".WithQuery().Param(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Check if a user is a collaborator of a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="collaborator">username of the collaborator</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/collaborators/{collaborator}", "Check if a user is a collaborator of a repository")]
    public Task<StatusCodeResult> CheckCollaboratorAsync(string owner, string repo, string collaborator, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/collaborators/{collaborator}", cancelToken).StatusResponseAsync(cancelToken);

    /// <summary>Get repository permissions for a user</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="collaborator">username of the collaborator</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>RepoCollaboratorPermission</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/collaborators/{collaborator}/permission", "Get repository permissions for a user")]
    public Task<RepoCollaboratorPermission> GetCollaboratorPermissionAsync(string owner, string repo, string collaborator, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/collaborators/{collaborator}/permission", cancelToken).JsonResponseAsync<RepoCollaboratorPermission>(cancelToken);

    /// <summary>Add a collaborator to a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="collaborator">username of the collaborator to add</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/collaborators/{collaborator}", "Add a collaborator to a repository")]
    public Task AddCollaboratorAsync(string owner, string repo, string collaborator, AddCollaboratorOption options, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/collaborators/{collaborator}", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a collaborator from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="collaborator">username of the collaborator to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/collaborators/{collaborator}", "Delete a collaborator from a repository")]
    public Task DeleteCollaboratorAsync(string owner, string repo, string collaborator, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/collaborators/{collaborator}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Subscription
    /// <summary>List a repo&apos;s watchers</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/subscribers", "List a repo's watchers")]
    public Task<User[]> ListSubscribersAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/subscribers".WithQuery().Param(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Check if the current user is watching a repo</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>WatchInfo</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/subscription", "Check if the current user is watching a repo")]
    public Task<WatchInfo> GetSubscriptionAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/subscription", cancelToken).JsonResponseAsync<WatchInfo>(cancelToken);

    /// <summary>Watch a repo</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>WatchInfo</returns>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/subscription", "Watch a repo")]
    public Task<WatchInfo> WatchAsync(string owner, string repo, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/subscription", cancelToken).JsonResponseAsync<WatchInfo>(cancelToken);

    /// <summary>Unwatch a repo</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/subscription", "Unwatch a repo")]
    public Task UnwatchAsync(string owner, string repo, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/subscription", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Team
    /// <summary>List a repository&apos;s teams</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TeamList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/teams", "List a repository's teams")]
    public Task<Team[]> ListTeamsAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/teams", cancelToken).JsonResponseAsync<Team[]>(cancelToken);

    /// <summary>Check if a team is assigned to a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="team">team name</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Team</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/teams/{team}", "Check if a team is assigned to a repository")]
    public Task<Team> GetTeamAssignedAsync(string owner, string repo, string team, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/teams/{team}", cancelToken).JsonResponseAsync<Team>(cancelToken);

    /// <summary>Add a team to a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="team">team name</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/teams/{team}", "Add a team to a repository")]
    public Task AddTeamAsync(string owner, string repo, string team, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/teams/{team}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a team from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="team">team name</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/teams/{team}", "Delete a team from a repository")]
    public Task DeleteTeamAsync(string owner, string repo, string team, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/teams/{team}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Topic
    /// <summary>Get list of topics that a repository has</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TopicNames</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/topics", "Get list of topics that a repository has")]
    public Task<TopicName> ListTopicsAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/topics".WithQuery().Param(paging), cancelToken).JsonResponseAsync<TopicName>(cancelToken);

    /// <summary>search topics via keyword</summary>
    /// <param name="q">keywords to search</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TopicListResponse</returns>
    [ForgejoEndpoint("GET", "/topics/search", "search topics via keyword")]
    [ManualEdit("得られる結果値に合わせた戻り値型を使用")]
    public Task<TopicSearchResults> SearchTopicsAsync(string q, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest("topics/search".WithQuery().Param(q).Param(paging), cancelToken).JsonResponseAsync<TopicSearchResults>(cancelToken);

    /// <summary>Add a topic to a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="topic">name of the topic to add</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/topics/{topic}", "Add a topic to a repository")]
    public Task AddTopicAsync(string owner, string repo, string topic, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/topics/{topic}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Replace list of topics for a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("PUT", "/repos/{owner}/{repo}/topics", "Replace list of topics for a repository")]
    public Task ReplaceTopicsAsync(string owner, string repo, RepoTopicOptions options, CancellationToken cancelToken = default)
        => PutRequest($"repos/{owner}/{repo}/topics", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete a topic from a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="topic">name of the topic to delete</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/topics/{topic}", "Delete a topic from a repository")]
    public Task DeleteTopicAsync(string owner, string repo, string topic, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/topics/{topic}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);
    #endregion

    #region Transfer
    /// <summary>Transfer a repo ownership</summary>
    /// <param name="owner">owner of the repo to transfer</param>
    /// <param name="repo">name of the repo to transfer</param>
    /// <param name="options">Transfer Options</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/transfer", "Transfer a repo ownership")]
    public Task<Repository> TransferOwnerAsync(string owner, string repo, TransferRepoOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/transfer", options, cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Accept a repo transfer</summary>
    /// <param name="owner">owner of the repo to transfer</param>
    /// <param name="repo">name of the repo to transfer</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/transfer/accept", "Accept a repo transfer")]
    public Task<Repository> AcceptTransferAsync(string owner, string repo, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/transfer/accept", cancelToken).JsonResponseAsync<Repository>(cancelToken);

    /// <summary>Reject a repo transfer</summary>
    /// <param name="owner">owner of the repo to transfer</param>
    /// <param name="repo">name of the repo to transfer</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Repository</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/transfer/reject", "Reject a repo transfer")]
    public Task<Repository> RejectTransferAsync(string owner, string repo, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/transfer/reject", cancelToken).JsonResponseAsync<Repository>(cancelToken);
    #endregion

    #region Wiki
    /// <summary>Get all wiki pages</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>WikiPageList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/wiki/pages", "Get all wiki pages")]
    public Task<WikiPageMetaData[]> ListWikiPagesAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/wiki/pages".WithQuery().Param(paging), cancelToken).JsonResponseAsync<WikiPageMetaData[]>(cancelToken);

    /// <summary>Get a wiki page</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="pageName">name of the page</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>WikiPage</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/wiki/page/{pageName}", "Get a wiki page")]
    public Task<WikiPage> GetWikiPageAsync(string owner, string repo, string pageName, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/wiki/page/{pageName}", cancelToken).JsonResponseAsync<WikiPage>(cancelToken);

    /// <summary>Create a wiki page</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>WikiPage</returns>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/wiki/new", "Create a wiki page")]
    public Task<WikiPage> CreateWikiPageAsync(string owner, string repo, CreateWikiPageOptions options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/wiki/new", options, cancelToken).JsonResponseAsync<WikiPage>(cancelToken);

    /// <summary>Edit a wiki page</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="pageName">name of the page</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>WikiPage</returns>
    [ForgejoEndpoint("PATCH", "/repos/{owner}/{repo}/wiki/page/{pageName}", "Edit a wiki page")]
    public Task<WikiPage> UpdateWikiPageAsync(string owner, string repo, string pageName, CreateWikiPageOptions options, CancellationToken cancelToken = default)
        => PatchRequest($"repos/{owner}/{repo}/wiki/page/{pageName}", options, cancelToken).JsonResponseAsync<WikiPage>(cancelToken);

    /// <summary>Delete a wiki page</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="pageName">name of the page</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/wiki/page/{pageName}", "Delete a wiki page")]
    public Task DeleteWikiPageAsync(string owner, string repo, string pageName, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/wiki/page/{pageName}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Get revisions of a wiki page</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="pageName">name of the page</param>
    /// <param name="page">page number of results to return (1-based)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>WikiCommitList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/wiki/revisions/{pageName}", "Get revisions of a wiki page")]
    public Task<WikiCommitList> ListWikiPageRevisionsAsync(string owner, string repo, string pageName, int? page = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/wiki/revisions/{pageName}".WithQuery().Param(page), cancelToken).JsonResponseAsync<WikiCommitList>(cancelToken);
    #endregion

    #region Misc
    /// <summary>List a repository&apos;s activity feeds</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="date">the date of the activities to be found</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ActivityFeedsList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/activities/feeds", "List a repository's activity feeds")]
    public Task<Activity[]> ListActivitiesAsync(string owner, string repo, DateTimeOffset? date = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/activities/feeds".WithQuery().Param(date).Param(paging), cancelToken).JsonResponseAsync<Activity[]>(cancelToken);

    /// <summary>Get an archive of a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="archive">the git reference for download with attached archive format (e.g. master.zip)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>success</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/archive/{archive}", "Get an archive of a repository")]
    [ManualEdit("応答本文のデータを利用するため独自定義の結果型を使用")]
    public Task<ResponseResult<DownloadResult>> GetArchiveAsync(string owner, string repo, string archive, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/archive/{archive}", cancelToken).DownloadResponseAsync(cancelToken);

    /// <summary>Return all users that have write access and can be assigned to issues</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/assignees", "Return all users that have write access and can be assigned to issues")]
    public Task<User[]> ListAssigneesAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/assignees", cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>Update avatar</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/repos/{owner}/{repo}/avatar", "Update avatar")]
    public Task UpdateAvatarAsync(string owner, string repo, UpdateRepoAvatarOption options, CancellationToken cancelToken = default)
        => PostRequest($"repos/{owner}/{repo}/avatar", options, cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Delete avatar</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/repos/{owner}/{repo}/avatar", "Delete avatar")]
    public Task DeleteAvatarAsync(string owner, string repo, CancellationToken cancelToken = default)
        => DeleteRequest($"repos/{owner}/{repo}/avatar", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Get languages and number of bytes of code written</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LanguageStatistics</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/languages", "Get languages and number of bytes of code written")]
    public Task<IDictionary<string, long>> ListCodeLanguagesAsync(string owner, string repo, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/languages", cancelToken).JsonResponseAsync<IDictionary<string, long>>(cancelToken);

    /// <summary>List a repo&apos;s stargazers</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>UserList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/stargazers", "List a repo's stargazers")]
    public Task<User[]> ListStargazersAsync(string owner, string repo, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/stargazers".WithQuery().Param(paging), cancelToken).JsonResponseAsync<User[]>(cancelToken);

    /// <summary>List a repo&apos;s tracked times</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="user">optional filter by user (available for issue managers)</param>
    /// <param name="since">Only show times updated after the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="before">Only show times updated before the given time. This is a timestamp in RFC 3339 format</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>TrackedTimeList</returns>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/times", "List a repo's tracked times")]
    public Task<TrackedTime[]> ListTrackedTimesAsync(string owner, string repo, string? user = default, DateTimeOffset? since = default, DateTimeOffset? before = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/times".WithQuery().Param(user).Param(since).Param(before).Param(paging), cancelToken).JsonResponseAsync<TrackedTime[]>(cancelToken);

    /// <summary>Get the EditorConfig definitions of a file in a repository</summary>
    /// <param name="owner">owner of the repo</param>
    /// <param name="repo">name of the repo</param>
    /// <param name="filepath">filepath of file to get</param>
    /// <param name="ref">The name of the commit/branch/tag. Default the repository’s default branch (usually master)</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("GET", "/repos/{owner}/{repo}/editorconfig/{filepath}", "Get the EditorConfig definitions of a file in a repository")]
    [ManualEdit("得られる結果値に合わせた戻り値型を使用")]
    public Task<IDictionary<string, string>> GetEditorConfigDefinitionAsync(string owner, string repo, string filepath, string? @ref = default, CancellationToken cancelToken = default)
        => GetRequest($"repos/{owner}/{repo}/editorconfig/{filepath}".WithQuery().Param(@ref), cancelToken).JsonResponseAsync<IDictionary<string, string>>(cancelToken);
    #endregion

}
