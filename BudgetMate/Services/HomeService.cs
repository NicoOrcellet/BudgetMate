using BudgetMate.Models;
using BudgetMate.ViewModels;

namespace BudgetMate.Services
{
    public class HomeService
    {
        private readonly MoneyManagerContext _context;
        private readonly MoneyTransactionService _moneyTransactionService;
        private readonly SavingLimitService _savingLimitService;
        public HomeService(MoneyManagerContext context, MoneyTransactionService moneyTransactionService, SavingLimitService savingLimitService)
        {
            _context = context;
            _moneyTransactionService = moneyTransactionService;
            _savingLimitService = savingLimitService;
        }

        public TotalTransactionsViewModel GetTotalTransactions(string userId)
        {
            var viewModel = new TotalTransactionsViewModel();
            if (userId == null)
            {
                viewModel = new TotalTransactionsViewModel
                {
                    totalIncome = 0,
                    totalExpense = 0,
                    monthIncome = 0,
                    monthExpense = 0,
                    weekIncome = 0,
                    weekExpense = 0,
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
                totalIncome =  _moneyTransactionService.GetTotalIncomeFrom(id),
                totalExpense =  _moneyTransactionService.GetTotalExpenseFrom(id),
                monthIncome =  _moneyTransactionService.GetActualMonthIncomeFrom(id),
                monthExpense =  _moneyTransactionService.GetActualMonthExpenseFrom(id),
                weekIncome =  _moneyTransactionService.GetActualWeekIncomeFrom(id),
                weekExpense =  _moneyTransactionService.GetActualWeekExpenseFrom(id),
                lastMoneyTransactions =  _moneyTransactionService.GetLastTransactions(id, 5) ?? new List<MoneyTransaction>(),
                monthLimit = _savingLimitService.GetSavingLimitsFrom(id)[0]?.Amount,
                weekLimit = _savingLimitService.GetSavingLimitsFrom(id)[1]?.Amount,
            };
            return viewModel;
        }

    }
}
