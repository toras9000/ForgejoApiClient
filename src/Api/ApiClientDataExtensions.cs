namespace ForgejoApiClient.Api;

/// <summary>API戻り値型の拡張メソッド</summary>
public static class ApiClientDataExtensions
{
    /// <summary>ステータスコード結果値を評価する。</summary>
    /// <param name="self">取得されたステータスコード結果値</param>
    /// <returns>成否を表す場合はその真偽値を返却。それ以外の場合はエラーとみなして例外を送出する。</returns>
    public static bool EvalStatusCode(this StatusCodeResult self)
        => (int)self.Code switch
        {
            >= 200 and < 300 => true,
            404 => false,
            _ => throw new ErrorResponseException(self.Code, self.Message ?? $"HTTP {(int)self.Code}"),
        };

}
