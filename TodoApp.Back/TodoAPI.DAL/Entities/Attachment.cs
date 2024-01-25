

namespace TodoAPI.DAL.Entities
{
    public class Attachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Filename { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Fullpath { get; set; } = string.Empty;
        public long Size { get; set; }

        public Guid GoalId { get; set; }
        public Goal Goal { get; set; }
    }
}
