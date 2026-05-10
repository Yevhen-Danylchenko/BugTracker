using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly BugService _bugService;

        public HomeController(BugService bugService)
        {
            _bugService = bugService;
        }

        // DASHBOARD
        public async Task<IActionResult> Index(string? status, string? priority, string? assignedTo)
        {
            StatusEnum? statusEnum = null;
            PriorityEnum? priorityEnum = null;

            if (!string.IsNullOrEmpty(status) && Enum.TryParse(status, out StatusEnum s))
                statusEnum = s;

            if (!string.IsNullOrEmpty(priority) && Enum.TryParse(priority, out PriorityEnum p))
                priorityEnum = p;

            // Використовуємо метод із BugService з фільтрами
            var bugs = await _bugService.GetFilteredAsync(statusEnum, priorityEnum, assignedTo);

            var statsByStatus = bugs
                .GroupBy(b => b.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            var criticalCount = bugs.Count(b => b.Priority == PriorityEnum.Critical);

            var lastChanges = bugs
                .OrderByDescending(b => b.UpdatedAt)
                .Take(5)
                .ToList();

            var model = new DashboardViewModel
            {
                StatsByStatus = statsByStatus,
                CriticalCount = criticalCount,
                LastChanges = lastChanges
            };

            return View(model);
        }


        // CREATE
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Bug bug)
        {
            if (ModelState.IsValid)
            {
                bug.CreatedAt = DateTime.UtcNow;
                bug.UpdatedAt = DateTime.UtcNow;
                await _bugService.AddAsync(bug);
                return RedirectToAction(nameof(Index));
            }
            return View(bug);
        }

        // EDIT
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var bug = (await _bugService.GetAllAsync()).FirstOrDefault(b => b.Id == id);
            if (bug == null) return NotFound();
            return View(bug);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Bug bug)
        {
            if (ModelState.IsValid)
            {
                bug.UpdatedAt = DateTime.UtcNow;
                await _bugService.UpdateAsync(bug);
                return RedirectToAction(nameof(Index));
            }
            return View(bug);
        }

        // DELETE
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bugService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // CHANGE STATUS
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, StatusEnum status)
        {
            var bug = (await _bugService.GetAllAsync()).FirstOrDefault(b => b.Id == id);
            if (bug == null) return NotFound();

            bug.Status = status;
            bug.UpdatedAt = DateTime.UtcNow;
            await _bugService.UpdateAsync(bug);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

