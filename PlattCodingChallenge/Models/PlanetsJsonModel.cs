using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
	public class PlanetsJsonModel
	{
		public PlanetsJsonModel()
		{
			Planets = new List<PlanetDetailsJsonModel>();
		}

		public List<PlanetDetailsJsonModel> Planets { get; set; }
    }
}
