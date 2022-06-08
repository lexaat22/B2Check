using B2Check.model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B2Check
{
    public static class Utils
    {
        public static OracleConnection GetConnection(string name, string login, string password)
        {
            OracleConnection mConn = new OracleConnection();
            OracleConnectionStringBuilder connBuilder = new OracleConnectionStringBuilder(ConfigurationManager.ConnectionStrings[name].ConnectionString);
            connBuilder.UserID = login;
            connBuilder.Password = password;
            mConn.ConnectionString = connBuilder.ConnectionString;
            return mConn;
        }

        public static double GetProgress(string log, string pass)
        {
            double result = 0.0;
            OracleConnection bkConn = GetConnection("BkConn", log.ToUpper(), pass);
            OracleCommand cmd = new OracleCommand("select round(sms.black.getProgress, 5) from dual", bkConn);
            try
            {
                bkConn.Open();
                var o = cmd.ExecuteScalar();
                if (o != null)
                {
                    result = Convert.ToDouble(o);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                bkConn.Close();
            }
            return result;
        }

        public static int GetProcCount(string log, string pass)
        {
            int result = 0;
            OracleConnection bkConn = GetConnection("BkConn", log.ToUpper(), pass);
            OracleCommand cmd = new OracleCommand("select sms.black.getProcCount from dual", bkConn);
            try
            {
                bkConn.Open();
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    result = Convert.ToInt32(o);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                bkConn.Close();
            }
            return result;
        }

        public static List<BLSource> GetBLSources(string log, string pass)
        {
            List<BLSource> result = new List<BLSource>();
            OracleConnection bkConn = GetConnection("BkConn", log.ToUpper(), pass);
            OracleCommand cmd = new OracleCommand("select id, name from table(sms.black.getSources)", bkConn);
            OracleDataReader dr = null;
            try
            {
                bkConn.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(new BLSource()
                    {
                        Id = dr["id"].ToString(),
                        Name = dr["name"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (dr != null) { dr.Close(); }
                bkConn.Close();
            }
            return result;
        }

        public static void StartSearch(string log, string pass)
        {
            OracleConnection bkConn = GetConnection("BkConn", log.ToUpper(), pass);
            OracleCommand cmd = new OracleCommand("sms.black.StartSearch", bkConn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                bkConn.Open();
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                bkConn.Close();
            }
        }

        public static async Task<DataTable> GetResultTable(string log, string pass, int percent, string lists, string fio = "")
        {
            DataTable dt = new DataTable();
            OracleConnection bkConn = GetConnection("BkConn", log.ToUpper(), pass);
            OracleCommand cmd = new OracleCommand("select * from sms.black.getSearchResult(:proc, :list, :fio)", bkConn);
            OracleDataReader dr = null;
            try
            {
                cmd.Parameters.Clear();
                _ = cmd.Parameters.Add(new OracleParameter("proc", percent));
                _ = cmd.Parameters.Add(new OracleParameter("list", lists));
                _ = cmd.Parameters.Add(new OracleParameter("fio", fio.ToUpper()));

                dt.Columns.Add("checked", typeof(int));
                dt.Columns.Add("siteid", typeof(int));
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("name_b2", typeof(string));
                dt.Columns.Add("name_list", typeof(string));
                dt.Columns.Add("list_id", typeof(int));
                dt.Columns.Add("entity_id", typeof(int));
                dt.Columns.Add("sname", typeof(string));
                dt.Columns.Add("similar", typeof(int));
                dt.Columns.Add("stime", typeof(string));
                dt.Columns.Add("checked_by", typeof(string));

                bkConn.Open();

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DataRow row = dt.NewRow();
                    row["checked"] = Convert.ToInt32(dr["checked"].ToString());
                    row["siteid"] = Convert.ToInt32(dr["siteid"].ToString());
                    row["id"] = Convert.ToInt32(dr["id"].ToString());
                    row["name_b2"] = dr["name_b2"].ToString();
                    row["name_list"] = dr["name_list"].ToString();
                    row["list_id"] = Convert.ToInt32(dr["list_id"].ToString());
                    row["entity_id"] = Convert.ToInt32(dr["entity_id"].ToString());
                    row["sname"] = dr["sname"].ToString();
                    row["similar"] = Convert.ToInt32(dr["similar"].ToString());
                    row["stime"] = dr["stime"].ToString();
                    row["checked_by"] = dr["checked_by"].ToString();
                    dt.Rows.Add(row);
                }
            }
            catch 
            {
                throw;
            }
            finally
            {
                if (dr != null) { dr.Close(); }
                bkConn.Close();
            }
            return dt;
        }

        public static async Task SetChecked(string log, string pass, int site, int id, int list_id, int check)
        {
            OracleConnection bkConn = GetConnection("BkConn", log.ToUpper(), pass);
            OracleCommand cmd = new OracleCommand("sms.black.setChecked", bkConn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Clear();
                _ = cmd.Parameters.Add(new OracleParameter("site_", site));
                _ = cmd.Parameters.Add(new OracleParameter("id_", id));
                _ = cmd.Parameters.Add(new OracleParameter("list_id_", list_id));
                _ = cmd.Parameters.Add(new OracleParameter("checked_", check));

                bkConn.Open();

                _ = cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                bkConn.Close();
            }
        }
    }
}
