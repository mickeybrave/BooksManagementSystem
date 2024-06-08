using System.ComponentModel.DataAnnotations;

namespace BooksManagementSystem.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [MinLength(3, ErrorMessage = "Firstname cannot shorter than 3 characters")]
        [StringLength(100, ErrorMessage = "Firstname cannot be longer than 100 characters")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [StringLength(100, ErrorMessage = "Lastname cannot be longer than 100 characters")]
        [MinLength(3, ErrorMessage = "Lastname cannot be shorter than 3 characters")]
        public string Lastname { get; set; }
    }
}
