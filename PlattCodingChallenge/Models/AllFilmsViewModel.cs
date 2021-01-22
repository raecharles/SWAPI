using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
    public class AllFilmsViewModel
    {
		public AllFilmsViewModel()
		{
			Films = new List<FilmDetailsViewModel>();
		}

		public List<FilmDetailsViewModel> Films { get; set; }
	}
}
