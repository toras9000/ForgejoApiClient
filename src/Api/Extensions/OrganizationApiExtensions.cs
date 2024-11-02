using ForgejoApiClient.Api.Scopes;

namespace ForgejoApiClient.Api.Extensions;

/// <summary>organization スコープAPIの拡張メソッド</summary>
public static class OrganizationApiExtensions
{
    /// <summary>対象のユーザが組織のメンバーであるか否かを取得する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="org">対象組織</param>
    /// <param name="username">対象ユーザ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>メンバーであるか否か</returns>
    public static async ValueTask<bool> IsMemberAsync(this IOrganizationApi self, string org, string username, CancellationToken cancelToken = default)
    {
        var status = await self.CheckMemberAsync(org, username, cancelToken).ConfigureAwait(false);
        return status.EvalStatusCode();
    }

    /// <summary>対象のユーザが組織で公開されているか否かを取得する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="org">対象組織</param>
    /// <param name="username">対象ユーザ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>公開されているか否か</returns>
    public static async ValueTask<bool> IsPublicMemberAsync(this IOrganizationApi self, string org, string username, CancellationToken cancelToken = default)
    {
        var status = await self.CheckPublicMemberAsync(org, username, cancelToken).ConfigureAwait(false);
        return status.EvalStatusCode();
    }
}
