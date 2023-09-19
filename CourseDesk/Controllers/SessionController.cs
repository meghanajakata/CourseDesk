using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CourseDesk.Data;
using CourseDesk.Models;
using CourseDesk.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CourseDesk.Controllers
{
    public class SessionController : Controller
    {
        private readonly DbConnection _context;
        private readonly IUserRepository userRepository;
        private readonly IEnrollmentRepository enrollmentRepository;
        public SessionController(DbConnection context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("Auth");
        }

        public IActionResult SignUp()
        {
            return View("Auth");
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {

            User userObj = userRepository.GetUserByUsername(user);
            if (userObj == null)
            {
                Debug.WriteLine(user.Email);
                userRepository.AddUser(user);
                HttpContext.Session.SetInt32("user_id", user.Id);
                ViewData["success"] = "Registration is Successful...\n Please login";
                return View("Auth");

            }

            ViewData["error"] = "User already exists";
            
            return View("Auth",user);
        }

        public IActionResult Login()
        {
            return View("Auth");
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            Debug.WriteLine($"user {user.Username}, {user.Password}");
            User userObj = userRepository.GetUserByUsernameAndPassword(user);

            if (userObj != null)
            {
                HttpContext.Session.SetInt32("user_id", userObj.Id);
                HttpContext.Session.SetString("usertype",userObj.User_type);
                switch(userRepository.GetUserType(user))
                {
                    case "Instructor":
                        return RedirectToAction(nameof(Instructor));

                    case "Student":
                        return RedirectToAction(nameof(Student));

                    default:
                        return RedirectToAction("Index", "Home");

                }

            }
            ViewData["error"] = "Username or Password does not exist";
            return View("Auth",user);
        }

        public IActionResult Instructor()
        {
            return View(@"Views\Instructor\dashboard.cshtml");
        }

        public IActionResult Student()
        {
            int student_id = (int)HttpContext.Session.GetInt32("user_id");

            var enrolled_courses = enrollmentRepository.GetEnrolledCourses(student_id);
            return View(@"Views\Student\Welcome.cshtml", enrolled_courses);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
