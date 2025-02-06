using BudgetMate.Models;
using BudgetMate.Services;
using BudgetMate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BudgetMate.Controllers
{
    public class HomeController : Controller
    {
        private readonly MoneyTransactionService _moneyTransactionService;

        public HomeController(MoneyTransactionService transactionService)
        {
            _moneyTransactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            var totalIncome = await _moneyTransactionService.GetTotalIncomeFrom(0);
            var totalExpense = await _moneyTransactionService.GetTotalExpenseFrom(0);
            var monthIncome = await _moneyTransactionService.GetActualMonthIncomeFrom(0);
            var monthExpense = await _moneyTransactionService.GetActualMonthExpenseFrom(0);
            var weekIncome = await _moneyTransactionService.GetActualWeekIncomeFrom(0);
            var weekExpense = await _moneyTransactionService.GetActualWeekExpenseFrom(0);
            var lastTransactions = await _moneyTransactionService.GetLastTransactions(0,5);
            var viewModel = new TotalTransactionsViewModel
            {
                totalIncome = totalIncome,
                totalExpense = totalExpense,
                monthIncome = monthIncome,
                monthExpense = monthExpense,
                weekIncome = weekIncome,
                weekExpense = weekExpense,
                lastMoneyTransactions = lastTransactions,
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
