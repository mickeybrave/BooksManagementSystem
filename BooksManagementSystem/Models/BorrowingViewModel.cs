using BooksManagementSystem.Areas.Identity.Data;
using BooksManagementSystem.Infra;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace BooksManagementSystem.Models
{
    public class BorrowingViewModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("BookViewModel"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public BookViewModel BookViewModel { get; set; }

        [Required]
        [ForeignKey("BooksManagementSystemUser"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public BooksManagementSystemUser BooksManagementSystemUser { get; set; }

        [CheckDateRange(CanBeNull = false, FieldName = "Borrowed date")]
        [Required]
        [Display(Name = "Borrowed date")]
        public DateTime? BorrowedDate { get; set; }

        [Display(Name = "Returned date")]
        [CheckDateRange(CanBeNull = true, FieldName = "Returned date")]
        public DateTime? ReturnedDate { get; set; }

        [Display(Name = "Book information")]
        public string BookInfo { get { return BookViewModel?.Title + " " + BookViewModel?.AuthorViewModel?.AuthorFirstname + " " + BookViewModel?.AuthorViewModel?.AuthorSecondname; } }

        [Display(Name = "User information")]
        public string UserInfo { get { return BooksManagementSystemUser?.LastName + " " + BooksManagementSystemUser?.Firstname; } }


    }
}
