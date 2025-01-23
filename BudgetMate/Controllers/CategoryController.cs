using BudgetMate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BudgetMate.Controllers
{
    public class CategoryController : Controller
    {
        private readonly MoneyManagerContext _context;

        public CategoryController(MoneyManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

    }
}
