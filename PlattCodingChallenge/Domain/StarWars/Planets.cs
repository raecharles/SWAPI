﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Domain.StarWars
{
    public class Planets
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("next")]
        public string Next { get; set; }
        [JsonProperty("previous")]
        public string Previous { get; set; }
        [JsonProperty("results")]
        public List<Planet> Results { get; set; }
    }
    public class Planet
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("rotation_period")] 
        public string LengthOfDay { get; set; }
        [JsonProperty("orbital_period")]
        public string LengthOfYear { get; set; }
        [JsonProperty("diameter")]
        public string Diameter { get; set; }
        [JsonProperty("climate")]
        public string Climate { get; set; }
        [JsonProperty("gravity")]
        public string Gravity { get; set; }
        [JsonProperty("terrain")]
        public string Terrain { get; set; }
        [JsonProperty("surface_water")]
        public string SurfaceWaterPercentage { get; set; }
        [JsonProperty("population")]
        public string Population { get; set; }
        [JsonProperty("residents")]
        public List<string> Residents { get; set; }
        [JsonProperty("films")]
        public List<string> Films { get; set; }
        /*[JsonProperty("created")]
        public string Created { get; set; }
        [JsonProperty("edited")]
        public string Edited { get; set; }*/
        [JsonProperty("url")]
        public string Url { get; set; }
        public string Id { get; set; }
    }
}
