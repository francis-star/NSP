////////////////////////////://///////////////////////////////////////////////
//模块名：分类信息设定
//开发者：田维华
//开发时间：2016年11月10日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCWeb2016;
using System.Data;
using Model;

namespace DAL
{
    public class DAL_LevelSetDts : SqlBase
    {
        #region 获取模块信息（左侧菜单：资讯信息维护、企业信息维护等）
        /// <summary>
        /// 获取模块信息（左侧菜单：资讯信息维护、企业信息维护等）
        /// </summary>
        /// <returns></returns>
        public DataTable GetESysModule()
        {
            string sql = "SELECT SM_Code,SM_Name FROM E_SysModule WHERE SM_PCode='1017' AND SM_Name<>'企业信息维护'";
            return SearchData(sql);
        }
        #endregion

        #region 获取节点信息
        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="LS_Code">code</param>
        /// <returns>节点信息DataTable</returns>
        public DataTable GetLevelSet(string LS_Code)
        {
            string sql1 = string.Format("SELECT L1.LS_Code,L1.LS_Name,L2.LS_Name AS LS_PName,L2.LS_User_Code AS LS_PUser_Code,L1.LS_Type,L1.LS_User_Code FROM XXSD_levelSet L1 LEFT JOIN XXSD_levelSet L2 ON  L1.LS_PCode=L2.LS_Code WHERE   L1.LS_Code='{0}'", LS_Code);
            return SearchData(sql1);
        }
        #endregion

        #region 新增节点信息
        /// <summary>
        /// 新增节点信息
        /// </summary>
        /// <param name="LS_PCode">父级节点编号</param>
        /// <param name="LS_Name">节点名称</param>
        /// <param name="LS_Type">类型[仅第三级节点设置时使用]</param>
        /// <param name="LS_User_Code">可使用者编号</param>
        /// <returns></returns>
        public string InsertLevelSet(string LS_PCode, string LS_Name, string LS_Type, string LS_User_Code)
        {
            string sql1 = string.Format("SELECT LS_Name From XXSD_levelSet WHERE LS_PCode='{0}' AND LS_Name='{1}'", LS_PCode, LS_Name);
            DataTable dt = SearchData(sql1);
            if (dt.Rows.Count > 0)
            {
                return "-1";
            }
            else
            {
                string LS_Code = GetCode();
                string sql = string.Format("INSERT INTO XXSD_levelSet (LS_Code,LS_PCode,LS_Name,LS_Type,LS_User_Code) VALUES ('{0}','{1}','{2}','{3}','{4}')", LS_Code, LS_PCode, LS_Name, LS_Type, LS_User_Code);
                return UpdateData(sql).ToString().ToLower();
            }
        }
        #endregion

        #region 修改节点信息
        /// <summary>
        /// 修改节点信息
        /// </summary>
        /// <param name="LS_Code">节点编号</param>
        /// <param name="LS_Name">节点名称</param>
        /// <param name="LS_Type">类型[仅第三级节点设置时使用]</param>
        /// <param name="LS_User_Code">可使用者编号</param>
        /// <returns></returns>
        public string UpdateLevelSet(string LS_Code, string LS_Name, string LS_Type, string LS_User_Code)
        {
            DataTable dt1 = GetLevelSet(LS_Code);//获取节点信息
            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["LS_Name"].ToString() != LS_Name)//判断节点名称是否改变
                {
                    string sql1 = string.Format("SELECT LS_Name From XXSD_levelSet WHERE LS_PCode IN (SELECT LS_PCode FROM XXSD_levelSet WHERE LS_Code='{0}') AND LS_Name='{1}'", LS_Code, LS_Name);
                    DataTable dt = SearchData(sql1);
                    if (dt.Rows.Count > 0)
                    {
                        return "-1";//存在同名节点
                    }
                    else
                    {
                        string sql = string.Format("UPDATE XXSD_levelSet SET LS_Name = '{0}',LS_Type='{1}',LS_User_Code='{2}' WHERE LS_Code = '{3}'", LS_Name, LS_Type, LS_User_Code, LS_Code);
                        return UpdateData(sql).ToString().ToLower();
                    }
                }
                else
                {
                    //节点名称为改变
                    string sql = string.Format("UPDATE XXSD_levelSet SET LS_Name = '{0}',LS_Type='{1}',LS_User_Code='{2}' WHERE LS_Code = '{3}'", LS_Name, LS_Type, LS_User_Code, LS_Code);
                    return UpdateData(sql).ToString().ToLower();
                }
            }
            else
            {
                return "false";
            }
        }
        #endregion

        

        #region 获取用户的信息(E_User表)
        /// <summary>
        /// 获取用户的信息(E_User表)
        /// </summary>
        /// <param name="LS_Code">节点编号</param>
        /// <returns></returns>
        public DataTable GetUsers(string LS_Code)
        {
            StringBuilder sql = new StringBuilder();

            DataTable Dt1 = GetLevelSet(LS_Code);
            if (Dt1.Rows.Count > 0)
            {
                StringBuilder WhereStr = new StringBuilder();
                sql.Append("SELECT User_Code AS UserCode,[User_Name] AS UserName,User_Post FROM dbo.AF_User WHERE 1=1");
                string LS_User_Code = Dt1.Rows[0]["LS_User_Code"].ToString();
                string[] LS_User_Codes = LS_User_Code.Split(';');
                if (LS_User_Codes.Length > 0)
                {
                    for (int i = 0; i < LS_User_Codes.Length; i++)
                    {
                        if (LS_User_Codes[i].ToString() != "" && LS_User_Codes[i].ToString() != null)
                        {
                            if (i == 0)
                                WhereStr.AppendFormat("User_Code='{0}'", LS_User_Codes[i].ToString());
                            WhereStr.AppendFormat(" OR User_Code='{0}'", LS_User_Codes[i].ToString());
                        }
                    }
                    if (WhereStr.ToString() != "" && WhereStr.ToString() != null)
                        sql.AppendFormat(" AND ({0})", WhereStr.ToString());
                }
            }
            else
            {
                sql.Append("SELECT User_Code AS UserCode,[User_Name] AS UserName FROM dbo.AF_User");
            }

            DataTable dt2 = SearchData(sql.ToString());
            #region 加载职位，未成功
            //DataTable dt3 = new DataTable();
            //if (dt2.Rows.Count > 0)
            //{
            //    StringBuilder WhereStrs = new StringBuilder();
            //    for (int k = 0; k < dt2.Rows.Count; k++)
            //    {
            //        if (k == 0)
            //            WhereStrs.AppendFormat(" U.User_Post='{0}'", dt2.Rows[k]["User_Post"].ToString());
            //        else
            //            WhereStrs.AppendFormat(" OR U.User_Post='{0}'", dt2.Rows[k]["User_Post"].ToString());
            //    }
            //    StringBuilder sql3 = new StringBuilder();
            //    sql3.Append("SELECT S.CSet_Code,S.CSet_Name FROM E_User U,E_CodeSet S WHERE U.User_Post=S.CSet_Code");
            //    if (WhereStrs.Length > 0)
            //        sql3.AppendFormat(" AND ({0})", WhereStrs);
            //    sql3.Append("GROUP BY S.CSet_Code,S.CSet_Name");
            //    dt3 = SearchData(sql3.ToString());
            //}
            //if (dt3.Rows.Count > 0)
            //{
            //    for (int j = 0; j < dt3.Rows.Count; j++)
            //    {
            //        DataRow dr = dt2.NewRow();
            //        dr["UserCode"] = dt3.Rows[j]["CSet_Code"].ToString();
            //        dr["UserName"] = dt3.Rows[j]["CSet_Name"].ToString();
            //        dr["User_Post"] = "";
            //        dt2.Rows.Add(dr);
            //        dt2.AcceptChanges();
            //    }
            //}
            #endregion

            return dt2;
        }
        #endregion
    }
}
