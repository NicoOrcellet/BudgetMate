using BudgetMate.Models;

namespace BudgetMate.ViewModels
{
    public class FilterViewModel
    {
        public string? searchingMethod { get; set; }
        public string? startingDate {  get; set; }
        public string? endingDate { get; set; }
        public decimal? minAmount { get; set; }
        public decimal? maxAmount { get; set; }
        public string? category { get; set; }
        public List<MoneyTransaction> incomeList { get; set; }
        public List<MoneyTransaction> expenseList { get; set; }
        public List<Category> actualCategories { get; set; }
        public List<Category> allCategories { get; set; }
    }
}
