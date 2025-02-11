using BudgetMate.Models;
using BudgetMate.ViewModels;

namespace BudgetMate.Services
{
    public class ConfigurationService
    {
        private readonly MoneyManagerContext _context;
        private readonly SavingLimitService _savingLimitService;
        private readonly MoneyTransactionService _moneyTransactionService;
        public ConfigurationService(MoneyManagerContext context, SavingLimitService savingLimitService, MoneyTransactionService moneyTransactionService)
        {
            _context = context;
            _savingLimitService = savingLimitService;
            _moneyTransactionService = moneyTransactionService;
        }

        public async Task<ConfigurationViewModel> GetLimitsAndExpensesFrom(string userId)
        {
            if (userId == null)
            {
                return new ConfigurationViewModel
                {
                    monthSavingLimit = null,
                    weekSavingLimit = null,
                    monthExpense = null,
                    weekExpense = null,
                };
            }
            if (!int.TryParse(userId, out var id))
            {
                throw new ArgumentException("El ID de usuario no es válido"); ;
            };
            return new ConfigurationViewModel
            {
                monthSavingLimit = _savingLimitService.GetSavingLimitsFrom(id)[0],
                weekSavingLimit = _savingLimitService.GetSavingLimitsFrom(id)[1],
                monthExpense = await _moneyTransactionService.GetActualMonthExpenseFrom(id),
                weekExpense = await _moneyTransactionService.GetActualWeekExpenseFrom(id),
            };
        }

        public void CreateSavingLimit(string periodValue, string limitAmount, string userId)
        {
            _savingLimitService.CreateSavingLimit(periodValue, limitAmount, userId);
        }

        public void ModifySavingLimit(int limitId, string periodValue, string limitAmount, string userId)
        {
            _savingLimitService.ModifySavingLimit(limitId, periodValue, limitAmount, userId);
        }

        public void DeleteSavingLimit(int eraseLimitId, string userId)
        {
            _savingLimitService.DeleteSavingLimit(eraseLimitId, userId);
        }

    }
}
