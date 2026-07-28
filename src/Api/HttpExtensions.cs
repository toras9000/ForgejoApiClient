using System.Net.Http.Headers;
using System.Text;

namespace ForgejoApiClient.Api;

/// <summary>HTTP関連の拡張メソッド</summary>
public static class HttpExtensions
{
    #region HTTP要求
    /// <summary>HTTP要求に認証ヘッダを設定する</summary>
    /// <param name="request">HTTP要求</param>
    /// <param name="auth">HTTP認証情報</param>
    /// <returns>HTTP要求自身</returns>
    internal static HttpRequestMessage WithAuthentication(this HttpRequestMessage request, IHttpAuthentication auth)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, auth.Parameters);
        return request;
    }
    #endregion

}
