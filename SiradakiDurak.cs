using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab2
{
	public class SiradakiDuraka
	{
		[JsonProperty("stopId")]
		public string StopId { get; set; }

		[JsonProperty("mesafe")]
		public double Mesafe { get; set; }

		[JsonProperty("sure")]
		public int Sure { get; set; }

		[JsonProperty("ucret")]
		public double Ucret { get; set; }
	}
}