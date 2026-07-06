using BE_CourseSelling.Core.Common;

namespace BE_CourseSelling.Core.Entities
{
    public class CourseDTO : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ThumbnailUrl { get; set; }

        public bool IsPublished { get; set; }
    }
}
