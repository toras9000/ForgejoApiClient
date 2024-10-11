using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForgejoApiClient.Converters;

/// <summary>enumと文字列のコンバータを生成するファクトリ</summary>
[RequiresDynamicCode("User dynamic instance creation.")]
public class EnumJsonConverterFactory : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        // enum 型ならOK
        if (typeToConvert.IsEnum) return true;

        // Nullable<enum> もOK
        var nullableType = Nullable.GetUnderlyingType(typeToConvert);
        if (nullableType?.IsEnum == true) return true;

        // それ以外はNG
        return false;
    }

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        // enum 型用のコンバータ生成
        if (typeToConvert.IsEnum)
        {
            var converterType = typeof(MapEnumJsonConverter<>).MakeGenericType(typeToConvert);
            return Activator.CreateInstance(converterType) as JsonConverter;
        }

        // Nullable<enum> 型用のコンバータ生成
        var nullableType = Nullable.GetUnderlyingType(typeToConvert);
        if (nullableType?.IsEnum == true)
        {
            var converterType = typeof(NullableMapEnumJsonConverter<>).MakeGenericType(nullableType);
            return Activator.CreateInstance(converterType) as JsonConverter;
        }

        return null;
    }
}

/// <summary>enumとJSON文字列のコンバータ</summary>
/// <typeparam name="TEnum">対象enum型</typeparam>
public class MapEnumJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    // 公開メソッド
    #region 変換
    /// <inheritdoc />
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = StringEnumConverter<TEnum>.Instance;
        var value = reader.GetString() ?? throw new JsonException("Unexpected value");
        return converter.TryToEnum(value, out var member) ? member : throw new JsonException("Cannot map");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var textValue = StringEnumConverter<TEnum>.Instance.TryToString(value, out var name) ? name : value.ToString();
        writer.WriteStringValue(textValue);
    }
    #endregion
}

/// <summary>enum?とJSON文字列のコンバータ</summary>
/// <typeparam name="TEnum">対象enum型</typeparam>
public class NullableMapEnumJsonConverter<TEnum> : JsonConverter<TEnum?> where TEnum : struct, Enum?
{
    // 公開メソッド
    #region 変換
    /// <inheritdoc />
    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString() ?? throw new JsonException("Unexpected value");
        if (string.IsNullOrWhiteSpace(value)) return default;
        var converter = StringEnumConverter<TEnum>.Instance;
        return converter.TryToEnum(value, out var member) ? member : throw new JsonException("Cannot map");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TEnum? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            var textValue = StringEnumConverter<TEnum>.Instance.TryToString(value.Value, out var name) ? name : value.ToString();
            writer.WriteStringValue(textValue);
        }
    }
    #endregion
}

/// <summary>マッピング属性によるenumと文字列の変換処理</summary>
/// <typeparam name="TEnum">対象enum型</typeparam>
public class StringEnumConverter<TEnum> where TEnum : struct, Enum
{
    // 公開フィールド
    #region インスタンス
    /// <summary>シングルトンインスタンス</summary>
    public static readonly StringEnumConverter<TEnum> Instance = new StringEnumConverter<TEnum>();
    #endregion

    // 公開メソッド
    #region 変換
    /// <summary>文字列をenum値に変換する</summary>
    /// <param name="str">変換元の文字列</param>
    /// <param name="member">変換結果のenum値</param>
    /// <returns>変換出来たか否か</returns>
    public bool TryToEnum(string str, out TEnum member) => this.mapToEnum.TryGetValue(str, out member);

    /// <summary>enum値を文字列に変換する</summary>
    /// <param name="member"></param>
    /// <param name="str"></param>
    /// <returns>変換出来たか否か</returns>
    public bool TryToString(TEnum member, [MaybeNullWhen(false)] out string str) => this.mapToString.TryGetValue(member, out str);
    #endregion

    // 構築(非公開)
    #region コンストラクタ
    /// <summary>デフォルトコンストラクタ</summary>
    private StringEnumConverter()
    {
        // 相互の変換テーブルを作る
        var enumValues = Enum.GetValues<TEnum>();
        this.mapToString = new Dictionary<TEnum, string>(capacity: enumValues.Length);
        this.mapToEnum = new Dictionary<string, TEnum>(capacity: enumValues.Length, StringComparer.OrdinalIgnoreCase);
        foreach (var member in enumValues)
        {
            // 列挙メンバの名称取得
            var memberName = Enum.GetName(member);
            if (memberName == null) continue;

            // 列挙メンバ属性
            var mapAttr = typeof(TEnum).GetField(memberName)?.GetCustomAttribute<MapEnumAttribute>();
            if (mapAttr == null)
            {
                this.mapToString[member] = memberName;
                this.mapToEnum[memberName] = member;
            }
            else
            {
                this.mapToString[member] = mapAttr.Name;
                this.mapToEnum[mapAttr.Name] = member;
                this.mapToEnum.TryAdd(memberName, member);  // 非優先の変換
            }
        }
    }
    #endregion

    // 非公開フィールド
    #region テーブル
    /// <summary>enum値から文字列への変換テーブル</summary>
    private readonly Dictionary<TEnum, string> mapToString;

    /// <summary>文字列からenum値への変換テーブル</summary>
    private readonly Dictionary<string, TEnum> mapToEnum;
    #endregion

}