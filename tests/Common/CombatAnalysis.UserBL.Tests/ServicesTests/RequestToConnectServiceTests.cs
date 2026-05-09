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

public class RequestToConnectServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        const int requestToConnect1 = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";
        var now = DateTimeOffset.Now;

        var requestToConnectDto = new RequestToConnectDto(
            Id: requestToConnect1,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        );
        var requestToConnect = new RequestToConnect(
            Id: requestToConnect1,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        mockMapper.Setup(m => m.Map<RequestToConnect>(requestToConnectDto)).Returns(requestToConnect);
        mockMapper.Setup(m => m.Map<RequestToConnectDto>(requestToConnect)).Returns(requestToConnectDto);

        mockRepository.Setup(m => m.CreateAsync(requestToConnect)).ReturnsAsync(requestToConnect);

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(requestToConnectDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestToConnectDto.Id, result.Id);
        Assert.Equal(requestToConnectDto.ToAppUserId, result.ToAppUserId);
        Assert.Equal(requestToConnectDto.AppUserId, result.AppUserId);
        Assert.Equal(requestToConnectDto.When, result.When);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<RequestToConnect>(It.IsAny<RequestToConnectDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<RequestToConnect>()), Times.Once);
        mockMapper.Verify(m => m.Map<RequestToConnectDto>(It.IsAny<RequestToConnect>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntity()
    {
        // Arrange
        const int requestToConnect1 = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-222";
        var now = DateTimeOffset.Now;

        var requestToConnectDto = new RequestToConnectDto(
            Id: requestToConnect1,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<RequestToConnectException>(() => service.CreateAsync(requestToConnectDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<RequestToConnect>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int requestId = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(requestId);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int requestId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(requestId));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int requestToConnect1 = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-222";
        var now = DateTimeOffset.Now;

        var requests = new List<RequestToConnect> {
            new(
                Id: requestToConnect1,
                ToAppUserId: user1Id,
                AppUserId: user2Id,
                When: now
            ),
        };
        var requestsDto = new List<RequestToConnectDto> {
            new(
                Id: requestToConnect1,
                ToAppUserId: user1Id,
                AppUserId: user2Id,
                When: now
            ),
        };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<RequestToConnectDto>>(requests)).Returns(requestsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(requests);

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

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
        var requests = new List<RequestToConnect>();
        var requestsDto = new List<RequestToConnectDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<RequestToConnectDto>>(requests)).Returns(requestsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(requests);

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

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
        const int requestToConnect1 = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";
        var now = DateTimeOffset.Now;

        var requestToConnectDto = new RequestToConnectDto(
            Id: requestToConnect1,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        );
        var requestToConnect = new RequestToConnect(
            Id: requestToConnect1,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        mockMapper.Setup(m => m.Map<RequestToConnectDto>(requestToConnect)).Returns(requestToConnectDto);

        mockRepository.Setup(m => m.GetByIdAsync(requestToConnect1)).ReturnsAsync(requestToConnect);

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(requestToConnect1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestToConnect1, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const int requestToConnect1 = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";
        var now = DateTimeOffset.Now;

        var requestToConnectDto = new RequestToConnectDto(
            Id: requestToConnect1,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        );
        var requestToConnect = new RequestToConnect(
            Id: requestToConnect1,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        );

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        mockMapper.Setup(m => m.Map<RequestToConnectDto>(requestToConnect)).Returns(requestToConnectDto);

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(requestToConnect1);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowExecption_ShouldNotReturnEntity()
    {
        // Arrange
        const int requestToConnect1 = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        var service = new RequestToConnectService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(requestToConnect1));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int requestToConnect1 = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-222";
        var now = DateTimeOffset.Now;

        var requests = new List<RequestToConnect> {
            new(
                Id: requestToConnect1,
                ToAppUserId: user1Id,
                AppUserId: user2Id,
                When: now
            ),
        };
        var requestsDto = new List<RequestToConnectDto> {
            new(
                Id: requestToConnect1,
                ToAppUserId: user1Id,
                AppUserId: user2Id,
                When: now
            ),
        };
        Expression<Func<RequestToConnect, string>> expression = c => c.ToAppUserId;
        Expression<Func<RequestToConnectDto, string>> expressionDto = c => c.ToAppUserId;

        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<RequestToConnect, string>>>(), user1Id))
            .ReturnsAsync(requests);

        var service = new RequestToConnectService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.ToAppUserId, user1Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<RequestToConnect, string>>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const string calledUserId = "uid-221";

        var requests = new List<RequestToConnect>();
        var requestsDto = new List<RequestToConnectDto>();

        Expression<Func<RequestToConnect, string>> expression = c => c.ToAppUserId;
        Expression<Func<RequestToConnectDto, string>> expressionDto = c => c.ToAppUserId;

        var mockRepository = new Mock<IGenericRepository<RequestToConnect, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<RequestToConnect, string>>>(), calledUserId))
            .ReturnsAsync(requests);

        var service = new RequestToConnectService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.ToAppUserId, calledUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<RequestToConnect, string>>>(), It.IsAny<string>()), Times.Once);
    }
}
