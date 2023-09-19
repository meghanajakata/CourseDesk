using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseDesk.Data;
using CourseDesk.Models;
using System.Diagnostics;

namespace CourseDesk.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly DbConnection _context;

        public CartItemsController(DbConnection context)
        {
            _context = context;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var dbConnection = _context.CartItem.Include(c => c.Cart).Include(c => c.Course);
            return View(await dbConnection.ToListAsync());
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CartItem == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Cart)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id");
            ViewData["CourseId"] = new SelectList(_context.CoursesMaterials, "Id", "Id");
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(int? id)
        {
            string _message = "Not yet Added";
            Debug.WriteLine($"Course Id value is {id}");
            if(HttpContext.Session.GetInt32("user_id") == null)
            {
                _message = "Login to add courses to Cart";
            }
            int? student_id = HttpContext.Session.GetInt32("user_id");
            if(student_id == null)
            {
                _message = "Login to add courses to Cart";
                
            }
            if(id == null)
            {
                _message = "Course Id value not retrived";
            }
            Cart cartObj = _context.Cart.Where(c => c.StudentId == student_id).FirstOrDefault();

            if(cartObj == null)
            {
                //int cart_id = (int);
                cartObj = new Cart();
                cartObj.StudentId = (int)student_id;
                cartObj.TotalAmount = 0;
                cartObj.Carted_at =  DateTime.Today;
                _context.Add(cartObj);
                _context.SaveChanges();
            }
            Debug.WriteLine($"cartObj values are {cartObj.Id}, {cartObj.StudentId}");
            CartItem cartItem = _context.CartItem.FirstOrDefault(u => u.CartId == cartObj.Id && u.CourseId == id);
            if(cartItem == null)
            {
                cartItem = new CartItem();
                cartItem.CartId = cartObj.Id;
                cartItem.CourseId = (int)id; 
                _context.CartItem.Add(cartItem);
                _context.SaveChanges();

                Debug.WriteLine($"Values for CartItem are {cartItem.Id}, {cartItem.CartId}, {cartItem.CourseId}");
                _message = "Successfully Added to Cart";
                
            }
            else if(cartItem != null)
            {
                _message = "Course already added to cart";
            }

            decimal totalCartPrice = _context.CartItem
                                    .Where(ci => ci.CartId == cartObj.Id)
                                    .Select(ci => ci.Course.Price)
                                    .Sum();
            // Handle case when there are no items in the cart
            Debug.WriteLine($"Toatal Cart Price is  {totalCartPrice}");
            cartObj.TotalAmount = totalCartPrice;
            _context.SaveChanges();
            return Json(new { message = _message });

        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CartId,CourseId")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", cartItem.CartId);
            ViewData["CourseId"] = new SelectList(_context.CoursesMaterials, "Id", "Id", cartItem.CourseId);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CartItem == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", cartItem.CartId);
            ViewData["CourseId"] = new SelectList(_context.CoursesMaterials, "Id", "Id", cartItem.CourseId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CartId,CourseId")] CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", cartItem.CartId);
            ViewData["CourseId"] = new SelectList(_context.CoursesMaterials, "Id", "Id", cartItem.CourseId);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CartItem == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Cart)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CartItem == null)
            {
                return Problem("Entity set 'DbConnection.CartItem'  is null.");
            }
            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int? id)
        {
            if(HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int student_id = (int)HttpContext.Session.GetInt32("user_id");
            var cart_item = _context.CartItem.Where(u => u.Id == id).FirstOrDefault();
            _context.Remove(cart_item);
            _context.SaveChanges();

            Cart cart = _context.Cart.Where(u => u.StudentId == student_id).FirstOrDefault();
            cart.TotalAmount = _context.CartItem
                                  .Where(ci => ci.CartId == cart.Id)
                                  .Select(ci => ci.Course.Price)
                                  .Sum();
            _context.SaveChanges();
            return RedirectToAction("GoToBag","Carts");
        }

        private bool CartItemExists(int id)
        {
          return (_context.CartItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}