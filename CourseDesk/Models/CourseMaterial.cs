using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseDesk.Models
{
    public class CourseMaterial
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        [DataType(DataType.Time)]
        public DateTime Duration { get; set; }

        [DataType(DataType.Date)]
        public DateTime Uploaded_at { get; set; }
        public int PersonId { get; set; }
        public int CategoryId { get; set; } 
        public string ImageUrl { get; set; }
        public virtual User Person { get; set; }
        public virtual Category CourseCategory { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<CourseStatus> CourseStatus { get; set; }

    }

}
