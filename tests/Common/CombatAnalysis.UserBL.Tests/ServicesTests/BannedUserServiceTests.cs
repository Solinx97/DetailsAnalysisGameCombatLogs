using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Exceptions;
using CombatAnalysis.UserBL.Mapping;
using CombatAnalysis.UserBL.Services;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CombatAnalysis.UserBL.Tests.ServicesTests;

public class BannedUserServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        const int bannedUserId = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";

        var bannedUserDto = new BannedUserDto(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        );
        var bannedUser = new BannedUser(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        mockMapper.Setup(m => m.Map<BannedUser>(bannedUserDto)).Returns(bannedUser);
        mockMapper.Setup(m => m.Map<BannedUserDto>(bannedUser)).Returns(bannedUserDto);

        mockRepository.Setup(m => m.CreateAsync(bannedUser)).ReturnsAsync(bannedUser);

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(bannedUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bannedUserDto.Id, result.Id);
        Assert.Equal(bannedUserDto.WhomBannedId, result.WhomBannedId);
        Assert.Equal(bannedUserDto.BannedUserId, result.BannedUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<BannedUser>(It.IsAny<BannedUserDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<BannedUser>()), Times.Once);
        mockMapper.Verify(m => m.Map<BannedUserDto>(It.IsAny<BannedUser>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntity()
    {
        // Arrange
        const int bannedUserId = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-222";

        var bannedUserDto = new BannedUserDto(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<BannedUserException>(() => service.CreateAsync(bannedUserDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<BannedUser>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int bannedUserId = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(bannedUserId);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int bannedUserId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(bannedUserId));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int bannedUserId1 = 1;
        const string user1Id1 = "uid-222";
        const string user1Id2 = "uid-223";

        var bannedUsers = new List<BannedUser> {
            new(
                Id: bannedUserId1,
                WhomBannedId: user1Id1,
                BannedUserId: user1Id2
            ),
        };
        var bannedUsersDto = new List<BannedUserDto> {
            new(
                Id: bannedUserId1,
                WhomBannedId: user1Id1,
                BannedUserId: user1Id2
            ),
        };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<BannedUserDto>>(bannedUsers)).Returns(bannedUsersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(bannedUsers);

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

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
        var bannedUsers = new List<BannedUser>();
        var bannedUsersDto = new List<BannedUserDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<BannedUserDto>>(bannedUsers)).Returns(bannedUsersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(bannedUsers);

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

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
        const int bannedUserId = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";

        var bannedUserDto = new BannedUserDto(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        );
        var bannedUser = new BannedUser(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        mockMapper.Setup(m => m.Map<BannedUserDto>(bannedUser)).Returns(bannedUserDto);

        mockRepository.Setup(m => m.GetByIdAsync(bannedUserId)).ReturnsAsync(bannedUser);

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(bannedUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bannedUserId, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const int bannedUserId = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";

        var bannedUserDto = new BannedUserDto(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        );
        var bannedUser = new BannedUser(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        mockMapper.Setup(m => m.Map<BannedUserDto>(bannedUser)).Returns(bannedUserDto);

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(bannedUserId);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowExecption_ShouldNotReturnEntity()
    {
        // Arrange
        const int bannedUserId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        var service = new BannedUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(bannedUserId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int bannedUserId1 = 1;
        const string user1Id1 = "uid-222";
        const string user1Id2 = "uid-223";

        var bannedUsers = new List<BannedUser> {
            new(
                Id: bannedUserId1,
                WhomBannedId: user1Id1,
                BannedUserId: user1Id2
            ),
        };
        var bannedUsersDto = new List<BannedUserDto> {
            new(
                Id: bannedUserId1,
                WhomBannedId: user1Id1,
                BannedUserId: user1Id2
            ),
        };
        Expression<Func<BannedUser, string>> expression = c => c.WhomBannedId;
        Expression<Func<BannedUserDto, string>> expressionDto = c => c.WhomBannedId;

        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<BannedUser, string>>>(), user1Id1))
            .ReturnsAsync(bannedUsers);

        var service = new BannedUserService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.WhomBannedId, user1Id1);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<BannedUser, string>>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const string calledUserId = "uid-221";

        var bannedUsers = new List<BannedUser>();
        var bannedUsersDto = new List<BannedUserDto>();

        Expression<Func<BannedUser, string>> expression = c => c.WhomBannedId;
        Expression<Func<BannedUserDto, string>> expressionDto = c => c.WhomBannedId;

        var mockRepository = new Mock<IGenericRepository<BannedUser, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<BannedUser, string>>>(), calledUserId))
            .ReturnsAsync(bannedUsers);

        var service = new BannedUserService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.WhomBannedId, calledUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<BannedUser, string>>>(), It.IsAny<string>()), Times.Once);
    }
}
