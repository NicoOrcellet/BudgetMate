using BudgetMate.Models;

namespace BudgetMate.ViewModels
{
    public class FilterViewModel
    {
        public string? searchingMethod { get; set; }
        public string? firstDate {  get; set; }
        public string? lastDate { get; set; }
        public decimal? minAmount { get; set; }
        public decimal? maxAmount { get; set; }
        public string? category { get; set; }
        public List<MoneyTransaction> transactionList { get; set; }
        public List<Category> categoryList { get; set; }
    }
}
