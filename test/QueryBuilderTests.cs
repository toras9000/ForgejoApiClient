using System.Runtime.CompilerServices;
using System.Text;
using ForgejoApiClient.Api;

namespace ForgejoApiClient.Tests;

[TestClass]
public class QueryBuilderTests
{
    [TestMethod]
    public void Constructor()
    {
        {
            var target = new QueryBuilder("path");
            target.ToString().Should().Be("path");
        }
    }

    [TestMethod]
    public void Append()
    {
        var target = new QueryBuilder("path");
        target.Append("a", 10).Append("b", "str").Append("c", 'x');

        target.ToString().Should().Be("path?a=10&b=str&c=x");
    }

    [TestMethod]
    public void Param_paging()
    {
        var builder = new QueryBuilder("path", append: false);
        var paging = new PagingOptions(page: 5, limit: 6);
        builder.Param(paging).ToString().Should().Be("path?page=5&limit=6");
    }

    [TestMethod]
    public void Param_value()
    {
        {
            var builder = new QueryBuilder("path", append: false);
            var value = 100;
            builder.Param(value).ToString().Should().Be($"path?value={value}");
        }
        {
            var builder = new QueryBuilder("path", append: false);
            var value = (int?)null;
            builder.Param(value).ToString().Should().Be($"path");
        }
    }

    [TestMethod]
    public void Param_array()
    {
        {
            var builder = new QueryBuilder("path", append: false);
            var values = new[] { 100, 200 };
            builder.Param(values).ToString().Should().Be("path?values=100&values=200");
        }
        {
            var builder = new QueryBuilder("path", append: false);
            var values = (ICollection<int>)new[] { 100, 200 };
            builder.Param(values).ToString().Should().Be("path?values=100&values=200");
        }
    }

    [TestMethod]
    public void Param_DateTime()
    {
        var builder = new QueryBuilder("path", append: false);
        var time = new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        builder.Param(time).ToString().Should().Be($"path?time=2025-01-02T03:04:05Z");
    }

    [TestMethod]
    public void Param_DateTime_nullable()
    {
        {
            var builder = new QueryBuilder("path", append: false);
            var time = (DateTime?)new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            builder.Param(time).ToString().Should().Be($"path?time=2025-01-02T03:04:05Z");
        }
        {
            var builder = new QueryBuilder("path", append: false);
            var time = (DateTime?)null;
            builder.Param(time).ToString().Should().Be($"path");
        }
    }

    [TestMethod]
    public void Param_DateTimeOffset()
    {
        var builder = new QueryBuilder("path", append: false);
        var time = new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.Zero);
        builder.Param(time).ToString().Should().Be($"path?time=2025-01-02T03:04:05Z");
    }

    [TestMethod]
    public void Param_DateTimeOffset_nullable()
    {
        {
            var builder = new QueryBuilder("path", append: false);
            var time = (DateTimeOffset?)new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.Zero);
            builder.Param(time).ToString().Should().Be($"path?time=2025-01-02T03:04:05Z");
        }
        {
            var builder = new QueryBuilder("path", append: false);
            var time = (DateTimeOffset?)null;
            builder.Param(time).ToString().Should().Be($"path");
        }
    }
}
