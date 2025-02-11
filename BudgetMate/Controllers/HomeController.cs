using BudgetMate.Models;
using BudgetMate.Services;
using BudgetMate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BudgetMate.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;

        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new TotalTransactionsViewModel();
            viewModel.lastMoneyTransactions = new List<MoneyTransaction>();
            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    viewModel = await _homeService.GetTotalTransactions(userId);
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado";
                }
            }
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
