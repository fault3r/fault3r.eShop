
using System;
using AccountService.Domain.Exceptions.Identity;
using AccountService.Domain.ValueObjects;
using FluentAssertions;

namespace AccountService.Tests.UnitTests.Domain.ValueObjects;

public class IdentityTests
{
    [Fact]
    public void ParameterlessConstructor_GeneratesNewNonEmptyGuid()
    {
        var identity = new Identity();

        identity.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ThrowsEmptyGuidException()
    {
        Action act = () => new Identity(Guid.Empty);

        act.Should().Throw<EmptyGuidException>();
    }

    [Fact]
    public void Constructor_WithValidGuid_SetsValue()
    {
        var guid = Guid.NewGuid();

        var identity = new Identity(guid);

        identity.Value.Should().Be(guid);
    }

    [Fact]
    public void New_ReturnsNonEmptyGuid()
    {
        var identity = Identity.New();

        identity.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void From_WithNullOrWhitespace_ThrowsMissingGuidException()
    {
        Action act1 = () => Identity.From(null!);
        Action act2 = () => Identity.From("   ");

        act1.Should().Throw<MissingGuidException>();
        act2.Should().Throw<MissingGuidException>();
    }

    [Fact]
    public void From_WithInvalidGuid_ThrowsInvalidGuidException()
    {
        Action act = () => Identity.From("not-a-guid");

        act.Should().Throw<InvalidGuidException>()
            .WithMessage("*not-a-guid*");
    }

    [Fact]
    public void From_WithValidGuid_ReturnsIdentity()
    {
        var guid = Guid.NewGuid();
        var input = guid.ToString();

        var identity = Identity.From(input);

        identity.Value.Should().Be(guid);
    }

    [Fact]
    public void IsValid_ReturnsTrueForValidGuid()
    {
        var result = Identity.IsValid(Guid.NewGuid().ToString());

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_ReturnsFalseForEmptyOrInvalidGuid()
    {
        Identity.IsValid(Guid.Empty.ToString()).Should().BeFalse();
        Identity.IsValid("invalid-guid").Should().BeFalse();
    }

    [Fact]
    public void ToString_ReturnsGuidAsString()
    {
        var guid = Guid.NewGuid();
        var identity = new Identity(guid);

        identity.ToString().Should().Be(guid.ToString());
    }

    [Fact]
    public void Equality_WorksForSameGuid()
    {
        var guid = Guid.NewGuid();
        var a = new Identity(guid);
        var b = new Identity(guid);

        a.Should().Be(b);
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equality_FailsForDifferentGuids()
    {
        var a = new Identity(Guid.NewGuid());
        var b = new Identity(Guid.NewGuid());

        a.Should().NotBe(b);
        (a == b).Should().BeFalse();
        (a != b).Should().BeTrue();
    }
}