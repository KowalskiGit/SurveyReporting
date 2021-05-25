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
    public partial class Filter : System.Web.UI.UserControl
    {
        string _whereclause = "";
        string _heading = "";
        public event EventHandler FilterApplied;

        public string WhereClause
        {
            get { return _whereclause; }
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
                //Bind Survey Status
                string sql = @"SELECT ID, Name
                               FROM Statuses
                               ORDER BY ID";
                SqlDataReader dr;
                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    using (dr = data.ExecuteReader(sql))
                    {
                        SurveyStatus.DataSource = dr;
                        SurveyStatus.DataValueField = "ID";
                        SurveyStatus.DataTextField = "Name";
                        SurveyStatus.DataBind();
                    }

                    //Bind Survey Type
                    sql = @"SELECT ID, Name
                            FROM Types
                            ORDER BY ID";
                    using (dr = data.ExecuteReader(sql))
                    {
                        SurveyType.DataSource = dr;
                        SurveyType.DataValueField = "ID";
                        SurveyType.DataTextField = "Name";
                        SurveyType.DataBind();
                    }

                    //Bind Project Manager
                    sql = @"SELECT PersonnelID, MIN(FirstName + ' ' + Surname) AS FullName 
                            FROM PersonnelView Pe INNER JOIN ProjectsView Pr ON
                                Pe.PersonnelID = Pr.ProjectLeader
                            GROUP BY Pe.PersonnelID
                            ORDER BY FullName";
                    using (dr = data.ExecuteReader(sql))
                    {
                        ProjectManager.DataSource = dr;
                        ProjectManager.DataValueField = "PersonnelID";
                        ProjectManager.DataTextField = "FullName";
                        ProjectManager.DataBind();
                    }

                    //Bind Section
                    sql = @"SELECT Section
                            FROM OrganisationUnitSections
                            GROUP BY Section
                            ORDER BY Section";
                    using (dr = data.ExecuteReader(sql))
                    {
                        Group.DataSource = dr;
                        Group.DataValueField = "Section";
                        Group.DataTextField = "Section";
                        Group.DataBind();
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
                //Get a Where clause from the fields that was entered.
                string where = "";
                string heading = "";

                //SurveyFrom
                bool isDate;
                DateTime theDate;
                if (SurveyFrom.Text != "")
                {
                    isDate = DateTime.TryParse(SurveyFrom.Text, out theDate);
                    if (isDate)
                    {
                        where += "SurveysView.DatePublished >= " + theDate.DBValue();
                        heading += " from " + theDate.Date;
                    }
                }
                //SurveyTo
                if (SurveyTo.Text != "")
                {
                    isDate = DateTime.TryParse(SurveyTo.Text + " 23:59:29", out theDate);
                    if (isDate)
                    {
                        where = Functions.Conjunct(where, "SurveysView.DatePublished <= " + theDate.DBValue());
                        heading += " to " + theDate.Date;
                    }
                }

                //Survey Name
                if (SurveyName.Text != "")
                {
                    where = Functions.Conjunct(where, "SurveysView.Name LIKE '%" + SurveyName.Text + "%'");
                    heading += " and Survey Name contains " + SurveyName.Text;
                }

                //Survey Status
                if (SurveyStatus.Text != "")
                {
                    where = Functions.Conjunct(where, "SurveysView.StatusID = " + SurveyStatus.Text.DBValue());
                    heading += " for Survey Status " + SurveyStatus.Text;
                }

                //Survey Type
                if (SurveyType.Text != "")
                {
                    where = Functions.Conjunct(where, "SurveysView.TypeID = " + SurveyType.Text.DBValue());
                    heading += " for Survey Type " + SurveyType.Text;
                }

                //ProjectManager
                if (ProjectManager.Text != "")
                {
                    where = Functions.Conjunct(where, "SurveysView.ProjectLeader = " + ProjectManager.Text.DBValue());
                    heading += " and for Project Manager " + ProjectManager.Text;
                }

                //Group / Section
                if (Group.Text != "")
                {
                    where = Functions.Conjunct(where, "SurveysView.Section = " + Group.Text.Trim().DBValue());
                    heading += " and for Group " + Group.Text.Trim();
                }

                //Organisation Unit
                if (OrganisationUnit.Text != "")
                {
                    if (RollupOrgs.Checked)
                    {
                        where = Functions.Conjunct(where, "SurveysView.OrganisationUnit IN (" + 
                                                   GeneralFunctions.LowerLevelOrganisationUnits(OrganisationUnit.Text) + ")");
                        heading += " and for Organisation Unit " + OrganisationUnit.Text + ", including lower levels";
                    }
                    else
                    {
                        where = Functions.Conjunct(where, "SurveysView.OrganisationUnit = " + OrganisationUnit.Text.DBValue());
                        heading += " and for Organisation Unit " + OrganisationUnit.Text;
                    }
                }

                //Assign control 
                WhereClause = where;
                //Remove first word from Heading
                heading = heading.Trim();
                heading = heading.Substring(heading.IndexOf(" ") + 1);
                Heading = heading;
                
                FilterApplied.Invoke(this, new EventArgs());
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        protected void SurveyFrom_Load(object sender, EventArgs e)
        {
            SurveyFrom.Attributes.Add("onclick", "return GetDate(this);");
        }

        protected void SurveyTo_Load(object sender, EventArgs e)
        {
            SurveyTo.Attributes.Add("onclick", "return GetDate(this);");
        }
    }
}