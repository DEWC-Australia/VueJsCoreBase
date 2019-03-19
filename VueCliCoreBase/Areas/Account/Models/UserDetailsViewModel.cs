using System.ComponentModel.DataAnnotations;
using Models.Base;

namespace Areas.Account.Models
{
    public class UserDetailsViewModel : ViewModelBase
    {
        [Required, Display(Description = "Email"), EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

    }
}
