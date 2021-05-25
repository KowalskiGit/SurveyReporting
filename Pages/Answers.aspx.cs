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
    public partial class Answers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Register event when filter is changed.
            this.Filter.FilterApplied += new EventHandler(this.FilterApplied);
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
                string where = Filter.WhereClause;
                //Change where so that it looks in the SurveyPerTasksView as the OU filter should run per Task.
                if (where != "")
                {
                    where = where.Replace("SurveysView.", "SurveysPerTaskView.");
                    where = "SurveysView.SurveyID IN (SELECT SurveyID FROM SurveysPerTaskView WHERE " + where + ")";
                }

                string sql = @"SELECT C.[FirstName] + ' ' + C.[Surname] AS ContactPerson, DateResponded, 
                                      CASE WHEN ISNULL(P.PersonnelID, 0) <> 0 THEN P.[FirstName] + ' ' + P.[Surname] ELSE TaskResponsiblePerson END AS ProjectLeader,
                                      Text AS Question, Rating, Comments
                        FROM SurveysView
                             inner join SurveyRecipientQuestionResponses A on SurveysView.SurveyID = A.SurveyID
                             inner join Questions Q on A.QuestionID = Q.ID
                             inner join SurveyRecipients R on R.ContactPersonID = A.ContactPersonID AND R.SurveyID = A.SurveyID
                             inner join ContactPersonsView C on C.ContactPersonID = A.ContactPersonID
                             left join PersonnelView P on SurveysView.ProjectLeader = P.PersonnelID
							 left join (SELECT SurveyID, MIN(ResponsiblePersonName) AS TaskResponsiblePerson 
										FROM SurveysPerTaskView subs INNER JOIN TasksView subt on subs.TaskID = subt.TaskID 
										GROUP BY SurveyID
										) sub ON sub.SurveyID = SurveysView.SurveyID";
                if (where != "")
                    sql += " WHERE " + where;
                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    gvAnswers.EmptyDataText = "No Records Found.  Please set the filter criteria and click Apply.";
                    gvAnswers.DataSource = data.ExecuteReader(sql);
                    gvAnswers.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}