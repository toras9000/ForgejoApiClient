using System.Runtime.CompilerServices;

namespace ForgejoApiClient;

/// <summary>APIフォームデータコンテンツ生成補助クラス</summary>
internal class FormData : MultipartFormDataContent
{
    // 構築
    #region コンストラクタ
    /// <summary>デフォルトコンストラクタ</summary>
    public FormData() : base() { }

    /// <summary>ファイル内容データを追加するコンストラクタ</summary>
    public FormData(Stream content, [CallerArgumentExpression(nameof(content))] string? name = "")
    {
        this.File(content, name);
    }
    #endregion

    // 公開メソッド
    #region コンテンツ
    /// <summary>ファイル内容パラメータを追加する</summary>
    /// <param name="content">ファイル内容ストリーム</param>
    /// <param name="name">ファイル名</param>
    /// <returns>自身のインスタンス</returns>
    public FormData File(Stream content, [CallerArgumentExpression(nameof(content))] string? name = "")
    {
        if (name == null) this.Add(new StreamContent(content));
        else this.Add(new StreamContent(content), name, "file.txt");
        return this;
    }
    #endregion

    #region 型
    /// <summary>HTTPコンテンツを取得する</summary>
    /// <returns>HttpContent型データ</returns>
    public HttpContent AsContent() => this;
    #endregion
}