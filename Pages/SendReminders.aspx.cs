using Foundation.Data.SqlClient;
using HelperLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyReporting.Pages
{
    public partial class SendReminders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SqlDataReader dr;
                string sql = @"SELECT ID, Name
                               FROM Surveys
                               WHERE DatePublished IS NOT NULL 
                                 AND DateClosed Is NULL 
                                 AND DateSuspended IS NULL
                               ORDER BY ID DESC";

                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    using (dr = data.ExecuteReader(sql))
                    {
                        OpenSurvey.DataSource = dr;
                        OpenSurvey.DataValueField = "ID";
                        OpenSurvey.DataTextField = "Name";
                        OpenSurvey.DataBind();
                    }
                }

                //Read default email from template
                string path = AppDomain.CurrentDomain.BaseDirectory;
                TextReader tr = new StreamReader(path + "Templates\\EmailReminder.txt");
                string message = tr.ReadToEnd();
                message = message.Replace("\r\n", Environment.NewLine);
                txtMessage.Text = message;
            }
        }

        /// <summary>Send enmail</summary>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            //Get recipients
            var surveyID = OpenSurvey.SelectedValue;
            if (!string.IsNullOrEmpty(surveyID))
            {
                SqlDataReader dr;
                string sql = $@"SELECT C.ContactPersonID, FirstName, Surname, FirstName + ' ' + Surname AS FullName, Email, Password
                            FROM ContactPersonsView C 
                                 INNER JOIN SurveyRecipients R ON C.ContactPersonID = R.ContactPersonID
                                 INNER JOIN Users U ON U.LoginName = C.Email
                            WHERE SurveyID = {surveyID} 
                               AND Email IS NOT NULL
                               AND DateResponded IS NULL";

                string allMails = "Reminders were send to the following recipients: <br/>";
                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    using (dr = data.ExecuteReader(sql))
                    {
                        while (dr.Read())
                        {
                            string recipient = dr["Email"].ToString();
                            allMails += dr["FullName"].ToString() + "(" + dr["Email"].ToString() + ")<br/>";

                            //Replace tags in message
                            string message = txtMessage.Text;
                            message = message.Replace("<%Recipient.EmailAddress%>", dr["Email"].ToString());
                            message = message.Replace("<%Recipient.FirstName%>", dr["FirstName"].ToString());
                            message = message.Replace("<%Recipient.Surname%>", dr["Surname"].ToString());
                            message = message.Replace("<%Recipient.FullName%>", dr["FullName"].ToString());
                            message = message.Replace("<%Recipient.DecryptedPassword%>", DecryptPassword(dr["Password"].ToString()));

                            //Send mail
                            GeneralFunctions.SendMail(txtFrom.Text, recipient, txtSubject.Text, message);

                            //Update the table that a reminder was send.
                            sql = $@"UPDATE SurveyRecipients
                                     SET DateReminded = '{DateTime.Now}'
                                     WHERE SurveyID = {surveyID} 
                                       AND ContactPersonID = '{dr["ContactPersonID"].ToString().Replace("'", "''")}'";
                            using (Provider data2 = new Provider(Connection.ConnectionString, true))
                            {
                                data2.ExecuteNonQuery(sql);
                            }
                        }
                    }
                }

                litReminderSend.Text = allMails;
            }
        }

        private string DecryptPassword(string password)
        {
            return PasswordHelper.Decrypt(password).ToString();
        }

        //Refresh the stats
        protected void OpenSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            string surveyID = OpenSurvey.SelectedValue;
            if (!string.IsNullOrEmpty(surveyID))
            {
                Provider data = new Provider(Connection.ConnectionString, true);

                //Get Responded
                string sql = $@"SELECT Count(1)
                                FROM SurveyRecipients 
                                WHERE SurveyID = {surveyID}
                                  AND DateResponded IS NOT NULL";
                litHaveResponded.Text = data.ExecuteScalar(sql).ToString();

                //Get NOT Responded
                sql = $@"SELECT Count(1)
                         FROM SurveyRecipients 
                         WHERE SurveyID = {surveyID}
                           AND DateResponded IS NULL";
                litHaveNotResponded.Text = data.ExecuteScalar(sql).ToString();
            }
            else
            {
                litHaveResponded.Text = "";
                litHaveNotResponded.Text = "";
            }
        }
    }
}