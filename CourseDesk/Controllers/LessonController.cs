using Microsoft.AspNetCore.Mvc;
using CourseDesk.Models;
using CourseDesk.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CourseDesk.Controllers
{
    public class LessonController : Controller
    {
        private readonly DbConnection _context;

        public LessonController(DbConnection context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadVideo(int? id)
        {
            if (HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            CourseMaterial course = _context.CoursesMaterials.FirstOrDefault(c => c.Id == id);
            ViewData["Course"] = course.Title;
            return View(@"Views\Instructor\UploadVideo.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> UploadVideo(IFormFile Video_Url, string? Description, string? course)
        {
            if(HttpContext.Session.GetInt32("user_id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int instructor_id = (int)HttpContext.Session.GetInt32("user_id");
            Debug.WriteLine($"Course name is {course}");
            Debug.WriteLine($"Course name is {Description}");

            Debug.WriteLine($"In Upload video method.. The values are {Video_Url.FileName}");

            Lesson lesson = new Lesson();
            
            if(lesson != null && Video_Url != null)
            {
                try
                {
                    var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/lessons");

                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Video_Url.FileName);

                    // Combine the directory and file name to get the full path
                    var filePath = Path.Combine(uploadDirectory, fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Video_Url.CopyToAsync(stream);
                    }

                    lesson.CourseId = _context.CoursesMaterials.Where(c => c.Title == course).Select(c => c.Id).FirstOrDefault();
                    lesson.Video_Url = "/lessons/" + fileName;
                    lesson.Description = Description;

                    Debug.WriteLine($"The values for the uploaded video is {lesson.CourseId} {lesson.Video_Url} {lesson.Duration}");
                    Debug.WriteLine($"Lesson Values are {lesson.Id} {lesson.CourseId} {lesson.Video_Url}");
                }

                catch(Exception e)
                {
                    Debug.WriteLine($"Exception in path {e.Message.ToString()}");
                }

                Debug.WriteLine($"Lesson Values are {lesson.Id} {lesson.CourseId} {lesson.Video_Url}");
                _context.Lesson.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lessons), new { id = lesson.CourseId });

            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Lessons(int? id)
        {
            CourseMaterial course = _context.CoursesMaterials.Include(u => u.Lessons).FirstOrDefault(u => u.Id == id);
            var courseContent = _context.Lesson.Include(u => u.Course).Where(u => u.CourseId == id).ToList();

            return View(@"Views/Instructor/Lessons.cshtml",course);
            //return View(@"Views/Instructor/Lessons.cshtml",courseContent);
        }
    }
}
