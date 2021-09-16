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
    public partial class basicsuccesspage : System.Web.UI.Page
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
                tempo.Text = get_who;
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
                    if (what == 1 && Session["Role"].ToString() == "Basic" && Session["Who_Is_It"].ToString() == get_who)
                    {
                        SqlCommand user = new SqlCommand("Select dbo.whichUser(@user)", Connect);
                        user.Parameters.Add("@user", SqlDbType.NVarChar);
                        user.Parameters["@user"].Value = get_who;
                        Object which_user = user.ExecuteScalar();
                        theUser.Text = "Welcome, " + which_user.ToString();
                        //Now dealing With sessions and Stuff
                        Object temp;
                        string Word, Act;
                        Word = Session["Item"].ToString();//Clinical Subitem
                        Label14.Text = Word;
                        Act = Session["Incident"].ToString();//Incident ID
                        Label2.Text = Act;
                        //Report's Clinical Section
                        SqlCommand getMedicalSection = new SqlCommand("Select dbo.getMedicalSection(@ID)", Connect);
                        getMedicalSection.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getMedicalSection.Parameters["@ID"].Value = Label2.Text;
                        temp = getMedicalSection.ExecuteScalar();
                        Label4.Text = temp.ToString();
                        //Full Name Of Patient Reported
                        SqlCommand getPatientName = new SqlCommand("Select dbo.getPatientName(@ID)", Connect);
                        getPatientName.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getPatientName.Parameters["@ID"].Value = Label2.Text;
                        temp = getPatientName.ExecuteScalar();
                        Label6.Text = temp.ToString();
                        //ID Number Of Patient Reported
                        SqlCommand getPatientID = new SqlCommand("Select dbo.getPatientID(@ID)", Connect);
                        getPatientID.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getPatientID.Parameters["@ID"].Value = Label2.Text;
                        temp = getPatientID.ExecuteScalar();
                        Label8.Text = temp.ToString();
                        //Report Submitted By
                        SqlCommand getUserName = new SqlCommand("Select dbo.getUserName(@ID)", Connect);
                        getUserName.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getUserName.Parameters["@ID"].Value = Label2.Text;
                        temp = getUserName.ExecuteScalar();
                        Label10.Text = temp.ToString();
                        //The Reporter's ID Number
                        SqlCommand getUserID = new SqlCommand("Select dbo.getUserID(@ID)", Connect);
                        getUserID.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getUserID.Parameters["@ID"].Value = Label2.Text;
                        temp = getUserID.ExecuteScalar();
                        Label12.Text = temp.ToString();
                        //Facility Name
                        SqlCommand getSiteName = new SqlCommand("Select dbo.getSiteName(@ID)", Connect);
                        getSiteName.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getSiteName.Parameters["@ID"].Value = Label2.Text;
                        temp = getSiteName.ExecuteScalar();
                        Label16.Text = temp.ToString();
                        //Facility Location
                        SqlCommand getSiteLocation = new SqlCommand("Select dbo.getSiteLocation(@ID)", Connect);
                        getSiteLocation.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getSiteLocation.Parameters["@ID"].Value = Label2.Text;
                        temp = getSiteLocation.ExecuteScalar();
                        Label18.Text = temp.ToString();
                        //Submission Date
                        Label20.Text = DateTime.Today.ToShortDateString();
                        SqlCommand getIncidentDate = new SqlCommand("Select dbo.getIncidentDate(@ID)", Connect);
                        getIncidentDate.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getIncidentDate.Parameters["@ID"].Value = Label2.Text;
                        temp = getIncidentDate.ExecuteScalar();
                        Label22.Text = temp.ToString();
                        //Incident Date
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("basicoptions.aspx");
        }
    }
}
