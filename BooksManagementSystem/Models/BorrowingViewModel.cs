using BooksManagementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        [Display(Name = "Borrowed date")]
        public DateTime BorrowedDate { get; set; }

        public DateTime? ReturnedDate { get; set; }



    }
}
