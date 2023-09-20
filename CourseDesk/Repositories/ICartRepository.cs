using CourseDesk.Models;

namespace CourseDesk.Repositories
{
    public interface ICartRepository
    {
        public void AddCart(Cart cart);
        public void AddCartItem(CartItem cartItem);
        public Cart GetCartByUserId(int userId);
        public CartItem GetCartItemByCartIdAndCourseId(int cartId, int courseId);
        public CartItem GetCartItemByCartItemId(int cartItemId);
        public IEnumerable<CartItem> GetCartitemsByCartId(int cartId);
        public decimal GetTotalAmountOfCartByCartId(int cartId);
        public void UpdateCart(Cart cart);
        public void RemoveCartItem(CartItem cartItem);

    }
}
