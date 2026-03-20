
using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Persistence.Contexts;
using UserService.Infrastructure.Repositories;

namespace UserService.Tests.UnitTests.Infrastructure.Tests.Repositories;

public class EfUserRepositoryTests
{
    private readonly Mock<IDatabaseContext> _mockDbContext;
    private readonly Mock<DbSet<User>> _mockDbSet;
    private readonly User user;
    private readonly CancellationToken cancellationToken;

    public EfUserRepositoryTests()
    {
        _mockDbContext = new Mock<IDatabaseContext>();
        _mockDbSet = new Mock<DbSet<User>>();

        _mockDbContext.Setup(x => x.Users).Returns(_mockDbSet.Object);

        user = User.Create(
            Identity.From(Guid.NewGuid()),
            Email.Parse("test@example.com"),
            PasswordHash.Parse("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"),
            PasswordSalt.Parse("salt"),
            FullName.From("Hamed", "Damaavandi"),
            Role.User,
            Status.Pending
        );

        cancellationToken = new CancellationToken();
    }

    [Fact]
    public async Task CreateAsync_WhenUserIsValid_AddsUserToDbContext()
    {
        var _repository = new EfUserRepository(_mockDbContext.Object);

        await _repository.CreateAsync(user, cancellationToken);

        _mockDbSet.Verify(x => x.AddAsync(user, cancellationToken), Times.Once);
    }
}