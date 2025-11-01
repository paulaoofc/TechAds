using Xunit;
using FluentAssertions;
using TechAds.Domain.ValueObjects;

public class ValueObjectsTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name+tag@example.co.uk")]
    [InlineData("test.email@subdomain.example.com")]
    public void Email_ValidEmail_ShouldCreateSuccessfully(string validEmail)
    {
        var email = new Email(validEmail);
        email.Value.Should().Be(validEmail);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@invalid.com")]
    [InlineData("invalid.com")]
    [InlineData("test@.com")]
    [InlineData("test..test@example.com")]
    public void Email_InvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        Action act = () => new Email(invalidEmail);
        act.Should().Throw<ArgumentException>().WithMessage("*email*");
    }

    [Fact]
    public void Email_Equality_ShouldWork()
    {
        var email1 = new Email("test@example.com");
        var email2 = new Email("test@example.com");
        var email3 = new Email("other@example.com");

        email1.Should().Be(email2);
        email1.Should().NotBe(email3);
        email1.Equals(email2).Should().BeTrue();
        email1.Equals(email3).Should().BeFalse();
    }

    [Theory]
    [InlineData("csharp")]
    [InlineData("javascript")]
    [InlineData("react")]
    [InlineData("tag-with-dash")]
    [InlineData("tag_with_underscore")]
    public void Tag_ValidTag_ShouldCreateSuccessfully(string validTag)
    {
        var tag = new Tag(validTag);
        tag.Value.Should().Be(validTag);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("tag with spaces")]
    [InlineData("tag@special")]
    [InlineData("tag#hash")]
    public void Tag_InvalidTag_ShouldThrowArgumentException(string invalidTag)
    {
        Action act = () => new Tag(invalidTag);
        act.Should().Throw<ArgumentException>().WithMessage("*tag*");
    }

    [Fact]
    public void Tag_Equality_ShouldWork()
    {
        var tag1 = new Tag("csharp");
        var tag2 = new Tag("csharp");
        var tag3 = new Tag("javascript");

        tag1.Should().Be(tag2);
        tag1.Should().NotBe(tag3);
        tag1.Equals(tag2).Should().BeTrue();
        tag1.Equals(tag3).Should().BeFalse();
    }
}