
using System;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.Abstractions;

public class ValueObjectTests
{
        [Fact]
        public void Equality_ShouldWork_ForSameGuid()
        {
            var guid = Guid.NewGuid();
            var identity1 = new Identity(guid);
            var identity2 = new Identity(guid);

            Assert.True(identity1 == identity2);
        }

        [Fact]
        public void Equality_ShouldFail_ForDifferentGuid()
        {
            var identity1 = new Identity(Guid.NewGuid());
            var identity2 = new Identity(Guid.NewGuid());

            Assert.True(identity1 != identity2);
        }
}
