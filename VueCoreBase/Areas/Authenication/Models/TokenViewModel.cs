using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Areas.Authenication.Models
{
    public class TokenViewModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        public string FullName { get; set; }
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
