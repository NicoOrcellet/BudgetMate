using System;
using System.Collections.Generic;

namespace BudgetMate.Models;

public partial class SavingLimit
{
    public int SavingLimitId { get; set; }

    public string Period { get; set; } = null!;

    public decimal Amount { get; set; }

    public int UserId { get; set; }

    public DateOnly StartDate { get; set; }
}
