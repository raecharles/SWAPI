using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{	
	public class StarshipDetailsJsonModel
    {
		public string Name { get; set; }
		public string Manufacturer { get; set; }
		public string Cost { get; set; }
		public string HyperdriveRating { get; set; }
		public string Passengers { get; set; }
		public string MaxSpeed { get; set; }
		public string Class { get; set; }
    }
}
