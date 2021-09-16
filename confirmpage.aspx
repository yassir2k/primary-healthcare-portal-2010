<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="confirmpage.aspx.cs" Inherits="Primary_Healthcare.confirmPage" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Confirm Page</title>
    <link href="myCss.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="logoicon.png" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="page">
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
    <img src = "bar.png" />
    </center>
    <br />
    <br />
    <center>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                        BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" 
                        CellPadding="2" CellSpacing="2" DataKeyNames="User_ID,Site_ID" 
                        DataSourceID="SqlDataSource1" ForeColor="#777777" Width="750px">
                        <RowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="User_ID" HeaderText="User ID" ReadOnly="True" 
                                SortExpression="User_ID" />
                            <asp:BoundField DataField="User_Name" HeaderText="User Name" 
                                SortExpression="User_Name" />
                            <asp:BoundField DataField="Role" HeaderText="Designation" 
                                SortExpression="Role" />
                            <asp:BoundField DataField="Telephone" HeaderText="Telephone" 
                                SortExpression="Telephone" />
                            <asp:BoundField DataField="Site_ID" HeaderText="Site ID" 
                                SortExpression="Site_ID" ReadOnly="True" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#777777" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:NPHCDAConnectionString %>" 
                        SelectCommand="SELECT [User_ID], [User_Name], [Role], [Telephone], [Site_ID] FROM [Users] WHERE ([User_ID] = @User_ID)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="temp" Name="User_ID" PropertyName="Text" 
                                Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    </center>
                   <br />
                   <br />
                   <br />
                   <center>
        <asp:Button ID="confirmButton" runat="server" 
                           Text="Click To Confirm Above Details And Proceed >>" Height="40px" 
                           Font-Names="Copperplate Gothic Light" onclick="confirmButton_Click" 
            />
    </center>
    </div>
    </div>
    </div>
    <div id="footer">
    </div>
    </form>
</body>
</html>
