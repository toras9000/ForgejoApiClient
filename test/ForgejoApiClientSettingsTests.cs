namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientSettingsTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task GetApiSettingsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var settings = await client.Settings.GetApiSettingsAsync();
    }

    [TestMethod]
    public async Task GetAttachmentSettingsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var settings = await client.Settings.GetAttachmentSettingsAsync();
    }

    [TestMethod]
    public async Task GetRepositorySettingsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var settings = await client.Settings.GetRepositorySettingsAsync();
    }

    [TestMethod]
    public async Task GetUiSettingsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var settings = await client.Settings.GetUiSettingsAsync();
    }
}
