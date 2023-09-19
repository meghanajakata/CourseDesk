using System.ComponentModel.DataAnnotations;


namespace CourseDesk.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public decimal TotalAmount { get; set; }

        [DataType(DataType.Date)]
        public DateTime Carted_at { get; set; }
        public virtual User Student { get; set; }
        public virtual ICollection<CartItem> cartItems { get; set; }

    }
}
