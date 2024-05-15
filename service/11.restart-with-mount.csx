#r "nuget: Lestaly, 0.68.0"
#nullable enable
using Lestaly;
using Lestaly.Cx;

await Paved.RunAsync(config: c => c.AnyPause(), action: async () =>
{
    var restartScript = ThisSource.RelativeFile("10.restart.csx");
    await "dotnet".args("script", restartScript.FullName, "--", "--bind-mount").interactive();
});
