using FluentAssertions;
using Infra.Data.SqlServer;

namespace Test.Infra.Data.SqlServer;

public class SnakeCaseTypeMapperTests
{
    private sealed class Sample
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }

    [Fact]
    public void Constructor_Should_CreateMapper()
    {
        var mapper = new SnakeCaseTypeMapper<Sample>();
        mapper.Should().NotBeNull();
    }
}
