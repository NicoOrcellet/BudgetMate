using BudgetMate.Models;

namespace BudgetMate.ViewModels
{
    public class TotalTransactionsViewModel
    {
        public decimal totalIncome { get; set; }
        public decimal totalExpense { get; set; }
        public decimal monthIncome { get; set; }
        public decimal monthExpense { get; set; }
        public decimal weekIncome { get; set; }
        public decimal weekExpense { get; set; }
        public List<MoneyTransaction> lastMoneyTransactions { get; set; }
    }
}
