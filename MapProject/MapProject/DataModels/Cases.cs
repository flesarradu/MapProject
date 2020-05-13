using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.DataModels
{
    public class Cases
    {
        [JsonProperty("id")]
        int Id { get; set; }
        [JsonProperty("LocationName")]
        public string LocationName { get; set; }
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
        [JsonProperty("Radius")]
        public int Radius { get; set; }
        [JsonProperty("CaseNo")]
        public int CaseNo { get; set; }

    }
}
