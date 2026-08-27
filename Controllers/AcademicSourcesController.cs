using Microsoft.AspNetCore.Mvc;
using ResearchDesk.Models;

namespace ResearchDesk.Controllers
{
    public class AcademicSourcesController : Controller
    {
        private readonly ResearchDeskDbContext _context;

        public AcademicSourcesController(ResearchDeskDbContext context) => _context = context;

        public IActionResult Index() => View(_context.AcademicSources.ToList());

        public IActionResult Create(int? assignmentId, string returnUrl)
        {
            var model = new AcademicSource { AssignmentId = assignmentId ?? 0 };
            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AcademicSource source, string returnUrl)
        {
            if (source.AssignmentId <= 0 || !_context.Assignments.Any(a => a.Id == source.AssignmentId))
            {
                ModelState.AddModelError("AssignmentId", "You must select a valid assignment before saving an academic source.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(source);
                _context.SaveChanges();
                TempData["msg"] = "✅ Academic source added!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                if (source.AssignmentId > 0)
                    return RedirectToAction("Details", "Assignments", new { id = source.AssignmentId });

                return RedirectToAction("Index");
            }

            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(source);
        }

        public IActionResult Edit(int id, string returnUrl)
        {
            var source = _context.AcademicSources.Find(id);
            if (source == null) return NotFound();
            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(source);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AcademicSource source, string returnUrl)
        {
            if (id != source.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(source);
                _context.SaveChanges();
                TempData["msg"] = "✅ Academic source updated!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                if (source.AssignmentId > 0)
                    return RedirectToAction("Details", "Assignments", new { id = source.AssignmentId });

                return RedirectToAction("Index");
            }

            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(source);
        }

        public IActionResult Delete(int id, string returnUrl)
        {
            var source = _context.AcademicSources.Find(id);
            if (source == null) return NotFound();
            ViewBag.ReturnUrl = returnUrl;
            return View(source);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var source = _context.AcademicSources.Find(id);
            int assignmentId = source?.AssignmentId ?? 0;

            if (source != null)
            {
                _context.AcademicSources.Remove(source);
                _context.SaveChanges();
                TempData["msg"] = "✅ Academic source deleted!";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (assignmentId > 0)
                return RedirectToAction("Details", "Assignments", new { id = assignmentId });

            return RedirectToAction("Index");
        }
    }
}