using BooksManagementSystem.DAL.Authors;
using BooksManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorsDataRepository _repository;

        public AuthorController(IAuthorsDataRepository repository)
        {
            this._repository = repository;
        }

        // GET: AuthorController
        [Authorize(Roles = "Admin,User")]
        public ActionResult Index()
        {
            return View(_repository.GetAll());
        }

        // GET: AuthorController/Details/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int id)
        {
            var author = await _repository.GetDetails(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // GET: AuthorController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,AuthorFirstname,AuthorSecondname")]
                    AuthorViewModel author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.Create(author);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _repository.GetNotTracking(id);

            if (author == null)
            {
                return NotFound();
            }

            if (id != author.Id)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
                    await _repository.Edit(author);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _repository.GetNotTracking(id.GetValueOrDefault());
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repository.Delete(id);
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
