using System.Net;

namespace ForgejoApiClient.Api;

#pragma warning disable IDE1006 // 命名スタイル

/// <summary>BASIC認証情報</summary>
/// <param name="Username">ユーザ名</param>
/// <param name="Password">パスワード</param>
public record BasicAuthCredential(string Username, string Password);

/// <summary>ページングオプション</summary>
/// <param name="page">ページ番号</param>
/// <param name="limit">ページの最大項目数</param>
public record struct PagingOptions(int? page = default, int? limit = default);

/// <summary>API応答コードを表す結果型</summary>
/// <param name="Code">HTTP応答コード</param>
/// <param name="Message">API応答メッセージ(存在する場合)</param>
public record StatusCodeResult(HttpStatusCode Code, string? Message);

/// <summary>リソースのライフサイクルごと呼び出し元に返す結果データ型</summary>
/// <remarks>
/// この型ではAPI応答のライフサイクルを呼び出し元に任せる。
/// 必要以上にリソースのライフタイムが延命されることを防ぐため、呼び出し元は不要になった時点でこの結果値を破棄するべきとなる。
/// </remarks>
/// <typeparam name="TResult">結果データ型</typeparam>
/// <param name="Result">結果データ</param>
public class ResponseResult<TResult> : IDisposable where TResult : notnull, IDisposable
{
    // 公開プロパティ
    #region データ
    /// <summary>応答データ</summary>
    public TResult Result { get; }
    #endregion

    #region リソース管理
    /// <summary>破棄済みフラグ</summary>
    public bool IsDisposed { get; private set; }
    #endregion

    // 公開メソッド
    #region リソース管理
    /// <summary>リソースの破棄</summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion

    // 保護メソッド
    #region リソース管理
    /// <summary>リソースの破棄</summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.IsDisposed)
        {
            // マネージリソース破棄
            if (disposing)
            {
                this.Result.Dispose();
                this.response.Dispose();
            }

            // アンマネージリソース破棄
            ;

            // 破棄済みフラグ
            this.IsDisposed = true;
        }
    }
    #endregion

    // 構築(内部)
    #region コンストラクタ
    /// <summary>保持するリソースを指定するコンストラクタ</summary>
    /// <param name="rsp">HTTP応答メッセージ</param>
    /// <param name="result"></param>
    internal ResponseResult(HttpResponseMessage rsp, TResult result)
    {
        this.response = rsp;
        this.Result = result;
    }
    #endregion

    // 内部フィールド
    #region コンストラクタ
    /// <summary>HTTP応答メッセージ</summary>
    internal readonly HttpResponseMessage response;
    #endregion
}

/// <summary>ダウンロード応答を表す結果型</summary>
/// <param name="Stream">ダウンロードファイル内容ストリーム</param>
/// <param name="FileName">ダウンロードファイル名候補</param>
public sealed record DownloadResult(Stream Stream, string? FileName) : IDisposable
{
    /// <summary>リソース破棄</summary>
    public void Dispose() => this.Stream.Dispose();
}

/// <summary>チーム検索結果型</summary>
/// <param name="ok">検索成否</param>
/// <param name="data">検索結果</param>
public record TeamSearchResults(bool ok, Team[] data);

/// <summary>ユーザ検索結果型</summary>
/// <param name="ok">検索成否</param>
/// <param name="data">検索結果</param>
public record UserSearchResults(bool ok, User[] data);

/// <summary>アクションランナー登録トークン型</summary>
/// <param name="token">トークン</param>
public record RegistrationTokenResult(string token);

/// <summary>PGP検証パラメータ型</summary>
/// <param name="key_id">GPGキーID</param>
/// <param name="armored_signature">Armor形式のGPG署名</param>
public record VerifyGPGKeyOption(
    string key_id,
    string? armored_signature = default
);

/// <summary>トピック検索結果型</summary>
/// <param name="topics">検索結果</param>
public record TopicSearchResults(TopicResponse[] topics);
