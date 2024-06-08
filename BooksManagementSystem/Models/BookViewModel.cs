using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksManagementSystem.Models
{
    public class BookViewModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]

        [ForeignKey("AuthorViewModel"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AuthorId { get; set; }
        public AuthorViewModel AuthorViewModel { get; set; }

        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Is available")]
        public bool IsAvailable { get; set; }

    }
}
