<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reportpage.aspx.cs" Inherits="Primary_Healthcare.reportPage" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Reporting Page</title>
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
    <div id="reportPage">
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
    <br />
        <asp:Label ID="instructionOne" runat="server" 
        Text="1 - Enter The Patient Details:" CssClass="instructions" 
            Font-Size="Large"></asp:Label>
     <br />
     <br />
     <br />
                      <table class="fieldLabels" width="400" cellpadding="1" 
            cellspacing="1">
                      <tr>
                      <td class="labels">
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                              ErrorMessage="Patient Name Required" ControlToValidate="patientName" 
                              ValidationGroup="A">*</asp:RequiredFieldValidator>Patient's Name:
                      </td>
                      
                      <td>
                          <asp:TextBox ID="patientName" runat="server" Width="174px"></asp:TextBox>
                          
                      </td>
                      </tr>
                      
                      
                      <tr>
                      <td class="labels">
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                              ControlToValidate="patientAge" ErrorMessage="Patient Age Required" 
                              ValidationGroup="A">*</asp:RequiredFieldValidator>Patient's Age:
                      </td>
                      <td>
                          <asp:TextBox ID="patientAge" runat="server" Width="174px" MaxLength="3" onkeyup = "javascript:ValidateText(this)"></asp:TextBox>
                          
                      </td>
                      </tr>
                      
                      
                      <tr>
                      <td class="labels">
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                              ControlToValidate="patientSex" ErrorMessage="Patient Sex Required" 
                              ValidationGroup="A">*</asp:RequiredFieldValidator>Patient's Sex:
                      </td>
                      <td>
                          <asp:TextBox ID="patientSex" runat="server" Width="174px" MaxLength="1"></asp:TextBox>
                          
                      </td>
                      </tr>
                      
                      
                      
                      <tr>
                      <td class="labels">
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                              ControlToValidate="patientAddress" ErrorMessage="Patient Address Required" 
                              ValidationGroup="A">*</asp:RequiredFieldValidator>Patient's Address:
                      </td>
                      <td>
                          <asp:TextBox ID="patientAddress" runat="server" Height="60px" 
                              TextMode="MultiLine" Width="174px"></asp:TextBox>
                          
                      </td>
                      </tr>
                      
                      
                      <tr>
                      <td class="labels">
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                              ControlToValidate="patientRegistrationNo" 
                              ErrorMessage="Patient Registration No Required" ValidationGroup="A">*</asp:RequiredFieldValidator>Patient's Registration No:
                      </td>
                      <td>
                          <asp:TextBox ID="patientRegistrationNo" runat="server" Width="174px"></asp:TextBox>
                          
                      </td>
                      </tr>
                      </table>
                      
                      
        <asp:Panel ID="Panel1" runat="server"  Height="150px" 
            Width="300px" align="Right" CssClass="panelRight">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" align="left" 
                ValidationGroup="A"/>    
        </asp:Panel>

    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <table>
    <tr>
    <td class="instruction_2">
        <asp:Label ID="instruction2" runat="server" 
            Text="2 - Please Select The Medical Section:" Font-Size="Large" ></asp:Label>
    </td>
    <td>
        <asp:DropDownList ID="reportSection" runat="server" Width="200px" 
            AutoPostBack="True" 
            onselectedindexchanged="reportSection_SelectedIndexChanged" 
            CssClass="shiftlistbox">
            <asp:ListItem>Choose One...</asp:ListItem>
            <asp:ListItem>Antenatal</asp:ListItem>
            <asp:ListItem>Child Death</asp:ListItem>
            <asp:ListItem>Commodities</asp:ListItem>
            <asp:ListItem>Deliveries</asp:ListItem>
            <asp:ListItem>Maternal Death</asp:ListItem>
            <asp:ListItem>Outreaches</asp:ListItem>
            <asp:ListItem>Referrals</asp:ListItem>
            <asp:ListItem>Routine Immunization</asp:ListItem>
        </asp:DropDownList>
    </td>
    </tr>
    </table>
    <br />
    <br />
        <asp:Label ID="instructionThree" runat="server" 
            Text="3 - Please Select The Report Item: " CssClass="instruction_2" 
            Font-Size="Large"></asp:Label>
        <asp:DropDownList ID="reportItem" runat="server" CssClass="shiftlistbox" Width="400px" 
                          onselectedindexchanged="reportItem_SelectedIndexChanged" 
                          AutoPostBack="True">
        </asp:DropDownList>
        <br />
        <br />
        <div id="motherChild">
        <asp:Label ID="childMotherMortalityLabel" runat="server" Text="Label" 
                          CssClass="instruction_3" Font-Size="Large"></asp:Label>&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="childMotherMortalityOptions" runat="server" Width="410px" 
                AutoPostBack="True" 
                onselectedindexchanged="childMotherMortalityOptions_SelectedIndexChanged">
        </asp:DropDownList>
        </div>
        <br />
        <br />
        <center>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                ErrorMessage="Event Date Missing" Text="*" ControlToValidate="eventDate" 
                CssClass="instruction_2b" ValidationGroup="A">*</asp:RequiredFieldValidator>
        <asp:Label ID="lastInstruction" runat="server" Text="" 
            Font-Size="Large" CssClass="instruction_2a"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                                
        <asp:TextBox ID="eventDate" runat="server" Width="175px"></asp:TextBox>
        </center>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <cc1:CalendarExtender ID="CalendarExtender1" runat="server"  
            TargetControlID="eventDate">
        </cc1:CalendarExtender>
        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" 
            TargetControlID="eventDate" WatermarkText="Click To Select Date" WatermarkCssClass="Fordate">
        </cc1:TextBoxWatermarkExtender>
        <br />
        <br />
        <center>
                    <asp:Button ID="submitEntry" runat="server" 
                Text="Please Validate Your Inputs And Click Here To Submit" Height="35px" Enabled="False" 
                onclick="submitEntry_Click" ValidationGroup="A" />
        </center>
    </div>
    </div>
    </div>
    <div id="footer">
    </div>
    </form>
</body>
</html>
