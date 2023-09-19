using CourseDesk.Models;
using CourseDesk.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CourseDesk.Controllers
{
    public class PaymentController : Controller
    {

        private readonly DbConnection _context;

        public PaymentController(DbConnection context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            int student_id = (int)HttpContext.Session.GetInt32("user_id");
            Cart cartObj = _context.Cart.Include(c => c.cartItems).ThenInclude(u => u.Course).Where(c => c.StudentId == student_id).FirstOrDefault();
            //int student_id = 3;
            //var cartitems = _context.CartItem.Include(u => u.Course).Include(u => u.Cart).Where(u => u.CartId == cartObj.Id).ToList();
            
            
            return View(@"Views/Student/Checkout.cshtml",cartObj);
        }

        [HttpPost]
        public IActionResult Proceed(FormData form, string mode)
        {
            if (mode == "card")
            {
                if (form.Name != null && form.Number != null && form.cvv != null)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false , message = "Enter the details accordingly"});
            }

            else if(mode == "upi")
            {
                if(form.payment_address != null)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Enter the details accordingly" });
            }

            return Json(new { success = false, message = "Please select a payment option" });
        }


        public IActionResult Success(int? cartId)
        {
            Payment paymentObj = new Payment();
            Debug.WriteLine($"cart id is {cartId}");
            Cart cartObj = _context.Cart.FirstOrDefault(u => u.Id == cartId);
            Debug.WriteLine($"cart value {cartObj.Id}, {cartObj.TotalAmount}");
            paymentObj.Amount = cartObj.TotalAmount;
            Debug.WriteLine($"Payment values are {paymentObj.Amount}");
            _context.Add(paymentObj);
            _context.SaveChanges();

            EnrollmentController enrollment = new EnrollmentController(_context);
            bool IsEnrolled = enrollment.UpdateEnrollment(cartId, paymentObj.Id);



            return View("~/Views/Student/PaymentSuccess.cshtml");
        }

    }


}
