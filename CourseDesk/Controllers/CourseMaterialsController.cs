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
using Microsoft.AspNetCore.Http;


namespace CourseDesk.Controllers
{
    public class CourseMaterialsController : Controller
    {
        private readonly DbConnection _context;

        public CourseMaterialsController(DbConnection context)
        {
            _context = context;
        }

        // GET: CourseMaterials
        public async Task<IActionResult> Index()
        {
            var dbConnection = _context.CoursesMaterials.Include(c => c.CourseCategory).Include(c => c.Person);
            return View(await dbConnection.ToListAsync());
        }

        [Authorization(UserType.Instructor)]
        public IActionResult InstructorCourses()
        {
            //if (HttpContext.Session.GetInt32("user_id") == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            int instructor_id = (int)HttpContext.Session.GetInt32("user_id");
            var instructorCourses = _context.CoursesMaterials.Where(u => u.PersonId == instructor_id).ToList();
            return View(@"Views\Instructor\Courses.cshtml", instructorCourses);
        }

        public IActionResult StudentCourses()
        {
            var courses = _context.CoursesMaterials.Include(c => c.CourseCategory).Include(c => c.Person);
            return View(@"Views\Student\Courses.cshtml",courses.ToList());
            
        }

        // GET: CourseMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CoursesMaterials == null)
            {
                return NotFound();
            }

            var courseMaterial = await _context.CoursesMaterials
                .Include(c => c.CourseCategory)
                .Include(c => c.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseMaterial == null)
            {
                return NotFound();
            }

            return View(courseMaterial);
        }

        // GET: CourseMaterials/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View(@"Views\Instructor\Create.cshtml");
        }

        // POST: CourseMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile ImageUrl, CourseMaterial courseMaterial)
        {
            if(courseMaterial != null && ImageUrl !=null)
            {
                if(HttpContext.Session.GetInt32("user_id") == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                int id = (int)HttpContext.Session.GetInt32("user_id");

                int instructor_id= _context.Users.Where(u => u.Id == id).Select(user => user.Id).FirstOrDefault();
                courseMaterial.PersonId = instructor_id;
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
                _context.Add(courseMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(InstructorCourses));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", courseMaterial.CategoryId);
            return View(@"Views\Instructor\Create.cshtml",courseMaterial);
        }

        // GET: CourseMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CoursesMaterials == null)
            {
                return NotFound();
            }

            var courseMaterial = await _context.CoursesMaterials.FindAsync(id);
            if (courseMaterial == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", courseMaterial.CategoryId);
            return View(courseMaterial);
        }

        // POST: CourseMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,Duration,Uploaded_at,PersonId,CategoryId")] CourseMaterial courseMaterial)
        {
            if (id != courseMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseMaterialExists(courseMaterial.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", courseMaterial.CategoryId);
            return View(courseMaterial);
        }

        // GET: CourseMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CoursesMaterials == null)
            {
                return NotFound();
            }

            var courseMaterial = await _context.CoursesMaterials
                .Include(c => c.CourseCategory)
                .Include(c => c.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseMaterial == null)
            {
                return NotFound();
            }

            return View(courseMaterial);
        }

        // POST: CourseMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CoursesMaterials == null)
            {
                return Problem("Entity set 'DbConnection.CoursesMaterials'  is null.");
            }
            var courseMaterial = await _context.CoursesMaterials.FindAsync(id);
            if (courseMaterial != null)
            {
                _context.CoursesMaterials.Remove(courseMaterial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseMaterialExists(int id)
        {
          return (_context.CoursesMaterials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
