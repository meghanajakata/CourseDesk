using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseDesk.Data;
using CourseDesk.Models;
using CourseDesk.Filters;
using CourseDesk.Repositories;
using Microsoft.AspNetCore.Http;


namespace CourseDesk.Controllers
{
    public class CourseMaterialsController : Controller
    {
        private readonly DbConnection _context;
        private readonly ICourseMaterialRepository _courseMaterialRepository;
        private readonly ICategoryRepository _categoryRepository;
        public CourseMaterialsController(ICourseMaterialRepository courseMaterialRepository , ICategoryRepository categoryRepository)
        {
            _courseMaterialRepository = courseMaterialRepository;
            _categoryRepository = categoryRepository;
        }


        // GET: CourseMaterials/Create
        [Authorization(UserType.Instructor)]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View(@"Views\Instructor\Create.cshtml");
        }

        // POST: CourseMaterials/Create      
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile ImageUrl, CourseMaterial courseMaterial)
        {
            if(courseMaterial != null && ImageUrl !=null)
            {
                int id = (int)HttpContext.Session.GetInt32("user_id");
                courseMaterial.PersonId = id;
                Debug.WriteLine("Course Details");

                try
                {
                    var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    // Ensure the directory exists; create it if not
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    // Generate a unique file name to avoid overwriting existing files
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);

                    // Combine the directory and file name to get the full path
                    var filePath = Path.Combine(uploadDirectory, fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }

                    // Set the ImageUrl property of your model to the path of the saved file
                    courseMaterial.ImageUrl = "/uploads/" + fileName;
                }   
                catch(Exception e)
                {
                    Debug.WriteLine($"Exception in path {e.Message.ToString()}");
                }
                
                Debug.WriteLine($"course details {courseMaterial.PersonId}, {courseMaterial.Title} ");
                _courseMaterialRepository.AddCourseMaterial(courseMaterial);

                return RedirectToAction(nameof(InstructorCourses));
            }
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name", courseMaterial.CategoryId);
            return View(@"Views\Instructor\Create.cshtml",courseMaterial);
        }

        /// <summary>
        /// Represents the courses with respect to particular user
        /// </summary>
        /// <returns></returns>
        [Authorization(UserType.Instructor)]
        public IActionResult InstructorCourses()
        {
            int instructor_id = (int)HttpContext.Session.GetInt32("user_id");
            var instructorCourses = _context.CoursesMaterials.Where(u => u.PersonId == instructor_id).ToList();
            return View(@"Views\Instructor\Courses.cshtml", instructorCourses);
        }

        /// <summary>
        /// Repreents the courses not enrolled by the student
        /// </summary>
        /// <returns></returns>
        public IActionResult StudentCourses()
        {
            int student_id = (int)HttpContext.Session.GetInt32("user_id");
            var courses = _courseMaterialRepository.GetCoursesNotEnrolledByStudent(student_id);
            return View(@"Views\Student\Courses.cshtml", courses.ToList());
        }

        /// <summary>
        /// Returns all the courses from the table
        /// </summary>
        /// <returns></returns>
        public IActionResult AllCourses()
        {
            var courses = _categoryRepository.GetAllCategories();
            return View(@"Views\Session\AllCourses.cshtml",courses);
        }

        public IActionResult GetByCategory(int id)
        {
            var courses = _courseMaterialRepository.GetCoursesByCategory(id);
            return View(courses);
        }


    }
}
