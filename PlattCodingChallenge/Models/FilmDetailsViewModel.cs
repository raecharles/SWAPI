using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
    public class FilmDetailsViewModel
    {
        public FilmDetailsViewModel()
        {
            //Characters = new List<string>();
            /*Planets = new List<string>();
            Starships = new List<string>();
            Vehicles = new List<string>();*/
            Species = new List<string>();
        }

        public string Title { get; set; }
        public int EpisodeId { get; set; }
        public string OpeningCrawl { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        public string ReleaseDate { get; set; }
        public string Characters { get; set; }
        public string Planets { get; set; }
        public string Starships { get; set; }
        public string Vehicles { get; set; }
        public List<string> Species { get; set; }
    }
}
