<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="basicspecialcases.aspx.cs" Inherits="nphc.basicspecialcases" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>NPHCDA Monthly Reports Page</title>
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
            <a href="basicoptions.aspx" class="shiftLink">
        <asp:Label ID="Label23" runat="server" Text="Click here to go back to Options page" ForeColor="Maroon"></asp:Label>
    </a>
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
    <br />
    <center>
        <asp:Label ID="Label1" runat="server" 
            Text="This Page is Reserved For Possible Future Expansion" 
            Font-Names="Algerian" Font-Size="Larger"></asp:Label>
    </center>
    </div>
    </div>
    </div>
    <div id="footer">
    </div>
    </form>
</body>
</html>
