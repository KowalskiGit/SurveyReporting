using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SurveyReporting;
using Barnstone;
using Foundation.Data.SqlClient;
using System.Data;

namespace SurveyReporting.Pages
{
    public partial class SurveyablePeople : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Register event when filter is changed.
            this.ProjectFilter.FilterApplied += new EventHandler(this.FilterApplied);
        }


        protected void FilterApplied(object sender, EventArgs e)
        {
            //Reload the page to refresh the graphs.
            ApplyFilter();
        }


        private void ApplyFilter(string SortExpression = "ProjectID", string SortDirection = "ASC")
        {
            try
            {
                string where = ProjectFilter.WhereClause;

                string sql = @"SELECT * FROM (
                                    --Projects with default Contact Persons for Project Surveys
                                   SELECT ProjectsView.ProjectID, MIN(ProjectsView.ProjectName) AS ProjectName, MIN(ProjectLeader) AS ProjectLeader, 
                                          MIN(PersonnelView.FirstName + ' ' + PersonnelView.Surname) AS ProjectLeaderName, 
                                          MIN(ProjectsView.CompanyID) AS CompanyID, MIN(CompanyName) AS CompanyName, ContactPersonID AS ContactPersonID, MIN(C.Email) AS Email
                                   FROM ContactPersonsView C
                                        JOIN ProjectsView on ProjectsView.CompanyID = C.CompanyID
                                        JOIN PersonnelView on ProjectsView.ProjectLeader = PersonnelView.PersonnelID
                                        JOIN CompaniesView on CompaniesView.CompanyID = C.CompanyID
                                        JOIN TasksView ON ProjectsView.ProjectID = TasksView.ProjectID 
                                   WHERE " + Functions.Conjunct("C.IsDefaultForScheduledProjectSurvey = 1", where) + $@" 
                                   GROUP BY ProjectsView.ProjectID, C.ContactPersonID
                                   UNION ALL
                                   --Projects linked to Companies that does not have any Default Contact Person
                                   SELECT ProjectsView.ProjectID, MIN(ProjectsView.ProjectName) AS ProjectName, MIN(ProjectLeader) AS ProjectLeader, 
                                           MIN(PersonnelView.FirstName + ' ' + PersonnelView.Surname) AS ProjectLeaderName, 
                                           MIN(ProjectsView.CompanyID) AS CompanyID, MIN(CompanyName) AS CompanyName, NULL AS ContactPersonID, NULL AS Email
                                   FROM ContactPersonsView C
                                       JOIN ProjectsView on ProjectsView.CompanyID = C.CompanyID
                                       JOIN PersonnelView on ProjectsView.ProjectLeader = PersonnelView.PersonnelID
                                       JOIN CompaniesView on CompaniesView.CompanyID = C.CompanyID
                                       JOIN TasksView ON ProjectsView.ProjectID = TasksView.ProjectID 
                                   WHERE {Functions.Conjunct("1=1", where)}
                                   GROUP BY ProjectsView.ProjectID
                                   HAVING 
                                   (SELECT Count(1) FROM ContactPersonsView subcp WHERE subcp.CompanyID = MIN(ProjectsView.CompanyID) AND subcp.IsDefaultForScheduledProjectSurvey = 0) = 
                                   (SELECT Count(1) FROM ContactPersonsView subcp WHERE subcp.CompanyID = MIN(ProjectsView.CompanyID)) --NumberOfNonDefaultPersons = TotalContactPersons
                            ) Q ";
                sql += " ORDER BY " + SortExpression + " " + SortDirection;

                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    gvSurveyablePeople.EmptyDataText = "No Records Found.  Please set the filter criteria and click Apply.";
                    gvSurveyablePeople.DataSource = data.ExecuteReader(sql);
                    gvSurveyablePeople.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvSurveyablePeople_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (this.ViewState["SortDirection"] == null)
                    this.ViewState["SortDirection"] = "ASC";
                else if (this.ViewState["SortDirection"].ToString() == "ASC")
                    this.ViewState["SortDirection"] = "DESC";
                else if (this.ViewState["SortDirection"].ToString() == "DESC")
                    this.ViewState["SortDirection"] = "ASC";

                ApplyFilter(e.SortExpression, this.ViewState["SortDirection"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
   
    }   
}
