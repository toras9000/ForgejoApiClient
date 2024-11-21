using ForgejoApiClient.Api.Scopes;

namespace ForgejoApiClient.Api.Extensions;

/// <summary>repository スコープAPIの拡張メソッド</summary>
public static class RepositoryApiExtensions
{
    /// <summary>対象のユーザがリポジトリの共同作業者であるか否かを取得する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="collaborator">対象ユーザ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>共同作業者であるか否か</returns>
    public static async ValueTask<bool> IsCollaboratorAsync(this IRepositoryApi self, string owner, string repo, string collaborator, CancellationToken cancelToken = default)
    {
        var status = await self.CheckCollaboratorAsync(owner, repo, collaborator, cancelToken).ConfigureAwait(false);
        return status.EvalStatusCode();
    }

    /// <summary>対象のリポジトリにフラグが設定されているか否かを取得する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="flag">フラグ</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>フラグが設定されているか否か</returns>
    public static async ValueTask<bool> IsFlagGivenAsync(this IRepositoryApi self, string owner, string repo, string flag, CancellationToken cancelToken = default)
    {
        var status = await self.CheckFlagGivenAsync(owner, repo, flag, cancelToken).ConfigureAwait(false);
        return status.EvalStatusCode();
    }

    /// <summary>対象のプルリクエストがマージされているか否かを取得する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="index">プルリクエスト番号</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>マージされているか否か</returns>
    public static async ValueTask<bool> IsPullRequestMergeAsync(this IRepositoryApi self, string owner, string repo, long index, CancellationToken cancelToken = default)
    {
        var status = await self.CheckPullRequestMergeAsync(owner, repo, index, cancelToken).ConfigureAwait(false);
        return status.EvalStatusCode();
    }

    /// <summary>リリースにファイルを添付する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリのオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="id">リリースID</param>
    /// <param name="file">添付するファイル情報</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>添付情報</returns>
    public static async Task<Attachment> CreateReleaseAttachmentAsync(this IRepositoryApi self, string owner, string repo, long id, FileInfo file, CancellationToken cancelToken = default)
    {
        var options = new FileStreamOptions();
        options.Mode = FileMode.Open;
        options.Access = FileAccess.Read;
        options.Share = FileShare.None;
        options.BufferSize = 0;

        using var fileStream = new FileStream(file.FullName, options);
        return await self.CreateReleaseAttachmentAsync(owner, repo, id, fileStream, file.Name, cancelToken);
    }

    /// <summary>リリースにファイルを添付する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリのオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="id">リリースID</param>
    /// <param name="content">添付するファイル内容</param>
    /// <param name="name">ファイル名</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>添付情報</returns>
    public static async Task<Attachment> CreateReleaseAttachmentAsync(this IRepositoryApi self, string owner, string repo, long id, byte[] content, string? name = default, CancellationToken cancelToken = default)
    {
        using var stream = new MemoryStream(content, writable: false);
        return await self.CreateReleaseAttachmentAsync(owner, repo, id, stream, name, cancelToken);
    }
}
