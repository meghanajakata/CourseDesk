using CourseDesk.Models;
using CourseDesk.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;

namespace CourseDesk.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DbConnection _context;
        public CartRepository(DbConnection context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds cart object to the cart table
        /// </summary>
        /// <param name="cart"></param>
        public void AddCart(Cart cart)
        {
            _context.Cart.Add(cart);
            _context.SaveChanges();
        }

        public void AddCartItem(CartItem cartItem)
        {
            _context.CartItem.Add(cartItem);
            _context.SaveChanges();
        }
        public Cart GetCartByUserId(int studentId)
        {
            return _context.Cart.Where(c => c.StudentId == studentId).FirstOrDefault();
        }

        public CartItem GetCartItemByCartIdAndCourseId(int cartId,int courseId) 
        {
            return _context.CartItem.FirstOrDefault(u => u.CartId == cartId && u.CourseId == courseId);
        }

        public CartItem GetCartItemByCartItemId(int cartItemId)
        {
            return _context.CartItem.Where(u => u.Id == cartItemId).FirstOrDefault();
        }
        public decimal GetTotalAmountOfCartByCartId(int cartId)
        {
            return _context.CartItem.Where(ci => ci.CartId == cartId).Select(ci => ci.Course.Price).Sum();
        }
        public IEnumerable<CartItem> GetCartitemsByCartId(int cartId)
        {
            return _context.CartItem.Include(c => c.Course).Where(u => u.CartId == cartId).ToList();
        }
        public void UpdateCart(Cart cart)
        {
            _context.SaveChanges();
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            _context.Remove(cartItem);
            _context.SaveChanges();
        }
    }
}
