

namespace TodoAPI.DAL.Entities
{
    public class Goal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<CategoryGoal> GoalCategories { get; set; }
    }
}
