using BudgetMate.Models;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System.Globalization;

namespace BudgetMate.Services
{
    public class SavingLimitService
    {
        private readonly MoneyManagerContext _context;
        public SavingLimitService(MoneyManagerContext context)
        {
            _context = context;
        }

        public List<SavingLimit> GetSavingLimitsFrom(int id)
        {
            var limits = new List<SavingLimit>();
            SavingLimit? monthLimit = _context.SavingLimits.FirstOrDefault(l => l.UserId == id && l.Period == "M");
            SavingLimit? weekLimit = _context.SavingLimits.FirstOrDefault(l => l.UserId == id && l.Period == "W");
            limits.Add(monthLimit);
            limits.Add(weekLimit);
            return limits;
        }

        public SavingLimit validateSavingLimit(string periodValue, string limitAmount)
        {
            if (periodValue != "Mensual" && periodValue != "Semanal")
            {
                throw new ArgumentException("El período seleccionado no es válido");
            }
            if (Decimal.TryParse(limitAmount, CultureInfo.InvariantCulture, out decimal amount))
            {
                var model = new SavingLimit
                {
                    Amount = amount,
                    Period = periodValue == "Mensual" ? "M" : "W",
                };
                return model;
            }
            else
            {
                throw new ArgumentException("El monto ingresado no es valido");
            }
            
        }

        public void CreateSavingLimit(string periodValue, string limitAmount, string userId)
        {
            var model = validateSavingLimit(periodValue, limitAmount);
            if (!int.TryParse(userId, out var id))
            {
                throw new ArgumentException("El ID de usuario no es válido"); ;
            };
            model.UserId = id;
            model.StartDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Add(model);
            _context.SaveChanges();
        }

        public void ModifySavingLimit(int limitId, string periodValue, string limitAmount, string userId)
        {
            SavingLimit? savingLimit = _context.SavingLimits.FirstOrDefault(x => x.SavingLimitId == limitId);
            if (!int.TryParse(userId, out var id))
            {
                throw new Exception();
            };
            if (savingLimit == null)
            {
                throw new ArgumentException("No se pudo modificar la transacción");
            }
            var model = validateSavingLimit(periodValue, limitAmount);
            savingLimit.Amount = model.Amount;
            savingLimit.Period = model.Period;
            _context.SaveChanges();
        }

        public void DeleteSavingLimit(int eraseLimitId, string userId)
        {
            SavingLimit? savingLimit = _context.SavingLimits.FirstOrDefault(x => x.SavingLimitId == eraseLimitId);
            if (!int.TryParse(userId, out var id))
            {
                throw new Exception();
            };
            if (savingLimit == null)
            {
                throw new ArgumentException("No se pudo modificar la transacción");
            }
            _context.SavingLimits.Remove(savingLimit);
            _context.SaveChanges();
        }

    }
}
