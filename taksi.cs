using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab2
{
	public class TaksiInfo
	{
		[JsonProperty("openingFee")]
		public double AcilisUcreti { get; set; }

		[JsonProperty("costPerKm")]
		public double KmBasiUcret { get; set; }
	}
}