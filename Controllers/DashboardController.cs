using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResearchDesk.Models;

namespace ResearchDesk.Controllers
{
    public class DashboardController : Controller
    {
       
        private readonly ResearchDeskDbContext _context;

        public DashboardController(ResearchDeskDbContext context)
            => _context = context;  // Dependency injection 


        public IActionResult Index()
        {
            // Get upcoming assignments (not completed, due date is today or later)
            var upcoming = _context.Assignments
                .Where(a => !a.IsCompleted && a.DueDate >= DateTime.Today)
                .OrderBy(a => a.DueDate)
                .Take(5)
                .ToList();

            // Get the 5 most recent notes
            var recentNotes = _context.Notes
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToList();

            ViewBag.Upcoming = upcoming;
            ViewBag.RecentNotes = recentNotes;
            ViewBag.TotalAssignments = _context.Assignments.Count(); 

            return View();
        }
    }
}