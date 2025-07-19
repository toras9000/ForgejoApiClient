namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientMiscellaneousTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task ListGitignoreTemplatesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var templates = await client.Miscellaneous.ListGitignoreTemplatesAsync();
        templates.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetGitignoreTemplateAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var template = await client.Miscellaneous.GetGitignoreTemplateAsync("C");
        template.name.Should().Be("C");
    }

    [TestMethod]
    public async Task ListLabelTemplatesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var templates = await client.Miscellaneous.ListLabelTemplatesAsync();
        templates.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task ListTemplateLabelsAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var labels = await client.Miscellaneous.ListTemplateLabelsAsync("Default");
        labels.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task ListLicenseTemplatesAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var templates = await client.Miscellaneous.ListLicenseTemplatesAsync();
        templates.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task GetLicenseTemplateAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var license = await client.Miscellaneous.GetLicenseTemplateAsync("MIT");
        license.name.Should().Be("MIT");
    }

    [TestMethod]
    public async Task RenderMarkdownAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var doc = await client.Miscellaneous.RenderMarkdownAsync(new(Text: "- aaa"));
        doc.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task RenderMarkdownRawAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var doc = await client.Miscellaneous.RenderMarkdownRawAsync("- aaa");
        doc.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task RenderMarkupAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var doc = await client.Miscellaneous.RenderMarkupAsync(new(Mode: "gfm", Text: "- aaa"));
        doc.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetNodeInfoAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var info = await client.Miscellaneous.GetNodeInfoAsync();
        info.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetSigningKeyGpgAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var key = await client.Miscellaneous.GetSigningKeyGpgAsync();
        key.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetSigningKeySshAsync()
    {
        Assert.Inconclusive("このAPIは構成で [repository.signing] FORMAT=ssh の時に機能する模様");

        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var key = await client.Miscellaneous.GetSigningKeySshAsync();
        key.Should().NotBeNull();

    }

    [TestMethod]
    public async Task GetVersionAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var version = await client.Miscellaneous.GetVersionAsync();
        version.Should().NotBeNull();
    }



}
