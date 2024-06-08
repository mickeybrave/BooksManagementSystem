using BooksManagementSystem.Data;
using BooksManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.Controllers
{
    public class AuthorController : Controller
    {
        private readonly BooksManagementSystemContext _context;

        public AuthorController(BooksManagementSystemContext context)
        {
            _context = context;
        }

        // GET: AuthorController
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_context.Authors);
        }

        // GET: AuthorController/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorFirstname,AuthorSecondname")]
                    AuthorViewModel author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(author);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "posible reason is " + ex.Message);
            }
            return View(author);
        }

        // GET: AuthorController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (id != author.Id)
            {
                return NotFound();
            }

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorFirstname,AuthorSecondname")]
                    AuthorViewModel author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "posible reason is " + ex.Message);
                }
            }
            return View(author);
        }

        // GET: AuthorController/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(author);
        }

        // POST: AuthorController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var authorToDelete = new AuthorViewModel() { Id = id };
                _context.Entry(authorToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
