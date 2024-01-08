
namespace TodoAPI.DAL.Entities
{
    public class CategoryGoal
    {
        public Guid GoalId { get; set; }
        public Goal Goal { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
