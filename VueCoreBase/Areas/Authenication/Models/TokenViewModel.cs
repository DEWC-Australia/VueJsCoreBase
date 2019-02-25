using System;
using Newtonsoft.Json;

namespace Areas.Authenication.Models
{
    public class TokenViewModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("access_token_expiration")]
        public DateTime AccessTokenExpiration { get; set; }
    }
}
