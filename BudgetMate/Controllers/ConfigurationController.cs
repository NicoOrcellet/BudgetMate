using BudgetMate.Models;
using BudgetMate.Services;
using BudgetMate.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BudgetMate.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly ConfigurationService _configurationService;
        public ConfigurationController(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new ConfigurationViewModel
            {
                monthSavingLimit = _configurationService.GetSavingLimitsFrom(0)[0],
                weekSavingLimit = _configurationService.GetSavingLimitsFrom(0)[1],
                monthExpense = await _configurationService.GetActualMonthExpenseFrom(0),
                weekExpense = await _configurationService.GetActualWeekExpenseFrom(0),
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(string periodValue, string limitAmount)
        {
            try
            {
                _configurationService.CreateSavingLimit(periodValue, limitAmount);
            }
            catch (ArgumentException err)
            {
                TempData["ErrorMessage"] = err.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado";
            }
            TempData["Period"] = periodValue == "Semanal" ? "week" : "month";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Modify(int limitId, string periodValue, string limitAmount)
        {
            try
            {
                _configurationService.ModifySavingLimit(limitId, periodValue, limitAmount);
            }
            catch (ArgumentException err)
            {
                TempData["ErrorMessage"] = err.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado";
            }
            TempData["Period"] = periodValue == "Mensual" ? "month" : "week";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int eraseLimitId)
        {
            try
            {
                _configurationService.DeleteSavingLimit(eraseLimitId);
            }
            catch (ArgumentException err)
            {
                TempData["ErrorMessage"] = err.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado";
            }
            return RedirectToAction("Index");
        }

    }
}
