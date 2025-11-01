using System.Text.RegularExpressions;

namespace TechAds.Domain.ValueObjects;

public class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));
        
        if (!Regex.IsMatch(value, @"^[^\s@]+@[^\s@]+\.[^\s@]+$") || value.Contains(".."))
            throw new ArgumentException("Invalid email format", nameof(value));
        
        Value = value;
    }

    private Email() { }

    public override bool Equals(object? obj)
    {
        return obj is Email email && Value == email.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Email left, Email right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Email left, Email right)
    {
        return !left.Equals(right);
    }
}