using Microsoft.AspNetCore.Mvc;
using PlattCodingChallenge.Models;
using PlattCodingChallenge.Services;
using PlattCodingChallenge.Domain.StarWars;
using PlattCodingChallenge.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System;
using System.Linq;

namespace PlattCodingChallenge.Controllers
{
	public class HomeController : Controller
	{
		private readonly IStarWarsService _starWarsService;
		private readonly IViewMapperHelper _viewMapperHelper;
		//error handling / pages
		//vue
		//cancellation tokens?
		//testing?
		public HomeController(IStarWarsService starWarsService, IViewMapperHelper viewMapperHelper)
        {
			_starWarsService = starWarsService;
			_viewMapperHelper = viewMapperHelper;
        }
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult GetAllPlanets()
		{			
			// TODO: Implement this controller action
			List<Planet> planetList = _starWarsService.GetPlanets().Result;
			var model = _viewMapperHelper.AllPlanetsMapper(planetList);          
			return View(model);
		}

		public ActionResult GetPlanetById(int planetid)
		{
			// TODO: Implement this controller action
			Planet planet = _starWarsService.GetPlanetById(planetid).Result;
			var model = _viewMapperHelper.PlanetMapper(planet);
			return View(model);
		}

		public ActionResult GetResidentsOfPlanet(string planetname)
		{			
			// TODO: Implement this controller action
			List<Person> planetResidents = _starWarsService.GetResidentsByPlanet(planetname).Result;
			PlanetResidentsViewModel model = _viewMapperHelper.ResidentsMapper(planetResidents);
			return View(model);
		}

		public ActionResult VehicleSummary()
		{	
			// TODO: Implement this controller action
			List<Vehicle> vehicleList = _starWarsService.GetVehicles().Result;
			VehicleSummaryViewModel model = _viewMapperHelper.VehicleMapper(vehicleList);
			return View(model);
		}
    }
}
