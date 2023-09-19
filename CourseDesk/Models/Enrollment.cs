using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int PaymentId { get; set; }
        public virtual User User { get; set; }
        public virtual CourseMaterial Course{ get; set; }
        public virtual Payment Payment { get; set; }
    }
}
