//using Chat.Domain.Aggregates;
//using Chat.Domain.ValueObjects;
//using Chat.Infrastructure.IntegrationTests.Factory;
//using Chat.Infrastructure.Repositories;

//namespace Chat.Infrastructure.IntegrationTests.RepositoryTests;

//public class PersonalChatRepositoryTests : RepositoryTestsBase
//{
//    [Fact]
//    public async Task GetByUserIdAsync_Collection_ShouldReturnChatsByUserId()
//    {
//        // Arrange
//        const string userId = "uid-1";

//        using var context = CreateInMemoryContext();
//        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new PersonalChatRepository(context);

//        // Act
//        var result = await repo.GetByUserIdAsync(userId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Single(result);
//    }

//    [Fact]
//    public async Task UpdateInitiatorUnreadMessageCountAsync_ShouldUpdateCountInitiatorUnreadMessage()
//    {
//        // Arrange
//        PersonalChatId id = 1;
//        const int initiatorUnreadMessageCount = 5;

//        using var context = CreateInMemoryContext();
//        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new PersonalChatRepository(context);

//        // Act
//        await repo.UpdateInitiatorUnreadMessageCountAsync(id, initiatorUnreadMessageCount);
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//        Assert.Equal(initiatorUnreadMessageCount, result.InitiatorUnreadMessages);
//    }

//    [Fact]
//    public async Task UpdateCompanionUnreadMessageCountAsync_ShouldUpdateCountCompanionUnreadMessage()
//    {
//        // Arrange
//        PersonalChatId id = 1;
//        const int companionUnreadMessageCount = 4;

//        using var context = CreateInMemoryContext();
//        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new PersonalChatRepository(context);

//        // Act
//        await repo.UpdateCompanionUnreadMessageCountAsync(id, companionUnreadMessageCount);
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//        Assert.Equal(companionUnreadMessageCount, result.CompanionUnreadMessages);
//    }
//}
