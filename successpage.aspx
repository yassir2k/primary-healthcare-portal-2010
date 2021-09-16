<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="successpage.aspx.cs" Inherits="Primary_Healthcare.successPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Entry Confirmation Page</title>
    <link href="myCss.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="logoicon.png" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="reportPage">
    <div id="banner">
    <div id="topbgAdmin">
<span class="topLabel" >
        <asp:Label ID="phcLabel" runat="server" Text="Label" ForeColor="White" 
            Font-Bold="True"></asp:Label>
 </span>
<span class="date">
        <asp:Label ID="phcDate" runat="server" Text="Where Are You" ForeColor="White" 
            Font-Bold="True"></asp:Label>
</span>
    <br />
    <br />
        <a href="optionsPage.aspx" class="shiftLink">
        <asp:Label ID="Label23" runat="server" Text="Click here to go back to Options page" ForeColor="Maroon"></asp:Label>
    </a>
    <br />
    <br />
    <br />
        <asp:LinkButton ID="logOut" runat="server" CssClass="signOut" 
            Font-Bold="True" ForeColor="White" Font-Size="Small" 
            PostBackUrl="~/index.aspx" onclick="logOut_Click">Click Here To Logout of Your Profile</asp:LinkButton>
        <asp:Label ID="tempo" runat="server" Text="" Visible="False"></asp:Label>
    <br />
    <br />
        <asp:Label ID="theUser" runat="server" Text="Label" CssClass="forNameDisplay" 
            Font-Size="Large"></asp:Label>
    <br />
    <br />
        <asp:Label ID="theNote" runat="server" Text="You Have Successfully Submitted 
        Your Report With The Following Details Below:" CssClass="forNoteDisplay"></asp:Label>
    <br />
    <br />
    <center>
        <asp:Table ID="Table1" runat="server" CellPadding="5" CellSpacing="5">
        <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label1" runat="server" Text="Incident ID:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label2" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
        <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label21" runat="server" Text="Incident Date:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label22" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
        <asp:TableRow>
        <asp:TableCell>
                <asp:Label ID="Label3" runat="server" Text="Report's Clinical Section:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label4" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
        <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label13" runat="server" Text="Item Reported From Clinical Section Above:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label14" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
                <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label5" runat="server" Text="Full Name Of Patient Reported:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label6" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
                <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label7" runat="server" Text="ID Number Of Patient Reported:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label8" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
                <asp:TableRow>
        <asp:TableCell>
                <asp:Label ID="Label9" runat="server" Text="Report Submitted By:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label10" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
                <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label11" runat="server" Text="The Reporter's ID Number:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label12" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
                <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label15" runat="server" Text="Facility Name:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label16" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
                <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label17" runat="server" Text="Facility Location" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label18" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        
                <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label19" runat="server" Text="Submission Date:" Font-Names="Copperplate Gothic Light" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="Label20" runat="server" Text="Label" Font-Names="cursive" ForeColor="Maroon" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </center>

    <br />
    <center>
        <asp:Button ID="Button1" runat="server" 
            Text="Click To Go Back To Options Page <<"  Height="35px" 
            onclick="Button1_Click" Width="240px" />
    </center>    
    </div>
    </div>
    
    </div>
    <div id="footer">
    </div>
    </form>
</body>
</html>
