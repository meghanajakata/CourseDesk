using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseDesk.Data;
using CourseDesk.Models;
using CourseDesk.Filters;
using System.Diagnostics;
using CourseDesk.Repositories;

namespace CourseDesk.Controllers
{
    public class CartsController : Controller
    {
        private readonly DbConnection _context;
        private readonly ICartRepository _cartRepository;
        public CartsController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpPost]
        [Authorization(UserType.Student)]
        public ActionResult AddToCart(int id)
        {
            string _message = "Not yet Added";
            Debug.WriteLine($"Course Id value is {id}");
            int student_id = (int)HttpContext.Session.GetInt32("user_id");
            if (id == null)
            {
                _message = "Course Id value not retrieved";
            }
            Cart cartObj = _cartRepository.GetCartByUserId(student_id);
            if (cartObj == null)
            {
                cartObj = new Cart();
                cartObj.StudentId = student_id;
                cartObj.Carted_at = DateTime.Today;
                _cartRepository.AddCart(cartObj);
            }
            Debug.WriteLine($"cartObj values are {cartObj.Id}, {cartObj.StudentId}");
            CartItem cartItem = _cartRepository.GetCartItemByCartIdAndCourseId(cartObj.Id, (int)id);
            if (cartItem == null)
            {
                cartItem = new CartItem();
                cartItem.CartId = cartObj.Id;
                cartItem.CourseId = (int)id;
                _cartRepository.AddCartItem(cartItem);
                Debug.WriteLine($"Values for CartItem are {cartItem.Id}, {cartItem.CartId}, {cartItem.CourseId}");
                _message = "Successfully Added to Cart";

            }
            else if (cartItem != null)
            {
                _message = "Course already added to cart";
            }

            decimal totalCartPrice = _cartRepository.GetTotalAmountOfCartByCartId(cartObj.Id);
            // Handle case when there are no items in the cart
            Debug.WriteLine($"Toatal Cart Price is  {totalCartPrice}");
            cartObj.TotalAmount = totalCartPrice;
            _cartRepository.UpdateCart(cartObj);
            return Json(new { message = _message });

        }


        /// <summary>
        /// Removes the item from CartItem table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int? id)
        {
            if (HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int student_id = (int)HttpContext.Session.GetInt32("user_id");
            var cartItem = _cartRepository.GetCartItemByCartItemId((int)id);
            _cartRepository.RemoveCartItem(cartItem);

            Cart cart = _cartRepository.GetCartByUserId(student_id);
            cart.TotalAmount = _cartRepository.GetTotalAmountOfCartByCartId(cart.Id);
            _cartRepository.UpdateCart(cart);

            return RedirectToAction("GoToBag", "Carts");
        }

        /// <summary>
        /// Returns the Cart of the user
        /// </summary>
        /// <returns></returns>
        [Authorization(UserType.Student)]
        public IActionResult GoToBag()
        {
            if (HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int student_id = (int)HttpContext.Session.GetInt32("user_id");
            //int student_id = 3;
            //Cart cart = _context.Cart.FirstOrDefault(u => u.StudentId == student_id);
            Cart cart = _cartRepository.GetCartByUserId(student_id);
            if (cart != null)
            {
                IEnumerable<CartItem> cartItems = _cartRepository.GetCartitemsByCartId(cart.Id);
                ViewBag.Total = cart.TotalAmount;
                return View(@"Views\Student\GoToBag.cshtml", cartItems);
            }

            return NotFound();
        }


    }
}

