﻿using BooksManagementSystem.DAL.Authors;
using BooksManagementSystem.DAL.Books;
using BooksManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly IBooksDataRepository _booksDataRepository;
        private readonly IAuthorsDataRepository _authorsDataRepository;

        public BookController(IBooksDataRepository booksDataRepository, IAuthorsDataRepository authorsDataRepository)
        {
            this._booksDataRepository = booksDataRepository;
            this._authorsDataRepository = authorsDataRepository;
        }

        //   GET: BookController
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
            var allBooksFullInfo = await _booksDataRepository.GetAllBooksFullInfo();
            if (allBooksFullInfo == null)
            {
                return NotFound();
            }
            return View(allBooksFullInfo);
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _booksDataRepository.GetDetails(id);
            if (book == null)
            {
                return NotFound();
            }
            await InitAuthor(book);

            return View(book);
        }

        private async Task InitAuthor(BookViewModel book)
        {
            var author = await _authorsDataRepository.GetDetails(book.AuthorId);
            ViewData["Author"] = author.AuthorFirstname + " " + author.AuthorSecondname;
        }

        // GET: BookController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            InitiateAuthors();
            return View();
        }

        private void InitiateAuthors()
        {
            ViewData["Authors"] = _authorsDataRepository.GetAll().Select(s => new SelectListItem
            {
                Text = s.AuthorFirstname + " " + s.AuthorSecondname,
                Value = s.Id.ToString()
            });
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,AuthorId,ISBN,Category,Description,IsAvailable")]
                    BookViewModel book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _booksDataRepository.Create(book);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "posible reason is " + ex.Message);
            }
            InitiateAuthors();
            return View(book);
        }

        // GET: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _booksDataRepository.GetNotTracking(id);

            if (book == null)
            {
                return NotFound();

            }
            if (id != book.Id)
            {
                return NotFound();
            }

           
            InitiateAuthors();
            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorId,ISBN,Category,Description,IsAvailable")]
                    BookViewModel book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _booksDataRepository.Edit(book);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "posible reason is " + ex.Message);
                }
            }
            InitiateAuthors();
            return View(book);
        }

        // GET: BookController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, bool? saveChangesError = false)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var book = await _booksDataRepository.GetNotTracking(id);
            if (book == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            await InitAuthor(book);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _booksDataRepository.Delete(id);
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
