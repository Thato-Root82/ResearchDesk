using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResearchDesk.Models;

namespace ResearchDesk.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly ResearchDeskDbContext _context;

        public AssignmentsController(ResearchDeskDbContext context)
            => _context = context;

        public IActionResult Index()
        {
            return View(_context.Assignments.ToList());
        }

       
        public async Task<IActionResult> Details(int id, bool includeFrameworks = false)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Notes)
                .Include(a => a.AISources)
                .Include(a => a.AcademicSources)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
                return NotFound();

            // Pass to view whether to include frameworks
            ViewBag.IncludeFrameworks = includeFrameworks;

            // Get all available frameworks (for optional inclusion in references)
            ViewBag.AllFrameworks = await _context.Frameworks.ToListAsync();

            // Return the view with assignment data
            return View(assignment);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignment);
                _context.SaveChanges();

                TempData["msg"] = "Assignment added successfully!";
                return RedirectToAction("Index");
            }

            return View(assignment);
        }

        public IActionResult Edit(int id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment == null)
                return NotFound();

            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Assignment assignment)
        {
            if (id != assignment.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(assignment);
                _context.SaveChanges();

                TempData["msg"] = "Assignment updated!";
                return RedirectToAction("Index");
            }

            return View(assignment);
        }

        public IActionResult Delete(int id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment == null)
                return NotFound();

            return View(assignment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                _context.SaveChanges();
                TempData["msg"] = "Assignment deleted.";
            }

            return RedirectToAction("Index");
        }
    }
}