using CourseDesk.Models;
using CourseDesk.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CourseDesk.Repositories
{
    public class CourseMaterialRepository : ICourseMaterialRepository
    {
        private readonly DbConnection _context;
        public CourseMaterialRepository(DbConnection context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds and saves the course object to database
        /// </summary>
        /// <param name="course"></param>
        public void AddCourseMaterial(CourseMaterial course)
        {
            _context.CoursesMaterials.Add(course);
            _context.SaveChanges();
        }

        /// <summary>
        /// Returns the courseMaterial with respect to its Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseMaterial GetCourseById(int id)
        {
            return null;
        }

        /// <summary>
        /// Returns list of courses with respect to userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<CourseMaterial> GetCoursesByUserId(int userId) 
        {
            var courses = _context.CoursesMaterials.Include(u => u.CourseCategory).Where(u => u.PersonId == userId).ToList();
            return courses;
        }

        /// <summary>
        /// Returns the courses with respect to category id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IEnumerable<CourseMaterial> GetCoursesByCategory(int categoryId)
        {
            var courses = _context.CoursesMaterials.Where(u => u.CategoryId == categoryId).ToList();
            return courses;
        }

        /// <summary>
        /// Returns the list of courses not enrolled by student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IEnumerable<CourseMaterial> GetCoursesNotEnrolledByStudent(int studentId)
        {
            // Get the IDs of courses that the student is enrolled in
            var enrolledCourseIds = _context.Enrollment
                .Where(e => e.UserId == studentId)
                .Select(e => e.CourseId)
                .ToList();

            // Retrieve the courses that the student is not enrolled in
            var notEnrolledCourses = _context.CoursesMaterials
                .Where(cm => !enrolledCourseIds.Contains(cm.Id))
                .ToList();

            return notEnrolledCourses;
        }

        /// <summary>
        /// Returns all the courses from the table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CourseMaterial> GetAllCourses()
        {
            var courses = _context.CoursesMaterials.Include(u => u.CourseCategory).ToList();
            return courses;
        }


        
    }
}
