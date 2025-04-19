#r "nuget: Lestaly, 0.73.0"
#nullable enable
using Lestaly;
using Lestaly.Cx;

await "dotnet".args("script", ThisSource.RelativeFile("10.restart.csx").FullName, "--", "--bind-mount").interactive();
