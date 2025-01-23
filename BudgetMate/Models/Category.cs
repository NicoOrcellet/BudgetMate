using System;
using System.Collections.Generic;

namespace BudgetMate.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<MoneyTransaction> MoneyTransactions { get; set; } = new List<MoneyTransaction>();
}
