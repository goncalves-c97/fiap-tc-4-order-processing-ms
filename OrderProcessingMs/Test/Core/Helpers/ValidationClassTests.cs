using Core.Helpers;
using FluentAssertions;

namespace Test.Core.Helpers;

public class ValidationClassTests
{
    private sealed class SampleValidator : ValidatorClass
    {
        private readonly int _id;
        private readonly string? _name;
        public SampleValidator(int id, string? name)
        {
            _id = id;
            _name = name;
            Validate();
        }

        protected override void Validate()
        {
            IdValidation(_id, nameof(_id), validateZero: true);
            NotEmptyStringValidation(nameof(_name), _name);
        }
    }

    [Fact]
    public void Validator_Should_RegisterErrors_AndExposeSummary()
    {
        var v = new SampleValidator(0, null);
        v.IsValid.Should().BeFalse();
        v.Errors.Should().HaveCount(2);
        v.Errors.Summary.Should().NotBeNullOrWhiteSpace();
        v.ContainsError(GenericErrors.IdZeroError, nameof(SampleValidator)).Should().BeFalse();
    }

    [Fact]
    public void Validator_Should_BeValid_WhenInputsOk()
    {
        var v = new SampleValidator(1, "x");
        v.IsValid.Should().BeTrue();
        v.Errors.Should().BeEmpty();
    }
}
