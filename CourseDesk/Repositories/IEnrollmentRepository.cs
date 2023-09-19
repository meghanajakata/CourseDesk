using CourseDesk.Models;

namespace CourseDesk.Repositories
{
    public interface IEnrollmentRepository
    {
        public List<Enrollment> GetEnrolledCourses(int id);
    }
}
