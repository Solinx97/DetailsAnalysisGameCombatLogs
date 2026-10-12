//using CombatAnalysis.NotificationDAL.Entities;
//using CombatAnalysis.NotificationDAL.Repositories;
//using CombatAnalysis.NotificationDAL.IntegrationTests.Factory;
//using CombatAnalysis.UserDAL.IntegrationTests.RepositoryTests;
//using System.Linq.Expressions;

//namespace CombatAnalysis.NotificationDAL.IntegrationTests.RepositoryTests;

//public class GenericRepositoryTests : RepositoryTestsBase
//{
//    [Fact]
//    public async Task CreateAsync_Entity_ShouldCreateNewEntityAndReturnCreatedEntity()
//    {
//        // Arrange
//        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldCreateNewEntityAndReturnCreatedEntity));

//        var repo = new GenericRepository<Notification, int>(context);
//        var notification = NotificationTestDataFactory.Create();

//        // Act
//        var result = await repo.CreateAsync(notification);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Single(context.Set<Notification>());
//    }

//    [Fact]
//    public async Task UpdateAsync_ShouldUpdateEcistedEntityById()
//    {
//        // Arrange
//        const int id = 1;
//        const string initiatorId = "uid-234";

//        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateEcistedEntityById));
//        var repo = new GenericRepository<Notification, int>(context);
//        await context.Set<Notification>().AddRangeAsync(NotificationTestDataFactory.CreateCollection());
//        await context.SaveChangesAsync();

//        var notification = NotificationTestDataFactory.Create(initiatorId: initiatorId);

//        // Act
//        await repo.UpdateAsync(id, notification);

//        var updatedEntity = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(updatedEntity);
//        Assert.Equal(id, updatedEntity.Id);
//        Assert.Equal(initiatorId, updatedEntity.InitiatorId);
//    }

//    [Fact]
//    public async Task UpdateAsync_ThrowKeyNotFoundException_ShouldNotUpdateExistedEntityById()
//    {
//        // Arrange
//        const int id = 23;
//        const string initiatorId = "uid-234";

//        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateEcistedEntityById));
//        var repo = new GenericRepository<Notification, int>(context);
//        await context.Set<Notification>().AddRangeAsync(NotificationTestDataFactory.CreateCollection());
//        await context.SaveChangesAsync();

//        var notification = NotificationTestDataFactory.Create(id: id, initiatorId: initiatorId);

//        // Act and Assert
//        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(id, notification));
//    }

//    [Fact]
//    public async Task DeleteAsync_ShouldDeleteEntity()
//    {
//        // Arrange
//        const int id = 1;

//        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldDeleteEntity));

//        var repo = new GenericRepository<Notification, int>(context);
//        await context.Set<Notification>().AddRangeAsync(NotificationTestDataFactory.CreateCollection());
//        await context.SaveChangesAsync();

//        // Act
//        await repo.DeleteAsync(id);

//        // Assert
//        Assert.Equal(2, context.Set<Notification>().Count());
//    }

//    [Fact]
//    public async Task GetAllAsync_Collection_ShouldReturnAllElements()
//    {
//        // Arrange
//        using var context = CreateInMemoryContext(nameof(GetAllAsync_Collection_ShouldReturnAllElements));

//        var repo = new GenericRepository<Notification, int>(context);
//        await context.Set<Notification>().AddRangeAsync(NotificationTestDataFactory.CreateCollection());
//        await context.SaveChangesAsync();

//        // Act
//        var result = await repo.GetAllAsync();

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal(3, result.Count());
//    }

//    [Fact]
//    public async Task GetByIdAsync_Entity_ShouldReturnEntityById()
//    {
//        // Arrange
//        const int id = 1;

//        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Entity_ShouldReturnEntityById));

//        var repo = new GenericRepository<Notification, int>(context);
//        await context.Set<Notification>().AddRangeAsync(NotificationTestDataFactory.Create());
//        await context.SaveChangesAsync();

//        // Act
//        var result = await repo.GetByIdAsync(id);

//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(id, result.Id);
//    }

//    [Fact]
//    public async Task GetByParamAsync_Collection_ShouldReturnElementsByParam()
//    {
//        // Arrange
//        const string initiatorId = "uid-22";

//        using var context = CreateInMemoryContext(nameof(GetByParamAsync_Collection_ShouldReturnElementsByParam));

//        var repo = new GenericRepository<Notification, int>(context);
//        await context.Set<Notification>().AddRangeAsync(NotificationTestDataFactory.Create());
//        await context.SaveChangesAsync();

//        Expression<Func<Notification, string>> expression = n => n.InitiatorId;

//        // Act
//        var result = await repo.GetByParamAsync(expression, initiatorId);

//        // Assert
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Single(result);
//    }
//}
