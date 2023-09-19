using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class Category
    {
        [Key]

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CourseMaterial> Materials { get; set; }
    }
}
