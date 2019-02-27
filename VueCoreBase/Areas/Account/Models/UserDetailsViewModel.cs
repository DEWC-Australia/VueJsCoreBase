using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace Areas.Account.Models
{
    public class UserDetailsViewModel
    {

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required, Display(Description = "Work Email"), EmailAddress]
        public string Email { get; set; }


        [Required, Display(Description = "Notifications Email"), EmailAddress]
        public string NotificationEmail { get; set; }

        [Required, Display(Description = "Notifications Enable")]
        public bool Notifications { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
