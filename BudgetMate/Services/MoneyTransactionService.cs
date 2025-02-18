using BudgetMate.Models;
using BudgetMate.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
        public List<MoneyTransaction> GetAllTransactionsFrom(int id)
        {
            var transactions = _context.MoneyTransactions.Include(t => t.Category).Where(t => t.UserId == id).ToList();
            return transactions;
        }

        public List<Category> GetAllCategoriesFrom(int id)
        {
            var transactions = GetAllTransactionsFrom(id);
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

        public decimal GetTotalIncomeFrom(int id)
        {
            var incomeAmount = 0m;
            List<MoneyTransaction> transactions = GetAllTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == true)
                {
                    incomeAmount += transaction.Amount;
                }
            }
            return incomeAmount;
        }

        public decimal GetTotalExpenseFrom(int id)
        {
            var extenseAmount = 0m;
            List<MoneyTransaction> transactions = GetAllTransactionsFrom(id);
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
        public List<MoneyTransaction> GetActualMonthTransactionsFrom(int id)
        {
            var transactions = GetAllTransactionsFrom(id);
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
        public decimal GetActualMonthIncomeFrom(int id)
        {
            var incomeAmount = 0m;
            List<MoneyTransaction> transactions = GetActualMonthTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == true)
                {
                    incomeAmount += transaction.Amount;
                }
            }
            return incomeAmount;
        }

        public decimal GetActualMonthExpenseFrom(int id)
        {
            var extenseAmount = 0m;
            List<MoneyTransaction> transactions = GetActualMonthTransactionsFrom(id);
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
        public List<MoneyTransaction> GetActualWeekTransactionsFrom(int id)
        {
            var transactions = GetAllTransactionsFrom(id);
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
        public decimal GetActualWeekIncomeFrom(int id)
        {
            var incomeAmount = 0m;
            List<MoneyTransaction> transactions = GetActualWeekTransactionsFrom(id);
            foreach (var transaction in transactions)
            {
                if (transaction.IsIncome == true)
                {
                    incomeAmount += transaction.Amount;
                }
            }
            return incomeAmount;
        }

        public decimal GetActualWeekExpenseFrom(int id)
        {
            var extenseAmount = 0m;
            List<MoneyTransaction> transactions = GetActualWeekTransactionsFrom(id);
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

        public List<MoneyTransaction> GetLastTransactions(int id, int count)
        {
            var transactions = GetAllTransactionsFrom(id);
            var sortedTransactions = transactions.OrderByDescending(t => t.TransactionDate).ToList();
            var lastTransactions = sortedTransactions.Take(count).ToList();
            return lastTransactions;
        }

        //Filtering
        public FilterViewModel GetFilteredTransactions(string? searchingMethod, DateTime? startingDate, DateTime? endingDate, decimal? minAmount, decimal? maxAmount, string? categorySelected, string userId)
        {
            var viewModel = new FilterViewModel { };
            var filteredIncomes = new List<MoneyTransaction>();
            var filteredExpenses = new List<MoneyTransaction>();
            if (userId == null)
            {
                viewModel = new FilterViewModel
                {
                    searchingMethod = null,
                    startingDate = null,
                    endingDate = null,
                    minAmount = null,
                    maxAmount = null,
                    incomeList = filteredIncomes,
                    expenseList = filteredExpenses,
                    actualCategories = new List<Category>(),
                    allCategories = new List<Category>(),
                };
                return viewModel;
            }
            if (!int.TryParse(userId, out var id))
            {
                throw new Exception();
            }
            var transactions = GetAllTransactionsFrom(id);
            switch (searchingMethod)
            {
                case "date":
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
                    filteredIncomes = transactions.Where(t => t.TransactionDate >= firstDate && t.TransactionDate <= lastDate && t.IsIncome).ToList();
                    filteredExpenses = transactions.Where(t => t.TransactionDate >= firstDate && t.TransactionDate <= lastDate && !t.IsIncome).ToList();
                    viewModel = new FilterViewModel
                    {
                        startingDate = firstDate.ToString("yyyy-MM-dd") == "0001-01-01" ? null : firstDate.ToString("yyyy-MM-dd"),
                        endingDate = lastDate.ToString("yyyy-MM-dd"),
                        minAmount = null,
                        maxAmount = null,
                    };
                    break;
                case "category":
                    filteredIncomes = transactions.Where(t => t.Category.CategoryName == categorySelected && t.IsIncome).ToList();
                    filteredExpenses = transactions.Where(t => t.Category.CategoryName == categorySelected && !t.IsIncome).ToList();
                    viewModel = new FilterViewModel
                    {
                        startingDate = null,
                        endingDate = null,
                        minAmount = null,
                        maxAmount = null,
                        category = categorySelected,
                    };
                    break;
                case "amount":
                    minAmount = minAmount == null ? 0m : minAmount;
                    maxAmount = maxAmount == null ? 999999999m : maxAmount;
                    filteredIncomes = transactions.Where(t => t.Amount >= minAmount && t.Amount <= maxAmount && t.IsIncome).ToList();
                    filteredExpenses = transactions.Where(t => t.Amount >= minAmount && t.Amount <= maxAmount && !t.IsIncome).ToList();
                    viewModel = new FilterViewModel
                    {
                        startingDate = null,
                        endingDate = null,
                        minAmount = minAmount == 0m ? null : minAmount,
                        maxAmount = maxAmount == 999999999m ? null : maxAmount,
                    };
                    break;
                default:
                    filteredIncomes = transactions.Where(t => t.IsIncome).ToList();
                    filteredExpenses = transactions.Where(t => !t.IsIncome).ToList();
                    viewModel = new FilterViewModel
                    {
                        startingDate = null,
                        endingDate = null,
                        minAmount = null,
                        maxAmount = null,
                    };
                    break;
            };
            viewModel.actualCategories = GetAllCategoriesFrom(id);
            viewModel.allCategories = _categoryService.GetAllCategories();
            viewModel.searchingMethod = searchingMethod;
            viewModel.incomeList = filteredIncomes.OrderByDescending(t => t.TransactionDate).ToList();
            viewModel.expenseList = filteredExpenses.OrderByDescending(t => t.TransactionDate).ToList();
            return viewModel;
        }

        //CUD

        public MoneyTransaction validateTransactionInfo(MoneyTransaction model, DateTime addedDate, string addedAmount, string transactionType, string userId)
        {
            if (!_categoryService.ExistsCategory(model.CategoryId))
            {
                throw new ArgumentException("La categoría no existe");
            }
            if (transactionType != "income" && transactionType != "expense")
            {
                throw new ArgumentException("El tipo de transacción no existe");
            }
            if (!int.TryParse(userId, out var id))
            {
                throw new ArgumentException("El ID de usuario no es válido"); ;
            }
            var transactionDate = DateOnly.FromDateTime(addedDate);
            if (transactionDate == DateOnly.MinValue)
            {
                throw new ArgumentException("La fecha ingresada es inválida");
            }
            model.TransactionDate = transactionDate;
            if (Decimal.TryParse(addedAmount, CultureInfo.InvariantCulture, out decimal amount))
            {
                model.Amount = amount;
                model.UserId = id;
                return model;
            }
            else
            {
                throw new ArgumentException("El monto ingresado no es valido");
            }
        }

        public void CreateTransaction(MoneyTransaction model, DateTime addedDate, string addedAmount, string transactionType, string userId)
        {
            validateTransactionInfo(model, addedDate, addedAmount, transactionType, userId);
            model.IsIncome = transactionType == "income";
            _context.Add(model);
            _context.SaveChanges();
        }

        public void ModifyTransaction(int transactionId, MoneyTransaction model, DateTime addedDate, string addedAmount, string transactionType, string userId)
        {
            validateTransactionInfo(model, addedDate, addedAmount, transactionType, userId);
            var transaction = _context.MoneyTransactions.First(t => t.TransactionId == transactionId && t.UserId == model.UserId);
            if (transaction != null)
            {
                transaction.TransactionDate = model.TransactionDate;
                transaction.Amount = model.Amount;
                transaction.CategoryId = model.CategoryId;
                transaction.TransactionDescription = model.TransactionDescription;
                transaction.IsIncome = transactionType == "income";
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("No se pudo modificar la transacción");
            }
        }

        public void DeleteTransaction(int transactionId, string userId)
        {
            if (!int.TryParse(userId, out var id))
            {
                throw new Exception();
            }
            MoneyTransaction? transaction = _context.MoneyTransactions.FirstOrDefault(t => t.TransactionId == transactionId && t.UserId == id);
            if (transaction != null)
            {
                _context.MoneyTransactions.Remove(transaction);
                _context.SaveChanges();
            } else
            {
                throw new ArgumentException("No se pudo eliminar la transacción");
            }
        }

    };
}
