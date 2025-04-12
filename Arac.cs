using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab2
{
	abstract class Arac
	{
		public int KatedilenMesafe;
		public int KatedilenSure;
		public abstract double HesaplaUcret(Yolcu yolcu);
		public abstract string AracTipi();
	}

	class Otobus : Arac
	{
		public override double HesaplaUcret(Yolcu yolcu)
		{
			return yolcu.HesaplaUcret(KatedilenMesafe * 2, false); 
		}

		public override string AracTipi()
		{
			return "Otobüs";
		}
	}

	
	class Taksi : Arac
	{
		private double BaslangicUcret;
		private double MesafeBasiUcret;

		public Taksi(double baslangicUcret, double mesafeBasiUcret)
		{
			this.BaslangicUcret = baslangicUcret;
			this.MesafeBasiUcret = mesafeBasiUcret;
		}

		public override double HesaplaUcret(Yolcu yolcu)
		{
			double baseFare = BaslangicUcret + (MesafeBasiUcret * KatedilenMesafe);


			if (yolcu == null)
			{
				return baseFare;
			}


			return yolcu.HesaplaUcret(baseFare, true);
		}
        public override string AracTipi()
		{
			return "Taksi";
		}
	}


	class Tramvay : Arac
	{
		public override double HesaplaUcret(Yolcu yolcu)
		{
			return yolcu.HesaplaUcret(KatedilenMesafe * 1.5, false); 
		}

		public override string AracTipi()
		{
			return "Tramvay";
		}
	}

}