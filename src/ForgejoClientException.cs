using System.Net;

namespace ForgejoApiClient;

/// <summary>ForgejoClient で利用する例外の基本クラス</summary>
public class ForgejoClientException : Exception
{
    // 構築
    #region コンストラクタ
    /// <summary>デフォルトコンストラクタ</summary>
    public ForgejoClientException() : base() { }

    /// <summary>メッセージを指定するコンストラクタ</summary>
    /// <param name="message">例外メッセージ</param>
    public ForgejoClientException(string? message) : base(message) { }

    /// <summary>例外メッセージと内部例外を指定するコンストラクタ</summary>
    /// <param name="message">例外メッセージ</param>
    /// <param name="innerException">内部例外</param>
    public ForgejoClientException(string? message, Exception? innerException) : base(message, innerException) { }
    #endregion
}

/// <summary>API要求に対するエラー応答を表す例外クラス</summary>
/// <param name="code">HTTPステータスコード</param>
/// <param name="message">エラーメッセージ</param>
public class ErrorResponseException(HttpStatusCode code, string message) : ForgejoClientException(message)
{
    /// <summary>HTTPステータスコード</summary>
    public HttpStatusCode StatusCode { get; } = code;
}

/// <summary>API要求に対する予期しない応答を表す例外クラス</summary>
/// <param name="message">エラーメッセージ</param>
public class UnexpectedResponseException(string message) : ForgejoClientException(message);

/// <summary>応答内容の解釈エラーを表す例外クラス</summary>
/// <param name="message">例外メッセージ</param>
/// <param name="innerException">内部例外</param>
public class ResponseInterpretException(string message, Exception? innerException = null) : ForgejoClientException(message, innerException);
