
namespace TodoAPI.DAL.Entities
{
    public class Goal : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public Guid CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<CategoryGoal> GoalCategories { get; set; }

        public GoalNotificator Notificator { get; set; }

        public override void OptimizeDepth()
        {
            if (Collection != null)
            {
                Collection.Goals = null;
                Collection.Account = null;
            }

            if (Attachments != null)
                foreach (var a in Attachments)
                {
                    a.Goal = null;
                }

            if (GoalCategories != null)
                foreach (var gc in GoalCategories)
                {
                    gc.Goal = null;
                    gc.Category.CategoryGoals = null;
                }
        }
    }
}
