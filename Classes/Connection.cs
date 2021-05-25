using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SurveyReporting
{
    public static class Connection
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SurveyReporting"].ConnectionString;
            }
        }
    }
}
