using CourseDesk.Models;
using CourseDesk.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CourseDesk.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbConnection _context;
        public CategoryRepository(DbConnection context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all the categories in the table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetAllCategories()
        {
            var categories = _context.Category.Include(u => u.Materials).ToList();
            return categories;
        }
    }
}
