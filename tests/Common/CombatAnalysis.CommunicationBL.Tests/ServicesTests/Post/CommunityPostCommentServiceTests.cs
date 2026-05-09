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

public class CommunityPostCommentServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = CommunityPostCommentTestDataFactory.CreateDto();
        var entity = CommunityPostCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostComment>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CommunityPostCommentDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

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
        mockMapper.Verify(m => m.Map<CommunityPostComment>(It.IsAny<CommunityPostCommentDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityPostComment>()), Times.Once);
        mockMapper.Verify(m => m.Map<CommunityPostCommentDto>(It.IsAny<CommunityPostComment>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int communityId = 0;

        var entityDto = CommunityPostCommentTestDataFactory.CreateDto(communityId: communityId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityPostCommentDto.CommunityId), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityPostComment>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = CommunityPostCommentTestDataFactory.CreateDto();
        var entity = CommunityPostCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostComment>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityPostComment>(It.IsAny<CommunityPostCommentDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPostComment>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = CommunityPostCommentTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPostComment>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int id = 1;
        const int communityId = 0;

        var entityDto = CommunityPostCommentTestDataFactory.CreateDto(communityId: communityId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityPostCommentDto.CommunityId), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPostComment>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = CommunityPostCommentTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostCommentTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostCommentDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

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
        var customers = new List<CommunityPostComment>();
        var customersDto = new List<CommunityPostCommentDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostCommentDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = CommunityPostCommentTestDataFactory.CreateDto();
        var entity = CommunityPostCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostCommentDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = CommunityPostCommentTestDataFactory.CreateDto();
        var entity = CommunityPostCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostCommentDto>(entity)).Returns(entityDto);

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        var service = new CommunityPostCommentService(mockRepository.Object, mockMapper.Object);

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

        var entitiesDto = CommunityPostCommentTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostCommentTestDataFactory.CreateCollection();

        Expression<Func<CommunityPostComment, int>> expression = c => c.CommunityId;
        Expression<Func<CommunityPostCommentDto, int>> expressionDto = c => c.CommunityId;

        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPostComment, int>>>(), communitId))
            .ReturnsAsync(entities);

        var service = new CommunityPostCommentService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityId, communitId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPostComment, int>>>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int communitId = 23;

        var customerUsers = new List<CommunityPostComment>();
        var customerUsersDto = new List<CommunityPostCommentDto>();

        Expression<Func<CommunityPostComment, int>> expression = c => c.CommunityId;
        Expression<Func<CommunityPostCommentDto, int>> expressionDto = c => c.CommunityId;

        var mockRepository = new Mock<IGenericRepository<CommunityPostComment, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPostComment, int>>>(), communitId))
            .ReturnsAsync(customerUsers);

        var service = new CommunityPostCommentService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityId, communitId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPostComment, int>>>(), It.IsAny<int>()), Times.Once);
    }
}
