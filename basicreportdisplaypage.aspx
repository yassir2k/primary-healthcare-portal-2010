<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="basicreportdisplaypage.aspx.cs" Inherits="Primary_Healthcare.basicreportdisplaypage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>NPHCDA Report Viewing Page</title>
    <link href="myCss.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="logoicon.png" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="displayReportPage">
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
        <asp:Label ID="Label47" runat="server" Text="Click here to go back to Options page" ForeColor="Maroon"></asp:Label>
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
    <center>
        <asp:Panel ID="Panel1" runat="server" BackColor="White" Width="880px" 
            Height="2300px">
            
            <asp:Label ID="panelTip" runat="server" Text="SITE MEDICAL REPORT" 
                Font-Names="Copperplate Gothic Bold" ForeColor="Red" Font-Size="X-Large"></asp:Label>
            <br />
            <br />
            <asp:Table ID="TableA" runat="server" align="Left" CellPadding="2" 
                CellSpacing="2">
                <asp:TableRow ID="TableRow1" runat="server"> 
                <asp:TableCell align="left">
                    <asp:Label ID="Label1" runat="server" Text="SITE IDENTIFICATION" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                </asp:TableCell>
                </asp:TableRow> 
                <asp:TableRow ID="TableRow2" runat="server">
                <asp:TableCell align="left">
                 <asp:Label ID="Label2" runat="server" Text="Facility Code" Font-Bold="True"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result1" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>
                </asp:TableRow> 
                <asp:TableRow ID="TableRow3" runat="Server">
                <asp:TableCell align="left">
                <asp:Label ID="Label7" runat="server" Text="Facility Name" Font-Bold="True"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result2" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow4" runat="server">
                <asp:TableCell align="left">
                <asp:Label ID="Label6" runat="server" Text="Name Of Reporting Officer" Font-Bold="True"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result3" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow5" runat="server">
                <asp:TableCell align="left">
                <asp:Label ID="Label5" runat="server" Text="Date Of Reporting" Font-Bold="True"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result4" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow6" runat="server">
                <asp:TableCell align="left">
                <asp:Label ID="Label3" runat="server" Text="Name Of Ward" Font-Bold="True"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result5" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow7" runat="server">
                <asp:TableCell align="left">
                <asp:Label ID="Label8" runat="server" Text="Name Of LGA" Font-Bold="True"></asp:Label>                
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result6" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow8" runat="server">
                <asp:TableCell align="left">
                <asp:Label ID="Label4" runat="server" Text="Name Of State" Font-Bold="True"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result7" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow9" runat="server">
                <asp:TableCell align="left">
                <asp:Label ID="Label9" runat="server" Text="Name Of Zone" Font-Bold="True"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result8" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow10" runat="server">
                <asp:TableCell align="left">
                <asp:Label ID="Label10" runat="server" Text="Reporting Period" Font-Bold="True"></asp:Label>
                </asp:TableCell>
                <asp:TableCell align="left">
                <asp:Label ID="result9" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
                </asp:TableCell>                                                                                                                                                               
                </asp:TableRow>
            </asp:Table>
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
            <br />
            <br />            
            <br />
            <br />                                                            
            <!-- Done With Facility Identification -->
            <!-- Now Its Antenatal Care & Pregnancy Outcome -->
            <asp:Table ID="Table2" runat="server" align="Left">
            <asp:TableRow ID="TableRow11" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label11" runat="server" Text="ANTENATAL CARE & PREGNANCY OUTCOME" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result10" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow12" runat="Server">
            <asp:TableCell align="left">
                <asp:Label ID="Label12" runat="server" Text="Total No. of Antenatal Attendance" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result11" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow13" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label13" runat="server" Text="No. of New Antenatal Attendance" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result12" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow14" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label14" runat="server" Text="No. of Pregnant Women with 4 & Above Antenatal Visits" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result13" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow15" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label15" runat="server" Text="Total No. of Births" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result14" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow16" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label16" runat="server" Text="No. of Live Births" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result15" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow17" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label17" runat="server" Text="No. of Live Births With Weight < 2.5kg" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result16" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow18" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label18" runat="server" Text="No. of Live Births With Weight > 2.5kg" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result17" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow19" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label19" runat="server" Text="No. of Still Births" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result18" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow20" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label20" runat="server" Text="No. of Abortions" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result19" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow21" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label21" runat="server" Text="Total No. of Deliveries" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result20" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow22" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label22" runat="server" Text="No. of Deliveries By Skilled Birth Attendants" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result21" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>                                                                                                                                    
            </asp:Table>
            <!-- Done With Antenatal Care & Pregnancy Outcome -->
            <!-- Now Maternal Mortality -->
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
            <br />
            <br />            
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Table ID="Table1" runat="Server" align="Left">
            <asp:TableRow ID="TableRow23" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label23" runat="server" Text="MATERNAL MORTALITY" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow24" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label24" runat="server" Text="Total No. of Maternal Deaths" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result22" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow25" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label25" runat="server" Text="No. of Maternal Deaths in the PHC Facility" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result23" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow26" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label26" runat="server" Text="No. of Maternal Deaths in the Target Community" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result24" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow27" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label27" runat="server" Text="No. of Maternal Deaths due To Ante partum Haemorrhage" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result25" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow28" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label28" runat="server" Text="No. of Maternal Deaths due To Post partum Haemorrhage" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result26" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow29" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label29" runat="server" Text="No. of Maternal Deaths due To Obstructed Labour" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result27" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow30" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label30" runat="server" Text="No. of Maternal Deaths due To Sepsis" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result28" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow31" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label31" runat="server" Text="No. of Maternal Deaths due To Eclampsia" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result29" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow32" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label32" runat="server" Text="No. of Maternal Deaths due To other causes" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result30" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            </asp:Table>   
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
            <br />
            <br />            
            <br />
            <br />
            <!-- Done with Maternal Mortality -->
            <!-- Now, The Beginning Of Child Mortality -->
            <asp:Table ID="Table3" runat="server" align="Left">
            <asp:TableRow ID="TableRow33" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label33" runat="server" Text="CHILD MORTALITY" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            </asp:TableCell>
            <asp:TableCell align="left">
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow34" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label34" runat="server" Text="A." Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label35" runat="server" Text="Total No. of Deaths in babies aged 0-28days" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result31" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow35" runat="Server">
            <asp:TableCell align="left">
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label37" runat="server" Text="No. of Deaths in babies due to Sepsis" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result32" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow36" runat="Server">
            <asp:TableCell align="left">
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label36" runat="server" Text="No. of Deaths in babies due to Asphyxia" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result33" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow> 
            <asp:TableRow ID="TableRow37" runat="Server">
            <asp:TableCell align="left">
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label38" runat="server" Text="No. of Deaths in babies due to Neonatal Tetanus" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result34" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowA" runat="Server">
            <asp:TableCell align="left">
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label39" runat="server" Text="No. of Deaths in babies due to other causes" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result35" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>   
            <asp:TableRow ID="TableRowB" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label41" runat="server" Text="&nbsp;" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label42" runat="server" Text=" " Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label43" runat="server" Text=" " Font-Bold="True"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowD" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label40" runat="server" Text="B." Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label44" runat="server" Text="Total No. of Deaths in babies aged 1-11 months " Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result36" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow> 
            <asp:TableRow ID="TableRowE" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label45" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label46" runat="server" Text="No. of Deaths in babies due to Pneumonia" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result37" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowF" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label48" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label49" runat="server" Text="No. of Deaths in babies due to Malaria" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result38" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowG" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label51" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label52" runat="server" Text="No. of Deaths in babies due to Diarrhoea diseases" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result39" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowH" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label54" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label55" runat="server" Text="No. of Deaths in babies due to Malnutrition" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result40" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowI" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label57" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label58" runat="server" Text="No. of Deaths in babies due to Measles" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result41" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>    
            
            
            <asp:TableRow ID="TableRowJ" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label60" runat="server" Text="&nbsp;" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label61" runat="server" Text=" " Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label62" runat="server" Text=" " Font-Bold="True"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow47" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label63" runat="server" Text="C." Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label64" runat="server" Text="Total No. of Deaths in babies aged 12-59 months " Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result42" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow> 
            <asp:TableRow ID="TableRowL" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label65" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label66" runat="server" Text="No. of Deaths in babies due to Pneumonia" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result43" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowM" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label68" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label69" runat="server" Text="No. of Deaths in babies due to Malaria" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result44" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow38" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label71" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label72" runat="server" Text="No. of Deaths in babies due to Diarrhoea diseases" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result45" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow39" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label74" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label75" runat="server" Text="No. of Deaths in babies due to Malnutrition" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result46" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowC" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label77" runat="server" Text="" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="Label78" runat="server" Text="No. of Deaths in babies due to Measles" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result47" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>                                                                                                                            
            </asp:Table>  
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
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />                      
        <asp:Table runat="Server" align="Left">
        <asp:TableRow runat="Server">
        <asp:TableCell align="left">
        <asp:Label ID="Label80" runat="server" Text="ROUTINE IMMUNIZATION" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
        </asp:TableCell>
        <asp:TableCell align="left">
        </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow41" runat="Server">
        <asp:TableCell align="left">
        <asp:Label ID="Label81" runat="server" Text="No. Of Children given BCG" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell align="left">
        <asp:Label ID="result48" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow42" runat="Server">
        <asp:TableCell align="left">
        <asp:Label ID="Label82" runat="server" Text="No. Of Infants given DPT1 Vaccinations" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell align="left">
        <asp:Label ID="result49" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="TableRow43" runat="Server">
        <asp:TableCell align="left">
        <asp:Label ID="Label83" runat="server" Text="No. Of Infants given DPT3 Vaccinations" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell align="left">
        <asp:Label ID="result50" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="TableRow44" runat="Server">
        <asp:TableCell align="left">
        <asp:Label ID="Label84" runat="server" Text="No. Of Infants given Measles Vaccinations" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell align="left">
        <asp:Label ID="result51" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="TableRow45" runat="Server">
        <asp:TableCell align="left">
        <asp:Label ID="Label85" runat="server" Text="No. Of Children 0 - 59 months given Vitamin A" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell align="left">
        <asp:Label ID="result52" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>    
        <asp:TableRow ID="TableRow46" runat="Server">
        <asp:TableCell align="left">
        <asp:Label ID="Label86" runat="server" Text="No. Of Nursing Mothers given Vitamin A" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell align="left">
        <asp:Label ID="result53" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRowK" runat="Server">
        <asp:TableCell align="left">
        <asp:Label ID="Label87" runat="server" Text="No. Of Women that have received at least 2+ Doses of Tetanus Toxoid" Font-Bold="True"></asp:Label>
        </asp:TableCell>
        <asp:TableCell align="left">
        <asp:Label ID="result54" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
        </asp:TableCell>
        </asp:TableRow>                                 
        </asp:Table>
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
        <br />
        <br />        
            <asp:Table ID="Table4" runat="server" align="Left">
            <asp:TableRow runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label88" runat="server" Text="FAMILY PLANNING SERVICES" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            
            </asp:TableCell>
            </asp:TableRow>
            
            <asp:TableRow ID="TableRow48" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label89" runat="Server" Text="Total No. Of Women aged 15-49 Years attending Family Planning Clinics" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result55" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>   
            <asp:TableRow ID="TableRow49" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label90" runat="Server" Text="No. of Women aged 15-49 years using modern contraception" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result56" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow> 
            
            <asp:TableRow ID="TableRowN" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label91" runat="Server" Text="No. of New Acceptors Of a Family Planning Clinics" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result57" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>                                  
            </asp:Table>  
            
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Table ID="Table5" runat="server" align="Left">
            <asp:TableRow runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label92" runat="server" Text="REFERRALS TO DESIGNATED GENERAL HOSPITAL" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowO" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label93" runat="server" Text="Total No. of Clients Referred" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result58" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>    
            <asp:TableRow ID="TableRowP" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label94" runat="server" Text="No. of Women Referred for Emergency Obstetric Care" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result59" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow> 
            <asp:TableRow ID="TableRowQ" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label95" runat="server" Text="No. of Children 0-59 months Referred" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result60" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>   
            <asp:TableRow ID="TableRow54" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label96" runat="server" Text="Total No. of Feedback on Referrals received from General Hospital" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result61" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>                                         
            </asp:Table>    
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Table ID="Table6" runat="server" align="Left">
            <asp:TableRow runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label97" runat="server" Text="COMMUNITY OUTREACH SERVICES" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow55" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label98" runat="server" Text="No. of Outreaches conducted in the month" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result62" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow56" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label99" runat="server" Text="No. of Clients reached with Maternal Health Services" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result63" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow57" runat="server">
            <asp:TableCell align="left">
            <asp:Label ID="Label100" runat="server" Text="No. of Clients reached with Child Health Services" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result64" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>                                    
            </asp:Table>  
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Table ID="Table7" runat="server" align="Left">
            <asp:TableRow runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label101" runat="server" Text="DISTRIBUTION OF COMMODITIES" Font-Names="Copperplate Gothic Bold" ForeColor="Green" Font-Size="Larger"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow58" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label102" runat="server" Text="No. of Mama Kits in stock" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result65" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow59" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label103" runat="server" Text="No. of Mama kits distributed in the month" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result66" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow60" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label104" runat="server" Text="No. of personal health records in stock" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result67" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow61" runat="Server">
            <asp:TableCell align="left">
            <asp:Label ID="Label105" runat="server" Text="No. of personal health record distributed" Font-Bold="True"></asp:Label>
            </asp:TableCell>
            <asp:TableCell align="left">
            <asp:Label ID="result68" runat="server" Text="" Font-Bold="False" ForeColor="#CC0000"></asp:Label>
            </asp:TableCell>
            </asp:TableRow>                                                
            </asp:Table>                                                                                                                                         
        </asp:Panel>
    </center>
    </div>
    </div>
    </div>
    
    <div id="footer">
    </div>
    </form>
</body>
</html>
