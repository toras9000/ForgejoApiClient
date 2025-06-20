#r "nuget: Lestaly, 0.84.0"
#nullable enable
using Lestaly;
using Lestaly.Cx;

await "dotnet".args("script", ThisSource.RelativeFile("10.restart.csx"), "--", "--bind-mount").interactive();
