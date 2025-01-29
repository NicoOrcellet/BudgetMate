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

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

    }
}
