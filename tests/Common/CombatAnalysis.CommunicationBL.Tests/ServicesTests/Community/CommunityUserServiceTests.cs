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

public class CommunityUserServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = CommunityUserTestDataFactory.CreateDto();
        var entity = CommunityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        mockMapper.Setup(m => m.Map<CommunityUser>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CommunityUserDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.CommunityId, result.CommunityId);
        Assert.Equal(entityDto.Username, result.Username);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);
        Assert.Equal(entityDto.CommunityId, result.CommunityId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityUser>(It.IsAny<CommunityUserDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityUser>()), Times.Once);
        mockMapper.Verify(m => m.Map<CommunityUserDto>(It.IsAny<CommunityUser>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsCountryEmpty()
    {
        // Arrange
        const int communityId = 0;

        var entityDto = CommunityUserTestDataFactory.CreateDto(communityId: communityId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityUserDto.CommunityId), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CommunityUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const string id = "uid-1";

        var entityDto = CommunityUserTestDataFactory.CreateDto();
        var entity = CommunityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        mockMapper.Setup(m => m.Map<CommunityUser>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, entity));

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CommunityUser>(It.IsAny<CommunityUserDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<CommunityUser>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdEmpty()
    {
        // Arrange
        const string id = "";

        var entityDto = CommunityUserTestDataFactory.CreateDto(id: id);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<CommunityUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsCommunityIdIncorrect()
    {
        // Arrange
        const string id = "uid-1";
        const int communityId = 0;

        var entityDto = CommunityUserTestDataFactory.CreateDto(communityId: communityId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(CommunityUserDto.CommunityId), () => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<CommunityUser>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const string id = "uid-1";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(id);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const string id = "";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var entitiesDto = CommunityUserTestDataFactory.CreateDtoCollection();
        var entities = CommunityUserTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityUserDto>>(entities)).Returns(entitiesDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(entities);

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

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
        var customers = new List<CommunityUser>();
        var customersDto = new List<CommunityUserDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CommunityUserDto>>(customers)).Returns(customersDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(customers);

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

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
        const string id = "uid-1";

        var entityDto = CommunityUserTestDataFactory.CreateDto();
        var entity = CommunityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        mockMapper.Setup(m => m.Map<CommunityUserDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const string id = "uid-1324";

        var entityDto = CommunityUserTestDataFactory.CreateDto();
        var entity = CommunityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        mockMapper.Setup(m => m.Map<CommunityUserDto>(entity)).Returns(entityDto);

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowExecption_ShouldNotReturnEntity()
    {
        // Arrange
        const string id = "";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        var service = new CommunityUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetByIdAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int communitId = 1;

        var entitiesDto = CommunityUserTestDataFactory.CreateDtoCollection();
        var entities = CommunityUserTestDataFactory.CreateCollection();

        Expression<Func<CommunityUser, int>> expression = c => c.CommunityId;
        Expression<Func<CommunityUserDto, int>> expressionDto = c => c.CommunityId;

        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityUser, int>>>(), communitId))
            .ReturnsAsync(entities);

        var service = new CommunityUserService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityId, communitId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityUser, int>>>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int communitId = 23;

        var customerUsers = new List<CommunityUser>();
        var customerUsersDto = new List<CommunityUserDto>();

        Expression<Func<CommunityUser, int>> expression = c => c.CommunityId;
        Expression<Func<CommunityUserDto, int>> expressionDto = c => c.CommunityId;

        var mockRepository = new Mock<IGenericRepository<CommunityUser, string>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityUser, int>>>(), communitId))
            .ReturnsAsync(customerUsers);

        var service = new CommunityUserService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(c => c.CommunityId, communitId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<CommunityUser, int>>>(), It.IsAny<int>()), Times.Once);
    }
}
