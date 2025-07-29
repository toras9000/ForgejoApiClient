namespace ForgejoApiClient.Api.Scopes;

/// <summary>package スコープのAPIインタフェース</summary>
public interface IPackageApi : IApiScope
{
    /// <summary>Gets all packages of an owner</summary>
    /// <param name="owner">owner of the packages</param>
    /// <param name="type">package type filter</param>
    /// <param name="q">name filter</param>
    /// <param name="paging">ページングオプション</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PackageList</returns>
    [ForgejoEndpoint("GET", "/packages/{owner}", "Gets all packages of an owner")]
    public Task<Package[]> ListAsync(string owner, string? type = default, string? q = default, PagingOptions paging = default, CancellationToken cancelToken = default)
        => GetRequest($"packages/{owner}".WithQuery().Param(type).Param(q).Param(paging), cancelToken).JsonResponseAsync<Package[]>(cancelToken);

    /// <summary>Gets a package</summary>
    /// <param name="owner">owner of the package</param>
    /// <param name="type">type of the package</param>
    /// <param name="name">name of the package</param>
    /// <param name="version">version of the package</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>Package</returns>
    [ForgejoEndpoint("GET", "/packages/{owner}/{type}/{name}/{version}", "Gets a package")]
    public Task<Package> GetAsync(string owner, string type, string name, string version, CancellationToken cancelToken = default)
        => GetRequest($"packages/{owner}/{type}/{name}/{version}", cancelToken).JsonResponseAsync<Package>(cancelToken);

    /// <summary>Delete a package</summary>
    /// <param name="owner">owner of the package</param>
    /// <param name="type">type of the package</param>
    /// <param name="name">name of the package</param>
    /// <param name="version">version of the package</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("DELETE", "/packages/{owner}/{type}/{name}/{version}", "Delete a package")]
    public Task DeleteAsync(string owner, string type, string name, string version, CancellationToken cancelToken = default)
        => DeleteRequest($"packages/{owner}/{type}/{name}/{version}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Gets all files of a package</summary>
    /// <param name="owner">owner of the package</param>
    /// <param name="type">type of the package</param>
    /// <param name="name">name of the package</param>
    /// <param name="version">version of the package</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>PackageFileList</returns>
    [ForgejoEndpoint("GET", "/packages/{owner}/{type}/{name}/{version}/files", "Gets all files of a package")]
    public Task<PackageFile[]> ListFilesAsync(string owner, string type, string name, string version, CancellationToken cancelToken = default)
        => GetRequest($"packages/{owner}/{type}/{name}/{version}/files", cancelToken).JsonResponseAsync<PackageFile[]>(cancelToken);

    /// <summary>Link a package to a repository</summary>
    /// <param name="owner">owner of the package</param>
    /// <param name="type">type of the package</param>
    /// <param name="name">name of the package</param>
    /// <param name="repo_name">name of the repository to link.</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/packages/{owner}/{type}/{name}/-/link/{repo_name}", "Link a package to a repository")]
    public Task LinkRepository(string owner, string type, string name, string repo_name, CancellationToken cancelToken = default)
        => PostRequest($"packages/{owner}/{type}/{name}/-/link/{repo_name}", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

    /// <summary>Unlink a package from a repository</summary>
    /// <param name="owner">owner of the package</param>
    /// <param name="type">type of the package</param>
    /// <param name="name">name of the package</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    [ForgejoEndpoint("POST", "/packages/{owner}/{type}/{name}/-/unlink", "Unlink a package from a repository")]
    public Task UnlinkRepository(string owner, string type, string name, CancellationToken cancelToken = default)
        => PostRequest($"packages/{owner}/{type}/{name}/-/unlink", cancelToken).JsonResponseAsync<EmptyResult>(cancelToken);

}
