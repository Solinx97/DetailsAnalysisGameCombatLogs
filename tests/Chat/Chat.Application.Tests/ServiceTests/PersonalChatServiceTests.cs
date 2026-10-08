//using AutoMapper;
//using Chat.Application.DTOs;
//using Chat.Application.Services;
//using Chat.Application.Tests.Factory;
//using Chat.Domain.Aggregates;
//using Chat.Domain.Repositories;
//using Chat.Domain.ValueObjects;
//using Chat.Infrastructure.Exceptions;
//using Moq;

//namespace Chat.Application.Tests.ServiceTests;

//public class PersonalChatServiceTests
//{
//    [Fact]
//    public async Task CreateAsync_PersonalChat_ShouldCreateEntityAndReturnCreatedEntity()
//    {
//        // Arrange
//        var personalChatDto = PersonalChatTestData.CreateDto();
//        var personalChat = PersonalChatTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IPersonalChatRepository>();

//        mockMapper.Setup(m => m.Map<PersonalChatDto>(It.IsAny<PersonalChat>())).Returns(personalChatDto);

//        mockRepository.Setup(m => m.CreateAsync(It.IsAny<PersonalChat>())).ReturnsAsync(new PersonalChat(personalChat.InitiatorId, personalChat.CompanionId));

//        var service = new PersonalChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.CreateAsync(personalChatDto);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(personalChatDto.Id, result.Id);
//        Assert.Equal(personalChatDto.InitiatorId, result.InitiatorId);
//        Assert.Equal(personalChatDto.CompanionId, result.CompanionId);
//        Assert.Equal(personalChatDto.InitiatorId, result.InitiatorId);
//        Assert.Equal(personalChatDto.InitiatorUnreadMessages, result.InitiatorUnreadMessages);
//        Assert.Equal(personalChatDto.CompanionUnreadMessages, result.CompanionUnreadMessages);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.CreateAsync(It.IsAny<PersonalChat>()), Times.Once);
//        mockMapper.Verify(m => m.Map<PersonalChatDto>(It.IsAny<PersonalChat>()), Times.Once);
//    }

//    [Fact]
//    public async Task UpdateChatAsync_ShouldUpdateChatInitiatorUnreadMessagesProperty()
//    {
//        // Arrange
//        PersonalChatId id = 1;
//        const int initiatorUnreadMessages = 4;
//        const int companionUnreadMessages = 0;

//        var personalChatDto = PersonalChatTestData.CreateDto(companionUnreadMessages: companionUnreadMessages);
//        var personalChat = PersonalChatTestData.Create(companionUnreadMessages: companionUnreadMessages);

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IPersonalChatRepository>();

//        mockMapper.Setup(m => m.Map<PersonalChatDto>(It.IsAny<PersonalChat>())).Returns(personalChatDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<PersonalChatId>())).ReturnsAsync(personalChat);
//        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

//        var service = new PersonalChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        await service.UpdateChatAsync(id, initiatorUnreadMessages, null);

//        // Assert
//        Assert.Equal(initiatorUnreadMessages, personalChat.InitiatorUnreadMessages);
//        Assert.Equal(companionUnreadMessages, personalChat.CompanionUnreadMessages);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task UpdateChatAsync_ShouldUpdateChatCompanionUnreadMessagesProperty()
//    {
//        // Arrange
//        PersonalChatId id = 1;
//        const int companionUnreadMessages = 4;
//        const int initiatorUnreadMessages = 0;

//        var personalChatDto = PersonalChatTestData.CreateDto(initiatorUnreadMessages: initiatorUnreadMessages);
//        var personalChat = PersonalChatTestData.Create(initiatorUnreadMessages: initiatorUnreadMessages);

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IPersonalChatRepository>();

//        mockMapper.Setup(m => m.Map<PersonalChatDto>(It.IsAny<PersonalChat>())).Returns(personalChatDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<PersonalChatId>())).ReturnsAsync(personalChat);
//        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

//        var service = new PersonalChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        await service.UpdateChatAsync(id, null, companionUnreadMessages);

//        // Assert
//        Assert.Equal(companionUnreadMessages, personalChat.CompanionUnreadMessages);
//        Assert.Equal(initiatorUnreadMessages, personalChat.InitiatorUnreadMessages);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteAsync_ShouldRemoveEntity()
//    {
//        // Arrange
//        const int chatId = 1;

//        var personalChatsDto = PersonalChatTestData.CreateDtoCollection();
//        var personalChats = PersonalChatTestData.CreateCollection();

//        var mockRepository = new Mock<IPersonalChatRepository>();

//        var service = new PersonalChatService(mockRepository.Object, null);

//        // Act
//        await service.DeleteAsync(chatId);

//        // Assert and Verify correct method calls
//        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<PersonalChatId>()), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteAsync_ThrowEntityNotFoundException_ShouldNotRemoveEntityAsEntityDoesNotExist()
//    {
//        // Arrange
//        const int chatId = 0;

//        var mockRepository = new Mock<IPersonalChatRepository>();

//        mockRepository.Setup(r => r.DeleteAsync(It.IsAny<PersonalChatId>())).Throws(new EntityNotFoundException(typeof(PersonalChat), chatId));

//        var service = new PersonalChatService(mockRepository.Object, null);

//        // Act and Assert
//        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.DeleteAsync(chatId));

//        // Verify correct method calls
//        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<PersonalChatId>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetAllAsync_Collection_ShouldReturnAllEntity()
//    {
//        // Arrange
//        var personalChatsDto = PersonalChatTestData.CreateDtoCollection();
//        var personalChats = PersonalChatTestData.CreateCollection();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IPersonalChatRepository>();

//        mockMapper.Setup(m => m.Map<IEnumerable<PersonalChatDto>>(It.IsAny<IEnumerable<PersonalChat>>())).Returns(personalChatsDto);

//        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(personalChats);

//        var service = new PersonalChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetAllAsync();

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
//        mockMapper.Verify(m => m.Map<IEnumerable<PersonalChatDto>>(It.IsAny<IEnumerable<PersonalChat>>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetByIdAsync_PersonalChat_ShouldReturnCorrectEntity()
//    {
//        // Arrange
//        const int chatId = 1;

//        var personalChatDto = PersonalChatTestData.CreateDto();
//        var personalChat = PersonalChatTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IPersonalChatRepository>();

//        mockMapper.Setup(m => m.Map<PersonalChatDto>(It.IsAny<PersonalChat>())).Returns(personalChatDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<PersonalChatId>())).ReturnsAsync(personalChat);

//        var service = new PersonalChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetByIdAsync(chatId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(personalChatDto.Id, result.Id);
//        Assert.Equal(personalChatDto.InitiatorId, result.InitiatorId);
//        Assert.Equal(personalChatDto.CompanionId, result.CompanionId);
//        Assert.Equal(personalChatDto.InitiatorId, result.InitiatorId);
//        Assert.Equal(personalChatDto.InitiatorUnreadMessages, result.InitiatorUnreadMessages);
//        Assert.Equal(personalChatDto.CompanionUnreadMessages, result.CompanionUnreadMessages);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<PersonalChatId>()), Times.Once);
//        mockMapper.Verify(m => m.Map<PersonalChatDto>(It.IsAny<PersonalChat>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetByUserIdAsync_Collection_ShouldReturnCollectionByUserId()
//    {
//        // Arrange
//        const string userId = "uid-1";

//        var personalChatsDto = PersonalChatTestData.CreateDtoCollection();
//        var personalChats = PersonalChatTestData.CreateCollection();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IPersonalChatRepository>();

//        mockMapper.Setup(m => m.Map<IEnumerable<PersonalChatDto>>(It.IsAny<IEnumerable<PersonalChat>>())).Returns(personalChatsDto);

//        mockRepository.Setup(m => m.GetByUserIdAsync(It.IsAny<string>())).ReturnsAsync(personalChats);

//        var service = new PersonalChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetByUserIdAsync(userId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetByUserIdAsync(It.IsAny<string>()), Times.Once);
//        mockMapper.Verify(m => m.Map<IEnumerable<PersonalChatDto>>(It.IsAny<IEnumerable<PersonalChat>>()), Times.Once);
//    }
//}
