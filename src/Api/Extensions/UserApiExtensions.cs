using ForgejoApiClient.Api.Scopes;

namespace ForgejoApiClient.Api.Extensions;

/// <summary>user スコープAPIの拡張メソッド</summary>
public static class UserApiExtensions
{
    /// <summary>対象のユーザをフォローしているか否かを取得する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="username">フォローしているか調べる対象ユーザ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>フォローしているか否か</returns>
    public static async ValueTask<bool> IsFollowingAsync(this IUserApi self, string username, CancellationToken cancelToken = default)
    {
        var status = await self.CheckFollowingAsync(username, cancelToken).ConfigureAwait(false);
        return status.EvalStatusCode();
    }

    /// <summary>指定のユーザが対象のユーザをフォローしているか否かを取得する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="username">情報取得するユーザ</param>
    /// <param name="target">フォローしているか調べる対象ユーザ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>フォローしているか否か</returns>
    public static async ValueTask<bool> IsUserFollowingAsync(this IUserApi self, string username, string target, CancellationToken cancelToken = default)
    {
        var status = await self.CheckUserFollowingAsync(username, target, cancelToken).ConfigureAwait(false);
        return status.EvalStatusCode();
    }

    /// <summary>対象のリポジトリにスターを付けているか否かを取得する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>スターを付けているか否か</returns>
    public static async ValueTask<bool> IsStarredAsync(this IUserApi self, string owner, string repo, CancellationToken cancelToken = default)
    {
        var status = await self.CheckStarredAsync(owner, repo, cancelToken).ConfigureAwait(false);
        return status.EvalStatusCode();
    }

}
