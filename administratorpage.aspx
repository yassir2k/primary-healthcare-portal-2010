<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="administratorpage.aspx.cs" Inherits="Primary_Healthcare.administratorPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>NPHCDA Administrator Page</title>
<link href="myCss.css" rel="stylesheet" type="text/css" />
<link rel="shortcut icon" href="logoicon.png" />
</head>
<script type ="text/javascript" language="javascript" >
function ValidateText(i)
{
    if (i.value.length > 0 && i.value.length !=11)
    {
    i.value = i.value.replace(/[^\d]+/g, '');
    }
}
</script>
<body>
    <form id="form1" runat="server">
    <div id="adminPage">
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
    
            <asp:Label ID="theUser" runat="server" Text="Label" CssClass="forNameDisplay" 
            Font-Size="Large"></asp:Label>  
    <br />
    <br />
    <center>
        <asp:Panel ID="Panel1" runat="server" BackColor="Silver" Height="405px" 
            Width="450px">
            <center>
            <asp:Label ID="Label1" runat="server" Text="Create A New User" Font-Names="Algerian"></asp:Label></center>
            <br />
            <table style="width: 100%;" cellpadding="2" cellspacing="2">
                <tr>
                    <td align="left">
                        User ID:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="userID" runat="server" Width="180px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="User ID required" ControlToValidate="userID" 
                            ValidationGroup="B">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        User Name:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="userName" runat="server" Width="180px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="User Name required" ControlToValidate="userName" 
                            ValidationGroup="B">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        User Date of Birth:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="userDOB" runat="server" Width="180px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ErrorMessage="User's date of birth required" ControlToValidate="userDOB" 
                            ValidationGroup="B">*</asp:RequiredFieldValidator>
                    </td>
                </tr>                
                <tr>
                    <td align="left">
                        User Role:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="userRole" runat="server" Width="187px">
                            <asp:ListItem>Choose One...</asp:ListItem>
                            <asp:ListItem>Administrator</asp:ListItem>
                            <asp:ListItem>Basic</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                    <tr>
                    <td align="left">
                        User Telephone:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="userTelephone" runat="server" Width="180px" onkeyup = "javascript:ValidateText(this)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        User Password:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="userPassword" runat="server" Width="180px"></asp:TextBox>
                    </td>
                </tr> 
                </table>
            <br />
            <center>
            <asp:Button ID="createUser" runat="server" Text="Create User" 
                onclick="createUser_Click" ValidationGroup="B" /></center>
                <br />
                <center>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" align="middle"
                ValidationGroup="B" />
                </center>
        </asp:Panel>
        <br />
        <br />
        </center>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                  <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                      TargetControlID="userDOB">
                  </cc1:CalendarExtender>        
        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
            TargetControlID="userDOB" WatermarkText="Click here to select date" 
            WatermarkCssClass="Fordate">
        </cc1:TextBoxWatermarkExtender>
        <br />
    </div>
    </div>
    </div>
    <div id="footer">
    </div>
    </form>
</body>
</html>
