using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Foundation.Data.SqlClient;
using System.Data.SqlClient;
using Barnstone;

namespace SurveyReporting.Controls
{
    public partial class ProjectFilter : System.Web.UI.UserControl
    {
        string _whereclause = "";
        string _heading = "";
        public event EventHandler FilterApplied;

        public string WhereClause
        {
            get 
            {
                if (_whereclause == "")
                    GetWhereClause();
                return _whereclause; 
            }
            set { _whereclause = value; }
        }

        public string Heading
        {
            get { return _heading; }
            set { _heading += value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Bind Project ID
                string sql = @"SELECT ProjectID, ProjectID + ': ' + ProjectName AS ProjectDescription
                               FROM ProjectsView
                               ORDER BY ProjectID";
                SqlDataReader dr;
                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    using (dr = data.ExecuteReader(sql))
                    {
                        ProjectID.DataSource = dr;
                        ProjectID.DataValueField = "ProjectID";
                        ProjectID.DataTextField = "ProjectDescription";
                        ProjectID.DataBind();
                    }

                    //Bind Company ID
                    sql = @"SELECT CompanyID, CompanyID + ' ' + CompanyName AS CompanyDescription
                            FROM CompaniesView
                            ORDER BY CompanyID";
                    using (dr = data.ExecuteReader(sql))
                    {
                        CompanyID.DataSource = dr;
                        CompanyID.DataValueField = "CompanyID";
                        CompanyID.DataTextField = "CompanyDescription";
                        CompanyID.DataBind();
                    }

                    //Bind OrganisationUnitsView
                    sql = @"SELECT OrganisationUnitID, OrganisationUnitID + ': ' + Description AS OUDescription
                            FROM OrganisationUnitsView
                            ORDER BY Description";
                    using (dr = data.ExecuteReader(sql))
                    {
                        OrganisationUnit.DataSource = dr;
                        OrganisationUnit.DataValueField = "OrganisationUnitID";
                        OrganisationUnit.DataTextField = "OUDescription";
                        OrganisationUnit.DataBind();
                    }
                }
            }
        }

        protected void ApplyFilter_Click(object sender, EventArgs e)
        {
            try
            {
                GetWhereClause();

                FilterApplied.Invoke(this, new EventArgs());
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetWhereClause()
        {
            //Get a Where clause from the fields that was entered.
            string where = "";
            string heading = "";

            //Project ID
            if (ProjectID.Text != "")
            {
                where = Functions.Conjunct(where, "ProjectsView.ProjectID = " + ProjectID.Text.DBValue());
                heading += " for Project " + ProjectID.Text;
            }

            //Project Name
            if (ProjectName.Text != "")
            {
                where = Functions.Conjunct(where, "ProjectsView.ProjectName LIKE '%" + ProjectName.Text + "%'");
                heading += " and Project Name contains " + ProjectName.Text;
            }

            //ProjectManager
            if (ProjectManager.Text != "")
            {
                where = Functions.Conjunct(where, "PersonnelView.FirstName + ' ' + PersonnelView.Surname LIKE '%" + ProjectManager.Text + "%'");
                heading += " and Project Manager contains " + ProjectManager.Text;
            }

            //Contact Person
            if (ContactPerson.Text != "")
            {
                where = Functions.Conjunct(where, "ContactPersonID LIKE '%" + ContactPerson.Text + "%'");
                heading += " and Contact Person contains " + ContactPerson.Text;
            }

            //Company ID
            if (CompanyID.Text != "")
            {
                where = Functions.Conjunct(where, "CompaniesView.CompanyID = " + CompanyID.Text.DBValue());
                heading += " for Company " + CompanyID.Text;
            }

            //Company Name
            if (CompanyName.Text != "")
            {
                where = Functions.Conjunct(where, "CompanyName LIKE '%" + CompanyName.Text + "%'");
                heading += " for Company that contains " + CompanyID.Text;
            }

            //Organisation Unit
            if (OrganisationUnit.Text != "")
            {
                if (Rollup.Checked)
                {
                    where = Functions.Conjunct(where, "TasksView.OrganisationUnit IN (" +
                                               GeneralFunctions.LowerLevelOrganisationUnits(OrganisationUnit.Text) + ")");
                    heading += " and for Organisation Unit " + OrganisationUnit.Text + ", including lower levels";
                }
                else
                {
                    where = Functions.Conjunct(where, "TasksView.OrganisationUnit = " + OrganisationUnit.Text.DBValue());
                    heading += " and for Organisation Unit " + OrganisationUnit.Text;
                }
            }

            //Tasks Closed
            bool isDate;
            DateTime theDate;
            if (TasksClosedFrom.Text != "")
            {
                isDate = DateTime.TryParse(TasksClosedFrom.Text, out theDate);
                if (isDate)
                {
                    where += "TasksView.ClosedOn >= " + theDate.DBValue();
                    heading += " Tasks Closed From " + theDate.Date;
                }
            }
            //SurveyTo
            if (TasksClosedTo.Text != "")
            {
                isDate = DateTime.TryParse(TasksClosedTo.Text + " 23:59:29", out theDate);
                if (isDate)
                {
                    where = Functions.Conjunct(where, "TasksView.ClosedOn <= " + theDate.DBValue());
                    heading += " Tasks Closed To " + theDate.Date;
                }
            }

            //And the Yes/No fields
            if (Closed.Text == "True")
            {
                where = Functions.Conjunct(where, "ProjectsView.ClosedOn Is Not Null");
                heading += " and that is Closed";
            }
            else if (Closed.Text == "False")
            {
                where = Functions.Conjunct(where, "ProjectsView.ClosedOn Is Null");
                //Default: No heading.
            }

            //Exclude Action Plan
            if (ExcludeActionPlan.Text == "True")
            {
                where = Functions.Conjunct(where, "ProjectsView.ProjectType <> 'Act Plan'");
                heading += " excluding Action Plan Projects";
            }

            //Assign control 
            WhereClause = where;
            //Remove first word from Heading
            heading = heading.Trim();
            heading = heading.Substring(heading.IndexOf(" ") + 1);
            Heading = heading;
        }

        protected void TasksClosedFrom_Load(object sender, EventArgs e)
        {
            TasksClosedFrom.Attributes.Add("onclick", "return GetDate(this);");
        }

        protected void TasksClosedTo_Load(object sender, EventArgs e)
        {
            TasksClosedTo.Attributes.Add("onclick", "return GetDate(this);");
        }
    }
}