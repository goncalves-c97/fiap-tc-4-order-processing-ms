using Core.Adapters;
using FluentAssertions;

namespace Test.Core.Adapters;

public class ObjectToJsonStringAdapterTests
{
    private sealed record Sample(int Id, string Name);

    [Fact]
    public void ConvertToJsonString_Should_ReturnJson()
    {
        var json = ObjectToJsonStringAdapter.ConvertToJsonString(new Sample(1, "x"));

        json.Should().Contain("\"Id\":1");
        json.Should().Contain("\"Name\":\"x\"");
    }
}
