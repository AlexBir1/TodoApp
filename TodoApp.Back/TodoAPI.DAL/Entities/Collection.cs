
namespace TodoAPI.DAL.Entities
{
    public class Collection : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public ICollection<Goal> Goals { get; set; }

        public string AccountId { get; set; }
        public Account Account { get; set; }

        public override void OptimizeDepth()
        {
            Account.Collections = null;
        }
    }
}
