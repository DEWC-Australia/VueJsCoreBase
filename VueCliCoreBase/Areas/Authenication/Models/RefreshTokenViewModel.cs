using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Areas.Authenication.Models
{
    public class RefreshTokenViewModel
    {
        [Required, JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [Required, JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }
}