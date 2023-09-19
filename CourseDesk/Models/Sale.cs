using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public decimal Amount { get; set; }
        public virtual CourseMaterial Course { get; set; }  
        public virtual User Instructor { get; set; }

    }
}
