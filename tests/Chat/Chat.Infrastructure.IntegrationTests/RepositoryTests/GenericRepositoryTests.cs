//using Chat.Domain.Aggregates;
//using Chat.Domain.ValueObjects;
//using Chat.Infrastructure.Exceptions;
//using Chat.Infrastructure.Repositories;
//using Chat.Infrastructure.IntegrationTests.Factory;

//namespace Chat.Infrastructure.IntegrationTests.RepositoryTests;

//public class GenericRepositoryTests : RepositoryTestsBase
//{
//    [Fact]
//    public async Task CreateAsync_PersonalChat_ShouldCreateEntityAndReturnCreatedEntity()
//    {
//        // Arrange
//        PersonalChatId id = 1;

//        using var context = CreateInMemoryContext();
//        var repo = new GenericRepository<PersonalChat, PersonalChatId>(context);
//        var chat = PersonalChatTestData.Create();

//        // Act
//        var result = await repo.CreateAsync(chat);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//        Assert.Single(context.Set<PersonalChat>());
//    }

//    [Fact]
//    public async Task DeleteAsync_ShouldRemoveEntity()
//    {
//        // Arrange
//        PersonalChatId id = 1;

//        using var context = CreateInMemoryContext();
//        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GenericRepository<PersonalChat, PersonalChatId>(context);

//        // Act
//        await repo.DeleteAsync(id);

//        // Assert
//        Assert.NotEmpty(context.Set<PersonalChat>());
//        Assert.Equal(2, context.Set<PersonalChat>().Count());
//    }

//    [Fact]
//    public async Task DeleteAsync_ShouldNotRemoveEntityAsEntityDoesNotExist()
//    {
//        // Arrange
//        PersonalChatId id = 5;

//        using var context = CreateInMemoryContext();
//        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GenericRepository<PersonalChat, PersonalChatId>(context);

//        // Act and Assert
//        await Assert.ThrowsAsync<EntityNotFoundException>(() => repo.DeleteAsync(id));
//    }

//    [Fact]
//    public async Task GetAllAsync_Collection_ShouldReturnAllEntity()
//    {
//        // Arrange
//        using var context = CreateInMemoryContext();
//        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GenericRepository<PersonalChat, PersonalChatId>(context);

//        // Act
//        var result = await repo.GetAllAsync();

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());
//    }

//    [Fact]
//    public async Task GetByIdAsync_PersonalChat_ShouldReturnCorrectEntity()
//    {
//        // Arrange
//        PersonalChatId id = 1;

//        using var context = CreateInMemoryContext();
//        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GenericRepository<PersonalChat, PersonalChatId>(context);

//        // Act
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//    }

//    [Fact]
//    public async Task GetByParamAsync_Collection_ShouldReturnCollectionUsePagination()
//    {
//        // Arrange
//        const int page = 1;
//        const int pageSize = 2;

//        using var context = CreateInMemoryContext();
//        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
//        await context.SaveChangesAsync();

//        var repo = new GenericRepository<PersonalChat, PersonalChatId>(context);

//        // Act
//        var result = await repo.GetByPaginationAsync(page, pageSize);

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(2, result.Count());
//    }
//}
