using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgejoApiClient.Tests.helper;

public static class TestSimpleExtensions
{
    public static async Task<string> ReadAllTextAsync(this Stream self, Encoding? encoding = null)
    {
        var reader = new StreamReader(self, encoding ?? Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}
