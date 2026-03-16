
using System;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.Abstractions;

public class ValueObjectTests
{
    [Fact]
    public void Equals_WithSameValues_AreEqual()
    {
        var valueObject1 = Status.Active;
        var valueObject2 = Status.Parse("active");

        Assert.True(valueObject1.Equals((object)valueObject2));
        Assert.True(valueObject1.Equals(valueObject2));
    }

    [Fact]
    public void EqualsOperator_WithSameValues_AreEqual()
    {
        var valueObject1 = FullName.From("Hamed", "Damaavandi");
        var valueObject2 = FullName.Parse("Hamed Damaavandi");

        Assert.True(valueObject1 == valueObject2);
        Assert.False(valueObject1 != valueObject2);
    }
}
