
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
    private readonly User user;
    private readonly Email email;
    private readonly string password;
    private readonly string dummySalt;
    private readonly string dummyHash;
    private readonly CancellationToken cancellationToken;

    public UserDomainServiceTests()
    {
        user = User.Create(
            Identity.From(Guid.NewGuid()),
            Email.Parse("test@example.com"),
            PasswordHash.Parse("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"),
            PasswordSalt.Parse("salt"),
            FullName.From("John", "Doe"),
            Role.User,
            Status.Pending
        );
        email = Email.Parse("test@example.com");
        password = "test-password";
        dummySalt = "dummySalt";
        dummyHash = "dummyHash";

        _mockRepository = new Mock<IUserRepository>();
        _mockHasher = new Mock<IPasswordHasher>();
        _mockHasher.Setup(x => x.DummySalt).Returns(dummySalt);
        _mockHasher.Setup(x => x.DummyHash).Returns(dummyHash);
        _domainService = new UserDomainService(_mockRepository.Object, _mockHasher.Object);

        cancellationToken = new CancellationToken();
    }

    [Fact]
    public async Task VerifyCanCreateAsync_WhenEmailDoesNotExist_ReturnsTrue()
    {
        _mockRepository.Setup(x => x.GetByEmailAsync(email, cancellationToken))
            .ReturnsAsync((User?)null);

        var canCreate = await _domainService.VerifyCanCreateAsync(email, cancellationToken);

        Assert.True(canCreate);
        _mockRepository.Verify(x => x.GetByEmailAsync(email, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task VerifyCanCreateAsync_WhenEmailExists_ReturnsFalse()
    {
        _mockRepository.Setup(x => x.GetByEmailAsync(email, cancellationToken))
            .ReturnsAsync(user);

        var canCreate = await _domainService.VerifyCanCreateAsync(email, cancellationToken);

        Assert.False(canCreate);
        _mockRepository.Verify(x => x.GetByEmailAsync(email, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task VerifyCredentialsAsync_WithInvalidIdentity_ReturnsFailure()
    {
        var result = await _domainService.VerifyCredentialsAsync("invalid-email", password, cancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Invalid identity!", result.Error);
        _mockRepository.Verify(x => x.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockHasher.Verify(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task VerifyCredentialsAsync_WhenUserNotFound_ReturnsFailure()
    {
        _mockRepository.Setup(x => x.GetByEmailAsync(email, cancellationToken))
            .ReturnsAsync((User?)null!);

        _mockHasher.Setup(x => x.Verify(password + dummySalt, dummyHash))
            .Returns(false);

        var result = await _domainService.VerifyCredentialsAsync(email, password, cancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Invalid credentials!", result.Error);
        _mockRepository.Verify(x => x.GetByEmailAsync(email, cancellationToken), Times.Once);
        _mockHasher.Verify(x => x.Verify(password + dummySalt, dummyHash), Times.Once);
    }

    [Fact]
    public async Task VerifyCredentialsAsync_WithInvalidCredentials_ReturnsFailure()
    {
        _mockRepository.Setup(x => x.GetByEmailAsync(email, cancellationToken))
            .ReturnsAsync(user);

        _mockHasher.Setup(x => x.Verify(password + user.PasswordSalt, user.PasswordHash))
            .Returns(false);

        var result = await _domainService.VerifyCredentialsAsync(email, password, cancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Invalid credentials!", result.Error);
        _mockRepository.Verify(x => x.GetByEmailAsync(email, cancellationToken), Times.Once);
        _mockHasher.Verify(x => x.Verify(password + user.PasswordSalt, user.PasswordHash), Times.Once);
    }

    [Fact]
    public async Task VerifyCredentialsAsync_WithValidCredentials_ReturnsSuccess()
    {
        _mockRepository.Setup(x => x.GetByEmailAsync(email, cancellationToken))
            .ReturnsAsync(user);

        _mockHasher.Setup(x => x.Verify(password + user.PasswordSalt, user.PasswordHash))
            .Returns(true);

        var result = await _domainService.VerifyCredentialsAsync(email, password, cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.Equal(user, result.Value);
        _mockRepository.Verify(x => x.GetByEmailAsync(email, cancellationToken), Times.Once);
        _mockHasher.Verify(x => x.Verify(password + user.PasswordSalt, user.PasswordHash), Times.Once);
    }
}