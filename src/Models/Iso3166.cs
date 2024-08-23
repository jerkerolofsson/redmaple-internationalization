using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedMaple.Internationalization.Models
{

    internal class Iso3166
    {
        public required string name { get; set; }

        [JsonPropertyName("alpha-2")]
        public required string alpha2 { get; set; }
        
        [JsonPropertyName("alpha-3")]
        public required string alpha3 { get; set; }

        [JsonPropertyName("country-code")]
        public required string countrycode { get; set; }

        [JsonPropertyName("iso_3166-2")]
        public required string iso_3166_2 { get; set; }

        [JsonPropertyName("region")]
        public required string region { get; set; }

        [JsonPropertyName("sub-region")]
        public required string subregion { get; set; }

        [JsonPropertyName("intermediate-region")]
        public required string intermediateregion { get; set; }

        [JsonPropertyName("region-code")]
        public required string regioncode { get; set; }

        [JsonPropertyName("sub-region-code")]
        public required string subregioncode { get; set; }

        [JsonPropertyName("intermediate-region-code")]
        public required string intermediateregioncode { get; set; }
    }

}
