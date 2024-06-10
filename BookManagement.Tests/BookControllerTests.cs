using BooksManagementSystem.Controllers;
using BooksManagementSystem.DAL.Authors;
using BooksManagementSystem.DAL.Books;
using BooksManagementSystem.Models;
using Moq;

namespace BookManagement.Tests
{
    public class BookControllerTests
    {
        [Fact]
        public async void Index_Test_NoBooksFound_NotFound_result()
        {
            var bookRepo = new Mock<IBooksDataRepository>();

            var authorsRepo = new Mock<IAuthorsDataRepository>();
            BookController bookController = new BookController(bookRepo.Object, authorsRepo.Object);

            var result = await bookController.Index();
            Assert.NotNull(result);
            Assert.Equal(404, ((Microsoft.AspNetCore.Mvc.StatusCodeResult)result).StatusCode);
        }

        [Fact]
        public async void Index_Test_BooksFound_ResultOK()
        {
            var bookRepo = new Mock<IBooksDataRepository>();
            var authorsRepo = new Mock<IAuthorsDataRepository>();

            bookRepo.Setup(t => t.GetAllBooksFullInfo()).Returns(Task.FromResult<IQueryable<BookViewModel>>((new List<BookViewModel> { new BookViewModel { } }).AsQueryable<BookViewModel>()));



            BookController bookController = new BookController(bookRepo.Object, authorsRepo.Object);

            var result = await bookController.Index();
            Assert.NotNull(result);
            Assert.NotNull(((Microsoft.AspNetCore.Mvc.ViewResult)result).Model);
        }

        [Fact]
        public async void Details_Test_NoBooksFound_NotFound_result()
        {
            var bookRepo = new Mock<IBooksDataRepository>();

            var authorsRepo = new Mock<IAuthorsDataRepository>();
            BookController bookController = new BookController(bookRepo.Object, authorsRepo.Object);

            var result = await bookController.Details(1);
            Assert.NotNull(result);
            Assert.Equal(404, ((Microsoft.AspNetCore.Mvc.StatusCodeResult)result).StatusCode);
        }

        [Fact]
        public async void Details_Test_BookFound_AuthorFound_ResultOK()
        {
            var bookRepo = new Mock<IBooksDataRepository>();
            var authorsRepo = new Mock<IAuthorsDataRepository>();

            bookRepo.Setup(t => t.GetDetails(It.IsAny<int>())).Returns(Task.FromResult<BookViewModel>((new BookViewModel { Id = 1, AuthorId = 1 })));

            authorsRepo.Setup(t => t.GetDetails(It.IsAny<int>())).Returns(Task.FromResult<AuthorViewModel>((new AuthorViewModel { Id = 1, AuthorFirstname = "Aaaa", AuthorSecondname = "Bbbb" })));


            BookController bookController = new BookController(bookRepo.Object, authorsRepo.Object);

            var result = await bookController.Details(1);
            Assert.NotNull(result);
            Assert.NotNull(((Microsoft.AspNetCore.Mvc.ViewResult)result).Model);

            Assert.NotNull(bookController.ViewData["Author"]);
            Assert.Equal("Aaaa Bbbb", bookController.ViewData["Author"]);
        }

        [Fact]
        public async void Edit_Test_BookFound_AuthorFound_ResultOK()
        {
            var bookRepo = new Mock<IBooksDataRepository>();
            var authorsRepo = new Mock<IAuthorsDataRepository>();

            bookRepo.Setup(t => t.GetNotTracking(It.IsAny<int>())).Returns(Task.FromResult<BookViewModel>((new BookViewModel { Id = 1, AuthorId = 1 })));

            authorsRepo.Setup(t => t.GetAll()).Returns((IOrderedQueryable<AuthorViewModel>)new List<AuthorViewModel>() { new AuthorViewModel { AuthorSecondname = "Aaaa", AuthorFirstname = "Bbbb" } }.AsQueryable<AuthorViewModel>());


            BookController bookController = new BookController(bookRepo.Object, authorsRepo.Object);

            var result = await bookController.Edit(1);
            Assert.NotNull(result);
            Assert.NotNull(((Microsoft.AspNetCore.Mvc.ViewResult)result).Model);
            Assert.NotNull( bookController.ViewData["Authors"]);

        }

    }
}