using BooksManagementSystem.DAL.Accounts;
using BooksManagementSystem.DAL.Books;
using BooksManagementSystem.DAL.Borrowing;
using BooksManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BooksManagementSystem.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly IBorrowingDataRepository _borrowingDataRepository;
        private readonly IBooksDataRepository _booksDataRepository;
        private readonly IAccountsRepository _accountsRepository;

        public BorrowingController(IBorrowingDataRepository borrowingDataRepository,
            IBooksDataRepository booksDataRepository,
            IAccountsRepository accountsRepository)
        {
            _borrowingDataRepository = borrowingDataRepository;
            _booksDataRepository = booksDataRepository;
            _accountsRepository = accountsRepository;
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
            var fullInfo = await _borrowingDataRepository.GetAllBorrowingFullInfo(User.IsInRole("Admin"), User.Identity.Name);
            if (fullInfo == null)
            {
                return NotFound();
            }
            return View(fullInfo);
        }

        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Details(int id)
        {
            var borrowing = await _borrowingDataRepository.GetDetails(id);
            if (borrowing == null)
            {
                return NotFound();
            }
            await InitBook(borrowing);
            await InitUser(borrowing);
            return View(borrowing);
        }

        private async Task InitiateBooks(int? bookId)
        {
            var allAvailableBooks = await _booksDataRepository.GetAllAvailableBooks(bookId);
            ViewData["Books"] = allAvailableBooks.Select(s => new SelectListItem
            {
                Text = s.Title,
                Value = s.Id.ToString()
            });
        }

        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Create()
        {
            await InitiateUsers();
            await InitiateBooks(null);
            var currentUser = await _accountsRepository.GetUsersByEmailAsync(User.Identity.Name);
            var borrowing = new BorrowingViewModel { UserId = currentUser.Id };
            await InitUser(borrowing);
            return View(borrowing);
        }

        private async Task InitiateUsers()
        {
            var allUsers = await _accountsRepository.GetUsersAsync();
            ViewData["Users"] = allUsers.Select(s => new SelectListItem
            {
                Text = s.Firstname + " " + s.LastName,
                Value = s.Id.ToString()
            });
        }

        private async Task InitBook(BorrowingViewModel borrowing)
        {
            var book = await _booksDataRepository.GetDetails(borrowing.BookId);
            ViewData["Book"] = book.Title;
        }

        private async Task InitUser(BorrowingViewModel borrowing)
        {
            var user = await _accountsRepository.GetUsersAsync(borrowing.UserId);
            ViewData["User"] = user.Firstname + " " + user.LastName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([Bind("Id,BookId,UserId,BorrowedDate,ReturnedDate")]
                    BorrowingViewModel borrowing)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var currentUser = await _accountsRepository.GetUsersByEmailAsync(User.Identity.Name);
                    borrowing.UserId = currentUser.Id;

                    borrowing.NotificationMessage = "Book borrowed successfully!";
                    borrowing.NotificationType = MessageNotificationType.Borrowed;
                    TempData["SuccessMessage"] = "Book borrowed successfully!";

                    await _borrowingDataRepository.Create(borrowing);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "posible reason is " + ex.Message);

                borrowing.NotificationMessage = ex.Message;
                borrowing.NotificationType = MessageNotificationType.Failure;
            }
            await InitiateUsers();
            await InitiateBooks(borrowing.BookId);
            await InitUser(borrowing);
            return View(borrowing);
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int id)
        {
            var borrowing = await _borrowingDataRepository.GetNotTracking(id);

            if (borrowing == null)
            {
                return NotFound();

            }
            if (id != borrowing.Id)
            {
                return NotFound();
            }

            await InitBook(borrowing);
            await InitUser(borrowing);
            await InitiateUsers();
            await InitiateBooks(borrowing.BookId);
            return View(borrowing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId,BorrowedDate,ReturnedDate")]
                    BorrowingViewModel borrowing)
        {
            if (id != borrowing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    borrowing.NotificationMessage = "Book returned successfully!";
                    borrowing.NotificationType = borrowing.ReturnedDate == null ? MessageNotificationType.Borrowed : MessageNotificationType.Returned;

                    TempData["SuccessMessage"] = $"Book {borrowing.NotificationType} successfully!";

                    await _borrowingDataRepository.Edit(borrowing);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "posible reason is " + ex.Message);
                    borrowing.NotificationMessage = ex.Message;
                    borrowing.NotificationType = MessageNotificationType.Failure;
                }
            }
            await InitBook(borrowing);
            await InitUser(borrowing);
            await InitiateUsers();
            await InitiateBooks(borrowing.BookId);
            return View(borrowing);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, bool? saveChangesError = false)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var borrowing = await _borrowingDataRepository.GetNotTracking(id);
            if (borrowing == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            await InitBook(borrowing);
            await InitUser(borrowing);
            return View(borrowing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _borrowingDataRepository.Delete(id);
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
