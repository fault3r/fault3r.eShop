
using System;
using Moq;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.DomainServices;
using UserService.Domain.Repositories;
using UserService.Domain.Security;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.DomainServices;

public class UserDomainServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<IPasswordHasher> _mockHasher;
    private readonly UserDomainService _domainService;
    private readonly Email email = Email.Parse("test@example.com");

    public UserDomainServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockHasher = new Mock<IPasswordHasher>();
        _domainService = new UserDomainService(_mockRepository.Object, _mockHasher.Object);
    }

    [Fact]
    public async Task VerifyCanCreateAsync_WhenEmailDoesNotExist_ReturnsTrue()
    {
        _mockRepository.Setup(x => x.GetByEmailAsync(email, default))
            .ReturnsAsync((User?)null);

        var canCreate = await _domainService.VerifyCanCreateAsync(email, default);

        Assert.True(canCreate);
        _mockRepository.Verify(x => x.GetByEmailAsync(email, default), Times.Once);
    }

    [Fact]
    public async Task VerifyCanCreateAsync_WhenEmailExists_ReturnsFalse()
    {
        var user = new User(Identity.From(Guid.NewGuid()));

        _mockRepository.Setup(x => x.GetByEmailAsync(email, default))
            .ReturnsAsync(user);

        var canCreate = await _domainService.VerifyCanCreateAsync(email, default);

        Assert.False(canCreate);
        _mockRepository.Verify(x => x.GetByEmailAsync(email, default), Times.Once);        
    }

    [Fact]
    public async Task VerifyCanCreateAsync_WhenEmailIsNull_ThrowsArgumentNullException()
    {
        async Task act() => await _domainService.VerifyCanCreateAsync(null!, default);

        await Assert.ThrowsAsync<ArgumentNullException>(act);

        _mockRepository.Verify(x => x.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}