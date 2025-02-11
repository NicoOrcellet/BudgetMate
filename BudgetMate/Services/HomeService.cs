using BudgetMate.Models;
using BudgetMate.ViewModels;

namespace BudgetMate.Services
{
    public class HomeService
    {
        private readonly MoneyManagerContext _context;
        private readonly MoneyTransactionService _moneyTransactionService;
        public HomeService(MoneyManagerContext context, MoneyTransactionService moneyTransactionService)
        {
            _context = context;
            _moneyTransactionService = moneyTransactionService;
        }

        public async Task<TotalTransactionsViewModel> GetTotalTransactions(string userId)
        {
            var viewModel = new TotalTransactionsViewModel();
            if (userId == null)
            {
                viewModel = new TotalTransactionsViewModel
                {
                    totalIncome = null,
                    totalExpense = null,
                    monthIncome = null,
                    monthExpense = null,
                    weekIncome = null,
                    weekExpense = null,
                    lastMoneyTransactions = new List<MoneyTransaction>(),
                };
                return viewModel;
            }
            if (!int.TryParse(userId, out var id))
            {
                throw new ArgumentException("El ID de usuario no es válido"); ;
            }
            viewModel = new TotalTransactionsViewModel
            {
                totalIncome = await _moneyTransactionService.GetTotalIncomeFrom(id),
                totalExpense = await _moneyTransactionService.GetTotalExpenseFrom(id),
                monthIncome = await _moneyTransactionService.GetActualMonthIncomeFrom(id),
                monthExpense = await _moneyTransactionService.GetActualMonthExpenseFrom(id),
                weekIncome = await _moneyTransactionService.GetActualWeekIncomeFrom(id),
                weekExpense = await _moneyTransactionService.GetActualWeekExpenseFrom(id),
                lastMoneyTransactions = await _moneyTransactionService.GetLastTransactions(id, 5) ?? new List<MoneyTransaction>(),
            };
            return viewModel;
        }

    }
}
