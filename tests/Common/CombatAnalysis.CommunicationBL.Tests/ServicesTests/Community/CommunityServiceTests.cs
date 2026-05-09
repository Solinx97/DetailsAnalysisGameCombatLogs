using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationBL.Mapping;
using CombatAnalysis.CommunicationBL.Services.Community;
using CombatAnalysis.CommunicationBL.Tests.Factory;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Tests.ServicesTests.Community;

public class CommunityServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = CommunityTestDataFactory.CreateDto();
        var entity = CommunityTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<CommunicationDAL.Entities.Community.Community>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CommunityDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object, 
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Name, result.Name);
        Assert.Equal(entityDto.Description, result.Description);
        Assert.Equal(entityDto.PolicyType, result.PolicyType);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunicationDAL.Entities.Community.Community>(It.IsAny<CommunityDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunicationDAL.Entities.Community.Community>()), Times.Once);
        mockMapper.Verify(m => m.Map<CommunityDto>(It.IsAny<CommunicationDAL.Entities.Community.Community>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCommunityNameEmpty()
    {
        // Arrange
        const string name = "";

        var entityDto = CommunityTestDataFactory.CreateDto(name: name);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CommunityDto.Name), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunicationDAL.Entities.Community.Community>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int id = 1;

        var entityDto = CommunityTestDataFactory.CreateDto();
        var entity = CommunityTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<CommunicationDAL.Entities.Community.Community>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunicationDAL.Entities.Community.Community>(It.IsAny<CommunityDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunicationDAL.Entities.Community.Community>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIncorrect()
    {
        // Arrange
        const int id = 0;

        var entityDto = CommunityTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunicationDAL.Entities.Community.Community>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityNameEmpty()
    {
        // Arrange
        const int id = 1;
        const string name = "";

        var entityDto = CommunityTestDataFactory.CreateDto(name: name);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CommunityDto.Name), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<CommunicationDAL.Entities.Community.Community>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var transactionMock = new Mock<IDbContextTransaction>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockSQLRepository.Setup(s => s.BeginTransactionAsync(It.IsAny<bool>())).ReturnsAsync(transactionMock.Object);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(id);

        // Verify correct method calls
        mockSQLRepository.Verify(r => r.BeginTransactionAsync(It.IsAny<bool>()), Times.Once);
        transactionMock.Verify(r => r.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int id = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = CommunityTestDataFactory.CreateDtoCollection();
        var entities = CommunityTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

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
        var customers = new List<CommunicationDAL.Entities.Community.Community>();
        var customersDto = new List<CommunityDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

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

        var entityDto = CommunityTestDataFactory.CreateDto();
        var entity = CommunityTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<CommunityDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

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

        var entityDto = CommunityTestDataFactory.CreateDto();
        var entity = CommunityTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<CommunityDto>(entity)).Returns(entityDto);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

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
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

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

        var entitiesDto = CommunityTestDataFactory.CreateDtoCollection();
        var entities = CommunityTestDataFactory.CreateCollection();

        Expression<Func<CommunicationDAL.Entities.Community.Community, string>> expression = c => c.Name;
        Expression<Func<CommunityDto, string>> expressionDto = c => c.Name;

        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunicationDAL.Entities.Community.Community, string>>>(), name))
            .ReturnsAsync(entities);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.Name, name);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunicationDAL.Entities.Community.Community, string>>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const string name = "d name";

        var entities = new List<CommunicationDAL.Entities.Community.Community>();
        var entitiesDto = new List<CommunityDto>();

        Expression<Func<CommunicationDAL.Entities.Community.Community, string>> expression = c => c.Name;
        Expression<Func<CommunityDto, string>> expressionDto = c => c.Name;

        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunicationDAL.Entities.Community.Community, string>>>(), name))
            .ReturnsAsync(entities);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.Name, name);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunicationDAL.Entities.Community.Community, string>>>(), It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetAllWithPaginationAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;

        var entitiesDto = CommunityTestDataFactory.CreateDtoCollection();
        var entities = CommunityTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllWithPaginationAsync(pageSize)).ReturnsAsync(entities);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllWithPaginationAsync(pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllWithPaginationAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetAllWithPaginationAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int pageSize = 0;

        var entitiesDto = CommunityTestDataFactory.CreateDtoCollection();
        var entities = CommunityTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllWithPaginationAsync(pageSize)).ReturnsAsync([]);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllWithPaginationAsync(pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllWithPaginationAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetMoreWithPaginationAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int pageSize = 5;
        const int offset = 0;

        var entitiesDto = CommunityTestDataFactory.CreateDtoCollection();
        var entities = CommunityTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetMoreWithPaginationAsync(offset, pageSize)).ReturnsAsync(entities);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act
        var result = await service.GetMoreWithPaginationAsync(offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetMoreWithPaginationAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CountAsync_Count_ShouldReturnCountOfCollection()
    {
        // Arrange
        const int count = 3;

        var entitiesDto = CommunityTestDataFactory.CreateDtoCollection();
        var entities = CommunityTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICommunityRepository>();
        var mockSQLRepository = new Mock<ISqlContextService>();
        var mockInviteService = new Mock<IService<InviteToCommunityDto, int>>();
        var mockCommunityUserService = new Mock<IService<CommunityUserDto, string>>();
        var mockCommunityPostService = new Mock<ICommunityPostService>();
        var mockCommunityPostCommentService = new Mock<IService<CommunityPostCommentDto, int>>();
        var mockCommunityPostLikeService = new Mock<IService<CommunityPostLikeDto, int>>();
        var mockCommunityPostDislikeService = new Mock<IService<CommunityPostDislikeDto, int>>();
        var mockCommunityDiscissuionService = new Mock<IService<CommunityDiscussionDto, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.CountAsync()).ReturnsAsync(count);

        var service = new CommunityService(mockRepository.Object, mockSQLRepository.Object, mockInviteService.Object,
            mockCommunityUserService.Object, mockCommunityPostService.Object, mockCommunityPostCommentService.Object, mockCommunityPostLikeService.Object,
            mockCommunityPostDislikeService.Object, mockCommunityDiscissuionService.Object, mockMapper.Object);

        // Act
        var result = await service.CountAsync();

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountAsync(), Times.Once);
    }
}
