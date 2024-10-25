namespace ForgejoApiClient.Tests;

[TestClass]
public class ForgejoApiClientActivityPubTests : ForgejoApiClientTestsBase
{
    [TestMethod]
    public async Task GetPersonActorAsync()
    {
        using var client = new ForgejoClient(this.TestService, this.TestToken);
        var pub = await client.ActivityPub.GetPersonActorAsync(1);
    }

    [TestMethod]
    public async Task SendToInboxAsync()
    {
        Assert.Inconclusive("利用方法がわからない。");

        using var client = new ForgejoClient(this.TestService, this.TestToken);
        await client.ActivityPub.SendToInboxAsync(1);
    }

}
