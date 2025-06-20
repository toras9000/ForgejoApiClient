#r "nuget: Lestaly, 0.84.0"
#nullable enable
using System.Text.Json;
using Lestaly;
using Lestaly.Cx;

return await Paved.ProceedAsync(async () =>
{
    // コンテナを停止
    WriteLine("Stop service ...");
    var composeFile = ThisSource.RelativeFile("./docker/compose.yml");
    var mountVolumeFile = ThisSource.RelativeFile("./docker/mount-volume.yml");
    await "docker".args("compose", "--file", composeFile, "--file", mountVolumeFile, "down", "--remove-orphans", "--volumes").result().success();
    WriteLine();

    // データディレクトリを削除
    WriteLine("Delete data directory ...");
    var volumesDir = ThisSource.RelativeDirectory("./docker/volumes");
    volumesDir.DeleteRecurse();
    WriteLine();
});
