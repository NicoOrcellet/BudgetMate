using BudgetMate.Models;
using BudgetMate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetMate.Services
{
    public class MoneyTransactionService
    {
        private readonly MoneyManagerContext _context;
        private readonly CategoryService _categoryService;
        
        public MoneyTransactionService(MoneyManagerContext context, CategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        //All transactions
        public async Task<List<MoneyTransaction>> GetAllTransactionsFrom(int id)
        {
            var transactions = await _context.MoneyTransactions.Include(t => t.Category).ToListAsync();
            return transactions;
        }

        public async Task<List<Category>> GetAllCategoriesFrom(int id)
        {
            var transactions = await GetAllTransactionsFrom(id);
            var categories = new List<Category>();
            foreach (var transiction in transactions)
            {
                if (!categories.Contains(transiction.Category))
                {
                    categories.Add(transiction.Category);
                }
            }
            return categories;
        }

        public async Task<decimal> GetTotalIncomeFrom(int id)
        {
            var incomeAmount = 0m;
            List<MoneyTransaction> transactions = await GetAllTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == true)
                {
                    incomeAmount += transaction.Amount;
                }
            }
            return incomeAmount;
        }

        public async Task<decimal> GetTotalExpenseFrom(int id)
        {
            var extenseAmount = 0m;
            List<MoneyTransaction> transactions = await GetAllTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == false)
                {
                    extenseAmount += transaction.Amount;
                }
            }
            return extenseAmount;
        }

        //Transactions from current Month
        public async Task<List<MoneyTransaction>> GetActualMonthTransactionsFrom(int id)
        {
            var transactions = await GetAllTransactionsFrom(id);
            var filteredTransactions = new List<MoneyTransaction>();
            foreach (var transaction in transactions)
            {
                if (transaction.TransactionDate.Month == DateTime.Now.Month &&
                    transaction.TransactionDate.Year == DateTime.Now.Year)
                {
                    filteredTransactions.Add(transaction);
                }

            }
            return filteredTransactions;
        }
        public async Task<decimal> GetActualMonthIncomeFrom(int id)
        {
            var incomeAmount = 0m;
            List<MoneyTransaction> transactions = await GetActualMonthTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == true)
                {
                    incomeAmount += transaction.Amount;
                }
            }
            return incomeAmount;
        }

        public async Task<decimal> GetActualMonthExpenseFrom(int id)
        {
            var extenseAmount = 0m;
            List<MoneyTransaction> transactions = await GetActualMonthTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == false)
                {
                    extenseAmount += transaction.Amount;
                }
            }
            return extenseAmount;
        }

        //Transactions from current Month
        public async Task<List<MoneyTransaction>> GetActualWeekTransactionsFrom(int id)
        {
            var transactions = await GetAllTransactionsFrom(id);
            var today = DateOnly.FromDateTime(DateTime.Now);
            var weekStart = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
            var weekEnd = weekStart.AddDays(6);
            var filteredTransactions = new List<MoneyTransaction>();
            foreach (var transaction in transactions)
            {
                if (transaction.TransactionDate >= weekStart && transaction.TransactionDate <= weekEnd)
                {
                    filteredTransactions.Add(transaction);
                }

            }
            return filteredTransactions;
        }
        public async Task<decimal> GetActualWeekIncomeFrom(int id)
        {
            var incomeAmount = 0m;
            List<MoneyTransaction> transactions = await GetActualWeekTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == true)
                {
                    incomeAmount += transaction.Amount;
                }
            }
            return incomeAmount;
        }

        public async Task<decimal> GetActualWeekExpenseFrom(int id)
        {
            var extenseAmount = 0m;
            List<MoneyTransaction> transactions = await GetActualWeekTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == false)
                {
                    extenseAmount += transaction.Amount;
                }
            }
            return extenseAmount;
        }

        //Last transactions

        public async Task<List<MoneyTransaction>> GetLastTransactions(int id, int count)
        {
            var transactions = await GetAllTransactionsFrom(id);
            var sortedTransactions = transactions.OrderByDescending(t => t.TransactionDate).ToList();
            var lastTransactions = sortedTransactions.Take(count).ToList();
            return lastTransactions;
        }

        //Filtering
        public async Task<FilterViewModel> GetFilteredTransactions(string? searchingMethod, DateTime? startingDate, DateTime? endingDate, decimal? minAmount, decimal? maxAmount, string? categorySelected)
        {
            var transactions = await GetAllTransactionsFrom(0);
            var filteredIncomes = new List<MoneyTransaction>();
            var filteredExpenses = new List<MoneyTransaction>();
            var viewModel = new FilterViewModel { };
            if (searchingMethod == "date")
            {
                DateOnly firstDate = DateOnly.MinValue;
                if (startingDate.HasValue)
                {
                    firstDate = DateOnly.FromDateTime(startingDate.Value);
                }
                DateOnly lastDate = DateOnly.FromDateTime(DateTime.Now.Date);
                if (endingDate.HasValue)
                {
                    lastDate = DateOnly.FromDateTime(endingDate.Value);
                }
                foreach (var t in transactions)
                {
                    if ((t.TransactionDate >= firstDate) && (t.TransactionDate <= lastDate))
                    {
                        if (t.IsIncome)
                        {
                            filteredIncomes.Add(t);
                        }
                        else
                        {
                            filteredExpenses.Add(t);
                        }
                    }
                }
                viewModel = new FilterViewModel
                {
                    searchingMethod = searchingMethod,
                    startingDate = firstDate.ToString("yyyy-MM-dd") == "0001-01-01" ? null : firstDate.ToString("yyyy-MM-dd"),
                    endingDate = lastDate.ToString("yyyy-MM-dd"),
                    minAmount = null,
                    maxAmount = null,
                    incomeList = filteredIncomes,
                    expenseList = filteredExpenses,
                };
            }
            else if (searchingMethod == "category")
            {
                foreach (var t in transactions)
                {
                    if (t.Category.CategoryName == categorySelected)
                    {
                        if (t.IsIncome)
                        {
                            filteredIncomes.Add(t);
                        }
                        else
                        {
                            filteredExpenses.Add(t);
                        }
                    }
                }
                viewModel = new FilterViewModel
                {
                    searchingMethod = searchingMethod,
                    startingDate = null,
                    endingDate = null,
                    minAmount = null,
                    maxAmount = null,
                    category = categorySelected,
                    incomeList = filteredIncomes,
                    expenseList = filteredExpenses,
                };
            }
            else if (searchingMethod == "amount")
            {
                minAmount = minAmount == null ? 0m : minAmount;
                maxAmount = maxAmount == null ? 999999999m : maxAmount;
                foreach (var t in transactions)
                {
                    if ((t.Amount >= minAmount) && (t.Amount <= maxAmount))
                    {
                        if (t.IsIncome)
                        {
                            filteredIncomes.Add(t);
                        }
                        else
                        {
                            filteredExpenses.Add(t);
                        }
                    }
                }
                viewModel = new FilterViewModel
                {
                    searchingMethod = searchingMethod,
                    startingDate = null,
                    endingDate = null,
                    minAmount = minAmount,
                    maxAmount = maxAmount == 999999999m ? null : maxAmount,
                    incomeList = filteredIncomes,
                    expenseList = filteredExpenses,
                };
            }
            else
            {
                foreach (var t in transactions)
                {
                    if (t.IsIncome)
                    {
                        filteredIncomes.Add(t);
                    }
                    else
                    {
                        filteredExpenses.Add(t);
                    }
                }

                viewModel = new FilterViewModel
                {
                    searchingMethod = searchingMethod,
                    startingDate = null,
                    endingDate = null,
                    minAmount = null,
                    maxAmount = null,
                    incomeList = filteredIncomes,
                    expenseList = filteredExpenses,
                };
            }

            viewModel.actualCategories = await GetAllCategoriesFrom(0);
            viewModel.allCategories = await _categoryService.GetAllCategories();

            return viewModel;
        }

        //Create

        public void CreateTransaction(DateTime addedDate, decimal addedAmount, int addedCategory, string? addedDescription, string transactionType)
        {
            var moneyTransaction = new MoneyTransaction
            {
                Amount = addedAmount,
                TransactionDate = DateOnly.FromDateTime(addedDate),
                UserId = 0,
                TransactionDescription = addedDescription,
                CategoryId = addedCategory,
                IsIncome = transactionType == "income",
            };
            _context.Add(moneyTransaction);
            _context.SaveChanges();
        }

    };
}
