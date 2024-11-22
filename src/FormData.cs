using System.Runtime.CompilerServices;

namespace ForgejoApiClient;

/// <summary>APIフォームデータコンテンツ生成補助クラス</summary>
internal class FormData : MultipartFormDataContent
{
    // 構築
    #region コンストラクタ
    /// <summary>デフォルトコンストラクタ</summary>
    public FormData() : base() { }
    #endregion

    // 公開メソッド
    #region コンテンツ
    /// <summary>ファイル内容パラメータを追加する</summary>
    /// <param name="content">ファイル内容ストリーム</param>
    /// <param name="name">パラメータ名</param>
    /// <returns>自身のインスタンス</returns>
    public FormData File(Stream? content, [CallerArgumentExpression(nameof(content))] string? name = "")
    {
        if (content != null)
        {
            if (name == null) this.Add(new StreamContent(content));
            else this.Add(new StreamContent(content), name, "file.txt");
        }
        return this;
    }

    /// <summary>文字列パラメータを追加する</summary>
    /// <param name="value">文字列パラメータ値</param>
    /// <param name="name">パラメータ名</param>
    /// <returns>自身のインスタンス</returns>
    public FormData Scalar(string? value, [CallerArgumentExpression(nameof(value))] string? name = "")
    {
        if (value != null)
        {
            if (name == null) this.Add(new StringContent(value));
            else this.Add(new StringContent(value), name);
        }
        return this;
    }
    #endregion

    #region 型
    /// <summary>HTTPコンテンツを取得する</summary>
    /// <returns>HttpContent型データ</returns>
    public HttpContent AsContent() => this;
    #endregion
}