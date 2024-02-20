using Bogus;
using Bogus.DataSets;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Implementations;
using TodoAPI.XUnit.DBContext;

namespace TodoAPI.XUnit.Repositories
{
    public class CollectionRepositoryTest
    {
        private readonly Faker<Collection> CollectionFaker;

        public CollectionRepositoryTest()
        {
            CollectionFaker = new Faker<Collection>()
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.Title, new Lorem().Word())
                .RuleFor(x => x.AccountId, _ => Guid.NewGuid().ToString());
        }

        [Fact]
        public async void CreateAsync_CreatesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCollections = CollectionFaker.Generate();

            var result = await new CollectionRepository(context).CreateAsync(testCollections);

            Assert.NotNull(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void CreateAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCollections = CollectionFaker.Generate();

            await context.Collections.AddAsync(testCollections);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new CollectionRepository(context).CreateAsync(testCollections));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_DeletesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCollections = CollectionFaker.Generate();

            await context.Collections.AddAsync(testCollections);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var result = await new CollectionRepository(context).DeleteAsync(testCollections.Id.ToString());

            Assert.NotNull(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new CollectionRepository(context).DeleteAsync(Guid.NewGuid().ToString()));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public async void UpdateAsync_UpdatesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCollections = CollectionFaker.Generate(2);
            testCollections[1].Id = testCollections[0].Id;

            await context.Collections.AddAsync(testCollections[0]);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var сollection = await new CollectionRepository(context).UpdateAsync(testCollections[1].Id.ToString(), testCollections[1]);

            Assert.NotNull(сollection);
            Assert.Equal(testCollections[1].Title, сollection.Title);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new CollectionRepository(context).UpdateAsync(Guid.NewGuid().ToString(), CollectionFaker.Generate()));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_ReturnsAListOfValues()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = CollectionFaker.Generate();
            
            await context.Collections.AddAsync(testGoals);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var goals = await new CollectionRepository(context).GetAllAsync();

            Assert.NotNull(goals);
            Assert.NotEmpty(goals);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new CollectionRepository(context).GetAllAsync());

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_ReturnsAListOfValuesWithSpecifiedTitle()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = CollectionFaker.Generate(2);
            
            await context.Collections.AddRangeAsync(testGoals);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var goals = await new CollectionRepository(context).GetAllAsync(x => x.Title == testGoals[0].Title);

            Assert.NotNull(goals);
            Assert.NotEmpty(goals);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new CollectionRepository(context).GetByIdAsync(Guid.NewGuid().ToString()));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_ReturnsValueWithSpecifiedId()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = CollectionFaker.Generate();

            await context.Collections.AddAsync(testGoals);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var goals = await new CollectionRepository(context).GetByIdAsync(testGoals.Id.ToString());

            Assert.NotNull(goals);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }
    }
}
