
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text;

namespace prolab2
{
    public partial class sonuclar : System.Web.UI.Page
    {
        private static List<duraklar> durakListesi;
        private static TaksiInfo taksiInfo;
        private const string VeriSetiPath = "C://Users//alper//source//repos//prolab2//prolab2//veriseti.json";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    if (!ValidateSessionData())
                    {
                        ShowError("Geçersiz veri! Lütfen tekrar deneyin.");
                        return;


                    }
                    var yolcu = (Yolcu)Session["Yolcu"];
                    var startLat = Convert.ToDouble(Session["StartLat"]);
                    var startLon = Convert.ToDouble(Session["StartLon"]);
                    var endLat = Convert.ToDouble(Session["EndLat"]);
                    var endLon = Convert.ToDouble(Session["EndLon"]);
                    var distance = Convert.ToDouble(Session["Distance"]);

                    LoadVeriSeti();
                    DisplayCoordinatesAndInfo(startLat, startLon, endLat, endLon, yolcu);


                    bool isValidForPublicTransport = yolcu.Odeme.TopluTasimaIcinGecerliMi();

                    if (!isValidForPublicTransport)
                    {

                        CalculateAndDisplayTaxiRoute(startLat, startLon, endLat, endLon, yolcu);
                    }
                    else
                    {

                        CalculateAndDisplayCombinedRoute(startLat, startLon, endLat, endLon, yolcu);
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Beklenmeyen bir hata oluştu: {ex.Message}");
  
                }
            }
        }

        private bool ValidateSessionData()
        {
            return Session["Yolcu"] != null &&
                   Session["StartLat"] != null &&
                   Session["StartLon"] != null &&
                   Session["EndLat"] != null &&
                   Session["EndLon"] != null &&
                   Session["Distance"] != null;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
            pnlResults.Visible = false;
        }

        private void LoadVeriSeti()
        {
           
            string filePath = VeriSetiPath;
            string json = File.ReadAllText(filePath);
            var veriSeti = JsonConvert.DeserializeObject<VeriSeti>(json);
            durakListesi = veriSeti.Duraklar;
            taksiInfo = veriSeti.Taksi;
        }

        private void DisplayCoordinatesAndInfo(double startLat, double startLon, double endLat, double endLon, Yolcu yolcu)
        {
            lblStartCoords.Text = $"{startLat.ToString("F6", CultureInfo.InvariantCulture)}, {startLon.ToString("F6", CultureInfo.InvariantCulture)}";
            lblEndCoords.Text = $"{endLat.ToString("F6", CultureInfo.InvariantCulture)}, {endLon.ToString("F6", CultureInfo.InvariantCulture)}";
            lblYolcuTipi.Text = yolcu.YolcuTipi;
            lblOdemeTipi.Text = yolcu.Odeme.OdemeTipi();
            lblTarih.Text = DateTime.Now.ToString("dd MMM yyyy HH:mm", new CultureInfo("tr-TR"));
        }

        private void CalculateAndDisplayTaxiRoute(double startLat, double startLon, double endLat, double endLon, Yolcu yolcu)
        {

            var startKonum = new Konum(startLat, startLon);
            var endKonum = new Konum(endLat, endLon);


            double directDistance = startKonum.MesafeHesapla(endKonum);


            Taksi taksi = new Taksi(taksiInfo.AcilisUcreti, taksiInfo.KmBasiUcret);
            taksi.KatedilenMesafe = (int)directDistance;
            double taxiCost = taksi.HesaplaUcret(yolcu);

            var rota = new Rota
            {
                ToplamCost = taxiCost,
                ToplamDistance = directDistance,
                ToplamZaman = directDistance * 2, 
                adim = new List<string> { $"Taksi ile doğrudan yolculuk ({directDistance:F2} km)" },
                TaksiKullanildi = true
            };

            // Display the route
            DisplayRouteResults(startKonum, endKonum, rota);
            DisplayRouteDetails(rota);
            pnlResults.Visible = true;


            lblRouteType.Text = "Taksi";
            lblRouteTypeInfo.Text = "Nakit veya Kredi Kartı ödeme yöntemi seçildiği için sadece taksi kullanılabilir.";
        }

        private void CalculateAndDisplayCombinedRoute(double startLat, double startLon, double endLat, double endLon, Yolcu yolcu)
        {

            var startKonum = new Konum(startLat, startLon);
            var endKonum = new Konum(endLat, endLon);


            var startDurak = FindNearestStop(startKonum);
            var endDurak = FindNearestStop(endKonum);


            double startToStopDist = startKonum.MesafeHesapla(new Konum(startDurak.Enlem, startDurak.Boylam));
            double endToStopDist = endKonum.MesafeHesapla(new Konum(endDurak.Enlem, endDurak.Boylam));


            double directDistance = startKonum.MesafeHesapla(endKonum);


            bool offerDirectTaxi = directDistance <= 5 || startToStopDist > 3 || endToStopDist > 3;


            bool taksiIleBaşla = startToStopDist > 3;
            bool taksiIleBitir = endToStopDist > 3;

         
            var rota = Dijkstra(startDurak, endDurak);

       
            if (taksiIleBaşla)
            {
                Taksi taksi = new Taksi(taksiInfo.AcilisUcreti, taksiInfo.KmBasiUcret);
                taksi.KatedilenMesafe = (int)startToStopDist;
                double taksiUcreti = taksi.HesaplaUcret(yolcu);

                rota.adim.Insert(0, $"Taksi ile başlangıç noktasından {startDurak.DurakName} durağına ({startToStopDist:F2} km)");
                rota.ToplamCost += taksiUcreti;
                rota.ToplamZaman += startToStopDist * 2; 
                rota.ToplamDistance += startToStopDist;
                rota.TaksiKullanildi = true;
            }

            if (taksiIleBitir)
            {
                Taksi taksi = new Taksi(taksiInfo.AcilisUcreti, taksiInfo.KmBasiUcret);
                taksi.KatedilenMesafe = (int)endToStopDist;
                double taksiUcreti = taksi.HesaplaUcret(yolcu);

                rota.adim.Add($"Taksi ile {endDurak.DurakName} durağından hedef noktaya ({endToStopDist:F2} km)");
                rota.ToplamCost += taksiUcreti;
                rota.ToplamZaman += endToStopDist * 2; 
                rota.ToplamDistance += endToStopDist;
                rota.TaksiKullanildi = true;
            }


            DisplayRouteResults(startKonum, endKonum, rota);
            DisplayRouteDetails(rota);


            if (offerDirectTaxi)
            {
                Taksi taksi = new Taksi(taksiInfo.AcilisUcreti, taksiInfo.KmBasiUcret);
                taksi.KatedilenMesafe = (int)directDistance;
                double taksiUcreti = taksi.HesaplaUcret(yolcu);

                litAlternativeRoute.Text = $"<div class='alternative-route'>" +
                    $"<h4>Alternatif Rota:</h4>" +
                    $"<p>Doğrudan taksi ile yolculuk:</p>" +
                    $"<ul>" +
                    $"<li>Mesafe: {directDistance:F2} km</li>" +
                    $"<li>Yaklaşık süre: {(int)(directDistance * 2)} dk</li>" +
                    $"<li>Ücret: {taksiUcreti:F2} TL</li>" +
                    $"</ul></div>";

                pnlAlternativeRoute.Visible = true;
            }
            else
            {
                pnlAlternativeRoute.Visible = false;
            }

          
            ApplyPassengerDiscount(rota, yolcu);

   
            if (rota.TaksiKullanildi)
            {
                lblRouteType.Text = "Kombinasyon (Toplu Taşıma + Taksi)";
                lblRouteTypeInfo.Text = "Uzak duraklar için taksi, duraklar arası için toplu taşıma kullanılıyor.";
            }
            else
            {
                lblRouteType.Text = "Toplu Taşıma";
                lblRouteTypeInfo.Text = "Tüm rota toplu taşıma araçları kullanılarak hesaplanmıştır.";
            }

           
            pnlResults.Visible = true;
        }

        private void ApplyPassengerDiscount(Rota rota, Yolcu yolcu)
        {
      
         
            lblDiscount.Text = $"({yolcu.YolcuTipi} indirim uygulanmıştır)";
        }

        private void DisplayRouteResults(Konum startKonum, Konum endKonum, Rota rota)
        {
            var directDistance = startKonum.MesafeHesapla(endKonum);
            lblDirectDistance.Text = $"{directDistance.ToString("F2", CultureInfo.InvariantCulture)} km";
            lblRouteDistance.Text = $"{rota.ToplamDistance.ToString("F2", CultureInfo.InvariantCulture)} km";
            lblEstimatedTime.Text = $"{(int)rota.ToplamZaman} dk";
            lblEstimatedCost.Text = $"{rota.ToplamCost.ToString("F2", CultureInfo.InvariantCulture)} TL";
        }

        private void DisplayRouteDetails(Rota rota)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<h3>Rota Adımları:</h3>");
            sb.Append("<ol class='route-steps'>");

            for (int i = 0; i < rota.adim.Count; i++)
            {
                sb.AppendFormat("<li>{0}</li>", rota.adim[i]);
            }

            sb.Append("</ol>");
            sb.AppendFormat("<div class='route-summary'>");
            sb.AppendFormat("<p><strong>Toplam Mesafe:</strong> {0:F2} km</p>", rota.ToplamDistance);
            sb.AppendFormat("<p><strong>Toplam Süre:</strong> {0:F0} dk</p>", rota.ToplamZaman);
            sb.AppendFormat("<p><strong>Toplam Ücret:</strong> {0:F2} TL</p>", rota.ToplamCost);
            sb.Append("</div>");

            litRouteInfo.Text = sb.ToString();
        }

        // Find the nearest stop to the given location
        private static duraklar FindNearestStop(Konum location)
        {
            return durakListesi.OrderBy(d => location.MesafeHesapla(new Konum(d.Enlem, d.Boylam))).First();
        }

   
        private static Rota Dijkstra(duraklar start, duraklar end)
        {
            Dictionary<string, double> distances = new Dictionary<string, double>();
            Dictionary<string, string> previous = new Dictionary<string, string>();
            Dictionary<string, double> times = new Dictionary<string, double>();
            Dictionary<string, double> costs = new Dictionary<string, double>();
            Dictionary<string, List<string>> transfers = new Dictionary<string, List<string>>();
            HashSet<string> visited = new HashSet<string>();

            foreach (var durak in durakListesi)
            {
                distances[durak.DurakId] = double.MaxValue;
                times[durak.DurakId] = double.MaxValue;
                costs[durak.DurakId] = double.MaxValue;
                previous[durak.DurakId] = null;
                transfers[durak.DurakId] = new List<string>();
            }

            distances[start.DurakId] = 0;
            times[start.DurakId] = 0;
            costs[start.DurakId] = 0;

            PriorityQueue<string, double> pq = new PriorityQueue<string, double>();
            pq.Enqueue(start.DurakId, 0);

            while (pq.Count > 0)
            {
                string currentId = pq.Dequeue();

                if (currentId == end.DurakId)
                    break;

                if (visited.Contains(currentId))
                    continue;

                visited.Add(currentId);

                var current = durakListesi.First(d => d.DurakId == currentId);

         
                foreach (var next in current.SiradakiDurak)
                {
                    double newDist = distances[currentId] + next.Mesafe;
                    double newTime = times[currentId] + next.Sure;
                    double newCost = costs[currentId] + next.Ucret;

                    if (newDist < distances[next.StopId])
                    {
                        distances[next.StopId] = newDist;
                        times[next.StopId] = newTime;
                        costs[next.StopId] = newCost;
                        previous[next.StopId] = currentId;

                    
                        transfers[next.StopId] = new List<string>(transfers[currentId]);

                        pq.Enqueue(next.StopId, newDist);
                    }
                }

             
                if (current.Transfer != null)
                {
                    string transferId = current.Transfer.AktarmaId;
                    double transferDist = 0.1; 
                    double transferTime = current.Transfer.TransferSure;
                    double transferCost = current.Transfer.TransferUcret;

                    double newDist = distances[currentId] + transferDist;
                    double newTime = times[currentId] + transferTime;
                    double newCost = costs[currentId] + transferCost;

                    if (newDist < distances[transferId])
                    {
                        distances[transferId] = newDist;
                        times[transferId] = newTime;
                        costs[transferId] = newCost;
                        previous[transferId] = currentId;

                        transfers[transferId] = new List<string>(transfers[currentId]);
                        transfers[transferId].Add(current.DurakId + "->" + transferId);

                        pq.Enqueue(transferId, newDist);
                    }
                }
            }

            Rota rota = new Rota();
            rota.adim = new List<string>();

            if (previous[end.DurakId] == null && start.DurakId != end.DurakId)
            {
                rota.adim.Add("Toplu taşıma ile rota bulunamadı, taksi ile gidebilirsiniz!");

                Konum startKonum = new Konum(start.Enlem, start.Boylam);
                Konum endKonum = new Konum(end.Enlem, end.Boylam);
                double taxiDistance = startKonum.MesafeHesapla(endKonum);
                rota.adim.Add($"Başlangıç noktasından hedef noktasına taksi ile mesafe: {taxiDistance:F2} km");

                Taksi taksi = new Taksi(taksiInfo.AcilisUcreti, taksiInfo.KmBasiUcret);
                taksi.KatedilenMesafe = (int)taxiDistance;
                double taxiCost = taksi.HesaplaUcret(null);

                rota.ToplamCost = taxiCost;
                rota.ToplamDistance = taxiDistance;
                rota.ToplamZaman = taxiDistance * 2; 
                rota.TaksiKullanildi = true;

                return rota;
            }

            string currentDurak = end.DurakId;
            List<string> path = new List<string>();

            while (currentDurak != null)
            {
                path.Insert(0, currentDurak);
                currentDurak = previous[currentDurak];
            }

  
            double totalDistance = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                var currentStop = durakListesi.First(d => d.DurakId == path[i]);
                var nextStop = durakListesi.First(d => d.DurakId == path[i + 1]);

            
                var connection = currentStop.SiradakiDurak.FirstOrDefault(n => n.StopId == nextStop.DurakId);

                if (connection != null)
                {
              
                    totalDistance += connection.Mesafe;
                }
                else if (currentStop.Transfer != null && currentStop.Transfer.AktarmaId == nextStop.DurakId)
                {
                   
                    totalDistance += 0.1;
                }
                else
                {
                    Konum currentKonum = new Konum(currentStop.Enlem, currentStop.Boylam);
                    Konum nextKonum = new Konum(nextStop.Enlem, nextStop.Boylam);
                    totalDistance += currentKonum.MesafeHesapla(nextKonum);
                }
            }

            string lastType = null;
            for (int i = 0; i < path.Count; i++)
            {
                var durak = durakListesi.First(d => d.DurakId == path[i]);
                string step = durak.DurakName;


                if (i > 0)
                {
                    string prevId = path[i - 1];
                    var prevDurak = durakListesi.First(d => d.DurakId == prevId);

           
                    bool isTransfer = false;
                    foreach (var transfer in transfers[durak.DurakId])
                    {
                        string[] parts = transfer.Split(new string[] { "->" }, StringSplitOptions.None);

                        if (parts[0] == prevId && parts[1] == durak.DurakId)
                        {
                            isTransfer = true;
                            break;
                        }
                    }

                    if (isTransfer || durak.Type != prevDurak.Type)
                    {
                        step = $"{step} (AKTARMA: {prevDurak.Type} -> {durak.Type})";
                    }
                }

                rota.adim.Add(step);
                lastType = durak.Type;
            }

            rota.ToplamCost = costs[end.DurakId];
            rota.ToplamDistance = totalDistance;
            rota.ToplamZaman = times[end.DurakId];

            return rota;
        }
    }
    }