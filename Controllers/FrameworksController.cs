using Microsoft.AspNetCore.Mvc;
using ResearchDesk.Models;

namespace ResearchDesk.Controllers
{
    public class FrameworksController : Controller
    {
        private readonly ResearchDeskDbContext _context;
        public FrameworksController(ResearchDeskDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Frameworks.ToList());

        public IActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new Framework());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Framework framework, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(framework);
                _context.SaveChanges();
                TempData["msg"] = "✅ Framework added!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(framework);
        }

        public IActionResult Edit(int id, string returnUrl)
        {
            var framework = _context.Frameworks.Find(id);
            if (framework == null) return NotFound();
            ViewBag.ReturnUrl = returnUrl;
            return View(framework);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Framework framework, string returnUrl)
        {
            if (id != framework.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(framework);
                _context.SaveChanges();
                TempData["msg"] = "✅ Framework updated!";
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(framework);
        }

        public IActionResult Delete(int id, string returnUrl)
        {
            var framework = _context.Frameworks.Find(id);
            if (framework == null) return NotFound();
            ViewBag.ReturnUrl = returnUrl;
            return View(framework);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var framework = _context.Frameworks.Find(id);
            if (framework != null)
            {
                _context.Frameworks.Remove(framework);
                _context.SaveChanges();
                TempData["msg"] = "✅ Framework deleted!";
            }
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index");
        }
    }
}