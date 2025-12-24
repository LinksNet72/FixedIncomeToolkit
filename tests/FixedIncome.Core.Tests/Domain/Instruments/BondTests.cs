using FixedIncome.Core.Domain.Instruments;
using FluentAssertions;

namespace FixedIncome.Core.Tests.Domain.Instruments;

public class BondTests
{
    private readonly Bond _bond = BuildValidBond;

    [Fact]
    public void ShouldInitaliseSuccessfulBond()
    {
        _bond.Isin.Should().NotBeNullOrEmpty();
        _bond.Currency.Should().NotBeNullOrEmpty();
        _bond.FaceValue.Should().BeGreaterThan(0);
        _bond.MaturityDate.Should().BeAfter(_bond.IssueDate);
        _bond.CouponRate.Should().BeGreaterThanOrEqualTo(0);
        _bond.CouponFrequency.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData("", "USD", 1000, 5.0, 2)]
    [InlineData("US1234567890", " ", 1000, 5.0, 2)]
    [InlineData("US1234567890", "USD", -1000, 5.0, 2)]
    [InlineData("US1234567890", "USD", 1000, -5.0, 2)]
    [InlineData("US1234567890", "USD", 1000, 5.0, 0)]
    public void ShouldThrowExceptionForInvalidBondParameters(
        string isin,
        string currency,
        decimal faceValue,
        decimal couponRate,
        int couponFrequency
    )
    {
        var act = () =>
            new Bond(
                isin: isin,
                currency: currency,
                faceValue: faceValue,
                maturityDate: new DateTimeOffset(DateTime.Today),
                couponRate: couponRate,
                issueDate: new DateTimeOffset(DateTime.Today.AddYears(2)),
                couponFrequency: couponFrequency
            );

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ShouldThrowWhenIssueDateIsGreaterThanMaturityDate()
    {
        var act = () =>
            new Bond(
                isin: "US1234567890",
                currency: "USD",
                faceValue: 1000m,
                maturityDate: new DateTimeOffset(DateTime.Today.AddYears(-1)),
                couponRate: 5.0m,
                issueDate: new DateTimeOffset(DateTime.Today),
                couponFrequency: 2
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
