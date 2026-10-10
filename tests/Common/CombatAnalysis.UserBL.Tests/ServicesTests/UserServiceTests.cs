using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Services;
using CombatAnalysis.UserBL.Tests.Factory;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using Moq;

namespace CombatAnalysis.UserBL.Tests.ServicesTests;

public class UserServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var userDto = TestDataFactory.CreateAppUserDto();
        var user = TestDataFactory.CreateAppUser();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<AppUser>(userDto)).Returns(user);
        mockMapper.Setup(m => m.Map<AppUserDto>(user)).Returns(userDto);

        mockRepository.Setup(m => m.CreateAsync(user)).ReturnsAsync(user);

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto.Id, result.Id);
        Assert.Equal(userDto.FirstName, result.FirstName);
        Assert.Equal(userDto.LastName, result.LastName);
        Assert.Equal(userDto.Birthday, result.Birthday);
        Assert.Equal(userDto.Username, result.Username);
        Assert.Equal(userDto.AboutMe, result.AboutMe);
        Assert.Equal(userDto.PhoneNumber, result.PhoneNumber);
        Assert.Equal(userDto.Gender, result.Gender);
        Assert.Equal(userDto.IdentityUserId, result.IdentityUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<AppUser>(It.IsAny<AppUserDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<AppUser>()), Times.Once);
        mockMapper.Verify(m => m.Map<AppUserDto>(It.IsAny<AppUser>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsUsernameEmpty()
    {
        // Arrange
        var userDto = TestDataFactory.CreateAppUserDto(username: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(AppUserDto.Username), () => service.CreateAsync(userDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsFirstNameEmpty()
    {
        // Arrange
        var userDto = TestDataFactory.CreateAppUserDto(firstName: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(AppUserDto.FirstName), () => service.CreateAsync(userDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsLastNameEmpty()
    {
        // Arrange
        var userDto = TestDataFactory.CreateAppUserDto(lastName: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(AppUserDto.LastName), () => service.CreateAsync(userDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const string userId = "uid-222";

        var userDto = TestDataFactory.CreateAppUserDto(id: userId);
        var user = TestDataFactory.CreateAppUser(id: userId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<AppUser>(userDto)).Returns(user);

        mockRepository.Setup(m => m.UpdateAsync(userId, user));

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(userId, userDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<AppUser>(It.IsAny<AppUserDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<AppUser>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdEmpty()
    {
        // Arrange
        const string userId = "uid-222";

        var userDto = TestDataFactory.CreateAppUserDto(id: userId, username: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(string.Empty, userDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsUsernameEmpty()
    {
        // Arrange
        const string userId = "uid-222";

        var userDto = TestDataFactory.CreateAppUserDto(id: userId, username: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(AppUserDto.Username), () => service.UpdateAsync(userId, userDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsFirstNameEmpty()
    {
        // Arrange
        const string userId = "uid-222";

        var userDto = TestDataFactory.CreateAppUserDto(id: userId, firstName: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(AppUserDto.FirstName), () => service.UpdateAsync(userId, userDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsLastNameEmpty()
    {
        // Arrange
        const string userId = "uid-222";

        var userDto = TestDataFactory.CreateAppUserDto(id: userId, lastName: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(AppUserDto.LastName), () => service.UpdateAsync(userId, userDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const string userId = "uid-22";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(userId);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const string userId = "";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteAsync(userId));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var usersDto = new List<AppUserDto> {
            TestDataFactory.CreateAppUserDto()
        };
        var users = new List<AppUser> {
            TestDataFactory.CreateAppUser()
        };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<AppUserDto>>(users)).Returns(usersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(users);

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        var usersDto = new List<AppUserDto>();
        var users = new List<AppUser>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<AppUserDto>>(users)).Returns(usersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(users);

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_OneEntity_ShouldReturnOneEntity()
    {
        // Arrange
        const string userId = "uid-22";

        var userDto = TestDataFactory.CreateAppUserDto(id: userId);
        var user = TestDataFactory.CreateAppUser(id: userId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<AppUserDto>(user)).Returns(userDto);

        mockRepository.Setup(m => m.GetByIdAsync(userId)).ReturnsAsync(user);

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const string userId = "uid-22";

        var userDto = TestDataFactory.CreateAppUserDto(id: userId);
        var user = TestDataFactory.CreateAppUser(id: userId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<AppUserDto>(user)).Returns(userDto);

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(userId);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowExecption_ShouldNotReturnEntity()
    {
        // Arrange
        const string userId = "";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetByIdAsync(userId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task CheckByUsernameAsync_True_ShouldReturnThatUsernameAlreadyUsed()
    {
        // Arrange
        const string username = "Solinx";

        var usersDto = new List<AppUserDto> {
            TestDataFactory.CreateAppUserDto(username: username)
        };
        var users = new List<AppUser> {
            TestDataFactory.CreateAppUser(username: username)
        };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<AppUserDto>>(users)).Returns(usersDto);

        mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CheckByUsernameAsync(username);

        // Assert
        Assert.True(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task FindByIdentityUserIdAsync_AppUserDto_ShouldReturnAppUserDtoThatIndentifiedByIdentityUserId()
    {
        // Arrange
        const string identityUserId = "uid-11";

        var userDto = TestDataFactory.CreateAppUserDto(identityUserId: identityUserId);
        var user = TestDataFactory.CreateAppUser(identityUserId: identityUserId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<AppUserDto>(user)).Returns(userDto);

        mockRepository.Setup(r => r.FindByIdentityUserIdAsync(identityUserId)).ReturnsAsync(user);

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdentityUserIdAsync(identityUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto.Id, result.Id);
        Assert.Equal(userDto.FirstName, result.FirstName);
        Assert.Equal(userDto.LastName, result.LastName);
        Assert.Equal(userDto.Birthday, result.Birthday);
        Assert.Equal(userDto.Username, result.Username);
        Assert.Equal(userDto.AboutMe, result.AboutMe);
        Assert.Equal(userDto.PhoneNumber, result.PhoneNumber);
        Assert.Equal(userDto.Gender, result.Gender);
        Assert.Equal(userDto.IdentityUserId, result.IdentityUserId);

        // Verify correct method calls
        mockRepository.Verify(r => r.FindByIdentityUserIdAsync(It.IsAny<string>()), Times.Once);
        mockMapper.Verify(m => m.Map<AppUserDto>(It.IsAny<AppUser>()), Times.Once);
    }

    [Fact]
    public async Task FindByUsernameStartAtAsync_Collection_ShouldReturnCollectionOfUsersByStartAtUsername()
    {
        // Arrange
        const string startAtUsername = "sol";
        const string username1 = "solinx";
        const string username2 = "solena";

        var usersDto = new List<AppUserDto> {
            TestDataFactory.CreateAppUserDto(username: username1),
            TestDataFactory.CreateAppUserDto(username: username2)
        };
        var users = new List<AppUser> {
            TestDataFactory.CreateAppUser(username: username1),
            TestDataFactory.CreateAppUser(username: username2)
        };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<AppUserDto>>(users)).Returns(usersDto);

        mockRepository.Setup(r => r.FindByUsernameStartAtAsync(startAtUsername)).ReturnsAsync(users);

        var service = new UserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.FindByUsernameStartAtAsync(startAtUsername);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.FindByUsernameStartAtAsync(It.IsAny<string>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<AppUserDto>>(It.IsAny<List<AppUser>>()), Times.Once);
    }
}
