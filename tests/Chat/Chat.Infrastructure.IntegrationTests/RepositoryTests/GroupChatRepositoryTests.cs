//using Chat.Domain.Aggregates;
//using Chat.Domain.Enums.GroupChatRules;
//using Chat.Domain.ValueObjects;
//using Chat.Infrastructure.Repositories;
//using Chat.Infrastructure.IntegrationTests.Factory;

//namespace Chat.Infrastructure.IntegrationTests.RepositoryTests;

//public class GroupChatRepositoryTests : RepositoryTestsBase
//{
//    [Fact]
//    public async Task UpdateNameAsync_ShouldUpdateChatNameById()
//    {
//        // Arrange
//        GroupChatId id = 1;
//        const string chatName = "new chat";

//        using var context = CreateInMemoryContext();
//        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GroupChatRepository(context);

//        // Act
//        await repo.UpdateNameAsync(id, chatName);
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//        Assert.Equal(chatName, result.Name);
//    }

//    [Fact]
//    public async Task PassOwnerAsync_ShouldPassNewChatOwner()
//    {
//        // Arrange
//        GroupChatId id = 1;
//        UserId ownerId = "uid-23";

//        using var context = CreateInMemoryContext();
//        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GroupChatRepository(context);

//        // Act
//        await repo.PassOwnerAsync(id, ownerId);
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//        Assert.Equal(ownerId, result.OwnerId);
//    }

//    [Fact]
//    public async Task AddRulesAsync_RulesShouldBeAddedToChat()
//    {
//        // Arrange
//        GroupChatId id = 1;

//        using var context = CreateInMemoryContext();
//        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GroupChatRepository(context);
//        var rules = GroupChatRulesTestData.Create(chatId: id);

//        // Act
//        await repo.AddRulesAsync(rules);
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//        Assert.NotNull(result.Rules);
//    }

//    [Fact]
//    public async Task RemoveRulesAsync_RulesShouldBeRemovedToChat()
//    {
//        // Arrange
//        GroupChatId id = 1;

//        using var context = CreateInMemoryContext();
//        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection(addRules: true));
//        await context.SaveChangesAsync();

//        var repo = new GroupChatRepository(context);

//        // Act
//        await repo.RemoveRulesAsync(id);
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//        Assert.Null(result.Rules);
//    }

//    [Fact]
//    public async Task UpdateRulesAsync_RulesShouldBeUpdatedToChat()
//    {
//        // Arrange
//        GroupChatId id = 1;
//        const InvitePeopleRestrictions invitePeopleRestrictions = InvitePeopleRestrictions.Owner;

//        using var context = CreateInMemoryContext();
//        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection(addRules: true));
//        await context.SaveChangesAsync();

//        var repo = new GroupChatRepository(context);
//        var rules = GroupChatRulesTestData.Create(chatId: id, invitePeople: invitePeopleRestrictions);

//        // Act
//        await repo.UpdateRulesAsync(rules);
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//        Assert.NotNull(result.Rules);
//        Assert.Equal(invitePeopleRestrictions, result.Rules.InvitePeople);
//    }
//}
