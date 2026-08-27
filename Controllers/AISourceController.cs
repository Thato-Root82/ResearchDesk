using Microsoft.AspNetCore.Mvc;
using ResearchDesk.Models;

namespace ResearchDesk.Controllers
{
    public class AISourcesController : Controller
    {
        private readonly ResearchDeskDbContext _context;

        public AISourcesController(ResearchDeskDbContext context) => _context = context;

        public IActionResult Index() => View(_context.AISources.ToList());

        public IActionResult Create(int? assignmentId, string returnUrl)
        {
            var model = new AISource { AssignmentId = assignmentId ?? 0 };
            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AISource aiSource, string returnUrl)
        {
            if (aiSource.AssignmentId <= 0 || !_context.Assignments.Any(a => a.Id == aiSource.AssignmentId))
            {
                ModelState.AddModelError("AssignmentId", "You must select a valid assignment before saving an AI source.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(aiSource);
                _context.SaveChanges();
                TempData["msg"] = "✅ AI Source added!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                if (aiSource.AssignmentId > 0)
                    return RedirectToAction("Details", "Assignments", new { id = aiSource.AssignmentId });

                return RedirectToAction("Index");
            }

            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(aiSource);
        }

        public IActionResult Edit(int id, string returnUrl)
        {
            var aiSource = _context.AISources.Find(id);
            if (aiSource == null) return NotFound();
            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(aiSource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AISource aiSource, string returnUrl)
        {
            if (id != aiSource.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(aiSource);
                _context.SaveChanges();
                TempData["msg"] = "✅ AI Source updated!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                if (aiSource.AssignmentId > 0)
                    return RedirectToAction("Details", "Assignments", new { id = aiSource.AssignmentId });

                return RedirectToAction("Index");
            }

            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(aiSource);
        }

        public IActionResult Delete(int id, string returnUrl)
        {
            var aiSource = _context.AISources.Find(id);
            if (aiSource == null) return NotFound();
            ViewBag.ReturnUrl = returnUrl;
            return View(aiSource);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var aiSource = _context.AISources.Find(id);
            int assignmentId = aiSource?.AssignmentId ?? 0;

            if (aiSource != null)
            {
                _context.AISources.Remove(aiSource);
                _context.SaveChanges();
                TempData["msg"] = "✅ AI Source deleted!";
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (assignmentId > 0)
                return RedirectToAction("Details", "Assignments", new { id = assignmentId });

            return RedirectToAction("Index");
        }
    }
}