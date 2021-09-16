<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="basicoptions.aspx.cs" Inherits="Primary_Healthcare.basicoptions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>NPHCDA Options Page</title>
    <link href="myCss.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="logoicon.png" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="page">
    <div id="banner">
    <div id="topbg">
    <span class="topLabel">
        <asp:Label ID="phcLabel" runat="server" Text="Label" ForeColor="White" 
            Font-Bold="True"></asp:Label>
    </span>

    <span class="date">
        <asp:Label ID="phcDate" runat="server" Text="Label" ForeColor="White" 
            Font-Bold="True"></asp:Label>
    </span>
    <br />
    <br />
    <br />
    <br />
    <br />
        <asp:LinkButton ID="logOut" runat="server" CssClass="signOut" 
            Font-Bold="True" ForeColor="White" Font-Size="Small" 
            PostBackUrl="~/index.aspx" onclick="logOut_Click">Click Here To Logout of Your Profile</asp:LinkButton>
        <asp:Label ID="temp" runat="server" Text="" Visible="False"></asp:Label>       
    <br />
    <br />
        <asp:Label ID="theUser" runat="server" Text="Label" CssClass="forNameDisplay"></asp:Label>
    <br />
    <br />
    <br />
    <center> 
        <asp:Image ID="Image5" runat="server" ImageUrl="menu.png" /> 
        </center>
    <center>
    <div id = "buttons1">
    <div class="space">
    <a href = "basicreportpage.aspx"> <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/medicalIncident.png"/> </a>
    </div>
    <div class="space">
    </div>
    <a href = "basicreportdisplaypage.aspx"> <asp:Image ID="Image3" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/siteReport.png"/>  </a>
    <div class="space">
    </div>
    <a href = "basicspecialcases.aspx"> <asp:Image ID="Image4" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/specialCases.png"/>  </a>
    </div>
    </center>
    </div>
    </div>
    </div>
    <div id="footer">
    
    </div>
    </form>
</body>
</html>
