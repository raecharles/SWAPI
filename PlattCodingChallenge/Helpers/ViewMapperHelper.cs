using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PlattCodingChallenge.Models;
using PlattCodingChallenge.Domain.StarWars;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Helpers
{
    public interface IViewMapperHelper
    {
        AllPlanetsViewModel AllPlanetsMapper(List<Planet> planetList);
        SinglePlanetViewModel PlanetMapper(Planet planet);
        PlanetResidentsViewModel ResidentsMapper(List<Person> people, string planet = "");
        VehicleSummaryViewModel VehiclesMapper(List<Vehicle> vehicles);
        AllFilmsViewModel AllFilmsMapper(List<Film> films);
        PeopleJsonModel PeopleMatchedMapper(List<Person> people, string ids = "");
        PlanetsJsonModel PlanetsMatchedMapper(List<Planet> planets, string ids = "");
        VehiclesJsonModel VehiclesMatchedMapper(List<Vehicle> vehicles, string ids = "");
        StarshipsJsonModel StarshipsMatchedMapper(List<Starship> starships, string ids = "");
    }

    public class ViewMapperHelper : IViewMapperHelper
    {
        public ViewMapperHelper()
        {
            
        }
        //refactor to individual classes
        public AllPlanetsViewModel AllPlanetsMapper(List<Planet> planetList)
        {
            var model = new AllPlanetsViewModel();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Planet, PlanetDetailsViewModel>()
                .ForMember(dest => dest.Diameter, opt => opt.MapFrom(src => (src.Diameter == "unknown") ? -1 : int.Parse(src.Diameter)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Regex.Match(src.Url, @"\d+").Value))
            );

            var mapper = config.CreateMapper();
            model.Planets = mapper.Map<List<PlanetDetailsViewModel>>(planetList);
            model.Planets = model.Planets.OrderByDescending(o => o.Diameter).ToList();
            model.AverageDiameter = model.Planets.Where(i => i.Diameter > 0).Average(i => i.Diameter);          
            return model;
        }
  
        public SinglePlanetViewModel PlanetMapper(Planet planet)
        {
            var model = new SinglePlanetViewModel();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Planet, SinglePlanetViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Regex.Match(src.Url, @"\d+").Value))
            );

            var mapper = config.CreateMapper();
            model = mapper.Map<SinglePlanetViewModel>(planet); 
            return model;
        }

        public PlanetResidentsViewModel ResidentsMapper(List<Person> people, string planet = "")
        {
            var model = new PlanetResidentsViewModel();
            model.Planet = (planet != "") ? planet.First().ToString().ToUpper() + planet.Substring(1) : "Unknown";
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Person, ResidentSummary>()
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Mass))
            );

            var mapper = config.CreateMapper();
            model.Residents = mapper.Map<List<ResidentSummary>>(people);
            model.Residents = model.Residents.OrderBy(r => r.Name).ToList();
            return model;
        }

        public VehicleSummaryViewModel VehiclesMapper(List<Vehicle> vehicles)
        {
            var model = new VehicleSummaryViewModel();
            model.VehicleCount = 0;     
            int mfrCount = 0;

            foreach (Vehicle veh in vehicles)
            {
                if (veh.CostInCredits != "unknown")
                {
                    //manufacturer doesn't exist yet
                    if (!model.Details.Any(v => v.ManufacturerName == veh.Manufacturer))
                    {
                        
                        List<Vehicle> mfrVehicles = vehicles.Where(v => v.Manufacturer == veh.Manufacturer).ToList();
                        //vStats.VehicleCount = mfrVehicles.Count;
                        //List<Vehicle> costVehicles = mfrVehicles.Where(v => (Int32.TryParse(v.CostInCredits, out int cost))).ToList();
                        List<Vehicle> costVehicles = mfrVehicles.Where(v => v.CostInCredits != "unknown").ToList();
                        if (costVehicles.Count > 0)
                        {
                            VehicleStatsViewModel vStats = new VehicleStatsViewModel
                            {
                                ManufacturerName = veh.Manufacturer,
                                AverageCost = costVehicles.Average(v => Convert.ToInt32(v.CostInCredits)),
                                VehicleCount = costVehicles.Count
                            };

                            var config = new MapperConfiguration(cfg =>
                                    cfg.CreateMap<Vehicle, VehicleDetailViewModel>()
                                );

                            var mapper = config.CreateMapper();
                            vStats.Vehicles = mapper.Map<List<VehicleDetailViewModel>>(costVehicles);
                            
                            //aveCost = costVehicles.Average(v => Convert.ToInt32(v.CostInCredits));                          
                            model.VehicleCount += costVehicles.Count;
                            model.Details.Add(vStats);
                            //vStats.AverageCost = vehicles.Where(v => !string.IsNullOrEmpty(v.CostInCredits) && v.CostInCredits.All(Char.IsDigit)).Convert.ToIntAverage(v => v.);
                            mfrCount++;
                        }                                                
                    }
                }
            }
            model.ManufacturerCount = mfrCount;
            model.Details = model.Details.OrderByDescending(v => v.VehicleCount).ThenByDescending(a => a.AverageCost).ToList();
           
            return model;
        }
        public AllFilmsViewModel AllFilmsMapper(List<Film> films)
        {
            var model = new AllFilmsViewModel();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Film, FilmDetailsViewModel>()
                //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => LazyRoman(src.EpisodeId))
                .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => string.Join(", ", src.Characters)))
                .ForMember(dest => dest.Planets, opt => opt.MapFrom(src => string.Join(", ", src.Planets)))
                .ForMember(dest => dest.Starships, opt => opt.MapFrom(src => string.Join(", ", src.Starships)))
                .ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => string.Join(", ", src.Vehicles)))
                .ForMember(dest => dest.Characters, opt => opt.MapFrom(src => string.Join(", ", src.Characters)))
            );

            var mapper = config.CreateMapper();
            model.Films = mapper.Map<List<FilmDetailsViewModel>>(films);
            model.Films = model.Films.OrderBy(o => o.EpisodeId).ToList();
            return model;
        }
        public PeopleJsonModel PeopleMatchedMapper(List<Person> people, string ids = "")
        {   
            List<string> urls = new List<string>();
            if (ids != "")
            {
                urls = ids.Split(",").Select(u => u.Trim()).ToList();
                people = people.Where(p => urls.Contains(p.Url)).ToList();
            }
            var model = new PeopleJsonModel();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Person, PersonDetailsJsonModel>()
               .ForMember(dest => dest.Eyes, opt => opt.MapFrom(src => src.EyeColor))
               .ForMember(dest => dest.Hair, opt => opt.MapFrom(src => src.HairColor))
               //.ForMember(dest => dest.DOB, opt => opt.MapFrom(src => src.BirthYear))
               .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Mass))
            );

            var mapper = config.CreateMapper();
            model.Characters = mapper.Map<List<PersonDetailsJsonModel>>(people);
            model.Characters = model.Characters.OrderBy(r => r.Name).ToList();
            return model;
        }
        public PlanetsJsonModel PlanetsMatchedMapper(List<Planet> planets, string ids = "")
        {
            List<string> urls = new List<string>();
            if (ids != "")
            {
                urls = ids.Split(",").Select(u => u.Trim()).ToList();
                planets = planets.Where(p => urls.Contains(p.Url)).ToList();
            }
            var model = new PlanetsJsonModel();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Planet, PlanetDetailsJsonModel>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.LengthOfYear))
            );

            var mapper = config.CreateMapper();
            model.Planets = mapper.Map<List<PlanetDetailsJsonModel>>(planets);
            model.Planets = model.Planets.OrderBy(p => p.Name).ToList();  
            return model;
        }
        public VehiclesJsonModel VehiclesMatchedMapper(List<Vehicle> vehicles, string ids = "")
        {
            List<string> urls = new List<string>();
            if (ids != "")
            {
                urls = ids.Split(",").Select(u => u.Trim()).ToList();
                vehicles = vehicles.Where(p => urls.Contains(p.Url)).ToList();
            }
            var model = new VehiclesJsonModel();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Vehicle, VehicleDetailsJsonModel>()
                //.ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.MaxSpeed))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.VehicleClass))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.CostInCredits))
            );

            var mapper = config.CreateMapper();
            model.Vehicles = mapper.Map<List<VehicleDetailsJsonModel>>(vehicles);
            model.Vehicles = model.Vehicles.OrderBy(p => p.Name).ToList();
            return model;
        }
        public StarshipsJsonModel StarshipsMatchedMapper(List<Starship> starships, string ids = "")
        {
            List<string> urls = new List<string>();
            if (ids != "")
            {
                urls = ids.Split(",").Select(u => u.Trim()).ToList();
                starships = starships.Where(p => urls.Contains(p.Url)).ToList();
            }
            var model = new StarshipsJsonModel();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Starship, StarshipDetailsJsonModel>()
                .ForMember(dest => dest.MaxSpeed, opt => opt.MapFrom(src => src.MaxAtmospheringSpeed))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.StarshipClass))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.CostInCredits))
            );

            var mapper = config.CreateMapper();
            model.Starships = mapper.Map<List<StarshipDetailsJsonModel>>(starships);
            model.Starships = model.Starships.OrderBy(p => p.Name).ToList();
            return model;
        }
        /*// <summary>
        /// didn't need to be done, normally create actual rn class
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string LazyRoman(int str)
        {
            string rn = "";

            switch (str)
            {
                case "1":
                    rn = "I";
                    break;
                case "2":
                    rn = "II";
                    break;
                case "3":
                    rn = "III";
                    break;
                case "4":
                    rn = "IV";
                    break;
                case "5":
                    rn = "V";
                    break;
                case "6":
                    rn = "VI";
                    break;
                case "7":
                    rn = "VII";
                    break;
                case "8":
                    rn = "VIII";
                    break;
                case "9":
                    rn = "IX";
                    break;
                default:
                    break;
            }
            return rn;
        }*/
    }
    
}
