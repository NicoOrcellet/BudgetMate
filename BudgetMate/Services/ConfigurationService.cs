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

        public List<SavingLimit> GetSavingLimitsFrom(int id)
        {
            return _savingLimitService.GetSavingLimitsFrom(id);
        }

        public async Task<decimal> GetActualMonthExpenseFrom(int id)
        {
            return await _moneyTransactionService.GetActualMonthExpenseFrom(id);
        }

        public async Task<decimal> GetActualWeekExpenseFrom(int id)
        {
            return await _moneyTransactionService.GetActualWeekExpenseFrom(id);
        }

        public void CreateSavingLimit(string periodValue, string limitAmount)
        {
            _savingLimitService.CreateSavingLimit(periodValue, limitAmount);
        }

        public void ModifySavingLimit(int limitId, string periodValue, string limitAmount)
        {
            _savingLimitService.ModifySavingLimit(limitId, periodValue, limitAmount);
        }

        public void DeleteSavingLimit(int eraseLimitId)
        {
            _savingLimitService.DeleteSavingLimit(eraseLimitId);
        }

    }
}
