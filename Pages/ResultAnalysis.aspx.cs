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
using System.Data.SqlClient;
using System.Configuration;


namespace SurveyReporting.Pages
{
    public class NameValueDto
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }

    public partial class ResultAnalysis : System.Web.UI.Page
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
                string where = Functions.Conjunct("OrganisationUnit Is Not Null", Filter.WhereClause);
                //Change where so that it looks in the SurveyPerTasksView as the OU filter should run per Task.
                if (where != "")
                {
                    where = where.Replace("SurveysView.", "SurveysPerTaskView.");
                }

                //Get OU's to build Data Table columns up
                string sql = @"SELECT OrganisationUnit 
                               FROM SurveysPerTaskView";
                if (where != "")
                    sql += " WHERE " + where;
                sql += "GROUP BY OrganisationUnit";

                DataTable dtOUs = new DataTable();
                SqlConnection con = new SqlConnection(Connection.ConnectionString);
                con.Open();
                SqlCommand command = new SqlCommand(sql, con);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dtOUs.Load(reader);
                }

                //Build up cols for final DT.
                DataTable results = new DataTable();
                results.Columns.Add("CategoryName", typeof(string));
                foreach (DataRow ou in dtOUs.Rows)
                {
                    DataColumn col1 = results.Columns.Add(ou[0].ToString(), typeof(double));
                    col1.AllowDBNull = true;
                    col1.DefaultValue = 0;
                }
                DataColumn col = results.Columns.Add("Average", typeof(double));
                col.AllowDBNull = true;
                col.DefaultValue = 0;

                //Get list of Categories
                List<string> categories = new List<string>();
                sql = @"SELECT CategoryName 
                        FROM  SurveysPerTaskView
                        INNER JOIN SurveyRecipientQuestionResponses A on SurveysPerTaskView.SurveyID = A.SurveyID 
                        INNER JOIN Questions Q on A.QuestionID = Q.ID";
                if (where != "")
                    sql += " WHERE " + where;
                sql += "GROUP BY CategoryName";
                using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
                {
                    connection.Open();
                    using (command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                categories.Add(reader.GetString(0));
                        }
                    }
                }

                //Get Avg ratings per OU
                sql = @"SELECT CategoryName, OrganisationUnit, Avg(Rating) Rating, Count(Distinct concat(QuestionID, ContactPersonID)) AS NoOfSurveys
                        FROM SurveysPerTaskView
                        INNER JOIN SurveyRecipientQuestionResponses A on SurveysPerTaskView.SurveyID = A.SurveyID 
                        INNER JOIN Questions Q on A.QuestionID = Q.ID";
                if (where != "")
                    sql += " WHERE " + where;
                sql += @"GROUP BY CategoryName, OrganisationUnit
                        ORDER BY 1, 2";

                DataTable subR = new DataTable();
                command = new SqlCommand(sql, con);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    subR.Load(reader);
                }

                //Iterate through avg results per OU and populate the main results data table.
                IList<NameValueDto> ouGraph = new List<NameValueDto>();

                //Get first row (averages)
                DataRow row1 = results.NewRow();
                row1["CategoryName"] = "Average";
                double runningTotal = 0;
                
                foreach (DataRow ou in dtOUs.Rows)
                {
                    NameValueDto ouGraphRow = new NameValueDto();
                    double avg = Convert.ToDouble(subR.AsEnumerable()
                                      .Where(p => p.Field<string>("OrganisationUnit") == ou[0].ToString())
                                      .Select(p => p.Field<double>("Rating")).Average());
                    row1[ou[0].ToString()] = Math.Round(avg, 2);
                    runningTotal += avg;

                    //For the graph
                    ouGraphRow.Name = ou[0].ToString() + " (" + 
                                      GeneralFunctions.DLookup("Description", "OrganisationUnitsView", $"OrganisationUnitID = '{ou[0].ToString()}'") + ")";
                    ouGraphRow.Value = Math.Round((decimal)avg, 2);
                    ouGraph.Add(ouGraphRow);
                }
                row1["Average"] = Math.Round(runningTotal / dtOUs.Rows.Count, 2); //Average of averages
                results.Rows.Add(row1);

                //Fill average of grid
                List<NameValueDto> categoryGraph = new List<NameValueDto>();

                foreach (var category in categories)
                {
                    NameValueDto catGraphRow = new NameValueDto();
                    DataRow row;
                    row = results.NewRow();
                    row["CategoryName"] = category;
                    catGraphRow.Name = category;

                    byte colnr = 0;
                    int surveyCount = 0;
                    double runningAverage = 0;
                    foreach (DataRow ou in dtOUs.Rows)
                    {
                        colnr++;
                        //Individual value
                        double value = Convert.ToDouble(subR.AsEnumerable()
                                              .Where(p => p.Field<string>("CategoryName") == category
                                                       && p.Field<string>("OrganisationUnit") == ou[0].ToString())
                                              .Select(p => p.Field<double>("Rating")).FirstOrDefault());
                        row[colnr] = value;

                        //Total Surveys in the group
                        int count = Convert.ToInt32(subR.AsEnumerable()
                                         .Where(p => p.Field<string>("CategoryName") == category
                                                  && p.Field<string>("OrganisationUnit") == ou[0].ToString())
                                         .Select(p => p.Field<int>("NoOfSurveys")).FirstOrDefault());
                        surveyCount += count;
                        runningAverage += value * count;
                    }

                    //Row average
                    row["Average"] = runningAverage / surveyCount;
                    results.Rows.Add(row);

                    //Build up data source for graphs.
                    catGraphRow.Value = Math.Round((decimal)runningAverage / surveyCount, 2);
                    categoryGraph.Add(catGraphRow);
                }

                try
                {
                    gvResultAnalysis.EmptyDataText = "No Records Found";
                    gvResultAnalysis.DataSource = results;  
                    gvResultAnalysis.DataBind();

                    //Draw graphs
                    PerOUGraph.Series[0].Points.DataBind(ouGraph, "Name", "Value",
                                                                 "Tooltip = AverageRating");
                    PerCategoryGraph.Series[0].Points.DataBind(categoryGraph, "Name", "Value",
                                                                 "Tooltip = AverageRating");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }



                //DrawPerOUGraph(Filter.WhereClause);
                //DrawPerCategoryGraph(Filter.WhereClause);


                //Old crosstab query
                //                string sql = @"EXEC CrossTab 
                //                                    'SurveysView
                //		                                  inner join SurveyRecipientQuestionResponses A on SurveysView.SurveyID = A.SurveyID AND A.SurveyID = SurveysView.SurveyID
                //		                                  inner join Questions Q on A.QuestionID = Q.ID',  --  @SQLSource     
                //                                    'OrganisationUnit',                                    --  @ColFieldID    
                //                                    'OrganisationUnit',                                    --  @ColFieldName  
                //                                    'OrganisationUnit',                                    --  @ColFieldOrder 
                //                                    'Rating',											   --  @CalcFieldName 
                //                                    'CategoryName',										   --  @RowFieldNames 
                //                                    Null,                                                  --  @TempTableName 
                //                                    'AVG',                                                 --  @CalcOperation 
                //                                    0,                                                     --  @Debug         
                //                                    " + where.DBValue() + @",                              --  @SourceFilter  
                //                                    0,                                                     --  @NumColOrdering
                //                                    'Total',                                               --  @RowTotals    
                //                                    NULL,                                                  --  @ColTotals    
                //                                    'CategoryName',                                        --  @OrderBy      
                //                                    'float'                                                --  @CalcFieldType";




                //SqlConnection con = new SqlConnection(Connection.ConnectionString);
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "CrossTab";
                //cmd.Parameters.Add("@SQLSource", SqlDbType.VarChar).Value = @"SurveysPerTaskView
                //                    inner join SurveyRecipientQuestionResponses A 
                //                                on SurveysPerTaskView.SurveyID = A.SurveyID 
                //                               AND A.SurveyID = SurveysPerTaskView.SurveyID
                //                    inner join Questions Q on A.QuestionID = Q.ID";
                //cmd.Parameters.Add("@ColFieldID", SqlDbType.VarChar).Value = "OrganisationUnit";
                //cmd.Parameters.Add("@ColFieldName", SqlDbType.VarChar).Value = "OrganisationUnit";
                //cmd.Parameters.Add("@ColFieldOrder", SqlDbType.VarChar).Value = "OrganisationUnit";
                //cmd.Parameters.Add("@CalcFieldName", SqlDbType.VarChar).Value = "Rating";
                //cmd.Parameters.Add("@RowFieldNames", SqlDbType.VarChar).Value = "CategoryName";
                ////cmd.Parameters.Add("@TempTableName", SqlDbType.VarChar).Value = "Null";
                //cmd.Parameters.Add("@CalcOperation", SqlDbType.VarChar).Value = "AVG";
                //cmd.Parameters.Add("@Debug", SqlDbType.Bit).Value = 0;
                //cmd.Parameters.Add("@SourceFilter", SqlDbType.VarChar).Value = where.ToString();
                //cmd.Parameters.Add("@NumColOrdering", SqlDbType.Bit).Value = 0;
                //cmd.Parameters.Add("@RowTotals", SqlDbType.VarChar).Value = "Average";  //Possibly remove as this is the average of averages
                //cmd.Parameters.Add("@ColTotals", SqlDbType.VarChar).Value = "' Average'";
                //cmd.Parameters.Add("@OrderBy", SqlDbType.VarChar).Value = "CategoryName";
                //cmd.Parameters.Add("@CalcFieldType", SqlDbType.VarChar).Value = "float";

                //cmd.Connection = con;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvResultAnalysis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (byte i = 1; i <= e.Row.Cells.Count - 1; i++)
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                        if (e.Row.Cells[i].Text == "0")  //Don't display zero's.
                            e.Row.Cells[i].Text = "";
                        else
                            e.Row.Cells[i].Text = Math.Round(Decimal.Parse(e.Row.Cells[i].Text), 2).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void DrawPerOUGraph(string WhereClause)
        {
            try
            {
                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    //Bind graph to reader.
                    string where = WhereClause == "" ? "" : "WHERE " + WhereClause + " ";
                    if (where != "")
                        where = where.Replace("SurveysView.", "SurveysPerTaskView.");

                    SqlDataReader dr = data.ExecuteReader(@"SELECT OU.OrganisationUnitID + ' (' + LEFT(MIN(OU.Description), 20) + ')' AS OrganisationUnit, 
                                                                   ROUND(Avg(A.Rating),1) AS AverageRating
                                                            FROM SurveyRecipientQuestionResponses A 
                                                                 INNER JOIN SurveysPerTaskView ON SurveysPerTaskView.SurveyID = A.SurveyID 
                                                                 INNER JOIN OrganisationUnitsView OU ON OU.OrganisationUnitID = SurveysPerTaskView.OrganisationUnit " +
                                                            where + @" 
                                                            GROUP BY OU.OrganisationUnitID");
                    PerOUGraph.Series[0].Points.DataBind(dr, "OrganisationUnit", "AverageRating",
                                                             "Tooltip = AverageRating");
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void DrawPerCategoryGraph(string WhereClause)
        {
            try
            {
                using (Provider data = new Provider(Connection.ConnectionString, true))
                {
                    //Bind Point graph to reader.
                    string where = WhereClause == "" ? "" : "WHERE " + WhereClause + " ";
                    if (where != "")
                        where = where.Replace("SurveysView.", "SurveysPerTaskView.");

                    SqlDataReader dr = data.ExecuteReader(@"SELECT Q.CategoryName, 
                                                                   ROUND(Avg(A.Rating),1) AS AverageRating
                                                            FROM SurveyRecipientQuestionResponses A 
                                                                 INNER JOIN Questions Q ON Q.ID = A.QuestionID                                                                 
                                                                 INNER JOIN SurveysPerTaskView ON SurveysPerTaskView.SurveyID = A.SurveyID 
                                                                 INNER JOIN OrganisationUnitsView OU ON OU.OrganisationUnitID = SurveysPerTaskView.OrganisationUnit " +
                                                            where + @" 
                                                            GROUP BY Q.CategoryName");
                    PerCategoryGraph.Series[0].Points.DataBind(dr, "CategoryName", "AverageRating",
                                                             "Tooltip = AverageRating");
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}