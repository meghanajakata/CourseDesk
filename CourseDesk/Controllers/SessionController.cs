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
        private readonly IUserRepository _userRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public SessionController(IUserRepository userRepository,IEnrollmentRepository enrollmentRepository)
        {
            _userRepository = userRepository;
            _enrollmentRepository = enrollmentRepository;
        }
        public IActionResult Index()
        {
            return View("Auth");
        }

        public IActionResult SignUp()
        {
            return View("Auth");
        }

        /// <summary>
        /// Performs SignUp operation
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SignUp(User user)
        {

            User userObj = _userRepository.GetUserByUsername(user);
            if (userObj == null)
            {
                Debug.WriteLine(user.Email);
                _userRepository.AddUser(user);
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

        /// <summary>
        /// Performs Login Operation
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        
        [HttpPost]
        public IActionResult Login(User user)
        {
            Debug.WriteLine($"user {user.Username}, {user.Password}");
            User userObj = _userRepository.GetUserByUsernameAndPassword(user.Username, user.Password);
            if (userObj != null)
            {
                HttpContext.Session.SetInt32("user_id", userObj.Id);
                HttpContext.Session.SetString("user_type",userObj.User_type);
                switch(_userRepository.GetUserType(userObj))
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

        /// <summary>
        /// Performs Logout operation
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Instructor()
        {
            return View(@"Views\Instructor\dashboard.cshtml");
        }

        public IActionResult Student()
        {
            int student_id = (int)HttpContext.Session.GetInt32("user_id");

            var enrolled_courses = _enrollmentRepository.GetEnrolledCourses(student_id);
            return View(@"Views\Student\Welcome.cshtml", enrolled_courses);
        }
    }
}
