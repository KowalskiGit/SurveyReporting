using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SurveyReporting;
using Barnstone;
using Foundation.Data.SqlClient;

namespace SurveyReporting.Pages
{
    public partial class NoResponses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Register event when filter is changed.
                this.Filter.FilterApplied += new EventHandler(this.FilterApplied);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void FilterApplied(object sender, EventArgs e)
        {
            //Reload the page to refresh the graphs.
            ApplyFilter();
        }


        private void ApplyFilter()
        {
            //Get Whereclause from filter.
            string where = Filter.WhereClause;
            //Change where so that it looks in the SurveyPerTasksView as the OU filter should run per Task.
            if (where != "")
            {
                where = where.Replace("SurveysView.", "SurveysPerTaskView.");
                where = "SurveysView.SurveyID IN (SELECT SurveyID FROM SurveysPerTaskView WHERE " + where + ")";
            }

            string sql = @"SELECT [FirstName] + ' ' + [Surname] AS FullName, SurveysView.Name AS SurveyName, 
                            SurveysView.DatePublished, DateReminded, EMail
                        FROM SurveysView 
                            INNER JOIN SurveyRecipients ON SurveysView.SurveyID = SurveyRecipients.SurveyID
                            INNER JOIN Surveys ON SurveysView.SurveyID = Surveys.ID
                            INNER JOIN ContactPersonsView ON SurveyRecipients.ContactPersonID = ContactPersonsView.ContactPersonID
                            LEFT JOIN SurveyRecipientQuestionResponses ON 
                                    (SurveyRecipients.ContactPersonID = SurveyRecipientQuestionResponses.ContactPersonID) 
                                AND (SurveyRecipients.SurveyID = SurveyRecipientQuestionResponses.SurveyID)
                        WHERE " + Functions.Conjunct("SurveyRecipientQuestionResponses.ContactPersonID Is Null", where);
            using (Provider data = new Provider(Connection.ConnectionString, true))
            {
                gvNoResponses.EmptyDataText = "No Records Found.  Please set the filter criteria and click Apply.";
                gvNoResponses.DataSource = data.ExecuteReader(sql);
                gvNoResponses.DataBind();
            }
        }
    }
}