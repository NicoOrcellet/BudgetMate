using BudgetMate.Models;
using Microsoft.Identity.Client;

namespace BudgetMate.ViewModels
{
    public class ConfigurationViewModel
    {
        public SavingLimit? monthSavingLimit { get; set; }
        public SavingLimit? weekSavingLimit { get; set; }
        public decimal? monthExpense {  get; set; }
        public decimal? weekExpense { get; set; }
    }
}
