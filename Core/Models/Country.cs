using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Country
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("iso2")]
        public string Code { get; set; }

        [Required]
        [JsonProperty("region")]
        public string Region { get; set; }

        public List<State> States { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
