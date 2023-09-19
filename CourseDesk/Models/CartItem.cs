using System.ComponentModel.DataAnnotations;

namespace CourseDesk.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public int CourseId { get; set; }
        public virtual CourseMaterial Course { get; set; }
        public virtual Cart  Cart { get; set; }
    }
}
