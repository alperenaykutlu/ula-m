using Newtonsoft.Json;
using prolab2;
using System.Collections.Generic;

public class duraklar
{
	[JsonProperty("id")]
	public string DurakId { get; set; }

	[JsonProperty("name")]
	public string DurakName { get; set; }

	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("lat")]
	public double Enlem { get; set; }

	[JsonProperty("lon")]
	public double Boylam { get; set; }

	[JsonProperty("sonDurak")]
	public bool SonDurakMi { get; set; }

	[JsonProperty("nextStops")]
	public List<SiradakiDuraka> SiradakiDurak { get; set; }

	[JsonProperty("transfer")]
	public Aktarma Transfer { get; set; }


	public duraklar()
	{
		SiradakiDurak = new List<SiradakiDuraka>();
	}

}
