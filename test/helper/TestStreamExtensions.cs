using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgejoApiClient.Tests.helper;

internal static class TestStreamExtensions
{
    public static async ValueTask<byte[]> ReadBytesAsync(this Stream self, int length, TimeSpan timeout = default)
    {
        var buffer = new byte[length];
        using var breaker = new CancellationTokenSource(timeout == TimeSpan.Zero ? Timeout.InfiniteTimeSpan : timeout);
        var size = await self.ReadAsync(buffer, breaker.Token);
        return buffer.Length < length ? buffer[..size] : buffer;
    }
}
