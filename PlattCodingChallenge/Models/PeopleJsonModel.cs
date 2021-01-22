using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
	public class PeopleJsonModel
	{
		public PeopleJsonModel()
		{
			Characters = new List<PersonDetailsJsonModel>();
		}

		public List<PersonDetailsJsonModel> Characters { get; set; }
    }
}
