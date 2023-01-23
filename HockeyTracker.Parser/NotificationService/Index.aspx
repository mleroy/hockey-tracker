<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="NotificationService.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hockey Tracker</title>
    <style type="text/css">
        body {
            font-size: 10pt;
            font-family: 'Segoe UI';
            background-color: #fafafa;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

            table td {
                border: 1px solid black;
                padding: 5px;
            }

        img.teamImage {
            margin: 0 5px;
        }

        img.small {
            width: 25px;
            height: 25px;
        }

        .installations {
            font-size: large;
        }

        .yesterday {
            color: #A0A0A0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>App installations</h1>
        <div style="float: left;">
            <h3>Today</h3>
            <div class="installations"><%: InstallationsToday %></div>
        </div>
        <div class="yesterday" style="float: right;">
            <h3>Yesterday</h3>
            <div class="installations"><%: InstallationsYesterday%></div>
        </div>
        <br style="clear: both;" />
        <h1>Newest users</h1>
        <div runat="server" id="AppInstallations" />
        <table id="UsersTable" runat="server">
            <tr>
                <th>UserId
                </th>
                <th>Teams
                </th>
                <th>Last Updated
                </th>
            </tr>
        </table>
        <h1>Last notifications</h1>
        <table id="HistoryTable" runat="server">
            <tr>
                <th>Time
                </th>
                <th>Title
                </th>
                <th>Message
                </th>
                <th>Count
                </th>
            </tr>
        </table>
        <h1>Teams</h1>
        <table id="Teams" runat="server">
            <tr>
                <th>Team
                </th>
                <th>Goals subscribers
                </th>
                <th>Final scores subscribers
                </th>
                <th>Live Tile subscribers
                </th>
            </tr>
        </table>
    </form>
</body>
</html>
