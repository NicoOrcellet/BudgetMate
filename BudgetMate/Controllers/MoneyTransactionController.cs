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
            var viewModel = await _transactionService.GetFilteredTransactions(searchingMethod, startingDate, endingDate, minAmount, maxAmount, categorySelected);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(MoneyTransaction model, DateTime addedDate, string addedAmount, string transactionType)
        {
            try
            {
                model.IsIncome = transactionType == "income";
                _transactionService.CreateTransaction(model, addedDate, addedAmount, transactionType);
            }
            catch (ArgumentException err)
            {
                TempData["ErrorMessage"] = err.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Modify(int transactionId, MoneyTransaction model, DateTime addedDate, string addedAmount, string transactionType)
        {
            try
            {
                model.IsIncome = transactionType == "income";
                _transactionService.ModifyTransaction(transactionId, model, addedDate, addedAmount, transactionType);
            }
            catch (ArgumentException err)
            {
                TempData["ErrorMessage"] = err.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int transactionId)
        {
            try
            {
                _transactionService.DeleteTransaction(transactionId);
            }
            catch (ArgumentException err)
            {
                TempData["ErrorMessage"] = err.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado";
            }
            return RedirectToAction("Index");
        }

    };

}
