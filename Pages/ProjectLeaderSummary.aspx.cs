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
    public partial class ProjectLeaderSummary : System.Web.UI.Page
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
            try
            {
                //Get Whereclause from filter.
                string where = Filter.WhereClause;
                //Change where so that it looks in the SurveyPerTasksView as the OU filter should run per Task.
                if (where != "")
                {
                    where = where.Replace("SurveysView.", "SurveysPerTaskView.");
                    where = "SurveysView.SurveyID IN (SELECT SurveyID FROM SurveysPerTaskView WHERE " + where + ")";
                }

                string sql = @"SELECT ProjectLeader, MIN(P.FirstName + ' ' + P.Surname) AS ProjectLeaderName, ROUND(AVG(Rating), 2) AS AvgScore
	                           FROM SurveysView
                                    inner join PersonnelView P on P.PersonnelID = SurveysView.ProjectLeader
                                    inner join SurveyRecipientQuestionResponses A on SurveysView.SurveyID = A.SurveyID AND A.SurveyID = SurveysView.SurveyID ";
                if (where != "")
                    sql += " WHERE " + where;
                sql += "GROUP BY ProjectLeader";

                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    gvProjectLeaderSummary.EmptyDataText = "No Records Found.  Please set the filter criteria and click Apply.";
                    gvProjectLeaderSummary.DataSource = data.ExecuteReader(sql);
                    gvProjectLeaderSummary.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}