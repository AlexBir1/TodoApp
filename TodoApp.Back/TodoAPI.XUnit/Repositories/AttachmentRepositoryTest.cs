using Bogus;
using Bogus.DataSets;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Implementations;
using TodoAPI.XUnit.DBContext;

namespace TodoAPI.XUnit.Repositories
{
    public class AttachmentRepositoryTest
    {
        private readonly Faker<Attachment> AttachmentFaker;

        public AttachmentRepositoryTest()
        {
            AttachmentFaker = new Faker<Attachment>()
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.Filename, new Lorem().Word())
                .RuleFor(x => x.Fullpath, new Lorem().Word())
                .RuleFor(x => x.ContentType, new Lorem().Word())
                .RuleFor(x => x.GoalId, _ => Guid.NewGuid())
                .RuleFor(x => x.Size, 2048);
        }

        [Fact]
        public async void CreateAttachment_CreatesAndReturnsNewValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testAttachment = AttachmentFaker.Generate();

            var result = new AttachmentRepository(context).CreateAsync(testAttachment);

            Assert.NotNull(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void CreateAttachment_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testAttachment = AttachmentFaker.Generate();

            await context.Attachments.AddAsync(testAttachment);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new AttachmentRepository(context).CreateAsync(testAttachment));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void DeleteAttachment_DeletesAndReturnsOldValue()
        {
            var context = FakeDBContext.MakeTestInstance();
            var testAttachment = AttachmentFaker.Generate();

            await context.Attachments.AddAsync(testAttachment);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var result = new AttachmentRepository(context).DeleteAsync(testAttachment.Id.ToString());
            Assert.NotNull(result);

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async void DeleteAttachment_ThrowsAnException()
        {
            var context = FakeDBContext.MakeTestInstance();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await new AttachmentRepository(context).DeleteAsync(Guid.NewGuid().ToString()));

            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }
    }
}
