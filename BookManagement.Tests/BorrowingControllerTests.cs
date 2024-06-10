using BooksManagementSystem.Areas.Identity.Data;
using BooksManagementSystem.Controllers;
using BooksManagementSystem.DAL.Accounts;
using BooksManagementSystem.DAL.Authors;
using BooksManagementSystem.DAL.Books;
using BooksManagementSystem.DAL.Borrowing;
using BooksManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace BookManagement.Tests
{
    public class BorrowingControllerTests
    {
        [Fact]
        public async void Index_Test_NoBooksFound_NotFound_result()
        {
            var bookRepo = new Mock<IBooksDataRepository>();
            var borrowingRepo = new Mock<IBorrowingDataRepository>();
            var accountRepo = new Mock<IAccountsRepository>();

            BorrowingController borrowingController = new BorrowingController(borrowingRepo.Object, bookRepo.Object, accountRepo.Object);

            GenericIdentity myIdentity = new GenericIdentity("apiTestUser");
            myIdentity.AddClaims(new List<Claim>
            {
                 new Claim("", "")
            }
            );

            var user = new ClaimsPrincipal(myIdentity);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            borrowingController.ControllerContext = context;

            var result = await borrowingController.Index();
            Assert.NotNull(result);
            Assert.Equal(404, ((Microsoft.AspNetCore.Mvc.StatusCodeResult)result).StatusCode);
        }

        [Fact]
        public async void Index_Test_BooksFound_ResultOK()
        {
            var bookRepo = new Mock<IBooksDataRepository>();
            var borrowingRepo = new Mock<IBorrowingDataRepository>();
            var accountRepo = new Mock<IAccountsRepository>();

            bookRepo.Setup(t => t.GetAllBooksFullInfo()).Returns(Task.FromResult<IQueryable<BookViewModel>>((new List<BookViewModel> { new BookViewModel { } }).AsQueryable<BookViewModel>()));

            borrowingRepo.Setup(t => t.GetAllBorrowingFullInfo(It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IQueryable<BorrowingViewModel>>((new List<BorrowingViewModel> {
                    new BorrowingViewModel
                    {
                    }
                }).AsQueryable<BorrowingViewModel>()));


            BorrowingController borrowingController = new BorrowingController(borrowingRepo.Object, bookRepo.Object, accountRepo.Object);

            GenericIdentity myIdentity = new GenericIdentity("apiTestUser");
            myIdentity.AddClaims(new List<Claim>
            {
                 new Claim("", "")
            }
            );

            var user = new ClaimsPrincipal(myIdentity);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            borrowingController.ControllerContext = context;

            var result = await borrowingController.Index();
            Assert.NotNull(result);
            Assert.NotNull(((Microsoft.AspNetCore.Mvc.ViewResult)result).Model);
        }

        [Fact]
        public async void Details_Test_NoBorrowingDataFound_NotFound_result()
        {
            var bookRepo = new Mock<IBooksDataRepository>();
            var borrowingRepo = new Mock<IBorrowingDataRepository>();
            var accountRepo = new Mock<IAccountsRepository>();

            bookRepo.Setup(t => t.GetAllBooksFullInfo()).Returns(Task.FromResult<IQueryable<BookViewModel>>((new List<BookViewModel> { new BookViewModel { } }).AsQueryable<BookViewModel>()));

            borrowingRepo.Setup(t => t.GetAllBorrowingFullInfo(It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IQueryable<BorrowingViewModel>>((new List<BorrowingViewModel> {
                    new BorrowingViewModel
                    {
                    }
                }).AsQueryable<BorrowingViewModel>()));


            BorrowingController borrowingController = new BorrowingController(borrowingRepo.Object, bookRepo.Object, accountRepo.Object);

            GenericIdentity myIdentity = new GenericIdentity("apiTestUser");
            myIdentity.AddClaims(new List<Claim>
            {
                 new Claim("", "")
            }
            );

            var user = new ClaimsPrincipal(myIdentity);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            borrowingController.ControllerContext = context;

            var result = await borrowingController.Details(1);
            Assert.NotNull(result);
            Assert.Equal(404, ((Microsoft.AspNetCore.Mvc.StatusCodeResult)result).StatusCode);
        }

        [Fact]
        public async void Details_Test_InitBook_InitUser_ResultOK()
        {
            var bookRepo = new Mock<IBooksDataRepository>();
            var borrowingRepo = new Mock<IBorrowingDataRepository>();
            var accountRepo = new Mock<IAccountsRepository>();

            bookRepo.Setup(t => t.GetAllBooksFullInfo()).Returns(Task.FromResult<IQueryable<BookViewModel>>((new List<BookViewModel> { new BookViewModel { } }).AsQueryable<BookViewModel>()));
            bookRepo.Setup(t => t.GetDetails(It.IsAny<int>())).Returns(Task.FromResult<BookViewModel>(new BookViewModel { Title = "Boook" }));

            borrowingRepo.Setup(t => t.GetDetails(It.IsAny<int>()))
                .Returns(Task.FromResult<BorrowingViewModel>((
                    new BorrowingViewModel
                    {
                    })));


            accountRepo.Setup(t => t.GetUsersAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<BooksManagementSystemUser>((
                    new BooksManagementSystemUser
                    {
                        Firstname = "Aaaa",
                        LastName = "Bbbb"
                    })));

            BorrowingController borrowingController = new BorrowingController(borrowingRepo.Object, bookRepo.Object, accountRepo.Object);

            GenericIdentity myIdentity = new GenericIdentity("apiTestUser");
            myIdentity.AddClaims(new List<Claim>
            {
                 new Claim("", "")
            }
            );

            var user = new ClaimsPrincipal(myIdentity);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            borrowingController.ControllerContext = context;

            var result = await borrowingController.Details(1);

            Assert.NotNull(result);
            Assert.NotNull(((Microsoft.AspNetCore.Mvc.ViewResult)result).Model);
            //((Microsoft.AspNetCore.Mvc.ViewResult)result).ViewData.Keys
            Assert.NotNull(borrowingController.ViewData["User"]);
            Assert.NotNull(borrowingController.ViewData["Book"]);
            Assert.Equal(borrowingController.ViewData["Book"], "Boook");
            Assert.Equal(borrowingController.ViewData["User"], "Aaaa Bbbb");
        }

        [Fact]
        public async void Edit_Test_BookFound_AuthorFound_ResultOK()
        {
            var bookRepo = new Mock<IBooksDataRepository>();
            var borrowingRepo = new Mock<IBorrowingDataRepository>();
            var accountRepo = new Mock<IAccountsRepository>();

            bookRepo.Setup(t => t.GetAllBooksFullInfo()).Returns(Task.FromResult<IQueryable<BookViewModel>>((new List<BookViewModel> { new BookViewModel { } }).AsQueryable<BookViewModel>()));
            bookRepo.Setup(t => t.GetDetails(It.IsAny<int>())).Returns(Task.FromResult<BookViewModel>(new BookViewModel { Title = "Boook" }));
            bookRepo.Setup(t => t.GetAllAvailableBooks(It.IsAny<int>())).Returns(Task.FromResult<IQueryable<BookViewModel>>((new List<BookViewModel> { new BookViewModel { Title = "Boook" } }).AsQueryable<BookViewModel>()));


            borrowingRepo.Setup(t => t.GetDetails(It.IsAny<int>()))
                .Returns(Task.FromResult<BorrowingViewModel>((
                    new BorrowingViewModel
                    {
                    })));

            borrowingRepo.Setup(t => t.GetNotTracking(It.IsAny<int>()))
              .Returns(Task.FromResult<BorrowingViewModel>((
                  new BorrowingViewModel
                  {
                      Id = 1
                  })));

            accountRepo.Setup(t => t.GetUsersAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<BooksManagementSystemUser>((
                    new BooksManagementSystemUser
                    {
                        Firstname = "Aaaa",
                        LastName = "Bbbb"
                    })));


            accountRepo.Setup(t => t.GetUsersAsync())
             .Returns(Task.FromResult<List<BooksManagementSystemUser>>(
              new List<BooksManagementSystemUser>
              { new BooksManagementSystemUser
                 {
                     Firstname = "Aaaa",
                     LastName = "Bbbb"
                 }
              }
              ));



            BorrowingController borrowingController = new BorrowingController(borrowingRepo.Object, bookRepo.Object, accountRepo.Object);

            GenericIdentity myIdentity = new GenericIdentity("apiTestUser");
            myIdentity.AddClaims(new List<Claim>
            {
                 new Claim("", "")
            }
            );

            var user = new ClaimsPrincipal(myIdentity);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            borrowingController.ControllerContext = context;


            var result = await borrowingController.Edit(1);
            Assert.NotNull(result);
            Assert.NotNull(((Microsoft.AspNetCore.Mvc.ViewResult)result).Model);
            Assert.NotNull(borrowingController.ViewData["Book"]);
            Assert.NotNull(borrowingController.ViewData["User"]);
            Assert.NotNull(borrowingController.ViewData["Users"]);
            Assert.NotNull(borrowingController.ViewData["Books"]);
            Assert.Equal(borrowingController.ViewData["Book"], "Boook");
            Assert.Equal(borrowingController.ViewData["User"], "Aaaa Bbbb");
        }

    }
}