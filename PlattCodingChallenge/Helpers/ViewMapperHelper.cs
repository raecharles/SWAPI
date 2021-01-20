using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PlattCodingChallenge.Models;
using PlattCodingChallenge.Domain.StarWars;
using System.Text.RegularExpressions;

namespace PlattCodingChallenge.Helpers
{
    public interface IViewMapperHelper
    {
        AllPlanetsViewModel AllPlanetsMapper(List<Planet> planetList);
        SinglePlanetViewModel PlanetMapper(Planet planet);
        PlanetResidentsViewModel ResidentsMapper(List<Person> people);
        VehicleSummaryViewModel VehicleMapper(List<Vehicle> vehicles);
    }

    public class ViewMapperHelper : IViewMapperHelper
    {
        public ViewMapperHelper()
        {
            
        }
        //moved from controller... appropriate location?
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

        public PlanetResidentsViewModel ResidentsMapper(List<Person> people)
        {
            var model = new PlanetResidentsViewModel();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Person, ResidentSummary>()
            );

            var mapper = config.CreateMapper();
            model.Residents = mapper.Map<List<ResidentSummary>>(people);
            model.Residents = model.Residents.OrderBy(r => r.Name).ToList();
            return model;
        }

        public VehicleSummaryViewModel VehicleMapper(List<Vehicle> vehicles)
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
    }
}
