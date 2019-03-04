using Models.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Areas.Authenication.Models
{
    public class LoginViewModel: ViewModelBase
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
