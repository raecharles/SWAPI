using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
	public class VehicleStatsViewModel
	{
		public VehicleStatsViewModel()
        {
			Vehicles = new List<VehicleDetailViewModel>();
        }
		public string ManufacturerName { get; set; }

		public int VehicleCount { get; set; }

		public double AverageCost { get; set; }

		public List<VehicleDetailViewModel> Vehicles { get; set; }
	}

	public class VehicleDetailViewModel
    {
		public string Name { get; set; }
		public string Model { get; set; }
		public string Passengers { get; set; }
		public string MaxSpeed { get; set; }
		public string VehicleClass { get; set; }
    }
}
