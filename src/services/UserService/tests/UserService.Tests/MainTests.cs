using System.Linq.Expressions;
using UserService.Domain.ValueObjects;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void Test1()
    {
      var role = new Role(Role.RoleType.User);
      var rolee = Role.User;
      var roleee = Role.From(Role.RoleType.User);
      var roleeee = Role.Parse("user");
      Assert.True(role==roleee);
      Assert.True(roleeee==Role.User);
      Assert.False(rolee==Role.Admin);
      
    }
}