//using AutoMapper;
//using Chat.Application.DTOs;
//using Chat.Application.Services;
//using Chat.Application.Tests.Factory;
//using Chat.Domain.Aggregates;
//using Chat.Domain.Entities;
//using Chat.Domain.Repositories;
//using Chat.Domain.ValueObjects;
//using Chat.Infrastructure.Exceptions;
//using Moq;

//namespace Chat.Application.Tests.ServiceTests;

//public class GroupChatServiceTests
//{
//    [Fact]
//    public async Task CreateAsync_GroupChat_ShouldCreateEntityAndReturnCreatedEntity()
//    {
//        // Arrange
//        var groupChatDto = GroupChatTestData.CreateDto();
//        var groupChat = GroupChatTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGroupChatRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatDto>(It.IsAny<GroupChat>())).Returns(groupChatDto);

//        mockRepository.Setup(m => m.CreateAsync(It.IsAny<GroupChat>())).ReturnsAsync(new GroupChat(groupChat.Name, groupChat.OwnerId));

//        var service = new GroupChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.CreateAsync(groupChatDto);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(groupChatDto.Id, result.Id);
//        Assert.Equal(groupChatDto.Name, result.Name);
//        Assert.Equal(groupChatDto.OwnerId, result.OwnerId);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.CreateAsync(It.IsAny<GroupChat>()), Times.Once);
//        mockMapper.Verify(m => m.Map<GroupChatDto>(It.IsAny<GroupChat>()), Times.Once);
//    }

//    [Fact]
//    public async Task UpdateChatAsync_ShouldUpdateChatNameProperty()
//    {
//        // Arrange
//        GroupChatId id = 1;
//        const string newName = "updated name";
//        UserId ownerId = "uid-1";

//        var groupChatDto = GroupChatTestData.CreateDto(ownerId: ownerId);
//        var groupChat = GroupChatTestData.Create(ownerId: ownerId);

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGroupChatRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatDto>(It.IsAny<GroupChat>())).Returns(groupChatDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatId>())).ReturnsAsync(groupChat);
//        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

//        var service = new GroupChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        await service.UpdateChatAsync(id, newName, null);

//        // Assert
//        Assert.Equal(newName, groupChat.Name);
//        Assert.Equal(ownerId, groupChat.OwnerId);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task UpdateChatAsync_ShouldUpdateChatOwnerIdProperty()
//    {
//        // Arrange
//        GroupChatId id = 1;
//        const string name = "original name";
//        const string ownerId = "uid-43";

//        var groupChatDto = GroupChatTestData.CreateDto(name: name);
//        var groupChat = GroupChatTestData.Create(name: name);

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGroupChatRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatDto>(It.IsAny<GroupChat>())).Returns(groupChatDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatId>())).ReturnsAsync(groupChat);
//        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

//        var service = new GroupChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        await service.UpdateChatAsync(id, null, ownerId);

//        // Assert
//        Assert.Equal(name, groupChat.Name);
//        Assert.Equal(ownerId, groupChat.OwnerId);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteAsync_ShouldRemoveEntity()
//    {
//        // Arrange
//        const int chatId = 1;

//        var groupChatsDto = GroupChatTestData.CreateDtoCollection();
//        var groupChats = GroupChatTestData.CreateCollection();

//        var mockRepository = new Mock<IGroupChatRepository>();

//        var service = new GroupChatService(mockRepository.Object, null);

//        // Act
//        await service.DeleteAsync(chatId);

//        // Assert and Verify correct method calls
//        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<GroupChatId>()), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteAsync_ThrowEntityNotFoundException_ShouldNotRemoveEntityAsEntityDoesNotExist()
//    {
//        // Arrange
//        const int chatId = 0;

//        var mockRepository = new Mock<IGroupChatRepository>();

//        mockRepository.Setup(r => r.DeleteAsync(It.IsAny<GroupChatId>())).Throws(new EntityNotFoundException(typeof(GroupChat), chatId));

//        var service = new GroupChatService(mockRepository.Object, null);

//        // Act and Assert
//        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.DeleteAsync(chatId));

//        // Verify correct method calls
//        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<GroupChatId>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetAllAsync_Collection_ShouldReturnAllEntity()
//    {
//        // Arrange
//        var groupChatsDto = GroupChatTestData.CreateDtoCollection();
//        var groupChats = GroupChatTestData.CreateCollection();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGroupChatRepository>();

//        mockMapper.Setup(m => m.Map<IEnumerable<GroupChatDto>>(It.IsAny<IEnumerable<GroupChat>>())).Returns(groupChatsDto);

//        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(groupChats);

//        var service = new GroupChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetAllAsync();

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
//        mockMapper.Verify(m => m.Map<IEnumerable<GroupChatDto>>(It.IsAny<IEnumerable<GroupChat>>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetByIdAsync_GroupChat_ShouldReturnCorrectEntity()
//    {
//        // Arrange
//        const int chatId = 1;

//        var groupChatDto = GroupChatTestData.CreateDto();
//        var groupChat = GroupChatTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGroupChatRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatDto>(It.IsAny<GroupChat>())).Returns(groupChatDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatId>())).ReturnsAsync(groupChat);

//        var service = new GroupChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetByIdAsync(chatId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(groupChatDto.Id, result.Id);
//        Assert.Equal(groupChatDto.Name, result.Name);
//        Assert.Equal(groupChatDto.OwnerId, result.OwnerId);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatId>()), Times.Once);
//        mockMapper.Verify(m => m.Map<GroupChatDto>(It.IsAny<GroupChat>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetByUserIdAsync_Collection_ShouldReturnCollectionByUserId()
//    {
//        // Arrange
//        const int chatId = 1;

//        var groupChatRulesDto = GroupChatRulesTestData.CreateDto();
//        var groupChat = GroupChatTestData.Create(addRules: true);

//        var mockMapper = new Mock<IMapper>();
//        var mockRepository = new Mock<IGroupChatRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatRulesDto>(It.IsAny<GroupChatRules>())).Returns(groupChatRulesDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatId>())).ReturnsAsync(groupChat);

//        var service = new GroupChatService(mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetRulesAsync(chatId);

//        // Assert
//        Assert.NotNull(result);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatId>()), Times.Once);
//        mockMapper.Verify(m => m.Map<GroupChatRulesDto>(It.IsAny<GroupChatRules>()), Times.Once);
//    }
//}
