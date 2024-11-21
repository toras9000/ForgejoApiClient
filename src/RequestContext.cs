using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ForgejoApiClient.Api;
using ForgejoApiClient.Converters;

namespace ForgejoApiClient;

/// <summary>空の応答(応答ボディ無し)を期待する際に利用する型</summary>
internal record EmptyResult;

/// <summary>要求コンテキスト型</summary>
/// <remarks>
/// この型はAPI要求の結果を任意の型に解釈するための中継的な役割のクラスとなる。
/// 各APIによって
/// </remarks>
/// <remarks>API要求応答を指定するコンストラクタ</remarks>
/// <param name="requester">API要求デリゲート</param>
internal struct RequestContext(Task<HttpResponseMessage> requester)
{
    // 公開メソッド
    #region 応答処理
    /// <summary>API要求を行い応答をJSONとして解釈して型にマッピングする</summary>
    /// <typeparam name="TResult">応答結果データ型</typeparam>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>API応答データ</returns>
    public readonly Task<TResult> JsonResponseAsync<TResult>(CancellationToken cancelToken)
        => interpretResponseAsync(async (rsp) =>
        {
            // 空の応答の場合もある。空応答を期待する場合はデコードせずに。
            if (typeof(EmptyResult).Equals(typeof(TResult))) return default!;

            // JSON応答を取得
            var json = await rsp.Content.ReadFromJsonAsync<TResult>(options: JsonResponseOptions, cancelToken).ConfigureAwait(false);
            return json!;
        }, cancelToken);

    /// <summary>API要求を行い応答をテキストとして解釈する</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>API応答データ</returns>
    public readonly Task<string> TextResponseAsync(CancellationToken cancelToken)
        => interpretResponseAsync((rsp) => rsp.Content.ReadAsStringAsync(cancelToken), cancelToken);

    /// <summary>API要求を行い応答をストリームとして解釈する</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>API応答データ</returns>
    public readonly Task<ResponseResult<DownloadResult>> DownloadResponseAsync(CancellationToken cancelToken)
        => interpretResponseResultAsync(
            async (rsp) =>
            {
                var fileName = rsp.Content.Headers.ContentDisposition?.FileNameStar ?? rsp.Content.Headers.ContentDisposition?.FileName;
                var fileStream = await rsp.Content.ReadAsStreamAsync().ConfigureAwait(false);
                return new DownloadResult(fileStream, fileName);
            },
            cancelToken
        );

    /// <summary>API要求を行い応答をバイナリとして解釈する</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>API応答データ</returns>
    public readonly Task<byte[]> BinaryResponseAsync(CancellationToken cancelToken)
        => interpretResponseAsync((rsp) => rsp.Content.ReadAsByteArrayAsync(cancelToken), cancelToken);

    /// <summary>API要求を行い応答をステータスコードで解釈する</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>API応答データ</returns>
    public readonly Task<StatusCodeResult> StatusResponseAsync(CancellationToken cancelToken)
        => interpretResponseStatusAsync(cancelToken);
    #endregion

    // 内部フィールド
    #region 定数：シリアル化
    /// <summary>API要求時のJSONシリアライズオプション</summary>
    internal static readonly JsonSerializerOptions JsonRequestOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new EnumJsonConverterFactory(), },
    };

    /// <summary>API応答解釈時のJSONシリアライズオプション</summary>
    internal static readonly JsonSerializerOptions JsonResponseOptions = new JsonSerializerOptions
    {
        Converters = { new EnumJsonConverterFactory(), },
    };
    #endregion

    // 非公開メソッド
    #region 応答処理
    /// <summary>API要求を行いJSON応答を解釈する</summary>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>応答を得るタスク</returns>
    private async readonly Task<StatusCodeResult> interpretResponseStatusAsync(CancellationToken cancelToken)
    {
        // API要求
        using var response = await requester.ConfigureAwait(false);

        // 応答がJSON形式である場合、エラーメッセージの取り出しを試みる。
        var rspMsg = default(string);
        if (response.Content.Headers.ContentType?.MediaType == "application/json")
        {
            try
            {
                // JSONパースしてmessageプロパティ値の取得を試みる
                var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancelToken).ConfigureAwait(false);
                if (json.TryGetProperty("message", out var msgProp) && msgProp.GetString() is string msgText)
                {
                    rspMsg = msgText;
                }
            }
            catch { }
        }

        return new StatusCodeResult(Code: response.StatusCode, Message: rspMsg);
    }

    /// <summary>API要求を行いJSON応答を解釈する</summary>
    /// <typeparam name="TResult">応答データ型</typeparam>
    /// <param name="interpreter">応答の解釈デリゲート</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>応答を得るタスク</returns>
    private async readonly Task<ResponseResult<TResult>> interpretResponseResultAsync<TResult>(Func<HttpResponseMessage, Task<TResult>> interpreter, CancellationToken cancelToken) where TResult : notnull, IDisposable
    {
        // API要求
        var response = await requester.ConfigureAwait(false);

        // 要求が成功したことを確認
        try
        {
            await ensureSuccessResponseAsync(response, cancelToken).ConfigureAwait(false);
        }
        catch
        {
            try { response.Dispose(); } catch { }
            throw;
        }

        // 応答の解釈
        try
        {
            // 応答を解釈
            var result = await interpreter(response).ConfigureAwait(false);
            return new ResponseResult<TResult>(response, result);
        }
        catch (ForgejoClientException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ResponseInterpretException("An error occurred in the interpretation of the response data.", ex);
        }
    }

    /// <summary>API要求を行いJSON応答を解釈する</summary>
    /// <typeparam name="TResult">応答データ型</typeparam>
    /// <param name="interpreter">応答の解釈デリゲート</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>応答を得るタスク</returns>
    private async readonly Task<TResult> interpretResponseAsync<TResult>(Func<HttpResponseMessage, Task<TResult>> interpreter, CancellationToken cancelToken)
    {
        // API要求
        using var response = await requester.ConfigureAwait(false);

        // 要求が成功したことを確認
        await ensureSuccessResponseAsync(response, cancelToken).ConfigureAwait(false);

        // 応答の解釈
        try
        {
            // 応答を解釈
            return await interpreter(response).ConfigureAwait(false);
        }
        catch (ForgejoClientException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ResponseInterpretException("An error occurred in the interpretation of the response data.", ex);
        }
    }

    /// <summary>レスポンス内容が成功したことを確認する</summary>
    /// <param name="response">HTTP応答</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    private async readonly ValueTask ensureSuccessResponseAsync(HttpResponseMessage response, CancellationToken cancelToken)
    {
        // 成功を示す場合は続行
        if (response.IsSuccessStatusCode) return;

        // 応答がJSON形式である場合、エラーメッセージの取り出しを試みる。
        var error = default(Exception);
        if (response.Content.Headers.ContentType?.MediaType == "application/json")
        {
            try
            {
                // JSONパースしてmessageプロパティ値の取得を試みる
                var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancelToken).ConfigureAwait(false);
                if (json.TryGetProperty("message", out var msgProp) && msgProp.GetString() is string msgText)
                {
                    error = new ErrorResponseException(response.StatusCode, msgText);
                }
            }
            catch { }
        }

        // 特定のエラーでない場合は汎用の応答エラー例外
        error ??= new ErrorResponseException(response.StatusCode, response.ReasonPhrase ?? $"HTTP {(int)response.StatusCode}");

        // 例外を送出
        throw error;
    }
    #endregion

}
