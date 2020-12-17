using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class City
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("state_code")]
        public string StateCode { get; set; }

        [Required]
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
