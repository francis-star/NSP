using HCWeb2016;
using System;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class DAL_UserSetDts : SqlBase
    {
        /// <summary>
        /// 根据code查询用户
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetUser(string code)
        {
            string str = "SELECT *,CONVERT(NVARCHAR(10),User_EntryDate,120)[time] FROM AF_User WHERE User_Code='" + ValueHandler.GetStringValue(code) + "'";
            return SearchData(str);
        }

        /// <summary>
        /// 得到模块信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenu()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT *,0 AS SM_Type FROM AF_SysModule WHERE DataState = 0 ORDER BY SM_Index");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取用户的权限
        /// </summary>
        /// <param name="UserCode">用户Code</param>
        public DataTable GetUserModule(string UserCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT *,0 AS SM_Type FROM dbo.AF_UserPopedom WHERE UP_User_Code='" + UserCode + "'");
            return SearchData(sb.ToString());
        }

        #region 保存用户
        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="aF_User"></param>
        /// <returns></returns>
        public string SaveUser(AF_User aF_User)
        {
            StringBuilder strSql = new StringBuilder();
            if (ValueHandler.GetStringValue(aF_User.User_Code) == "")
            {
                DataTable dt = SearchData("SELECT COUNT(0) FROM AF_User WHERE User_LoginName='" + ValueHandler.GetStringValue(aF_User.User_LoginName) + "' OR User_Name='" + ValueHandler.GetStringValue(aF_User.User_Name) + "'");
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    return null;
            }
            else
            {
                DataTable dt = SearchData("SELECT COUNT(0) FROM AF_User WHERE (User_LoginName='" + ValueHandler.GetStringValue(aF_User.User_LoginName) + "' OR User_Name='" + ValueHandler.GetStringValue(aF_User.User_Name) + "') And User_Code<>'" + ValueHandler.GetStringValue(aF_User.User_Code) + "'");
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    return null;
            }
            strSql.Append("\r IF NOT EXISTS(SELECT * FROM AF_User WHERE User_Code ='" + ValueHandler.GetStringValue(aF_User.User_Code) + "')");
            strSql.Append("\r BEGIN ");
            strSql.Append("\r INSERT INTO AF_User(");
            strSql.Append("User_Code,User_LoginName,User_Password,User_Name,User_Sex,User_Age,User_Phone,User_Post,User_EntryDate,User_Place,JoinMan");
            strSql.Append(") SELECT ");
            string strCode = ValueHandler.GetStringValue(aF_User.User_Code);
            if (aF_User.User_Code == "")
                strCode = GetCode();
            strSql.Append("\r User_Code = '" + strCode + "',");
            strSql.Append(ConSql(aF_User));
            strSql.Append("\r END");
            strSql.Append("\r ELSE");
            strSql.Append("\r BEGIN ");
            strSql.Append("UPDATE AF_User SET ");
            strSql.Append(ConSql(aF_User));
            strSql.Append(" WHERE User_Code ='" + ValueHandler.GetStringValue(aF_User.User_Code) + "'");
            strSql.Append("\r END");
            UpdateData(strSql.ToString());
            return strCode;
        }
        /// <summary>
        /// 得到数据
        /// </summary>
        public string ConSql(AF_User Model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("User_LoginName = '" + ValueHandler.GetStringValue(Model.User_LoginName) + "', ");

            strSql.Append("User_Password = '" + Model.User_Password + "', ");

            strSql.Append("User_Name = '" + ValueHandler.GetStringValue(Model.User_Name) + "', ");

            strSql.Append("User_Sex = '" + ValueHandler.GetStringValue(Model.User_Sex) + "', ");

            strSql.Append("User_Age = " + ValueHandler.GetIntNumberValue(Model.User_Age) + ", ");

            strSql.Append("User_Phone = '" + ValueHandler.GetStringValue(Model.User_Phone) + "', ");

            strSql.Append("User_Post = '" + ValueHandler.GetStringValue(Model.User_Post) + "', ");

            strSql.Append("User_EntryDate = " + ValueHandler.GetMarkStringDateValue(Model.User_EntryDate) + ", ");

            strSql.Append("User_Place = '" + ValueHandler.GetStringValue(Model.User_Place) + "', ");

            strSql.Append("JoinMan = '" + ValueHandler.GetStringValue(Model.JoinMan) + "' ");
            return strSql.ToString();
        }
        #endregion

        #region 保存用户权限
        public bool SaveUserModule(ArrayList arr)
        {
            bool flag;
            SqlTransaction tran = null;//声明一个事务对象  
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnStringSQL"].ToString()))
                {
                    conn.Open();//打开链接  
                    using (tran = conn.BeginTransaction())
                    {
                        StringBuilder sb = new StringBuilder();
                        SqlCommand cmd = new SqlCommand("DELETE FROM AF_UserPopedom WHERE UP_User_Code='" + ValueHandler.GetStringValue(arr[0]) + "'", conn);
                        cmd.Transaction = tran;
                        cmd.ExecuteNonQuery();
                        var userCode = arr[0].ToString();

                        var q1 = arr.ToArray().ToList();
                        q1.RemoveAt(0);//删除第一个
                        var qModule = (from q in q1 select q.ToString().Split('|')[0]).Distinct().ToList();

                        foreach (var moudle in qModule)
                        {
                            sb.Remove(0, sb.Length);
                            var qModuleFuns = (from q in q1 where q.ToString().Contains(moudle + "|") select q.ToString().Replace(moudle + "|", "")).ToList();
                            DataTable dt = SearchData("SELECT SM_FunIDs FROM dbo.AF_SysModule WHERE SM_Code = '" + moudle + "'");
                            var mfuns = (from q in dt.AsEnumerable()
                                         select q.Field<string>("SM_FunIDs")).ToList();
                            if (mfuns.Count > 0)
                            {
                                sb.Append("\r INSERT INTO AF_UserPopedom(");
                                sb.Append("UP_Code,UP_User_Code,UP_SM_Code,UP_SM_FunIDs");
                                sb.Append(") SELECT ");
                                sb.Append("UP_Code = '" + GetCode() + "',");
                                sb.Append("UP_User_Code='" + userCode + "',UP_SM_Code='" + moudle + $"','{string.Join(",", mfuns[0].Split(',').ToList().Except(qModuleFuns).ToList())}';");
                            }
                            cmd.CommandText = sb.ToString();
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit(); //提交事务  
                        flag = true;
                    }
                }
            }
            catch
            {
                tran.Rollback();  //出错回滚 
                flag = false;
            }

            return flag;
        }

        #endregion
    }
}
