using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MapProject.DataModels
{
    public class User
    {
        [JsonProperty("id")]
       
        public string Id { get; set; }
        [JsonProperty("user")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }

        public bool IsPassword(string password)
        {
            return Password == password;
        }
    }
}
