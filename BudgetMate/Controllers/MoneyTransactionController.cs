using BudgetMate.Models;
using BudgetMate.Services;
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

        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionService.GetAllTransactionsFrom(0);
            return View(transactions);
        }

    }
}
