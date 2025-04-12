using System;

namespace prolab2
{
	public class Konum
	{
		public double enlem { get; set; }
		public double boylam { get; set; }

		public Konum(double enlem, double boylam)
		{
			this.enlem = enlem;
			this.boylam = boylam;
		}


		public double MesafeHesapla(Konum hedefKonum)
		{
			double R = 6371;

			double dLat = DereceToRadyan(hedefKonum.enlem - this.enlem);
			double dLon = DereceToRadyan(hedefKonum.boylam - this.boylam);

			double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
					   Math.Cos(DereceToRadyan(this.enlem)) * Math.Cos(DereceToRadyan(hedefKonum.enlem)) *
					   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			double mesafe = R * c;

			return mesafe;
		}


		private static double DereceToRadyan(double mesafe) => (Math.PI / 180) * mesafe;
	}
}
