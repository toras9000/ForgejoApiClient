#r "nuget: Lestaly, 0.68.0"
#r "nuget: AngleSharp, 1.1.2"
#nullable enable
using System.Threading;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using DocumentFormat.OpenXml.InkML;
using Lestaly;
using Lestaly.Cx;

var settings = new
{
    // サービスのURL
    ServiceURL = "http://localhost:9970",

    // 初回起動セットアップの設定
    Setup = new
    {
        // 初回起動セットアップを試行するか否か
        Perform = true,

        // セットアップ時の admin ユーザ名
        AdminUser = "forgejo-admin",
        // セットアップ時の admin パスワード
        AdminPass = "forgejo-admin-pass",
        // セットアップ時の admin メールアドレス
        AdminMail = "forgejo-admin@example.com",

        // 生成したテストインスタンス情報の保存ファイル
        InstanceInfoFile = ThisSource.RelativeFile("../test/test-forgejo-instance.json"),
    },
};

// docker compose ファイルパス
var composes = new
{
    // サービスのメインcomposeファイル
    ServiceFile = ThisSource.RelativeFile("./docker/compose.yml"),
    // 名前付きボリュームをマウントする追加composeファイル
    MountVolumeFile = ThisSource.RelativeFile("./docker/mount-volume.yml"),
    // ディレクトリをバインドマウントする追加composeファイル
    MountBindFile = ThisSource.RelativeFile("./docker/mount-bind.yml"),
};

record TestUserCredential(string Username, string Password);
record TestServiceInfo(string Url, TestUserCredential Admin, string Token);

await Paved.RunAsync(config: c => c.AnyPause(), action: async () =>
{
    // モードの選択
    var mountMode = Args.Any(a => a.AsSpan().SequenceEqual("--bind-mount"));
    var mountFile = mountMode ? composes.MountBindFile : composes.MountVolumeFile;
    var modeName = mountMode ? "bind-mount" : "volume-mount";

    // サービスの起動
    WriteLine($"Restart service ({modeName}) ...");
    await "docker".args("compose", "--file", composes.ServiceFile.FullName, "down", "--remove-orphans").result().success();
    await "docker".args("compose", "--file", composes.ServiceFile.FullName, "--file", mountFile.FullName, "up", "-d", "--wait").result().success();
    WriteLine("Container up completed.");

    // サービスリンクを表示
    WriteLine();
    WriteLine("Service URL");
    WriteLine($"  {Poster.Link[settings.ServiceURL]}");
    WriteLine();

    // 初回セットアップを試行しない場合はここで終了
    if (!settings.Setup.Perform) return;

    // 初回起動(セットアップフォームが表示されるか)を判別する
    WriteLine();
    WriteLine("Check initialization status ...");
    var config = Configuration.Default.WithDefaultLoader();
    var context = BrowsingContext.New(config);
    // ページ取得。なぜか空の内容が得られる場合があるので、空の場合はリトライする
    var document = default(IDocument);
    using (var breaker = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
    {
        while (document == null || document.Source.Length <= 0)
        {
            if (document != null) await Task.Delay(TimeSpan.FromMilliseconds(200));
            document = await context.OpenAsync(settings.ServiceURL);
        }
    }
    // 未初期化時に存在する要素の取得を試みる
    var container = document.QuerySelector<IHtmlDivElement>(".install-config-container");
    if (container == null)
    {
        // 初回起動画面を検出できない場合はすでにセットアップ済みと思われるので処理を終える
        WriteLine("  The instance has already been initialized.");
        return;
    }
    else
    {
        // 初回起動らしきページを得た場合はセットアップ処理継続
        WriteLine("  Detected that initial setup is required.");
    }

    // セットアップフォームの取得を試みる
    WriteLine("Perform initial setup ...");
    var forms = container.Descendants<IHtmlFormElement>().ToArray();
    if (forms.Length != 1) throw new PavedMessageException("Unexpected setup form");

    // セットアップパラメータを指定して送信
    var setupResult = await forms[0].SubmitAsync(new
    {
        admin_name = settings.Setup.AdminUser,
        admin_email = settings.Setup.AdminMail,
        admin_passwd = settings.Setup.AdminPass,
        admin_confirm_passwd = settings.Setup.AdminPass,
    });
    var loadingElement = setupResult.GetElementById("goto-user-login");
    if (loadingElement == null) throw new PavedMessageException("Unexpected setup result");
    WriteLine("  Setup is complete.");

    // コンテナ内の管理コマンドを実行してAPIトークンを生成する
    WriteLine("Generate access token ...");
    var token = await "docker".args([
        "compose", "--file", composes.ServiceFile.FullName, "exec", "-u", "1000", "app",
        "forgejo", "admin", "user", "generate-access-token",
            "--raw",
            "--username", settings.Setup.AdminUser,
            "--token-name", "api-client-lib-test",
            "--scopes", "all"
    ]).silent().result().success().output();
    WriteLine($"  Token: {token}");

    // テスト用サービスインスタンスの情報をファイルに保存
    var credential = new TestUserCredential(settings.Setup.AdminUser, settings.Setup.AdminPass);
    var instanceInfo = new TestServiceInfo(settings.ServiceURL, credential, token.Trim());
    await settings.Setup.InstanceInfoFile.WriteJsonAsync(instanceInfo);
});
