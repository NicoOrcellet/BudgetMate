using BudgetMate.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetMate.Services
{
    public class MoneyTransactionService
    {
        private readonly MoneyManagerContext _context;
        
        public MoneyTransactionService(MoneyManagerContext context)
        {
            _context = context;
        }

        //All transactions
        public async Task<List<MoneyTransaction>> GetAllTransactionsFrom(int id)
        {
            var transactions = await _context.MoneyTransactions.Include(t => t.Category).ToListAsync();
            return transactions;
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

    };
}
