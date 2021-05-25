using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyReporting.Pages
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnNoResponses_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/NoResponses.aspx");
        }

        protected void btnResultSummary_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/ResultsSummary.aspx");
        }

        protected void btnAnswers_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Answers.aspx");
        }

        protected void btnProjectLeaderSummary_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/ProjectLeaderSummary.aspx");
        }

        protected void btnResultAnalysis_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/ResultAnalysis.aspx");
        }

        protected void btnSurveyablePeople_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/SurveyablePeople.aspx");
        }

        protected void btnSendReminders_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/SendReminders.aspx");
        }
    }
}