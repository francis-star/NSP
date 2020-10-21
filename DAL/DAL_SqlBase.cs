using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_SqlBase
    {
        /// <summary>
        /// string-字段值,object-图片信息
        /// </summary>
        public Dictionary<string, object> ImageList = new Dictionary<string, object>();

        public static SqlConnection getCon()
        {
            string strConString = ConfigurationManager.AppSettings["ConnStringSQL"].ToString();

            SqlConnection con = new SqlConnection(strConString);
            return con;
        }


        /// <summary>
        /// 执行SQL语句填充dataset
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        public static DataSet FillDataSet(string sql)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = getCon();
            try
            {
                conn.Open();
                SqlCommand sqlCommd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommd);
                adapter.Fill(ds);
                conn.Close();
                return ds;
            }
            catch
            {
                conn.Close();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行无返回数据表的sql语句
        /// </summary>
        /// <returns></returns>
        public static bool UpdateData(string sql)
        {
            SqlConnection conn = getCon();
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                conn.Close();
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行无返回数据表的sql语句,带1个图片数据流
        /// </summary>
        /// <returns></returns>
        public static bool UpdateData(string sql, byte[] ImageData)
        {
            SqlConnection conn = getCon();
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                if (ImageData != null)
                {
                    cmd.Parameters.Add("@Data", SqlDbType.Image);
                    cmd.Parameters["@Data"].Value = ImageData;
                }
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                conn.Close();
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行带参数的无返回数据的存储过程
        /// </summary>
        /// <param name="SpName"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static bool ExcuteNonQuery_Sp(string SpName, SqlParameter[] parms, out int intResult)
        {
            SqlConnection conn = getCon();
            SqlCommand cmd = new SqlCommand(SpName, conn);

            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parm in parms)
                cmd.Parameters.Add(parm);
            try
            {
                conn.Open();
                //PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, SpName, parms);
                cmd.ExecuteNonQuery();
                intResult = 0;
                return true;
            }
            catch
            {
                conn.Close();
                intResult = 3;
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        public static DataTable SearchData(string sql)
        {
            SqlConnection conn = getCon();
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// 得到table数据源
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataRow GetTableRow(string sql)
        {
            SqlConnection conn = getCon();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = cmd;
                da.Fill(ds);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
                return null;
            }
            catch (Exception)
            {
                conn.Close();
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 增加Table带序号列
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="IndexName"></param>
        public static void AddRowIndex(DataTable dt, string IndexName)
        {
            DataColumn dc = new DataColumn(IndexName);
            dt.Columns.Add(dc);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][IndexName] = i + 1;
            }
        }


        //系统编码
        protected string GetCode()
        {
            return ValueHandler.GetStringValue(SearchData(@"DECLARE @NCode nvarchar(20)
                        exec dbo.SP_GetCode @NCode output
                        select @NCode").Rows[0][0]);
        }

        /// <summary>
        /// 使用SqlBulkCopy 提交DataTable到数据
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static bool ExecuteInsert(DataTable dt, string tableName)
        {
            using (SqlConnection con = getCon())
            {
                try
                {
                    con.Open();
                    using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(con))
                    {
                        //DataTable schema = new DataTable();
                        //schema = con.GetSchema();

                        sqlbulkcopy.DestinationTableName = tableName;
                        sqlbulkcopy.BulkCopyTimeout = 18000;
                        sqlbulkcopy.WriteToServer(dt);

                        return true;
                    }
                }
                catch
                {
                    //throw new Exception(ex.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
