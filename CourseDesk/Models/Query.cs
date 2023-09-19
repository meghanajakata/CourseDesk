using System.ComponentModel.DataAnnotations;


namespace CourseDesk.Models
{
    public class Query
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int InstructorId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public virtual User Student { get; set; }
        public virtual User Instructor { get; set; }
        
    }
}
