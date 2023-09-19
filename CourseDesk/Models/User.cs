using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter the password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter the email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter the phoneNumber")]
        public string Phone { get; set; }
        public string User_type { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Enter date")]
        public DateTime Created_at { get; set; }

        public virtual ICollection<CourseMaterial> CourseMaterials { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<InstructorPayment> InstructorPayments { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<CourseStatus> CourseStatus { get; set;}
        public virtual ICollection<Query> Queries { get; set; }

    }
}
