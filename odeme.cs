using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab2
{
	public abstract class Odeme
	{
		public abstract double Ucret(double temelucret);
		public abstract string OdemeTipi();
		public abstract bool TopluTasimaIcinGecerliMi();
		public abstract bool TaksiIcinGecerliMi();
	}

	// Credit card payment - 10% fee
	class Kredi : Odeme
	{
		public override double Ucret(double temelucret)
		{
			return temelucret * 1.1; // 10% fee for credit card
		}

		public override string OdemeTipi()
		{
			return "Kredi Kartı";
		}

		public override bool TopluTasimaIcinGecerliMi()
		{
			return true;
		}

		public override bool TaksiIcinGecerliMi()
		{
			return true;
		}
	}

	class Kentkart : Odeme
	{
		public override double Ucret(double temelucret)
		{
			return temelucret;
		}

		public override string OdemeTipi()
		{
			return "Kentkart";
		}

		public override bool TopluTasimaIcinGecerliMi()
		{
			return true;
		}

		public override bool TaksiIcinGecerliMi()
		{
			return false; 
		}
	}


	class Nakit : Odeme
	{
		public override double Ucret(double temelucret)
		{
			return temelucret; 
		}

		public override string OdemeTipi()
		{
			return "Nakit";
		}

		public override bool TopluTasimaIcinGecerliMi()
		{
			return false; 
		}

		public override bool TaksiIcinGecerliMi()
		{
			return true;
		}
	}
}