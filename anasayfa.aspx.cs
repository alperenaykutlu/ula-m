using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prolab2
{
	public partial class anasayfa : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		
		}
	
		protected void OnSubmitClick(object sender, EventArgs e)
		{

            var passenger = passengerType.SelectedValue;
            var payment = paymentMethod.SelectedValue;
            var startL = Convert.ToDouble(startLat.Text);
            var startLo = Convert.ToDouble(startLon.Text);
            var endL = Convert.ToDouble(endLat.Text);
            var endLo = Convert.ToDouble(endLon.Text);

    
            Odeme odeme;
            switch (payment)
            {
                case "1":
                    odeme = new Nakit();
                    break;
                case "2":
                    odeme = new Kredi();
                    break;
                case "3":
                    odeme = new Kentkart();
                    break;
                default:
                    throw new InvalidOperationException("Geçersiz ödeme tipi");
            }


            Yolcu yolcu;
            switch (passenger)
            {
                case "1":
                    yolcu = new Yolcu(odeme);         
                    break;
                case "2":
                    yolcu = new Ogrenci(odeme);        
                    break;
                case "3":
                    yolcu = new Yasli(odeme);           
                    break;
                case "4":
                    yolcu = new Gazi(odeme);         
                    break;
                default:
                    throw new InvalidOperationException("Geçersiz yolcu tipi");
            }
            Konum startKonum = new Konum(startL, startLo);
            Konum endKonum = new Konum(endL, endLo);


            double distance = startKonum.MesafeHesapla(endKonum);

      
            Session["Yolcu"] = yolcu;
            Session["StartLat"] = startL;
            Session["StartLon"] = startLo;
            Session["EndLat"] = endL;
            Session["EndLon"] = endLo;
            Session["Distance"] = distance;

       
            Response.Redirect("sonuclar.aspx");
        }
		
	}
}