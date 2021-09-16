<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Primary_Healthcare.index" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>NPHCDA Login Page</title>
    <link href="myCss.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="logoicon.png" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="forloginpage">
    <div id="banner">
    <div id="topbg">
    </div>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
        <center><asp:Image ID="Image1" runat="server" ImageUrl="~/loginBar.png" /></center>
        <br />
        <br />
        <center><asp:Login ID="Login1" runat="server" BackColor="#CCFFCC" BorderColor="#E6E2D8" 
            BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
            Font-Size="0.8em" ForeColor="#333333" TextLayout="TextOnTop" 
            FailureText="Unsuccessful Login. Please try again.">
            <TextBoxStyle Font-Size="0.8em" />
            <LayoutTemplate>
                <table border="0" cellpadding="4" cellspacing="0" 
                    style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" 
                                        style="color:White;background-color:#1C5E55;font-size:0.9em;font-weight:bold;">
                                        Log In</td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" >User 
                                        Name:</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                            ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Password is required." 
                                            ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="LoginButton" runat="server" BackColor="White" 
                                            BorderColor="#C5BBAF" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" 
                                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#1C5E55" Text="Log In" 
                                            ToolTip="Click to login" ValidationGroup="Login1" 
                                            onclick="LoginButton_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <LoginButtonStyle BackColor="White" BorderColor="#C5BBAF" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#1C5E55" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TitleTextStyle BackColor="#1C5E55" Font-Bold="True" Font-Size="0.9em" 
                ForeColor="White" />
        </asp:Login>
        </center>
    </div> 
            
    <div id="footer">
    </div>
    </form>
</body>
</html>
