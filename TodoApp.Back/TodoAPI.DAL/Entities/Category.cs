

namespace TodoAPI.DAL.Entities
{
    public class Category : BaseEntity
    {
        public string ColorTitle { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;

        public ICollection<CategoryGoal> CategoryGoals { get; set; }
        public string AccountId { get; set; } = string.Empty;
        public Account Account { get; set; }

        public override void OptimizeDepth()
        {
            Account.Collections = null;
        }
    }
}
