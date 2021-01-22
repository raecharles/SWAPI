using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
	public class StarshipsJsonModel
	{
		public StarshipsJsonModel()
		{
			Starships = new List<StarshipDetailsJsonModel>();
		}
		public List<StarshipDetailsJsonModel> Starships { get; set; }
    }
}
