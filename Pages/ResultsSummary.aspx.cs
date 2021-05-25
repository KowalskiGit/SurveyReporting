using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Foundation.Data.SqlClient;
using Barnstone;


namespace SurveyReporting.Pages
{
    public partial class ResultsSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Register event when filter is changed.
            this.Filter.FilterApplied += new EventHandler(this.FilterApplied); 
        }


        protected void FilterApplied(object sender, EventArgs e)
        {
            //Reload the page to refresh the graphs.
            //Page_Load(sender, e);
            ApplyFilter();
        }


        private void ApplyFilter()
        {
            Provider data = new Provider(Connection.ConnectionString, true);

            //Get Whereclause from filter.
            string where = Filter.WhereClause;
            //Change where so that it looks in the SurveyPerTasksView as the OU  filter should run per Task.
            if (where != "")
            {
                where = where.Replace("SurveysView.", "SurveysPerTaskView.");
                where = "SurveyID IN (SELECT SurveyID FROM SurveysPerTaskView WHERE " + where + ")";
            }

            if (where != "")
            {

                //Get No of Surveys
                string sql = @"SELECT Count(1)
                            FROM SurveysView ";
                if (where != "")
                    sql += " WHERE " + where;
                litNoOfSurveys.Text = data.ExecuteScalar(sql).ToString();

                //Get Number of Responses
                sql = @"SELECT Count(1)
                    FROM SurveysView 
                            LEFT JOIN SurveyRecipients ON SurveysView.SurveyID = SurveyRecipients.SurveyID 
                    WHERE DateResponded Is Not Null";
                if (where != "")
                    sql += " AND " + where.Replace("SurveyID IN (", "SurveysView.SurveyID IN (");
                litResponses.Text = data.ExecuteScalar(sql).ToString();

                //Get No Responses
                sql = @"SELECT Count(1)
                    FROM SurveysView 
                            LEFT JOIN SurveyRecipients ON SurveysView.SurveyID = SurveyRecipients.SurveyID 
                    WHERE DateResponded Is Null";
                if (where != "")
                    sql += " AND " + where.Replace("SurveyID IN (", "SurveysView.SurveyID IN (");
                litNoResponses.Text = data.ExecuteScalar(sql).ToString();

                //Percentage returns
                double responses = int.Parse(litResponses.Text);
                double noResponses = int.Parse(litNoResponses.Text);
                double answer = responses / (responses + noResponses) * 100;
                litPercentageReturns.Text = Math.Round(answer, 2).ToString() + " %";

                //Project Surveyed:
                sql = @"SELECT Count(DISTINCT ProjectID) As Counter
                    FROM Surveysview ";
                if (where != "")
                    sql += " WHERE " + where;
                litProjectsSurveyed.Text = data.ExecuteScalar(sql).ToString();

                //Projects With Responses:
                sql = @"SELECT Count(DISTINCT ProjectID) As Counter
                    FROM SurveysView
                            LEFT JOIN SurveyRecipients ON SurveysView.SurveyID = SurveyRecipients.SurveyID
                    WHERE DateResponded Is Not Null";
                if (where != "")
                    sql += " AND " + where.Replace("SurveyID IN (", "SurveysView.SurveyID IN (");
                litProjectsWithResponses.Text = data.ExecuteScalar(sql).ToString();
                responses = int.Parse(litProjectsWithResponses.Text);

                //Projects Without Responses:
                noResponses = int.Parse(litProjectsSurveyed.Text) - responses;
                litProjectsWithoutResponses.Text = noResponses.ToString();

                //Return Rate
                noResponses = int.Parse(litProjectsWithoutResponses.Text);
                answer = responses / (responses + noResponses) * 100;
                litProjectsReturnRate.Text = Math.Round(answer, 2).ToString() + " %";
            }
        }
    }
}