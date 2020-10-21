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
    public class DAL_LevelSet : SqlBase
    {
        #region 获取分类信息节点
        /// <summary>
        /// 获取分类信息节点
        /// </summary>
        /// <returns>分类信息节点表</returns>
        public DataTable GetLevelSets()
        {
            //string sql = string.Format("SELECT L.LS_Code,L.LS_PCode,(L.LS_Name+ISNULL(' | '+M.SM_Name,'')) AS LS_Name,L.LS_Type FROM XXSD_levelSet L LEFT JOIN E_SysModule M ON L.LS_Type=M.SM_Code WHERE L.DataState = 0 ORDER BY L.JoinDate DESC");
            string sql = "SELECT L.LS_Code,L.LS_PCode,L.LS_Name,L.LS_Type FROM XXSD_levelSet L  WHERE L.DataState = 0 ORDER BY L.JoinDate DESC";
            return SearchData(sql);
        }
        #endregion

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
                sql.Append("SELECT User_Code AS UserCode,[User_Name] AS UserName,User_Post FROM dbo.E_User WHERE User_LeaveDate IS NULL");
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
                sql.Append("SELECT User_Code AS UserCode,[User_Name] AS UserName FROM dbo.E_User WHERE User_LeaveDate IS NULL");
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

        #region 删除节点信息
        /// <summary>
        /// 删除节点信息
        /// </summary>
        /// <param name="LS_Code">节点编号</param>
        /// <returns></returns>
        public string DeleteLevelSet(string LS_Code)
        {
            string sql0 = string.Format("SELECT COUNT(LS_Code) AS Num FROM XXSD_levelSet WHERE LS_PCode='{0}'", LS_Code);
            DataTable dt = SearchData(sql0);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["Num"].ToString()) == 0)
                {
                    string sql2 = string.Format("SELECT Pub_Code FROM XXSD_PublicInfo WHERE Pub_LS_Code1='{0}' OR Pub_LS_Code2='{1}' OR Pub_LS_Code3='{2}' OR Pub_LS_Code4='{3}'", LS_Code, LS_Code, LS_Code, LS_Code);
                    //string sql3 = string.Format("SELECT CD_Code FROM XXSD_CompanyDisplay WHERE CD_LS_Code1='{0}' OR CD_LS_Code2='{1}' OR CD_LS_Code3='{2}' OR CD_LS_Code4='{3}'", LS_Code, LS_Code, LS_Code, LS_Code);
                    //string sql4 = string.Format("SELECT RP_Code FROM XXSD_RightsProtection WHERE RP_LS_Code1='{0}' OR RP_LS_Code2='{1}' OR RP_LS_Code3='{2}'", LS_Code, LS_Code, LS_Code);
                    DataTable dt1 = SearchData(sql2);
                    //DataTable dt2 = SearchData(sql3);
                    //DataTable dt3 = SearchData(sql4);
                    if (dt1.Rows.Count > 0)
                    {
                        return "3";//节点下添加有信息或内容，不能删除
                    }
                    else
                    {
                        string sql1 = string.Format("DELETE XXSD_levelSet WHERE LS_Code = '{0}'", LS_Code);
                        bool ResultStr = UpdateData(sql1);
                        if (ResultStr == true)
                            return "0";//删除成功
                        else
                            return "1";//删除失败
                    }
                }
                else
                {
                    return "2";//存在子节点，不能删除
                }
            }
            else
            {
                return "error";//出现错误
            }
        }
        #endregion
    }
}
