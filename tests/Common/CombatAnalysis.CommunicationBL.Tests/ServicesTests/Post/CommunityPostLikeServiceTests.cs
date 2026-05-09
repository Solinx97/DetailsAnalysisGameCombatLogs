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

public class CommunityPostLikeServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = CommunityPostLikeTestDataFactory.CreateDto();
        var entity = CommunityPostLikeTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostLike>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CommunityPostLikeDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.CreatedAt, result.CreatedAt);
        Assert.Equal(entityDto.CommunityId, result.CommunityId);
        Assert.Equal(entityDto.CommunityPostId, result.CommunityPostId);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityPostLike>(It.IsAny<CommunityPostLikeDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityPostLike>()), Times.Once);
        mockMapper.Verify(m => m.Map<CommunityPostLikeDto>(It.IsAny<CommunityPostLike>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int communityId = 0;

        var entityDto = CommunityPostLikeTestDataFactory.CreateDto(communityId: communityId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityPostLikeDto.CommunityId), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityPostLike>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = CommunityPostLikeTestDataFactory.CreateDto();
        var entity = CommunityPostLikeTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostLike>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityPostLike>(It.IsAny<CommunityPostLikeDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPostLike>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = CommunityPostLikeTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPostLike>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int id = 1;
        const int communityId = 0;

        var entityDto = CommunityPostLikeTestDataFactory.CreateDto(communityId: communityId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityPostLikeDto.CommunityId), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPostLike>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = CommunityPostLikeTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostLikeTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostLikeDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

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
        var customers = new List<CommunityPostLike>();
        var customersDto = new List<CommunityPostLikeDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostLikeDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = CommunityPostLikeTestDataFactory.CreateDto();
        var entity = CommunityPostLikeTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostLikeDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = CommunityPostLikeTestDataFactory.CreateDto();
        var entity = CommunityPostLikeTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostLikeDto>(entity)).Returns(entityDto);

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        var service = new CommunityPostLikeService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int communitId = 1;

        var entitiesDto = CommunityPostLikeTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostLikeTestDataFactory.CreateCollection();

        Expression<Func<CommunityPostLike, int>> expression = c => c.CommunityId;
        Expression<Func<CommunityPostLikeDto, int>> expressionDto = c => c.CommunityId;

        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPostLike, int>>>(), communitId))
            .ReturnsAsync(entities);

        var service = new CommunityPostLikeService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityId, communitId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPostLike, int>>>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int communitId = 23;

        var customerUsers = new List<CommunityPostLike>();
        var customerUsersDto = new List<CommunityPostLikeDto>();

        Expression<Func<CommunityPostLike, int>> expression = c => c.CommunityId;
        Expression<Func<CommunityPostLikeDto, int>> expressionDto = c => c.CommunityId;

        var mockRepository = new Mock<IGenericRepository<CommunityPostLike, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPostLike, int>>>(), communitId))
            .ReturnsAsync(customerUsers);

        var service = new CommunityPostLikeService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityId, communitId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPostLike, int>>>(), It.IsAny<int>()), Times.Once);
    }
}
