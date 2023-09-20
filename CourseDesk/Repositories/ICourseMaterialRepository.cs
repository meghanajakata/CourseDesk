using CourseDesk.Models;

namespace CourseDesk.Repositories
{
    public interface ICourseMaterialRepository
    {
        public void AddCourseMaterial(CourseMaterial course);
        public IEnumerable<CourseMaterial> GetCoursesByUserId(int userId);
        public IEnumerable<CourseMaterial> GetCoursesByCategory(int categoryId);
        public IEnumerable<CourseMaterial> GetCoursesNotEnrolledByStudent(int studentId);
        public IEnumerable<CourseMaterial> GetAllCourses();
    }
}
