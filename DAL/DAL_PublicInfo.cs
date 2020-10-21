/////////////////////////////////////////////////////////////////////////////
//模块名：资讯信息维护
//开发者：田维华
//开发时间：2016年11月7日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCWeb2016;
using System.Data;

namespace DAL
{
    public class DAL_PublicInfo : SqlBase
    {
        #region 移动端、平台名称、信息类别、信息大类 四级联动
        /// <summary>
        /// 移动端、平台名称、信息类别、信息大类 四级联动
        /// </summary>
        /// <param name="LS_Code">级别编号</param>
        /// <param name="LS_Type">类型[仅第三级节点设置时使用]</param>
        /// <param name="LS_User_Code">当前登录用户编号</param>
        /// <returns>一级菜单或下一级菜单表，若为空，则没有菜单</returns>
        public DataTable GetSearchStrData(string LS_Code, string LS_Type, string LS_User_Code)
        {
            StringBuilder sql = new StringBuilder();
            if (ValueHandler.GetStringValue(LS_Code) != "")
            {
                sql.AppendFormat("SELECT LS_Code,LS_Name FROM dbo.XXSD_levelSet WHERE LS_PCode='{0}' AND DataState=0", ValueHandler.GetStringValue(LS_Code));
                if (ValueHandler.GetStringValue(LS_Type) != "")
                {
                    sql.AppendFormat(" AND LS_Type = '{0}'", LS_Type);
                }
                if (ValueHandler.GetStringValue(LS_User_Code) != "")
                {
                    sql.AppendFormat(" AND LS_User_Code LIKE '%{0}%'", LS_User_Code);
                }
                sql.Append(" ORDER BY JoinDate DESC ");
            }
            else
            {
                if (ValueHandler.GetStringValue(LS_Type) == "")
                {

                    sql.Append("SELECT LS_Code,LS_Name FROM dbo.XXSD_levelSet WHERE LS_PCode IS NULL AND LS_Type IS NULL AND DataState=0 ORDER BY JoinDate DESC");
                }
            }
            return SearchData(sql.ToString());
        }
        #endregion

        public DataTable GetList()
        {
            string sql = string.Format("SELECT * from XXSD_PublicInfo");
            return SearchData(sql);
        }

        public bool UpdateContent(string code, string content)
        {
            string sql = string.Format("update XXSD_PublicInfo set Pub_Content='{0}' where Pub_Code='{1}'", content, code);
            return UpdateData(sql);
        }

        #region 查询省、市、区
        /// <summary>
        /// 查询省、市、区
        /// </summary>
        /// <param name="SA_Kind">类型</param>
        /// <param name="SA_PCode">父级编号</param>
        /// <returns></returns>
        public DataTable GetArea(string SA_Kind, string SA_PCode)
        {
            string sql = string.Format("SELECT SA_Code,SA_Name FROM SYS_Area WHERE SA_Kind='{0}' AND SA_PCode='{1}' AND DataState=0 ORDER BY SA_Index ASC", ValueHandler.GetStringValue(SA_Kind), ValueHandler.GetStringValue(SA_PCode));
            return SearchData(sql);
        }
        #endregion

        #region 获取资讯信息
        /// <summary>
        /// 获取资讯信息
        /// </summary>
        /// <param name="Pub_LS_Code1">移动端Code</param>
        /// <param name="Pub_LS_Code2">平台名称Code</param>
        /// <param name="Pub_LS_Code3">信息大类Code</param>
        /// <param name="Pub_Title">标题</param>
        /// <param name="Pub_Code">Code</param>
        /// <param name="Pub_LS_Code4">4级节点（信息小类）</param>
        /// <param name="Pub_SA_Code1">省</param>
        /// <param name="Pub_SA_Code2">市</param>
        /// <param name="Pub_SA_Code3">区</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageNum">每页显示数据量</param>
        /// <returns>资讯信息DataTable</returns>
        public DataTable GetPublicInfo(string Pub_LS_Code1, string Pub_LS_Code2, string Pub_LS_Code3, string Pub_Title, string Pub_Code, string Pub_LS_Code4, string Pub_SA_Code1, string Pub_SA_Code2, string Pub_SA_Code3, string BeginTime, string EndTime, string PageIndex, string PageNum)
        {
            StringBuilder sql = new StringBuilder();
            if (ValueHandler.GetStringValue(Pub_Code) != "")
            {
                sql.AppendFormat("SELECT Pub_Code,Pub_LS_Code1,Pub_LS_Code2,Pub_LS_Code3,Pub_LS_Code4,Pub_Title,Pub_ArticleSource,Pub_Pic,Pub_DisplayMode,Pub_Content  FROM dbo.XXSD_PublicInfo WHERE Pub_Code='{0}'", Pub_Code);
            }
            else
            {
                sql.AppendFormat("SELECT TOP {0} Pub_Code,Pub_Title,LS_Name1,LS_Name2,Pub_KeyWords,JoinDate,JoinMan FROM(", ValueHandler.GetIntNumberValue(PageNum));
                sql.Append("SELECT P.Pub_Code,P.Pub_Title,L1.LS_Name AS 'LS_Name1',L2.LS_Name AS 'LS_Name2',P.Pub_KeyWords,P.JoinDate,P.JoinMan,ROW_NUMBER() OVER (ORDER BY P.JoinDate DESC) AS 'Num' FROM XXSD_PublicInfo P LEFT JOIN XXSD_LevelSet L1 ON P.Pub_LS_Code3=L1.LS_Code LEFT JOIN XXSD_LevelSet L2 ON P.Pub_LS_Code4=L2.LS_Code WHERE 1=1");
                if (ValueHandler.GetStringValue(Pub_LS_Code1) != "")
                    sql.AppendFormat(" AND P.Pub_LS_Code1 = '{0}'", ValueHandler.GetStringValue(Pub_LS_Code1));
                if (ValueHandler.GetStringValue(Pub_LS_Code2) != "")
                    sql.AppendFormat(" AND P.Pub_LS_Code2 = '{0}'", ValueHandler.GetStringValue(Pub_LS_Code2));
                if (ValueHandler.GetStringValue(Pub_LS_Code3) != "")
                    sql.AppendFormat(" AND P.Pub_LS_Code3 = '{0}'", ValueHandler.GetStringValue(Pub_LS_Code3));
                if (ValueHandler.GetStringValue(Pub_LS_Code4) != "")
                    sql.AppendFormat(" AND P.Pub_LS_Code4 = '{0}'", ValueHandler.GetStringValue(Pub_LS_Code4));
                if (ValueHandler.GetStringValue(Pub_SA_Code1) != "")
                    sql.AppendFormat(" AND P.Pub_SA_Code1 = '{0}'", ValueHandler.GetStringValue(Pub_SA_Code1));
                if (ValueHandler.GetStringValue(Pub_SA_Code2) != "")
                    sql.AppendFormat(" AND P.Pub_SA_Code2 = '{0}'", ValueHandler.GetStringValue(Pub_SA_Code2));
                if (ValueHandler.GetStringValue(Pub_SA_Code3) != "")
                    sql.AppendFormat(" AND P.Pub_SA_Code3 = '{0}'", ValueHandler.GetStringValue(Pub_SA_Code3));
                if (ValueHandler.GetStringValue(Pub_Title) != "")
                    sql.AppendFormat(" AND P.Pub_Title LIKE '%{0}%'", ValueHandler.GetStringValue(Pub_Title));
                if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                    sql.AppendFormat(" AND P.JoinDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
                if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                    sql.AppendFormat(" AND P.JoinDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
                sql.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1})", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            }
            return SearchData(sql.ToString());
        }
        #endregion

        #region  获取数据量
        /// <summary>
        /// 获取资讯信息数量
        /// </summary>
        /// <param name="Pub_LS_Code1">移动端Code</param>
        /// <param name="Pub_LS_Code2">平台名称Code</param>
        /// <param name="Pub_LS_Code3">信息大类Code</param>
        /// <param name="Pub_Title">标题</param>
        /// <param name="Pub_Code">Code</param>
        /// <param name="Pub_LS_Code4">4级节点（信息小类）</param>
        /// <param name="Pub_SA_Code1">省</param>
        /// <param name="Pub_SA_Code2">市</param>
        /// <param name="Pub_SA_Code3">区</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageNum">每页显示数据量</param>
        /// <returns>数据量</returns>
        public string GetDataCount(string Pub_LS_Code1, string Pub_LS_Code2, string Pub_LS_Code3, string Pub_Title, string Pub_Code, string Pub_LS_Code4, string Pub_SA_Code1, string Pub_SA_Code2, string Pub_SA_Code3, string BeginTime, string EndTime, string PageIndex, string PageNum)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT COUNT(Pub_Code) AS 'DataCount' FROM XXSD_PublicInfo P LEFT JOIN XXSD_LevelSet L1 ON P.Pub_LS_Code3=L1.LS_Code LEFT JOIN XXSD_LevelSet L2 ON P.Pub_LS_Code4=L2.LS_Code WHERE 1=1");
            if (ValueHandler.GetStringValue(Pub_LS_Code1) != "")
                sql.AppendFormat(" AND P.Pub_LS_Code1 = '{0}'", ValueHandler.GetStringValue(Pub_LS_Code1));
            if (ValueHandler.GetStringValue(Pub_LS_Code2) != "")
                sql.AppendFormat(" AND P.Pub_LS_Code2 = '{0}'", ValueHandler.GetStringValue(Pub_LS_Code2));
            if (ValueHandler.GetStringValue(Pub_LS_Code3) != "")
                sql.AppendFormat(" AND P.Pub_LS_Code3 = '{0}'", ValueHandler.GetStringValue(Pub_LS_Code3));
            if (ValueHandler.GetStringValue(Pub_LS_Code4) != "")
                sql.AppendFormat(" AND P.Pub_LS_Code4 = '{0}'", ValueHandler.GetStringValue(Pub_LS_Code4));
            if (ValueHandler.GetStringValue(Pub_SA_Code1) != "")
                sql.AppendFormat(" AND P.Pub_SA_Code1 = '{0}'", ValueHandler.GetStringValue(Pub_SA_Code1));
            if (ValueHandler.GetStringValue(Pub_SA_Code2) != "")
                sql.AppendFormat(" AND P.Pub_SA_Code2 = '{0}'", ValueHandler.GetStringValue(Pub_SA_Code2));
            if (ValueHandler.GetStringValue(Pub_SA_Code3) != "")
                sql.AppendFormat(" AND P.Pub_SA_Code3 = '{0}'", ValueHandler.GetStringValue(Pub_SA_Code3));
            if (ValueHandler.GetStringValue(Pub_Title) != "")
                sql.AppendFormat(" AND P.Pub_Title LIKE '%{0}%'", ValueHandler.GetStringValue(Pub_Title));
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND P.JoinDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND P.JoinDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            DataTable dt = SearchData(sql.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }
        #endregion

        #region 删除资讯信息
        /// <summary>
        /// 删除资讯信息
        /// </summary>
        /// <param name="Pub_Code">Code</param>
        /// <returns>true:删除成功,false:删除失败</returns>
        public bool DeletePublicInfo(string Pub_Code)
        {
            string sql = string.Format("DELETE FROM XXSD_PublicInfo WHERE Pub_Code='{0}'", ValueHandler.GetStringValue(Pub_Code));
            return UpdateData(sql);
        }
        #endregion

        #region 查询外呼人员
        public DataTable GetWHUser()
        {
            string sql = string.Format("SELECT DISTINCT Cust_WH_UserName FROM CXT_Customer WHERE Cust_WH_UserName IS NOT NULL");
            return SearchData(sql);
        }
        #endregion
    }
}
