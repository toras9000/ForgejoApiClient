#r "nuget: Lestaly.General, 0.105.0"
#nullable enable
using System.Text.Json;
using Lestaly;
using Lestaly.Cx;

return await Paved.ProceedAsync(async () =>
{
    await "dotnet".args("script", ThisSource.RelativeFile("01.delete-data.csx"), "--no-pause").echo();
    await "dotnet".args("script", ThisSource.RelativeFile("10.restart.csx"), "--no-pause").echo();
    await "dotnet".args("script", ThisSource.RelativeFile("@show-service.csx"), "--no-pause").echo();
});
