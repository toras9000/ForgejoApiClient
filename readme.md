# ForgejoApiClient

[![NugetShield]][NugetPackage]

[NugetPackage]: https://www.nuget.org/packages/ForgejoApiClient
[NugetShield]: https://img.shields.io/nuget/v/ForgejoApiClient

This is [Forgejo](https://forgejo.org/) API client library for .NET. (unofficial)  

This library is a relatively simple wrapping of the Forgejo API.  
The client class `ForgejoClient` has properties grouped by API category as indicated in the [Swagger documentation](https://codeberg.org/api/swagger).  
It should not be too difficult to find a correspondence to a method. However, in order to find the exact correspondence, the ForgejoEndpoint attribute is specified in the method.  
The endpoints indicated by the attribute can be matched against the document.   

Authentication supports only token authentication using HTTP headers.  

## Package and API version 

Although the Forgejo API specification may change from version to version, this library targets only a single version.  
If the version targeted by the library does not match the server version, there is a large possibility that it will not work properly.  
The server and client versions must be combined correctly.  

Package versions are in semantic versioning format, but are numbered according to the following arrangement.  
The version number of this package always uses the pre-release versioning format.   
The core version part represents the version of the target server.  
The version (rev.XX) portion of the pre-release is used to indicate the version of the library, not as a pre-release.  
Therefore, differences in pre-release version numbers are not necessarily trivial changes.  

## Examples

Some samples are shown below.  

### Create a repository in my namespace and add topics

```csharp
using ForgejoApiClient;
using ForgejoApiClient.Api;

var apiBase = new Uri(@"http://<your-hosting-server>/api/v1");
var apiToken = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
using var client = new ForgejoClient(apiBase, apiToken);

var me = await client.User.GetMeAsync();
if (me.login == null) throw new Exception("unexpected");

var repoOptions = new CreateRepoOption(name: "repo-name", default_branch: "main", @private: true);
var repo = await client.Repository.CreateAsync(repoOptions);

await client.Repository.AddTopicAsync(me.login, "repo-name", topic: "sample");
await client.Repository.AddTopicAsync(me.login, "repo-name", topic: "test");
```

### Create organizations and teams, add members

```csharp
using var client = new ForgejoClient(apiBase, apiToken);

var org = await client.Organization.CreateAsync(new(username: "org-name"));
var team_units = new Dictionary<string, string> { ["repo.code"] = "write", };
var team = await client.Organization.CreateTeamAsync(("org-name", new(name: "team-name", units_map: team_units));
await client.Organization.AddTeamMemberAsync(team.id!.Value, "user-name");
```

### Running the API in a different user context by sudo

```csharp
using var adminClient = new ForgejoClient(apiBase, apiToken);

var userClient = adminClient.Sudo("user-name");
await userClient.Repository.WatchAsync("owner-name", "repo-name");
```