using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class InstructorPayment
    {
        [Key]
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public decimal Amount { get; set; }
        public virtual User Instructor { get; set; }
    }
}
