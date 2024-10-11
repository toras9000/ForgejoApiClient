namespace ForgejoApiClient;

/// <summary>Forgejo APIエンドポイントの情報を示す属性</summary>
/// <remarks>
/// この属性はAPIクライアントメソッドと対応するエンドポイントの情報を保持することだけを目的とする。
/// APIクライアントとしての実行処理では一切利用されない。
/// コード生成スクリプトでは生成コードの更新位置を検出するマーカとして利用される。
/// </remarks>
/// <param name="method">HTTPメソッド</param>
/// <param name="endpoint">エンドポイントパス</param>
/// <param name="description">API概要</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ForgejoEndpointAttribute(string method, string endpoint, string description) : Attribute
{
    /// <summary>HTTPメソッド</summary>
    public string Method => method;

    /// <summary>エンドポイントパス</summary>
    public string Endpoint => endpoint;

    /// <summary>API概要</summary>
    public string Description => description;
}

/// <summary>スクリプト生成コードに対して手動編集したことを示す属性</summary>
/// <remarks>
/// この属性はスクリプトで生成したコードに対し、自動生成では対処できない部分を手動編集したことを示す目印として利用する。
/// バージョンアップ時などにはスクリプトでのコード再生成によりまた手動編集前の状態に戻ってしまうが、その際にAPI仕様変更による差分なのか、手動編集箇所に対する差分なのかを判別するための情報とすることを想定している。
/// APIクライアントとしての実行処理では一切利用されない。
/// </remarks>
/// <param name="description">手動編集のメモ</param>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class ManualEditAttribute(string description = "") : Attribute
{
    public string Description => description;
}

/// <summary>列挙値をJSONで文字列表現する際のマッピング情報属性</summary>
/// <param name="name">JSON文字列表現</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class MapEnumAttribute(string name) : Attribute
{
    /// <summary>JSON文字列表現</summary>
    public string Name { get; } = name;
}
