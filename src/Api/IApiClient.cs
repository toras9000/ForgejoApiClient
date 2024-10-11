using ForgejoApiClient.Api.Scopes;

namespace ForgejoApiClient.Api;

/// <summary>APIクライアント インタフェース</summary>
public interface IApiClient : IDisposable
{
    #region APIアクセス情報
    /// <summary>APIベースURI</summary>
    public Uri BaseUri { get; }
    #endregion

    #region 状態
    /// <summary>破棄済フラグ</summary>
    public bool IsDisposed { get; }
    #endregion

    #region APIカテゴリ
    /// <summary>activitypub スコープのAPIインタフェース</summary>
    public IActivityPubApi ActivityPub { get; }

    /// <summary>admin スコープのAPIインタフェース</summary>
    public IAdminApi Admin { get; }

    /// <summary>issue スコープのAPIインタフェース</summary>
    public IIssueApi Issue { get; }

    /// <summary>misc スコープのAPIインタフェース</summary>
    public IMiscellaneousApi Miscellaneous { get; }

    /// <summary>notification スコープのAPIインタフェース</summary>
    public INotificationApi Notification { get; }

    /// <summary>organization スコープのAPIインタフェース</summary>
    public IOrganizationApi Organization { get; }

    /// <summary>package スコープのAPIインタフェース</summary>
    public IPackageApi Package { get; }

    /// <summary>repository スコープのAPIインタフェース</summary>
    public IRepositoryApi Repository { get; }

    /// <summary>settings スコープのAPIインタフェース</summary>
    public ISettingsApi Settings { get; }

    /// <summary>user スコープのAPIインタフェース</summary>
    public IUserApi User { get; }
    #endregion
}

/// <summary>ユーザコンテキストを変更したAPIクライアントインタフェース</summary>
public interface ISudoApiClient : IApiClient
{
    #region APIアクセス情報
    /// <summary>APIコンテキストユーザ名</summary>
    public string SudoUser { get; }
    #endregion
}
