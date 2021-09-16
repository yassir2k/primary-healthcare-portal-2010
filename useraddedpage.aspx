<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="useraddedpage.aspx.cs" Inherits="Primary_Healthcare.useraddedPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>New User Added</title>
    <link href="myCss.css" rel="stylesheet" type="text/css" />   
    <link rel="shortcut icon" href="logoicon.png" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div id="forloginpage">
    <div id="banner">
    <div id="topbgAdmin">
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
        <a href="optionsPage.aspx" class="shiftLink">
        <asp:Label ID="Label2" runat="server" Text="Click here to go back to Options page" ForeColor="Maroon"></asp:Label>
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
     <br />
    <br />
    <br />
    <br />
    <center> 
        <asp:Label ID="newUserLabel" runat="server" Text="Label" Font-Names="Castellar" 
            ForeColor="Red"></asp:Label>
            <br />
            <br />
        <asp:Button ID="gobackMenuButton" runat="server" 
            Text="Click to go back to Menu Page >>" onclick="gobackMenuButton_Click" />
             </center>
    </div>
    </div>
    </div>
    <div id="footer">
    </div>
    </form>
</body>
</html>
