

namespace TodoAPI.DAL.Entities
{
    public class Attachment : BaseEntity
    {
        public string Filename { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Fullpath { get; set; } = string.Empty;
        public long Size { get; set; }

        public Guid GoalId { get; set; }
        public Goal Goal { get; set; }

        public override void OptimizeDepth()
        {
            Goal.Attachments = null;
            Goal.Collection = null;
            Goal.GoalCategories = null;
        }
    }
}
