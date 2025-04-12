<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sonuclar.aspx.cs" Inherits="prolab2.sonuclar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sonuçlar</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style>
        .result-card {
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .result-header {
            background-color: #f5f5f5;
            padding: 10px;
            margin-bottom: 15px;
            border-radius: 5px;
        }
        .result-row {
            padding: 8px 0;
            border-bottom: 1px solid #eee;
        }
        .route-steps {
            margin-left: 20px;
        }
        .route-summary {
            background-color: #f8f9fa;
            padding: 15px;
            margin-top: 20px;
            border-radius: 5px;
        }
        .alternative-route {
            background-color: #e9ecef;
            padding: 15px;
            margin-top: 20px;
            border-radius: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1 class="text-center mt-4 mb-4">Rota Sonuçları</h1>
            
            <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
            
            <asp:Panel ID="pnlResults" runat="server" Visible="false">
                <div class="card result-card">
                    <div class="result-header">
                        <h2>Yolculuk Bilgileri</h2>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Başlangıç Koordinatları:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblStartCoords" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Bitiş Koordinatları:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblEndCoords" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Yolcu Tipi:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblYolcuTipi" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Ödeme Tipi:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblOdemeTipi" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Tarih:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblTarih" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="result-header mt-4">
                        <h2>Rota Tipi</h2>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Kullanılan Ulaşım:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblRouteType" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Açıklama:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblRouteTypeInfo" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="result-header mt-4">
                        <h2>Mesafe ve Ücret Bilgileri</h2>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Doğrudan Mesafe:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblDirectDistance" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Toplam Rota Mesafesi:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblRouteDistance" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Tahmini Süre:</strong></div>
                        <div class="col-md-8"><asp:Label ID="lblEstimatedTime" runat="server"></asp:Label></div>
                    </div>
                    
                    <div class="row result-row">
                        <div class="col-md-4"><strong>Tahmini Ücret:</strong></div>
                        <div class="col-md-8">
                            <asp:Label ID="lblEstimatedCost" runat="server"></asp:Label>
                            <asp:Label ID="lblDiscount" runat="server" CssClass="text-muted ml-2" Font-Italic="true"></asp:Label>
                        </div>
                    </div>
                    
                    <div class="result-header mt-4">
                        <h2>Rota Detayları</h2>
                    </div>
                    
                    <asp:Literal ID="litRouteInfo" runat="server"></asp:Literal>
                    
                    <asp:Panel ID="pnlAlternativeRoute" runat="server" Visible="false" CssClass="mt-4">
                        <asp:Literal ID="litAlternativeRoute" runat="server"></asp:Literal>
                    </asp:Panel>
                </div>
                
                <div class="text-center mt-4 mb-5">
                    <a href="anasayfa.aspx" class="btn btn-primary">Yeni Rota Hesapla</a>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>