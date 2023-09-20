using CourseDesk.Models;

namespace CourseDesk.Repositories
{
    public interface ICategoryRepository
    {
        public IEnumerable<Category> GetAllCategories();
    }
}
