using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.DataModels
{
    public class Reviews
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("rating")]
        public int Rating { get; set; }
        [JsonProperty("location_name")]
        public string Location { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("GoogleId")]
        public string GoogleId { get; set; }
        [JsonProperty("user_latitude")]
        public double UserLatitude { get; set; }
        [JsonProperty("user_longitude")]
        public double UserLongitude { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }

    }
}
