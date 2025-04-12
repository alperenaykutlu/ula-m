using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab2
{
	public class Rota
	{
		public List<string> adim { get; set; }
		public double ToplamCost { get; set; }
		public double ToplamZaman { get; set; }
		public double ToplamDistance { get; set; }
		public bool TaksiKullanildi { get; set; }

		public Rota()
		{
			adim = new List<string>();
            ToplamCost = 0;
            ToplamZaman = 0;
            ToplamDistance = 0;
			TaksiKullanildi = false;
		}
	}
}