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

        public IActionResult Index()
        {
            var viewModel = new TotalTransactionsViewModel();
            viewModel.lastMoneyTransactions = new List<MoneyTransaction>();
            string? userId = Request.Cookies["userId"];
            viewModel = _homeService.GetTotalTransactions(userId);
            ViewData["Title"] = "Inicio";
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
