using System.Net.Http.Headers;
using System.Text;

namespace ForgejoApiClient.Api;

/// <summary>HTTP関連の拡張メソッド</summary>
public static class HttpExtensions
{
    #region HTTP要求
    /// <summary>HTTP要求にBASIC認証ヘッダを設定する</summary>
    /// <param name="request">HTTP要求</param>
    /// <param name="auth">BASIC認証情報</param>
    /// <returns>HTTP要求自身</returns>
    internal static HttpRequestMessage WithBasicAuth(this HttpRequestMessage request, BasicAuthCredential auth)
    {
        var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{auth.Username}:{auth.Password}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
        return request;
    }
    #endregion

}
