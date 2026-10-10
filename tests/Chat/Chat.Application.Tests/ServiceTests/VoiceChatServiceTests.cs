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

//public class VoiceChatServiceTests
//{
//    [Fact]
//    public async Task CreateAsync_VoiceChat_ShouldCreateEntityAndReturnCreatedEntity()
//    {
//        // Arrange
//        var voiceChatDto = VoiceChatTestData.CreateDto();
//        var voiceChat = VoiceChatTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGenericRepository<VoiceChat, VoiceChatId>>();

//        mockMapper.Setup(m => m.Map<VoiceChatDto>(It.IsAny<VoiceChat>())).Returns(voiceChatDto);

//        mockRepository.Setup(m => m.CreateAsync(It.IsAny<VoiceChat>())).ReturnsAsync(new VoiceChat(voiceChat.Id, voiceChat.AppUserId));

//        var service = new VoiceChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.CreateAsync(voiceChatDto);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(voiceChatDto.Id, result.Id);
//        Assert.Equal(voiceChatDto.AppUserId, result.AppUserId);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.CreateAsync(It.IsAny<VoiceChat>()), Times.Once);
//        mockMapper.Verify(m => m.Map<VoiceChatDto>(It.IsAny<VoiceChat>()), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteAsync_ShouldRemoveEntity()
//    {
//        // Arrange
//        const string chatId = "uid-1";

//        var voiceChatsDto = VoiceChatTestData.CreateDtoCollection();
//        var voiceChats = VoiceChatTestData.CreateCollection();

//        var mockRepository = new Mock<IGenericRepository<VoiceChat, VoiceChatId>>();

//        var service = new VoiceChatService(mockRepository.Object, null);

//        // Act
//        await service.DeleteAsync(chatId);

//        // Assert and Verify correct method calls
//        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<VoiceChatId>()), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteAsync_ThrowEntityNotFoundException_ShouldNotRemoveEntityAsEntityDoesNotExist()
//    {
//        // Arrange
//        const string chatId = "uid-33";

//        var mockRepository = new Mock<IGenericRepository<VoiceChat, VoiceChatId>>();

//        mockRepository.Setup(r => r.DeleteAsync(It.IsAny<VoiceChatId>())).Throws(new EntityNotFoundException(typeof(VoiceChat), chatId));

//        var service = new VoiceChatService(mockRepository.Object, null);

//        // Act and Assert
//        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.DeleteAsync(chatId));

//        // Verify correct method calls
//        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<VoiceChatId>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetAllAsync_Collection_ShouldReturnAllEntity()
//    {
//        // Arrange
//        var voiceChatsDto = VoiceChatTestData.CreateDtoCollection();
//        var voiceChats = VoiceChatTestData.CreateCollection();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGenericRepository<VoiceChat, VoiceChatId>>();

//        mockMapper.Setup(m => m.Map<IEnumerable<VoiceChatDto>>(It.IsAny<IEnumerable<VoiceChat>>())).Returns(voiceChatsDto);

//        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(voiceChats);

//        var service = new VoiceChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetAllAsync();

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
//        mockMapper.Verify(m => m.Map<IEnumerable<VoiceChatDto>>(It.IsAny<IEnumerable<VoiceChat>>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetByIdAsync_VoiceChat_ShouldReturnCorrectEntity()
//    {
//        // Arrange
//        const string chatId = "uid-1";

//        var voiceChatDto = VoiceChatTestData.CreateDto();
//        var voiceChat = VoiceChatTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGenericRepository<VoiceChat, VoiceChatId>>();

//        mockMapper.Setup(m => m.Map<VoiceChatDto>(It.IsAny<VoiceChat>())).Returns(voiceChatDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<VoiceChatId>())).ReturnsAsync(voiceChat);

//        var service = new VoiceChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetByIdAsync(chatId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(voiceChatDto.Id, result.Id);
//        Assert.Equal(voiceChatDto.AppUserId, result.AppUserId);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<VoiceChatId>()), Times.Once);
//        mockMapper.Verify(m => m.Map<VoiceChatDto>(It.IsAny<VoiceChat>()), Times.Once);
//    }
//}
