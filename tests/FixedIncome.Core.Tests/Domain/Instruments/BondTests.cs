using FixedIncome.Core.Domain.Instruments;
using FluentAssertions;

namespace FixedIncome.Core.Tests.Domain.Instruments;

public class BondTests
{
    private readonly Bond _bond = BuildValidBond;

    [Fact]
    public void ShouldInitialiseSuccessfulBond()
    {
        _bond.Isin.Should().NotBeNullOrEmpty();
        _bond.Currency.Should().NotBeNullOrEmpty();
        _bond.FaceValue.Should().BeGreaterThan(0);
        _bond.MaturityDate.Should().BeAfter(_bond.IssueDate);
        _bond.CouponRate.Should().BeGreaterThanOrEqualTo(0);
        _bond.CouponFrequency.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData("", "GBP", "ISIN cannot be null, contain whitespace or empty. (Parameter 'isin')")]
    [InlineData(
        "GB1234567890",
        " ",
        "Currency cannot be null, contain whitespace or empty. (Parameter 'currency')"
    )]
    public void ShouldThrowArgumentExceptionForInvalidBondParameters(
        string isin,
        string currency,
        string errorMessage
    )
    {
        var act = () =>
            new Bond(
                isin,
                currency,
                1000m,
                new DateTimeOffset(DateTime.Today.AddYears(5)),
                5.0m,
                new DateTimeOffset(DateTime.Today),
                2
            );

        act.Should().Throw<ArgumentException>().WithMessage(errorMessage);
    }

    [Theory]
    [InlineData(-1000, 5.0, 2, "Face value must be greater than zero. (Parameter 'faceValue')")]
    [InlineData(1000, -5.0, 2, "Coupon rate cannot be negative. (Parameter 'couponRate')")]
    [InlineData(
        1000,
        5.0,
        0,
        "Coupon frequency must be a positive number greater than zero. (Parameter 'couponFrequency')"
    )]
    public void ShouldThrowArgumentOutOfRangeExceptionForInvalidBondParameters(
        decimal faceValue,
        decimal couponRate,
        int couponFrequency,
        string errorMessage
    )
    {
        var act = () =>
            new Bond(
                "GB1234567890",
                "GBP",
                faceValue,
                new DateTimeOffset(DateTime.Today.AddYears(5)),
                couponRate,
                new DateTimeOffset(DateTime.Today),
                couponFrequency
            );

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage(errorMessage);
    }

    [Fact]
    public void ShouldThrowWhenIssueDateIsGreaterThanMaturityDate()
    {
        var act = () =>
            new Bond(
                "GB1234567890",
                "GBP",
                1000m,
                new DateTimeOffset(DateTime.Today.AddYears(-1)),
                5.0m,
                new DateTimeOffset(DateTime.Today),
                2
            );

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Issue date must be earlier than maturity date.");
    }

    private static Bond BuildValidBond =>
        new(
            "GB1234567890",
            "GBP",
            1000m,
            new DateTimeOffset(DateTime.Today.AddYears(5)),
            5.0m,
            new DateTimeOffset(DateTime.Today),
            2
        );
}
