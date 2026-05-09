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

public class CommunityPostServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = CommunityPostTestDataFactory.CreateDto();
        var entity = CommunityPostTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<CommunityPost>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CommunityPostDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object, 
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.CommunityName, result.CommunityName);
        Assert.Equal(entityDto.Owner, result.Owner);
        Assert.Equal(entityDto.Content, result.Content);
        Assert.Equal(entityDto.PostType, result.PostType);
        Assert.Equal(entityDto.PublicType, result.PublicType);
        Assert.Equal(entityDto.Restrictions, result.Restrictions);
        Assert.Equal(entityDto.Tags, result.Tags);
        Assert.Equal(entityDto.CreatedAt, result.CreatedAt);
        Assert.Equal(entityDto.LikeCount, result.LikeCount);
        Assert.Equal(entityDto.DislikeCount, result.DislikeCount);
        Assert.Equal(entityDto.CommentCount, result.CommentCount);
        Assert.Equal(entityDto.CommunityId, result.CommunityId);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityPost>(It.IsAny<CommunityPostDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityPost>()), Times.Once);
        mockMapper.Verify(m => m.Map<CommunityPostDto>(It.IsAny<CommunityPost>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCommunityPostNameEmpty()
    {
        // Arrange
        const string name = "";

        var entityDto = CommunityPostTestDataFactory.CreateDto(name: name);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CommunityPostDto.CommunityName), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityPost>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = CommunityPostTestDataFactory.CreateDto();
        var entity = CommunityPostTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<CommunityPost>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityPost>(It.IsAny<CommunityPostDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPost>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = CommunityPostTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPost>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityPostNameEmpty()
    {
        // Arrange
        const int id = 1;
        const string name = "";

        var entityDto = CommunityPostTestDataFactory.CreateDto(name: name);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CommunityPostDto.CommunityName), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunityPost>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var transactionMock = new Mock<IDbContextTransaction>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockSQLRepository.Setup(s => s.UseTransactionAsync()).ReturnsAsync(transactionMock.Object);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

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
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

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
        var customers = new List<CommunityPost>();
        var customersDto = new List<CommunityPostDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

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

        var entityDto = CommunityPostTestDataFactory.CreateDto();
        var entity = CommunityPostTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

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

        var entityDto = CommunityPostTestDataFactory.CreateDto();
        var entity = CommunityPostTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<CommunityPostDto>(entity)).Returns(entityDto);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

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
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const string name = "name";

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        Expression<Func<CommunityPost, string>> expression = c => c.CommunityName;
        Expression<Func<CommunityPostDto, string>> expressionDto = c => c.CommunityName;

        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPost, string>>>(), name))
            .ReturnsAsync(entities);

        var service = new CommunityPostService(mockRepository.Object, mapper, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityName, name);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPost, string>>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const string name = "d name";

        var entities = new List<CommunityPost>();
        var entitiesDto = new List<CommunityPostDto>();

        Expression<Func<CommunityPost, string>> expression = c => c.CommunityName;
        Expression<Func<CommunityPostDto, string>> expressionDto = c => c.CommunityName;

        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPost, string>>>(), name))
            .ReturnsAsync(entities);

        var service = new CommunityPostService(mockRepository.Object, mapper, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityName, name);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityPost, string>>>(), It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetByCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const int communityId = 1;

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetByCommunityIdAsync(communityId, pageSize)).ReturnsAsync(entities);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetByCommunityIdAsync(communityId, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByCommunityIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetMoreByCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const int offset = 0;
        const int communityId = 1;

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetMoreByCommunityIdAsync(communityId, offset, pageSize)).ReturnsAsync([]);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetMoreByCommunityIdAsync(communityId, offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetMoreByCommunityIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetNewByCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int communityId = 1;
        DateTimeOffset checkFrom = DateTimeOffset.Parse("11/23/2025");

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetNewByCommunityIdAsync(communityId, checkFrom)).ReturnsAsync(entities);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetNewByCommunityIdAsync(communityId, checkFrom);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetNewByCommunityIdAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset>()), Times.Once);
    }

    [Fact]
    public async Task GetByListOfCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const string communityIds = "1,2";

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetByListOfCommunityIdAsync(communityIds, pageSize)).ReturnsAsync(entities);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetByListOfCommunityIdAsync(communityIds, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByListOfCommunityIdAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetMoreByListOfCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const int offset = 0;
        const string communityIds = "1,2";

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetMoreByListOfCommunityIdAsync(communityIds, offset, pageSize)).ReturnsAsync(entities);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetMoreByListOfCommunityIdAsync(communityIds, offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetMoreByListOfCommunityIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetNewByListOfCommunityIdAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const string communityIds = "1,2";
        DateTimeOffset checkFrom = DateTimeOffset.Parse("11/23/2025");

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetNewByListOfCommunityIdAsync(communityIds, checkFrom)).ReturnsAsync(entities);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.GetNewByListOfCommunityIdAsync(communityIds, checkFrom);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetNewByListOfCommunityIdAsync(It.IsAny<string>(), It.IsAny<DateTimeOffset>()), Times.Once);
    }

    [Fact]
    public async Task CountAsync_Count_ShouldReturnCountOfCollection()
    {
        // Arrange
        const int count = 3;
        const int communityId = 1;

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.CountByCommunityIdAsync(communityId)).ReturnsAsync(count);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.CountByCommunityIdAsync(communityId);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountByCommunityIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CountByListOfCommunityIdAsync_Count_ShouldReturnCountOfCollection()
    {
        // Arrange
        const int count = 3;
        int[] communityId = [1, 2];

        var entitiesDto = CommunityPostTestDataFactory.CreateDtoCollection();
        var entities = CommunityPostTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityPostRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityPostDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.CountByListOfCommunityIdAsync(communityId)).ReturnsAsync(count);

        var service = new CommunityPostService(mockRepository.Object, mockMapper.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityPostCommentService.Object, mockSQLRepository.Object);

        // Act
        var result = await service.CountByListOfCommunityIdAsync(communityId);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountByListOfCommunityIdAsync(It.IsAny<int[]>()), Times.Once);
    }
}
