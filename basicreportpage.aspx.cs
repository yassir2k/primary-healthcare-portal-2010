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
    public partial class basicreportpage : System.Web.UI.Page
    {
        private void MessageBox(string msg)
        {
            Page.Controls.Add(new LiteralControl(
                           "<script language='javascript'> window.alert('" + msg + "')</script>"));
        }
        private int RandomNumber(int min, int max)//For Generating Random Integers In Creating Incident ID
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private string RandomString(int size, bool lowerCase)//For Generating Random Integers In Creating Incident ID
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString().ToUpper();
        }
        public string GetIncidentID()//For Generating A Combination Of Random Integers & Strings Of Length 10
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        static string DbConnectionStr = "Data Source=.\\SQLEXPRESS;Initial Catalog=NPHCDA;Integrated Security=SSPI";
        SqlConnection Connect = new SqlConnection(DbConnectionStr);
        protected void Page_Load(object sender, EventArgs e)
        {
            lastInstruction.Enabled = eventDate.Enabled = false;
            lastInstruction.Visible = eventDate.Visible = false;
            RequiredFieldValidator1.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            RequiredFieldValidator3.Enabled = false;
            RequiredFieldValidator4.Enabled = false;
            RequiredFieldValidator5.Enabled = false;
            RequiredFieldValidator6.Enabled = false;
            if (Session["Who_Is_it"] == null || Session["Role"] == null)
            {
                Response.Redirect("index.aspx");
            }
            else
            {
                Object theDate = DateTime.Now.ToShortDateString();
                phcDate.Text = theDate.ToString();
                string get_who = Session["Who_Is_It"].ToString();
                temp.Text = get_who;
                string DbConnectionStr = "Data Source=.\\SQLEXPRESS;Initial Catalog=NPHCDA;Integrated Security=SSPI";
                SqlConnection Connect = new SqlConnection(DbConnectionStr);
                if (IsPostBack == false)
                {
                    childMotherMortalityLabel.Visible = false;
                    childMotherMortalityLabel.Enabled = false;
                    childMotherMortalityOptions.Visible = false;
                    childMotherMortalityOptions.Enabled = false;
                }
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
                    if (what == 1 && Session["Role"].ToString() == "Basic" && Session["Who_Is_It"].ToString() == get_who)
                    {
                        SqlCommand user = new SqlCommand("Select dbo.whichUser(@user)", Connect);
                        user.Parameters.Add("@user", SqlDbType.NVarChar);
                        user.Parameters["@user"].Value = get_who;
                        Object which_user = user.ExecuteScalar();
                        theUser.Text = "Welcome, " + which_user.ToString();
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

        protected void submitEntry_Click(object sender, EventArgs e)
        {
            //Antenatal
            if (reportItem.Text == "New Antenatal Attendance")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Antenatal";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewAntenatalAttendance = new SqlCommand("newAntenatalAttendance", Connect);
                                addNewAntenatalAttendance.CommandType = CommandType.StoredProcedure;
                                addNewAntenatalAttendance.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewAntenatalAttendance.Parameters["@newCase"].Value = 1;
                                addNewAntenatalAttendance.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewAntenatalAttendance.Parameters["@incidentID"].Value = temporaryID;
                                addNewAntenatalAttendance.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewAntenatalAttendance.Parameters["@siteID"].Value = iFoundSite;
                                addNewAntenatalAttendance.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewAntenatalAttendance.Parameters["@userID"].Value = get_who;
                                addNewAntenatalAttendance.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewAntenatalAttendance.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewAntenatalAttendance.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewAntenatalAttendance.Parameters["@medicalSectionID"].Value = "Antenatal";
                                addNewAntenatalAttendance.ExecuteReader();
                                addNewAntenatalAttendance.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewAntenatalAttendance = new SqlCommand("Select dbo.checkNewAntenatalAttendance(@incidentID)", Connect);
                                checkNewAntenatalAttendance.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewAntenatalAttendance.Parameters["@incidentID"].Value = temporaryID;
                                Object newAntenatalAttendanceIncident = checkNewAntenatalAttendance.ExecuteScalar();
                                string confirmNewAntenatalAttendance = newAntenatalAttendanceIncident.ToString();
                                if (confirmNewAntenatalAttendance == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "New Antenatal Attendance";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "Pregnant Women With Above 4 Antenatal Visits")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Antenatal";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addPregnantWomenAboveFourAv = new SqlCommand("pregnantWomenAboveFourAv", Connect);
                                addPregnantWomenAboveFourAv.CommandType = CommandType.StoredProcedure;
                                addPregnantWomenAboveFourAv.Parameters.Add("@newCase", SqlDbType.Int);
                                addPregnantWomenAboveFourAv.Parameters["@newCase"].Value = 1;
                                addPregnantWomenAboveFourAv.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addPregnantWomenAboveFourAv.Parameters["@incidentID"].Value = temporaryID;
                                addPregnantWomenAboveFourAv.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addPregnantWomenAboveFourAv.Parameters["@siteID"].Value = iFoundSite;
                                addPregnantWomenAboveFourAv.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addPregnantWomenAboveFourAv.Parameters["@userID"].Value = get_who;
                                addPregnantWomenAboveFourAv.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addPregnantWomenAboveFourAv.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addPregnantWomenAboveFourAv.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addPregnantWomenAboveFourAv.Parameters["@medicalSectionID"].Value = "Antenatal";
                                addPregnantWomenAboveFourAv.ExecuteReader();
                                addPregnantWomenAboveFourAv.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewPregnantWomenAboveFourAv = new SqlCommand("Select dbo.checkNewAntenatalAttendance(@incidentID)", Connect);
                                checkNewPregnantWomenAboveFourAv.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewPregnantWomenAboveFourAv.Parameters["@incidentID"].Value = temporaryID;
                                Object newPregnantWomenAboveFourAvIncident = checkNewPregnantWomenAboveFourAv.ExecuteScalar();
                                string confirmNewPregnantWomenAboveFourAvAttendance = newPregnantWomenAboveFourAvIncident.ToString();
                                if (confirmNewPregnantWomenAboveFourAvAttendance == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Pregnant Women With Above 4 Antenatal Visits";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "Abortion")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Antenatal";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addAbortion = new SqlCommand("abortions", Connect);
                                addAbortion.CommandType = CommandType.StoredProcedure;
                                addAbortion.Parameters.Add("@newCase", SqlDbType.Int);
                                addAbortion.Parameters["@newCase"].Value = 1;
                                addAbortion.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addAbortion.Parameters["@incidentID"].Value = temporaryID;
                                addAbortion.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addAbortion.Parameters["@siteID"].Value = iFoundSite;
                                addAbortion.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addAbortion.Parameters["@userID"].Value = get_who;
                                addAbortion.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addAbortion.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addAbortion.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addAbortion.Parameters["@medicalSectionID"].Value = "Antenatal";
                                addAbortion.ExecuteReader();
                                addAbortion.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkAbortion = new SqlCommand("Select dbo.checkNewAntenatalAttendance(@incidentID)", Connect);
                                checkAbortion.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkAbortion.Parameters["@incidentID"].Value = temporaryID;
                                Object newAbortionIncident = checkAbortion.ExecuteScalar();
                                string confirmNewAbortion = newAbortionIncident.ToString();
                                if (confirmNewAbortion == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Abortion";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }//Antenatal

            //Child Mortality
            else if (reportItem.Text == "Aged 0 To 28 Days")
            {
                if (childMotherMortalityOptions.Text == "Death Due To Sepsis In 0 To 28 Days Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewSepsis28 = new SqlCommand("Sepsis28", Connect);
                                    addNewSepsis28.CommandType = CommandType.StoredProcedure;
                                    addNewSepsis28.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewSepsis28.Parameters["@newCase"].Value = 1;
                                    addNewSepsis28.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewSepsis28.Parameters["@incidentID"].Value = temporaryID;
                                    addNewSepsis28.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewSepsis28.Parameters["@siteID"].Value = iFoundSite;
                                    addNewSepsis28.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewSepsis28.Parameters["@userID"].Value = get_who;
                                    addNewSepsis28.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewSepsis28.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewSepsis28.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewSepsis28.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewSepsis28.ExecuteReader();
                                    addNewSepsis28.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewSepsis28 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewSepsis28.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewSepsis28.Parameters["@incidentID"].Value = temporaryID;
                                    Object newSepsis28Incident = checkNewSepsis28.ExecuteScalar();
                                    string confirmNewSepsis28 = newSepsis28Incident.ToString();
                                    if (confirmNewSepsis28 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Sepsis In 0 To 28 Days Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Asphyxia In 0 To 28 Days Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewAsphyxia28 = new SqlCommand("Asphyxia28", Connect);
                                    addNewAsphyxia28.CommandType = CommandType.StoredProcedure;
                                    addNewAsphyxia28.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewAsphyxia28.Parameters["@newCase"].Value = 1;
                                    addNewAsphyxia28.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewAsphyxia28.Parameters["@incidentID"].Value = temporaryID;
                                    addNewAsphyxia28.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewAsphyxia28.Parameters["@siteID"].Value = iFoundSite;
                                    addNewAsphyxia28.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewAsphyxia28.Parameters["@userID"].Value = get_who;
                                    addNewAsphyxia28.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewAsphyxia28.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewAsphyxia28.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewAsphyxia28.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewAsphyxia28.ExecuteReader();
                                    addNewAsphyxia28.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewAsphyxia28 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewAsphyxia28.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewAsphyxia28.Parameters["@incidentID"].Value = temporaryID;
                                    Object newAsphyxia28Incident = checkNewAsphyxia28.ExecuteScalar();
                                    string confirmNewAsphyxia28 = newAsphyxia28Incident.ToString();
                                    if (confirmNewAsphyxia28 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Asphyxia In 0 To 28 Days Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Neonental Tetanus In 0 To 28 Days Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewneonenatalTetanus28 = new SqlCommand("neonenatalTetanus28", Connect);
                                    addNewneonenatalTetanus28.CommandType = CommandType.StoredProcedure;
                                    addNewneonenatalTetanus28.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewneonenatalTetanus28.Parameters["@newCase"].Value = 1;
                                    addNewneonenatalTetanus28.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewneonenatalTetanus28.Parameters["@incidentID"].Value = temporaryID;
                                    addNewneonenatalTetanus28.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewneonenatalTetanus28.Parameters["@siteID"].Value = iFoundSite;
                                    addNewneonenatalTetanus28.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewneonenatalTetanus28.Parameters["@userID"].Value = get_who;
                                    addNewneonenatalTetanus28.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewneonenatalTetanus28.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewneonenatalTetanus28.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewneonenatalTetanus28.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewneonenatalTetanus28.ExecuteReader();
                                    addNewneonenatalTetanus28.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewneonenatalTetanus28 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewneonenatalTetanus28.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewneonenatalTetanus28.Parameters["@incidentID"].Value = temporaryID;
                                    Object newNeonenatalTetanus28 = checkNewneonenatalTetanus28.ExecuteScalar();
                                    string confirmNewneonenatalTetanus28 = newNeonenatalTetanus28.ToString();
                                    if (confirmNewneonenatalTetanus28 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Neonental Tetanus In 0 To 28 Days Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Other Causes In 0 To 28 Days Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewotherCauses28 = new SqlCommand("otherCauses28", Connect);
                                    addNewotherCauses28.CommandType = CommandType.StoredProcedure;
                                    addNewotherCauses28.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewotherCauses28.Parameters["@newCase"].Value = 1;
                                    addNewotherCauses28.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewotherCauses28.Parameters["@incidentID"].Value = temporaryID;
                                    addNewotherCauses28.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewotherCauses28.Parameters["@siteID"].Value = iFoundSite;
                                    addNewotherCauses28.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewotherCauses28.Parameters["@userID"].Value = get_who;
                                    addNewotherCauses28.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewotherCauses28.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewotherCauses28.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewotherCauses28.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewotherCauses28.ExecuteReader();
                                    addNewotherCauses28.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewotherCauses28 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewotherCauses28.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewotherCauses28.Parameters["@incidentID"].Value = temporaryID;
                                    Object newotherCauses28 = checkNewotherCauses28.ExecuteScalar();
                                    string confirmNewotherCauses28 = newotherCauses28.ToString();
                                    if (confirmNewotherCauses28 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Other Causes In 0 To 28 Days Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else
                {
                    ;
                }
            }
            else if (reportItem.Text == "Aged 1 To 11 Months")
            {
                if (childMotherMortalityOptions.Text == "Death Due To Pneumonia In 1 To 11 Months Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewpneumonia11 = new SqlCommand("pneumonia11", Connect);
                                    addNewpneumonia11.CommandType = CommandType.StoredProcedure;
                                    addNewpneumonia11.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewpneumonia11.Parameters["@newCase"].Value = 1;
                                    addNewpneumonia11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewpneumonia11.Parameters["@incidentID"].Value = temporaryID;
                                    addNewpneumonia11.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewpneumonia11.Parameters["@siteID"].Value = iFoundSite;
                                    addNewpneumonia11.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewpneumonia11.Parameters["@userID"].Value = get_who;
                                    addNewpneumonia11.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewpneumonia11.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewpneumonia11.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewpneumonia11.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewpneumonia11.ExecuteReader();
                                    addNewpneumonia11.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewpneumonia11 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewpneumonia11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewpneumonia11.Parameters["@incidentID"].Value = temporaryID;
                                    Object newpneumonia11 = checkNewpneumonia11.ExecuteScalar();
                                    string confirmNewpneumonia11 = newpneumonia11.ToString();
                                    if (confirmNewpneumonia11 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Pneumonia In 1 To 11 Months Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Malaria In 1 To 11 Months Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewmalaria11 = new SqlCommand("malaria11", Connect);
                                    addNewmalaria11.CommandType = CommandType.StoredProcedure;
                                    addNewmalaria11.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewmalaria11.Parameters["@newCase"].Value = 1;
                                    addNewmalaria11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewmalaria11.Parameters["@incidentID"].Value = temporaryID;
                                    addNewmalaria11.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewmalaria11.Parameters["@siteID"].Value = iFoundSite;
                                    addNewmalaria11.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewmalaria11.Parameters["@userID"].Value = get_who;
                                    addNewmalaria11.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewmalaria11.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewmalaria11.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewmalaria11.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewmalaria11.ExecuteReader();
                                    addNewmalaria11.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewmalaria11 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewmalaria11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewmalaria11.Parameters["@incidentID"].Value = temporaryID;
                                    Object newmalaria11 = checkNewmalaria11.ExecuteScalar();
                                    string confirmNewmalaria11 = newmalaria11.ToString();
                                    if (confirmNewmalaria11 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Malaria In 1 To 11 Months Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Diarrhoea Diseases In 1 To 11 Months Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewdiarrhoea11 = new SqlCommand("diarrhoea11", Connect);
                                    addNewdiarrhoea11.CommandType = CommandType.StoredProcedure;
                                    addNewdiarrhoea11.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewdiarrhoea11.Parameters["@newCase"].Value = 1;
                                    addNewdiarrhoea11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewdiarrhoea11.Parameters["@incidentID"].Value = temporaryID;
                                    addNewdiarrhoea11.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewdiarrhoea11.Parameters["@siteID"].Value = iFoundSite;
                                    addNewdiarrhoea11.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewdiarrhoea11.Parameters["@userID"].Value = get_who;
                                    addNewdiarrhoea11.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewdiarrhoea11.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewdiarrhoea11.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewdiarrhoea11.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewdiarrhoea11.ExecuteReader();
                                    addNewdiarrhoea11.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewdiarrhoea11 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewdiarrhoea11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewdiarrhoea11.Parameters["@incidentID"].Value = temporaryID;
                                    Object newdiarrhoea11 = checkNewdiarrhoea11.ExecuteScalar();
                                    string confirmNewdiarrhoea11 = newdiarrhoea11.ToString();
                                    if (confirmNewdiarrhoea11 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Diarrhoea Diseases In 1 To 11 Months Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Malnutrition In 1 To 11 Months Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewmalnutrition11 = new SqlCommand("malnutrition11", Connect);
                                    addNewmalnutrition11.CommandType = CommandType.StoredProcedure;
                                    addNewmalnutrition11.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewmalnutrition11.Parameters["@newCase"].Value = 1;
                                    addNewmalnutrition11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewmalnutrition11.Parameters["@incidentID"].Value = temporaryID;
                                    addNewmalnutrition11.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewmalnutrition11.Parameters["@siteID"].Value = iFoundSite;
                                    addNewmalnutrition11.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewmalnutrition11.Parameters["@userID"].Value = get_who;
                                    addNewmalnutrition11.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewmalnutrition11.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewmalnutrition11.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewmalnutrition11.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewmalnutrition11.ExecuteReader();
                                    addNewmalnutrition11.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewmalnutrition11 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewmalnutrition11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewmalnutrition11.Parameters["@incidentID"].Value = temporaryID;
                                    Object newmalnutrition11 = checkNewmalnutrition11.ExecuteScalar();
                                    string confirmNewmalnutrition11 = newmalnutrition11.ToString();
                                    if (confirmNewmalnutrition11 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Malnutrition In 1 To 11 Months Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Measles In 1 To 11 Months Old Babies")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewmeasles11 = new SqlCommand("measles11", Connect);
                                    addNewmeasles11.CommandType = CommandType.StoredProcedure;
                                    addNewmeasles11.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewmeasles11.Parameters["@newCase"].Value = 1;
                                    addNewmeasles11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewmeasles11.Parameters["@incidentID"].Value = temporaryID;
                                    addNewmeasles11.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewmeasles11.Parameters["@siteID"].Value = iFoundSite;
                                    addNewmeasles11.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewmeasles11.Parameters["@userID"].Value = get_who;
                                    addNewmeasles11.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewmeasles11.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewmeasles11.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewmeasles11.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewmeasles11.ExecuteReader();
                                    addNewmeasles11.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewmeasles11 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewmeasles11.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewmeasles11.Parameters["@incidentID"].Value = temporaryID;
                                    Object newmeasles11 = checkNewmeasles11.ExecuteScalar();
                                    string confirmNewmeasles11 = newmeasles11.ToString();
                                    if (confirmNewmeasles11 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Measles In 1 To 11 Months Old Babies";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else
                {
                    ;
                }
            }
            else if (reportItem.Text == "Aged 12 To 59 Months")
            {
                if (childMotherMortalityOptions.Text == "Death Due To Pneumonia In 12 To 59 Months Old Children")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewpneumonia59 = new SqlCommand("pneumonia59", Connect);
                                    addNewpneumonia59.CommandType = CommandType.StoredProcedure;
                                    addNewpneumonia59.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewpneumonia59.Parameters["@newCase"].Value = 1;
                                    addNewpneumonia59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewpneumonia59.Parameters["@incidentID"].Value = temporaryID;
                                    addNewpneumonia59.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewpneumonia59.Parameters["@siteID"].Value = iFoundSite;
                                    addNewpneumonia59.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewpneumonia59.Parameters["@userID"].Value = get_who;
                                    addNewpneumonia59.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewpneumonia59.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewpneumonia59.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewpneumonia59.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewpneumonia59.ExecuteReader();
                                    addNewpneumonia59.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewpneumonia59 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewpneumonia59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewpneumonia59.Parameters["@incidentID"].Value = temporaryID;
                                    Object newpneumonia59 = checkNewpneumonia59.ExecuteScalar();
                                    string confirmNewpneumonia59 = newpneumonia59.ToString();
                                    if (confirmNewpneumonia59 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Pneumonia In 12 To 59 Months Old Children";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Malaria In 12 To 59 Months Old Children")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewmalaria59 = new SqlCommand("malaria59", Connect);
                                    addNewmalaria59.CommandType = CommandType.StoredProcedure;
                                    addNewmalaria59.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewmalaria59.Parameters["@newCase"].Value = 1;
                                    addNewmalaria59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewmalaria59.Parameters["@incidentID"].Value = temporaryID;
                                    addNewmalaria59.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewmalaria59.Parameters["@siteID"].Value = iFoundSite;
                                    addNewmalaria59.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewmalaria59.Parameters["@userID"].Value = get_who;
                                    addNewmalaria59.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewmalaria59.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewmalaria59.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewmalaria59.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewmalaria59.ExecuteReader();
                                    addNewmalaria59.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewmalaria59 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewmalaria59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewmalaria59.Parameters["@incidentID"].Value = temporaryID;
                                    Object newmalaria59 = checkNewmalaria59.ExecuteScalar();
                                    string confirmNewmalaria59 = newmalaria59.ToString();
                                    if (confirmNewmalaria59 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Malaria In 12 To 59 Months Old Children";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Diarrhoea Diseases In 12 To 59 Months Old Children")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewdiarrhoea59 = new SqlCommand("diarrhoea59", Connect);
                                    addNewdiarrhoea59.CommandType = CommandType.StoredProcedure;
                                    addNewdiarrhoea59.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewdiarrhoea59.Parameters["@newCase"].Value = 1;
                                    addNewdiarrhoea59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewdiarrhoea59.Parameters["@incidentID"].Value = temporaryID;
                                    addNewdiarrhoea59.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewdiarrhoea59.Parameters["@siteID"].Value = iFoundSite;
                                    addNewdiarrhoea59.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewdiarrhoea59.Parameters["@userID"].Value = get_who;
                                    addNewdiarrhoea59.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewdiarrhoea59.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewdiarrhoea59.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewdiarrhoea59.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewdiarrhoea59.ExecuteReader();
                                    addNewdiarrhoea59.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewdiarrhoea59 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewdiarrhoea59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewdiarrhoea59.Parameters["@incidentID"].Value = temporaryID;
                                    Object newdiarrhoea59 = checkNewdiarrhoea59.ExecuteScalar();
                                    string confirmNewdiarrhoea59 = newdiarrhoea59.ToString();
                                    if (confirmNewdiarrhoea59 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Diarrhoea Diseases In 12 To 59 Months Old Children";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Malnutrition In 12 To 59 Months Old Children")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewmalnutrition59 = new SqlCommand("malnutrition59", Connect);
                                    addNewmalnutrition59.CommandType = CommandType.StoredProcedure;
                                    addNewmalnutrition59.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewmalnutrition59.Parameters["@newCase"].Value = 1;
                                    addNewmalnutrition59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewmalnutrition59.Parameters["@incidentID"].Value = temporaryID;
                                    addNewmalnutrition59.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewmalnutrition59.Parameters["@siteID"].Value = iFoundSite;
                                    addNewmalnutrition59.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewmalnutrition59.Parameters["@userID"].Value = get_who;
                                    addNewmalnutrition59.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewmalnutrition59.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewmalnutrition59.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewmalnutrition59.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewmalnutrition59.ExecuteReader();
                                    addNewmalnutrition59.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewmalnutrition59 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewmalnutrition59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewmalnutrition59.Parameters["@incidentID"].Value = temporaryID;
                                    Object newmalnutrition59 = checkNewmalnutrition59.ExecuteScalar();
                                    string confirmNewmalnutrition59 = newmalnutrition59.ToString();
                                    if (confirmNewmalnutrition59 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Malnutrition In 12 To 59 Months Old Children";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Death Due To Measles In 12 To 59 Months Old Children")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Child_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewmeasles59 = new SqlCommand("measles59", Connect);
                                    addNewmeasles59.CommandType = CommandType.StoredProcedure;
                                    addNewmeasles59.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewmeasles59.Parameters["@newCase"].Value = 1;
                                    addNewmeasles59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewmeasles59.Parameters["@incidentID"].Value = temporaryID;
                                    addNewmeasles59.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewmeasles59.Parameters["@siteID"].Value = iFoundSite;
                                    addNewmeasles59.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewmeasles59.Parameters["@userID"].Value = get_who;
                                    addNewmeasles59.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewmeasles59.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewmeasles59.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewmeasles59.Parameters["@medicalSectionID"].Value = "Child_Death";
                                    addNewmeasles59.ExecuteReader();
                                    addNewmeasles59.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewmeasles59 = new SqlCommand("Select dbo.checkNewChildMortality(@incidentID)", Connect);
                                    checkNewmeasles59.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewmeasles59.Parameters["@incidentID"].Value = temporaryID;
                                    Object newmeasles59 = checkNewmeasles59.ExecuteScalar();
                                    string confirmNewmeasles59 = newmeasles59.ToString();
                                    if (confirmNewmeasles59 == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Death Due To Measles In 12 To 59 Months Old Children";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else
                {
                    ;
                }
            }//Child Mortality

            //Commodities
            else if (reportItem.Text == "")
            {

            }//Commodities


            //Deliveries
            else if (reportItem.Text == "Live Birth With Weight Less Than Or Equals 2.5kg")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Delivery";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewliveBirthLessTwoHalf = new SqlCommand("liveBirthLessTwoHalf", Connect);
                                addNewliveBirthLessTwoHalf.CommandType = CommandType.StoredProcedure;
                                addNewliveBirthLessTwoHalf.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewliveBirthLessTwoHalf.Parameters["@newCase"].Value = 1;
                                addNewliveBirthLessTwoHalf.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewliveBirthLessTwoHalf.Parameters["@incidentID"].Value = temporaryID;
                                addNewliveBirthLessTwoHalf.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewliveBirthLessTwoHalf.Parameters["@siteID"].Value = iFoundSite;
                                addNewliveBirthLessTwoHalf.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewliveBirthLessTwoHalf.Parameters["@userID"].Value = get_who;
                                addNewliveBirthLessTwoHalf.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewliveBirthLessTwoHalf.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewliveBirthLessTwoHalf.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewliveBirthLessTwoHalf.Parameters["@medicalSectionID"].Value = "Delivery";
                                addNewliveBirthLessTwoHalf.ExecuteReader();
                                addNewliveBirthLessTwoHalf.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewliveBirthLessTwoHalf = new SqlCommand("Select dbo.checkNewDelivery(@incidentID)", Connect);
                                checkNewliveBirthLessTwoHalf.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewliveBirthLessTwoHalf.Parameters["@incidentID"].Value = temporaryID;
                                Object newliveBirthLessTwoHalf = checkNewliveBirthLessTwoHalf.ExecuteScalar();
                                string confirmNewliveBirthLessTwoHalf = newliveBirthLessTwoHalf.ToString();
                                if (confirmNewliveBirthLessTwoHalf == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Live Birth With Weight Less Than Or Equals 2.5kg";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "Live Birth With Weight Greater Than 2.5kg")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Delivery";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewliveBirthGreaterTwoHalf = new SqlCommand("liveBirthGreaterTwoHalf", Connect);
                                addNewliveBirthGreaterTwoHalf.CommandType = CommandType.StoredProcedure;
                                addNewliveBirthGreaterTwoHalf.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewliveBirthGreaterTwoHalf.Parameters["@newCase"].Value = 1;
                                addNewliveBirthGreaterTwoHalf.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewliveBirthGreaterTwoHalf.Parameters["@incidentID"].Value = temporaryID;
                                addNewliveBirthGreaterTwoHalf.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewliveBirthGreaterTwoHalf.Parameters["@siteID"].Value = iFoundSite;
                                addNewliveBirthGreaterTwoHalf.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewliveBirthGreaterTwoHalf.Parameters["@userID"].Value = get_who;
                                addNewliveBirthGreaterTwoHalf.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewliveBirthGreaterTwoHalf.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewliveBirthGreaterTwoHalf.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewliveBirthGreaterTwoHalf.Parameters["@medicalSectionID"].Value = "Delivery";
                                addNewliveBirthGreaterTwoHalf.ExecuteReader();
                                addNewliveBirthGreaterTwoHalf.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewliveBirthGreaterTwoHalf = new SqlCommand("Select dbo.checkNewDelivery(@incidentID)", Connect);
                                checkNewliveBirthGreaterTwoHalf.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewliveBirthGreaterTwoHalf.Parameters["@incidentID"].Value = temporaryID;
                                Object newliveBirthGreaterTwoHalf = checkNewliveBirthGreaterTwoHalf.ExecuteScalar();
                                string confirmNewliveBirthGreaterTwoHalf = newliveBirthGreaterTwoHalf.ToString();
                                if (confirmNewliveBirthGreaterTwoHalf == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Live Birth With Weight Greater Than 2.5kg";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "Still Birth")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Delivery";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewstillBirth = new SqlCommand("stillBirth", Connect);
                                addNewstillBirth.CommandType = CommandType.StoredProcedure;
                                addNewstillBirth.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewstillBirth.Parameters["@newCase"].Value = 1;
                                addNewstillBirth.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewstillBirth.Parameters["@incidentID"].Value = temporaryID;
                                addNewstillBirth.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewstillBirth.Parameters["@siteID"].Value = iFoundSite;
                                addNewstillBirth.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewstillBirth.Parameters["@userID"].Value = get_who;
                                addNewstillBirth.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewstillBirth.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewstillBirth.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewstillBirth.Parameters["@medicalSectionID"].Value = "Delivery";
                                addNewstillBirth.ExecuteReader();
                                addNewstillBirth.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewstillBirth = new SqlCommand("Select dbo.checkNewDelivery(@incidentID)", Connect);
                                checkNewstillBirth.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewstillBirth.Parameters["@incidentID"].Value = temporaryID;
                                Object newstillBirth = checkNewstillBirth.ExecuteScalar();
                                string confirmNewstillBirth = newstillBirth.ToString();
                                if (confirmNewstillBirth == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Still Birth";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }//Deliveries


            //Maternal Death
            else if (reportItem.Text == "Maternal Death In The Primary Healthcare Facility (PHF)")
            {
                if (childMotherMortalityOptions.Text == "Maternal Death Due To Ante Partum Haemorrhage")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewantepartumHaemorrhagePHC = new SqlCommand("antepartumHaemorrhagePHC", Connect);
                                    addNewantepartumHaemorrhagePHC.CommandType = CommandType.StoredProcedure;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@option", SqlDbType.Int);
                                    addNewantepartumHaemorrhagePHC.Parameters["@option"].Value = 1;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewantepartumHaemorrhagePHC.Parameters["@newCase"].Value = 1;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@incidentID"].Value = temporaryID;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@siteID"].Value = iFoundSite;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@userID"].Value = get_who;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewantepartumHaemorrhagePHC.ExecuteReader();
                                    addNewantepartumHaemorrhagePHC.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewantepartumHaemorrhagePHC = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewantepartumHaemorrhagePHC.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewantepartumHaemorrhagePHC.Parameters["@incidentID"].Value = temporaryID;
                                    Object newantepartumHaemorrhagePHC = checkNewantepartumHaemorrhagePHC.ExecuteScalar();
                                    string confirmNewantepartumHaemorrhagePHC = newantepartumHaemorrhagePHC.ToString();
                                    if (confirmNewantepartumHaemorrhagePHC == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Ante Partum Haemorrhage In PHC Facility";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Post Partum Haemorrhage")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewpostPartumHaemorrhagePHC = new SqlCommand("postPartumHaemorrhagePHC", Connect);
                                    addNewpostPartumHaemorrhagePHC.CommandType = CommandType.StoredProcedure;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@option", SqlDbType.Int);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@option"].Value = 1;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@newCase"].Value = 1;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@incidentID"].Value = temporaryID;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@siteID"].Value = iFoundSite;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@userID"].Value = get_who;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewpostPartumHaemorrhagePHC.ExecuteReader();
                                    addNewpostPartumHaemorrhagePHC.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewpostPartumHaemorrhagePHC = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewpostPartumHaemorrhagePHC.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewpostPartumHaemorrhagePHC.Parameters["@incidentID"].Value = temporaryID;
                                    Object newpostPartumHaemorrhagePHC = checkNewpostPartumHaemorrhagePHC.ExecuteScalar();
                                    string confirmNewpostPartumHaemorrhagePHC = newpostPartumHaemorrhagePHC.ToString();
                                    if (confirmNewpostPartumHaemorrhagePHC == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Post Partum Haemorrhage In PHC Facility";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Obstructed Labour")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewobstructedLabour = new SqlCommand("obstructedLabour", Connect);
                                    addNewobstructedLabour.CommandType = CommandType.StoredProcedure;
                                    addNewobstructedLabour.Parameters.Add("@option", SqlDbType.Int);
                                    addNewobstructedLabour.Parameters["@option"].Value = 1;
                                    addNewobstructedLabour.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewobstructedLabour.Parameters["@newCase"].Value = 1;
                                    addNewobstructedLabour.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@incidentID"].Value = temporaryID;
                                    addNewobstructedLabour.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@siteID"].Value = iFoundSite;
                                    addNewobstructedLabour.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@userID"].Value = get_who;
                                    addNewobstructedLabour.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewobstructedLabour.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewobstructedLabour.ExecuteReader();
                                    addNewobstructedLabour.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewobstructedLabour = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewobstructedLabour.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewobstructedLabour.Parameters["@incidentID"].Value = temporaryID;
                                    Object newobstructedLabour = checkNewobstructedLabour.ExecuteScalar();
                                    string confirmNewobstructedLabour = newobstructedLabour.ToString();
                                    if (confirmNewobstructedLabour == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Obstructed Labour In PHC Facility";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Sepsis")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewsepsis = new SqlCommand("sepsis", Connect);
                                    addNewsepsis.CommandType = CommandType.StoredProcedure;
                                    addNewsepsis.Parameters.Add("@option", SqlDbType.Int);
                                    addNewsepsis.Parameters["@option"].Value = 1;
                                    addNewsepsis.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewsepsis.Parameters["@newCase"].Value = 1;
                                    addNewsepsis.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@incidentID"].Value = temporaryID;
                                    addNewsepsis.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@siteID"].Value = iFoundSite;
                                    addNewsepsis.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@userID"].Value = get_who;
                                    addNewsepsis.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewsepsis.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewsepsis.ExecuteReader();
                                    addNewsepsis.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewsepsis = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewsepsis.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewsepsis.Parameters["@incidentID"].Value = temporaryID;
                                    Object newsepsis = checkNewsepsis.ExecuteScalar();
                                    string confirmNewsepsis = newsepsis.ToString();
                                    if (confirmNewsepsis == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Sepsis In PHC Facility";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Eclampsia")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNeweclampsia = new SqlCommand("eclampsia", Connect);
                                    addNeweclampsia.CommandType = CommandType.StoredProcedure;
                                    addNeweclampsia.Parameters.Add("@option", SqlDbType.Int);
                                    addNeweclampsia.Parameters["@option"].Value = 1;
                                    addNeweclampsia.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNeweclampsia.Parameters["@newCase"].Value = 1;
                                    addNeweclampsia.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@incidentID"].Value = temporaryID;
                                    addNeweclampsia.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@siteID"].Value = iFoundSite;
                                    addNeweclampsia.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@userID"].Value = get_who;
                                    addNeweclampsia.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNeweclampsia.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNeweclampsia.ExecuteReader();
                                    addNeweclampsia.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNeweclampsia = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNeweclampsia.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNeweclampsia.Parameters["@incidentID"].Value = temporaryID;
                                    Object neweclampsia = checkNeweclampsia.ExecuteScalar();
                                    string confirmNeweclampsia = neweclampsia.ToString();
                                    if (confirmNeweclampsia == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Eclampsia In PHC Facility";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Other Causes")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewotherCauses = new SqlCommand("otherCauses", Connect);
                                    addNewotherCauses.CommandType = CommandType.StoredProcedure;
                                    addNewotherCauses.Parameters.Add("@option", SqlDbType.Int);
                                    addNewotherCauses.Parameters["@option"].Value = 1;
                                    addNewotherCauses.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewotherCauses.Parameters["@newCase"].Value = 1;
                                    addNewotherCauses.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@incidentID"].Value = temporaryID;
                                    addNewotherCauses.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@siteID"].Value = iFoundSite;
                                    addNewotherCauses.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@userID"].Value = get_who;
                                    addNewotherCauses.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewotherCauses.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewotherCauses.ExecuteReader();
                                    addNewotherCauses.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewotherCauses = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewotherCauses.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewotherCauses.Parameters["@incidentID"].Value = temporaryID;
                                    Object newotherCauses = checkNewotherCauses.ExecuteScalar();
                                    string confirmNewotherCauses = newotherCauses.ToString();
                                    if (confirmNewotherCauses == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Other Causes In PHC Facility";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else
                {
                    ;
                }
            }
            else if (reportItem.Text == "Maternal Death In The Target Community")////////////////////////////////////////////////////
            {
                if (childMotherMortalityOptions.Text == "Maternal Death Due To Ante Partum Haemorrhage")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewantepartumHaemorrhagePHC = new SqlCommand("antepartumHaemorrhageTC", Connect);
                                    addNewantepartumHaemorrhagePHC.CommandType = CommandType.StoredProcedure;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@option", SqlDbType.Int);
                                    addNewantepartumHaemorrhagePHC.Parameters["@option"].Value = 1;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewantepartumHaemorrhagePHC.Parameters["@newCase"].Value = 1;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@incidentID"].Value = temporaryID;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@siteID"].Value = iFoundSite;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@userID"].Value = get_who;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewantepartumHaemorrhagePHC.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewantepartumHaemorrhagePHC.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewantepartumHaemorrhagePHC.ExecuteReader();
                                    addNewantepartumHaemorrhagePHC.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewantepartumHaemorrhagePHC = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewantepartumHaemorrhagePHC.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewantepartumHaemorrhagePHC.Parameters["@incidentID"].Value = temporaryID;
                                    Object newantepartumHaemorrhagePHC = checkNewantepartumHaemorrhagePHC.ExecuteScalar();
                                    string confirmNewantepartumHaemorrhagePHC = newantepartumHaemorrhagePHC.ToString();
                                    if (confirmNewantepartumHaemorrhagePHC == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Ante Partum Haemorrhage In Target Community";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Post Partum Haemorrhage")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewpostPartumHaemorrhagePHC = new SqlCommand("postPartumHaemorrhageTC", Connect);
                                    addNewpostPartumHaemorrhagePHC.CommandType = CommandType.StoredProcedure;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@option", SqlDbType.Int);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@option"].Value = 1;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@newCase"].Value = 1;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@incidentID"].Value = temporaryID;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@siteID"].Value = iFoundSite;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@userID"].Value = get_who;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewpostPartumHaemorrhagePHC.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewpostPartumHaemorrhagePHC.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewpostPartumHaemorrhagePHC.ExecuteReader();
                                    addNewpostPartumHaemorrhagePHC.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewpostPartumHaemorrhagePHC = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewpostPartumHaemorrhagePHC.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewpostPartumHaemorrhagePHC.Parameters["@incidentID"].Value = temporaryID;
                                    Object newpostPartumHaemorrhagePHC = checkNewpostPartumHaemorrhagePHC.ExecuteScalar();
                                    string confirmNewpostPartumHaemorrhagePHC = newpostPartumHaemorrhagePHC.ToString();
                                    if (confirmNewpostPartumHaemorrhagePHC == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Post Partum Haemorrhage In Target community";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Obstructed Labour")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewobstructedLabour = new SqlCommand("obstructedLabourTC", Connect);
                                    addNewobstructedLabour.CommandType = CommandType.StoredProcedure;
                                    addNewobstructedLabour.Parameters.Add("@option", SqlDbType.Int);
                                    addNewobstructedLabour.Parameters["@option"].Value = 1;
                                    addNewobstructedLabour.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewobstructedLabour.Parameters["@newCase"].Value = 1;
                                    addNewobstructedLabour.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@incidentID"].Value = temporaryID;
                                    addNewobstructedLabour.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@siteID"].Value = iFoundSite;
                                    addNewobstructedLabour.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@userID"].Value = get_who;
                                    addNewobstructedLabour.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewobstructedLabour.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewobstructedLabour.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewobstructedLabour.ExecuteReader();
                                    addNewobstructedLabour.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewobstructedLabour = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewobstructedLabour.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewobstructedLabour.Parameters["@incidentID"].Value = temporaryID;
                                    Object newobstructedLabour = checkNewobstructedLabour.ExecuteScalar();
                                    string confirmNewobstructedLabour = newobstructedLabour.ToString();
                                    if (confirmNewobstructedLabour == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Obstructed Labour In Target Community";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Sepsis")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewsepsis = new SqlCommand("sepsisTC", Connect);
                                    addNewsepsis.CommandType = CommandType.StoredProcedure;
                                    addNewsepsis.Parameters.Add("@option", SqlDbType.Int);
                                    addNewsepsis.Parameters["@option"].Value = 1;
                                    addNewsepsis.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewsepsis.Parameters["@newCase"].Value = 1;
                                    addNewsepsis.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@incidentID"].Value = temporaryID;
                                    addNewsepsis.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@siteID"].Value = iFoundSite;
                                    addNewsepsis.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@userID"].Value = get_who;
                                    addNewsepsis.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewsepsis.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewsepsis.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewsepsis.ExecuteReader();
                                    addNewsepsis.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewsepsis = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewsepsis.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewsepsis.Parameters["@incidentID"].Value = temporaryID;
                                    Object newsepsis = checkNewsepsis.ExecuteScalar();
                                    string confirmNewsepsis = newsepsis.ToString();
                                    if (confirmNewsepsis == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Sepsis In Target Community";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Eclampsia")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNeweclampsia = new SqlCommand("eclampsiaTC", Connect);
                                    addNeweclampsia.CommandType = CommandType.StoredProcedure;
                                    addNeweclampsia.Parameters.Add("@option", SqlDbType.Int);
                                    addNeweclampsia.Parameters["@option"].Value = 1;
                                    addNeweclampsia.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNeweclampsia.Parameters["@newCase"].Value = 1;
                                    addNeweclampsia.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@incidentID"].Value = temporaryID;
                                    addNeweclampsia.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@siteID"].Value = iFoundSite;
                                    addNeweclampsia.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@userID"].Value = get_who;
                                    addNeweclampsia.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNeweclampsia.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNeweclampsia.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNeweclampsia.ExecuteReader();
                                    addNeweclampsia.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNeweclampsia = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNeweclampsia.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNeweclampsia.Parameters["@incidentID"].Value = temporaryID;
                                    Object neweclampsia = checkNeweclampsia.ExecuteScalar();
                                    string confirmNeweclampsia = neweclampsia.ToString();
                                    if (confirmNeweclampsia == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Eclampsia In Target Community";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else if (childMotherMortalityOptions.Text == "Maternal Death Due To Other Causes")
                {
                    try
                    {
                        Connect.Open();
                    }
                    catch (System.Exception ex)
                    {

                    }
                    finally
                    {
                        SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                        addPatient.CommandType = CommandType.StoredProcedure;
                        addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientName"].Value = patientName.Text;
                        addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                        addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                        addPatient.Parameters["@Sex"].Value = patientSex.Text;
                        addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                        addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                        //Now Checking Whether Patient Exist In Database Or Not
                        SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        Object patientExists = checkPatient.ExecuteScalar();
                        checkPatient.CommandText = null;
                        string getReturnedValue = patientExists.ToString();
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                        {
                            MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                        }
                        else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                        {
                            addPatient.ExecuteReader();
                            addPatient.Connection.Close();
                            Connect.Open();
                            //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                            SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                            checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            patientExists = checkPatients.ExecuteScalar();
                            getReturnedValue = patientExists.ToString();//Feedback From Database
                            if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                            {
                                //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                                SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                                addIncidents.CommandType = CommandType.StoredProcedure;
                                addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                string temporaryID = GetIncidentID();
                                for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                                {
                                    SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                    checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                    Object incidentExists = checkIncidentID.ExecuteScalar();
                                    string tempor = incidentExists.ToString();
                                    if (temporaryID != tempor)
                                        break;
                                }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                                //Continuation of Populating Incident Table To The Database
                                addIncidents.Parameters["@incidentID"].Value = temporaryID;
                                addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                                addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                                addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                                //Lets Get The Site ID
                                string get_who = Session["Who_Is_It"].ToString();
                                SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                                getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                                getSiteID.Parameters["@userID"].Value = get_who;
                                Object FoundSite = getSiteID.ExecuteScalar();
                                string iFoundSite = FoundSite.ToString();
                                //End Of Getting Site ID
                                addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@siteID"].Value = iFoundSite;
                                addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@userID"].Value = get_who;
                                addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addIncidents.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                addIncidents.ExecuteReader();
                                addIncidents.Connection.Close();
                                Connect.Open();
                                //Now Checking Whether Incident Has Been Entered Into That Database
                                SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncident.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExist = checkIncident.ExecuteScalar();
                                if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                                {//Finally We Can Save The Item Intended For Reporting Into The Database
                                    SqlCommand addNewotherCauses = new SqlCommand("otherCausesTC", Connect);
                                    addNewotherCauses.CommandType = CommandType.StoredProcedure;
                                    addNewotherCauses.Parameters.Add("@option", SqlDbType.Int);
                                    addNewotherCauses.Parameters["@option"].Value = 1;
                                    addNewotherCauses.Parameters.Add("@newCase", SqlDbType.Int);
                                    addNewotherCauses.Parameters["@newCase"].Value = 1;
                                    addNewotherCauses.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@incidentID"].Value = temporaryID;
                                    addNewotherCauses.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@siteID"].Value = iFoundSite;
                                    addNewotherCauses.Parameters.Add("@userID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@userID"].Value = get_who;
                                    addNewotherCauses.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                    addNewotherCauses.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                    addNewotherCauses.Parameters["@medicalSectionID"].Value = "Maternal_Death";
                                    addNewotherCauses.ExecuteReader();
                                    addNewotherCauses.Connection.Close();
                                    Connect.Open();
                                    //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                    SqlCommand checkNewotherCauses = new SqlCommand("Select dbo.checkNewMaternalDeath(@incidentID)", Connect);
                                    checkNewotherCauses.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                    checkNewotherCauses.Parameters["@incidentID"].Value = temporaryID;
                                    Object newotherCauses = checkNewotherCauses.ExecuteScalar();
                                    string confirmNewotherCauses = newotherCauses.ToString();
                                    if (confirmNewotherCauses == "exist")
                                    {
                                        Session["Incident"] = temporaryID;
                                        Session["Item"] = "Maternal Death Due To Other Causes In Target Community";
                                        Response.Redirect("basicsuccesspage.aspx");
                                    }
                                }
                                else
                                {
                                    MessageBox("Unable To Complete Operations");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Save Patient Details Onto Database");
                            }
                        }
                    }
                }
                else
                {
                    ;
                }
            }
            //Maternal Death


            //Outreaches
            else if (reportItem.Text == "")
            {

            }//Outreaches


            //Referrals
            else if (reportItem.Text == "Refer A Woman For Emergency Obstetric Care")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Referrals";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewwomenReferred = new SqlCommand("womenReferred", Connect);
                                addNewwomenReferred.CommandType = CommandType.StoredProcedure;
                                addNewwomenReferred.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewwomenReferred.Parameters["@newCase"].Value = 1;
                                addNewwomenReferred.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewwomenReferred.Parameters["@incidentID"].Value = temporaryID;
                                addNewwomenReferred.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewwomenReferred.Parameters["@siteID"].Value = iFoundSite;
                                addNewwomenReferred.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewwomenReferred.Parameters["@userID"].Value = get_who;
                                addNewwomenReferred.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewwomenReferred.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewwomenReferred.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewwomenReferred.Parameters["@medicalSectionID"].Value = "Referrals";
                                addNewwomenReferred.ExecuteReader();
                                addNewwomenReferred.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewwomenReferred = new SqlCommand("Select dbo.checkNewReferral(@incidentID)", Connect);
                                checkNewwomenReferred.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewwomenReferred.Parameters["@incidentID"].Value = temporaryID;
                                Object newwomenReferred = checkNewwomenReferred.ExecuteScalar();
                                string confirmNewwomenReferred = newwomenReferred.ToString();
                                if (confirmNewwomenReferred == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Refer A Woman For Emergency Obstetric Care";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "Refer A Child 0 To 59 Months Of Age")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Referrals";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewchildrenReferred = new SqlCommand("childrenReferred", Connect);
                                addNewchildrenReferred.CommandType = CommandType.StoredProcedure;
                                addNewchildrenReferred.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewchildrenReferred.Parameters["@newCase"].Value = 1;
                                addNewchildrenReferred.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewchildrenReferred.Parameters["@incidentID"].Value = temporaryID;
                                addNewchildrenReferred.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewchildrenReferred.Parameters["@siteID"].Value = iFoundSite;
                                addNewchildrenReferred.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewchildrenReferred.Parameters["@userID"].Value = get_who;
                                addNewchildrenReferred.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewchildrenReferred.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewchildrenReferred.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewchildrenReferred.Parameters["@medicalSectionID"].Value = "Referrals";
                                addNewchildrenReferred.ExecuteReader();
                                addNewchildrenReferred.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewchildrenReferred = new SqlCommand("Select dbo.checkNewReferral(@incidentID)", Connect);
                                checkNewchildrenReferred.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewchildrenReferred.Parameters["@incidentID"].Value = temporaryID;
                                Object newchildrenReferred = checkNewchildrenReferred.ExecuteScalar();
                                string confirmNewchildrenReferred = newchildrenReferred.ToString();
                                if (confirmNewchildrenReferred == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Refer A Child 0 To 59 Months Of Age";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }//Referrals


            //Routine Immunization
            else if (reportItem.Text == "A Child Given BCG Vaccine")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewBCG = new SqlCommand("BCG", Connect);
                                addNewBCG.CommandType = CommandType.StoredProcedure;
                                addNewBCG.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewBCG.Parameters["@newCase"].Value = 1;
                                addNewBCG.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewBCG.Parameters["@incidentID"].Value = temporaryID;
                                addNewBCG.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewBCG.Parameters["@siteID"].Value = iFoundSite;
                                addNewBCG.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewBCG.Parameters["@userID"].Value = get_who;
                                addNewBCG.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewBCG.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewBCG.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewBCG.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                                addNewBCG.ExecuteReader();
                                addNewBCG.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewBCG = new SqlCommand("Select dbo.checkNewRoutineImmunization(@incidentID)", Connect);
                                checkNewBCG.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewBCG.Parameters["@incidentID"].Value = temporaryID;
                                Object newBCG = checkNewBCG.ExecuteScalar();
                                string confirmNewBCG = newBCG.ToString();
                                if (confirmNewBCG == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "A Child Given BCG Vaccine";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "An Infant Given DPT1 Vaccination")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewDPTI = new SqlCommand("DPTI", Connect);
                                addNewDPTI.CommandType = CommandType.StoredProcedure;
                                addNewDPTI.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewDPTI.Parameters["@newCase"].Value = 1;
                                addNewDPTI.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewDPTI.Parameters["@incidentID"].Value = temporaryID;
                                addNewDPTI.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewDPTI.Parameters["@siteID"].Value = iFoundSite;
                                addNewDPTI.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewDPTI.Parameters["@userID"].Value = get_who;
                                addNewDPTI.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewDPTI.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewDPTI.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewDPTI.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                                addNewDPTI.ExecuteReader();
                                addNewDPTI.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewDPTI = new SqlCommand("Select dbo.checkNewRoutineImmunization(@incidentID)", Connect);
                                checkNewDPTI.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewDPTI.Parameters["@incidentID"].Value = temporaryID;
                                Object newDPTI = checkNewDPTI.ExecuteScalar();
                                string confirmNewDPTI = newDPTI.ToString();
                                if (confirmNewDPTI == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "An Infant Given DPT1 Vaccination";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "An Infant Given DPT3 Vaccination")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewDPT3 = new SqlCommand("DPT3", Connect);
                                addNewDPT3.CommandType = CommandType.StoredProcedure;
                                addNewDPT3.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewDPT3.Parameters["@newCase"].Value = 1;
                                addNewDPT3.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewDPT3.Parameters["@incidentID"].Value = temporaryID;
                                addNewDPT3.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewDPT3.Parameters["@siteID"].Value = iFoundSite;
                                addNewDPT3.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewDPT3.Parameters["@userID"].Value = get_who;
                                addNewDPT3.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewDPT3.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewDPT3.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewDPT3.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                                addNewDPT3.ExecuteReader();
                                addNewDPT3.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewDPT3 = new SqlCommand("Select dbo.checkNewRoutineImmunization(@incidentID)", Connect);
                                checkNewDPT3.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewDPT3.Parameters["@incidentID"].Value = temporaryID;
                                Object newDPT3 = checkNewDPT3.ExecuteScalar();
                                string confirmNewDPT3 = newDPT3.ToString();
                                if (confirmNewDPT3 == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "An Infant Given DPT3 Vaccination";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "An Infant Given Measles Vaccination")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewmeaslesVaccination = new SqlCommand("measlesVaccination", Connect);
                                addNewmeaslesVaccination.CommandType = CommandType.StoredProcedure;
                                addNewmeaslesVaccination.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewmeaslesVaccination.Parameters["@newCase"].Value = 1;
                                addNewmeaslesVaccination.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewmeaslesVaccination.Parameters["@incidentID"].Value = temporaryID;
                                addNewmeaslesVaccination.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewmeaslesVaccination.Parameters["@siteID"].Value = iFoundSite;
                                addNewmeaslesVaccination.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewmeaslesVaccination.Parameters["@userID"].Value = get_who;
                                addNewmeaslesVaccination.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewmeaslesVaccination.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewmeaslesVaccination.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewmeaslesVaccination.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                                addNewmeaslesVaccination.ExecuteReader();
                                addNewmeaslesVaccination.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewmeaslesVaccination = new SqlCommand("Select dbo.checkNewRoutineImmunization(@incidentID)", Connect);
                                checkNewmeaslesVaccination.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewmeaslesVaccination.Parameters["@incidentID"].Value = temporaryID;
                                Object newmeaslesVaccination = checkNewmeaslesVaccination.ExecuteScalar();
                                string confirmNewmeaslesVaccination = newmeaslesVaccination.ToString();
                                if (confirmNewmeaslesVaccination == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "An Infant Given Measles Vaccination";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "A Child Aged 0 To 59 Months Given Vitamin A")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewvitaminA = new SqlCommand("vitaminA", Connect);
                                addNewvitaminA.CommandType = CommandType.StoredProcedure;
                                addNewvitaminA.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewvitaminA.Parameters["@newCase"].Value = 1;
                                addNewvitaminA.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewvitaminA.Parameters["@incidentID"].Value = temporaryID;
                                addNewvitaminA.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewvitaminA.Parameters["@siteID"].Value = iFoundSite;
                                addNewvitaminA.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewvitaminA.Parameters["@userID"].Value = get_who;
                                addNewvitaminA.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewvitaminA.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewvitaminA.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewvitaminA.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                                addNewvitaminA.ExecuteReader();
                                addNewvitaminA.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewvitaminA = new SqlCommand("Select dbo.checkNewRoutineImmunization(@incidentID)", Connect);
                                checkNewvitaminA.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewvitaminA.Parameters["@incidentID"].Value = temporaryID;
                                Object newvitaminA = checkNewvitaminA.ExecuteScalar();
                                string confirmNewvitaminA = newvitaminA.ToString();
                                if (confirmNewvitaminA == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "A Child Aged 0 To 59 Months Given Vitamin A";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "Nursing Mother Given Vitamin A")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewnursingMothervitaminA = new SqlCommand("nursingMothervitaminA", Connect);
                                addNewnursingMothervitaminA.CommandType = CommandType.StoredProcedure;
                                addNewnursingMothervitaminA.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewnursingMothervitaminA.Parameters["@newCase"].Value = 1;
                                addNewnursingMothervitaminA.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewnursingMothervitaminA.Parameters["@incidentID"].Value = temporaryID;
                                addNewnursingMothervitaminA.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewnursingMothervitaminA.Parameters["@siteID"].Value = iFoundSite;
                                addNewnursingMothervitaminA.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewnursingMothervitaminA.Parameters["@userID"].Value = get_who;
                                addNewnursingMothervitaminA.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewnursingMothervitaminA.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewnursingMothervitaminA.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewnursingMothervitaminA.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                                addNewnursingMothervitaminA.ExecuteReader();
                                addNewnursingMothervitaminA.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewnursingMothervitaminA = new SqlCommand("Select dbo.checkNewRoutineImmunization(@incidentID)", Connect);
                                checkNewnursingMothervitaminA.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewnursingMothervitaminA.Parameters["@incidentID"].Value = temporaryID;
                                Object newnursingMothervitaminA = checkNewnursingMothervitaminA.ExecuteScalar();
                                string confirmNewnursingMothervitaminA = newnursingMothervitaminA.ToString();
                                if (confirmNewnursingMothervitaminA == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Nursing Mother Given Vitamin A";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else if (reportItem.Text == "Woman Receiving Tetanus Toxoid")
            {
                try
                {
                    Connect.Open();
                }
                catch (System.Exception ex)
                {

                }
                finally
                {
                    SqlCommand addPatient = new SqlCommand("addPatient", Connect);
                    addPatient.CommandType = CommandType.StoredProcedure;
                    addPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    addPatient.Parameters.Add("@patientName", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientName"].Value = patientName.Text;
                    addPatient.Parameters.Add("@patientAge", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAge"].Value = patientAge.Text;
                    addPatient.Parameters.Add("@Sex", SqlDbType.NVarChar);
                    addPatient.Parameters["@Sex"].Value = patientSex.Text;
                    addPatient.Parameters.Add("@patientAddress", SqlDbType.NVarChar);
                    addPatient.Parameters["@patientAddress"].Value = patientAddress.Text;
                    //Now Checking Whether Patient Exist In Database Or Not
                    SqlCommand checkPatient = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                    checkPatient.Parameters.Add("@patientID", SqlDbType.NVarChar);
                    checkPatient.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                    Object patientExists = checkPatient.ExecuteScalar();
                    checkPatient.CommandText = null;
                    string getReturnedValue = patientExists.ToString();
                    if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Already Exists
                    {
                        MessageBox("The Patient With ID " + getReturnedValue + " And Name " + patientName.Text + " Already Exist In The Database");
                    }
                    else//Meaning This Patient Is A New Patient/Doesn't Exist In The Database Yet
                    {
                        addPatient.ExecuteReader();
                        addPatient.Connection.Close();
                        Connect.Open();
                        //We Check The Database To See Whether Patient We Just Saved Exist Or Not
                        SqlCommand checkPatients = new SqlCommand("Select dbo.checkPatient(@patientID)", Connect);
                        checkPatients.Parameters.Add("@patientID", SqlDbType.NVarChar);
                        checkPatients.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                        patientExists = checkPatients.ExecuteScalar();
                        getReturnedValue = patientExists.ToString();//Feedback From Database
                        if (getReturnedValue == patientRegistrationNo.Text)//Meaning This Patient Details Are Entered Into The Database
                        {
                            //Now We Populate The Relevant Incidents & Respective Tables For Reporting
                            SqlCommand addIncidents = new SqlCommand("addIncident", Connect);
                            addIncidents.CommandType = CommandType.StoredProcedure;
                            addIncidents.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            string temporaryID = GetIncidentID();
                            for (; ; )//Even Though IncidentID Is Generated Randomly, I Have To Be Sure It Is Not Repeated
                            {
                                SqlCommand checkIncidentID = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                                checkIncidentID.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkIncidentID.Parameters["@incidentID"].Value = temporaryID;
                                Object incidentExists = checkIncidentID.ExecuteScalar();
                                string tempor = incidentExists.ToString();
                                if (temporaryID != tempor)
                                    break;
                            }//To Eliminate The Case Of Generating Same Incident ID By The Random Function
                            //Continuation of Populating Incident Table To The Database
                            addIncidents.Parameters["@incidentID"].Value = temporaryID;
                            addIncidents.Parameters.Add("@incidentDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@incidentDate"].Value = eventDate.Text;
                            addIncidents.Parameters.Add("@reportingDate", SqlDbType.DateTime);
                            addIncidents.Parameters["@reportingDate"].Value = DateTime.Today;
                            //Lets Get The Site ID
                            string get_who = Session["Who_Is_It"].ToString();
                            SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@userID)", Connect);
                            getSiteID.Parameters.Add("@userID", SqlDbType.NVarChar);
                            getSiteID.Parameters["@userID"].Value = get_who;
                            Object FoundSite = getSiteID.ExecuteScalar();
                            string iFoundSite = FoundSite.ToString();
                            //End Of Getting Site ID
                            addIncidents.Parameters.Add("@siteID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@siteID"].Value = iFoundSite;
                            addIncidents.Parameters.Add("@userID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@userID"].Value = get_who;
                            addIncidents.Parameters.Add("@patientID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                            addIncidents.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                            addIncidents.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                            addIncidents.ExecuteReader();
                            addIncidents.Connection.Close();
                            Connect.Open();
                            //Now Checking Whether Incident Has Been Entered Into That Database
                            SqlCommand checkIncident = new SqlCommand("Select dbo.checkIncident(@incidentID)", Connect);
                            checkIncident.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                            checkIncident.Parameters["@incidentID"].Value = temporaryID;
                            Object incidentExist = checkIncident.ExecuteScalar();
                            if (incidentExist.ToString() == temporaryID)//Meaning This Incident Is Saved Into The Database
                            {//Finally We Can Save The Item Intended For Reporting Into The Database
                                SqlCommand addNewtetanusToxoid = new SqlCommand("tetanusToxoid", Connect);
                                addNewtetanusToxoid.CommandType = CommandType.StoredProcedure;
                                addNewtetanusToxoid.Parameters.Add("@newCase", SqlDbType.Int);
                                addNewtetanusToxoid.Parameters["@newCase"].Value = 1;
                                addNewtetanusToxoid.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                addNewtetanusToxoid.Parameters["@incidentID"].Value = temporaryID;
                                addNewtetanusToxoid.Parameters.Add("@siteID", SqlDbType.NVarChar);
                                addNewtetanusToxoid.Parameters["@siteID"].Value = iFoundSite;
                                addNewtetanusToxoid.Parameters.Add("@userID", SqlDbType.NVarChar);
                                addNewtetanusToxoid.Parameters["@userID"].Value = get_who;
                                addNewtetanusToxoid.Parameters.Add("@patientID", SqlDbType.NVarChar);
                                addNewtetanusToxoid.Parameters["@patientID"].Value = patientRegistrationNo.Text;
                                addNewtetanusToxoid.Parameters.Add("@medicalSectionID", SqlDbType.NVarChar);
                                addNewtetanusToxoid.Parameters["@medicalSectionID"].Value = "Routine_Immunization";
                                addNewtetanusToxoid.ExecuteReader();
                                addNewtetanusToxoid.Connection.Close();
                                Connect.Open();
                                //Now Lets Check Whether Data For New Antenatal Attendance Is Saved In The Database
                                SqlCommand checkNewtetanusToxoid = new SqlCommand("Select dbo.checkNewRoutineImmunization(@incidentID)", Connect);
                                checkNewtetanusToxoid.Parameters.Add("@incidentID", SqlDbType.NVarChar);
                                checkNewtetanusToxoid.Parameters["@incidentID"].Value = temporaryID;
                                Object newtetanusToxoid = checkNewtetanusToxoid.ExecuteScalar();
                                string confirmNewtetanusToxoid = newtetanusToxoid.ToString();
                                if (confirmNewtetanusToxoid == "exist")
                                {
                                    Session["Incident"] = temporaryID;
                                    Session["Item"] = "Woman Receiving Tetanus Toxoid";
                                    Response.Redirect("basicsuccesspage.aspx");
                                }
                            }
                            else
                            {
                                MessageBox("Unable To Complete Operations");
                            }
                        }
                        else
                        {
                            MessageBox("Unable To Save Patient Details Onto Database");
                        }
                    }
                }
            }
            else
            {
                ;
            }//Routine Immunization
        }

        protected void reportSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportSection.Text == "Antenatal")
            {
                childMotherMortalityLabel.Visible = false;
                childMotherMortalityLabel.Enabled = false;
                childMotherMortalityOptions.Visible = false;
                childMotherMortalityOptions.Enabled = false;
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
                reportItem.Items.Add("Choose One From The Antenatal Sub item...");
                reportItem.Items.Add("New Antenatal Attendance");
                reportItem.Items.Add("Pregnant Women With Above 4 Antenatal Visits");
                reportItem.Items.Add("Abortion");
            }
            else if (reportSection.Text == "Child Death")
            {
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
                reportItem.Items.Add("Choose One From The Child Death Age Range...");
                reportItem.Items.Add("Aged 0 To 28 Days");
                reportItem.Items.Add("Aged 1 To 11 Months");
                reportItem.Items.Add("Aged 12 To 59 Months");
            }
            else if (reportSection.Text == "Commodities")
            {
                childMotherMortalityLabel.Visible = false;
                childMotherMortalityLabel.Enabled = false;
                childMotherMortalityOptions.Visible = false;
                childMotherMortalityOptions.Enabled = false;
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
                reportItem.Items.Add("Choose One From The Commodities Sub Item...");
            }
            else if (reportSection.Text == "Deliveries")
            {
                childMotherMortalityLabel.Visible = false;
                childMotherMortalityLabel.Enabled = false;
                childMotherMortalityOptions.Visible = false;
                childMotherMortalityOptions.Enabled = false;
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
                reportItem.Items.Add("Choose One From The Deliveries Sub Item...");
                reportItem.Items.Add("Live Birth With Weight Less Than Or Equals 2.5kg");
                reportItem.Items.Add("Live Birth With Weight Greater Than 2.5kg");
                reportItem.Items.Add("Still Birth");
            }
            else if (reportSection.Text == "Maternal Death")
            {
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
                reportItem.Items.Add("Choose One From The Maternal Death Sub Item...");
                reportItem.Items.Add("Maternal Death In The Primary Healthcare Facility (PHF)");
                reportItem.Items.Add("Maternal Death In The Target Community");
            }
            else if (reportSection.Text == "Outreaches")
            {
                childMotherMortalityLabel.Visible = false;
                childMotherMortalityLabel.Enabled = false;
                childMotherMortalityOptions.Visible = false;
                childMotherMortalityOptions.Enabled = false;
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
                reportItem.Items.Add("Choose One From The Outreaches Sub Item...");
            }
            else if (reportSection.Text == "Referrals")
            {
                childMotherMortalityLabel.Visible = false;
                childMotherMortalityLabel.Enabled = false;
                childMotherMortalityOptions.Visible = false;
                childMotherMortalityOptions.Enabled = false;
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
                reportItem.Items.Add("Choose One From The Referrals Sub Item...");
                reportItem.Items.Add("Refer A Woman For Emergency Obstetric Care");
                reportItem.Items.Add("Refer A Child 0 To 59 Months Of Age");
            }
            else if (reportSection.Text == "Routine Immunization")
            {
                childMotherMortalityLabel.Visible = false;
                childMotherMortalityLabel.Enabled = false;
                childMotherMortalityOptions.Visible = false;
                childMotherMortalityOptions.Enabled = false;
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
                reportItem.Items.Add("Choose One From The Routine Immunization Sub Item...");
                reportItem.Items.Add("A Child Given BCG Vaccine");
                reportItem.Items.Add("An Infant Given DPT1 Vaccination");
                reportItem.Items.Add("An Infant Given DPT3 Vaccination");
                reportItem.Items.Add("An Infant Given Measles Vaccination");
                reportItem.Items.Add("A Child Aged 0 To 59 Months Given Vitamin A");
                reportItem.Items.Add("Nursing Mother Given Vitamin A");
                reportItem.Items.Add("Woman Receiving Tetanus Toxoid");
            }
            else
            {
                childMotherMortalityLabel.Visible = false;
                childMotherMortalityLabel.Enabled = false;
                childMotherMortalityOptions.Visible = false;
                childMotherMortalityOptions.Enabled = false;
                submitEntry.Enabled = false;
                reportItem.Items.Clear();
            }
        }

        protected void reportItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Beginning Antenatal
            if (reportItem.Text == "New Antenatal Attendance")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date Of new Antenatal Attendance";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "Pregnant Women With Above 4 Antenatal Visits")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date of Antenatal visits";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "Abortion")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date of Abortion";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            // Ending Antenatal

            // Beginning Child Mortality
            else if (reportItem.Text == "Aged 0 To 28 Days")
            {
                childMotherMortalityOptions.Items.Clear();
                childMotherMortalityOptions.Items.Add("Choose One...");
                childMotherMortalityOptions.Items.Add("Death Due To Sepsis In 0 To 28 Days Old Babies");
                childMotherMortalityOptions.Items.Add("Death Due To Asphyxia In 0 To 28 Days Old Babies");
                childMotherMortalityOptions.Items.Add("Death Due To Neonatal Tetanus In 0 To 28 Days Old Babies");
                childMotherMortalityOptions.Items.Add("Death Due To Other Causes In 0 To 28 Days Old Babies");
                childMotherMortalityLabel.Visible = true;
                childMotherMortalityLabel.Enabled = true;
                childMotherMortalityLabel.Text = "4 - Please Choose One From 0 To 28 Days Options";
                childMotherMortalityOptions.Visible = true;
                childMotherMortalityOptions.Enabled = true;
            }
            else if (reportItem.Text == "Aged 1 To 11 Months")
            {
                childMotherMortalityOptions.Items.Clear();
                childMotherMortalityOptions.Items.Add("Choose One...");
                childMotherMortalityOptions.Items.Add("Death Due To Pneumonia In 1 To 11 Months Old Babies");
                childMotherMortalityOptions.Items.Add("Death Due To Malaria In 1 To 11 Months Old Babies");
                childMotherMortalityOptions.Items.Add("Death Due To Diarrhoea Diseases In 1 To 11 Months Old Babies");
                childMotherMortalityOptions.Items.Add("Death Due To Malnutrition In 1 To 11 Months Old Babies");
                childMotherMortalityOptions.Items.Add("Death Due To Measles In 1 To 11 Months Old Babies");
                childMotherMortalityLabel.Visible = true;
                childMotherMortalityLabel.Enabled = true;
                childMotherMortalityLabel.Text = "4 - Please Choose One From 1 To 11 Months Options";
                childMotherMortalityOptions.Visible = true;
                childMotherMortalityOptions.Enabled = true;
            }
            else if (reportItem.Text == "Aged 12 To 59 Months")
            {
                childMotherMortalityOptions.Items.Clear();
                childMotherMortalityOptions.Items.Add("Choose One...");
                childMotherMortalityOptions.Items.Add("Death Due To Pneumonia In 12 To 59 Months Old Children");
                childMotherMortalityOptions.Items.Add("Death Due To Malaria In 12 To 59 Months Old Children");
                childMotherMortalityOptions.Items.Add("Death Due To Diarrhoea Diseases In 12 To 59 Months Old Children");
                childMotherMortalityOptions.Items.Add("Death Due To Malnutrition In 12 To 59 Months Old Children");
                childMotherMortalityOptions.Items.Add("Death Due To Measles In 12 To 59 Months Old Children");
                childMotherMortalityLabel.Visible = true;
                childMotherMortalityLabel.Enabled = true;
                childMotherMortalityLabel.Text = "4 - Please Choose One From 12 To 59 Months Options";
                childMotherMortalityOptions.Visible = true;
                childMotherMortalityOptions.Enabled = true;
            }
            // Ending Child Mortality

            // Beginning Deliveries
            else if (reportItem.Text == "Live Birth With Weight Less Than Or Equals 2.5kg")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Choose a date for this live birth (<2.5Kg)";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "Live Birth With Weight Greater Than 2.5kg")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Choose a date for this live birth (>2.5kg)";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "Still Birth")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date of still birth";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            //Ending Deliveries

            //Beginning Maternal Death
            else if (reportItem.Text == "Maternal Death In The Primary Healthcare Facility (PHF)")
            {
                childMotherMortalityOptions.Items.Clear();
                childMotherMortalityOptions.Items.Add("Select An Option For Maternal Death In PHF");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Ante Partum Haemorrhage");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Post Partum Haemorrhage");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Obstructed Labour");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Sepsis");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Eclampsia");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Other Causes");
                childMotherMortalityLabel.Visible = true;
                childMotherMortalityLabel.Enabled = true;
                childMotherMortalityLabel.Text = "4 - Choose One From Maternal Death In PHF Options";
                childMotherMortalityOptions.Visible = true;
                childMotherMortalityOptions.Enabled = true;
            }
            else if (reportItem.Text == "Maternal Death In The Target Community")
            {
                childMotherMortalityOptions.Items.Clear();
                childMotherMortalityOptions.Items.Add("Select An Option For Maternal Death In Target Community");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Ante Partum Haemorrhage");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Post Partum Haemorrhage");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Obstructed Labour");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Sepsis");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Eclampsia");
                childMotherMortalityOptions.Items.Add("Maternal Death Due To Other Causes");
                childMotherMortalityLabel.Visible = true;
                childMotherMortalityLabel.Enabled = true;
                childMotherMortalityLabel.Text = "4 - Choose One From Maternal Death In Target Community";
                childMotherMortalityOptions.Visible = true;
                childMotherMortalityOptions.Enabled = true;
            }
            //Ending Maternal Death

            //Beginning Referrals
            else if (reportItem.Text == "Refer A Woman For Emergency Obstetric Care")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date woman was Referred to EOC";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "Refer A Child 0 To 59 Months Of Age")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date of Child referral";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            //Ending Referrals

            //Beginning Routine Immunization
            else if (reportItem.Text == "A Child Given BCG Vaccine")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date the Child was given BCG Vaccine";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "An Infant Given DPT1 Vaccination")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date the Child was given DPT1 Vaccination";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "An Infant Given DPT3 Vaccination")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date the Child was given DPT3 Vaccination";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "An Infant Given Measles Vaccination")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date the Infant was given measles vaccination";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "A Child Aged 0 To 59 Months Given Vitamin A")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date the child was given Vitamin A";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "Nursing Mother Given Vitamin A")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date the Mother was given Vitamin A";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else if (reportItem.Text == "Woman Receiving Tetanus Toxoid")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Text = "4 - Select the date the woman received Tetanus Toxoid";
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
            }
            else
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = false;
            }
        }

        protected void childMotherMortalityOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (childMotherMortalityOptions.Text == "Death Due To Sepsis In 0 To 28 Days Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death ";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Asphyxia In 0 To 28 Days Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Neonental Tetanus In 0 To 28 Days Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Other Causes In 0 To 28 Days Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Pneumonia In 1 To 11 Months Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Malaria In 1 To 11 Months Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Diarrhoea Diseases In 1 To 11 Months Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Malnutrition In 1 To 11 Months Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Measles In 1 To 11 Months Old Babies")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Pneumonia In 12 To 59 Months Old Children")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Malaria In 12 To 59 Months Old Children")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Diarrhoea Diseases In 12 To 59 Months Old Children")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Malnutrition In 12 To 59 Months Old Children")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Death Due To Measles In 12 To 59 Months Old Children")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Child's death";
            }
            else if (childMotherMortalityOptions.Text == "Maternal Death Due To Ante Partum Haemorrhage")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Maternal death";
            }
            else if (childMotherMortalityOptions.Text == "Maternal Death Due To Post Partum Haemorrhage")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Maternal death";
            }
            else if (childMotherMortalityOptions.Text == "Maternal Death Due To Obstructed Labour")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Maternal death";
            }
            else if (childMotherMortalityOptions.Text == "Maternal Death Due To Sepsis")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Maternal death";
            }
            else if (childMotherMortalityOptions.Text == "Maternal Death Due To Eclampsia")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Maternal death";
            }
            else if (childMotherMortalityOptions.Text == "Maternal Death Due To Other Causes")
            {
                RequiredFieldValidator1.Enabled = true;
                RequiredFieldValidator2.Enabled = true;
                RequiredFieldValidator3.Enabled = true;
                RequiredFieldValidator4.Enabled = true;
                RequiredFieldValidator5.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
                lastInstruction.Enabled = eventDate.Enabled = true;
                lastInstruction.Visible = eventDate.Visible = true;
                submitEntry.Enabled = true;
                lastInstruction.Text = "5 - Select the date of Maternal death";
            }
            else
            {
                RequiredFieldValidator1.Enabled = false;
                RequiredFieldValidator2.Enabled = false;
                RequiredFieldValidator3.Enabled = false;
                RequiredFieldValidator4.Enabled = false;
                RequiredFieldValidator5.Enabled = false;
                RequiredFieldValidator6.Enabled = false;
                lastInstruction.Enabled = eventDate.Enabled = false;
                lastInstruction.Visible = eventDate.Visible = false;
                submitEntry.Enabled = false;
            }
        }
    }
}
