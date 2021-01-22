using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
	public class VehiclesJsonModel
	{
		public VehiclesJsonModel()
		{
			Vehicles = new List<VehicleDetailsJsonModel>();
		}
		public List<VehicleDetailsJsonModel> Vehicles { get; set; }
    }
}
