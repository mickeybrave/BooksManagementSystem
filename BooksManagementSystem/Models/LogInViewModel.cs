using System.ComponentModel.DataAnnotations;

namespace BooksManagementSystem.Models
{
    public class LogInViewModel
    {
        [Required( ErrorMessage ="Email address is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
      

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
