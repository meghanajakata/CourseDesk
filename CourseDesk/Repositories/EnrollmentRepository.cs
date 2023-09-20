using CourseDesk.Models;
using CourseDesk.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CourseDesk.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DbConnection _context;
        public EnrollmentRepository(DbConnection context)
        {
            _context = context;
        }

        public List<Enrollment> GetEnrolledCourses(int id)
        {
            return _context.Enrollment.Include(u => u.Course).Where(u => u.UserId == id).ToList();
        }
        
    }
}
