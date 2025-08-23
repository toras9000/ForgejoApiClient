# ForgejoApiClient

[![NugetShield]][NugetPackage]

[NugetPackage]: https://www.nuget.org/packages/ForgejoApiClient  
[NugetShield]: https://img.shields.io/nuget/v/ForgejoApiClient

This is [Forgejo](https://forgejo.org/) API client library for .NET. (unofficial)  

This library provides a relatively simple wrapper around the Forgejo API.  
The `ForgejoClient` class exposes properties grouped by API category, as defined in the [Swagger documentation](https://codeberg.org/api/swagger).  
It is generally straightforward to find the corresponding method. However, to identify the exact mapping, each method is annotated with a `ForgejoEndpoint` attribute.  
The endpoint specified in the attribute can be cross-referenced with the official documentation.

Authentication is supported only via token authentication using HTTP headers.

## Package and API Version

Although the Forgejo API specification may change between versions, this library targets a single specific API version.  
If the targeted API version does not match the server version, there is a high likelihood that it will not function correctly.  
The server and client versions must be matched appropriately.

Package versions follow [Semantic Versioning](https://semver.org/) but with the following convention:  
- The package version always uses the *pre-release* format.  
- The core version number corresponds to the targeted Forgejo server version.  
- The `rev.XX` portion appears in the pre-release position, but here it represents the library’s own version and does not mean the library is a pre-release.  
  Therefore, differences in the rev.XX value may represent significant changes, not just minor updates.

## Examples

Below are some usage examples.

### Create a repository in your namespace and add topics

```csharp
using ForgejoApiClient;
using ForgejoApiClient.Api;

var apiBase = new Uri(@"http://<your-hosting-server>/api/v1");
var apiToken = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
using var client = new ForgejoClient(apiBase, apiToken);

var me = await client.User.GetMeAsync();
if (me.login == null) throw new Exception("Unexpected user info");

var repoOptions = new CreateRepoOption(name: "repo-name", default_branch: "main", @private: true);
var repo = await client.Repository.CreateAsync(repoOptions);

await client.Repository.AddTopicAsync(me.login, "repo-name", topic: "sample");
await client.Repository.AddTopicAsync(me.login, "repo-name", topic: "test");
```

### Create an organization and team, then add members

```csharp
using var client = new ForgejoClient(apiBase, apiToken);

var org = await client.Organization.CreateAsync(new(username: "org-name"));
var teamUnits = new Dictionary<string, string> { ["repo.code"] = "write" };
var team = await client.Organization.CreateTeamAsync("org-name", new(name: "team-name", units_map: teamUnits));
await client.Organization.AddTeamMemberAsync(team.id!.Value, "user-name");
```

### Save a content response

```csharp
using var client = new ForgejoClient(apiBase, apiToken);

using (var archiveDownload = await client.Repository.GetArchiveAsync("owner-name", "repo-name", "main.zip"))
using (var fileStream = File.OpenWrite(archiveDownload.Result.FileName ?? "main.zip"))
{
    await archiveDownload.Result.Stream.CopyToAsync(fileStream);
}
```

### Create quota settings and assign users

```csharp
using var client = new ForgejoClient(apiBase, apiToken);

var quotaGroup = await client.Admin.CreateQuotaGroupAsync(new(name: "package-quota"));
var quotaRule = await client.Admin.CreateQuotaRuleAsync(
    new(name: "limit-packages-500M", limit: 500 * 1024 * 1024, subjects: ["size:assets:packages:all"])
);
await client.Admin.AddQuotaGroupRuleAsync("package-quota", "limit-packages-500M");

await client.Admin.AddQuotaGroupUserAsync("package-quota", "user-name");
```

### Dispatch the workflow and wait for completion.

```csharp
using var client = new ForgejoClient(apiBase, apiToken);

var options = new DispatchWorkflowOption(@ref: "main", return_run_info: true);
var run = await client.Repository.DispatchActionsWorkflowAsync("owner", "repo", "work.yml", options, cancelToken);
if (run?.id == null) throw new Exception("Failed to dispatch");
using var breaker = CancellationTokenSource.CreateLinkedTokenSource(cancelToken);
breaker.CancelAfter(TimeSpan.FromMinutes(5));
while (true)
{
    await Task.Delay(TimeSpan.FromSeconds(5), breaker.Token);
    var runInfo = await client.Repository.GetActionsRunAsync("owner", "repo", run.id.Value, breaker.Token);
    if (DateTimeOffset.UnixEpoch < runInfo.stopped) break;
}
```

### Run the API in a different user context using sudo

```csharp
using var adminClient = new ForgejoClient(apiBase, apiToken);

var userClient = adminClient.Sudo("user-name");
await userClient.Repository.WatchAsync("owner-name", "repo-name");
```