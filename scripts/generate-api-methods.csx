#r "nuget: NSwag.CodeGeneration.CSharp, 14.4.0"
#r "nuget: Humanizer.Core, 2.14.1"
#r "nuget: Lestaly, 0.84.0"
#r "nuget: Kokuban, 0.2.0"
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Text.RegularExpressions;
using Humanizer;
using Kokuban;
using Lestaly;
using Namotion.Reflection;
using NJsonSchema.CodeGeneration.CSharp;
using NJsonSchema.CodeGeneration.CSharp.Models;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

// SwaggerファイルからAPIを呼び出すメソッドソースコードを生成する。
// 生成するのは別途定義済みの型と組み合わせる、このクライアント固有のコード。
// このコード生成の一番の目的は、APIのバージョンアップ時にどこが変化したのかを把握できること。
// (新しいバージョンを参照した再生成によって差分確認を可能とする)
// ただし、Swaggerを強く尊重するわけではなく、生成結果には手を加えて好みの形に調整する。(特に明確な型名が付いていないJSON返却データなどはそうせざるをえない。)
// 調整した箇所には ManualEdit 属性を付与し、API変化による変化と区別可能にする想定としている。

var config = new
{
    // Swaggerファイル取得URL
    ForgejoSwaggerUrl = @"http://localhost:9970/swagger.v1.json",

    // コードの格納ディレクトリ
    CodeDirectory = ThisSource.RelativeDirectory("../src/Api/Scopes"),

    // コードの保存エンコーディング
    CodeEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true),

    // スコープ名と生成物の名称マップ
    ScopeMapping = new[]
    {
        new { Scope = "activitypub",    Name = "ActivityPub",   },
        new { Scope = "admin",          Name = "Admin",         },
        new { Scope = "miscellaneous",  Name = "Miscellaneous", },
        new { Scope = "notification",   Name = "Notification",  },
        new { Scope = "organization",   Name = "Organization",  },
        new { Scope = "package",        Name = "Package",       },
        new { Scope = "issue",          Name = "Issue",         },
        new { Scope = "repository",     Name = "Repository",    },
        new { Scope = "settings",       Name = "Settings",      },
        new { Scope = "user",           Name = "User",          },
    },
};

return await Paved.ProceedAsync(async () =>
{
    // Swaggerファイルの読み込み
    WriteLine("Get swagger");
    var apiDoc = await OpenApiDocument.FromUrlAsync(new(config.ForgejoSwaggerUrl));

    // コード生成の準備
    WriteLine("Collect API operation info");

    // 生成の設定
    var settings = new CSharpClientGeneratorSettings();
    settings.CSharpGeneratorSettings.DateType = "DateTimeOffset";
    settings.CSharpGeneratorSettings.DateTimeType = "DateTimeOffset";
    settings.CSharpGeneratorSettings.TimeType = "TimeSpan";
    settings.CSharpGeneratorSettings.TimeSpanType = "TimeSpan";
    settings.CSharpGeneratorSettings.ArrayType = "ICollection";
    settings.CSharpGeneratorSettings.ArrayInstanceType = "List";
    settings.CSharpGeneratorSettings.ArrayBaseType = "List";
    settings.CSharpGeneratorSettings.DictionaryType = "IDictionary";
    settings.CSharpGeneratorSettings.DictionaryInstanceType = "Dictionary";
    settings.CSharpGeneratorSettings.DictionaryBaseType = "Dictionary";
    settings.CSharpGeneratorSettings.GenerateOptionalPropertiesAsNullable = true;
    settings.CSharpGeneratorSettings.GenerateNullableReferenceTypes = true;
    settings.ResponseArrayType = "ICollection";
    settings.ResponseDictionaryType = "IDictionary";
    settings.ParameterArrayType = "ICollection";
    settings.ParameterDictionaryType = "IDictionary";

    // 型リゾルバに定義された型と、プロパティで利用されている列挙型を登録する
    var resolver = new CSharpTypeResolver(settings.CSharpGeneratorSettings);
    resolver.RegisterSchemaDefinitions(apiDoc.Definitions);

    // コード生成処理
    var generator = new CSharpClientGenerator(apiDoc, settings, resolver);

    // 生成用の情報をまとめる
    var knownTypes = resolver.Types.Values.ToHashSet();
    var context = new GenerationContext(generator, resolver, knownTypes);

    // APIエンドポイント情報を収集
    var endpoints = apiDoc.Operations
        .Select(desc => context.InterpretApiEndpoint(desc))
        .Where(ep => !ep.Deprecated)
        .ToArray();

    // スコープ名の一覧を作成
    var scopeComp = StringComparer.OrdinalIgnoreCase;
    var scopes = endpoints.SelectMany(e => e.Categories).Distinct().ToHashSet(scopeComp);

    // スコープ毎のコード生成
    WriteLine("Generate operation code:");
    foreach (var map in config.ScopeMapping)
    {
        WriteLine(Chalk.Cyan[$"Scope '{map.Scope}'"]);

        // 処理対象スコープ名を一覧から取り除き。あとで未処理スコープ名を検出するために使う。
        var removed = scopes.Remove(map.Scope);
        if (!removed)
        {
            WriteLine(Chalk.Yellow[$".. Not found scope"]);
            continue;
        }

        // コードの生成
        try
        {
            // APIスコープと対応する生成識別子名を取得。
            var scopeApis = endpoints.Where(ep => ep.Categories.Any(c => scopeComp.Equals(c, map.Scope)));

            // 該当識別名のファイルが既に存在すれば更新を。なければ新規作成。
            var codeFile = config.CodeDirectory.RelativeFile($"I{map.Name}Api.cs");
            if (codeFile.Exists)
            {
                updateApiCode(map.Name, codeFile, config.CodeEncoding, scopeApis);
            }
            else
            {
                createApiCode(map.Name, codeFile, config.CodeEncoding, scopeApis);
            }
        }
        catch (Exception ex)
        {
            WriteLine(Chalk.Red[$".. {ex.Message}"]);
        }
    }

    // スコープ毎のコード生成
    if (0 < scopes.Count)
    {
        WriteLine("Unprocessed scopes:");
        foreach (var scope in scopes)
        {
            WriteLine(Chalk.Yellow[$"Scope '{scope}'"]);
        }
    }

});

/// <summary>APIパラメータ情報</summary>
/// <param name="Kind">パラメータの種類</param>
/// <param name="Name">パラメータ名称 (変数名には使えない)</param>
/// <param name="Variable">パラメータ変数名</param>
/// <param name="Description">パラメータの説明</param>
/// <param name="Type">パラメータの型</param>
/// <param name="Required">必須パラメータであるか</param>
record ApiParameter(OpenApiParameterKind Kind, string Name, string Variable, string Description, string Type, bool Required);

/// <summary>API結果値情報</summary>
/// <param name="Type">結果値の型</param>
/// <param name="Description">結果値の説明</param>
record ApiResult(string Type, string Description);

/// <summary>APIエンドポイント情報</summary>
/// <param name="Categories">APIカテゴリ(スコープ)</param>
/// <param name="Path">APIエンドポイントパス</param>
/// <param name="Method">API呼び出しHTTPメソッド</param>
/// <param name="Deprecated">廃止APIであるか否か</param>
/// <param name="Summary">API概要</param>
/// <param name="Parameters">APIパラメータ</param>
/// <param name="Result">API結果値</param>
record ApiEndpoint(string[] Categories, string Path, string Method, bool Deprecated, string Summary, ApiParameter[] Parameters, ApiResult Result)
{
    /// <summary>GETメソッドであるかを判定する</summary>
    /// <returns>GETメソッドであるか否か</returns>
    public bool IsGET() => string.Equals(this.Method, "GET", StringComparison.OrdinalIgnoreCase);
}

/// <summary>コード生成コンテキスト</summary>
/// <param name="Generator">コードジェネレータインスタンス</param>
/// <param name="Resolver">型解決インスタンス</param>
/// <param name="KnownTypes">定義済みの型(コード生成中に暗黙に追加されるものを含まない)</param>
record GenerationContext(CSharpClientGenerator Generator, CSharpTypeResolver Resolver, ISet<string> KnownTypes);

/// <summary>コード生成定数</summary>
static class GenerationConstants
{
    public const string BodyParamVariable = "options";
    public const string PagingParamVariable = "paging";
    public const string NothingResult = "void";
    public const string StringResult = "string";
}

/// <summary>テキストアイテムの順次参照</summary>
class TextItemsScanner(string[] items)
{
    /// <summary>テキスト要素を1つ取得</summary>
    /// <param name="item">取得されたテキスト。戻り値が真の場合のみ有効。</param>
    /// <returns>要素を取り出したか否か。要素をすべて取り出し終えた後は偽を返す。</returns>
    public bool Take([NotNullWhen(true)] out string? item)
    {
        if (items.Length <= this.index)
        {
            item = null;
            return false;
        }
        item = items[this.index++];
        return true;
    }

    private int index = 0;
}

/// <summary>コード生成向けのAPIパラメータ情報を抽出</summary>
/// <param name="context">コード生成コンテキスト</param>
/// <param name="description">APIパラメータモデル情報</param>
/// <returns>抽出したAPIパラメータ情報</returns>
static ApiParameter InterpretParameter(this GenerationContext context, CSharpParameterModel model)
{
    var paramType = model.Type;
    var schema = model.Schema;

    // パラメータ型が列挙型であるか
    if (schema.IsEnumeration)
    {
        // 名前付きのenum型になっているかを判定
        if (!context.KnownTypes.Contains(paramType.TrimEnd('?')))
        {
            // 型として独立していない場合、文字列型を利用しておく。
            paramType = model.Type.EndsWith('?') ? "string?" : "string";
        }
    }
    // パラメータが配列型であるか
    else if (schema.IsArray && schema.Item != null && schema.Item.IsEnumeration)
    {
        if (!context.Resolver.Types.TryGetValue(schema.Item, out var itemType)
         || !context.KnownTypes.Contains(itemType.TrimEnd('?')))
        {
            paramType = model.Type.EndsWith('?') ? "string[]?" : "string[]";
        }
    }
    // パラメータがプロパティを持っていないオブジェクト型であるか
    else if (schema.IsObject && schema.Properties.Count <= 0)
    {
        // ちゃんとスキーママップが定義されていない型なので、JSON型のまま扱わせる。
        paramType = "System.Text.Json.JsonElement";
    }

    return new ApiParameter(model.Kind, model.Name, model.VariableName, model.Description, paramType, model.IsRequired);
}

/// <summary>コード生成向けのAPIエンドポイント情報を抽出</summary>
/// <param name="context">コード生成コンテキスト</param>
/// <param name="description">APIエンドポイント定義情報</param>
/// <returns>抽出したAPIエンドポイント情報</returns>
static ApiEndpoint InterpretApiEndpoint(this GenerationContext context, OpenApiOperationDescription description)
{
    // APIのカテゴリ(スコープ)。複数の場合がある。
    var categories = description.Operation.Tags?.ToArray();
    if (categories == null || categories.Length < 1) throw new Exception("Unnexpected");

    // APIの情報をC#向けに解釈
    var opModel = new CSharpOperationModel(description.Operation, context.Generator.Settings, context.Generator, context.Resolver);
    var parameters = opModel.Parameters.Select(p => context.InterpretParameter(p)).OrderBy(p => p.Kind).ToArray();

    // 戻り値の配列型を具体型に置き換え
    var resultType = opModel.SyncResultType;
    if (resultType.Match("ICollection<(.*)>") is var arayMatch && arayMatch.Success)
    {
        var elemType = arayMatch.Groups[1].Value;
        resultType = $"{elemType}[]";
    }
    var result = new ApiResult(resultType, opModel.ResultDescription);

    return new ApiEndpoint(categories, description.Path, description.Method, opModel.IsDeprecated, opModel.Summary, parameters, result);
}

/// <summary>APIメソッドアノテーションコードを生成する</summary>
/// <param name="ep">コード生成用APIパラメータ情報</param>
/// <returns>アノテーションコード</returns>
string makeApiMethodAnnotation(ApiEndpoint ep)
{
    return $"""[ForgejoEndpoint("{ep.Method.ToUpperInvariant()}", "{ep.Path}", "{ep.Summary}")]""";
}

/// <summary>APIメソッドドキュメントコメントを生成する</summary>
/// <param name="ep">コード生成用APIパラメータ情報</param>
/// <returns>メソッドドキュメントコメント</returns>
IEnumerable<string> makeApiMethodComments(ApiEndpoint ep)
{
    static string? escape(string? text) => SecurityElement.Escape(text);

    // メソッド概要
    yield return $"/// <summary>{escape(ep.Summary)}</summary>";

    // Path パラメータ
    foreach (var param in ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.Path))
    {
        yield return $$"""/// <param name="{{escape(param.Variable.TrimStart('@'))}}">{{escape(param.Description)}}</param>""";
    }

    // Body/FormData パラメータチェック
    var hasBody = ep.Parameters.Any(p => p.Kind == OpenApiParameterKind.Body);
    var hasForm = ep.Parameters.Any(p => p.Kind == OpenApiParameterKind.FormData);
    if (hasBody && hasForm) throw new Exception("どう扱うべきかわからないもの");

    // Body パラメータ
    foreach (var param in ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.Body))
    {
        yield return $$"""/// <param name="{{escape(GenerationConstants.BodyParamVariable.TrimStart('@'))}}">{{escape(param.Description)}}</param>""";
    }

    // FormData パラメータ
    foreach (var param in ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.FormData))
    {
        yield return $$"""/// <param name="{{escape(param.Variable.TrimStart('@'))}}">{{escape(param.Description)}}</param>""";
    }

    // Query パラメータ
    var queryParams = ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.Query).ToList();
    var hasPaging = queryParams.Any(p => p.Name == "limit") && queryParams.Any(p => p.Name == "page");
    if (hasPaging) queryParams.RemoveAll(p => p.Name is "limit" or "page");
    foreach (var param in queryParams)
    {
        yield return $"""/// <param name="{escape(param.Variable.TrimStart('@'))}">{escape(param.Description)}</param>""";
    }

    // ページングパラメータ
    if (hasPaging)
    {
        yield return $"""/// <param name="{escape(GenerationConstants.PagingParamVariable.TrimStart('@'))}">ページングオプション</param>""";
    }

    // キャンセルパラメータ
    yield return $"""/// <param name="cancelToken">キャンセルトークン</param>""";

    // 戻り値
    if (ep.Result.Type != GenerationConstants.NothingResult)
    {
        yield return $"""/// <returns>{ep.Result.Description}</returns>""";
    }
}

/// <summary>APIメソッドコードを生成する</summary>
/// <param name="name">メソッド名。nullの場合はAPIエンドポイントから生成する。</param>
/// <param name="ep">コード生成用APIパラメータ情報</param>
/// <returns>メソッドコード</returns>
IEnumerable<string> makeApiMethodDefine(string? name, ApiEndpoint ep)
{
    // メソッドの名前が指定されていない場合は Path から作る。
    var methodName = name ?? ep.Path.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Replace("{", "").Replace("}", "").Replace("-", "_").Replace(".", "_").Pascalize())
            .Prepend(ep.Method.Pascalize())
            .JoinString();

    // メソッド引数リスト生成
    var paramsBuilder = new StringBuilder();

    // 最初に Path パラメータを並べる。
    foreach (var param in ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.Path))
    {
        paramsBuilder.Append($"{param.Type} {param.Variable}, ");
    }

    // Body/FormData パラメータチェック
    var hasBody = ep.Parameters.Any(p => p.Kind == OpenApiParameterKind.Body);
    var hasForm = ep.Parameters.Any(p => p.Kind == OpenApiParameterKind.FormData);
    if (hasBody && hasForm) throw new Exception("どう扱うべきかわからないもの");

    // Body パラメータは固定の名称で並べる。基本的には1つしかないはず。
    foreach (var param in ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.Body))
    {
        // 型情報が nullable であっても、引数としては null 不可とする。
        paramsBuilder.Append($"{param.Type.TrimEnd('?')} {GenerationConstants.BodyParamVariable}, ");
    }

    // FormData パラメータ
    foreach (var param in ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.FormData).OrderBy(p => p.Required ? 0 : 1))
    {
        var paramType = param.Type switch
        {
            "FileParameter" => param.Required ? "Stream" : "Stream?",
            _ => param.Type,
        };
        paramsBuilder.Append($"{paramType} {param.Variable}");
        if (!param.Required) paramsBuilder.Append(" = default");
        paramsBuilder.Append($", ");
    }

    // Query パラメータにページングパラメータが含まれる場合は特別扱いする。個別のパラメータからは取り除く。
    var queryParams = ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.Query).ToList();
    var hasPaging = queryParams.Any(p => p.Name == "limit") && queryParams.Any(p => p.Name == "page");
    if (hasPaging) queryParams.RemoveAll(p => p.Name is "limit" or "page");

    // Query パラメータを並べる。
    foreach (var param in queryParams.OrderBy(p => p.Required ? 0 : 1))
    {
        paramsBuilder.Append($"{param.Type} {param.Variable}");
        if (!param.Required) paramsBuilder.Append(" = default");
        paramsBuilder.Append(", ");
    }

    // ページングパラメータを持つ場合はその専用パラメータを追加
    if (hasPaging)
    {
        paramsBuilder.Append($"PagingOptions {GenerationConstants.PagingParamVariable} = default, ");
    }

    // メソッドシグネチャコードを生成
    var retType = ep.Result.Type switch
    {
        GenerationConstants.NothingResult => ep.IsGET() ? "Task<StatusCodeResult>" : "Task",
        _ => $"Task<{ep.Result.Type}>",
    };
    yield return $"""public {retType} {methodName}({paramsBuilder}CancellationToken cancelToken = default)""";

    // APIエンドポイントパスを作成
    var embPath = 0 < ep.Path.IndexOf('{');
    var apiPath = new StringBuilder(capacity: 256);
    apiPath.Append(embPath ? "$\"" : "\"");
    apiPath.Append(ep.Path.AsSpan().TrimStart('/'));
    if (embPath)
    {
        foreach (var param in ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.Path))
        {
            apiPath.Replace($"{{{param.Name}}}", $"{{{param.Variable}}}");
        }
    }
    apiPath.Append("\"");

    // APIパスにクエリ追加
    var first = true;
    foreach (var param in queryParams)
    {
        if (first) apiPath.Append($".WithQuery({param.Variable})"); else apiPath.Append($".Param({param.Variable})");
        first = false;
    }
    if (hasPaging)
    {
        if (first) apiPath.Append($".WithQuery({GenerationConstants.PagingParamVariable})"); else apiPath.Append($".Param({GenerationConstants.PagingParamVariable})");
    }

    // Body/FormDataパラメータ引数
    var bodyArg = "";
    if (hasBody)
    {
        bodyArg = $", {GenerationConstants.BodyParamVariable}";
    }
    else if (hasForm)
    {
        bodyArg = $", new FormData()";
        foreach (var param in ep.Parameters.Where(p => p.Kind == OpenApiParameterKind.FormData))
        {
            bodyArg += param.Type switch
            {
                "FileParameter" => $".File({param.Variable})",
                _ => $".Scalar({param.Variable})",
            };
        }
        bodyArg += $".AsContent()";
    }

    // 結果取得コード
    var resultTaker = ep.Result.Type switch
    {
        GenerationConstants.NothingResult => ep.IsGET() ? ".StatusResponseAsync(cancelToken)" : ".JsonResponseAsync<EmptyResult>(cancelToken)",
        GenerationConstants.StringResult => ".TextResponseAsync(cancelToken)",
        _ => $".JsonResponseAsync<{ep.Result.Type}>(cancelToken)",
    };

    // API呼び出し処理コードを生成
    yield return $"""    => {ep.Method.Pascalize()}Request({apiPath}{bodyArg}, cancelToken){resultTaker};""";
}

/// <summary>新しくAPIクライアントコード生成する</summary>
/// <param name="name">APIスコープ識別名称</param>
/// <param name="codeFile">保存ファイル</param>
/// <param name="codeEnc">保存エンコーディング</param>
/// <param name="scopeApis">生成するAPIエンドポイント情報</param>
void createApiCode(string name, FileInfo codeFile, Encoding codeEnc, IEnumerable<ApiEndpoint> scopeApis)
{
    using var codeWriter = codeFile.WithDirectoryCreate().CreateTextWriter(encoding: codeEnc);

    // 先導コード生成
    codeWriter.WriteLine($$"""
    namespace ForgejoApiClient.Api.Scopes;

    public interface I{{name}}Api : IApiScope
    {
    """);

    // API呼び出しコード生成
    foreach (var ep in scopeApis)
    {
        foreach (var comment in makeApiMethodComments(ep))
        {
            codeWriter.WriteLine($"""    {comment}""");
        }
        codeWriter.WriteLine($"""    {makeApiMethodAnnotation(ep)}""");
        foreach (var method in makeApiMethodDefine(default, ep))
        {
            codeWriter.WriteLine($"""    {method}""");
        }
        codeWriter.WriteLine();
    }

    // 後続コード生成
    codeWriter.WriteLine("}");
}

/// <summary>APIクライアントコード更新する</summary>
/// <param name="name"></param>
/// <param name="codeFile"></param>
/// <param name="codeEnc"></param>
/// <param name="scopeApis"></param>
void updateApiCode(string name, FileInfo codeFile, Encoding codeEnc, IEnumerable<ApiEndpoint> scopeApis)
{
    // API定義コードを更新して列挙する
    IEnumerable<string> updateCodeLines(TextItemsScanner scanner)
    {
        // クラス定義の開始までそのまま出力
        var typePattern = new Regex(@$"^\s*public\s+interface\s+I{name}Api\s*:\s*IApiScope\s*$");
        while (scanner.Take(out var line))
        {
            yield return line;
            if (typePattern.IsMatch(line)) break;
        }

        // クラス本体の開始までそのまま出力
        while (scanner.Take(out var line))
        {
            yield return line;
            if (line.StartsWith('{') && line.AsSpan(1).IsWhiteSpace()) break;
        }

        // 大文字と小文字を区別しない比較処理
        var apiEndpoints = scopeApis.ToDictionary(a => $"{a.Method}:{a.Path}", comparer: StringComparer.OrdinalIgnoreCase);

        // クラス本体の更新処理
        var attrPattern = new Regex(@"^\s*\[(?<attr>.*)\]\s*");
        var commentPattern = new Regex(@"^\s*///");
        var epPatten = new Regex("""ForgejoEndpoint\s*\(\s*"(?<method>\w+)"\s*,\s*"(?<path>.*?)"\s*,\s*"(?<desc>.*?)"\)""");
        var methodPattern = new Regex(@"^\s*public\s+Task(?:<.+>)?\s+(?<func>\w+)\s*\(");
        var expPattern = new Regex(@"^\s*=>\s*\w+Request\s*\(");
        var closePending = false;
        while (scanner.Take(out var line))
        {
            // クラス本体の終了を見つけたら更新処理終了
            if (line.StartsWith('}') && line.AsSpan(1).IsWhiteSpace()) { closePending = true; break; }

            // ドキュメントコメントコードを見つける。ドキュメントコメントは一旦除去する。
            var commentMatch = commentPattern.Match(line);
            if (commentMatch.Success) { continue; }

            // 属性コードを見つける。それ以外はそのまま出力。
            var attrMatch = attrPattern.Match(line);
            if (!attrMatch.Success) { yield return line; continue; }

            // それがエンドポイント定義の属性であるかを判定。そうでないものはそのまま出力。
            var epMatch = epPatten.Match(line, attrMatch.Index, attrMatch.Length);
            if (!epMatch.Success) { yield return line; continue; }

            // エンドポイント定義属性の後ろにある属性は破棄する。これは、そういう取り扱いにするという決め事。主に ManualEdit 属性を捨てる目的。
            while (scanner.Take(out line)) { if (!attrPattern.IsMatch(line)) break; }

            // APIメソッド定義が続くかを判定
            if (line == null) break;
            var methodMatch = methodPattern.Match(line);
            if (!methodMatch.Success) { yield return line; continue; }

            // 関数の式本体によるAPI呼び出しが続くかを判定
            if (!scanner.Take(out line)) break;
            var expMatch = expPattern.Match(line);
            if (!expMatch.Success) { yield return line; continue; }

            // 検出した定義と同じAPIの情報を取得
            var orgMethod = epMatch.Groups["method"].Value;
            var orgPath = epMatch.Groups["path"].Value;
            var orgKey = $"{orgMethod}:{orgPath}";
            if (!apiEndpoints.Remove(orgKey, out var newEp)) continue;

            // 新しいメソッド定義を生成
            var orgFunc = methodMatch.Groups["func"].Value;
            foreach (var comment in makeApiMethodComments(newEp))
            {
                yield return $"""    {comment}""";
            }
            yield return $"""    {makeApiMethodAnnotation(newEp)}""";
            foreach (var method in makeApiMethodDefine(orgFunc, newEp))
            {
                yield return $"""    {method}""";
            }
        }

        // 更新生成対象になかったAPI定義からコードを生成
        foreach (var ep in apiEndpoints.Values)
        {
            foreach (var comment in makeApiMethodComments(ep))
            {
                yield return $"""    {comment}""";
            }
            yield return $"""    {makeApiMethodAnnotation(ep)}""";
            foreach (var method in makeApiMethodDefine(default, ep))
            {
                yield return $"""    {method}""";
            }
            yield return "";
        }

        // 閉じブラケットをスキップしてきている場合はここで出力
        if (closePending)
        {
            yield return "}";
        }

        // 残りのコードをそのまま出力
        while (scanner.Take(out var line))
        {
            yield return line;
        }
    }

    // ファイルを読み取り、API定義を基に更新して出力する。
    var codeLines = codeFile.ReadAllLines();
    var newLines = updateCodeLines(new(codeLines));
    codeFile.WriteAllLines(newLines, codeEnc);
}