using EventManager.Application.Services;
using EventManager.Domain.Interfaces;
using Moq;
using Xunit;

namespace EventManager.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockUserRepository.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Domain.Entities.User>());

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<Domain.Entities.User>
        {
            new Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                GraphId = "graph-1",
                Email = "test@example.com",
                DisplayName = "Test User",
                GivenName = "Test",
                Surname = "User"
            }
        };

        _mockUserRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("test@example.com", result[0].Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((Domain.Entities.User?)null);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new Domain.Entities.User
        {
            Id = userId,
            GraphId = "graph-1",
            Email = "test@example.com",
            DisplayName = "Test User"
        };

        _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }
}
