using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace MapProject.DataModels
{
    public class Users
    {
        [JsonProperty("id")]
        [AutoIncrement]
        public string Id { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Height")]
        public double Height { get; set; }
        [JsonProperty("Weight")]
        public double Weight { get; set; }
        [JsonProperty("RiskLevel")]
        public int RiskLevel { get; set; }
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
        [JsonProperty("Nationality")]
        public string Nationality { get; set; }
        /*
               [Version]
              [JsonProperty(PropertyName = "version")]
              public byte[] Version { get; set; }

              [JsonProperty(PropertyName = "createdAt")]
              public DateTime? CreatedAt { get; set; }

              [JsonProperty(PropertyName = "updatedAt")]
              public DateTime? UpdatedAt { get; set; }

        */
        public bool IsPassword(string password)
        {
            return Password == password;
        }
    }
}
