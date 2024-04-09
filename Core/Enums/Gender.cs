namespace Curacaru.Backend.Core.Enums;

using System.ComponentModel;

/// <summary>A biological gender</summary>
public enum Gender
{
    [Description("female")]
    Female = 0,

    [Description("male")]
    Male = 1
}

public static class GenderExtensions
{
    public static string ToFriendlyString(this Gender gender)
        => gender switch
        {
            Gender.Female => "Frau",
            Gender.Male => "Herr",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };
}