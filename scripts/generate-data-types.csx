#r "nuget: NSwag.CodeGeneration.CSharp, 14.5.0"
#r "nuget: Kokuban, 0.2.0"
#r "nuget: Lestaly.General, 0.102.0"
#nullable enable
using System.Security;
using Kokuban;
using Lestaly;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using NJsonSchema.CodeGeneration.CSharp.Models;
using NSwag;

// SwaggerファイルからAPIで利用するデータ型ソースコードを生成する。
// APIクライアントの構造は自分好みに作りたいが、APIで扱うデータはドキュメントと照らし合わせやすいよう、元の命名を利用したいという考えがある。
// また、データは比較的多く変更が生ずるであろうから、自動的に追随したい。
// そのため、Generatorでクライアント型を生成するのではなく、ただデータ型だけを生成する目的のスクリプトとなっている。

var config = new
{
    // Swaggerファイル取得URL
    ForgejoSwaggerUrl = @"http://localhost:9970/swagger.v1.json",

    // データ型定義
    TypeDefineFile = ThisSource.RelativeFile("../src/Api/ApiData.cs"),

    // データ型定義名前空間
    TypeDefineNamespace = "ForgejoApiClient.Api",

    // C#の予約語 (プロパティ名利用で特殊処理が必要なもの)
    ReserveWords = new HashSet<string>([
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
        "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
        "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface",
        "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
        "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof",
        "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong",
        "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while",
    ]),
};

return await Paved.ProceedAsync(async () =>
{
    // Swaggerファイルの読み込み
    WriteLine("Get swagger");
    var apiDoc = await OpenApiDocument.FromUrlAsync(new(config.ForgejoSwaggerUrl));

    // コード生成の準備
    WriteLine("Collect API data-type info");

    // 生成の設定
    var settings = new CSharpGeneratorSettings();
    settings.DateType = "DateTimeOffset";
    settings.DateTimeType = "DateTimeOffset";
    settings.TimeType = "TimeSpan";
    settings.TimeSpanType = "TimeSpan";
    settings.ArrayType = "ICollection";
    settings.ArrayInstanceType = "List";
    settings.ArrayBaseType = "List";
    settings.DictionaryType = "IDictionary";
    settings.DictionaryInstanceType = "Dictionary";
    settings.DictionaryBaseType = "Dictionary";
    settings.GenerateOptionalPropertiesAsNullable = true;
    settings.GenerateNullableReferenceTypes = true;
    settings.ClassStyle = CSharpClassStyle.Record;
    settings.JsonLibrary = CSharpJsonLibrary.SystemTextJson;
    settings.JsonPolymorphicSerializationStyle = CSharpJsonPolymorphicSerializationStyle.SystemTextJson;
    settings.GenerateDataAnnotations = false;
    settings.InlineNamedArrays = true;
    settings.InlineNamedDictionaries = true;
    settings.InlineNamedTuples = true;

    // 型リゾルバに定義された型と、プロパティで利用されている列挙型を登録する
    var resolver = new CSharpTypeResolver(settings);
    resolver.RegisterSchemaDefinitions(apiDoc.Definitions);

    // 生成コードファイルを書き込み用に開く
    WriteLine("Generate data-type code");
    using var writer = config.TypeDefineFile.WithDirectoryCreate().CreateTextWriter();

    // ヘッダコードを出力
    writer.WriteLine("using System;");
    writer.WriteLine("using System.Collections.Generic;");
    writer.WriteLine("using System.Text.Json.Serialization;");
    writer.WriteLine("using ForgejoApiClient.Converters;");
    writer.WriteLine("");
    writer.WriteLine("#pragma warning disable IDE1006 // 命名スタイル");
    writer.WriteLine("");
    writer.WriteLine($"namespace {config.TypeDefineNamespace};");
    writer.WriteLine("");

    // 型のコードを出力
    var defTypes = resolver.Types.ToDictionary();
    foreach (var code in generateTypeDefines(resolver, defTypes))
    {
        writer.WriteLine(code);
    }

    // 型モデルを生成した際に追加される型もある。それを追加で出力する。
    var addedTypes = resolver.Types.Where(t => !defTypes.ContainsKey(t.Key));  // 上で出力していないものを対象とする
    foreach (var code in generateTypeDefines(resolver, addedTypes))
    {
        writer.WriteLine(code);
    }

    WriteLine(Chalk.Green["Generation completed"]);
});


// 型の概要コメント生成
IEnumerable<string> generateDescription(string? description, string beginTag, string endTag)
{
    static string? escape(string? text) => SecurityElement.Escape(text);

    var descLines = description?.Split(['\r', '\n']) ?? [];
    if (descLines.Length < 2)
    {
        yield return $"/// {beginTag}{escape(description)}{endTag}";
    }
    else
    {
        yield return $"/// {beginTag}";
        foreach (var line in descLines) yield return $"/// {escape(line)}";
        yield return $"/// {endTag}";
    }
}

// 型の定義コード生成
IEnumerable<string> generateTypeDefines(CSharpTypeResolver resolver, IEnumerable<KeyValuePair<JsonSchema, string>> types)
{
    // 各データ型の定義コードを出力
    foreach (var define in types)
    {
        var typeSchema = define.Key;
        var typeName = define.Value;

        // 列挙型か否かを判定
        if (typeSchema.IsEnumeration)
        {
            // 列挙定義の出力
            var enumModel = new EnumTemplateModel(typeName, typeSchema, resolver.Settings);
            foreach (var desc in generateDescription(enumModel.Description, "<summary>", "</summary>"))
            {
                yield return desc;
            }
            if (typeSchema.IsDeprecated)
            {
                yield return $"[Obsolete(\"{typeSchema.DeprecatedMessage}\")]";
            }
            yield return $"public enum {enumModel.Name}";
            yield return "{";
            foreach (var entryModel in enumModel.Enums)
            {
                var entryName = config.ReserveWords.Contains(entryModel.Name) ? $"@{entryModel.Name}" : entryModel.Name;
                foreach (var desc in generateDescription(entryModel.Value, "<summary>", "</summary>"))
                {
                    yield return $"    {desc}";
                }
                yield return $"    [MapEnum(\"{entryModel.Value}\")]";
                yield return $"    {entryName} = {entryModel.InternalValue},";
            }
            yield return "};";
            yield return "";
        }
        else
        {
            // 型情報取得
            var classModel = new ClassTemplateModel(typeName, resolver.Settings, resolver, typeSchema, new());
            var properties = classModel.Properties
                .Select(p =>
                {
                    var propName = config.ReserveWords.Contains(p.Name) ? $"@{p.Name}" : p.Name;
                    return new { PropName = propName, Model = p, };
                })
                .OrderBy(p => p.Model.IsNullable)
                .ToArray();
            if (0 < properties.Length)
            {
                // オブジェクト型定義の出力
                foreach (var desc in generateDescription(classModel.Description, "<summary>", "</summary>"))
                {
                    yield return desc;
                }
                foreach (var prop in properties)
                {
                    foreach (var desc in generateDescription(prop.Model.Description, $"<param name=\"{prop.PropName.TrimStart('@')}\">", "</param>"))
                    {
                        yield return desc;
                    }
                }
                if (typeSchema.IsDeprecated)
                {
                    yield return $"[Obsolete(\"{typeSchema.DeprecatedMessage}\")]";
                }
                if (properties.Length == 0)
                {
                    yield return $"public record {classModel.ClassName}();";
                }
                else
                {
                    yield return $"public record {classModel.ClassName}(";
                    var count = 0;
                    for (var i = 0; i < properties.Length; i++)
                    {
                        var prop = properties[i];
                        var neesSepa = i < (properties.Length - 1);
                        if (prop.Model.IsDeprecated)
                        {
                            yield return $"[Obsolete(\"{prop.Model.DeprecatedMessage}\")]";
                        }
                        yield return $"    {prop.Model.Type} {prop.PropName}{(prop.Model.IsNullable ? $" = default" : "")}{(neesSepa ? "," : "")}";
                        count++;
                    }
                    yield return $");";
                }
                yield return "";
            }
        }
    }
}
