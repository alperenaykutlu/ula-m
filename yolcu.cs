using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace prolab2
{
	public class Yolcu
	{
		public readonly Odeme Odeme;
		public string YolcuTipi { get; protected set; }

		public Yolcu(Odeme odeme)
		{
			this.Odeme = odeme;
			this.YolcuTipi = "Normal";
		}

		public virtual double HesaplaUcret(double baseFare, bool taksiMi)
		{
			if (taksiMi)
			{
			
				return Odeme.Ucret(baseFare);
			}
			else
			{
		
				return Odeme.Ucret(baseFare);
			}
		}
	}


	class Ogrenci : Yolcu
	{
		public Ogrenci(Odeme odeme) : base(odeme)
		{
			this.YolcuTipi = "Öğrenci";
		}

		public override double HesaplaUcret(double baseFare, bool taksiMi)
		{
			if (taksiMi)
			{
				
				return Odeme.Ucret(baseFare);
			}
			else
			{
			
				return Odeme.Ucret(baseFare * 0.5);
			}
		}
	}


	class Yasli : Yolcu
	{
		public Yasli(Odeme odeme) : base(odeme)
		{
			this.YolcuTipi = "Yaşlı";
		}

		public override double HesaplaUcret(double baseFare, bool taksiMi)
		{
			if (taksiMi)
			{
			
				return Odeme.Ucret(baseFare);
			}
			else
			{

				return Odeme.Ucret(baseFare * 0.7);
			}
		}
	}


	class Gazi : Yolcu
	{
		public Gazi(Odeme odeme) : base(odeme)
		{
			this.YolcuTipi = "Gazi";
		}

		public override double HesaplaUcret(double baseFare, bool taksiMi)
		{
			if (taksiMi)
			{

				return Odeme.Ucret(baseFare);
			}
			else
			{
			
				return 0;
			}
		}
	}

}