using Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Areas.Account.Models
{
    public class RegisterViewModel: ViewModelBase
    {
        [Required]
        [StringLength(250, ErrorMessage = "{0} max {1} characters long.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "{0} max {1} characters long.")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(250, ErrorMessage = "{0} max {1} characters long.")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
