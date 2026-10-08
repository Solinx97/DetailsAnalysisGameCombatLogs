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

//public class GroupChatUserServiceTests
//{
//    [Fact]
//    public async Task CreateAsync_GroupChatUser_ShouldCreateEntityAndReturnCreatedEntity()
//    {
//        // Arrange
//        const int chatId = 1;

//        var groupChat = GroupChatTestData.Create(id: chatId);
//        var groupChatUserDto = GroupChatUserTestData.CreateDto();
//        var groupChatUser = GroupChatUserTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatUserDto>(It.IsAny<GroupChatUser>())).Returns(groupChatUserDto);

//        mockChatRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatId>())).ReturnsAsync(groupChat);
//        mockRepository.Setup(m => m.CreateAsync(It.IsAny<GroupChatUser>())).ReturnsAsync(new GroupChatUser(groupChatUser.Id, groupChatUser.Username, groupChatUser.GroupChatId, groupChatUser.AppUserId));

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.CreateAsync(groupChatUserDto);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(groupChatUserDto.Id, result.Id);
//        Assert.Equal(groupChatUserDto.Username, result.Username);
//        Assert.Equal(groupChatUserDto.UnreadMessages, result.UnreadMessages);
//        Assert.Equal(groupChatUserDto.LastReadMessageId, result.LastReadMessageId);
//        Assert.Equal(groupChatUserDto.GroupChatId, result.GroupChatId);
//        Assert.Equal(groupChatUserDto.AppUserId, result.AppUserId);

//        // Verify correct method calls
//        mockChatRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatId>()), Times.Once);
//        mockRepository.Verify(r => r.CreateAsync(It.IsAny<GroupChatUser>()), Times.Once);
//        mockMapper.Verify(m => m.Map<GroupChatUserDto>(It.IsAny<GroupChatUser>()), Times.Once);
//    }

//    [Fact]
//    public async Task UpdateChatAsync_ShouldUpdateChatUserLastMessageIdProperty()
//    {
//        // Arrange
//        GroupChatUserId id = "uid-1";
//        GroupChatMessageId lastReadMessageId = Guid.NewGuid();
//        const int unreadMessages = 0;

//        var groupChatUserDto = GroupChatUserTestData.CreateDto();
//        var groupChatUser = GroupChatUserTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatUserDto>(It.IsAny<GroupChatUser>())).Returns(groupChatUserDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatUserId>())).ReturnsAsync(groupChatUser);
//        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, mockMapper.Object);

//        // Act
//        await service.UpdateChatUserAsync(id, lastReadMessageId, null);

//        // Assert
//        Assert.NotNull(groupChatUser.LastReadMessageId);
//        Assert.Equal(lastReadMessageId.Value, groupChatUser.LastReadMessageId.Value);
//        Assert.Equal(unreadMessages, groupChatUser.UnreadMessages);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatUserId>()), Times.Once);
//        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task UpdateChatAsync_ShouldUpdateChatUserUnreadMessagesProperty()
//    {
//        // Arrange
//        GroupChatUserId id = "uid-1";
//        GroupChatMessageId lastReadMessageId = null;
//        const int unreadMessages = 5;

//        var groupChatUserDto = GroupChatUserTestData.CreateDto();
//        var groupChatUser = GroupChatUserTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatUserDto>(It.IsAny<GroupChatUser>())).Returns(groupChatUserDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatUserId>())).ReturnsAsync(groupChatUser);
//        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, mockMapper.Object);

//        // Act
//        await service.UpdateChatUserAsync(id, null, unreadMessages);

//        // Assert
//        Assert.Equal(lastReadMessageId, groupChatUser.LastReadMessageId);
//        Assert.Equal(unreadMessages, groupChatUser.UnreadMessages);

//        // Verify correct method calls
//        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatUserId>()), Times.Once);
//        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteAsync_ShouldRemoveEntity()
//    {
//        // Arrange
//        const string chatUserId = "uid-1";

//        var groupChatUsersDto = GroupChatUserTestData.CreateDtoCollection();
//        var groupChatUsers = GroupChatUserTestData.CreateCollection();

//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, null);

//        // Act
//        await service.DeleteAsync(chatUserId);

//        // Assert and Verify correct method calls
//        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<GroupChatUserId>()), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteAsync_ThrowEntityNotFoundException_ShouldNotRemoveEntityAsEntityDoesNotExist()
//    {
//        // Arrange
//        const string chatUserId = "uid-234";

//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockRepository.Setup(r => r.DeleteAsync(It.IsAny<GroupChatUserId>())).Throws(new EntityNotFoundException(typeof(GroupChatUser), chatUserId));

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, null);

//        // Act and Assert
//        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.DeleteAsync(chatUserId));

//        // Verify correct method calls
//        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<GroupChatUserId>()), Times.Once);
//    }

//    [Fact]
//    public async Task GetAllAsync_Collection_ShouldReturnAllEntity()
//    {
//        // Arrange
//        var groupChatUsersDto = GroupChatUserTestData.CreateDtoCollection();
//        var groupChatUsers = GroupChatUserTestData.CreateCollection();

//        var mockMapper = new Mock<IMapper>();
//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockMapper.Setup(m => m.Map<IEnumerable<GroupChatUserDto>>(It.IsAny<IEnumerable<GroupChatUser>>())).Returns(groupChatUsersDto);

//        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(groupChatUsers);

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetAllAsync();

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());

//        // Verify correct method calls
//        mockMapper.Verify(m => m.Map<IEnumerable<GroupChatUserDto>>(It.IsAny<IEnumerable<GroupChatUser>>()), Times.Once);
//        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task GetByIdAsync_GroupChatUser_ShouldReturnCorrectEntity()
//    {
//        // Arrange
//        const string chatUserId = "uid-1";

//        var groupChatUserDto = GroupChatUserTestData.CreateDto();
//        var groupChatUser = GroupChatUserTestData.Create();

//        var mockMapper = new Mock<IMapper>();
//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatUserDto>(It.IsAny<GroupChatUser>())).Returns(groupChatUserDto);

//        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatUserId>())).ReturnsAsync(groupChatUser);

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.GetByIdAsync(chatUserId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(groupChatUserDto.Id, result.Id);
//        Assert.Equal(groupChatUserDto.Username, result.Username);
//        Assert.Equal(groupChatUserDto.UnreadMessages, result.UnreadMessages);
//        Assert.Equal(groupChatUserDto.LastReadMessageId, result.LastReadMessageId);
//        Assert.Equal(groupChatUserDto.GroupChatId, result.GroupChatId);
//        Assert.Equal(groupChatUserDto.AppUserId, result.AppUserId);

//        // Verify correct method calls
//        mockMapper.Verify(m => m.Map<GroupChatUserDto>(It.IsAny<GroupChatUser>()), Times.Once);
//        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatUserId>()), Times.Once);
//    }

//    [Fact]
//    public async Task FindAllAsync_Collection_ShouldReturnCollectionByChatId()
//    {
//        // Arrange
//        const int chatId = 1;

//        var groupChatUsers = GroupChatUserTestData.CreateCollection();
//        var groupChatUsersDto = GroupChatUserTestData.CreateDtoCollection();

//        var mockMapper = new Mock<IMapper>();
//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockMapper.Setup(m => m.Map<IEnumerable<GroupChatUserDto>>(It.IsAny<IEnumerable<GroupChatUser>>())).Returns(groupChatUsersDto);

//        mockRepository.Setup(m => m.FindAllAsync(It.IsAny<int>())).ReturnsAsync(groupChatUsers);

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.FindAllAsync(chatId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());

//        // Verify correct method calls
//        mockMapper.Verify(m => m.Map<IEnumerable<GroupChatUserDto>>(It.IsAny<IEnumerable<GroupChatUser>>()), Times.Once);
//        mockRepository.Verify(r => r.FindAllAsync(It.IsAny<int>()), Times.Once);
//    }

//    [Fact]
//    public async Task FindByAppUserIdAsync_Entity_ShouldReturnCollectionByAppUserId()
//    {
//        // Arrange
//        const int chatId = 1;
//        const string appUserId = "uid-1";

//        var groupChatUser = GroupChatUserTestData.Create();
//        var groupChatUserDto = GroupChatUserTestData.CreateDto();

//        var mockMapper = new Mock<IMapper>();
//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockMapper.Setup(m => m.Map<GroupChatUserDto>(It.IsAny<GroupChatUser>())).Returns(groupChatUserDto);

//        mockRepository.Setup(m => m.FindByAppUserIdAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(groupChatUser);

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.FindByAppUserIdAsync(chatId, appUserId);

//        // Assert
//        Assert.NotNull(result);

//        // Verify correct method calls
//        mockMapper.Verify(m => m.Map<GroupChatUserDto>(It.IsAny<GroupChatUser>()), Times.Once);
//        mockRepository.Verify(r => r.FindByAppUserIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
//    }

//    [Fact]
//    public async Task FindAllByAppUserIdAsync_Collection_ShouldReturnCollectionByAppUserId()
//    {
//        // Arrange
//        const int chatId = 1;
//        const string appUserId = "uid-1";

//        var groupChatUsers = GroupChatUserTestData.CreateCollection();
//        var groupChatUsersDto = GroupChatUserTestData.CreateDtoCollection();

//        var mockMapper = new Mock<IMapper>();
//        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
//        var mockRepository = new Mock<IGroupChatUserRepository>();

//        mockMapper.Setup(m => m.Map<IEnumerable<GroupChatUserDto>>(It.IsAny<IEnumerable<GroupChatUser>>())).Returns(groupChatUsersDto);

//        mockRepository.Setup(m => m.FindAllByAppUserIdAsync(It.IsAny<string>())).ReturnsAsync(groupChatUsers);

//        var service = new GroupChatUserService(mockChatRepository.Object, mockRepository.Object, mockMapper.Object);

//        // Act
//        var result = await service.FindAllByAppUserIdAsync(appUserId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());

//        // Verify correct method calls
//        mockMapper.Verify(m => m.Map<IEnumerable<GroupChatUserDto>>(It.IsAny<IEnumerable<GroupChatUser>>()), Times.Once);
//        mockRepository.Verify(r => r.FindAllByAppUserIdAsync(It.IsAny<string>()), Times.Once);
//    }
//}
