<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs"
    Inherits="NotificationService.Notifications" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hockey Tracker</title>
        <style type="text/css">
        body
        {
            font-size: 10pt;
            font-family: Verdana;
        }
        table
        {
            width: 100%;
            border-collapse: collapse;
        }
        
        table td
        {
            border: 1px solid black;
            padding: 5px;
        }
        
        img.teamImage
        {
            margin: 0 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h2>
        Send next game info</h2>
    <table>
        <tr>
            <td>
                Team short name:
            </td>
            <td>
                <input type="text" id="TileNotif_TeamShortName" runat="server" style="width: 100%;" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="SendTileNotif" Text="WOOP WOOP!" runat="server" Style="padding: 5px;" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
