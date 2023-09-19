using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }    
    }
}
