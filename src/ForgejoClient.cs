using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using ForgejoApiClient.Api;
using ForgejoApiClient.Api.Scopes;

namespace ForgejoApiClient;

/// <summary>Forgejo APIクライアント</summary>
[RequiresDynamicCode("Use dynamic creation converter.")]
public class ForgejoClient : IApiClient, IDisposable
{
    // 構築
    #region コンストラクタ
    /// <summary>コンストラクタ</summary>
    /// <param name="baseUri">APIベースURI</param>
    /// <param name="token">APIトークン</param>
    /// <param name="handler">HTTPメッセージハンドラ</param>
    public ForgejoClient(Uri baseUri, string token, HttpMessageHandler? handler = default)
    {
        ArgumentNullException.ThrowIfNull(baseUri);

        // APIリクエストベースアドレスを保持
        const string apiBasePath = "/api/v1";
        var absUri = baseUri.AbsoluteUri.AsSpan().TrimEnd('/');
        this.BaseUri = absUri.EndsWith(apiBasePath) ? new Uri($"{absUri}/") : new Uri($"{absUri}{apiBasePath}/");

        // トークンの保持
        this.apiToken = token;

        // HTTPクライアントオブジェクト作成
        this.httpHandler = handler ?? new HttpClientHandler();

        // デフォルトの(トークンの)ユーザコンテキストで呼び出しを行うクライアントオブジェクト
        this.core = new ForgejoClientCore(this, default);
    }
    #endregion

    // 公開プロパティ
    #region APIアクセス情報
    /// <summary>APIベースURI</summary>
    public Uri BaseUri { get; }
    #endregion

    #region 設定
    /// <summary>要求のタイムアウト時間</summary>
    public TimeSpan Timeout
    {
        get => this.core.Timeout;
        set => this.core.Timeout = value;
    }
    #endregion

    #region 状態
    /// <summary>破棄済フラグ</summary>
    public bool IsDisposed { get; private set; }
    #endregion

    #region APIカテゴリ
    /// <summary>admin スコープのAPIインタフェース</summary>
    public IAdminApi Admin => this.core.Admin;

    /// <summary>issue スコープのAPIインタフェース</summary>
    public IIssueApi Issue => this.core.Issue;

    /// <summary>misc スコープのAPIインタフェース</summary>
    public IMiscellaneousApi Miscellaneous => this.core.Miscellaneous;

    /// <summary>notification スコープのAPIインタフェース</summary>
    public INotificationApi Notification => this.core.Notification;

    /// <summary>organization スコープのAPIインタフェース</summary>
    public IOrganizationApi Organization => this.core.Organization;

    /// <summary>package スコープのAPIインタフェース</summary>
    public IPackageApi Package => this.core.Package;

    /// <summary>repository スコープのAPIインタフェース</summary>
    public IRepositoryApi Repository => this.core.Repository;

    /// <summary>settings スコープのAPIインタフェース</summary>
    public ISettingsApi Settings => this.core.Settings;

    /// <summary>user スコープのAPIインタフェース</summary>
    public IUserApi User => this.core.User;
    #endregion

    // 公開メソッド
    #region APIコンテキスト
    /// <summary>別ユーザコンテキストのAPIクライアント型を生成する</summary>
    /// <remarks>
    /// 返却されるクライアントはベースとなるインスタンスのライフサイクルに依存する。
    /// ベースインスタンスを破棄すると、別ユーザコンテキストインスタンスも無効となる。
    /// </remarks>
    /// <param name="user">対象ユーザ名</param>
    /// <returns>別ユーザコンテキストのAPIクライアント</returns>
    public ISudoApiClient Sudo(string user)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(user);
        return new ForgejoClientCore(this, user);
    }
    #endregion

    #region 破棄
    /// <summary>リソース破棄</summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion

    // 保護メソッド
    #region 破棄
    /// <summary>リソース破棄</summary>
    /// <param name="disposing">マネージ破棄過程であるか否か</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.IsDisposed)
        {
            // マネージ破棄過程であればマネージオブジェクトを破棄する
            if (disposing)
            {
                this.core?.Dispose();
                this.httpHandler?.Dispose();
            }

            // 破棄済みマーク
            this.IsDisposed = true;
        }
    }
    #endregion

    // 非公開型
    #region HTTP
    /// <summary>Forgejo APIアクセス用ヘッダを設定するメッセージハンドラ</summary>
    /// <param name="outer">APIクライアントインスタンス</param>
    /// <param name="user">ユーザコンテキスト。nullの場合はAPIトークンのユーザコンテキスト</param>
    private class ForgejoMessageHandler(ForgejoClient outer, string? user) : DelegatingHandler(outer.httpHandler)
    {
        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 当初リクエスト処理レイヤーを設けて共通の取り扱いができるかと思いこれを用意したのだが、
            // APIキー系など、APIエンドポイントごとに異なる認証を必要とする場合があったため、
            // 現在では、DelegatingHandlerを利用するよりもHttpRequestMessageを生成する箇所に自前の処理レイヤーを入れた方が良さそうな感じがしている。
            // ただ、まだそのあたりを精査できていない。内部的な話であり公開I/Fとは切り離して考えられる部分なのであと回す。

            if (request.Headers.Authorization == null) request.Headers.Authorization = new AuthenticationHeaderValue("token", $"{outer.apiToken}");
            if (user != null) request.Headers.Add("Sudo", user);
            return base.SendAsync(request, cancellationToken);
        }
    }
    #endregion

    #region APIクライアント
    /// <summary>APIクライアントコア</summary>
    private class ForgejoClientCore : ISudoApiClient
    {
        // 構築
        #region コンストラクタ
        /// <summary>コンテキスト情報を指定するコンストラクタ</summary>
        /// <param name="outer">APIクライアントインスタンス</param>
        /// <param name="user">ユーザコンテキスト。nullの場合はAPIトークンのユーザコンテキスト</param>
        public ForgejoClientCore(ForgejoClient outer, string? user)
        {
            // ユーザコンテキストに応じたクライアントの初期化
            this.outer = outer;
            if (user == null)
            {
                this.SudoUser = "";
                this.http = new HttpClient(new ForgejoMessageHandler(outer, null), disposeHandler: true);
            }
            else
            {
                this.SudoUser = user;
                this.http = new HttpClient(new ForgejoMessageHandler(outer, user), disposeHandler: false);
            }

            // スコープごとのAPIインタフェースオブジェクトを生成
            this.Admin = new AdminApi(this);
            this.Issue = new IssueApi(this);
            this.Miscellaneous = new MiscellaneousApi(this);
            this.Notification = new NotificationApi(this);
            this.Organization = new OrganizationApi(this);
            this.Repository = new RepositoryApi(this);
            this.Package = new PackageApi(this);
            this.Settings = new SettingsApi(this);
            this.User = new UserApi(this);
        }
        #endregion

        // 公開プロパティ
        #region API情報
        /// <summary>APIベースURI</summary>
        public Uri BaseUri => outer.BaseUri;

        /// <summary>コンテキストユーザ</summary>
        public string SudoUser { get; }
        #endregion

        #region 設定
        /// <summary>要求のタイムアウト時間</summary>
        public TimeSpan Timeout
        {
            get => this.http.Timeout;
            set => this.http.Timeout = value;
        }
        #endregion

        #region 状態
        /// <summary>破棄済フラグ</summary>
        public bool IsDisposed => this.disposed || outer.IsDisposed;
        #endregion

        #region APIカテゴリ
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

        // 公開メソッド
        #region 破棄
        /// <summary>リソースの破棄</summary>
        public void Dispose()
        {
            this.http.Dispose();
            this.disposed = true;
        }
        #endregion

        // 非公開型
        #region APIインタフェース実装
        /// <summary>APIスコープインタフェース実装用の共通ベース型</summary>
        /// <param name="core">リソースを提供するためのAPI実装インスタンス</param>
        private class ApiScopeBase(ForgejoClientCore core) : IApiScope
        {
            Uri IApiScope.BaseUrl => core.BaseUri;
            HttpClient IApiScope.Http => core.http;
        }

        /// <summary>admin スコープのAPIインタフェース実装クラス</summary>
        private class AdminApi(ForgejoClientCore client) : ApiScopeBase(client), IAdminApi;

        /// <summary>issue スコープのAPIインタフェース実装クラス</summary>
        private class IssueApi(ForgejoClientCore client) : ApiScopeBase(client), IIssueApi;

        /// <summary>misc スコープのAPIインタフェース実装クラス</summary>
        private class MiscellaneousApi(ForgejoClientCore client) : ApiScopeBase(client), IMiscellaneousApi;

        /// <summary>notification スコープのAPIインタフェース実装クラス</summary>
        private class NotificationApi(ForgejoClientCore client) : ApiScopeBase(client), INotificationApi;

        /// <summary>organization スコープのAPIインタフェース実装クラス</summary>
        private class OrganizationApi(ForgejoClientCore client) : ApiScopeBase(client), IOrganizationApi;

        /// <summary>package スコープのAPIインタフェース実装クラス</summary>
        private class PackageApi(ForgejoClientCore client) : ApiScopeBase(client), IPackageApi;

        /// <summary>repository スコープのAPIインタフェース実装クラス</summary>
        private class RepositoryApi(ForgejoClientCore client) : ApiScopeBase(client), IRepositoryApi;

        /// <summary>settings スコープのAPIインタフェース実装クラス</summary>
        private class SettingsApi(ForgejoClientCore client) : ApiScopeBase(client), ISettingsApi;

        /// <summary>user スコープのAPIインタフェース実装クラス</summary>
        private class UserApi(ForgejoClientCore client) : ApiScopeBase(client), IUserApi;
        #endregion

        #region リソース
        /// <summary>HTTPクライアント</summary>
        private readonly HttpClient http;

        /// <summary>APIクライアント</summary>
        private readonly ForgejoClient outer;
        #endregion

        #region 状態
        /// <summary>破棄済みフラグ</summary>
        private bool disposed;
        #endregion
    }
    #endregion

    // 非公開フィールド
    #region リソース
    /// <summary>要求を処理させるHTTPメッセージハンドラ</summary>
    private readonly HttpMessageHandler httpHandler;

    /// <summary>トークンコンテキストのAPIクライアントオブジェクト</summary>
    private readonly ForgejoClientCore core;
    #endregion

    #region API情報
    /// <summary>APIトークン</summary>
    private readonly string apiToken;
    #endregion

}
