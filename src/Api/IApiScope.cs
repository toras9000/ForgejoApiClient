using System.Buffers.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace ForgejoApiClient.Api;

/// <summary>APIスコープインタフェース</summary>
/// <remarks>
/// スコープごとに分けたAPIメソッド群で利用する共通メソッドを定義するのが目的。
/// </remarks>
public interface IApiScope
{
    // 内部プロパティ
    #region APIアクセス情報
    /// <summary>APIベースURI</summary>
    internal Uri BaseUrl { get; }

    /// <summary>APIアクセス用HTTPクライアント</summary>
    internal HttpClient Http { get; }
    #endregion

    // 内部メソッド
    #region HTTP要求
    /// <summary>APIにGETメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext GetRequest(string endpoint, CancellationToken cancelToken)
        => new RequestContext(this.Http.GetAsync(new Uri(this.BaseUrl, endpoint), cancelToken));

    /// <summary>APIにGETメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <param name="auth">BASIC認証情報</param>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext GetRequest(BasicAuthCredential auth, string endpoint, CancellationToken cancelToken)
    {
        var uri = new Uri(this.BaseUrl, endpoint);
        var request = new HttpRequestMessage(HttpMethod.Get, uri).WithBasicAuth(auth);
        return new RequestContext(this.Http.SendAsync(request, cancelToken));
    }

    /// <summary>APIにPOSTメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext PostRequest(string endpoint, CancellationToken cancelToken)
        => new RequestContext(this.Http.PostAsync(new Uri(this.BaseUrl, endpoint), default, cancelToken));

    /// <summary>APIにJSON-BODYのPOSTメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <typeparam name="TArg">要求データの型</typeparam>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="content">要求データ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext PostRequest(string endpoint, HttpContent content, CancellationToken cancelToken)
        => new RequestContext(this.Http.PostAsync(new Uri(this.BaseUrl, endpoint), content, cancelToken));

    /// <summary>APIにJSON-BODYのPOSTメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <typeparam name="TArg">要求データの型</typeparam>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="args">要求データ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext PostRequest<TArg>(string endpoint, TArg args, CancellationToken cancelToken)
        => new RequestContext(this.Http.PostAsJsonAsync(new Uri(this.BaseUrl, endpoint), args, RequestContext.JsonRequestOptions, cancelToken));

    /// <summary>APIにJSON-BODYのPOSTメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <typeparam name="TArg">要求データの型</typeparam>
    /// <param name="auth">BASIC認証情報</param>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="args">要求データ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext PostRequest<TArg>(BasicAuthCredential auth, string endpoint, TArg args, CancellationToken cancelToken)
    {
        var uri = new Uri(this.BaseUrl, endpoint);
        var request = new HttpRequestMessage(HttpMethod.Post, uri).WithBasicAuth(auth);
        request.Content = JsonContent.Create(args, options: RequestContext.JsonRequestOptions);
        return new RequestContext(this.Http.SendAsync(request, cancelToken));
    }

    /// <summary>APIにPUTメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext PutRequest(string endpoint, CancellationToken cancelToken)
        => new RequestContext(this.Http.PutAsync(new Uri(this.BaseUrl, endpoint), default, cancelToken));

    /// <summary>APIにJSON-BODYのPUTメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <typeparam name="TArg">要求データの型</typeparam>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="args">要求データ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext PutRequest<TArg>(string endpoint, TArg args, CancellationToken cancelToken)
        => new RequestContext(this.Http.PutAsJsonAsync(new Uri(this.BaseUrl, endpoint), args, RequestContext.JsonRequestOptions, cancelToken));

    /// <summary>APIにPATCHメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext PatchRequest(string endpoint, CancellationToken cancelToken)
        => new RequestContext(this.Http.PatchAsync(new Uri(this.BaseUrl, endpoint), default, cancelToken));

    /// <summary>APIにJSON-BODYのPATCHメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <typeparam name="TArg">要求データの型</typeparam>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="args">要求データ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext PatchRequest<TArg>(string endpoint, TArg args, CancellationToken cancelToken)
        => new RequestContext(this.Http.PatchAsJsonAsync(new Uri(this.BaseUrl, endpoint), args, RequestContext.JsonRequestOptions, cancelToken));

    /// <summary>APIにDELETEメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext DeleteRequest(string endpoint, CancellationToken cancelToken)
        => new RequestContext(this.Http.DeleteAsync(new Uri(this.BaseUrl, endpoint), cancelToken));

    /// <summary>APIにDELETEメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <param name="auth">BASIC認証情報</param>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext DeleteRequest(BasicAuthCredential auth, string endpoint, CancellationToken cancelToken)
    {
        var uri = new Uri(this.BaseUrl, endpoint);
        var request = new HttpRequestMessage(HttpMethod.Delete, uri).WithBasicAuth(auth);
        return new RequestContext(this.Http.SendAsync(request, cancelToken));
    }

    /// <summary>APIにJSON-BODYのDELETEメソッドアクセスを行う要求コンテキストを作成する</summary>
    /// <typeparam name="TArg">要求データの型</typeparam>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="args">要求データ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>要求コンテキスト</returns>
    internal RequestContext DeleteRequest<TArg>(string endpoint, TArg args, CancellationToken cancelToken)
    {
        var uri = new Uri(this.BaseUrl, endpoint);
        var request = new HttpRequestMessage(HttpMethod.Delete, uri) { Content = JsonContent.Create(args, options: RequestContext.JsonRequestOptions), };
        return new RequestContext(this.Http.SendAsync(request, cancelToken));
    }
    #endregion
}
