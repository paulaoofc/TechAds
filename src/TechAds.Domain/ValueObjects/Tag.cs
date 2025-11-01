namespace TechAds.Domain.ValueObjects;

public class Tag
{
    public string Value { get; }

    public Tag(string value)
    {
        Value = value;
    }

    private Tag() { }
}