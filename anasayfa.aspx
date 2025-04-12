<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="anasayfa.aspx.cs" Inherits="prolab2.anasayfa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />  
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
        <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css" />

<title>Ulaşım Planlayıcısı</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f5f5f5;
        }
        h1 {
            color: #2c3e50;
            text-align: center;
            margin-bottom: 30px;
        }
        .form-container {
            background-color: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }
        .form-group {
            margin-bottom: 20px;
        }
        label {
            display: block;
            margin-bottom: 8px;
            font-weight: 600;
            color: #444;
        }
        select, input {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 16px;
        }
        .coordinates-section {
            display: flex;
            gap: 15px;
            margin-bottom: 15px;
        }
        .coordinates-section div {
            flex: 1;
        }
        button {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 12px 20px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            font-weight: 600;
            width: 100%;
            transition: background-color 0.3s;
        }
        button:hover {
            background-color: #2980b9;
        }
        .alert {
            padding: 10px;
            background-color: #f8d7da;
            color: #721c24;
            border-radius: 4px;
            margin-bottom: 20px;
            display: none;
        }
    </style>
</head>
<body>
    <h1>Şehir Ulaşım Planlayıcısı</h1>
    
    <div class="form-container">
        <div id="Div1" class="alert" runat="server" Visible="false"></div>
        
        <form id="Form1" runat="server">
            <div class="form-group">
                <label for="passenger-type">Yolcu Tipinizi Seçin:</label>
                <asp:DropDownList ID="passengerType" runat="server">
                    <asp:ListItem Text="Normal Yolcu" Value="1" />
                    <asp:ListItem Text="Öğrenci" Value="2" />
                    <asp:ListItem Text="Yaşlı" Value="3" />
                    <asp:ListItem Text="Gazi" Value="4" />
                </asp:DropDownList>
            </div>
            
            <div class="form-group">
                <label for="payment-method">Ödeme Yönteminizi Seçin:</label>
                <asp:DropDownList ID="paymentMethod" runat="server">
                    <asp:ListItem Text="Nakit (Sadece taksi için geçerli)" Value="1" />
                    <asp:ListItem Text="Kredi Kartı (Taksi ve toplu taşıma için geçerli)" Value="2" />
                    <asp:ListItem Text="Kentkart (Sadece toplu taşıma için geçerli)" Value="3" />
                </asp:DropDownList>
            </div>
            
            <div class="form-group">
                <label>Başlangıç Koordinatları:</label>
                <div class="coordinates-section">
                    <div>
                        <label for="start-lat">Enlem:</label>
                        <asp:TextBox ID="startLat" runat="server" placeholder="Örn: 38.4192"></asp:TextBox>
                    </div>
                    <div>
                        <label for="start-lon">Boylam:</label>
                        <asp:TextBox ID="startLon" runat="server" placeholder="Örn: 27.1287"></asp:TextBox>
                    </div>
                </div>
            </div>
            
            <div class="form-group">
                <label>Hedef Koordinatları:</label>
                <div class="coordinates-section">
                    <div>
                        <label for="end-lat">Enlem:</label>
                        <asp:TextBox ID="endLat" runat="server" placeholder="Örn: 38.4560"></asp:TextBox>
                    </div>
                    <div>
                        <label for="end-lon">Boylam:</label>
                        <asp:TextBox ID="endLon" runat="server" placeholder="Örn: 27.2122"></asp:TextBox>
                    </div>
                </div>
            </div>
            
            <asp:Button ID="submitButton" runat="server" Text="Rota Hesapla" OnClick="OnSubmitClick" />
        </form>
    </div>
</body>
</html>