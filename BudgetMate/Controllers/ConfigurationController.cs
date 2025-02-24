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
        public IActionResult Index()
        {
            var viewModel = new ConfigurationViewModel();
            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    viewModel = _configurationService.GetLimitsAndExpensesFrom(userId);
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado";
                }
            }
            ViewData["Title"] = "Configuración";
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(string periodValue, string limitAmount)
        {
            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    _configurationService.CreateSavingLimit(periodValue, limitAmount, userId);
                }
                catch (ArgumentException err)
                {
                    TempData["ErrorMessage"] = err.Message;
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado";
                }
            }
            TempData["Period"] = periodValue == "Semanal" ? "week" : "month";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Modify(int limitId, string periodValue, string limitAmount)
        {
            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    _configurationService.ModifySavingLimit(limitId, periodValue, limitAmount, userId);
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
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int eraseLimitId)
        {
            string? userId = Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    _configurationService.DeleteSavingLimit(eraseLimitId, userId);
                }
                catch (ArgumentException err)
                {
                    TempData["ErrorMessage"] = err.Message;
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado";
                }
            }
            return RedirectToAction("Index");
        }

    }
}
