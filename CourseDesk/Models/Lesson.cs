using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Video_Url { get; set; }
        public string Description { get; set; }
        public DateTime Duration { get; set; }
        public virtual CourseMaterial Course { get; set; }
    }
}
