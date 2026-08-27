using Microsoft.AspNetCore.Mvc;
using ResearchDesk.Models;

namespace ResearchDesk.Controllers
{
    public class NotesController : Controller
    {
        private readonly ResearchDeskDbContext _context;
        public NotesController(ResearchDeskDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Notes.ToList());

        public IActionResult Create(int? assignmentId, string returnUrl)
        {
            var model = new Note { AssignmentId = assignmentId ?? 0 };
            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Note note, string returnUrl)
        {
            note.CreatedAt = DateTime.Now;

            if (note.AssignmentId <= 0 || !_context.Assignments.Any(a => a.Id == note.AssignmentId))
            {
                ModelState.AddModelError("AssignmentId", "You must select an assignment before saving a note.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(note);
                _context.SaveChanges();
                TempData["msg"] = "✅ Note added!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                if (note.AssignmentId > 0)
                    return RedirectToAction("Details", "Assignments", new { id = note.AssignmentId });
                return RedirectToAction("Index");
            }

            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(note);
        }

        public IActionResult Edit(int id, string returnUrl)
        {
            var note = _context.Notes.Find(id);
            if (note == null) return NotFound();
            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Note note, string returnUrl)
        {
            if (id != note.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(note);
                _context.SaveChanges();
                TempData["msg"] = "✅ Note updated!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                if (note.AssignmentId > 0)
                    return RedirectToAction("Details", "Assignments", new { id = note.AssignmentId });
                return RedirectToAction("Index");
            }
            ViewBag.Assignments = _context.Assignments.ToList();
            ViewBag.ReturnUrl = returnUrl;
            return View(note);
        }

        public IActionResult Delete(int id, string returnUrl)
        {
            var note = _context.Notes.Find(id);
            if (note == null) return NotFound();
            ViewBag.ReturnUrl = returnUrl;
            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var note = _context.Notes.Find(id);
            int assignmentId = note?.AssignmentId ?? 0;
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
                TempData["msg"] = "✅ Note deleted!";
            }
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            if (assignmentId > 0)
                return RedirectToAction("Details", "Assignments", new { id = assignmentId });
            return RedirectToAction("Index");
        }
    }
}