using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using ForgejoApiClient.Api;

namespace ForgejoApiClient;

/// <summary>APIエンドポイントのクエリ文字列生成用補助クラス</summary>
internal struct QueryBuilder
{
    // 構築
    #region コンストラクタ
    /// <summary>構築ベース情報を指定するコンストラクタ</summary>
    /// <param name="path">ベースとなるAPIエンドポイントパス</param>
    /// <param name="append">ベースパスにクエリを追加する(既にクエリが含まれている)か否か。</param>
    public QueryBuilder(string path, bool append = false)
        : this(append)
    {
        this.builder.Append(path.AsSpan().TrimStart('/'));
    }

    /// <summary>構築ベース情報を指定するコンストラクタ</summary>
    /// <param name="handler">ベースとなるAPIエンドポイントパスの文字列補間ハンドラ</param>
    /// <param name="append">ベースパスにクエリを追加する(既にクエリが含まれている)か否か。</param>
    public QueryBuilder(ref StringBuilder.AppendInterpolatedStringHandler handler, bool append = false)
        : this(append)
    {
        this.builder.Append(CultureInfo.InvariantCulture, ref handler);
    }
    #endregion

    // 公開メソッド
    #region クエリ
    /// <summary>クエリパラメータを追加する</summary>
    /// <typeparam name="T">パラメータ型</typeparam>
    /// <param name="name">パラメータ名</param>
    /// <param name="value">パラメータ値</param>
    /// <returns>自身のインスタンス</returns>
    public QueryBuilder Append<T>(string name, T value)
    {
        this.builder.Append(this.append ? '&' : '?');
        this.append = true;
        this.builder.Append(name);
        this.builder.Append('=');
        this.builder.Append(CultureInfo.InvariantCulture, $"{value}");
        return this;
    }
    #endregion

    #region 文字列
    /// <summary>クエリ文字列を取得する</summary>
    /// <returns>クエリ文字列</returns>
    public override readonly string ToString()
        => this.builder.ToString();
    #endregion

    // 演算子オーバロード
    #region 演算子
    /// <summary>文字列への暗黙変換オペレータ</summary>
    /// <param name="query">クエリビルダインスタンス</param>
    public static implicit operator string(QueryBuilder query) => query.ToString();
    #endregion

    // 構築(非公開)
    #region コンストラクタ
    /// <summary>基本的な初期化を行うコンストラクタ</summary>
    /// <param name="append">ベースパスにクエリを追加する(既にクエリが含まれている)か否か。</param>
    private QueryBuilder(bool append)
    {
        this.append = append;
        this.builder = new StringBuilder(capacity: 256);
    }
    #endregion

    // 非公開フィールド
    #region コンストラクタ
    /// <summary>文字列バッファ</summary>
    private readonly StringBuilder builder;

    /// <summary>クエリ追加(バッファ内でクエリ開始済み)状態</summary>
    private bool append;
    #endregion
}

/// <summary>QueryBuilder の拡張メソッド</summary>
internal static class QueryBuilderExtensions
{
    /// <summary>ページングパラメータを追加したQueryBuilderを生成する</summary>
    /// <param name="self">ベースとする文字列</param>
    /// <param name="paging">ページングオプション</param>
    /// <returns>生成したQueryBuilderインスタンス</returns>
    public static QueryBuilder WithQuery(this string self, PagingOptions paging)
        => new QueryBuilder(self).Param(paging);

    /// <summary>ページングパラメータを追加したQueryBuilderを生成する</summary>
    /// <param name="self">ベースとする文字列</param>
    /// <param name="paging">ページングオプション</param>
    /// <returns>生成したQueryBuilderインスタンス</returns>
    public static QueryBuilder WithQuery(this ref StringBuilder.AppendInterpolatedStringHandler self, PagingOptions paging)
        => new QueryBuilder(ref self).Param(paging);

    /// <summary>パラメータを追加したQueryBuilderを生成する</summary>
    /// <typeparam name="T">追加するパラメータ値の型</typeparam>
    /// <param name="self">ベースとする文字列</param>
    /// <param name="value">追加するパラメータ値。この引数に指定した式表現がパラメータ名として扱われる。パラメータ名と同じ名前の変数を渡すことを想定している。</param>
    /// <param name="name">valueパラメータに指定した式の文字列表現。パラメータ名として利用する。</param>
    /// <returns>生成したQueryBuilderインスタンス</returns>
    public static QueryBuilder WithQuery<T>(this string self, T? value, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        var builder = new QueryBuilder(self);
        return value == null ? builder : builder.Append(name, value);
    }

    /// <summary>パラメータを追加したQueryBuilderを生成する</summary>
    /// <typeparam name="T">追加するパラメータ値の型</typeparam>
    /// <param name="self">ベースとする文字列</param>
    /// <param name="value">追加するパラメータ値。この引数に指定した式表現がパラメータ名として扱われる。パラメータ名と同じ名前の変数を渡すことを想定している。</param>
    /// <param name="name">valueパラメータに指定した式の文字列表現。パラメータ名として利用する。</param>
    /// <returns>生成したQueryBuilderインスタンス</returns>
    public static QueryBuilder WithQuery<T>(this ref StringBuilder.AppendInterpolatedStringHandler self, T value, [CallerArgumentExpression(nameof(value))] string name = "")
        => new QueryBuilder(ref self).Append(name, value);

    /// <summary>QueryBuilderにページングパラメータを追加する</summary>
    /// <param name="self">対象QueryBuilder</param>
    /// <param name="paging">ページングオプション</param>
    /// <returns>対象QueryBuilder自身</returns>
    public static QueryBuilder Param(in this QueryBuilder self, PagingOptions paging)
    {
        if (paging.page != null) self.Append("page", paging.page);
        if (paging.limit != null) self.Append("limit", paging.limit);
        return self;
    }

    /// <summary>QueryBuilderにパラメータを追加する</summary>
    /// <typeparam name="T">追加するパラメータ値の型</typeparam>
    /// <param name="self">対象QueryBuilder</param>
    /// <param name="value">追加するパラメータ値。この引数に指定した式表現がパラメータ名として扱われる。パラメータ名と同じ名前の変数を渡すことを想定している。</param>
    /// <param name="name">valueパラメータに指定した式の文字列表現。パラメータ名として利用する。</param>
    /// <returns>対象QueryBuilder自身</returns>
    public static QueryBuilder Param<T>(in this QueryBuilder self, T? value, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        return value == null ? self : self.Append(name, value);
    }

    /// <summary>QueryBuilderにパラメータを追加する</summary>
    /// <param name="self">対象QueryBuilder</param>
    /// <param name="value">追加するパラメータ値。この引数に指定した式表現がパラメータ名として扱われる。パラメータ名と同じ名前の変数を渡すことを想定している。</param>
    /// <param name="name">valueパラメータに指定した式の文字列表現。パラメータ名として利用する。</param>
    /// <returns>対象QueryBuilder自身</returns>
    public static QueryBuilder Param(in this QueryBuilder self, DateTime? value, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        return value == null ? self : self.Append(name, $"{value.Value.ToUniversalTime():yyyy-MM-dd'T'HH:mm:ss'Z'}");
    }

    /// <summary>QueryBuilderにパラメータを追加する</summary>
    /// <param name="self">対象QueryBuilder</param>
    /// <param name="value">追加するパラメータ値。この引数に指定した式表現がパラメータ名として扱われる。パラメータ名と同じ名前の変数を渡すことを想定している。</param>
    /// <param name="name">valueパラメータに指定した式の文字列表現。パラメータ名として利用する。</param>
    /// <returns>対象QueryBuilder自身</returns>
    public static QueryBuilder Param(in this QueryBuilder self, DateTimeOffset? value, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        return value == null ? self : self.Append(name, $"{value.Value.ToUniversalTime():yyyy-MM-dd'T'HH:mm:ss'Z'}");
    }
}