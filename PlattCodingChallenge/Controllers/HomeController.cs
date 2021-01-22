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
		//check static mobile
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
			PlanetResidentsViewModel model = _viewMapperHelper.ResidentsMapper(planetResidents, planetname);
			return View(model);
		}		
		public ActionResult VehicleSummary()
		{	
			// TODO: Implement this controller action
			List<Vehicle> vehicleList = _starWarsService.GetVehicles().Result;
			VehicleSummaryViewModel model = _viewMapperHelper.VehiclesMapper(vehicleList);
			return View(model);
		}

		public ActionResult GetFilms()
		{
			// TODO: Implement this controller action
			List<Film> filmList = _starWarsService.GetFilms().Result;
			AllFilmsViewModel model = _viewMapperHelper.AllFilmsMapper(filmList);
			return View(model);
		}
		[HttpGet]
		public ActionResult GetPeople(string ids = "")
		{
			List<Person> chars = _starWarsService.GetPeople().Result;
			PeopleJsonModel model = _viewMapperHelper.PeopleMatchedMapper(chars, ids);
			var json = new JsonResult(model);
			json.StatusCode = 200;
			json.ContentType = "application/json";
			return json;
		}
		[HttpGet]
		public ActionResult GetPlanets(string ids = "")
		{
			//var viewModel = GetAllPlanets(); could reuse then match against Id
			List<Planet> planetList = _starWarsService.GetPlanets().Result;
			var model = _viewMapperHelper.PlanetsMatchedMapper(planetList, ids);
			var json = new JsonResult(model);
			json.StatusCode = 200;
			json.ContentType = "application/json";
			return json;
		}
		[HttpGet]
		public ActionResult GetVehicles(string ids = "")
		{
			List<Vehicle> vehicles = _starWarsService.GetVehicles().Result;
			VehiclesJsonModel model = _viewMapperHelper.VehiclesMatchedMapper(vehicles, ids);
			var json = new JsonResult(model);
			json.StatusCode = 200;
			json.ContentType = "application/json";
			return json;
		}
		[HttpGet]
		public ActionResult GetStarships(string ids = "")
		{
			List<Starship> ships = _starWarsService.GetStarships().Result;
			StarshipsJsonModel model = _viewMapperHelper.StarshipsMatchedMapper(ships, ids);
			var json = new JsonResult(model);
			json.StatusCode = 200;
			json.ContentType = "application/json";
			return json;
		}
	}
}
