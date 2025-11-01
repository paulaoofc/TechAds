namespace TechAds.Domain.ValueObjects;

public class Tag
{
    public string Value { get; }

    public Tag(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tag cannot be empty", nameof(value));
        
        if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9_-]+$"))
            throw new ArgumentException("Tag can only contain letters, numbers, underscores and hyphens", nameof(value));
        
        Value = value;
    }

    private Tag() { }

    public override bool Equals(object? obj)
    {
        return obj is Tag tag && Value == tag.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Tag left, Tag right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Tag left, Tag right)
    {
        return !left.Equals(right);
    }
}