using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Mapping;
using CombatAnalysis.CommunicationBL.Services.Community;
using CombatAnalysis.CommunicationBL.Tests.Factory;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Tests.ServicesTests.Community;

public class CommunityDiscussionCommentServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = CommunityDiscussionCommentTestDataFactory.CreateDto();
        var entity = CommunityDiscussionCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        mockMapper.Setup(m => m.Map<CommunityDiscussionComment>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CommunityDiscussionCommentDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Content, result.Content);
        Assert.Equal(entityDto.When, result.When);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);
        Assert.Equal(entityDto.CommunityDiscussionId, result.CommunityDiscussionId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityDiscussionComment>(It.IsAny<CommunityDiscussionCommentDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityDiscussionComment>()), Times.Once);
        mockMapper.Verify(m => m.Map<CommunityDiscussionCommentDto>(It.IsAny<CommunityDiscussionComment>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int communitDiscussionId = 0;

        var entityDto = CommunityDiscussionCommentTestDataFactory.CreateDto(communityCommentId: communitDiscussionId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityDiscussionCommentDto.CommunityDiscussionId), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityDiscussionComment>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = CommunityDiscussionCommentTestDataFactory.CreateDto();
        var entity = CommunityDiscussionCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        mockMapper.Setup(m => m.Map<CommunityDiscussionComment>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityDiscussionComment>(It.IsAny<CommunityDiscussionCommentDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityDiscussionComment>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = CommunityDiscussionCommentTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityDiscussionComment>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int id = 1;
        const int communitDiscussionId = 0;

        var entityDto = CommunityDiscussionCommentTestDataFactory.CreateDto(communityCommentId: communitDiscussionId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityDiscussionCommentDto.CommunityDiscussionId), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityDiscussionComment>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = CommunityDiscussionCommentTestDataFactory.CreateDtoCollection();
        var entities = CommunityDiscussionCommentTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDiscussionCommentDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

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
        var customers = new List<CommunityDiscussionComment>();
        var customersDto = new List<CommunityDiscussionCommentDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDiscussionCommentDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = CommunityDiscussionCommentTestDataFactory.CreateDto();
        var entity = CommunityDiscussionCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        mockMapper.Setup(m => m.Map<CommunityDiscussionCommentDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = CommunityDiscussionCommentTestDataFactory.CreateDto();
        var entity = CommunityDiscussionCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        mockMapper.Setup(m => m.Map<CommunityDiscussionCommentDto>(entity)).Returns(entityDto);

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int communitDiscussionId = 1;

        var entitiesDto = CommunityDiscussionCommentTestDataFactory.CreateDtoCollection();
        var entities = CommunityDiscussionCommentTestDataFactory.CreateCollection();

        Expression<Func<CommunityDiscussionComment, int>> expression = c => c.CommunityDiscussionId;
        Expression<Func<CommunityDiscussionCommentDto, int>> expressionDto = c => c.CommunityDiscussionId;

        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityDiscussionComment, int>>>(), communitDiscussionId))
            .ReturnsAsync(entities);

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityDiscussionId, communitDiscussionId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityDiscussionComment, int>>>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int communitDiscussionId = 23;

        var customerUsers = new List<CommunityDiscussionComment>();
        var customerUsersDto = new List<CommunityDiscussionCommentDto>();

        Expression<Func<CommunityDiscussionComment, int>> expression = c => c.CommunityDiscussionId;
        Expression<Func<CommunityDiscussionCommentDto, int>> expressionDto = c => c.CommunityDiscussionId;

        var mockRepository = new Mock<IGenericRepository<CommunityDiscussionComment, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityDiscussionComment, int>>>(), communitDiscussionId))
            .ReturnsAsync(customerUsers);

        var service = new CommunityDiscussionCommentService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityDiscussionId, communitDiscussionId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityDiscussionComment, int>>>(), It.IsAny<int>()), Times.Once);
    }
}
