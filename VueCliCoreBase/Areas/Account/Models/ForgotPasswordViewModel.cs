using Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Areas.Account.Models
{
    public class ForgotPasswordViewModel: ViewModelBase
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
