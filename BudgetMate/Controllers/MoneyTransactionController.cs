using BudgetMate.Models;
using BudgetMate.Services;
using BudgetMate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BudgetMate.Controllers
{
    public class MoneyTransactionController : Controller
    {
        private readonly MoneyManagerContext _context;
        private readonly MoneyTransactionService _transactionService;
        public MoneyTransactionController(MoneyManagerContext context, MoneyTransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchingMethod, DateTime? startingDate, DateTime? endingDate, decimal? minAmount, decimal? maxAmount, string? categorySelected)
        {
            var viewModel = await _transactionService.GetFilteredTransactions(searchingMethod, startingDate, endingDate, minAmount, maxAmount, categorySelected);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(DateTime addedDate, decimal addedAmount, int addedCategory, string? addedDescription, string transactionType)
        {
            _transactionService.CreateTransaction(addedDate, addedAmount, addedCategory, addedDescription, transactionType);
            return RedirectToAction("Index");
        }

    };

}
