using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlattCodingChallenge.Domain.StarWars;

namespace PlattCodingChallenge.Services
{
    public interface IStarWarsService
    {
        public JsonSerializerSettings Settings { get; set; }
        Task<List<Planet>> GetPlanets(string name = "");
        Task<Planet> GetPlanetById(int id);
        Task<List<Person>> GetPeople(string name = "");
        Task<Person> GetPersonById(int id);
        Task<List<Person>> GetResidentsByPlanet(string planet);
        Task<List<Vehicle>> GetVehicles(string nameOrModel = "");
    }
    public class StarWarsService : IStarWarsService
    {
        //private static HttpClient Client = new HttpClient();
        private readonly IHttpClientFactory _httpClientFactory;
        public JsonSerializerSettings Settings { get; set; }
        public HttpClient Client { get; set; }
        public StarWarsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            Client = _httpClientFactory.CreateClient();
            //Client.BaseAddress = new Uri("https://swapi.dev/api/");
            //null check
            Settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }
        /// <summary>
        /// Get all planets; continue requesting next page of planets (10) until no pages available in next property
        /// http://swapi.dev/api/planets/ <!-- api will redirect w/o trailing slash
        /// </summary>
        /// <returns>Task, list of domain > planet models</returns>
        public async Task<List<Planet>> GetPlanets(string name = "")
        {           
            List<Planet> planetList = new List<Planet>();           
            var response = await Client.GetStringAsync("planets/?search=" + name);            
            Planets pl = JsonConvert.DeserializeObject<Planets>(response,Settings);
            planetList.AddRange(pl.Results);
            while (pl.Next != null)
            {
                string qs = new Uri(pl.Next).Query;
                response = await Client.GetStringAsync("planets/" + qs);
                pl = JsonConvert.DeserializeObject<Planets>(response, Settings);
                planetList.AddRange(pl.Results);
            }
            return planetList;                                   
        }
        /// <summary>
        /// http://swapi.dev/api/planets/{id}/
        /// </summary>
        /// <returns>Task Single domain > planet model</returns>
        public async Task<Planet> GetPlanetById(int id)
        {
            var response = await Client.GetStringAsync("planets/" + id + "/");
            Planet model = JsonConvert.DeserializeObject<Planet>(response, Settings);            
            return model;
        }
        /// <summary>
        /// Get all people; continue requesting next page of planets (10) until no pages available in next property
        /// http://swapi.dev/api/people/ <!-- api will redirect w/o trailing slash
        /// </summary>
        /// <returns>Task, list of domain > planet models</returns>
        public async Task<List<Person>> GetPeople(string name = "")
        {
            List<Person> personList = new List<Person>();
            var response = await Client.GetStringAsync("people/?search=" + name);
            People pl = JsonConvert.DeserializeObject<People>(response, Settings);
            personList.AddRange(pl.Results);
            while (pl.Next != null)
            {
                string qs = new Uri(pl.Next).Query;
                response = await Client.GetStringAsync("people/" + qs);
                pl = JsonConvert.DeserializeObject<People>(response, Settings);
                personList.AddRange(pl.Results);
            }
            return personList;
        }
        /// <summary>
        /// http://swapi.dev/api/people/{id}/
        /// </summary>
        /// <returns>Task Single domain > person model</returns>
        public async Task<Person> GetPersonById(int id)
        {
            var response = await Client.GetStringAsync("people/" + id + "/");
            Person model = JsonConvert.DeserializeObject<Person>(response, Settings);
            return model;
        }
        /// <summary>
        /// Leverages GetPlanets query by name and GetPeople
        /// </summary>
        /// <returns>Task List domain > person model</returns>
        public async Task<List<Person>> GetResidentsByPlanet(string planet)
        {
            List<Person> resList = new List<Person>();
            List<Person> people = this.GetPeople().Result;
            List<Planet> planets = this.GetPlanets(planet).Result;
            Planet pl = planets.First(item => item.Name.ToLower() == planet.ToLower());
            resList = people.Where(resident => resident.Homeworld == pl.Url).ToList<Person>();            
            return resList;
        }
        /// <summary>
        /// Get all vehicles; continue requesting next page of planets (10) until no pages available in next property
        /// http://swapi.dev/api/vehicles/ <!-- api will redirect w/o trailing slash
        /// </summary>
        /// <returns>Task, list of domain > vehicle models</returns>
        public async Task<List<Vehicle>> GetVehicles(string nameOrModel = "")
        {
            List<Vehicle> vehicleList = new List<Vehicle>();
            var response = await Client.GetStringAsync("vehicles/?search=" + nameOrModel);
            Vehicles veh = JsonConvert.DeserializeObject<Vehicles>(response, Settings);
            vehicleList.AddRange(veh.Results);
            while (veh.Next != null)
            {
                string qs = new Uri(veh.Next).Query;
                response = await Client.GetStringAsync("vehicles/" + qs);
                veh = JsonConvert.DeserializeObject<Vehicles>(response, Settings);
                vehicleList.AddRange(veh.Results);
            }
            return vehicleList;
        }
    }
}
