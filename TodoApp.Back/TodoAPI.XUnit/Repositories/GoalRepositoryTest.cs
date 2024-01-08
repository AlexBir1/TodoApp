using Bogus;
using Bogus.DataSets;
using System.Reflection.Metadata;
using TodoAPI.DAL.DBContext;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Implementations;
using TodoAPI.XUnit.DBContext;

namespace TodoAPI.XUnit.Repositories
{
    public class GoalRepositoryTest
    {
        private readonly Faker<Goal> GoalFaker;

        public GoalRepositoryTest()
        {
            GoalFaker = new Faker<Goal>()
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.Title, _ => new Lorem().Word())
                .RuleFor(x => x.Description, _ => new Lorem().Sentence(5))
                .RuleFor(x => x.IsCompleted, _ => Convert.ToBoolean(new Random().Next(0, 1)))
                .RuleFor(x => x.CreationDate, _ => DateTime.Now)
                .RuleFor(x => x.UpdateDate, _ => DateTime.Now)
                .RuleFor(x => x.StartDate, _ => DateTime.Now);
        }

        [Fact]
        public async void CreateAsync_CreatesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = GoalFaker.Generate();

            var repo = new GoalRepository(context);
            var goal = await repo.CreateAsync(testGoals);

            Assert.NotNull(goal);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_DeletesAndReturnsValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = GoalFaker.Generate();
            
            await context.Goals.AddAsync(testGoals);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var repo = new GoalRepository(context);
            var goal = await repo.DeleteAsync(testGoals.Id.ToString());

            Assert.NotNull(goal);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new GoalRepository(context).DeleteAsync(Guid.NewGuid().ToString()));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_UpdatesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = GoalFaker.Generate(2);

            await context.Goals.AddAsync(testGoals[0]);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            testGoals[1].Id = testGoals[0].Id;

            var repo = new GoalRepository(context);
            var goal = await repo.UpdateAsync(testGoals[1].Id.ToString(), testGoals[1]);

            Assert.NotNull(goal);
            Assert.Equal(testGoals[1].Title, goal.Title);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new GoalRepository(context).UpdateAsync(Guid.NewGuid().ToString(), GoalFaker.Generate()));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_ReturnsAListOfValues()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = GoalFaker.Generate();
            
            await context.Goals.AddAsync(testGoals);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var repo = new GoalRepository(context);
            var goals = await repo.GetAllAsync();

            Assert.NotNull(goals);
            Assert.NotEmpty(goals);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new GoalRepository(context).GetAllAsync());

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_ReturnsAListOfValuesWithSpecifiedTitle()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = GoalFaker.Generate(2);

            await context.Goals.AddRangeAsync(testGoals);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var repo = new GoalRepository(context);
            var goals = await repo.GetAllAsync(x => x.Title.Contains(testGoals[0].Title));

            Assert.NotNull(goals);
            Assert.NotEmpty(goals);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_ReturnsValueWithSpecifiedId()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testGoals = GoalFaker.Generate();

            await context.Goals.AddAsync(testGoals);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var repo = new GoalRepository(context);
            var goal = await repo.GetByIdAsync(testGoals.Id.ToString());

            Assert.NotNull(goal);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new GoalRepository(context).GetByIdAsync(Guid.NewGuid().ToString()));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }
    }
}
