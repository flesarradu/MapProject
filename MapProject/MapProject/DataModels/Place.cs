using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.DataModels
{
    public class Places
    {
        [JsonProperty("id")]
        int Id { get; set; }
        [JsonProperty("LocationName")]
        public string LocationName { get; set; }
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
    }
}
