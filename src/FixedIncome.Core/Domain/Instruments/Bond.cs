namespace FixedIncome.Core.Domain.Instruments;

public class Bond
{
    public string Isin { get; }
    public string Currency { get; }
    public decimal FaceValue { get; }
    public DateTimeOffset MaturityDate { get; }
    public decimal CouponRate { get; }
    public DateTimeOffset IssueDate { get; }
    public int CouponFrequency { get; }

    public Bond(
        string isin,
        string currency,
        decimal faceValue,
        DateTimeOffset maturityDate,
        decimal couponRate,
        DateTimeOffset issueDate,
        int couponFrequency
    )
    {
        if (string.IsNullOrWhiteSpace(isin))
        {
            throw new ArgumentException(
                "ISIN cannot be null, contain whitespace or empty.",
                nameof(isin)
            );
        }
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException(
                "Currency cannot be null, contain whitespace or empty.",
                nameof(currency)
            );
        }

        if (faceValue <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(faceValue),
                "Face value must be greater than zero."
            );
        }
        if (issueDate >= maturityDate)
        {
            throw new ArgumentException("Issue date must be earlier than maturity date.");
        }
        if (couponRate < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(couponRate),
                "Coupon rate cannot be negative."
            );
        }
        if (couponFrequency <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(couponFrequency),
                "Coupon frequency must be a positive number greater than zero."
            );
        }

        Isin = isin;
        Currency = currency;
        FaceValue = faceValue;
        MaturityDate = maturityDate;
        CouponRate = couponRate;
        IssueDate = issueDate;
        CouponFrequency = couponFrequency;
    }
}
