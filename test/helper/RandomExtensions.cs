using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace ForgejoApiClient.Tests.helper;

internal static class RandomExtensions
{
    public static string MakeString(this Random self, int length, ReadOnlySpan<char> choices = default)
    {
        if (choices.IsEmpty) choices = "abcdefghijklmopqrstuvwxyzABCDEFGHIJKLMOPQRSTUVWXYZ0123456789-_.";
        var text = (stackalloc char[length]);
        self.GetItems(choices, text);
        return text.ToString();
    }

    public static string MakeLowerString(this Random self, int length, ReadOnlySpan<char> choices = default)
        => self.MakeString(length, "abcdefghijklmopqrstuvwxyz");
}
