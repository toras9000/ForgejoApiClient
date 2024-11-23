using ForgejoApiClient.Api.Scopes;

namespace ForgejoApiClient.Api.Extensions;

/// <summary>issue スコープAPIの拡張メソッド</summary>
public static class IssueApiExtensions
{
    /// <summary>イシューにファイルを添付する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリのオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="index">イシュー番号</param>
    /// <param name="file">添付するファイル情報</param>
    /// <param name="updated_at">添付の更新日時</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>添付情報</returns>
    public static async Task<Attachment> CreateFileAttachmentAsync(this IIssueApi self, string owner, string repo, long index, FileInfo file, DateTimeOffset? updated_at = default, CancellationToken cancelToken = default)
    {
        var options = new FileStreamOptions();
        options.Mode = FileMode.Open;
        options.Access = FileAccess.Read;
        options.Share = FileShare.None;
        options.BufferSize = 0;

        using var fileStream = new FileStream(file.FullName, options);
        return await self.CreateAttachmentAsync(owner, repo, index, fileStream, file.Name, updated_at, cancelToken);
    }

    /// <summary>イシューにファイルを添付する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリのオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="index">イシュー番号</param>
    /// <param name="content">添付するファイル内容</param>
    /// <param name="name">ファイル名</param>
    /// <param name="updated_at">添付の更新日時</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>添付情報</returns>
    public static async Task<Attachment> CreateFileAttachmentAsync(this IIssueApi self, string owner, string repo, long index, byte[] content, string? name = default, DateTimeOffset? updated_at = default, CancellationToken cancelToken = default)
    {
        using var stream = new MemoryStream(content, writable: false);
        return await self.CreateAttachmentAsync(owner, repo, index, stream, name, updated_at, cancelToken);
    }

    /// <summary>イシューコメントにファイルを添付する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリのオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="id">コメントID</param>
    /// <param name="file">添付するファイル情報</param>
    /// <param name="updated_at">添付の更新日時</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>添付情報</returns>
    public static async Task<Attachment> CreateCommentFileAttachmentAsync(this IIssueApi self, string owner, string repo, long id, FileInfo file, DateTimeOffset? updated_at = default, CancellationToken cancelToken = default)
    {
        var options = new FileStreamOptions();
        options.Mode = FileMode.Open;
        options.Access = FileAccess.Read;
        options.Share = FileShare.None;
        options.BufferSize = 0;

        using var fileStream = new FileStream(file.FullName, options);
        return await self.CreateCommentAttachmentAsync(owner, repo, id, fileStream, file.Name, updated_at, cancelToken);
    }

    /// <summary>イシューコメントにファイルを添付する</summary>
    /// <param name="self">APIインタフェース</param>
    /// <param name="owner">リポジトリのオーナ</param>
    /// <param name="repo">リポジトリ名</param>
    /// <param name="id">コメントID</param>
    /// <param name="content">添付するファイル内容</param>
    /// <param name="name">ファイル名</param>
    /// <param name="updated_at">添付の更新日時</param>
    /// <param name="cancelToken">キャンセルトークン</param>
    /// <returns>添付情報</returns>
    public static async Task<Attachment> CreateCommentFileAttachmentAsync(this IIssueApi self, string owner, string repo, long id, byte[] content, string? name = default, DateTimeOffset? updated_at = default, CancellationToken cancelToken = default)
    {
        using var stream = new MemoryStream(content, writable: false);
        return await self.CreateCommentAttachmentAsync(owner, repo, id, stream, name, updated_at, cancelToken);
    }

}
