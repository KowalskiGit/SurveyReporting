using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using Foundation.Data.SqlClient;
using Barnstone;
using System.Data.SqlClient;

namespace SurveyReporting
{
    public class GeneralFunctions
    {

        const char ElementSeperator = (char)7;

        public static string SubTree(string Identifier, DataTable dt, string IDName, string ParentIDName, bool ExcludeCurrentLevel)
        //Get a comma-delimited list of nodes in a subtree.
        {
            try
            {
                string treeList = "";
                Hashtable hierarchy = GetHierarchy(dt, IDName, ParentIDName);
                if (!ExcludeCurrentLevel)
                    treeList = Identifier.DBValue();

                foreach (DataRow dr in dt.Rows)
                {
                    if (!InPath(dr[0].ToString(), treeList))
                    {
                        if (InPath(Identifier, GetPath((string)dr[0], hierarchy)))
                            //if (ExcludeCurrentLevel && Identifier != dr[0].ToString())  //Previous if.  Have a bug when ExcludeCurrentLevel = true
                            if (Identifier != dr[0].ToString())
                                treeList = treeList + ElementSeperator + ((string)dr[0]).DBValue();
                    }
                }
                treeList = treeList.Replace(ElementSeperator, ',');
                //Remove initial comma
                if (treeList.Length > 0 && treeList.Substring(0, 1) == ",")
                    treeList = treeList.Substring(1);
                return treeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Hashtable GetHierarchy(DataTable dt, string IDName, string ParentIDName)
        //Load the hierarchy into a Collection (ItemName, ParentName)
        {
            try
            {
                Hashtable hierarchy = new Hashtable();

                foreach (DataRow dr in dt.Rows)
                {
                    hierarchy.Add(dr[IDName].ToString(), dr[ParentIDName].ToString());
                }
                return hierarchy;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string GetPath(string ItemID, Hashtable Hierarchy)
        //Determine the path from an element to the root of the tree.
        {
            try
            {
                byte counter = 0;
                string elements = ItemID;  //.Append
                string parentID = Hierarchy[ItemID].ToString();

                while (parentID != "" && counter < 100)
                {
                    elements = parentID + ElementSeperator + elements;  //.Insert
                    counter++;
                    parentID = (Hierarchy[parentID] + "").ToString();
                }
                return elements;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InPath(string ItemSearched, string Path)
        //Determine if an element is in the path.
        {
            try
            {
                Path = ElementSeperator + Path + ElementSeperator;
                return Path.IndexOf(ElementSeperator + ItemSearched + ElementSeperator) != -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Depth(string Path)
        //Get the depth of a node in a hierarchy
        {
            try
            {
                if (Path == "")
                    return 0;
                else
                {
                    int Position = 0, Counter = 0;
                    do
                    {
                        Position = Path.IndexOf(ElementSeperator, Position + 1);
                        Counter++;
                    } while (Position != -1);
                    return Counter;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string LowerLevelOrganisationUnits(string OrganisationUnit)
        //Return a list of all Organisation Units below the specified unit
        {
            Provider data = null;
            try
            {
                string sql = @"SELECT OrganisationUnitID, ParentOrganisationUnit
                               FROM OrganisationUnitsView";
                data = new Provider(Connection.ConnectionString, true);
                DataTable dt = data.ExecuteDataSet(sql).Tables[0];

                return GeneralFunctions.SubTree(OrganisationUnit, dt, "OrganisationUnitID", "ParentOrganisationUnit", false);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.Disconnect();
                data.Dispose();
            }
        }

        public static string DLookup(string field, string table, string whereclause = "")
        {
            Provider data = new Provider(Connection.ConnectionString, true);
            try
            {
                string sql = @"SELECT " + field + @" 
                               FROM " + table;
                if (whereclause != "")
                    sql += " WHERE " + whereclause;

                return data.ExecuteScalar(sql).ToString();
            }
            catch (NullReferenceException)
            {
                //Value does not exist in the database
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.Disconnect();
            }
        }

        public static int DSum(string field, string table, string whereclause = "")
        {
            Provider data = new Provider(Connection.ConnectionString, true);
            try
            {
                string sql = @"SELECT SUM(" + field + @") 
                               FROM " + table;
                if (whereclause != "")
                    sql += " WHERE " + whereclause;

                return int.Parse(data.ExecuteScalar(sql).ToString());
            }
            catch (NullReferenceException)
            {
                //Value does not exist in the database
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.Disconnect();
            }
        }

        public static int DCount(string field, string table, string whereclause = "")
        {
            Provider data = new Provider(Connection.ConnectionString, true);
            try
            {
                string sql = @"SELECT Count(" + field + @") 
                               FROM " + table;
                if (whereclause != "")
                    sql += " WHERE " + whereclause;

                return int.Parse(data.ExecuteScalar(sql).ToString());
            }
            catch (NullReferenceException)
            {
                //Value does not exist in the database
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.Disconnect();
            }
        }

        public static void SendMail(string sender, string recipients, string subject, string message)
        {
            string sp = "SendMail";
            using (var conn = new SqlConnection(Connection.ConnectionString))
            using (var command = new SqlCommand(sp, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                  )
            {
                command.Parameters.AddWithValue("@From", sender);
                command.Parameters.AddWithValue("@To", recipients);
                command.Parameters.AddWithValue("@Subject", subject);
                command.Parameters.AddWithValue("@Body", message);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}