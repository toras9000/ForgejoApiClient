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
        => GetRequest($"packages/{owner}".WithQuery(type).Param(q).Param(paging), cancelToken).JsonResponseAsync<Package[]>(cancelToken);

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
    public Task<PackageFile[]> GetFilesAsync(string owner, string type, string name, string version, CancellationToken cancelToken = default)
        => GetRequest($"packages/{owner}/{type}/{name}/{version}/files", cancelToken).JsonResponseAsync<PackageFile[]>(cancelToken);

}
