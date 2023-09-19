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

namespace CourseDesk.Controllers
{
    public class CartsController : Controller
    {
        private readonly DbConnection _context;

        public CartsController(DbConnection context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var dbConnection = _context.Cart.Include(c => c.Student);
            return View(await dbConnection.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,TotalAmount,Carted_at")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Email", cart.StudentId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Email", cart.StudentId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,TotalAmount,Carted_at")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Email", cart.StudentId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'DbConnection.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return (_context.Cart?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorization(UserType.Student)]
        public IActionResult GoToBag()
        {
            if (HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int student_id = (int)HttpContext.Session.GetInt32("user_id");
            //int student_id = 3;
            Cart cart = _context.Cart.FirstOrDefault(u => u.StudentId == student_id);

           

            if (cart != null)
            {
                List<CartItem> cartItems = _context.CartItem.Include(c => c.Course).Where(u => u.CartId == cart.Id).ToList();
                //decimal totalAmount = GetTotalAmount(cartItems);
                //cart.TotalAmount = totalAmount;
                ViewBag.Total = cart.TotalAmount;
                return View(@"Views\Student\GoToBag.cshtml", cartItems);
            }

            return NotFound();
        }

        public decimal GetTotalAmount(List<CartItem> model)
        {
            decimal totalAmount = 0;
            foreach(var item in model)
            {
                totalAmount = totalAmount + item.Course.Price;
            }
            return totalAmount;
        }
    }
}
