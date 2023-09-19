using Microsoft.AspNetCore.Mvc;
using CourseDesk.Models;
using CourseDesk.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using NuGet.Packaging;
using Microsoft.Identity.Client;

namespace CourseDesk.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly DbConnection _context;

        public EnrollmentController(DbConnection context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public bool UpdateEnrollment(int? cart_id, int? payment_id)
        {
            List<Enrollment> enrollment = new List<Enrollment>();
            var cartItems = _context.CartItem.Include(u => u.Cart).Where(x => x.CartId == cart_id).ToList();
            try
            {
                enrollment.AddRange(cartItems.Select(item => new Enrollment
                {
                    UserId = item.Cart.StudentId,
                    PaymentId = (int)payment_id,
                    CourseId = item.CourseId
                }));

                foreach(var item in enrollment)
                {
                    Debug.WriteLine($"enrolled item values are {item.UserId} , {item.PaymentId}, {item.CourseId}");
                }

                _context.AddRange(enrollment);
                _context.RemoveRange(cartItems);

                Debug.WriteLine("Added to Enrollment and removed cartitems successfully");
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;   
            }

        }

        public IActionResult EnrolledCourses()
        {
            if(HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int student_id = (int)HttpContext.Session.GetInt32("user_id");
            var enrollement = _context.Enrollment.Include(u => u.Course).Where(u => u.UserId == student_id).ToList();

            return View(@"Views/Student/EnrolledCourses.cshtml",enrollement);
        }
    }
}
