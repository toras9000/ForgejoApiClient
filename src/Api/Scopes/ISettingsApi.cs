namespace ForgejoApiClient.Api.Scopes;

/// <summary>settings スコープのAPIインタフェース</summary>
/// <remarks>
/// これはAPIアクセスとしては misc パーミッションが適用される模様
/// </remarks>
public interface ISettingsApi : IApiScope
{
    /// <summary>Get instance&apos;s global settings for api</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GeneralAPISettings</returns>
    [ForgejoEndpoint("GET", "/settings/api", "Get instance's global settings for api")]
    public Task<GeneralAPISettings> GetApiSettingsAsync(CancellationToken cancelToken = default)
        => GetRequest("settings/api", cancelToken).JsonResponseAsync<GeneralAPISettings>(cancelToken);

    /// <summary>Get instance&apos;s global settings for Attachment</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GeneralAttachmentSettings</returns>
    [ForgejoEndpoint("GET", "/settings/attachment", "Get instance's global settings for Attachment")]
    public Task<GeneralAttachmentSettings> GetAttachmentSettingsAsync(CancellationToken cancelToken = default)
        => GetRequest("settings/attachment", cancelToken).JsonResponseAsync<GeneralAttachmentSettings>(cancelToken);

    /// <summary>Get instance&apos;s global settings for repositories</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GeneralRepoSettings</returns>
    [ForgejoEndpoint("GET", "/settings/repository", "Get instance's global settings for repositories")]
    public Task<GeneralRepoSettings> GetRepositorySettingsAsync(CancellationToken cancelToken = default)
        => GetRequest("settings/repository", cancelToken).JsonResponseAsync<GeneralRepoSettings>(cancelToken);

    /// <summary>Get instance&apos;s global settings for ui</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>GeneralUISettings</returns>
    [ForgejoEndpoint("GET", "/settings/ui", "Get instance's global settings for ui")]
    public Task<GeneralUISettings> GetUiSettingsAsync(CancellationToken cancelToken = default)
        => GetRequest("settings/ui", cancelToken).JsonResponseAsync<GeneralUISettings>(cancelToken);

}
