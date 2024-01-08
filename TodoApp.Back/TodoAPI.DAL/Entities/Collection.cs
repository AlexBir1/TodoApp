
namespace TodoAPI.DAL.Entities
{
    public class Collection
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;

        public ICollection<Goal> Goals { get; set; }

        public string AccountId { get; set; }
        public Account Account { get; set; }
    }
}
