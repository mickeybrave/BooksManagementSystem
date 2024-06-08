using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagementSystem.Models
{
    public class AuthorViewModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Author first name")]
        public string AuthorFirstname { get; set; }
        [Required]
        [Display(Name = "Author second name")]
        public string AuthorSecondname { get; set; }

        public virtual ICollection<BookViewModel> Books { get; set; }


    }
}
