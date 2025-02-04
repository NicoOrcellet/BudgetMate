using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetMate.Models;

public partial class MoneyTransaction
{
    public int TransactionId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public bool IsIncome { get; set; }
    public DateOnly TransactionDate { get; set; }
    public int CategoryId { get; set; }

    public string? TransactionDescription { get; set; }

    public virtual Category Category { get; set; } = null!;
}
