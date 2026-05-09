using AutoMapper;
using CombatAnalysis.NotificationBL.DTO;
using CombatAnalysis.NotificationBL.Mapping;
using CombatAnalysis.NotificationBL.Services;
using CombatAnalysis.NotificationBL.Tests.Factory;
using CombatAnalysis.NotificationDAL.Entities;
using CombatAnalysis.NotificationDAL.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace CombatAnalysis.NotificationBL.Tests.ServicesTests;

public class NotificationServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var notificationDto = NotificationTestDataFactory.CreateDto();
        var notification = NotificationTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        mockMapper.Setup(m => m.Map<Notification>(notificationDto)).Returns(notification);
        mockMapper.Setup(m => m.Map<NotificationDto>(notification)).Returns(notificationDto);

        mockRepository.Setup(m => m.CreateAsync(notification)).ReturnsAsync(notification);

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(notificationDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(notificationDto.Id, result.Id);
        Assert.Equal(notificationDto.Type, result.Type);
        Assert.Equal(notificationDto.Status, result.Status);
        Assert.Equal(notificationDto.InitiatorId, result.InitiatorId);
        Assert.Equal(notificationDto.InitiatorName, result.InitiatorName);
        Assert.Equal(notificationDto.RecipientId, result.RecipientId);
        Assert.Equal(notificationDto.CreatedAt, result.CreatedAt);
        Assert.Equal(notificationDto.ReadAt, result.ReadAt);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<Notification>(It.IsAny<NotificationDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Notification>()), Times.Once);
        mockMapper.Verify(m => m.Map<NotificationDto>(It.IsAny<Notification>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsInitiatorIdEmpty()
    {
        // Arrange
        var notificationDto = NotificationTestDataFactory.CreateDto(initiatorId: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(NotificationDto.InitiatorId), () => service.CreateAsync(notificationDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Notification>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const int notificationId = 1;

        var notificationDto = NotificationTestDataFactory.CreateDto(id: notificationId);
        var notification = NotificationTestDataFactory.Create(id: notificationId);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        mockMapper.Setup(m => m.Map<Notification>(notificationDto)).Returns(notification);

        mockRepository.Setup(m => m.UpdateAsync(notificationId, notification));

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(notificationId, notificationDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<Notification>(It.IsAny<NotificationDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Notification>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowArgumentOutOfRangeException_ShouldNotUpdateEntityAsIdEmpty()
    {
        // Arrange
        const int notificationId = 0;

        var notificationDto = NotificationTestDataFactory.CreateDto();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(notificationId, notificationDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<Notification>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int notififcationId = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(notififcationId);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int notificationId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(notificationId));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        var notificationsDto = new List<NotificationDto> {
            NotificationTestDataFactory.CreateDto()
        };
        var notifications = new List<Notification> {
            NotificationTestDataFactory.Create()
        };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<NotificationDto>>(notifications)).Returns(notificationsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(notifications);

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        var notificationsDto = new List<NotificationDto>();
        var notifications = new List<Notification>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        mockMapper.Setup(m => m.Map<IEnumerable<NotificationDto>>(notifications)).Returns(notificationsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(notifications);

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

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
        const int notificationId = 1;

        var notificationDto = NotificationTestDataFactory.CreateDto();
        var notification = NotificationTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        mockMapper.Setup(m => m.Map<NotificationDto>(notification)).Returns(notificationDto);

        mockRepository.Setup(m => m.GetByIdAsync(notificationId)).ReturnsAsync(notification);

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(notificationId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(notificationId, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const int notificationId = 1;

        var notificationDto = NotificationTestDataFactory.CreateDto();
        var notification = NotificationTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        mockMapper.Setup(m => m.Map<NotificationDto>(notification)).Returns(notificationDto);

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(notificationId);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowExecption_ShouldNotReturnEntity()
    {
        // Arrange
        const int notificationId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        var service = new NotificationService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(notificationId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_NotEmptyCollection_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const string recipientId = "uid-23";

        var notifications = new List<Notification> { NotificationTestDataFactory.Create() };
        var notificationsDtp = new List<NotificationDto> { NotificationTestDataFactory.CreateDto() };

        Expression<Func<Notification, string>> expression = c => c.RecipientId;
        Expression<Func<NotificationDto, string>> expressionDto = c => c.RecipientId;

        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<NotificationBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<Notification, string>>>(), recipientId))
            .ReturnsAsync(notifications);

        var service = new NotificationService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(n => n.RecipientId, recipientId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<Notification, string>>>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const string initiatorId = "uid-232";

        var notifications = new List<Notification>();
        var notificationsDtp = new List<NotificationDto>();

        Expression<Func<Notification, string>> expression = c => c.InitiatorId;
        Expression<Func<NotificationDto, string>> expressionDto = c => c.InitiatorId;

        var mockRepository = new Mock<IGenericRepository<Notification, int>>();

        // Use real Automapper as method call MapperExpresiion extension
        var loggerFactory = LoggerFactory.Create(builder => { });
        var config = new MapperConfiguration(cfg => cfg.AddProfile<NotificationBLMapper>(), loggerFactory);
        var mapper = config.CreateMapper();

        mockRepository
            .Setup(r => r.GetByParamAsync(It.IsAny<Expression<Func<Notification, string>>>(), initiatorId))
            .ReturnsAsync(notifications);

        var service = new NotificationService(mockRepository.Object, mapper);

        // Act
        var result = await service.GetByParamAsync(n => n.InitiatorId, initiatorId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<Expression<Func<Notification, string>>>(), It.IsAny<string>()), Times.Once);
    }
}
