namespace Curacaru.Backend.Core.Enums;

/// <summary>Enumeration for the clearance type.</summary>
public enum ClearanceType
{
    ReliefAmount = 0,

    CareBenefit = 1,

    PreventiveCare = 2,

    SelfPayment = 3
}

public static class ClearanceTypeExtensions
{
    public static string ToFriendlyString(this ClearanceType clearanceType)
    {
        return clearanceType switch
        {
            ClearanceType.ReliefAmount => "Entlastungsbetrag § 45b SGB XI",
            ClearanceType.PreventiveCare => "Verhinderungspflege § 39 SGB XI",
            ClearanceType.CareBenefit => "Pflegesachleistungen § 36 SGB XI (max. 40%)",
            ClearanceType.SelfPayment => "Selbstzahler",
            _ => throw new ArgumentOutOfRangeException(nameof(clearanceType))
        };
    }
}