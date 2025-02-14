using BudgetMate.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetMate.Services
{
    public class CategoryService
    {
        private readonly MoneyManagerContext _context;
        public CategoryService(MoneyManagerContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }

        public bool ExistsCategory(int id)
        {
            return _context.Categories.Any(c => c.CategoryId == id);
        }

    }
}
