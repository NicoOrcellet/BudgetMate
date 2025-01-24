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
            return View();
        }

    }
}
