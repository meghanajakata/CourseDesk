using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class CourseStatus
    {
        [Key] 
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int CourseId { get; set; }
        public virtual CourseMaterial Course { get; set; }
        public virtual User Student { get; set; }
    }
}
