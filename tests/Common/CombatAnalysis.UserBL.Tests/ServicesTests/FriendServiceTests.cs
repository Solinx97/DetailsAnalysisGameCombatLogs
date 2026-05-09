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

public class FriendServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        const int friendId = 1;
        const string user1Id = "uid-222";
        const string user1Username = "Solinx";
        const string user2Id = "uid-223";
        const string user2Username = "Kiril";

        var friendDALDto = new UserDAL.DTO.FriendDto(
            Id: friendId,
            WhoFriendUsername: user1Username,
            WhoFriendId: user1Id,
            ForWhomUsername: user2Username,
            ForWhomId: user2Id
        );
        var friendDto = new FriendDto(
            Id: friendId,
            WhoFriendUsername: user1Username,
            WhoFriendId: user1Id,
            ForWhomUsername: user2Username,
            ForWhomId: user2Id
        );
        var friend = new Friend(
            Id: friendId,
            WhoFriendId: user1Id,
            ForWhomId: user2Id
        );
        var friendCreateDto = new FriendCreateDto(
            Id: friendId,
            WhoFriendId: user1Id,
            ForWhomId: user2Id
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        mockMapper.Setup(m => m.Map<Friend>(friendCreateDto)).Returns(friend);
        mockMapper.Setup(m => m.Map<FriendDto>(friendDALDto)).Returns(friendDto);

        mockRepository.Setup(m => m.CreateAsync(friend)).ReturnsAsync(friendDALDto);

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(friendCreateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(friendDALDto.Id, result.Id);
        Assert.Equal(friendDALDto.WhoFriendUsername, result.WhoFriendUsername);
        Assert.Equal(friendDALDto.WhoFriendId, result.WhoFriendId);
        Assert.Equal(friendDALDto.ForWhomUsername, result.ForWhomUsername);
        Assert.Equal(friendDALDto.ForWhomId, result.ForWhomId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<Friend>(It.IsAny<FriendCreateDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Friend>()), Times.Once);
        mockMapper.Verify(m => m.Map<FriendDto>(It.IsAny<UserDAL.DTO.FriendDto>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntity()
    {
        // Arrange
        const int friendId = 1;
        const string user1Id = "uid-222";
        const string user1Username = "Solinx";
        const string user2Id = "uid-222";
        const string user2Username = "Solinx";

        var friendDALDto = new UserDAL.DTO.FriendDto(
            Id: friendId,
            WhoFriendUsername: user1Username,
            WhoFriendId: user1Id,
            ForWhomUsername: user2Username,
            ForWhomId: user2Id
        );
        var friend = new Friend(
            Id: friendId,
            WhoFriendId: user1Id,
            ForWhomId: user2Id
        );
        var friendCreateDto = new FriendCreateDto(
            Id: friendId,
            WhoFriendId: user1Id,
            ForWhomId: user2Id
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        mockRepository.Setup(m => m.CreateAsync(friend)).ReturnsAsync(friendDALDto);

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<FriendException>(() => service.CreateAsync(friendCreateDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Friend>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int friendId = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(friendId);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int friendId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(friendId));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int friendId = 1;
        const string user1Id = "uid-222";
        const string user1Username = "Solinx";
        const string user2Id = "uid-222";
        const string user2Username = "Solinx";

        var friendsDALDto = new List<UserDAL.DTO.FriendDto> {
            new(
                Id: friendId,
                WhoFriendUsername: user1Username,
                WhoFriendId: user1Id,
                ForWhomUsername: user2Username,
                ForWhomId: user2Id
            )
        };
        var friendsDto = new List<FriendDto> {
            new(
                Id: friendId,
                WhoFriendUsername: user1Username,
                WhoFriendId: user1Id,
                ForWhomUsername: user2Username,
                ForWhomId: user2Id
            )
        };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<FriendDto>>(friendsDALDto)).Returns(friendsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(friendsDALDto);

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

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
        var friendsDALDto = new List<UserDAL.DTO.FriendDto>();
        var friendsDto = new List<FriendDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<FriendDto>>(friendsDALDto)).Returns(friendsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(friendsDALDto);

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

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
        const int friendId = 1;
        const string user1Id = "uid-222";
        const string user1Username = "Solinx";
        const string user2Id = "uid-222";
        const string user2Username = "Solinx";

        var friendDto = new FriendDto(
            Id: friendId,
            WhoFriendUsername: user1Username,
            WhoFriendId: user1Id,
            ForWhomUsername: user2Username,
            ForWhomId: user2Id
        );
        var friend = new UserDAL.DTO.FriendDto (
            Id: friendId,
            WhoFriendUsername: user1Username,
            WhoFriendId: user1Id,
            ForWhomUsername: user2Username,
            ForWhomId: user2Id
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        mockMapper.Setup(m => m.Map<FriendDto>(friend)).Returns(friendDto);

        mockRepository.Setup(m => m.GetByIdAsync(friendId)).ReturnsAsync(friend);

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(friendId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(friendId, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const int friendId = 1;
        const string user1Id = "uid-222";
        const string user1Username = "Solinx";
        const string user2Id = "uid-222";
        const string user2Username = "Solinx";

        var friendDto = new FriendDto(
            Id: friendId,
            WhoFriendUsername: user1Username,
            WhoFriendId: user1Id,
            ForWhomUsername: user2Username,
            ForWhomId: user2Id
        );
        var friend = new UserDAL.DTO.FriendDto(
            Id: friendId,
            WhoFriendUsername: user1Username,
            WhoFriendId: user1Id,
            ForWhomUsername: user2Username,
            ForWhomId: user2Id
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        mockMapper.Setup(m => m.Map<FriendDto>(friend)).Returns(friendDto);

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(friendId);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowExecption_ShouldNotReturnEntity()
    {
        // Arrange
        const int friendId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IFriendRepository>();

        var service = new FriendService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(friendId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int friendId = 1;
        const string user1Id = "uid-222";
        const string user1Username = "Solinx";
        const string user2Id = "uid-222";
        const string user2Username = "Solinx";

        var friendsDALDto = new List<UserDAL.DTO.FriendDto> {
            new(
                Id: friendId,
                WhoFriendUsername: user1Username,
                WhoFriendId: user1Id,
                ForWhomUsername: user2Username,
                ForWhomId: user2Id
            )
        };
        var friendsDto = new List<FriendDto> {
            new(
                Id: friendId,
                WhoFriendUsername: user1Username,
                WhoFriendId: user1Id,
                ForWhomUsername: user2Username,
                ForWhomId: user2Id
            )
        };
        Expression<Func<Friend, string>> expression = c => c.WhoFriendId;
        Expression<Func<FriendDto, string>> expressionDto = c => c.WhoFriendId;

        var mockRepository = new Mock<IFriendRepository>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<string>(), user1Id))
            .ReturnsAsync(friendsDALDto);

        var service = new FriendService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(nameof(FriendDto.WhoFriendId), user1Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const string calledUserId = "uid-222";

        var friendsDALDto = new List<UserDAL.DTO.FriendDto>();
        var friendsDto = new List<FriendDto>();
        Expression<Func<Friend, string>> expression = c => c.WhoFriendId;
        Expression<Func<FriendDto, string>> expressionDto = c => c.WhoFriendId;

        var mockRepository = new Mock<IFriendRepository>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<string>(), calledUserId))
            .ReturnsAsync(friendsDALDto);

        var service = new FriendService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(nameof(FriendDto.WhoFriendId), calledUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
