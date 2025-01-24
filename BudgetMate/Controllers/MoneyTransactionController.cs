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
        public async Task<IActionResult> Index(string? searchingMethod, DateTime? firstDateFilter, DateTime? lastDateFilter, decimal? minAmountFilter, decimal? maxAmountFilter, string? categoryFilter)
        {
            var transactions = await _transactionService.GetAllTransactionsFrom(0);
            var filteredTransactions = new List<MoneyTransaction>();
            var viewModel = new FilterViewModel { };
            if (searchingMethod == "date")
            {
                DateOnly firstDate = DateOnly.FromDateTime(firstDateFilter.Value);
                DateOnly lastDate = DateOnly.FromDateTime(lastDateFilter.Value);
                foreach (var t in transactions)
                {
                    if ((t.TransactionDate >= firstDate) && (t.TransactionDate <= lastDate))
                    {
                        filteredTransactions.Add(t);
                    }
                }
                viewModel = new FilterViewModel
                {
                    searchingMethod = searchingMethod,
                    firstDate = DateOnly.FromDateTime(firstDateFilter.Value).ToString("yyyy-MM-dd"),
                    lastDate = DateOnly.FromDateTime(lastDateFilter.Value).ToString("yyyy-MM-dd"),
                    minAmount = null,
                    maxAmount = null,
                    transactionList = filteredTransactions,
                    categoryList = await _transactionService.GetAllCategoriesFrom(0),
                };
            }
            else if (searchingMethod == "category")
            {
                foreach (var t in transactions)
                {
                    if (t.Category.CategoryName == categoryFilter)
                    {
                        filteredTransactions.Add(t);
                    }
                }
                viewModel = new FilterViewModel
                {
                    searchingMethod = searchingMethod,
                    firstDate = null,
                    lastDate = null,
                    minAmount = null,
                    maxAmount = null,
                    category = categoryFilter,
                    transactionList = filteredTransactions,
                    categoryList = await _transactionService.GetAllCategoriesFrom(0),
                };
            }
            else if (minAmountFilter != null)
            {
                foreach (var t in transactions)
                {
                    if ((t.Amount >= minAmountFilter) && (t.Amount <= maxAmountFilter))
                    {
                        filteredTransactions.Add(t);
                    }
                }
                viewModel = new FilterViewModel
                {
                    searchingMethod = searchingMethod,
                    firstDate = null,
                    lastDate = null,
                    minAmount = minAmountFilter,
                    maxAmount = maxAmountFilter,
                    transactionList = filteredTransactions,
                    categoryList = await _transactionService.GetAllCategoriesFrom(0),
                };
            }
            else
            {
                viewModel = new FilterViewModel
                {
                    searchingMethod = searchingMethod,
                    firstDate = null,
                    lastDate = null,
                    minAmount = null,
                    maxAmount = null,
                    transactionList = transactions,
                    categoryList = await _transactionService.GetAllCategoriesFrom(0),
                };
            }

            return View(viewModel);
        }


    }
}
