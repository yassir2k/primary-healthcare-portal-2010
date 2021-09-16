using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace Primary_Healthcare
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Object theDate = DateTime.Now.ToShortDateString();
            phcDate.Text = theDate.ToString();
            if (Session["Who_Is_it"] == null || Session["Role"] == null)
            {
                Response.Redirect("index.aspx");
            }
            else
            {
                string get_who = Session["Who_Is_It"].ToString();
                temp.Text = get_who;
                string DbConnectionStr = "Data Source=.\\SQLEXPRESS;Initial Catalog=NPHCDA;Integrated Security=SSPI";
                SqlConnection Connect = new SqlConnection(DbConnectionStr);
                try
                {
                    Connect.Open();
                }
                catch (System.Exception)
                {

                }
                finally
                {
                    SqlCommand facility = new SqlCommand("Select dbo.check_ID(@user)", Connect);
                    facility.Parameters.Add("@user", SqlDbType.NVarChar);
                    facility.Parameters["@user"].Value = get_who;
                    Object which_Facility = facility.ExecuteScalar();
                    phcLabel.Text = which_Facility.ToString();
                    //Authenticity
                    SqlCommand userRoleSite = new SqlCommand("Select dbo.userRoleSite(@ID,@Site)", Connect);
                    userRoleSite.Parameters.Add("@ID", SqlDbType.NVarChar);
                    userRoleSite.Parameters["@ID"].Value = get_who;
                    //Lets Get Site ID
                    SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@user)", Connect);
                    getSiteID.Parameters.Add("@user", SqlDbType.NVarChar);
                    getSiteID.Parameters["@user"].Value = get_who;
                    which_Facility = getSiteID.ExecuteScalar();
                    //Site ID
                    userRoleSite.Parameters.Add("@Site", SqlDbType.NVarChar);
                    userRoleSite.Parameters["@Site"].Value = which_Facility.ToString();
                    which_Facility = userRoleSite.ExecuteScalar();
                    int what = Convert.ToInt16(which_Facility.ToString());
                    if (what == 2 && Session["Role"].ToString() == "Administrator" && Session["Who_Is_It"].ToString() == get_who)
                    {
                        SqlCommand user = new SqlCommand("Select dbo.whichUser(@user)", Connect);
                        user.Parameters.Add("@user", SqlDbType.NVarChar);
                        user.Parameters["@user"].Value = get_who;
                        Object which_user = user.ExecuteScalar();
                        theUser.Text = "Welcome, " + which_user.ToString();
                        //Facility Code
                        SqlCommand getSiteID1 = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                        getSiteID1.Parameters.Add("@userID", SqlDbType.NVarChar);
                        getSiteID1.Parameters["@userID"].Value = get_who;
                        Object whichSite = getSiteID1.ExecuteScalar();
                        result1.Text = whichSite.ToString();
                        //Facility Name
                        result2.Text = which_Facility.ToString();
                        //Name of Reporting Officer
                        result3.Text = which_user.ToString();
                        //Date of Reporting
                        result4.Text = DateTime.Now.ToShortDateString();
                        //Name of Ward
                        SqlCommand getWardName = new SqlCommand("Select dbo.getWardName(@userID)", Connect);
                        getWardName.Parameters.Add("@userID", SqlDbType.NVarChar);
                        getWardName.Parameters["@userID"].Value = get_who;
                        Object whichWard = getWardName.ExecuteScalar();
                        result5.Text = whichWard.ToString();
                        //Name Of LGA
                        SqlCommand getLGA = new SqlCommand("Select dbo.getLGA(@userID)", Connect);
                        getLGA.Parameters.Add("@userID", SqlDbType.NVarChar);
                        getLGA.Parameters["@userID"].Value = get_who;
                        Object whichLGA = getLGA.ExecuteScalar();
                        result6.Text = whichLGA.ToString();
                        //Name Of State
                        SqlCommand getState = new SqlCommand("Select dbo.getState(@userID)", Connect);
                        getState.Parameters.Add("@userID", SqlDbType.NVarChar);
                        getState.Parameters["@userID"].Value = get_who;
                        Object whichState = getState.ExecuteScalar();
                        result7.Text = whichState.ToString();
                        //Name Of Zone
                        SqlCommand getZone = new SqlCommand("Select dbo.getZone(@userID)", Connect);
                        getZone.Parameters.Add("@userID", SqlDbType.NVarChar);
                        getZone.Parameters["@userID"].Value = get_who;
                        Object whichZone = getZone.ExecuteScalar();
                        result8.Text = whichZone.ToString();
                        //Total Number of Antenatal attendance
                        SqlCommand totalAntenatalAttendance = new SqlCommand("Select dbo.totalAntenatalAttendance(@ID)", Connect);
                        totalAntenatalAttendance.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalAntenatalAttendance.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalAntenatalAttendance = totalAntenatalAttendance.ExecuteScalar();
                        result11.Text = ThetotalAntenatalAttendance.ToString();
                        //Number Of New Antenatal Attendance
                        SqlCommand getNewAntenatalAttendance = new SqlCommand("Select dbo.getNewAntenatalAttendance(@ID)", Connect);
                        getNewAntenatalAttendance.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getNewAntenatalAttendance.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object TheNewAntenatalAttendance = getNewAntenatalAttendance.ExecuteScalar();
                        result12.Text = TheNewAntenatalAttendance.ToString();
                        //No Of Pregnant Women with 4 & Above Antenatal visits
                        SqlCommand pregnantWomenWithAbove4AV = new SqlCommand("Select dbo.pregnantWomenWithAbove4AV(@ID)", Connect);
                        pregnantWomenWithAbove4AV.Parameters.Add("@ID", SqlDbType.NVarChar);
                        pregnantWomenWithAbove4AV.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThepregnantWomenWithAbove4AV = pregnantWomenWithAbove4AV.ExecuteScalar();
                        result13.Text = ThepregnantWomenWithAbove4AV.ToString();
                        //Total No of Births
                        SqlCommand totalNoOfBirths = new SqlCommand("Select dbo.totalNoOfBirths(@ID)", Connect);
                        totalNoOfBirths.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalNoOfBirths.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalNoOfBirths = totalNoOfBirths.ExecuteScalar();
                        result14.Text = ThetotalNoOfBirths.ToString();
                        //No Of Live Birth
                        SqlCommand noOfLiveBirth = new SqlCommand("Select dbo.noOfLiveBirth(@ID)", Connect);
                        noOfLiveBirth.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfLiveBirth.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfLiveBirth = noOfLiveBirth.ExecuteScalar();
                        result15.Text = ThenoOfLiveBirth.ToString();
                        //Live Birth With Weight Less Than 2.5Kg
                        SqlCommand liveBirthLess2_5 = new SqlCommand("Select dbo.liveBirthLess2_5(@ID)", Connect);
                        liveBirthLess2_5.Parameters.Add("@ID", SqlDbType.NVarChar);
                        liveBirthLess2_5.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object TheliveBirthLess2_5 = liveBirthLess2_5.ExecuteScalar();
                        result16.Text = TheliveBirthLess2_5.ToString();
                        //Live Birth With Weight More Than 2.5Kg
                        SqlCommand liveBirthMore2_5 = new SqlCommand("Select dbo.liveBirthMore2_5(@ID)", Connect);
                        liveBirthMore2_5.Parameters.Add("@ID", SqlDbType.NVarChar);
                        liveBirthMore2_5.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object TheliveBirthMore2_5 = liveBirthMore2_5.ExecuteScalar();
                        result17.Text = TheliveBirthMore2_5.ToString();
                        //No Of Still Birth
                        SqlCommand noOfStillBirth = new SqlCommand("Select dbo.noOfStillBirth(@ID)", Connect);
                        noOfStillBirth.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfStillBirth.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfStillBirth = noOfStillBirth.ExecuteScalar();
                        result18.Text = ThenoOfStillBirth.ToString();
                        //No Of Abortions
                        SqlCommand noOfAbortions = new SqlCommand("Select dbo.noOfAbortions(@ID)", Connect);
                        noOfAbortions.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfAbortions.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfAbortions = noOfAbortions.ExecuteScalar();
                        result19.Text = ThenoOfAbortions.ToString();
                        //Total No. of Deliveries
                        SqlCommand totalNoOfDeliveries = new SqlCommand("Select dbo.totalNoOfDeliveries(@ID)", Connect);
                        totalNoOfDeliveries.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalNoOfDeliveries.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalNoOfDeliveries = totalNoOfDeliveries.ExecuteScalar();
                        result20.Text = ThetotalNoOfDeliveries.ToString();
                        //No Of Deliveries By Skilled Birth Attendants
                        SqlCommand noOfDeliveriesBySKA = new SqlCommand("Select dbo.noOfDeliveriesBySKA(@ID)", Connect);
                        noOfDeliveriesBySKA.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfDeliveriesBySKA.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfDeliveriesBySKA = noOfDeliveriesBySKA.ExecuteScalar();
                        result21.Text = ThenoOfDeliveriesBySKA.ToString();
                        //Total No. Of Maternal Death
                        SqlCommand totalNoOfMaternalDeath = new SqlCommand("Select dbo.totalNoOfMaternalDeath(@ID)", Connect);
                        totalNoOfMaternalDeath.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalNoOfMaternalDeath.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalNoOfMaternalDeath = totalNoOfMaternalDeath.ExecuteScalar();
                        result22.Text = ThetotalNoOfMaternalDeath.ToString();
                        //No. of Maternal Death In The PHC Facility
                        SqlCommand noOfMaternalDeathPHC = new SqlCommand("Select dbo.noOfMaternalDeathPHC(@ID)", Connect);
                        noOfMaternalDeathPHC.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMaternalDeathPHC.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMaternalDeathPHC = noOfMaternalDeathPHC.ExecuteScalar();
                        result23.Text = ThenoOfMaternalDeathPHC.ToString();
                        //No of Maternal Death In The Target Community
                        SqlCommand noOfMaternalDeathTC = new SqlCommand("Select dbo.noOfMaternalDeathTC(@ID)", Connect);
                        noOfMaternalDeathTC.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMaternalDeathTC.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMaternalDeathTC = noOfMaternalDeathTC.ExecuteScalar();
                        result24.Text = ThenoOfMaternalDeathTC.ToString();
                        //No. of Maternal Deaths due To Ante partum Haemorrhage
                        SqlCommand noOfMaternalantePartum = new SqlCommand("Select dbo.noOfMaternalantePartum(@ID)", Connect);
                        noOfMaternalantePartum.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMaternalantePartum.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMaternalantePartum = noOfMaternalantePartum.ExecuteScalar();
                        result25.Text = ThenoOfMaternalantePartum.ToString();
                        //No. of Maternal Deaths due To Post partum Haemorrhage
                        SqlCommand noOfMaternalpostPartum = new SqlCommand("Select dbo.noOfMaternalpostPartum(@ID)", Connect);
                        noOfMaternalpostPartum.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMaternalpostPartum.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMaternalpostPartum = noOfMaternalpostPartum.ExecuteScalar();
                        result26.Text = ThenoOfMaternalpostPartum.ToString();
                        //No. of Maternal Deaths due To Obstructed Labour
                        SqlCommand noOfMaternalobstructedLabour = new SqlCommand("Select dbo.noOfMaternalobstructedLabour(@ID)", Connect);
                        noOfMaternalobstructedLabour.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMaternalobstructedLabour.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMaternalobstructedLabour = noOfMaternalobstructedLabour.ExecuteScalar();
                        result27.Text = ThenoOfMaternalobstructedLabour.ToString();
                        //No. of Maternal Deaths due To Sepsis
                        SqlCommand noOfMaternalSepsis = new SqlCommand("Select dbo.noOfMaternalSepsis(@ID)", Connect);
                        noOfMaternalSepsis.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMaternalSepsis.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMaternalSepsis = noOfMaternalSepsis.ExecuteScalar();
                        result28.Text = ThenoOfMaternalSepsis.ToString();
                        //No. of Maternal Deaths due To Eclampsia
                        SqlCommand noOfMaternalEclampsia = new SqlCommand("Select dbo.noOfMaternalEclampsia(@ID)", Connect);
                        noOfMaternalEclampsia.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMaternalEclampsia.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMaternalEclampsia = noOfMaternalEclampsia.ExecuteScalar();
                        result29.Text = ThenoOfMaternalEclampsia.ToString();
                        //No. of Maternal Deaths due To other causes
                        SqlCommand noOfMaternalOthers = new SqlCommand("Select dbo.noOfMaternalOthers(@ID)", Connect);
                        noOfMaternalOthers.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMaternalOthers.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMaternalOthers = noOfMaternalOthers.ExecuteScalar();
                        result30.Text = ThenoOfMaternalOthers.ToString();
                        //Total No. of Deaths in babies aged 0-28days
                        SqlCommand totalNoOfBabiesDeath28 = new SqlCommand("Select dbo.totalNoOfBabiesDeath28(@ID)", Connect);
                        totalNoOfBabiesDeath28.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalNoOfBabiesDeath28.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalNoOfBabiesDeath28 = totalNoOfBabiesDeath28.ExecuteScalar();
                        result31.Text = ThetotalNoOfBabiesDeath28.ToString();
                        //No. of Deaths in babies due to Sepsis 28
                        SqlCommand noOfbabiesSepsis28 = new SqlCommand("Select dbo.noOfbabiesSepsis28(@ID)", Connect);
                        noOfbabiesSepsis28.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfbabiesSepsis28.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfbabiesSepsis28 = noOfbabiesSepsis28.ExecuteScalar();
                        result32.Text = ThenoOfbabiesSepsis28.ToString();
                        //No_Of_Deaths_In_Babies_Due_To_Asphyxia 28
                        SqlCommand noOfbabiesAsphyxia28 = new SqlCommand("Select dbo.noOfbabiesAsphyxia28(@ID)", Connect);
                        noOfbabiesAsphyxia28.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfbabiesAsphyxia28.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfbabiesAsphyxia28 = noOfbabiesAsphyxia28.ExecuteScalar();
                        result33.Text = ThenoOfbabiesAsphyxia28.ToString();
                        //No. of Deaths in babies due to Neonatal Tetanus 28
                        SqlCommand noOfbabiesNeonental28 = new SqlCommand("Select dbo.noOfbabiesNeonental28(@ID)", Connect);
                        noOfbabiesNeonental28.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfbabiesNeonental28.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfbabiesNeonental28 = noOfbabiesNeonental28.ExecuteScalar();
                        result34.Text = ThenoOfbabiesNeonental28.ToString();
                        //No. of Deaths in babies due to other causes 28
                        SqlCommand noOfbabiesOthers28 = new SqlCommand("Select dbo.noOfbabiesOthers28(@ID)", Connect);
                        noOfbabiesOthers28.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfbabiesOthers28.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfbabiesOthers28 = noOfbabiesOthers28.ExecuteScalar();
                        result35.Text = ThenoOfbabiesOthers28.ToString();
                        //Total No. of Deaths in babies aged 1-11 months 
                        SqlCommand totalBabies11 = new SqlCommand("Select dbo.totalBabies11(@ID)", Connect);
                        totalBabies11.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalBabies11.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalBabies11 = totalBabies11.ExecuteScalar();
                        result36.Text = ThetotalBabies11.ToString();
                        //No. of Deaths in babies due to Pneumonia 11
                        SqlCommand noPneumonia11 = new SqlCommand("Select dbo.noPneumonia11(@ID)", Connect);
                        noPneumonia11.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noPneumonia11.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoPneumonia11 = noPneumonia11.ExecuteScalar();
                        result37.Text = ThenoPneumonia11.ToString();
                        //No. of Deaths in babies due to Malaria 11
                        SqlCommand noMalaria11 = new SqlCommand("Select dbo.noMalaria11(@ID)", Connect);
                        noMalaria11.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noMalaria11.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoMalaria11 = noMalaria11.ExecuteScalar();
                        result38.Text = ThenoMalaria11.ToString();
                        //	No. of Deaths in babies due to Diarrhoea diseases
                        SqlCommand noDiarrhoea11 = new SqlCommand("Select dbo.noDiarrhoea11(@ID)", Connect);
                        noDiarrhoea11.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noDiarrhoea11.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoDiarrhoea11 = noDiarrhoea11.ExecuteScalar();
                        result39.Text = ThenoDiarrhoea11.ToString();
                        //	No. of Deaths in babies due to Malnutrition
                        SqlCommand noMalnutrition11 = new SqlCommand("Select dbo.noMalnutrition11(@ID)", Connect);
                        noMalnutrition11.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noMalnutrition11.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoMalnutrition11 = noMalnutrition11.ExecuteScalar();
                        result40.Text = ThenoMalnutrition11.ToString();
                        //No. of Deaths in babies due to Measles
                        SqlCommand noMeasles11 = new SqlCommand("Select dbo.noMeasles11(@ID)", Connect);
                        noMeasles11.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noMeasles11.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoMeasles11 = noMeasles11.ExecuteScalar();
                        result41.Text = ThenoMeasles11.ToString();
                        //Total No. of Deaths in babies aged 12-59 months
                        SqlCommand totalBabies59 = new SqlCommand("Select dbo.totalBabies59(@ID)", Connect);
                        totalBabies59.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalBabies59.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalBabies59 = totalBabies59.ExecuteScalar();
                        result42.Text = ThetotalBabies59.ToString();
                        //	No. of Deaths in babies due to Pneumonia 59
                        SqlCommand noOfPneumonia59 = new SqlCommand("Select dbo.noOfPneumonia59(@ID)", Connect);
                        noOfPneumonia59.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfPneumonia59.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfPneumonia59 = noOfPneumonia59.ExecuteScalar();
                        result43.Text = ThenoOfPneumonia59.ToString();
                        //No. of Deaths in babies due to Malaria 59
                        SqlCommand noOfMalaria59 = new SqlCommand("Select dbo.noOfMalaria59(@ID)", Connect);
                        noOfMalaria59.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMalaria59.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMalaria59 = noOfMalaria59.ExecuteScalar();
                        result44.Text = ThenoOfMalaria59.ToString();
                        //No. of Deaths in babies due to Diarrhoea diseases 59
                        SqlCommand noOfDiarrhoea59 = new SqlCommand("Select dbo.noOfDiarrhoea59(@ID)", Connect);
                        noOfDiarrhoea59.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfDiarrhoea59.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfDiarrhoea59 = noOfDiarrhoea59.ExecuteScalar();
                        result45.Text = ThenoOfDiarrhoea59.ToString();
                        //No. of Deaths in babies due to Malnutrition 59
                        SqlCommand noOfMalnutrition59 = new SqlCommand("Select dbo.noOfMalnutrition59(@ID)", Connect);
                        noOfMalnutrition59.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMalnutrition59.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMalnutrition59 = noOfMalnutrition59.ExecuteScalar();
                        result46.Text = ThenoOfMalnutrition59.ToString();
                        //	No. of Deaths in babies due to Measles 59
                        SqlCommand noOfMeasles59 = new SqlCommand("Select dbo.noOfMeasles59(@ID)", Connect);
                        noOfMeasles59.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfMeasles59.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfMeasles59 = noOfMeasles59.ExecuteScalar();
                        result47.Text = ThenoOfMeasles59.ToString();
                        // No. Of Children given BCG
                        SqlCommand noOfChildrenBcg = new SqlCommand("Select dbo.noOfChildrenBcg(@ID)", Connect);
                        noOfChildrenBcg.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfChildrenBcg.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfChildrenBcg = noOfChildrenBcg.ExecuteScalar();
                        result48.Text = ThenoOfChildrenBcg.ToString();
                        //No. Of Infants given DPT1 Vaccinations	
                        SqlCommand noOfInfantsDPT1 = new SqlCommand("Select dbo.noOfInfantsDPT1(@ID)", Connect);
                        noOfInfantsDPT1.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfInfantsDPT1.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfInfantsDPT1 = noOfInfantsDPT1.ExecuteScalar();
                        result49.Text = ThenoOfInfantsDPT1.ToString();
                        //No. Of Infants given DPT3 Vaccinations
                        SqlCommand noOfInfantsDPT3 = new SqlCommand("Select dbo.noOfInfantsDPT3(@ID)", Connect);
                        noOfInfantsDPT3.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfInfantsDPT3.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfInfantsDPT3 = noOfInfantsDPT3.ExecuteScalar();
                        result50.Text = ThenoOfInfantsDPT3.ToString();
                        //No. Of Infants given Measles Vaccinations	
                        SqlCommand noOfInfantsMeaslesVaccine = new SqlCommand("Select dbo.noOfInfantsMeaslesVaccine(@ID)", Connect);
                        noOfInfantsMeaslesVaccine.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfInfantsMeaslesVaccine.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfInfantsMeaslesVaccine = noOfInfantsMeaslesVaccine.ExecuteScalar();
                        result51.Text = ThenoOfInfantsMeaslesVaccine.ToString();
                        //No. Of Children 0 - 59 months given Vitamin A	
                        SqlCommand noOfChildren59VitA = new SqlCommand("Select dbo.noOfChildren59VitA(@ID)", Connect);
                        noOfChildren59VitA.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfChildren59VitA.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfChildren59VitA = noOfChildren59VitA.ExecuteScalar();
                        result52.Text = ThenoOfChildren59VitA.ToString();
                        //No. Of Nursing Mothers given Vitamin A	
                        SqlCommand noOfNursingMumsVitA = new SqlCommand("Select dbo.noOfNursingMumsVitA(@ID)", Connect);
                        noOfNursingMumsVitA.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfNursingMumsVitA.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfNursingMumsVitA = noOfNursingMumsVitA.ExecuteScalar();
                        result53.Text = ThenoOfNursingMumsVitA.ToString();
                        //No. Of Women that have received at least 2+ Doses of Tetanus Toxoid
                        SqlCommand noOfWomenTetanusToxoid = new SqlCommand("Select dbo.noOfWomenTetanusToxoid(@ID)", Connect);
                        noOfWomenTetanusToxoid.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noOfWomenTetanusToxoid.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoOfWomenTetanusToxoid = noOfWomenTetanusToxoid.ExecuteScalar();
                        result54.Text = ThenoOfWomenTetanusToxoid.ToString();
                        //Total No. Of Women aged 15-49 Years attending Family Planning Clinics	
                        SqlCommand totalAttendingFamilyPlanning49 = new SqlCommand("Select dbo.totalAttendingFamilyPlanning49(@ID)", Connect);
                        totalAttendingFamilyPlanning49.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalAttendingFamilyPlanning49.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalAttendingFamilyPlanning49 = totalAttendingFamilyPlanning49.ExecuteScalar();
                        result55.Text = ThetotalAttendingFamilyPlanning49.ToString();
                        //No. of Women aged 15-49 years using modern contraception	
                        SqlCommand noWomen49Contraception = new SqlCommand("Select dbo.noWomen49Contraception(@ID)", Connect);
                        noWomen49Contraception.Parameters.Add("@ID", SqlDbType.NVarChar);
                        noWomen49Contraception.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenoWomen49Contraception = noWomen49Contraception.ExecuteScalar();
                        result56.Text = ThenoWomen49Contraception.ToString();
                        //No. of New Acceptors Of a Family Planning Clinics
                        SqlCommand newAcceptorsfamilyPlanning = new SqlCommand("Select dbo.newAcceptorsfamilyPlanning(@ID)", Connect);
                        newAcceptorsfamilyPlanning.Parameters.Add("@ID", SqlDbType.NVarChar);
                        newAcceptorsfamilyPlanning.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThenewAcceptorsfamilyPlanning = newAcceptorsfamilyPlanning.ExecuteScalar();
                        result57.Text = ThenewAcceptorsfamilyPlanning.ToString();
                        //Total No. of Clients Referred	
                        SqlCommand totalWomenReferred = new SqlCommand("Select dbo.totalWomenReferred(@ID)", Connect);
                        totalWomenReferred.Parameters.Add("@ID", SqlDbType.NVarChar);
                        totalWomenReferred.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThetotalWomenReferred = totalWomenReferred.ExecuteScalar();
                        result58.Text = ThetotalWomenReferred.ToString();
                        //No. of Women Referred for Emergency Obstetric Care
                        SqlCommand womenReferredEOC = new SqlCommand("Select dbo.womenReferredEOC(@ID)", Connect);
                        womenReferredEOC.Parameters.Add("@ID", SqlDbType.NVarChar);
                        womenReferredEOC.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThewomenReferredEOC = womenReferredEOC.ExecuteScalar();
                        result59.Text = ThewomenReferredEOC.ToString();
                        //No. of Children 0-59 months Referred	
                        SqlCommand children59Referred = new SqlCommand("Select dbo.children59Referred(@ID)", Connect);
                        children59Referred.Parameters.Add("@ID", SqlDbType.NVarChar);
                        children59Referred.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object Thechildren59Referred = children59Referred.ExecuteScalar();
                        result60.Text = Thechildren59Referred.ToString();
                        //Total No. of Feedback on Referrals received from General Hospital
                        SqlCommand feedBackGH = new SqlCommand("Select dbo.feedBackGH(@ID)", Connect);
                        feedBackGH.Parameters.Add("@ID", SqlDbType.NVarChar);
                        feedBackGH.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThefeedBackGH = feedBackGH.ExecuteScalar();
                        result61.Text = ThefeedBackGH.ToString();
                        //No. of Outreaches conducted in the month
                        SqlCommand outreachesInAMonth = new SqlCommand("Select dbo.outreachesInAMonth(@ID)", Connect);
                        outreachesInAMonth.Parameters.Add("@ID", SqlDbType.NVarChar);
                        outreachesInAMonth.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object TheoutreachesInAMonth = outreachesInAMonth.ExecuteScalar();
                        result62.Text = TheoutreachesInAMonth.ToString();
                        //No. of Clients reached with Maternal Health Services
                        SqlCommand clientsReachedWithMHS = new SqlCommand("Select dbo.clientsReachedWithMHS(@ID)", Connect);
                        clientsReachedWithMHS.Parameters.Add("@ID", SqlDbType.NVarChar);
                        clientsReachedWithMHS.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object TheclientsReachedWithMHS = clientsReachedWithMHS.ExecuteScalar();
                        result63.Text = TheclientsReachedWithMHS.ToString();
                        //No. of Clients reached with Child Health Services
                        SqlCommand clientsReachedWithCHS = new SqlCommand("Select dbo.clientsReachedWithCHS(@ID)", Connect);
                        clientsReachedWithCHS.Parameters.Add("@ID", SqlDbType.NVarChar);
                        clientsReachedWithCHS.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object TheclientsReachedWithCHS = clientsReachedWithCHS.ExecuteScalar();
                        result64.Text = TheclientsReachedWithCHS.ToString();
                        //No. of Mama Kits in stock	
                        SqlCommand mamaKitsInStock = new SqlCommand("Select dbo.mamaKitsInStock(@ID)", Connect);
                        mamaKitsInStock.Parameters.Add("@ID", SqlDbType.NVarChar);
                        mamaKitsInStock.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThemamaKitsInStock = mamaKitsInStock.ExecuteScalar();
                        result65.Text = ThemamaKitsInStock.ToString();
                        //No. of Mama kits distributed in the month
                        SqlCommand mamaKitsDistributedInTheMoth = new SqlCommand("Select dbo.mamaKitsDistributedInTheMoth(@ID)", Connect);
                        mamaKitsDistributedInTheMoth.Parameters.Add("@ID", SqlDbType.NVarChar);
                        mamaKitsDistributedInTheMoth.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThemamaKitsDistributedInTheMoth = mamaKitsDistributedInTheMoth.ExecuteScalar();
                        result66.Text = ThemamaKitsDistributedInTheMoth.ToString();
                        //No. of personal health records in stock
                        SqlCommand personalHealthRecordInStock = new SqlCommand("Select dbo.personalHealthRecordInStock(@ID)", Connect);
                        personalHealthRecordInStock.Parameters.Add("@ID", SqlDbType.NVarChar);
                        personalHealthRecordInStock.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThepersonalHealthRecordInStock = personalHealthRecordInStock.ExecuteScalar();
                        result67.Text = ThepersonalHealthRecordInStock.ToString();
                        //No. of personal health record distributed
                        SqlCommand personalHealthRecordDistributed = new SqlCommand("Select dbo.personalHealthRecordDistributed(@ID)", Connect);
                        personalHealthRecordDistributed.Parameters.Add("@ID", SqlDbType.NVarChar);
                        personalHealthRecordDistributed.Parameters["@ID"].Value = whichSite.ToString();//Site ID here
                        Object ThepersonalHealthRecordDistributed = personalHealthRecordDistributed.ExecuteScalar();
                        result68.Text = ThepersonalHealthRecordDistributed.ToString();
                    }
                    else
                    {
                        Session.Abandon();
                        Session.Contents.RemoveAll();
                        FormsAuthentication.RedirectToLoginPage("index.aspx");
                    }
                }
            }
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Contents.RemoveAll();
            FormsAuthentication.RedirectToLoginPage("index.aspx");
        }
    }
}
