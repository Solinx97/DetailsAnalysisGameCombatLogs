using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Mapping;
using CombatAnalysis.CommunicationBL.Services.Post;
using CombatAnalysis.CommunicationBL.Tests.Factory;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Tests.ServicesTests.Post;

public class UserPostDislikeServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = UserPostDislikeTestDataFactory.CreateDto();
        var entity = UserPostDislikeTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        mockMapper.Setup(m => m.Map<UserPostDislike>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<UserPostDislikeDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.UserPostId, result.UserPostId);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<UserPostDislike>(It.IsAny<UserPostDislikeDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<UserPostDislike>()), Times.Once);
        mockMapper.Verify(m => m.Map<UserPostDislikeDto>(It.IsAny<UserPostDislike>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int userPostId = 0;

        var entityDto = UserPostDislikeTestDataFactory.CreateDto(userPostId: userPostId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(UserPostDislikeDto.UserPostId), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<UserPostDislike>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = UserPostDislikeTestDataFactory.CreateDto();
        var entity = UserPostDislikeTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        mockMapper.Setup(m => m.Map<UserPostDislike>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<UserPostDislike>(It.IsAny<UserPostDislikeDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPostDislike>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = UserPostDislikeTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPostDislike>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int id = 1;
        const int userPostId = 0;

        var entityDto = UserPostDislikeTestDataFactory.CreateDto(userPostId: userPostId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(UserPostDislikeDto.UserPostId), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPostDislike>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(id);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int id = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = UserPostDislikeTestDataFactory.CreateDtoCollection();
        var entities = UserPostDislikeTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDislikeDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        var customers = new List<UserPostDislike>();
        var customersDto = new List<UserPostDislikeDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDislikeDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

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
        const int id = 1;

        var entityDto = UserPostDislikeTestDataFactory.CreateDto();
        var entity = UserPostDislikeTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        mockMapper.Setup(m => m.Map<UserPostDislikeDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const int id = 32;

        var entityDto = UserPostDislikeTestDataFactory.CreateDto();
        var entity = UserPostDislikeTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        mockMapper.Setup(m => m.Map<UserPostDislikeDto>(entity)).Returns(entityDto);

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowExecption_ShouldNotReturnEntity()
    {
        // Arrange
        const int id = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        var service = new UserPostDislikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int userPostId = 1;

        var entitiesDto = UserPostDislikeTestDataFactory.CreateDtoCollection();
        var entities = UserPostDislikeTestDataFactory.CreateCollection();

        Expression<Func<UserPostDislike, int>> expression = c => c.UserPostId;
        Expression<Func<UserPostDislikeDto, int>> expressionDto = c => c.UserPostId;

        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPostDislike, int>>>(), userPostId))
            .ReturnsAsync(entities);

        var service = new UserPostDislikeService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.UserPostId, userPostId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPostDislike, int>>>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int userPostId = 23;

        var customerUsers = new List<UserPostDislike>();
        var customerUsersDto = new List<UserPostDislikeDto>();

        Expression<Func<UserPostDislike, int>> expression = c => c.UserPostId;
        Expression<Func<UserPostDislikeDto, int>> expressionDto = c => c.UserPostId;

        var mockRepository = new Mock<IGenericRepository<UserPostDislike, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPostDislike, int>>>(), userPostId))
            .ReturnsAsync(customerUsers);

        var service = new UserPostDislikeService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.UserPostId, userPostId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPostDislike, int>>>(), It.IsAny<int>()), Times.Once);
    }
}
