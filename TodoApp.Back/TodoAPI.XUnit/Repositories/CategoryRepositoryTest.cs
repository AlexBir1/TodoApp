using Bogus;
using Bogus.DataSets;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Implementations;
using TodoAPI.XUnit.DBContext;

namespace TodoAPI.XUnit.Repositories
{
    public class CategoryRepositoryTest
    {
        private readonly Faker<Category> CategoryFaker;

        public CategoryRepositoryTest()
        {
            CategoryFaker = new Faker<Category>()
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.ColorTitle, new Lorem().Word())
                .RuleFor(x => x.ColorHex, new Lorem().Word())
                .RuleFor(x => x.AccountId, new Lorem().Word());
        }

        [Fact]
        public async void CreateCategory_CreatesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCategory = CategoryFaker.Generate();

            var repo = new CategoryRepository(context);
            var result = await repo.CreateAsync(testCategory);

            Assert.NotNull(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void CreateCategory_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCategory = CategoryFaker.Generate();

            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new CategoryRepository(context).CreateAsync(testCategory));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void DeleteCategory_DeletesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCategory = CategoryFaker.Generate();

            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var repo = new CategoryRepository(context);
            var result = await repo.DeleteAsync(testCategory.Id.ToString());

            Assert.NotNull(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void DeleteCategory_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new CategoryRepository(context).DeleteAsync(Guid.NewGuid().ToString()));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void UpdateCategory_UpdatesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCategories = CategoryFaker.Generate(2);

            await context.Categories.AddAsync(testCategories[0]);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            testCategories[1].Id = testCategories[0].Id;

            var result = new CategoryRepository(context).UpdateAsync(testCategories[1].Id.ToString(), testCategories[1]);

            Assert.NotNull(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void UpdateCategory_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCategory = CategoryFaker.Generate();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new CategoryRepository(context).UpdateAsync(testCategory.Id.ToString(), testCategory));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetCategoryById_ReturnsValueWithSpecifiedId()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCategory = CategoryFaker.Generate();

            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var result = new CategoryRepository(context).GetByIdAsync(testCategory.Id.ToString());

            Assert.NotNull(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetCategoryById_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new CategoryRepository(context).GetByIdAsync(Guid.NewGuid().ToString()));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetAllCategories_ReturnsAListOfValues()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testCategory = CategoryFaker.Generate();

            await context.Categories.AddAsync(testCategory);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var result = await new CategoryRepository(context).GetAllAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void GetAllCategories_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<Exception>(async () => await new CategoryRepository(context).GetAllAsync());

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }
    }
}
