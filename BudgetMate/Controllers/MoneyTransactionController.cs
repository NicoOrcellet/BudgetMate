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
        public async Task<IActionResult> Index(int transictionId, string? searchingMethod, DateTime? startingDate, DateTime? endingDate, decimal? minAmount, decimal? maxAmount, string? categorySelected)
        {
            var viewModel = new FilterViewModel();
            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    viewModel = await _transactionService.GetFilteredTransactions(searchingMethod, startingDate, endingDate, minAmount, maxAmount, categorySelected, userId);
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado";
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(MoneyTransaction model, DateTime addedDate, string addedAmount, string transactionType)
        {

            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    _transactionService.CreateTransaction(model, addedDate, addedAmount, transactionType, userId);
                }
                catch (ArgumentException err)
                {
                    TempData["ErrorMessage"] = err.Message;
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Modify(int transactionId, MoneyTransaction model, DateTime addedDate, string addedAmount, string transactionType)
        {
            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    model.IsIncome = transactionType == "income";
                    _transactionService.ModifyTransaction(transactionId, model, addedDate, addedAmount, transactionType, userId);
                }
                catch (ArgumentException err)
                {
                    TempData["ErrorMessage"] = err.Message;
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int transactionId)
        {
            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    _transactionService.DeleteTransaction(transactionId, userId);
                }
                catch (ArgumentException err)
                {
                    TempData["ErrorMessage"] = err.Message;
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado";
                }
            }
            return RedirectToAction("Index");
        }

    };

}
