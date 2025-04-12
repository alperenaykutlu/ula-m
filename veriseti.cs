using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab2
{
	public class VeriSeti
	{
		[JsonProperty("city")]
		public string Sehir { get; set; }

		[JsonProperty("taxi")]
		public TaksiInfo Taksi { get; set; }

		[JsonProperty("duraklar")]
		public List<duraklar> Duraklar { get; set; }
	}

}