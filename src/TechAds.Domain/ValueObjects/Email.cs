namespace TechAds.Domain.ValueObjects;

public class Email
{
    public string Value { get; }

    public Email(string value)
    {
        Value = value;
    }
}