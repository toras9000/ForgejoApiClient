namespace ForgejoApiClient.Api.Scopes;

/// <summary>misc スコープのAPIインタフェース</summary>
public interface IMiscellaneousApi : IApiScope
{
    #region Gitignore Template
    /// <summary>Returns a list of all gitignore templates</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GitignoreTemplateList</returns>
    [ForgejoEndpoint("GET", "/gitignore/templates", "Returns a list of all gitignore templates")]
    public Task<string[]> ListGitignoreTemplatesAsync(CancellationToken cancelToken = default)
        => GetRequest("gitignore/templates", cancelToken).JsonResponseAsync(ApiDataSerializerContext.Default.StringArray, cancelToken);

    /// <summary>Returns information about a gitignore template</summary>
    /// <param name="name">name of the template</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GitignoreTemplateInfo</returns>
    [ForgejoEndpoint("GET", "/gitignore/templates/{name}", "Returns information about a gitignore template")]
    public Task<GitignoreTemplateInfo> GetGitignoreTemplateAsync(string name, CancellationToken cancelToken = default)
        => GetRequest($"gitignore/templates/{name}", cancelToken).JsonResponseAsync(ApiDataSerializerContext.Default.GitignoreTemplateInfo, cancelToken);
    #endregion

    #region Label Template
    /// <summary>Returns a list of all label templates</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LabelTemplateList</returns>
    [ForgejoEndpoint("GET", "/label/templates", "Returns a list of all label templates")]
    public Task<string[]> ListLabelTemplatesAsync(CancellationToken cancelToken = default)
        => GetRequest("label/templates", cancelToken).JsonResponseAsync(ApiDataSerializerContext.Default.StringArray, cancelToken);

    /// <summary>Returns all labels in a template</summary>
    /// <param name="name">name of the template</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LabelTemplateInfo</returns>
    [ForgejoEndpoint("GET", "/label/templates/{name}", "Returns all labels in a template")]
    public Task<LabelTemplate[]> ListTemplateLabelsAsync(string name, CancellationToken cancelToken = default)
        => GetRequest($"label/templates/{name}", cancelToken).JsonResponseAsync(ApiDataSerializerContext.Default.LabelTemplateArray, cancelToken);
    #endregion

    #region License Template
    /// <summary>Returns a list of all license templates</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LicenseTemplateList</returns>
    [ForgejoEndpoint("GET", "/licenses", "Returns a list of all license templates")]
    public Task<LicensesTemplateListEntry[]> ListLicenseTemplatesAsync(CancellationToken cancelToken = default)
        => GetRequest("licenses", cancelToken).JsonResponseAsync(ApiDataSerializerContext.Default.LicensesTemplateListEntryArray, cancelToken);

    /// <summary>Returns information about a license template</summary>
    /// <param name="name">name of the license</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>LicenseTemplateInfo</returns>
    [ForgejoEndpoint("GET", "/licenses/{name}", "Returns information about a license template")]
    public Task<LicenseTemplateInfo> GetLicenseTemplateAsync(string name, CancellationToken cancelToken = default)
        => GetRequest($"licenses/{name}", cancelToken).JsonResponseAsync(ApiDataSerializerContext.Default.LicenseTemplateInfo, cancelToken);
    #endregion

    #region Render
    /// <summary>Render a markdown document as HTML</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>MarkdownRender is a rendered markdown document</returns>
    [ForgejoEndpoint("POST", "/markdown", "Render a markdown document as HTML")]
    public Task<string> RenderMarkdownAsync(MarkdownOption options, CancellationToken cancelToken = default)
        => PostRequest("markdown", options, ApiDataSerializerContext.Default.MarkdownOption, cancelToken).TextResponseAsync(cancelToken);

    /// <summary>Render raw markdown as HTML</summary>
    /// <param name="options">Request body to render</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>MarkdownRender is a rendered markdown document</returns>
    [ForgejoEndpoint("POST", "/markdown/raw", "Render raw markdown as HTML")]
    public Task<string> RenderMarkdownRawAsync(string options, CancellationToken cancelToken = default)
        => PostRequest("markdown/raw", options, ApiDataSerializerContext.Default.String, cancelToken).TextResponseAsync(cancelToken);

    /// <summary>Render a markup document as HTML</summary>
    /// <param name="options"></param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>MarkupRender is a rendered markup document</returns>
    [ForgejoEndpoint("POST", "/markup", "Render a markup document as HTML")]
    public Task<string> RenderMarkupAsync(MarkupOption options, CancellationToken cancelToken = default)
        => PostRequest("markup", options, ApiDataSerializerContext.Default.MarkupOption, cancelToken).TextResponseAsync(cancelToken);
    #endregion

    #region Info
    /// <summary>Returns the nodeinfo of the Forgejo application</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>NodeInfo</returns>
    [ForgejoEndpoint("GET", "/nodeinfo", "Returns the nodeinfo of the Forgejo application")]
    public Task<NodeInfo> GetNodeInfoAsync(CancellationToken cancelToken = default)
        => GetRequest("nodeinfo", cancelToken).JsonResponseAsync(ApiDataSerializerContext.Default.NodeInfo, cancelToken);

    /// <summary>Get default signing-key.gpg</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GPG armored public key</returns>
    [ForgejoEndpoint("GET", "/signing-key.gpg", "Get default signing-key.gpg")]
    public Task<string> GetSigningKeyGpgAsync(CancellationToken cancelToken = default)
        => GetRequest("signing-key.gpg", cancelToken).TextResponseAsync(cancelToken);

    /// <summary>Get default signing-key.ssh</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>SSH public key in OpenSSH authorized key format</returns>
    [ForgejoEndpoint("GET", "/signing-key.ssh", "Get default signing-key.ssh")]
    public Task<string> GetSigningKeySshAsync(CancellationToken cancelToken = default)
        => GetRequest("signing-key.ssh", cancelToken).TextResponseAsync(cancelToken);

    /// <summary>Returns the version of the running application</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>ServerVersion</returns>
    [ForgejoEndpoint("GET", "/version", "Returns the version of the running application")]
    public Task<ServerVersion> GetVersionAsync(CancellationToken cancelToken = default)
        => GetRequest("version", cancelToken).JsonResponseAsync(ApiDataSerializerContext.Default.ServerVersion, cancelToken);
    #endregion

}
