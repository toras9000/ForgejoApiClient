using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ForgejoApiClient.Converters;

#pragma warning disable IDE1006 // 命名スタイル

namespace ForgejoApiClient.Api;

/// <summary>APIError is an api error with a message</summary>
/// <param name="message"></param>
/// <param name="url"></param>
public record APIError(
    string? message = default,
    string? url = default
);

/// <summary></summary>
/// <param name="id"></param>
/// <param name="name"></param>
/// <param name="scopes"></param>
/// <param name="sha1"></param>
/// <param name="token_last_eight"></param>
public record AccessToken(
    long? id = default,
    string? name = default,
    ICollection<string>? scopes = default,
    string? sha1 = default,
    string? token_last_eight = default
);

/// <summary></summary>
/// <param name="act_user"></param>
/// <param name="act_user_id"></param>
/// <param name="comment"></param>
/// <param name="comment_id"></param>
/// <param name="content"></param>
/// <param name="created"></param>
/// <param name="id"></param>
/// <param name="is_private"></param>
/// <param name="op_type"></param>
/// <param name="ref_name"></param>
/// <param name="repo"></param>
/// <param name="repo_id"></param>
/// <param name="user_id"></param>
public record Activity(
    User? act_user = default,
    long? act_user_id = default,
    Comment? comment = default,
    long? comment_id = default,
    string? content = default,
    DateTimeOffset? created = default,
    long? id = default,
    bool? is_private = default,
    string? op_type = default,
    string? ref_name = default,
    Repository? repo = default,
    long? repo_id = default,
    long? user_id = default
);

/// <summary>ActivityPub type</summary>
/// <param name="context"></param>
public record ActivityPub(
    string? @context = default
);

/// <summary>AddCollaboratorOption options when adding a user as a collaborator of a repository</summary>
/// <param name="permission"></param>
public record AddCollaboratorOption(
    string? permission = default
);

/// <summary>AddTimeOption options for adding time to an issue</summary>
/// <param name="time">time in seconds</param>
/// <param name="created"></param>
/// <param name="user_name">User who spent the time (optional)</param>
public record AddTimeOption(
    long time,
    DateTimeOffset? created = default,
    string? user_name = default
);

/// <summary>AnnotatedTag represents an annotated tag</summary>
/// <param name="message"></param>
/// <param name="object"></param>
/// <param name="sha"></param>
/// <param name="tag"></param>
/// <param name="tagger"></param>
/// <param name="url"></param>
/// <param name="verification"></param>
public record AnnotatedTag(
    string? message = default,
    AnnotatedTagObject? @object = default,
    string? sha = default,
    string? tag = default,
    CommitUser? tagger = default,
    string? url = default,
    PayloadCommitVerification? verification = default
);

/// <summary>AnnotatedTagObject contains meta information of the tag object</summary>
/// <param name="sha"></param>
/// <param name="type"></param>
/// <param name="url"></param>
public record AnnotatedTagObject(
    string? sha = default,
    string? type = default,
    string? url = default
);

/// <summary>Attachment a generic attachment</summary>
/// <param name="browser_download_url"></param>
/// <param name="created_at"></param>
/// <param name="download_count"></param>
/// <param name="id"></param>
/// <param name="name"></param>
/// <param name="size"></param>
/// <param name="uuid"></param>
public record Attachment(
    string? browser_download_url = default,
    DateTimeOffset? created_at = default,
    long? download_count = default,
    long? id = default,
    string? name = default,
    long? size = default,
    string? uuid = default
);

/// <summary></summary>
/// <param name="block_id"></param>
/// <param name="created_at"></param>
public record BlockedUser(
    long? block_id = default,
    DateTimeOffset? created_at = default
);

/// <summary>Branch represents a repository branch</summary>
/// <param name="commit"></param>
/// <param name="effective_branch_protection_name"></param>
/// <param name="enable_status_check"></param>
/// <param name="name"></param>
/// <param name="protected"></param>
/// <param name="required_approvals"></param>
/// <param name="status_check_contexts"></param>
/// <param name="user_can_merge"></param>
/// <param name="user_can_push"></param>
public record Branch(
    PayloadCommit? commit = default,
    string? effective_branch_protection_name = default,
    bool? enable_status_check = default,
    string? name = default,
    bool? @protected = default,
    long? required_approvals = default,
    ICollection<string>? status_check_contexts = default,
    bool? user_can_merge = default,
    bool? user_can_push = default
);

/// <summary>BranchProtection represents a branch protection for a repository</summary>
/// <param name="apply_to_admins"></param>
/// <param name="approvals_whitelist_teams"></param>
/// <param name="approvals_whitelist_username"></param>
/// <param name="block_on_official_review_requests"></param>
/// <param name="block_on_outdated_branch"></param>
/// <param name="block_on_rejected_reviews"></param>
/// <param name="branch_name">Deprecated: true</param>
/// <param name="created_at"></param>
/// <param name="dismiss_stale_approvals"></param>
/// <param name="enable_approvals_whitelist"></param>
/// <param name="enable_merge_whitelist"></param>
/// <param name="enable_push"></param>
/// <param name="enable_push_whitelist"></param>
/// <param name="enable_status_check"></param>
/// <param name="ignore_stale_approvals"></param>
/// <param name="merge_whitelist_teams"></param>
/// <param name="merge_whitelist_usernames"></param>
/// <param name="protected_file_patterns"></param>
/// <param name="push_whitelist_deploy_keys"></param>
/// <param name="push_whitelist_teams"></param>
/// <param name="push_whitelist_usernames"></param>
/// <param name="require_signed_commits"></param>
/// <param name="required_approvals"></param>
/// <param name="rule_name"></param>
/// <param name="status_check_contexts"></param>
/// <param name="unprotected_file_patterns"></param>
/// <param name="updated_at"></param>
public record BranchProtection(
    bool? apply_to_admins = default,
    ICollection<string>? approvals_whitelist_teams = default,
    ICollection<string>? approvals_whitelist_username = default,
    bool? block_on_official_review_requests = default,
    bool? block_on_outdated_branch = default,
    bool? block_on_rejected_reviews = default,
    string? branch_name = default,
    DateTimeOffset? created_at = default,
    bool? dismiss_stale_approvals = default,
    bool? enable_approvals_whitelist = default,
    bool? enable_merge_whitelist = default,
    bool? enable_push = default,
    bool? enable_push_whitelist = default,
    bool? enable_status_check = default,
    bool? ignore_stale_approvals = default,
    ICollection<string>? merge_whitelist_teams = default,
    ICollection<string>? merge_whitelist_usernames = default,
    string? protected_file_patterns = default,
    bool? push_whitelist_deploy_keys = default,
    ICollection<string>? push_whitelist_teams = default,
    ICollection<string>? push_whitelist_usernames = default,
    bool? require_signed_commits = default,
    long? required_approvals = default,
    string? rule_name = default,
    ICollection<string>? status_check_contexts = default,
    string? unprotected_file_patterns = default,
    DateTimeOffset? updated_at = default
);

/// <summary>ChangeFileOperation for creating, updating or deleting a file</summary>
/// <param name="operation">indicates what to do with the file</param>
/// <param name="path">path to the existing or new file</param>
/// <param name="content">new or updated file content, must be base64 encoded</param>
/// <param name="from_path">old path of the file to move</param>
/// <param name="sha">sha is the SHA for the file that already exists, required for update or delete</param>
public record ChangeFileOperation(
    ChangeFileOperationOperation operation,
    string path,
    string? content = default,
    string? from_path = default,
    string? sha = default
);

/// <summary>
/// ChangeFilesOptions options for creating, updating or deleting multiple files
/// Note: `author` and `committer` are optional (if only one is given, it will be used for the other, otherwise the authenticated user will be used)
/// </summary>
/// <param name="files">list of file operations</param>
/// <param name="author"></param>
/// <param name="branch">branch (optional) to base this file from. if not given, the default branch is used</param>
/// <param name="committer"></param>
/// <param name="dates"></param>
/// <param name="message">message (optional) for the commit of this file. if not supplied, a default message will be used</param>
/// <param name="new_branch">new_branch (optional) will make a new branch from `branch` before creating the file</param>
/// <param name="signoff">Add a Signed-off-by trailer by the committer at the end of the commit log message.</param>
public record ChangeFilesOptions(
    ICollection<ChangeFileOperation> files,
    Identity? author = default,
    string? branch = default,
    Identity? committer = default,
    CommitDateOptions? dates = default,
    string? message = default,
    string? new_branch = default,
    bool? signoff = default
);

/// <summary>ChangedFile store information about files affected by the pull request</summary>
/// <param name="additions"></param>
/// <param name="changes"></param>
/// <param name="contents_url"></param>
/// <param name="deletions"></param>
/// <param name="filename"></param>
/// <param name="html_url"></param>
/// <param name="previous_filename"></param>
/// <param name="raw_url"></param>
/// <param name="status"></param>
public record ChangedFile(
    long? additions = default,
    long? changes = default,
    string? contents_url = default,
    long? deletions = default,
    string? filename = default,
    string? html_url = default,
    string? previous_filename = default,
    string? raw_url = default,
    string? status = default
);

/// <summary>CombinedStatus holds the combined state of several statuses for a single commit</summary>
/// <param name="commit_url"></param>
/// <param name="repository"></param>
/// <param name="sha"></param>
/// <param name="state"></param>
/// <param name="statuses"></param>
/// <param name="total_count"></param>
/// <param name="url"></param>
public record CombinedStatus(
    string? commit_url = default,
    Repository? repository = default,
    string? sha = default,
    string? state = default,
    ICollection<CommitStatus>? statuses = default,
    long? total_count = default,
    string? url = default
);

/// <summary>Comment represents a comment on a commit or issue</summary>
/// <param name="assets"></param>
/// <param name="body"></param>
/// <param name="created_at"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="issue_url"></param>
/// <param name="original_author"></param>
/// <param name="original_author_id"></param>
/// <param name="pull_request_url"></param>
/// <param name="updated_at"></param>
/// <param name="user"></param>
public record Comment(
    ICollection<Attachment>? assets = default,
    string? body = default,
    DateTimeOffset? created_at = default,
    string? html_url = default,
    long? id = default,
    string? issue_url = default,
    string? original_author = default,
    long? original_author_id = default,
    string? pull_request_url = default,
    DateTimeOffset? updated_at = default,
    User? user = default
);

/// <summary></summary>
/// <param name="author"></param>
/// <param name="commit"></param>
/// <param name="committer"></param>
/// <param name="created"></param>
/// <param name="files"></param>
/// <param name="html_url"></param>
/// <param name="parents"></param>
/// <param name="sha"></param>
/// <param name="stats"></param>
/// <param name="url"></param>
public record Commit(
    User? author = default,
    RepoCommit? commit = default,
    User? committer = default,
    DateTimeOffset? created = default,
    ICollection<CommitAffectedFiles>? files = default,
    string? html_url = default,
    ICollection<CommitMeta>? parents = default,
    string? sha = default,
    CommitStats? stats = default,
    string? url = default
);

/// <summary>CommitAffectedFiles store information about files affected by the commit</summary>
/// <param name="filename"></param>
/// <param name="status"></param>
public record CommitAffectedFiles(
    string? filename = default,
    string? status = default
);

/// <summary>CommitDateOptions store dates for GIT_AUTHOR_DATE and GIT_COMMITTER_DATE</summary>
/// <param name="author"></param>
/// <param name="committer"></param>
public record CommitDateOptions(
    DateTimeOffset? author = default,
    DateTimeOffset? committer = default
);

/// <summary></summary>
/// <param name="created"></param>
/// <param name="sha"></param>
/// <param name="url"></param>
public record CommitMeta(
    DateTimeOffset? created = default,
    string? sha = default,
    string? url = default
);

/// <summary>CommitStats is statistics for a RepoCommit</summary>
/// <param name="additions"></param>
/// <param name="deletions"></param>
/// <param name="total"></param>
public record CommitStats(
    long? additions = default,
    long? deletions = default,
    long? total = default
);

/// <summary>CommitStatus holds a single status of a single Commit</summary>
/// <param name="context"></param>
/// <param name="created_at"></param>
/// <param name="creator"></param>
/// <param name="description"></param>
/// <param name="id"></param>
/// <param name="status"></param>
/// <param name="target_url"></param>
/// <param name="updated_at"></param>
/// <param name="url"></param>
public record CommitStatus(
    string? context = default,
    DateTimeOffset? created_at = default,
    User? creator = default,
    string? description = default,
    long? id = default,
    string? status = default,
    string? target_url = default,
    DateTimeOffset? updated_at = default,
    string? url = default
);

/// <summary></summary>
/// <param name="date"></param>
/// <param name="email"></param>
/// <param name="name"></param>
public record CommitUser(
    string? date = default,
    string? email = default,
    string? name = default
);

/// <summary>ContentsResponse contains information about a repo&apos;s entry&apos;s (dir, file, symlink, submodule) metadata and content</summary>
/// <param name="_links"></param>
/// <param name="content">`content` is populated when `type` is `file`, otherwise null</param>
/// <param name="download_url"></param>
/// <param name="encoding">`encoding` is populated when `type` is `file`, otherwise null</param>
/// <param name="git_url"></param>
/// <param name="html_url"></param>
/// <param name="last_commit_sha"></param>
/// <param name="name"></param>
/// <param name="path"></param>
/// <param name="sha"></param>
/// <param name="size"></param>
/// <param name="submodule_git_url">`submodule_git_url` is populated when `type` is `submodule`, otherwise null</param>
/// <param name="target">`target` is populated when `type` is `symlink`, otherwise null</param>
/// <param name="type">`type` will be `file`, `dir`, `symlink`, or `submodule`</param>
/// <param name="url"></param>
public record ContentsResponse(
    FileLinksResponse? _links = default,
    string? content = default,
    string? download_url = default,
    string? encoding = default,
    string? git_url = default,
    string? html_url = default,
    string? last_commit_sha = default,
    string? name = default,
    string? path = default,
    string? sha = default,
    long? size = default,
    string? submodule_git_url = default,
    string? target = default,
    string? type = default,
    string? url = default
);

/// <summary>CreateAccessTokenOption options when create access token</summary>
/// <param name="name"></param>
/// <param name="scopes"></param>
public record CreateAccessTokenOption(
    string name,
    ICollection<string>? scopes = default
);

/// <summary>CreateBranchProtectionOption options for creating a branch protection</summary>
/// <param name="apply_to_admins"></param>
/// <param name="approvals_whitelist_teams"></param>
/// <param name="approvals_whitelist_username"></param>
/// <param name="block_on_official_review_requests"></param>
/// <param name="block_on_outdated_branch"></param>
/// <param name="block_on_rejected_reviews"></param>
/// <param name="branch_name">Deprecated: true</param>
/// <param name="dismiss_stale_approvals"></param>
/// <param name="enable_approvals_whitelist"></param>
/// <param name="enable_merge_whitelist"></param>
/// <param name="enable_push"></param>
/// <param name="enable_push_whitelist"></param>
/// <param name="enable_status_check"></param>
/// <param name="ignore_stale_approvals"></param>
/// <param name="merge_whitelist_teams"></param>
/// <param name="merge_whitelist_usernames"></param>
/// <param name="protected_file_patterns"></param>
/// <param name="push_whitelist_deploy_keys"></param>
/// <param name="push_whitelist_teams"></param>
/// <param name="push_whitelist_usernames"></param>
/// <param name="require_signed_commits"></param>
/// <param name="required_approvals"></param>
/// <param name="rule_name"></param>
/// <param name="status_check_contexts"></param>
/// <param name="unprotected_file_patterns"></param>
public record CreateBranchProtectionOption(
    bool? apply_to_admins = default,
    ICollection<string>? approvals_whitelist_teams = default,
    ICollection<string>? approvals_whitelist_username = default,
    bool? block_on_official_review_requests = default,
    bool? block_on_outdated_branch = default,
    bool? block_on_rejected_reviews = default,
    string? branch_name = default,
    bool? dismiss_stale_approvals = default,
    bool? enable_approvals_whitelist = default,
    bool? enable_merge_whitelist = default,
    bool? enable_push = default,
    bool? enable_push_whitelist = default,
    bool? enable_status_check = default,
    bool? ignore_stale_approvals = default,
    ICollection<string>? merge_whitelist_teams = default,
    ICollection<string>? merge_whitelist_usernames = default,
    string? protected_file_patterns = default,
    bool? push_whitelist_deploy_keys = default,
    ICollection<string>? push_whitelist_teams = default,
    ICollection<string>? push_whitelist_usernames = default,
    bool? require_signed_commits = default,
    long? required_approvals = default,
    string? rule_name = default,
    ICollection<string>? status_check_contexts = default,
    string? unprotected_file_patterns = default
);

/// <summary>CreateBranchRepoOption options when creating a branch in a repository</summary>
/// <param name="new_branch_name">Name of the branch to create</param>
/// <param name="old_branch_name">
/// Deprecated: true
/// Name of the old branch to create from
/// </param>
/// <param name="old_ref_name">Name of the old branch/tag/commit to create from</param>
public record CreateBranchRepoOption(
    string new_branch_name,
    string? old_branch_name = default,
    string? old_ref_name = default
);

/// <summary>CreateEmailOption options when creating email addresses</summary>
/// <param name="emails">email addresses to add</param>
public record CreateEmailOption(
    ICollection<string>? emails = default
);

/// <summary>
/// CreateFileOptions options for creating files
/// Note: `author` and `committer` are optional (if only one is given, it will be used for the other, otherwise the authenticated user will be used)
/// </summary>
/// <param name="content">content must be base64 encoded</param>
/// <param name="author"></param>
/// <param name="branch">branch (optional) to base this file from. if not given, the default branch is used</param>
/// <param name="committer"></param>
/// <param name="dates"></param>
/// <param name="message">message (optional) for the commit of this file. if not supplied, a default message will be used</param>
/// <param name="new_branch">new_branch (optional) will make a new branch from `branch` before creating the file</param>
/// <param name="signoff">Add a Signed-off-by trailer by the committer at the end of the commit log message.</param>
public record CreateFileOptions(
    string content,
    Identity? author = default,
    string? branch = default,
    Identity? committer = default,
    CommitDateOptions? dates = default,
    string? message = default,
    string? new_branch = default,
    bool? signoff = default
);

/// <summary>CreateForkOption options for creating a fork</summary>
/// <param name="name">name of the forked repository</param>
/// <param name="organization">organization name, if forking into an organization</param>
public record CreateForkOption(
    string? name = default,
    string? organization = default
);

/// <summary>CreateGPGKeyOption options create user GPG key</summary>
/// <param name="armored_public_key">An armored GPG key to add</param>
/// <param name="armored_signature"></param>
public record CreateGPGKeyOption(
    string armored_public_key,
    string? armored_signature = default
);

/// <summary>CreateHookOption options when create a hook</summary>
/// <param name="config"></param>
/// <param name="type"></param>
/// <param name="active"></param>
/// <param name="authorization_header"></param>
/// <param name="branch_filter"></param>
/// <param name="events"></param>
[ManualEdit("config プロパティの型を変更")]
public record CreateHookOption(
    IDictionary<string, string> config,
    CreateHookOptionType type,
    bool? active = default,
    string? authorization_header = default,
    string? branch_filter = default,
    ICollection<string>? events = default
);

/// <summary>CreateIssueCommentOption options for creating a comment on an issue</summary>
/// <param name="body"></param>
/// <param name="updated_at"></param>
public record CreateIssueCommentOption(
    string body,
    DateTimeOffset? updated_at = default
);

/// <summary>CreateIssueOption options to create one issue</summary>
/// <param name="title"></param>
/// <param name="assignee">deprecated</param>
/// <param name="assignees"></param>
/// <param name="body"></param>
/// <param name="closed"></param>
/// <param name="due_date"></param>
/// <param name="labels">list of label ids</param>
/// <param name="milestone">milestone id</param>
/// <param name="ref"></param>
public record CreateIssueOption(
    string title,
    string? assignee = default,
    ICollection<string>? assignees = default,
    string? body = default,
    bool? closed = default,
    DateTimeOffset? due_date = default,
    ICollection<long>? labels = default,
    long? milestone = default,
    string? @ref = default
);

/// <summary>CreateKeyOption options when creating a key</summary>
/// <param name="key">An armored SSH key to add</param>
/// <param name="title">Title of the key to add</param>
/// <param name="read_only">Describe if the key has only read access or read/write</param>
public record CreateKeyOption(
    string key,
    string title,
    bool? read_only = default
);

/// <summary>CreateLabelOption options for creating a label</summary>
/// <param name="color"></param>
/// <param name="name"></param>
/// <param name="description"></param>
/// <param name="exclusive"></param>
/// <param name="is_archived"></param>
public record CreateLabelOption(
    string color,
    string name,
    string? description = default,
    bool? exclusive = default,
    bool? is_archived = default
);

/// <summary>CreateMilestoneOption options for creating a milestone</summary>
/// <param name="description"></param>
/// <param name="due_on"></param>
/// <param name="state"></param>
/// <param name="title"></param>
public record CreateMilestoneOption(
    string? description = default,
    DateTimeOffset? due_on = default,
    CreateMilestoneOptionState? state = default,
    string? title = default
);

/// <summary>CreateOAuth2ApplicationOptions holds options to create an oauth2 application</summary>
/// <param name="confidential_client"></param>
/// <param name="name"></param>
/// <param name="redirect_uris"></param>
public record CreateOAuth2ApplicationOptions(
    bool? confidential_client = default,
    string? name = default,
    ICollection<string>? redirect_uris = default
);

/// <summary>CreateOrUpdateSecretOption options when creating or updating secret</summary>
/// <param name="data">Data of the secret to update</param>
public record CreateOrUpdateSecretOption(
    string data
);

/// <summary>CreateOrgOption options for creating an organization</summary>
/// <param name="username"></param>
/// <param name="description"></param>
/// <param name="email"></param>
/// <param name="full_name"></param>
/// <param name="location"></param>
/// <param name="repo_admin_change_team_access"></param>
/// <param name="visibility">possible values are `public` (default), `limited` or `private`</param>
/// <param name="website"></param>
public record CreateOrgOption(
    string username,
    string? description = default,
    string? email = default,
    string? full_name = default,
    string? location = default,
    bool? repo_admin_change_team_access = default,
    CreateOrgOptionVisibility? visibility = default,
    string? website = default
);

/// <summary>CreatePullRequestOption options when creating a pull request</summary>
/// <param name="assignee"></param>
/// <param name="assignees"></param>
/// <param name="base"></param>
/// <param name="body"></param>
/// <param name="due_date"></param>
/// <param name="head"></param>
/// <param name="labels"></param>
/// <param name="milestone"></param>
/// <param name="title"></param>
public record CreatePullRequestOption(
    string? assignee = default,
    ICollection<string>? assignees = default,
    string? @base = default,
    string? body = default,
    DateTimeOffset? due_date = default,
    string? head = default,
    ICollection<long>? labels = default,
    long? milestone = default,
    string? title = default
);

/// <summary>CreatePullReviewComment represent a review comment for creation api</summary>
/// <param name="body"></param>
/// <param name="new_position">if comment to new file line or 0</param>
/// <param name="old_position">if comment to old file line or 0</param>
/// <param name="path">the tree path</param>
public record CreatePullReviewComment(
    string? body = default,
    long? new_position = default,
    long? old_position = default,
    string? path = default
);

/// <summary>CreatePullReviewOptions are options to create a pull review</summary>
/// <param name="body"></param>
/// <param name="comments"></param>
/// <param name="commit_id"></param>
/// <param name="event"></param>
public record CreatePullReviewOptions(
    string? body = default,
    ICollection<CreatePullReviewComment>? comments = default,
    string? commit_id = default,
    string? @event = default
);

/// <summary></summary>
/// <param name="interval"></param>
/// <param name="remote_address"></param>
/// <param name="remote_password"></param>
/// <param name="remote_username"></param>
/// <param name="sync_on_commit"></param>
public record CreatePushMirrorOption(
    string? interval = default,
    string? remote_address = default,
    string? remote_password = default,
    string? remote_username = default,
    bool? sync_on_commit = default
);

/// <summary>CreateReleaseOption options when creating a release</summary>
/// <param name="tag_name"></param>
/// <param name="body"></param>
/// <param name="draft"></param>
/// <param name="name"></param>
/// <param name="prerelease"></param>
/// <param name="target_commitish"></param>
public record CreateReleaseOption(
    string tag_name,
    string? body = default,
    bool? draft = default,
    string? name = default,
    bool? prerelease = default,
    string? target_commitish = default
);

/// <summary>CreateRepoOption options when creating repository</summary>
/// <param name="name">Name of the repository to create</param>
/// <param name="auto_init">Whether the repository should be auto-initialized?</param>
/// <param name="default_branch">DefaultBranch of the repository (used when initializes and in template)</param>
/// <param name="description">Description of the repository to create</param>
/// <param name="gitignores">Gitignores to use</param>
/// <param name="issue_labels">Label-Set to use</param>
/// <param name="license">License to use</param>
/// <param name="object_format_name">ObjectFormatName of the underlying git repository</param>
/// <param name="private">Whether the repository is private</param>
/// <param name="readme">Readme of the repository to create</param>
/// <param name="template">Whether the repository is template</param>
/// <param name="trust_model">TrustModel of the repository</param>
public record CreateRepoOption(
    string name,
    bool? auto_init = default,
    string? default_branch = default,
    string? description = default,
    string? gitignores = default,
    string? issue_labels = default,
    string? license = default,
    CreateRepoOptionObject_format_name? object_format_name = default,
    bool? @private = default,
    string? readme = default,
    bool? template = default,
    CreateRepoOptionTrust_model? trust_model = default
);

/// <summary>CreateStatusOption holds the information needed to create a new CommitStatus for a Commit</summary>
/// <param name="context"></param>
/// <param name="description"></param>
/// <param name="state"></param>
/// <param name="target_url"></param>
public record CreateStatusOption(
    string? context = default,
    string? description = default,
    string? state = default,
    string? target_url = default
);

/// <summary>CreateTagOption options when creating a tag</summary>
/// <param name="tag_name"></param>
/// <param name="message"></param>
/// <param name="target"></param>
public record CreateTagOption(
    string tag_name,
    string? message = default,
    string? target = default
);

/// <summary>CreateTeamOption options for creating a team</summary>
/// <param name="name"></param>
/// <param name="can_create_org_repo"></param>
/// <param name="description"></param>
/// <param name="includes_all_repositories"></param>
/// <param name="permission"></param>
/// <param name="units"></param>
/// <param name="units_map"></param>
public record CreateTeamOption(
    string name,
    bool? can_create_org_repo = default,
    string? description = default,
    bool? includes_all_repositories = default,
    CreateTeamOptionPermission? permission = default,
    ICollection<string>? units = default,
    IDictionary<string, string>? units_map = default
);

/// <summary>CreateUserOption create user options</summary>
/// <param name="email"></param>
/// <param name="username"></param>
/// <param name="created_at">
/// For explicitly setting the user creation timestamp. Useful when users are
/// migrated from other systems. When omitted, the user&apos;s creation timestamp
/// will be set to &quot;now&quot;.
/// </param>
/// <param name="full_name"></param>
/// <param name="login_name"></param>
/// <param name="must_change_password"></param>
/// <param name="password"></param>
/// <param name="restricted"></param>
/// <param name="send_notify"></param>
/// <param name="source_id"></param>
/// <param name="visibility"></param>
public record CreateUserOption(
    string email,
    string username,
    DateTimeOffset? created_at = default,
    string? full_name = default,
    string? login_name = default,
    bool? must_change_password = default,
    string? password = default,
    bool? restricted = default,
    bool? send_notify = default,
    long? source_id = default,
    string? visibility = default
);

/// <summary>CreateWikiPageOptions form for creating wiki</summary>
/// <param name="content_base64">content must be base64 encoded</param>
/// <param name="message">optional commit message summarizing the change</param>
/// <param name="title">page title. leave empty to keep unchanged</param>
public record CreateWikiPageOptions(
    string? content_base64 = default,
    string? message = default,
    string? title = default
);

/// <summary>Cron represents a Cron task</summary>
/// <param name="exec_times"></param>
/// <param name="name"></param>
/// <param name="next"></param>
/// <param name="prev"></param>
/// <param name="schedule"></param>
public record Cron(
    long? exec_times = default,
    string? name = default,
    DateTimeOffset? next = default,
    DateTimeOffset? prev = default,
    string? schedule = default
);

/// <summary>DeleteEmailOption options when deleting email addresses</summary>
/// <param name="emails">email addresses to delete</param>
public record DeleteEmailOption(
    ICollection<string>? emails = default
);

/// <summary>
/// DeleteFileOptions options for deleting files (used for other File structs below)
/// Note: `author` and `committer` are optional (if only one is given, it will be used for the other, otherwise the authenticated user will be used)
/// </summary>
/// <param name="sha">sha is the SHA for the file that already exists</param>
/// <param name="author"></param>
/// <param name="branch">branch (optional) to base this file from. if not given, the default branch is used</param>
/// <param name="committer"></param>
/// <param name="dates"></param>
/// <param name="message">message (optional) for the commit of this file. if not supplied, a default message will be used</param>
/// <param name="new_branch">new_branch (optional) will make a new branch from `branch` before creating the file</param>
/// <param name="signoff">Add a Signed-off-by trailer by the committer at the end of the commit log message.</param>
public record DeleteFileOptions(
    string sha,
    Identity? author = default,
    string? branch = default,
    Identity? committer = default,
    CommitDateOptions? dates = default,
    string? message = default,
    string? new_branch = default,
    bool? signoff = default
);

/// <summary>DeleteLabelOption options for deleting a label</summary>
/// <param name="updated_at"></param>
public record DeleteLabelsOption(
    DateTimeOffset? updated_at = default
);

/// <summary>DeployKey a deploy key</summary>
/// <param name="created_at"></param>
/// <param name="fingerprint"></param>
/// <param name="id"></param>
/// <param name="key"></param>
/// <param name="key_id"></param>
/// <param name="read_only"></param>
/// <param name="repository"></param>
/// <param name="title"></param>
/// <param name="url"></param>
public record DeployKey(
    DateTimeOffset? created_at = default,
    string? fingerprint = default,
    long? id = default,
    string? key = default,
    long? key_id = default,
    bool? read_only = default,
    Repository? repository = default,
    string? title = default,
    string? url = default
);

/// <summary>DismissPullReviewOptions are options to dismiss a pull review</summary>
/// <param name="message"></param>
/// <param name="priors"></param>
public record DismissPullReviewOptions(
    string? message = default,
    bool? priors = default
);

/// <summary>EditAttachmentOptions options for editing attachments</summary>
/// <param name="name"></param>
public record EditAttachmentOptions(
    string? name = default
);

/// <summary>EditBranchProtectionOption options for editing a branch protection</summary>
/// <param name="apply_to_admins"></param>
/// <param name="approvals_whitelist_teams"></param>
/// <param name="approvals_whitelist_username"></param>
/// <param name="block_on_official_review_requests"></param>
/// <param name="block_on_outdated_branch"></param>
/// <param name="block_on_rejected_reviews"></param>
/// <param name="dismiss_stale_approvals"></param>
/// <param name="enable_approvals_whitelist"></param>
/// <param name="enable_merge_whitelist"></param>
/// <param name="enable_push"></param>
/// <param name="enable_push_whitelist"></param>
/// <param name="enable_status_check"></param>
/// <param name="ignore_stale_approvals"></param>
/// <param name="merge_whitelist_teams"></param>
/// <param name="merge_whitelist_usernames"></param>
/// <param name="protected_file_patterns"></param>
/// <param name="push_whitelist_deploy_keys"></param>
/// <param name="push_whitelist_teams"></param>
/// <param name="push_whitelist_usernames"></param>
/// <param name="require_signed_commits"></param>
/// <param name="required_approvals"></param>
/// <param name="status_check_contexts"></param>
/// <param name="unprotected_file_patterns"></param>
public record EditBranchProtectionOption(
    bool? apply_to_admins = default,
    ICollection<string>? approvals_whitelist_teams = default,
    ICollection<string>? approvals_whitelist_username = default,
    bool? block_on_official_review_requests = default,
    bool? block_on_outdated_branch = default,
    bool? block_on_rejected_reviews = default,
    bool? dismiss_stale_approvals = default,
    bool? enable_approvals_whitelist = default,
    bool? enable_merge_whitelist = default,
    bool? enable_push = default,
    bool? enable_push_whitelist = default,
    bool? enable_status_check = default,
    bool? ignore_stale_approvals = default,
    ICollection<string>? merge_whitelist_teams = default,
    ICollection<string>? merge_whitelist_usernames = default,
    string? protected_file_patterns = default,
    bool? push_whitelist_deploy_keys = default,
    ICollection<string>? push_whitelist_teams = default,
    ICollection<string>? push_whitelist_usernames = default,
    bool? require_signed_commits = default,
    long? required_approvals = default,
    ICollection<string>? status_check_contexts = default,
    string? unprotected_file_patterns = default
);

/// <summary>EditDeadlineOption options for creating a deadline</summary>
/// <param name="due_date"></param>
public record EditDeadlineOption(
    DateTimeOffset due_date
);

/// <summary>EditGitHookOption options when modifying one Git hook</summary>
/// <param name="content"></param>
public record EditGitHookOption(
    string? content = default
);

/// <summary>EditHookOption options when modify one hook</summary>
/// <param name="active"></param>
/// <param name="authorization_header"></param>
/// <param name="branch_filter"></param>
/// <param name="config"></param>
/// <param name="events"></param>
public record EditHookOption(
    bool? active = default,
    string? authorization_header = default,
    string? branch_filter = default,
    IDictionary<string, string>? config = default,
    ICollection<string>? events = default
);

/// <summary>EditIssueCommentOption options for editing a comment</summary>
/// <param name="body"></param>
/// <param name="updated_at"></param>
public record EditIssueCommentOption(
    string body,
    DateTimeOffset? updated_at = default
);

/// <summary>EditIssueOption options for editing an issue</summary>
/// <param name="assignee">deprecated</param>
/// <param name="assignees"></param>
/// <param name="body"></param>
/// <param name="due_date"></param>
/// <param name="milestone"></param>
/// <param name="ref"></param>
/// <param name="state"></param>
/// <param name="title"></param>
/// <param name="unset_due_date"></param>
/// <param name="updated_at"></param>
public record EditIssueOption(
    string? assignee = default,
    ICollection<string>? assignees = default,
    string? body = default,
    DateTimeOffset? due_date = default,
    long? milestone = default,
    string? @ref = default,
    string? state = default,
    string? title = default,
    bool? unset_due_date = default,
    DateTimeOffset? updated_at = default
);

/// <summary>EditLabelOption options for editing a label</summary>
/// <param name="color"></param>
/// <param name="description"></param>
/// <param name="exclusive"></param>
/// <param name="is_archived"></param>
/// <param name="name"></param>
public record EditLabelOption(
    string? color = default,
    string? description = default,
    bool? exclusive = default,
    bool? is_archived = default,
    string? name = default
);

/// <summary>EditMilestoneOption options for editing a milestone</summary>
/// <param name="description"></param>
/// <param name="due_on"></param>
/// <param name="state"></param>
/// <param name="title"></param>
public record EditMilestoneOption(
    string? description = default,
    DateTimeOffset? due_on = default,
    string? state = default,
    string? title = default
);

/// <summary>EditOrgOption options for editing an organization</summary>
/// <param name="description"></param>
/// <param name="email"></param>
/// <param name="full_name"></param>
/// <param name="location"></param>
/// <param name="repo_admin_change_team_access"></param>
/// <param name="visibility">possible values are `public`, `limited` or `private`</param>
/// <param name="website"></param>
public record EditOrgOption(
    string? description = default,
    string? email = default,
    string? full_name = default,
    string? location = default,
    bool? repo_admin_change_team_access = default,
    EditOrgOptionVisibility? visibility = default,
    string? website = default
);

/// <summary>EditPullRequestOption options when modify pull request</summary>
/// <param name="allow_maintainer_edit"></param>
/// <param name="assignee"></param>
/// <param name="assignees"></param>
/// <param name="base"></param>
/// <param name="body"></param>
/// <param name="due_date"></param>
/// <param name="labels"></param>
/// <param name="milestone"></param>
/// <param name="state"></param>
/// <param name="title"></param>
/// <param name="unset_due_date"></param>
public record EditPullRequestOption(
    bool? allow_maintainer_edit = default,
    string? assignee = default,
    ICollection<string>? assignees = default,
    string? @base = default,
    string? body = default,
    DateTimeOffset? due_date = default,
    ICollection<long>? labels = default,
    long? milestone = default,
    string? state = default,
    string? title = default,
    bool? unset_due_date = default
);

/// <summary>EditReactionOption contain the reaction type</summary>
/// <param name="content"></param>
public record EditReactionOption(
    string? content = default
);

/// <summary>EditReleaseOption options when editing a release</summary>
/// <param name="body"></param>
/// <param name="draft"></param>
/// <param name="name"></param>
/// <param name="prerelease"></param>
/// <param name="tag_name"></param>
/// <param name="target_commitish"></param>
public record EditReleaseOption(
    string? body = default,
    bool? draft = default,
    string? name = default,
    bool? prerelease = default,
    string? tag_name = default,
    string? target_commitish = default
);

/// <summary>EditRepoOption options when editing a repository&apos;s properties</summary>
/// <param name="allow_fast_forward_only_merge">either `true` to allow fast-forward-only merging pull requests, or `false` to prevent fast-forward-only merging.</param>
/// <param name="allow_manual_merge">either `true` to allow mark pr as merged manually, or `false` to prevent it.</param>
/// <param name="allow_merge_commits">either `true` to allow merging pull requests with a merge commit, or `false` to prevent merging pull requests with merge commits.</param>
/// <param name="allow_rebase">either `true` to allow rebase-merging pull requests, or `false` to prevent rebase-merging.</param>
/// <param name="allow_rebase_explicit">either `true` to allow rebase with explicit merge commits (--no-ff), or `false` to prevent rebase with explicit merge commits.</param>
/// <param name="allow_rebase_update">either `true` to allow updating pull request branch by rebase, or `false` to prevent it.</param>
/// <param name="allow_squash_merge">either `true` to allow squash-merging pull requests, or `false` to prevent squash-merging.</param>
/// <param name="archived">set to `true` to archive this repository.</param>
/// <param name="autodetect_manual_merge">either `true` to enable AutodetectManualMerge, or `false` to prevent it. Note: In some special cases, misjudgments can occur.</param>
/// <param name="default_allow_maintainer_edit">set to `true` to allow edits from maintainers by default</param>
/// <param name="default_branch">sets the default branch for this repository.</param>
/// <param name="default_delete_branch_after_merge">set to `true` to delete pr branch after merge by default</param>
/// <param name="default_merge_style">set to a merge style to be used by this repository: &quot;merge&quot;, &quot;rebase&quot;, &quot;rebase-merge&quot;, &quot;squash&quot;, or &quot;fast-forward-only&quot;.</param>
/// <param name="description">a short description of the repository.</param>
/// <param name="enable_prune">enable prune - remove obsolete remote-tracking references when mirroring</param>
/// <param name="external_tracker"></param>
/// <param name="external_wiki"></param>
/// <param name="has_actions">either `true` to enable actions unit, or `false` to disable them.</param>
/// <param name="has_issues">either `true` to enable issues for this repository or `false` to disable them.</param>
/// <param name="has_packages">either `true` to enable packages unit, or `false` to disable them.</param>
/// <param name="has_projects">either `true` to enable project unit, or `false` to disable them.</param>
/// <param name="has_pull_requests">either `true` to allow pull requests, or `false` to prevent pull request.</param>
/// <param name="has_releases">either `true` to enable releases unit, or `false` to disable them.</param>
/// <param name="has_wiki">either `true` to enable the wiki for this repository or `false` to disable it.</param>
/// <param name="ignore_whitespace_conflicts">either `true` to ignore whitespace for conflicts, or `false` to not ignore whitespace.</param>
/// <param name="internal_tracker"></param>
/// <param name="mirror_interval">set to a string like `8h30m0s` to set the mirror interval time</param>
/// <param name="name">name of the repository</param>
/// <param name="private">
/// either `true` to make the repository private or `false` to make it public.
/// Note: you will get a 422 error if the organization restricts changing repository visibility to organization
/// owners and a non-owner tries to change the value of private.
/// </param>
/// <param name="template">either `true` to make this repository a template or `false` to make it a normal repository</param>
/// <param name="website">a URL with more information about the repository.</param>
/// <param name="wiki_branch">sets the branch used for this repository&apos;s wiki.</param>
public record EditRepoOption(
    bool? allow_fast_forward_only_merge = default,
    bool? allow_manual_merge = default,
    bool? allow_merge_commits = default,
    bool? allow_rebase = default,
    bool? allow_rebase_explicit = default,
    bool? allow_rebase_update = default,
    bool? allow_squash_merge = default,
    bool? archived = default,
    bool? autodetect_manual_merge = default,
    bool? default_allow_maintainer_edit = default,
    string? default_branch = default,
    bool? default_delete_branch_after_merge = default,
    string? default_merge_style = default,
    string? description = default,
    bool? enable_prune = default,
    ExternalTracker? external_tracker = default,
    ExternalWiki? external_wiki = default,
    bool? has_actions = default,
    bool? has_issues = default,
    bool? has_packages = default,
    bool? has_projects = default,
    bool? has_pull_requests = default,
    bool? has_releases = default,
    bool? has_wiki = default,
    bool? ignore_whitespace_conflicts = default,
    InternalTracker? internal_tracker = default,
    string? mirror_interval = default,
    string? name = default,
    bool? @private = default,
    bool? template = default,
    string? website = default,
    string? wiki_branch = default
);

/// <summary>EditTeamOption options for editing a team</summary>
/// <param name="name"></param>
/// <param name="can_create_org_repo"></param>
/// <param name="description"></param>
/// <param name="includes_all_repositories"></param>
/// <param name="permission"></param>
/// <param name="units"></param>
/// <param name="units_map"></param>
public record EditTeamOption(
    string name,
    bool? can_create_org_repo = default,
    string? description = default,
    bool? includes_all_repositories = default,
    EditTeamOptionPermission? permission = default,
    ICollection<string>? units = default,
    IDictionary<string, string>? units_map = default
);

/// <summary>EditUserOption edit user options</summary>
/// <param name="login_name"></param>
/// <param name="source_id"></param>
/// <param name="active"></param>
/// <param name="admin"></param>
/// <param name="allow_create_organization"></param>
/// <param name="allow_git_hook"></param>
/// <param name="allow_import_local"></param>
/// <param name="description"></param>
/// <param name="email"></param>
/// <param name="full_name"></param>
/// <param name="location"></param>
/// <param name="max_repo_creation"></param>
/// <param name="must_change_password"></param>
/// <param name="password"></param>
/// <param name="prohibit_login"></param>
/// <param name="pronouns"></param>
/// <param name="restricted"></param>
/// <param name="visibility"></param>
/// <param name="website"></param>
public record EditUserOption(
    string login_name,
    long source_id,
    bool? active = default,
    bool? admin = default,
    bool? allow_create_organization = default,
    bool? allow_git_hook = default,
    bool? allow_import_local = default,
    string? description = default,
    string? email = default,
    string? full_name = default,
    string? location = default,
    long? max_repo_creation = default,
    bool? must_change_password = default,
    string? password = default,
    bool? prohibit_login = default,
    string? pronouns = default,
    bool? restricted = default,
    string? visibility = default,
    string? website = default
);

/// <summary>Email an email address belonging to a user</summary>
/// <param name="email"></param>
/// <param name="primary"></param>
/// <param name="user_id"></param>
/// <param name="username"></param>
/// <param name="verified"></param>
public record Email(
    string? email = default,
    bool? primary = default,
    long? user_id = default,
    string? username = default,
    bool? verified = default
);

/// <summary>ExternalTracker represents settings for external tracker</summary>
/// <param name="external_tracker_format">External Issue Tracker URL Format. Use the placeholders {user}, {repo} and {index} for the username, repository name and issue index.</param>
/// <param name="external_tracker_regexp_pattern">External Issue Tracker issue regular expression</param>
/// <param name="external_tracker_style">External Issue Tracker Number Format, either `numeric`, `alphanumeric`, or `regexp`</param>
/// <param name="external_tracker_url">URL of external issue tracker.</param>
public record ExternalTracker(
    string? external_tracker_format = default,
    string? external_tracker_regexp_pattern = default,
    string? external_tracker_style = default,
    string? external_tracker_url = default
);

/// <summary>ExternalWiki represents setting for external wiki</summary>
/// <param name="external_wiki_url">URL of external wiki.</param>
public record ExternalWiki(
    string? external_wiki_url = default
);

/// <summary></summary>
/// <param name="author"></param>
/// <param name="committer"></param>
/// <param name="created"></param>
/// <param name="html_url"></param>
/// <param name="message"></param>
/// <param name="parents"></param>
/// <param name="sha"></param>
/// <param name="tree"></param>
/// <param name="url"></param>
public record FileCommitResponse(
    CommitUser? author = default,
    CommitUser? committer = default,
    DateTimeOffset? created = default,
    string? html_url = default,
    string? message = default,
    ICollection<CommitMeta>? parents = default,
    string? sha = default,
    CommitMeta? tree = default,
    string? url = default
);

/// <summary>FileDeleteResponse contains information about a repo&apos;s file that was deleted</summary>
/// <param name="commit"></param>
/// <param name="content"></param>
/// <param name="verification"></param>
public record FileDeleteResponse(
    FileCommitResponse? commit = default,
    object? content = default,
    PayloadCommitVerification? verification = default
);

/// <summary>FileLinksResponse contains the links for a repo&apos;s file</summary>
/// <param name="git"></param>
/// <param name="html"></param>
/// <param name="self"></param>
public record FileLinksResponse(
    string? git = default,
    string? html = default,
    string? self = default
);

/// <summary>FileResponse contains information about a repo&apos;s file</summary>
/// <param name="commit"></param>
/// <param name="content"></param>
/// <param name="verification"></param>
public record FileResponse(
    FileCommitResponse? commit = default,
    ContentsResponse? content = default,
    PayloadCommitVerification? verification = default
);

/// <summary>FilesResponse contains information about multiple files from a repo</summary>
/// <param name="commit"></param>
/// <param name="files"></param>
/// <param name="verification"></param>
public record FilesResponse(
    FileCommitResponse? commit = default,
    ICollection<ContentsResponse>? files = default,
    PayloadCommitVerification? verification = default
);

/// <summary>GPGKey a user GPG key to sign commit and tag in repository</summary>
/// <param name="can_certify"></param>
/// <param name="can_encrypt_comms"></param>
/// <param name="can_encrypt_storage"></param>
/// <param name="can_sign"></param>
/// <param name="created_at"></param>
/// <param name="emails"></param>
/// <param name="expires_at"></param>
/// <param name="id"></param>
/// <param name="key_id"></param>
/// <param name="primary_key_id"></param>
/// <param name="public_key"></param>
/// <param name="subkeys"></param>
/// <param name="verified"></param>
public record GPGKey(
    bool? can_certify = default,
    bool? can_encrypt_comms = default,
    bool? can_encrypt_storage = default,
    bool? can_sign = default,
    DateTimeOffset? created_at = default,
    ICollection<GPGKeyEmail>? emails = default,
    DateTimeOffset? expires_at = default,
    long? id = default,
    string? key_id = default,
    string? primary_key_id = default,
    string? public_key = default,
    ICollection<GPGKey>? subkeys = default,
    bool? verified = default
);

/// <summary>GPGKeyEmail an email attached to a GPGKey</summary>
/// <param name="email"></param>
/// <param name="verified"></param>
public record GPGKeyEmail(
    string? email = default,
    bool? verified = default
);

/// <summary>GeneralAPISettings contains global api settings exposed by it</summary>
/// <param name="default_git_trees_per_page"></param>
/// <param name="default_max_blob_size"></param>
/// <param name="default_paging_num"></param>
/// <param name="max_response_items"></param>
public record GeneralAPISettings(
    long? default_git_trees_per_page = default,
    long? default_max_blob_size = default,
    long? default_paging_num = default,
    long? max_response_items = default
);

/// <summary>GeneralAttachmentSettings contains global Attachment settings exposed by API</summary>
/// <param name="allowed_types"></param>
/// <param name="enabled"></param>
/// <param name="max_files"></param>
/// <param name="max_size"></param>
public record GeneralAttachmentSettings(
    string? allowed_types = default,
    bool? enabled = default,
    long? max_files = default,
    long? max_size = default
);

/// <summary>GeneralRepoSettings contains global repository settings exposed by API</summary>
/// <param name="forks_disabled"></param>
/// <param name="http_git_disabled"></param>
/// <param name="lfs_disabled"></param>
/// <param name="migrations_disabled"></param>
/// <param name="mirrors_disabled"></param>
/// <param name="stars_disabled"></param>
/// <param name="time_tracking_disabled"></param>
public record GeneralRepoSettings(
    bool? forks_disabled = default,
    bool? http_git_disabled = default,
    bool? lfs_disabled = default,
    bool? migrations_disabled = default,
    bool? mirrors_disabled = default,
    bool? stars_disabled = default,
    bool? time_tracking_disabled = default
);

/// <summary>GeneralUISettings contains global ui settings exposed by API</summary>
/// <param name="allowed_reactions"></param>
/// <param name="custom_emojis"></param>
/// <param name="default_theme"></param>
public record GeneralUISettings(
    ICollection<string>? allowed_reactions = default,
    ICollection<string>? custom_emojis = default,
    string? default_theme = default
);

/// <summary>GenerateRepoOption options when creating repository using a template</summary>
/// <param name="name">Name of the repository to create</param>
/// <param name="owner">The organization or person who will own the new repository</param>
/// <param name="avatar">include avatar of the template repo</param>
/// <param name="default_branch">Default branch of the new repository</param>
/// <param name="description">Description of the repository to create</param>
/// <param name="git_content">include git content of default branch in template repo</param>
/// <param name="git_hooks">include git hooks in template repo</param>
/// <param name="labels">include labels in template repo</param>
/// <param name="private">Whether the repository is private</param>
/// <param name="protected_branch">include protected branches in template repo</param>
/// <param name="topics">include topics in template repo</param>
/// <param name="webhooks">include webhooks in template repo</param>
public record GenerateRepoOption(
    string name,
    string owner,
    bool? avatar = default,
    string? default_branch = default,
    string? description = default,
    bool? git_content = default,
    bool? git_hooks = default,
    bool? labels = default,
    bool? @private = default,
    bool? protected_branch = default,
    bool? topics = default,
    bool? webhooks = default
);

/// <summary>GitBlobResponse represents a git blob</summary>
/// <param name="content"></param>
/// <param name="encoding"></param>
/// <param name="sha"></param>
/// <param name="size"></param>
/// <param name="url"></param>
public record GitBlobResponse(
    string? content = default,
    string? encoding = default,
    string? sha = default,
    long? size = default,
    string? url = default
);

/// <summary>GitEntry represents a git tree</summary>
/// <param name="mode"></param>
/// <param name="path"></param>
/// <param name="sha"></param>
/// <param name="size"></param>
/// <param name="type"></param>
/// <param name="url"></param>
public record GitEntry(
    string? mode = default,
    string? path = default,
    string? sha = default,
    long? size = default,
    string? type = default,
    string? url = default
);

/// <summary>GitHook represents a Git repository hook</summary>
/// <param name="content"></param>
/// <param name="is_active"></param>
/// <param name="name"></param>
public record GitHook(
    string? content = default,
    bool? is_active = default,
    string? name = default
);

/// <summary></summary>
/// <param name="sha"></param>
/// <param name="type"></param>
/// <param name="url"></param>
public record GitObject(
    string? sha = default,
    string? type = default,
    string? url = default
);

/// <summary>GitTreeResponse returns a git tree</summary>
/// <param name="page"></param>
/// <param name="sha"></param>
/// <param name="total_count"></param>
/// <param name="tree"></param>
/// <param name="truncated"></param>
/// <param name="url"></param>
public record GitTreeResponse(
    long? page = default,
    string? sha = default,
    long? total_count = default,
    ICollection<GitEntry>? tree = default,
    bool? truncated = default,
    string? url = default
);

/// <summary>GitignoreTemplateInfo name and text of a gitignore template</summary>
/// <param name="name"></param>
/// <param name="source"></param>
public record GitignoreTemplateInfo(
    string? name = default,
    string? source = default
);

/// <summary>Hook a hook is a web hook when one repository changed</summary>
/// <param name="active"></param>
/// <param name="authorization_header"></param>
/// <param name="branch_filter"></param>
/// <param name="config">Deprecated: use Metadata instead</param>
/// <param name="content_type"></param>
/// <param name="created_at"></param>
/// <param name="events"></param>
/// <param name="id"></param>
/// <param name="metadata"></param>
/// <param name="type"></param>
/// <param name="updated_at"></param>
/// <param name="url"></param>
public record Hook(
    bool? active = default,
    string? authorization_header = default,
    string? branch_filter = default,
    IDictionary<string, string>? config = default,
    string? content_type = default,
    DateTimeOffset? created_at = default,
    ICollection<string>? events = default,
    long? id = default,
    object? metadata = default,
    string? type = default,
    DateTimeOffset? updated_at = default,
    string? url = default
);

/// <summary>Identity for a person&apos;s identity like an author or committer</summary>
/// <param name="email"></param>
/// <param name="name"></param>
public record Identity(
    string? email = default,
    string? name = default
);

/// <summary>InternalTracker represents settings for internal tracker</summary>
/// <param name="allow_only_contributors_to_track_time">Let only contributors track time (Built-in issue tracker)</param>
/// <param name="enable_issue_dependencies">Enable dependencies for issues and pull requests (Built-in issue tracker)</param>
/// <param name="enable_time_tracker">Enable time tracking (Built-in issue tracker)</param>
public record InternalTracker(
    bool? allow_only_contributors_to_track_time = default,
    bool? enable_issue_dependencies = default,
    bool? enable_time_tracker = default
);

/// <summary>Issue represents an issue in a repository</summary>
/// <param name="assets"></param>
/// <param name="assignee"></param>
/// <param name="assignees"></param>
/// <param name="body"></param>
/// <param name="closed_at"></param>
/// <param name="comments"></param>
/// <param name="created_at"></param>
/// <param name="due_date"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="is_locked"></param>
/// <param name="labels"></param>
/// <param name="milestone"></param>
/// <param name="number"></param>
/// <param name="original_author"></param>
/// <param name="original_author_id"></param>
/// <param name="pin_order"></param>
/// <param name="pull_request"></param>
/// <param name="ref"></param>
/// <param name="repository"></param>
/// <param name="state"></param>
/// <param name="title"></param>
/// <param name="updated_at"></param>
/// <param name="url"></param>
/// <param name="user"></param>
public record Issue(
    ICollection<Attachment>? assets = default,
    User? assignee = default,
    ICollection<User>? assignees = default,
    string? body = default,
    DateTimeOffset? closed_at = default,
    long? comments = default,
    DateTimeOffset? created_at = default,
    DateTimeOffset? due_date = default,
    string? html_url = default,
    long? id = default,
    bool? is_locked = default,
    ICollection<Label>? labels = default,
    Milestone? milestone = default,
    long? number = default,
    string? original_author = default,
    long? original_author_id = default,
    long? pin_order = default,
    PullRequestMeta? pull_request = default,
    string? @ref = default,
    RepositoryMeta? repository = default,
    string? state = default,
    string? title = default,
    DateTimeOffset? updated_at = default,
    string? url = default,
    User? user = default
);

/// <summary></summary>
/// <param name="blank_issues_enabled"></param>
/// <param name="contact_links"></param>
public record IssueConfig(
    bool? blank_issues_enabled = default,
    ICollection<IssueConfigContactLink>? contact_links = default
);

/// <summary></summary>
/// <param name="about"></param>
/// <param name="name"></param>
/// <param name="url"></param>
public record IssueConfigContactLink(
    string? about = default,
    string? name = default,
    string? url = default
);

/// <summary></summary>
/// <param name="message"></param>
/// <param name="valid"></param>
public record IssueConfigValidation(
    string? message = default,
    bool? valid = default
);

/// <summary>IssueDeadline represents an issue deadline</summary>
/// <param name="due_date"></param>
public record IssueDeadline(
    DateTimeOffset? due_date = default
);

/// <summary>IssueFormField represents a form field</summary>
/// <param name="attributes"></param>
/// <param name="id"></param>
/// <param name="type"></param>
/// <param name="validations"></param>
/// <param name="visible"></param>
public record IssueFormField(
    IDictionary<string, object>? attributes = default,
    string? id = default,
    string? type = default,
    IDictionary<string, object>? validations = default,
    ICollection<string>? visible = default
);

/// <summary>IssueLabelsOption a collection of labels</summary>
/// <param name="labels">list of label IDs</param>
/// <param name="updated_at"></param>
public record IssueLabelsOption(
    ICollection<long>? labels = default,
    DateTimeOffset? updated_at = default
);

/// <summary>IssueMeta basic issue information</summary>
/// <param name="index"></param>
/// <param name="owner"></param>
/// <param name="repo"></param>
public record IssueMeta(
    long? index = default,
    string? owner = default,
    string? repo = default
);

/// <summary>IssueTemplate represents an issue template for a repository</summary>
/// <param name="about"></param>
/// <param name="body"></param>
/// <param name="content"></param>
/// <param name="file_name"></param>
/// <param name="labels"></param>
/// <param name="name"></param>
/// <param name="ref"></param>
/// <param name="title"></param>
[ManualEdit("labels プロパティの型を変更")]
public record IssueTemplate(
    string? about = default,
    ICollection<IssueFormField>? body = default,
    string? content = default,
    string? file_name = default,
    string[]? labels = default,
    string? name = default,
    string? @ref = default,
    string? title = default
);

/// <summary>Label a label to an issue or a pr</summary>
/// <param name="color"></param>
/// <param name="description"></param>
/// <param name="exclusive"></param>
/// <param name="id"></param>
/// <param name="is_archived"></param>
/// <param name="name"></param>
/// <param name="url"></param>
public record Label(
    string? color = default,
    string? description = default,
    bool? exclusive = default,
    long? id = default,
    bool? is_archived = default,
    string? name = default,
    string? url = default
);

/// <summary>LabelTemplate info of a Label template</summary>
/// <param name="color"></param>
/// <param name="description"></param>
/// <param name="exclusive"></param>
/// <param name="name"></param>
public record LabelTemplate(
    string? color = default,
    string? description = default,
    bool? exclusive = default,
    string? name = default
);

/// <summary>LicensesInfo contains information about a License</summary>
/// <param name="body"></param>
/// <param name="implementation"></param>
/// <param name="key"></param>
/// <param name="name"></param>
/// <param name="url"></param>
public record LicenseTemplateInfo(
    string? body = default,
    string? implementation = default,
    string? key = default,
    string? name = default,
    string? url = default
);

/// <summary>LicensesListEntry is used for the API</summary>
/// <param name="key"></param>
/// <param name="name"></param>
/// <param name="url"></param>
public record LicensesTemplateListEntry(
    string? key = default,
    string? name = default,
    string? url = default
);

/// <summary>MarkdownOption markdown options</summary>
/// <param name="Context">
/// Context to render
/// 
/// in: body
/// </param>
/// <param name="Mode">
/// Mode to render (comment, gfm, markdown)
/// 
/// in: body
/// </param>
/// <param name="Text">
/// Text markdown to render
/// 
/// in: body
/// </param>
/// <param name="Wiki">
/// Is it a wiki page ?
/// 
/// in: body
/// </param>
public record MarkdownOption(
    string? Context = default,
    string? Mode = default,
    string? Text = default,
    bool? Wiki = default
);

/// <summary>MarkupOption markup options</summary>
/// <param name="Context">
/// Context to render
/// 
/// in: body
/// </param>
/// <param name="FilePath">
/// File path for detecting extension in file mode
/// 
/// in: body
/// </param>
/// <param name="Mode">
/// Mode to render (comment, gfm, markdown, file)
/// 
/// in: body
/// </param>
/// <param name="Text">
/// Text markup to render
/// 
/// in: body
/// </param>
/// <param name="Wiki">
/// Is it a wiki page ?
/// 
/// in: body
/// </param>
public record MarkupOption(
    string? Context = default,
    string? FilePath = default,
    string? Mode = default,
    string? Text = default,
    bool? Wiki = default
);

/// <summary>MergePullRequestForm form for merging Pull Request</summary>
/// <param name="Do"></param>
/// <param name="MergeCommitID"></param>
/// <param name="MergeMessageField"></param>
/// <param name="MergeTitleField"></param>
/// <param name="delete_branch_after_merge"></param>
/// <param name="force_merge"></param>
/// <param name="head_commit_id"></param>
/// <param name="merge_when_checks_succeed"></param>
public record MergePullRequestOption(
    MergePullRequestOptionDo Do,
    string? MergeCommitID = default,
    string? MergeMessageField = default,
    string? MergeTitleField = default,
    bool? delete_branch_after_merge = default,
    bool? force_merge = default,
    string? head_commit_id = default,
    bool? merge_when_checks_succeed = default
);

/// <summary>
/// MigrateRepoOptions options for migrating repository&apos;s
/// this is used to interact with api v1
/// </summary>
/// <param name="clone_addr"></param>
/// <param name="repo_name"></param>
/// <param name="auth_password"></param>
/// <param name="auth_token"></param>
/// <param name="auth_username"></param>
/// <param name="description"></param>
/// <param name="issues"></param>
/// <param name="labels"></param>
/// <param name="lfs"></param>
/// <param name="lfs_endpoint"></param>
/// <param name="milestones"></param>
/// <param name="mirror"></param>
/// <param name="mirror_interval"></param>
/// <param name="private"></param>
/// <param name="pull_requests"></param>
/// <param name="releases"></param>
/// <param name="repo_owner">Name of User or Organisation who will own Repo after migration</param>
/// <param name="service"></param>
/// <param name="uid">deprecated (only for backwards compatibility)</param>
/// <param name="wiki"></param>
public record MigrateRepoOptions(
    string clone_addr,
    string repo_name,
    string? auth_password = default,
    string? auth_token = default,
    string? auth_username = default,
    string? description = default,
    bool? issues = default,
    bool? labels = default,
    bool? lfs = default,
    string? lfs_endpoint = default,
    bool? milestones = default,
    bool? mirror = default,
    string? mirror_interval = default,
    bool? @private = default,
    bool? pull_requests = default,
    bool? releases = default,
    string? repo_owner = default,
    MigrateRepoOptionsService? service = default,
    long? uid = default,
    bool? wiki = default
);

/// <summary>Milestone milestone is a collection of issues on one repository</summary>
/// <param name="closed_at"></param>
/// <param name="closed_issues"></param>
/// <param name="created_at"></param>
/// <param name="description"></param>
/// <param name="due_on"></param>
/// <param name="id"></param>
/// <param name="open_issues"></param>
/// <param name="state"></param>
/// <param name="title"></param>
/// <param name="updated_at"></param>
public record Milestone(
    DateTimeOffset? closed_at = default,
    long? closed_issues = default,
    DateTimeOffset? created_at = default,
    string? description = default,
    DateTimeOffset? due_on = default,
    long? id = default,
    long? open_issues = default,
    string? state = default,
    string? title = default,
    DateTimeOffset? updated_at = default
);

/// <summary>NewIssuePinsAllowed represents an API response that says if new Issue Pins are allowed</summary>
/// <param name="issues"></param>
/// <param name="pull_requests"></param>
public record NewIssuePinsAllowed(
    bool? issues = default,
    bool? pull_requests = default
);

/// <summary>NodeInfo contains standardized way of exposing metadata about a server running one of the distributed social networks</summary>
/// <param name="metadata"></param>
/// <param name="openRegistrations"></param>
/// <param name="protocols"></param>
/// <param name="services"></param>
/// <param name="software"></param>
/// <param name="usage"></param>
/// <param name="version"></param>
public record NodeInfo(
    object? metadata = default,
    bool? openRegistrations = default,
    ICollection<string>? protocols = default,
    NodeInfoServices? services = default,
    NodeInfoSoftware? software = default,
    NodeInfoUsage? usage = default,
    string? version = default
);

/// <summary>NodeInfoServices contains the third party sites this server can connect to via their application API</summary>
/// <param name="inbound"></param>
/// <param name="outbound"></param>
public record NodeInfoServices(
    ICollection<string>? inbound = default,
    ICollection<string>? outbound = default
);

/// <summary>NodeInfoSoftware contains Metadata about server software in use</summary>
/// <param name="homepage"></param>
/// <param name="name"></param>
/// <param name="repository"></param>
/// <param name="version"></param>
public record NodeInfoSoftware(
    string? homepage = default,
    string? name = default,
    string? repository = default,
    string? version = default
);

/// <summary>NodeInfoUsage contains usage statistics for this server</summary>
/// <param name="localComments"></param>
/// <param name="localPosts"></param>
/// <param name="users"></param>
public record NodeInfoUsage(
    long? localComments = default,
    long? localPosts = default,
    NodeInfoUsageUsers? users = default
);

/// <summary>NodeInfoUsageUsers contains statistics about the users of this server</summary>
/// <param name="activeHalfyear"></param>
/// <param name="activeMonth"></param>
/// <param name="total"></param>
public record NodeInfoUsageUsers(
    long? activeHalfyear = default,
    long? activeMonth = default,
    long? total = default
);

/// <summary>Note contains information related to a git note</summary>
/// <param name="commit"></param>
/// <param name="message"></param>
public record Note(
    Commit? commit = default,
    string? message = default
);

/// <summary>NotificationCount number of unread notifications</summary>
/// <param name="new"></param>
public record NotificationCount(
    long? @new = default
);

/// <summary>NotificationSubject contains the notification subject (Issue/Pull/Commit)</summary>
/// <param name="html_url"></param>
/// <param name="latest_comment_html_url"></param>
/// <param name="latest_comment_url"></param>
/// <param name="state"></param>
/// <param name="title"></param>
/// <param name="type"></param>
/// <param name="url"></param>
public record NotificationSubject(
    string? html_url = default,
    string? latest_comment_html_url = default,
    string? latest_comment_url = default,
    string? state = default,
    string? title = default,
    string? type = default,
    string? url = default
);

/// <summary>NotificationThread expose Notification on API</summary>
/// <param name="id"></param>
/// <param name="pinned"></param>
/// <param name="repository"></param>
/// <param name="subject"></param>
/// <param name="unread"></param>
/// <param name="updated_at"></param>
/// <param name="url"></param>
public record NotificationThread(
    long? id = default,
    bool? pinned = default,
    Repository? repository = default,
    NotificationSubject? subject = default,
    bool? unread = default,
    DateTimeOffset? updated_at = default,
    string? url = default
);

/// <summary></summary>
/// <param name="client_id"></param>
/// <param name="client_secret"></param>
/// <param name="confidential_client"></param>
/// <param name="created"></param>
/// <param name="id"></param>
/// <param name="name"></param>
/// <param name="redirect_uris"></param>
public record OAuth2Application(
    string? client_id = default,
    string? client_secret = default,
    bool? confidential_client = default,
    DateTimeOffset? created = default,
    long? id = default,
    string? name = default,
    ICollection<string>? redirect_uris = default
);

/// <summary>Organization represents an organization</summary>
/// <param name="avatar_url"></param>
/// <param name="description"></param>
/// <param name="email"></param>
/// <param name="full_name"></param>
/// <param name="id"></param>
/// <param name="location"></param>
/// <param name="name"></param>
/// <param name="repo_admin_change_team_access"></param>
/// <param name="username">deprecated</param>
/// <param name="visibility"></param>
/// <param name="website"></param>
public record Organization(
    string? avatar_url = default,
    string? description = default,
    string? email = default,
    string? full_name = default,
    long? id = default,
    string? location = default,
    string? name = default,
    bool? repo_admin_change_team_access = default,
    string? username = default,
    string? visibility = default,
    string? website = default
);

/// <summary>OrganizationPermissions list different users permissions on an organization</summary>
/// <param name="can_create_repository"></param>
/// <param name="can_read"></param>
/// <param name="can_write"></param>
/// <param name="is_admin"></param>
/// <param name="is_owner"></param>
public record OrganizationPermissions(
    bool? can_create_repository = default,
    bool? can_read = default,
    bool? can_write = default,
    bool? is_admin = default,
    bool? is_owner = default
);

/// <summary>PRBranchInfo information about a branch</summary>
/// <param name="label"></param>
/// <param name="ref"></param>
/// <param name="repo"></param>
/// <param name="repo_id"></param>
/// <param name="sha"></param>
public record PRBranchInfo(
    string? label = default,
    string? @ref = default,
    Repository? repo = default,
    long? repo_id = default,
    string? sha = default
);

/// <summary>Package represents a package</summary>
/// <param name="created_at"></param>
/// <param name="creator"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="name"></param>
/// <param name="owner"></param>
/// <param name="repository"></param>
/// <param name="type"></param>
/// <param name="version"></param>
public record Package(
    DateTimeOffset? created_at = default,
    User? creator = default,
    string? html_url = default,
    long? id = default,
    string? name = default,
    User? owner = default,
    Repository? repository = default,
    string? type = default,
    string? version = default
);

/// <summary>PackageFile represents a package file</summary>
/// <param name="Size"></param>
/// <param name="id"></param>
/// <param name="md5"></param>
/// <param name="name"></param>
/// <param name="sha1"></param>
/// <param name="sha256"></param>
/// <param name="sha512"></param>
public record PackageFile(
    long? Size = default,
    long? id = default,
    string? md5 = default,
    string? name = default,
    string? sha1 = default,
    string? sha256 = default,
    string? sha512 = default
);

/// <summary>PayloadCommit represents a commit</summary>
/// <param name="added"></param>
/// <param name="author"></param>
/// <param name="committer"></param>
/// <param name="id">sha1 hash of the commit</param>
/// <param name="message"></param>
/// <param name="modified"></param>
/// <param name="removed"></param>
/// <param name="timestamp"></param>
/// <param name="url"></param>
/// <param name="verification"></param>
public record PayloadCommit(
    ICollection<string>? added = default,
    PayloadUser? author = default,
    PayloadUser? committer = default,
    string? id = default,
    string? message = default,
    ICollection<string>? modified = default,
    ICollection<string>? removed = default,
    DateTimeOffset? timestamp = default,
    string? url = default,
    PayloadCommitVerification? verification = default
);

/// <summary>PayloadCommitVerification represents the GPG verification of a commit</summary>
/// <param name="payload"></param>
/// <param name="reason"></param>
/// <param name="signature"></param>
/// <param name="signer"></param>
/// <param name="verified"></param>
public record PayloadCommitVerification(
    string? payload = default,
    string? reason = default,
    string? signature = default,
    PayloadUser? signer = default,
    bool? verified = default
);

/// <summary>PayloadUser represents the author or committer of a commit</summary>
/// <param name="email"></param>
/// <param name="name">Full name of the commit author</param>
/// <param name="username"></param>
public record PayloadUser(
    string? email = default,
    string? name = default,
    string? username = default
);

/// <summary>Permission represents a set of permissions</summary>
/// <param name="admin"></param>
/// <param name="pull"></param>
/// <param name="push"></param>
public record Permission(
    bool? admin = default,
    bool? pull = default,
    bool? push = default
);

/// <summary>PublicKey publickey is a user key to push code to repository</summary>
/// <param name="created_at"></param>
/// <param name="fingerprint"></param>
/// <param name="id"></param>
/// <param name="key"></param>
/// <param name="key_type"></param>
/// <param name="read_only"></param>
/// <param name="title"></param>
/// <param name="url"></param>
/// <param name="user"></param>
public record PublicKey(
    DateTimeOffset? created_at = default,
    string? fingerprint = default,
    long? id = default,
    string? key = default,
    string? key_type = default,
    bool? read_only = default,
    string? title = default,
    string? url = default,
    User? user = default
);

/// <summary>PullRequest represents a pull request</summary>
/// <param name="allow_maintainer_edit"></param>
/// <param name="assignee"></param>
/// <param name="assignees"></param>
/// <param name="base"></param>
/// <param name="body"></param>
/// <param name="closed_at"></param>
/// <param name="comments"></param>
/// <param name="created_at"></param>
/// <param name="diff_url"></param>
/// <param name="due_date"></param>
/// <param name="head"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="is_locked"></param>
/// <param name="labels"></param>
/// <param name="merge_base"></param>
/// <param name="merge_commit_sha"></param>
/// <param name="mergeable"></param>
/// <param name="merged"></param>
/// <param name="merged_at"></param>
/// <param name="merged_by"></param>
/// <param name="milestone"></param>
/// <param name="number"></param>
/// <param name="patch_url"></param>
/// <param name="pin_order"></param>
/// <param name="requested_reviewers"></param>
/// <param name="state"></param>
/// <param name="title"></param>
/// <param name="updated_at"></param>
/// <param name="url"></param>
/// <param name="user"></param>
public record PullRequest(
    bool? allow_maintainer_edit = default,
    User? assignee = default,
    ICollection<User>? assignees = default,
    PRBranchInfo? @base = default,
    string? body = default,
    DateTimeOffset? closed_at = default,
    long? comments = default,
    DateTimeOffset? created_at = default,
    string? diff_url = default,
    DateTimeOffset? due_date = default,
    PRBranchInfo? head = default,
    string? html_url = default,
    long? id = default,
    bool? is_locked = default,
    ICollection<Label>? labels = default,
    string? merge_base = default,
    string? merge_commit_sha = default,
    bool? mergeable = default,
    bool? merged = default,
    DateTimeOffset? merged_at = default,
    User? merged_by = default,
    Milestone? milestone = default,
    long? number = default,
    string? patch_url = default,
    long? pin_order = default,
    ICollection<User>? requested_reviewers = default,
    string? state = default,
    string? title = default,
    DateTimeOffset? updated_at = default,
    string? url = default,
    User? user = default
);

/// <summary>PullRequestMeta PR info if an issue is a PR</summary>
/// <param name="draft"></param>
/// <param name="merged"></param>
/// <param name="merged_at"></param>
public record PullRequestMeta(
    bool? draft = default,
    bool? merged = default,
    DateTimeOffset? merged_at = default
);

/// <summary>PullReview represents a pull request review</summary>
/// <param name="body"></param>
/// <param name="comments_count"></param>
/// <param name="commit_id"></param>
/// <param name="dismissed"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="official"></param>
/// <param name="pull_request_url"></param>
/// <param name="stale"></param>
/// <param name="state"></param>
/// <param name="submitted_at"></param>
/// <param name="team"></param>
/// <param name="updated_at"></param>
/// <param name="user"></param>
public record PullReview(
    string? body = default,
    long? comments_count = default,
    string? commit_id = default,
    bool? dismissed = default,
    string? html_url = default,
    long? id = default,
    bool? official = default,
    string? pull_request_url = default,
    bool? stale = default,
    string? state = default,
    DateTimeOffset? submitted_at = default,
    Team? team = default,
    DateTimeOffset? updated_at = default,
    User? user = default
);

/// <summary>PullReviewComment represents a comment on a pull request review</summary>
/// <param name="body"></param>
/// <param name="commit_id"></param>
/// <param name="created_at"></param>
/// <param name="diff_hunk"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="original_commit_id"></param>
/// <param name="original_position"></param>
/// <param name="path"></param>
/// <param name="position"></param>
/// <param name="pull_request_review_id"></param>
/// <param name="pull_request_url"></param>
/// <param name="resolver"></param>
/// <param name="updated_at"></param>
/// <param name="user"></param>
public record PullReviewComment(
    string? body = default,
    string? commit_id = default,
    DateTimeOffset? created_at = default,
    string? diff_hunk = default,
    string? html_url = default,
    long? id = default,
    string? original_commit_id = default,
    ulong? original_position = default,
    string? path = default,
    ulong? position = default,
    long? pull_request_review_id = default,
    string? pull_request_url = default,
    User? resolver = default,
    DateTimeOffset? updated_at = default,
    User? user = default
);

/// <summary>PullReviewRequestOptions are options to add or remove pull review requests</summary>
/// <param name="reviewers"></param>
/// <param name="team_reviewers"></param>
public record PullReviewRequestOptions(
    ICollection<string>? reviewers = default,
    ICollection<string>? team_reviewers = default
);

/// <summary>PushMirror represents information of a push mirror</summary>
/// <param name="created"></param>
/// <param name="interval"></param>
/// <param name="last_error"></param>
/// <param name="last_update"></param>
/// <param name="remote_address"></param>
/// <param name="remote_name"></param>
/// <param name="repo_name"></param>
/// <param name="sync_on_commit"></param>
public record PushMirror(
    DateTimeOffset? created = default,
    string? interval = default,
    string? last_error = default,
    DateTimeOffset? last_update = default,
    string? remote_address = default,
    string? remote_name = default,
    string? repo_name = default,
    bool? sync_on_commit = default
);

/// <summary>Reaction contain one reaction</summary>
/// <param name="content"></param>
/// <param name="created_at"></param>
/// <param name="user"></param>
public record Reaction(
    string? content = default,
    DateTimeOffset? created_at = default,
    User? user = default
);

/// <summary></summary>
/// <param name="object"></param>
/// <param name="ref"></param>
/// <param name="url"></param>
public record Reference(
    GitObject? @object = default,
    string? @ref = default,
    string? url = default
);

/// <summary>Release represents a repository release</summary>
/// <param name="assets"></param>
/// <param name="author"></param>
/// <param name="body"></param>
/// <param name="created_at"></param>
/// <param name="draft"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="name"></param>
/// <param name="prerelease"></param>
/// <param name="published_at"></param>
/// <param name="tag_name"></param>
/// <param name="tarball_url"></param>
/// <param name="target_commitish"></param>
/// <param name="upload_url"></param>
/// <param name="url"></param>
/// <param name="zipball_url"></param>
public record Release(
    ICollection<Attachment>? assets = default,
    User? author = default,
    string? body = default,
    DateTimeOffset? created_at = default,
    bool? draft = default,
    string? html_url = default,
    long? id = default,
    string? name = default,
    bool? prerelease = default,
    DateTimeOffset? published_at = default,
    string? tag_name = default,
    string? tarball_url = default,
    string? target_commitish = default,
    string? upload_url = default,
    string? url = default,
    string? zipball_url = default
);

/// <summary>RenameUserOption options when renaming a user</summary>
/// <param name="new_username">New username for this user. This name cannot be in use yet by any other user.</param>
public record RenameUserOption(
    string new_username
);

/// <summary>ReplaceFlagsOption options when replacing the flags of a repository</summary>
/// <param name="flags"></param>
public record ReplaceFlagsOption(
    ICollection<string>? flags = default
);

/// <summary>RepoCollaboratorPermission to get repository permission for a collaborator</summary>
/// <param name="permission"></param>
/// <param name="role_name"></param>
/// <param name="user"></param>
public record RepoCollaboratorPermission(
    string? permission = default,
    string? role_name = default,
    User? user = default
);

/// <summary></summary>
/// <param name="author"></param>
/// <param name="committer"></param>
/// <param name="message"></param>
/// <param name="tree"></param>
/// <param name="url"></param>
/// <param name="verification"></param>
public record RepoCommit(
    CommitUser? author = default,
    CommitUser? committer = default,
    string? message = default,
    CommitMeta? tree = default,
    string? url = default,
    PayloadCommitVerification? verification = default
);

/// <summary>RepoTopicOptions a collection of repo topic names</summary>
/// <param name="topics">list of topic names</param>
public record RepoTopicOptions(
    ICollection<string>? topics = default
);

/// <summary>RepoTransfer represents a pending repo transfer</summary>
/// <param name="doer"></param>
/// <param name="recipient"></param>
/// <param name="teams"></param>
public record RepoTransfer(
    User? doer = default,
    User? recipient = default,
    ICollection<Team>? teams = default
);

/// <summary>Repository represents a repository</summary>
/// <param name="allow_fast_forward_only_merge"></param>
/// <param name="allow_merge_commits"></param>
/// <param name="allow_rebase"></param>
/// <param name="allow_rebase_explicit"></param>
/// <param name="allow_rebase_update"></param>
/// <param name="allow_squash_merge"></param>
/// <param name="archived"></param>
/// <param name="archived_at"></param>
/// <param name="avatar_url"></param>
/// <param name="clone_url"></param>
/// <param name="created_at"></param>
/// <param name="default_allow_maintainer_edit"></param>
/// <param name="default_branch"></param>
/// <param name="default_delete_branch_after_merge"></param>
/// <param name="default_merge_style"></param>
/// <param name="description"></param>
/// <param name="empty"></param>
/// <param name="external_tracker"></param>
/// <param name="external_wiki"></param>
/// <param name="fork"></param>
/// <param name="forks_count"></param>
/// <param name="full_name"></param>
/// <param name="has_actions"></param>
/// <param name="has_issues"></param>
/// <param name="has_packages"></param>
/// <param name="has_projects"></param>
/// <param name="has_pull_requests"></param>
/// <param name="has_releases"></param>
/// <param name="has_wiki"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="ignore_whitespace_conflicts"></param>
/// <param name="internal"></param>
/// <param name="internal_tracker"></param>
/// <param name="language"></param>
/// <param name="languages_url"></param>
/// <param name="link"></param>
/// <param name="mirror"></param>
/// <param name="mirror_interval"></param>
/// <param name="mirror_updated"></param>
/// <param name="name"></param>
/// <param name="object_format_name">ObjectFormatName of the underlying git repository</param>
/// <param name="open_issues_count"></param>
/// <param name="open_pr_counter"></param>
/// <param name="original_url"></param>
/// <param name="owner"></param>
/// <param name="parent"></param>
/// <param name="permissions"></param>
/// <param name="private"></param>
/// <param name="release_counter"></param>
/// <param name="repo_transfer"></param>
/// <param name="size"></param>
/// <param name="ssh_url"></param>
/// <param name="stars_count"></param>
/// <param name="template"></param>
/// <param name="updated_at"></param>
/// <param name="url"></param>
/// <param name="watchers_count"></param>
/// <param name="website"></param>
/// <param name="wiki_branch"></param>
public record Repository(
    bool? allow_fast_forward_only_merge = default,
    bool? allow_merge_commits = default,
    bool? allow_rebase = default,
    bool? allow_rebase_explicit = default,
    bool? allow_rebase_update = default,
    bool? allow_squash_merge = default,
    bool? archived = default,
    DateTimeOffset? archived_at = default,
    string? avatar_url = default,
    string? clone_url = default,
    DateTimeOffset? created_at = default,
    bool? default_allow_maintainer_edit = default,
    string? default_branch = default,
    bool? default_delete_branch_after_merge = default,
    string? default_merge_style = default,
    string? description = default,
    bool? empty = default,
    ExternalTracker? external_tracker = default,
    ExternalWiki? external_wiki = default,
    bool? fork = default,
    long? forks_count = default,
    string? full_name = default,
    bool? has_actions = default,
    bool? has_issues = default,
    bool? has_packages = default,
    bool? has_projects = default,
    bool? has_pull_requests = default,
    bool? has_releases = default,
    bool? has_wiki = default,
    string? html_url = default,
    long? id = default,
    bool? ignore_whitespace_conflicts = default,
    bool? @internal = default,
    InternalTracker? internal_tracker = default,
    string? language = default,
    string? languages_url = default,
    string? link = default,
    bool? mirror = default,
    string? mirror_interval = default,
    DateTimeOffset? mirror_updated = default,
    string? name = default,
    RepositoryObject_format_name? object_format_name = default,
    long? open_issues_count = default,
    long? open_pr_counter = default,
    string? original_url = default,
    User? owner = default,
    Repository? parent = default,
    Permission? permissions = default,
    bool? @private = default,
    long? release_counter = default,
    RepoTransfer? repo_transfer = default,
    long? size = default,
    string? ssh_url = default,
    long? stars_count = default,
    bool? template = default,
    DateTimeOffset? updated_at = default,
    string? url = default,
    long? watchers_count = default,
    string? website = default,
    string? wiki_branch = default
);

/// <summary>RepositoryMeta basic repository information</summary>
/// <param name="full_name"></param>
/// <param name="id"></param>
/// <param name="name"></param>
/// <param name="owner"></param>
public record RepositoryMeta(
    string? full_name = default,
    long? id = default,
    string? name = default,
    string? owner = default
);

/// <summary>SearchResults results of a successful search</summary>
/// <param name="data"></param>
/// <param name="ok"></param>
public record SearchResults(
    ICollection<Repository>? data = default,
    bool? ok = default
);

/// <summary>Secret represents a secret</summary>
/// <param name="created_at"></param>
/// <param name="name">the secret&apos;s name</param>
public record Secret(
    DateTimeOffset? created_at = default,
    string? name = default
);

/// <summary>ServerVersion wraps the version of the server</summary>
/// <param name="version"></param>
public record ServerVersion(
    string? version = default
);

/// <summary>StopWatch represent a running stopwatch</summary>
/// <param name="created"></param>
/// <param name="duration"></param>
/// <param name="issue_index"></param>
/// <param name="issue_title"></param>
/// <param name="repo_name"></param>
/// <param name="repo_owner_name"></param>
/// <param name="seconds"></param>
public record StopWatch(
    DateTimeOffset? created = default,
    string? duration = default,
    long? issue_index = default,
    string? issue_title = default,
    string? repo_name = default,
    string? repo_owner_name = default,
    long? seconds = default
);

/// <summary>SubmitPullReviewOptions are options to submit a pending pull review</summary>
/// <param name="body"></param>
/// <param name="event"></param>
public record SubmitPullReviewOptions(
    string? body = default,
    string? @event = default
);

/// <summary>Tag represents a repository tag</summary>
/// <param name="commit"></param>
/// <param name="id"></param>
/// <param name="message"></param>
/// <param name="name"></param>
/// <param name="tarball_url"></param>
/// <param name="zipball_url"></param>
public record Tag(
    CommitMeta? commit = default,
    string? id = default,
    string? message = default,
    string? name = default,
    string? tarball_url = default,
    string? zipball_url = default
);

/// <summary>Team represents a team in an organization</summary>
/// <param name="can_create_org_repo"></param>
/// <param name="description"></param>
/// <param name="id"></param>
/// <param name="includes_all_repositories"></param>
/// <param name="name"></param>
/// <param name="organization"></param>
/// <param name="permission"></param>
/// <param name="units"></param>
/// <param name="units_map"></param>
public record Team(
    bool? can_create_org_repo = default,
    string? description = default,
    long? id = default,
    bool? includes_all_repositories = default,
    string? name = default,
    Organization? organization = default,
    TeamPermission? permission = default,
    ICollection<string>? units = default,
    IDictionary<string, string>? units_map = default
);

/// <summary>TimelineComment represents a timeline comment (comment of any type) on a commit or issue</summary>
/// <param name="assignee"></param>
/// <param name="assignee_team"></param>
/// <param name="body"></param>
/// <param name="created_at"></param>
/// <param name="dependent_issue"></param>
/// <param name="html_url"></param>
/// <param name="id"></param>
/// <param name="issue_url"></param>
/// <param name="label"></param>
/// <param name="milestone"></param>
/// <param name="new_ref"></param>
/// <param name="new_title"></param>
/// <param name="old_milestone"></param>
/// <param name="old_project_id"></param>
/// <param name="old_ref"></param>
/// <param name="old_title"></param>
/// <param name="project_id"></param>
/// <param name="pull_request_url"></param>
/// <param name="ref_action"></param>
/// <param name="ref_comment"></param>
/// <param name="ref_commit_sha">commit SHA where issue/PR was referenced</param>
/// <param name="ref_issue"></param>
/// <param name="removed_assignee">whether the assignees were removed or added</param>
/// <param name="resolve_doer"></param>
/// <param name="review_id"></param>
/// <param name="tracked_time"></param>
/// <param name="type"></param>
/// <param name="updated_at"></param>
/// <param name="user"></param>
public record TimelineComment(
    User? assignee = default,
    Team? assignee_team = default,
    string? body = default,
    DateTimeOffset? created_at = default,
    Issue? dependent_issue = default,
    string? html_url = default,
    long? id = default,
    string? issue_url = default,
    Label? label = default,
    Milestone? milestone = default,
    string? new_ref = default,
    string? new_title = default,
    Milestone? old_milestone = default,
    long? old_project_id = default,
    string? old_ref = default,
    string? old_title = default,
    long? project_id = default,
    string? pull_request_url = default,
    string? ref_action = default,
    Comment? ref_comment = default,
    string? ref_commit_sha = default,
    Issue? ref_issue = default,
    bool? removed_assignee = default,
    User? resolve_doer = default,
    long? review_id = default,
    TrackedTime? tracked_time = default,
    string? type = default,
    DateTimeOffset? updated_at = default,
    User? user = default
);

/// <summary>TopicName a list of repo topic names</summary>
/// <param name="topics"></param>
public record TopicName(
    ICollection<string>? topics = default
);

/// <summary>TopicResponse for returning topics</summary>
/// <param name="created"></param>
/// <param name="id"></param>
/// <param name="repo_count"></param>
/// <param name="topic_name"></param>
/// <param name="updated"></param>
public record TopicResponse(
    DateTimeOffset? created = default,
    long? id = default,
    long? repo_count = default,
    string? topic_name = default,
    DateTimeOffset? updated = default
);

/// <summary>TrackedTime worked time for an issue / pr</summary>
/// <param name="created"></param>
/// <param name="id"></param>
/// <param name="issue"></param>
/// <param name="issue_id">deprecated (only for backwards compatibility)</param>
/// <param name="time">Time in seconds</param>
/// <param name="user_id">deprecated (only for backwards compatibility)</param>
/// <param name="user_name"></param>
public record TrackedTime(
    DateTimeOffset? created = default,
    long? id = default,
    Issue? issue = default,
    long? issue_id = default,
    long? time = default,
    long? user_id = default,
    string? user_name = default
);

/// <summary>TransferRepoOption options when transfer a repository&apos;s ownership</summary>
/// <param name="new_owner"></param>
/// <param name="team_ids">ID of the team or teams to add to the repository. Teams can only be added to organization-owned repositories.</param>
public record TransferRepoOption(
    string new_owner,
    ICollection<long>? team_ids = default
);

/// <summary>
/// UpdateFileOptions options for updating files
/// Note: `author` and `committer` are optional (if only one is given, it will be used for the other, otherwise the authenticated user will be used)
/// </summary>
/// <param name="content">content must be base64 encoded</param>
/// <param name="sha">sha is the SHA for the file that already exists</param>
/// <param name="author"></param>
/// <param name="branch">branch (optional) to base this file from. if not given, the default branch is used</param>
/// <param name="committer"></param>
/// <param name="dates"></param>
/// <param name="from_path">from_path (optional) is the path of the original file which will be moved/renamed to the path in the URL</param>
/// <param name="message">message (optional) for the commit of this file. if not supplied, a default message will be used</param>
/// <param name="new_branch">new_branch (optional) will make a new branch from `branch` before creating the file</param>
/// <param name="signoff">Add a Signed-off-by trailer by the committer at the end of the commit log message.</param>
public record UpdateFileOptions(
    string content,
    string sha,
    Identity? author = default,
    string? branch = default,
    Identity? committer = default,
    CommitDateOptions? dates = default,
    string? from_path = default,
    string? message = default,
    string? new_branch = default,
    bool? signoff = default
);

/// <summary>UpdateRepoAvatarUserOption options when updating the repo avatar</summary>
/// <param name="image">image must be base64 encoded</param>
public record UpdateRepoAvatarOption(
    string? image = default
);

/// <summary>UpdateUserAvatarUserOption options when updating the user avatar</summary>
/// <param name="image">image must be base64 encoded</param>
public record UpdateUserAvatarOption(
    string? image = default
);

/// <summary>User represents a user</summary>
/// <param name="active">Is user active</param>
/// <param name="avatar_url">URL to the user&apos;s avatar</param>
/// <param name="created"></param>
/// <param name="description">the user&apos;s description</param>
/// <param name="email"></param>
/// <param name="followers_count">user counts</param>
/// <param name="following_count"></param>
/// <param name="full_name">the user&apos;s full name</param>
/// <param name="id">the user&apos;s id</param>
/// <param name="is_admin">Is the user an administrator</param>
/// <param name="language">User locale</param>
/// <param name="last_login"></param>
/// <param name="location">the user&apos;s location</param>
/// <param name="login">the user&apos;s username</param>
/// <param name="login_name">the user&apos;s authentication sign-in name.</param>
/// <param name="prohibit_login">Is user login prohibited</param>
/// <param name="pronouns">the user&apos;s pronouns</param>
/// <param name="restricted">Is user restricted</param>
/// <param name="starred_repos_count"></param>
/// <param name="visibility">User visibility level option: public, limited, private</param>
/// <param name="website">the user&apos;s website</param>
public record User(
    bool? active = default,
    string? avatar_url = default,
    DateTimeOffset? created = default,
    string? description = default,
    string? email = default,
    long? followers_count = default,
    long? following_count = default,
    string? full_name = default,
    long? id = default,
    bool? is_admin = default,
    string? language = default,
    DateTimeOffset? last_login = default,
    string? location = default,
    string? login = default,
    string? login_name = default,
    bool? prohibit_login = default,
    string? pronouns = default,
    bool? restricted = default,
    long? starred_repos_count = default,
    string? visibility = default,
    string? website = default
);

/// <summary>UserHeatmapData represents the data needed to create a heatmap</summary>
/// <param name="contributions"></param>
/// <param name="timestamp"></param>
public record UserHeatmapData(
    long? contributions = default,
    long? timestamp = default
);

/// <summary>UserSettings represents user settings</summary>
/// <param name="description"></param>
/// <param name="diff_view_style"></param>
/// <param name="enable_repo_unit_hints"></param>
/// <param name="full_name"></param>
/// <param name="hide_activity"></param>
/// <param name="hide_email">Privacy</param>
/// <param name="language"></param>
/// <param name="location"></param>
/// <param name="pronouns"></param>
/// <param name="theme"></param>
/// <param name="website"></param>
public record UserSettings(
    string? description = default,
    string? diff_view_style = default,
    bool? enable_repo_unit_hints = default,
    string? full_name = default,
    bool? hide_activity = default,
    bool? hide_email = default,
    string? language = default,
    string? location = default,
    string? pronouns = default,
    string? theme = default,
    string? website = default
);

/// <summary>UserSettingsOptions represents options to change user settings</summary>
/// <param name="description"></param>
/// <param name="diff_view_style"></param>
/// <param name="enable_repo_unit_hints"></param>
/// <param name="full_name"></param>
/// <param name="hide_activity"></param>
/// <param name="hide_email">Privacy</param>
/// <param name="language"></param>
/// <param name="location"></param>
/// <param name="pronouns"></param>
/// <param name="theme"></param>
/// <param name="website"></param>
public record UserSettingsOptions(
    string? description = default,
    string? diff_view_style = default,
    bool? enable_repo_unit_hints = default,
    string? full_name = default,
    bool? hide_activity = default,
    bool? hide_email = default,
    string? language = default,
    string? location = default,
    string? pronouns = default,
    string? theme = default,
    string? website = default
);

/// <summary>WatchInfo represents an API watch status of one repository</summary>
/// <param name="created_at"></param>
/// <param name="ignored"></param>
/// <param name="reason"></param>
/// <param name="repository_url"></param>
/// <param name="subscribed"></param>
/// <param name="url"></param>
public record WatchInfo(
    DateTimeOffset? created_at = default,
    bool? ignored = default,
    object? reason = default,
    string? repository_url = default,
    bool? subscribed = default,
    string? url = default
);

/// <summary>WikiCommit page commit/revision</summary>
/// <param name="author"></param>
/// <param name="commiter"></param>
/// <param name="message"></param>
/// <param name="sha"></param>
public record WikiCommit(
    CommitUser? author = default,
    CommitUser? commiter = default,
    string? message = default,
    string? sha = default
);

/// <summary>WikiCommitList commit/revision list</summary>
/// <param name="commits"></param>
/// <param name="count"></param>
public record WikiCommitList(
    ICollection<WikiCommit>? commits = default,
    long? count = default
);

/// <summary>WikiPage a wiki page</summary>
/// <param name="commit_count"></param>
/// <param name="content_base64">Page content, base64 encoded</param>
/// <param name="footer"></param>
/// <param name="html_url"></param>
/// <param name="last_commit"></param>
/// <param name="sidebar"></param>
/// <param name="sub_url"></param>
/// <param name="title"></param>
public record WikiPage(
    long? commit_count = default,
    string? content_base64 = default,
    string? footer = default,
    string? html_url = default,
    WikiCommit? last_commit = default,
    string? sidebar = default,
    string? sub_url = default,
    string? title = default
);

/// <summary>WikiPageMetaData wiki page meta information</summary>
/// <param name="html_url"></param>
/// <param name="last_commit"></param>
/// <param name="sub_url"></param>
/// <param name="title"></param>
public record WikiPageMetaData(
    string? html_url = default,
    WikiCommit? last_commit = default,
    string? sub_url = default,
    string? title = default
);

/// <summary>indicates what to do with the file</summary>
public enum ChangeFileOperationOperation
{
    /// <summary>create</summary>
    [MapEnum("create")]
    Create = 0,
    /// <summary>update</summary>
    [MapEnum("update")]
    Update = 1,
    /// <summary>delete</summary>
    [MapEnum("delete")]
    Delete = 2,
};

/// <summary></summary>
public enum CreateHookOptionType
{
    /// <summary>forgejo</summary>
    [MapEnum("forgejo")]
    Forgejo = 0,
    /// <summary>dingtalk</summary>
    [MapEnum("dingtalk")]
    Dingtalk = 1,
    /// <summary>discord</summary>
    [MapEnum("discord")]
    Discord = 2,
    /// <summary>gitea</summary>
    [MapEnum("gitea")]
    Gitea = 3,
    /// <summary>gogs</summary>
    [MapEnum("gogs")]
    Gogs = 4,
    /// <summary>msteams</summary>
    [MapEnum("msteams")]
    Msteams = 5,
    /// <summary>slack</summary>
    [MapEnum("slack")]
    Slack = 6,
    /// <summary>telegram</summary>
    [MapEnum("telegram")]
    Telegram = 7,
    /// <summary>feishu</summary>
    [MapEnum("feishu")]
    Feishu = 8,
    /// <summary>wechatwork</summary>
    [MapEnum("wechatwork")]
    Wechatwork = 9,
    /// <summary>packagist</summary>
    [MapEnum("packagist")]
    Packagist = 10,
};

/// <summary></summary>
public enum CreateMilestoneOptionState
{
    /// <summary>open</summary>
    [MapEnum("open")]
    Open = 0,
    /// <summary>closed</summary>
    [MapEnum("closed")]
    Closed = 1,
};

/// <summary>possible values are `public` (default), `limited` or `private`</summary>
public enum CreateOrgOptionVisibility
{
    /// <summary>public</summary>
    [MapEnum("public")]
    Public = 0,
    /// <summary>limited</summary>
    [MapEnum("limited")]
    Limited = 1,
    /// <summary>private</summary>
    [MapEnum("private")]
    Private = 2,
};

/// <summary>ObjectFormatName of the underlying git repository</summary>
public enum CreateRepoOptionObject_format_name
{
    /// <summary>sha1</summary>
    [MapEnum("sha1")]
    Sha1 = 0,
    /// <summary>sha256</summary>
    [MapEnum("sha256")]
    Sha256 = 1,
};

/// <summary>TrustModel of the repository</summary>
public enum CreateRepoOptionTrust_model
{
    /// <summary>default</summary>
    [MapEnum("default")]
    Default = 0,
    /// <summary>collaborator</summary>
    [MapEnum("collaborator")]
    Collaborator = 1,
    /// <summary>committer</summary>
    [MapEnum("committer")]
    Committer = 2,
    /// <summary>collaboratorcommitter</summary>
    [MapEnum("collaboratorcommitter")]
    Collaboratorcommitter = 3,
};

/// <summary></summary>
public enum CreateTeamOptionPermission
{
    /// <summary>read</summary>
    [MapEnum("read")]
    Read = 0,
    /// <summary>write</summary>
    [MapEnum("write")]
    Write = 1,
    /// <summary>admin</summary>
    [MapEnum("admin")]
    Admin = 2,
};

/// <summary>possible values are `public`, `limited` or `private`</summary>
public enum EditOrgOptionVisibility
{
    /// <summary>public</summary>
    [MapEnum("public")]
    Public = 0,
    /// <summary>limited</summary>
    [MapEnum("limited")]
    Limited = 1,
    /// <summary>private</summary>
    [MapEnum("private")]
    Private = 2,
};

/// <summary></summary>
public enum EditTeamOptionPermission
{
    /// <summary>read</summary>
    [MapEnum("read")]
    Read = 0,
    /// <summary>write</summary>
    [MapEnum("write")]
    Write = 1,
    /// <summary>admin</summary>
    [MapEnum("admin")]
    Admin = 2,
};

/// <summary></summary>
public enum MergePullRequestOptionDo
{
    /// <summary>merge</summary>
    [MapEnum("merge")]
    Merge = 0,
    /// <summary>rebase</summary>
    [MapEnum("rebase")]
    Rebase = 1,
    /// <summary>rebase-merge</summary>
    [MapEnum("rebase-merge")]
    RebaseMerge = 2,
    /// <summary>squash</summary>
    [MapEnum("squash")]
    Squash = 3,
    /// <summary>fast-forward-only</summary>
    [MapEnum("fast-forward-only")]
    FastForwardOnly = 4,
    /// <summary>manually-merged</summary>
    [MapEnum("manually-merged")]
    ManuallyMerged = 5,
};

/// <summary></summary>
public enum MigrateRepoOptionsService
{
    /// <summary>git</summary>
    [MapEnum("git")]
    Git = 0,
    /// <summary>github</summary>
    [MapEnum("github")]
    Github = 1,
    /// <summary>gitea</summary>
    [MapEnum("gitea")]
    Gitea = 2,
    /// <summary>gitlab</summary>
    [MapEnum("gitlab")]
    Gitlab = 3,
    /// <summary>gogs</summary>
    [MapEnum("gogs")]
    Gogs = 4,
    /// <summary>onedev</summary>
    [MapEnum("onedev")]
    Onedev = 5,
    /// <summary>gitbucket</summary>
    [MapEnum("gitbucket")]
    Gitbucket = 6,
    /// <summary>codebase</summary>
    [MapEnum("codebase")]
    Codebase = 7,
};

/// <summary>ObjectFormatName of the underlying git repository</summary>
public enum RepositoryObject_format_name
{
    /// <summary>sha1</summary>
    [MapEnum("sha1")]
    Sha1 = 0,
    /// <summary>sha256</summary>
    [MapEnum("sha256")]
    Sha256 = 1,
};

/// <summary></summary>
public enum TeamPermission
{
    /// <summary>none</summary>
    [MapEnum("none")]
    None = 0,
    /// <summary>read</summary>
    [MapEnum("read")]
    Read = 1,
    /// <summary>write</summary>
    [MapEnum("write")]
    Write = 2,
    /// <summary>admin</summary>
    [MapEnum("admin")]
    Admin = 3,
    /// <summary>owner</summary>
    [MapEnum("owner")]
    Owner = 4,
};

