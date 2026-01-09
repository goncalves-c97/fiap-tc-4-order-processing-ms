using Core.Helpers;
using FluentAssertions;

namespace Test.Core.Helpers;

public class HashHelperTests
{
    [Fact]
    public void ComputeSha256Hash_Should_ReturnNonEmptyValue()
    {
        var result = HashHelper.ComputeSha256Hash("abc");
        result.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ComputeSha256Hash_SameInput_Should_ReturnSameHash()
    {
        var h1 = HashHelper.ComputeSha256Hash("same");
        var h2 = HashHelper.ComputeSha256Hash("same");
        h1.Should().Be(h2);
    }

    [Fact]
    public void ComputeSha256Hash_DifferentInput_Should_ReturnDifferentHash()
    {
        HashHelper.ComputeSha256Hash("a").Should().NotBe(HashHelper.ComputeSha256Hash("b"));
    }
}
