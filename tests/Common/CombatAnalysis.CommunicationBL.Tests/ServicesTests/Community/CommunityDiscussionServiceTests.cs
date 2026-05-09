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

public class CommunityDiscussionServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = CommunityDiscussionTestDataFactory.CreateDto();
        var entity = CommunityDiscussionTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        mockMapper.Setup(m => m.Map<CommunityDiscussion>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CommunityDiscussionDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Title, result.Title);
        Assert.Equal(entityDto.Content, result.Content);
        Assert.Equal(entityDto.When, result.When);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);
        Assert.Equal(entityDto.CommunityId, result.CommunityId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityDiscussion>(It.IsAny<CommunityDiscussionDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityDiscussion>()), Times.Once);
        mockMapper.Verify(m => m.Map<CommunityDiscussionDto>(It.IsAny<CommunityDiscussion>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int communityId = 0;

        var entityDto = CommunityDiscussionTestDataFactory.CreateDto(communityId: communityId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityDiscussionDto.CommunityId), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityDiscussion>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = CommunityDiscussionTestDataFactory.CreateDto();
        var entity = CommunityDiscussionTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        mockMapper.Setup(m => m.Map<CommunityDiscussion>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityDiscussion>(It.IsAny<CommunityDiscussionDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityDiscussion>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = CommunityDiscussionTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityDiscussion>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int id = 1;
        const int communityId = 0;

        var entityDto = CommunityDiscussionTestDataFactory.CreateDto(communityId: communityId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityDiscussionDto.CommunityId), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityDiscussion>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = CommunityDiscussionTestDataFactory.CreateDtoCollection();
        var entities = CommunityDiscussionTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDiscussionDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

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
        var customers = new List<CommunityDiscussion>();
        var customersDto = new List<CommunityDiscussionDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDiscussionDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = CommunityDiscussionTestDataFactory.CreateDto();
        var entity = CommunityDiscussionTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        mockMapper.Setup(m => m.Map<CommunityDiscussionDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = CommunityDiscussionTestDataFactory.CreateDto();
        var entity = CommunityDiscussionTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        mockMapper.Setup(m => m.Map<CommunityDiscussionDto>(entity)).Returns(entityDto);

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        var service = new CommunityDiscussionService(mockRepository.Object, mockMapper.Object);

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

        var entitiesDto = CommunityDiscussionTestDataFactory.CreateDtoCollection();
        var entities = CommunityDiscussionTestDataFactory.CreateCollection();

        Expression<Func<CommunityDiscussion, int>> expression = c => c.CommunityId;
        Expression<Func<CommunityDiscussionDto, int>> expressionDto = c => c.CommunityId;

        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityDiscussion, int>>>(), communitId))
            .ReturnsAsync(entities);

        var service = new CommunityDiscussionService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityId, communitId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityDiscussion, int>>>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int communitId = 23;

        var customerUsers = new List<CommunityDiscussion>();
        var customerUsersDto = new List<CommunityDiscussionDto>();

        Expression<Func<CommunityDiscussion, int>> expression = c => c.CommunityId;
        Expression<Func<CommunityDiscussionDto, int>> expressionDto = c => c.CommunityId;

        var mockRepository = new Mock<IGenericRepository<CommunityDiscussion, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityDiscussion, int>>>(), communitId))
            .ReturnsAsync(customerUsers);

        var service = new CommunityDiscussionService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityId, communitId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityDiscussion, int>>>(), It.IsAny<int>()), Times.Once);
    }
}
