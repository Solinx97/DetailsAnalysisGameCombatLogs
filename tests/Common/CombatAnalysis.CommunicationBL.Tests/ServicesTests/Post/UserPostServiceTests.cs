using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationBL.Mapping;
using CombatAnalysis.CommunicationBL.Services.Post;
using CombatAnalysis.CommunicationBL.Tests.Factory;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Tests.ServicesTests.Post;

public class UserPostServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = UserPostTestDataFactory.CreateDto();
        var entity = UserPostTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<UserPost>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<UserPostDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object, 
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Owner, result.Owner);
        Assert.Equal(entityDto.Content, result.Content);
        Assert.Equal(entityDto.PublicType, result.PublicType);
        Assert.Equal(entityDto.Tags, result.Tags);
        Assert.Equal(entityDto.CreatedAt, result.CreatedAt);
        Assert.Equal(entityDto.LikeCount, result.LikeCount);
        Assert.Equal(entityDto.DislikeCount, result.DislikeCount);
        Assert.Equal(entityDto.CommentCount, result.CommentCount);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<UserPost>(It.IsAny<UserPostDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<UserPost>()), Times.Once);
        mockMapper.Verify(m => m.Map<UserPostDto>(It.IsAny<UserPost>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsUserPostNameEmpty()
    {
        // Arrange
        const string content = "";

        var entityDto = UserPostTestDataFactory.CreateDto(content: content);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(UserPostDto.Content), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<UserPost>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = UserPostTestDataFactory.CreateDto();
        var entity = UserPostTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<UserPost>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<UserPost>(It.IsAny<UserPostDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPost>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = UserPostTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPost>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsUserPostNameEmpty()
    {
        // Arrange
        const int id = 1;
        const string content = "";

        var entityDto = UserPostTestDataFactory.CreateDto(content: content);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(UserPostDto.Content), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<UserPost>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var transactionMock = new Mock<IDbContextTransaction>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockSQLRepository.Setup(s => s.UseTransactionAsync()).ReturnsAsync(transactionMock.Object);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        await service.DeleteAsync(id);

        // Verify correct method calls
        mockSQLRepository.Verify(r => r.UseTransactionAsync(), Times.Once);
        transactionMock.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int id = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

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
        var customers = new List<UserPost>();
        var customersDto = new List<UserPostDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

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

        var entityDto = UserPostTestDataFactory.CreateDto();
        var entity = UserPostTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<UserPostDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

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

        var entityDto = UserPostTestDataFactory.CreateDto();
        var entity = UserPostTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<UserPostDto>(entity)).Returns(entityDto);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

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
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const string content = "content";

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        Expression<Func<UserPost, string>> expression = c => c.Content;
        Expression<Func<UserPostDto, string>> expressionDto = c => c.Content;

        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPost, string>>>(), content))
            .ReturnsAsync(entities);

        var service = new UserPostService(mockRepository.Object, mapper, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetByParamAsync(c => c.Content, content);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPost, string>>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const string content = "d content";

        var entities = new List<UserPost>();
        var entitiesDto = new List<UserPostDto>();

        Expression<Func<UserPost, string>> expression = c => c.Content;
        Expression<Func<UserPostDto, string>> expressionDto = c => c.Content;

        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPost, string>>>(), content))
            .ReturnsAsync(entities);

        var service = new UserPostService(mockRepository.Object, mapper, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetByParamAsync(c => c.Content, content);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<UserPost, string>>>(), It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetByCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const string userId = "uid-1";

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetByAppUserIdAsync(userId, pageSize)).ReturnsAsync(entities);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetByAppUserIdAsync(userId, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByAppUserIdAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetMoreByCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const int offset = 0;
        const string userId = "uid-1";

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetMoreByAppUserIdAsync(userId, offset, pageSize)).ReturnsAsync([]);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetMoreByAppUserIdAsync(userId, offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetMoreByAppUserIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetNewByCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const string userId = "uid-1";
        DateTimeOffset checkFrom = DateTimeOffset.Parse("11/23/2025");

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetNewByAppUserIdAsync(userId, checkFrom)).ReturnsAsync(entities);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetNewByAppUserIdAsync(userId, checkFrom);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetNewByAppUserIdAsync(It.IsAny<string>(), It.IsAny<DateTimeOffset>()), Times.Once);
    }

    [Fact]
    public async Task GetByListOfCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const string userIds = "uid-1,uid-2";

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetByListOfAppUserIdAsync(userIds, pageSize)).ReturnsAsync(entities);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetByListOfAppUserIdAsync(userIds, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByListOfAppUserIdAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetMoreByListOfCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const int offset = 0;
        const string userIds = "uid-1,uid-2";

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetMoreByListOfAppUserIdAsync(userIds, offset, pageSize)).ReturnsAsync(entities);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetMoreByListOfAppUserIdAsync(userIds, offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetMoreByListOfAppUserIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetNewByListOfCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const string userIds = "uid-1,uid-2";
        DateTimeOffset checkFrom = DateTimeOffset.Parse("11/23/2025");

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetNewByListOfAppUserIdAsync(userIds, checkFrom)).ReturnsAsync(entities);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetNewByListOfAppUserIdAsync(userIds, checkFrom);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetNewByListOfAppUserIdAsync(It.IsAny<string>(), It.IsAny<DateTimeOffset>()), Times.Once);
    }

    [Fact]
    public async Task CountAsync_Count_ShouldReturnCountOfCollection()
    {
        // Arrange
        const int count = 3;
        const string userId = "uid-1";

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.CountByAppUserIdAsync(userId)).ReturnsAsync(count);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.CountByAppUserIdAsync(userId);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountByAppUserIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CountByListOfCommunityIdAsync_Count_ShouldReturnCountOfCollection()
    {
        // Arrange
        const int count = 3;
        string[] userIds = ["uid-1", "uid-2"];

        var entitiesDto = UserPostTestDataFactory.CreateDtoCollection();
        var entities = UserPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IUserPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockUserPostLikeService = new Mock<IService<UserPostLikeDto, int>>();
        var mockUserPostDislikeService = new Mock<IService<UserPostDislikeDto, int>>();
        var mockUserPostCommentService = new Mock<IService<UserPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<UserPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.CountByListOfAppUserIdAsync(userIds)).ReturnsAsync(count);

        var service = new UserPostService(mockRepository.Object, mockMapper.Object, mockUserPostLikeService.Object,
            mockUserPostDislikeService.Object, mockUserPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.CountByListOfAppUserIdAsync(userIds);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountByListOfAppUserIdAsync(It.IsAny<string[]>()), Times.Once);
    }
}
