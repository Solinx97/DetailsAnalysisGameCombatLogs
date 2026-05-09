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

public class UserPostCommentServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = UserPostCommentTestDataFactory.CreateDto();
        var entity = UserPostCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        mockMapper.Setup(m => m.Map<UserPostComment>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<UserPostCommentDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.UserPostId, result.UserPostId);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<UserPostComment>(It.IsAny<UserPostCommentDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<UserPostComment>()), Times.Once);
        mockMapper.Verify(m => m.Map<UserPostCommentDto>(It.IsAny<UserPostComment>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int userPostId = 0;

        var entityDto = UserPostCommentTestDataFactory.CreateDto(userPostId: userPostId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(UserPostCommentDto.UserPostId), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<UserPostComment>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = UserPostCommentTestDataFactory.CreateDto();
        var entity = UserPostCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        mockMapper.Setup(m => m.Map<UserPostComment>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<UserPostComment>(It.IsAny<UserPostCommentDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPostComment>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = UserPostCommentTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPostComment>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const int id = 1;
        const int userPostId = 0;

        var entityDto = UserPostCommentTestDataFactory.CreateDto(userPostId: userPostId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(UserPostCommentDto.UserPostId), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPostComment>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = UserPostCommentTestDataFactory.CreateDtoCollection();
        var entities = UserPostCommentTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostCommentDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

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
        var customers = new List<UserPostComment>();
        var customersDto = new List<UserPostCommentDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostCommentDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = UserPostCommentTestDataFactory.CreateDto();
        var entity = UserPostCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        mockMapper.Setup(m => m.Map<UserPostCommentDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

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

        var entityDto = UserPostCommentTestDataFactory.CreateDto();
        var entity = UserPostCommentTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        mockMapper.Setup(m => m.Map<UserPostCommentDto>(entity)).Returns(entityDto);

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        var service = new UserPostCommentService(mockRepository.Object, mockMapper.Object);

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

        var entitiesDto = UserPostCommentTestDataFactory.CreateDtoCollection();
        var entities = UserPostCommentTestDataFactory.CreateCollection();

        Expression<Func<UserPostComment, int>> expression = c => c.UserPostId;
        Expression<Func<UserPostCommentDto, int>> expressionDto = c => c.UserPostId;

        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPostComment, int>>>(), userPostId))
            .ReturnsAsync(entities);

        var service = new UserPostCommentService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.UserPostId, userPostId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPostComment, int>>>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int userPostId = 23;

        var customerUsers = new List<UserPostComment>();
        var customerUsersDto = new List<UserPostCommentDto>();

        Expression<Func<UserPostComment, int>> expression = c => c.UserPostId;
        Expression<Func<UserPostCommentDto, int>> expressionDto = c => c.UserPostId;

        var mockRepository = new Mock<IGenericRepository<UserPostComment, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPostComment, int>>>(), userPostId))
            .ReturnsAsync(customerUsers);

        var service = new UserPostCommentService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.UserPostId, userPostId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPostComment, int>>>(), It.IsAny<int>()), Times.Once);
    }
}
