//using Chat.Domain.Aggregates;
//using Chat.Domain.Entities;
//using Chat.Domain.ValueObjects;
//using Chat.Infrastructure.IntegrationTests.Factory;
//using Chat.Infrastructure.Repositories;

//namespace Chat.Infrastructure.IntegrationTests.RepositoryTests;

//public class GroupChatUserUserRepositoryTests : RepositoryTestsBase
//{
//    [Fact]
//    public async Task FindAllAsync_Collection_ShouldReturnAllChatUsersByChatId()
//    {
//        // Arrange
//        GroupChatId chatId = 1;

//        using var context = CreateInMemoryContext();
//        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
//        await context.Set<GroupChatUser>().AddRangeAsync(GroupChatUserTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GroupChatUserRepository(context);

//        // Act
//        var result = await repo.FindAllAsync(chatId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Single(result);
//        Assert.Equal(chatId, result.First().GroupChatId);
//    }

//    [Fact]
//    public async Task FindAllByAppUserIdAsync_Collection_ShouldReturnAllChatUsersByUserId()
//    {
//        // Arrange
//        UserId userId = "uid-1-1";

//        using var context = CreateInMemoryContext();
//        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
//        await context.Set<GroupChatUser>().AddRangeAsync(GroupChatUserTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GroupChatUserRepository(context);

//        // Act
//        var result = await repo.FindAllByAppUserIdAsync(userId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Single(result);
//        Assert.Equal(userId, result.First().AppUserId);
//    }

//    [Fact]
//    public async Task FindByAppUserIdAsync_Entity_ShouldReturnChatUsersByUserIdAndCchatId()
//    {
//        // Arrange
//        GroupChatId chatId = 2;
//        UserId userId = "uid-1-1";

//        using var context = CreateInMemoryContext();
//        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
//        await context.Set<GroupChatUser>().AddRangeAsync(GroupChatUserTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GroupChatUserRepository(context);

//        // Act
//        var result = await repo.FindByAppUserIdAsync(chatId, userId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(chatId, result.GroupChatId);
//        Assert.Equal(userId, result.AppUserId);
//    }
//}
