using Newtonsoft.Json;
using prolab2;
using System.Collections.Generic;
namespace prolab2
{
	public class Aktarma
	{
		[JsonProperty("transferStopId")]
		public string AktarmaId { get; set; }

		[JsonProperty("transferSure")]
		public int TransferSure { get; set; }

		[JsonProperty("transferUcret")]
		public double TransferUcret { get; set; }
	}

}