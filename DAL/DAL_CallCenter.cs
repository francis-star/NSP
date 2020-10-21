/////////////////////////////////////////////////////////////////////////////
//模块名：营销管理中心
//开发者：田维华
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCWeb2016;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Model;

namespace DAL
{
    public class DAL_CallCenter : SqlBase
    {
        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageNum">每页显示数据量</param>
        /// <returns>返回营销管理中心数据</returns>
        public DataTable GetCallCenterData(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT TOP {0} Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,Cust_Area,Cust_Address,Cust_ReturnContent,CASE when Cust_State='退回' then Cust_ReturnBackMan else '' end Cust_ReturnBackMan FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sql.Append("SELECT Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,(Cust_ProvinceName+Cust_CityName+Cust_CountyName) AS 'Cust_Area',Cust_Address,Cust_ReturnContent,ROW_NUMBER() OVER (ORDER BY JoinDate DESC) AS 'Num',Cust_ReturnBackMan FROM CXT_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            if (ValueHandler.GetStringValue(Cust_Linkman) != "")
                sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
                sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                int hasBack = 0;
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                    if (Cust_States[i] == "退回")
                        hasBack = 1;
                }
                sql.AppendFormat(" AND ({0} {1})", Cust_StateStr.ToString(), hasBack == 1 ? " OR Cust_State ='异常退回' " : "");
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
            sql.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1}) order by Num  ", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sql.ToString());
        }
        #endregion

        #region 获取数据量
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns>数据量</returns>
        public string GetDataCount(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT COUNT(Cust_Code) AS 'DataCount' FROM CXT_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            if (ValueHandler.GetStringValue(Cust_Linkman) != "")
                sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
                sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                int hasBack = 0;
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0} {1})", Cust_StateStr.ToString(), hasBack == 1 ? " OR Cust_State ='异常退回' " : "");
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
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

        #region 删除营销管理中心数据
        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code">编码</param>
        /// <returns>true:删除成功,-1:非待审、退回数据,-2:未查询到数据，删除失败</returns>
        public string DeleteCallCenterData(string Cust_Code)
        {
            string sql1 = string.Format("SELECT Cust_State FROM CXT_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            DataTable dt = SearchData(sql1);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Cust_State"].ToString() == "待审" || dt.Rows[0]["Cust_State"].ToString() == "退回")
                {
                    string sql = string.Format("DELETE FROM CXT_Customer WHERE Cust_Code='{0}'", ValueHandler.GetStringValue(Cust_Code));
                    return UpdateData(sql).ToString().ToLower();
                }
                else
                {
                    return "-1";
                }
            }
            else
            {
                return "-2";
            }

        }
        #endregion

        #region 民企云

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageNum">每页显示数据量</param>
        /// <returns>返回营销管理中心数据</returns>
        public DataTable GetMQYCallCenterData(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT TOP {0} Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,Cust_Area,Cust_Address,Cust_ReturnContent,CASE when Cust_State='退回' then Cust_ReturnBackMan else '' end Cust_ReturnBackMan FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sql.Append("SELECT Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,(Cust_ProvinceName+Cust_CityName+Cust_CountyName) AS 'Cust_Area',Cust_Address,Cust_ReturnContent,ROW_NUMBER() OVER (ORDER BY JoinDate DESC) AS 'Num',Cust_ReturnBackMan FROM MQY_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            if (ValueHandler.GetStringValue(Cust_Linkman) != "")
                sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
                sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
            sql.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1}) order by Num  ", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sql.ToString());
        }
        #endregion

        #region 获取数据量
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns>数据量</returns>
        public string GetMQYDataCount(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT COUNT(Cust_Code) AS 'DataCount' FROM MQY_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            if (ValueHandler.GetStringValue(Cust_Linkman) != "")
                sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
                sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
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

        #region 删除营销管理中心数据
        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code">编码</param>
        /// <returns>true:删除成功,-1:非待审、退回数据,-2:未查询到数据，删除失败</returns>
        public string DeleteMQYCallCenterData(string Cust_Code)
        {
            string sql1 = string.Format("SELECT Cust_State FROM MQY_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            DataTable dt = SearchData(sql1);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Cust_State"].ToString() == "待审" || dt.Rows[0]["Cust_State"].ToString() == "退回")
                {
                    string sql = string.Format("DELETE FROM MQY_Customer WHERE Cust_Code='{0}'", ValueHandler.GetStringValue(Cust_Code));
                    return UpdateData(sql).ToString().ToLower();
                }
                else
                {
                    return "-1";
                }
            }
            else
            {
                return "-2";
            }

        }
        #endregion

        #endregion

        #region 维权通

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageNum">每页显示数据量</param>
        /// <returns>返回营销管理中心数据</returns>
        public DataTable GetWQTCallCenterData(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT TOP {0} Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,Cust_Area,Cust_Address,Cust_ReturnContent,CASE when Cust_State='退回' then Cust_ReturnBackMan else '' end Cust_ReturnBackMan FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sql.Append("SELECT Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,(Cust_ProvinceName+Cust_CityName+Cust_CountyName) AS 'Cust_Area',Cust_Address,Cust_ReturnContent,ROW_NUMBER() OVER (ORDER BY JoinDate DESC) AS 'Num',Cust_ReturnBackMan FROM WQT_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            if (ValueHandler.GetStringValue(Cust_Linkman) != "")
                sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
                sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
            sql.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1}) order by Num  ", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sql.ToString());
        }
        #endregion

        #region 获取数据量
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns>数据量</returns>
        public string GetWQTDataCount(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT COUNT(Cust_Code) AS 'DataCount' FROM WQT_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            if (ValueHandler.GetStringValue(Cust_Linkman) != "")
                sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
                sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
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

        #region 删除营销管理中心数据
        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code">编码</param>
        /// <returns>true:删除成功,-1:非待审、退回数据,-2:未查询到数据，删除失败</returns>
        public string DeleteWQTCallCenterData(string Cust_Code)
        {
            string sql1 = string.Format("SELECT Cust_State FROM WQT_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            DataTable dt = SearchData(sql1);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Cust_State"].ToString() == "待审" || dt.Rows[0]["Cust_State"].ToString() == "退回")
                {
                    string sql = string.Format("DELETE FROM WQT_Customer WHERE Cust_Code='{0}'", ValueHandler.GetStringValue(Cust_Code));
                    return UpdateData(sql).ToString().ToLower();
                }
                else
                {
                    return "-1";
                }
            }
            else
            {
                return "-2";
            }

        }
        #endregion

        #endregion

        #region 新消费宝典

        #region 获取营销管理中心数据

        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageNum">每页显示数据量</param>
        /// <returns>返回营销管理中心数据</returns>
        public DataTable GetXFBCallCenterData(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT TOP {0} Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,Cust_Area,Cust_Address,Cust_ReturnContent,CASE when Cust_State='退回' then Cust_ReturnBackMan else '' end Cust_ReturnBackMan  FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sql.Append("SELECT Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,(Cust_ProvinceName+Cust_CityName) AS 'Cust_Area',Cust_Address,Cust_ReturnBackMan,Cust_ReturnContent,ROW_NUMBER() OVER (ORDER BY JoinDate DESC) AS 'Num' FROM XFB_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            //if (ValueHandler.GetStringValue(Cust_Linkman) != "")
            //    sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            //if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
            //    sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
            sql.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1}) order by Num  ", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sql.ToString());
        }

        #endregion

        #region 获取数据量

        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns>数据量</returns>
        public string GetXFBDataCount(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT COUNT(Cust_Code) AS 'DataCount' FROM XFB_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            //if (ValueHandler.GetStringValue(Cust_Linkman) != "")
            //    sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            //if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
            //    sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
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

        #region 删除营销管理中心数据

        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code">编码</param>
        /// <returns>true:删除成功,-1:非待审、退回数据,-2:未查询到数据，删除失败</returns>
        public string DeleteXFBCallCenterData(string Cust_Code)
        {
            string sql1 = string.Format("SELECT Cust_State FROM XFB_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            DataTable dt = SearchData(sql1);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Cust_State"].ToString() == "待审" || dt.Rows[0]["Cust_State"].ToString() == "退回")
                {
                    string sql = string.Format("DELETE FROM XFB_Customer WHERE Cust_Code='{0}'", ValueHandler.GetStringValue(Cust_Code));
                    return UpdateData(sql).ToString().ToLower();
                }
                else
                {
                    return "-1";
                }
            }
            else
            {
                return "-2";
            }
        }

        #endregion

        #endregion

        #region 实时保

        #region 获取营销管理中心数据

        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageNum">每页显示数据量</param>
        /// <returns>返回营销管理中心数据</returns>
        public DataTable GetSSBCallCenterData(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT TOP {0} Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,Cust_Area,Cust_Address,Cust_ReturnContent,CASE when Cust_State='退回' then Cust_ReturnBackMan else '' end Cust_ReturnBackMan FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sql.Append("SELECT Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_State,Cust_OpenDate,(Cust_ProvinceName+Cust_CityName+Cust_CountyName) AS 'Cust_Area',Cust_Address,Cust_ReturnContent,ROW_NUMBER() OVER (ORDER BY JoinDate DESC) AS 'Num',Cust_ReturnBackMan FROM SSB_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            if (ValueHandler.GetStringValue(Cust_Linkman) != "")
                sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
                sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
            sql.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1}) order by Num  ", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sql.ToString());
        }

        #endregion

        #region 获取数据量

        /// <summary>
        ///  获取列表总量
        /// </summary>
        /// <param name="Cust_Name">名称</param>
        /// <param name="Cust_Phone">电话</param>
        /// <param name="Cust_Linkman">联系人</param>
        /// <param name="Cust_LinkPhone">联系电话</param>
        /// <param name="Cust_BillNumber">计费号码</param>
        /// <param name="Cust_State">客户状态 待呼，待审，已审，退订，退费、删除，退回</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns>数据量</returns>
        public string GetSSBDataCount(string Cust_Name, string Cust_Phone, string Cust_Linkman, string Cust_LinkPhone, string Cust_BillNumber, string Cust_State, string BeginTime, string EndTime, string PageIndex, string PageNum, string UserName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT COUNT(Cust_Code) AS 'DataCount' FROM SSB_Customer WHERE 1=1");
            if (ValueHandler.GetStringValue(Cust_Name) != "")
                sql.AppendFormat(" AND Cust_Name LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Name));
            if (ValueHandler.GetStringValue(Cust_Phone) != "")
                sql.AppendFormat(" AND Cust_Phone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Phone));
            if (ValueHandler.GetStringValue(Cust_Linkman) != "")
                sql.AppendFormat(" AND Cust_Linkman LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_Linkman));
            if (ValueHandler.GetStringValue(Cust_LinkPhone) != "")
                sql.AppendFormat(" AND Cust_LinkPhone LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_LinkPhone));
            if (ValueHandler.GetStringValue(Cust_BillNumber) != "")
                sql.AppendFormat(" AND Cust_BillNumber LIKE '%{0}%'", ValueHandler.GetStringValue(Cust_BillNumber));
            if (ValueHandler.GetStringValue(Cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(Cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sql.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND Cust_OpenDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            if (ValueHandler.GetStringValue(UserName) != "")
                sql.AppendFormat(" AND Cust_WH_UserName = '{0}'", ValueHandler.GetStringValue(UserName));
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

        #region 删除营销管理中心数据

        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code">编码</param>
        /// <returns>true:删除成功,-1:非待审、退回数据,-2:未查询到数据，删除失败</returns>
        public string DeleteSSBCallCenterData(string Cust_Code)
        {
            string sql1 = string.Format("SELECT Cust_State FROM SSB_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            DataTable dt = SearchData(sql1);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Cust_State"].ToString() == "待审" || dt.Rows[0]["Cust_State"].ToString() == "退回")
                {
                    string sql = string.Format("DELETE FROM SSB_Customer WHERE Cust_Code='{0}'", ValueHandler.GetStringValue(Cust_Code));
                    return UpdateData(sql).ToString().ToLower();
                }
                else
                {
                    return "-1";
                }
            }
            else
            {
                return "-2";
            }
        }

        #endregion

        #region 计费处理

        #region 导入计费

        public string PlImportChargeData(string remark, string orignName, string userCode, List<BatchChargeData> list)
        {
            string flag;
            SqlTransaction tran = null;//声明一个事务对象  
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnStringSQL"].ToString()))
                {
                    conn.Open();//打开链接  
                    using (tran = conn.BeginTransaction())
                    {
                        string OD_Code = GetCode();
                        //先插入主表信息
                        SqlCommand cmd = new SqlCommand($@"INSERT INTO dbo.KF_ChargeData
	                                            (
	                                            CD_Code,
	                                            CD_FileName,
	                                            CD_Remark, 
	                                            JoinMan, 
	                                            UpdateMan 
	                                            )
                                            VALUES 
	                                            (
	                                            '{OD_Code}',
	                                            '{orignName}',
	                                            '{remark}', 
	                                            '{userCode}',
	                                            '{userCode}'
	                                            )", conn);
                        cmd.Transaction = tran;
                        cmd.ExecuteNonQuery();

                        DataTable table = new DataTable();
                        //为数据表创建相对应的数据列
                        table.Columns.Add("CDD_Name");
                        table.Columns.Add("CDD_Phone");
                        table.Columns.Add("CDD_ActiveDate");
                        table.Columns.Add("CDD_CD_Code");
                        table.Columns.Add("JoinMan");
                        table.Columns.Add("ODD_Code");

                        foreach (var data in list)
                        {
                            DataRow dr = table.NewRow();//创建数据行
                            dr["CDD_Name"] = data.custName.Length > 50 ? data.custName.Substring(0, 50) : data.custName;
                            dr["CDD_Phone"] = data.billNo;
                            dr["CDD_ActiveDate"] = Convert.ToDateTime(data.activeDate).ToShortDateString();
                            dr["CDD_CD_Code"] = OD_Code;
                            dr["JoinMan"] = userCode;

                            //将创建的数据行添加到table中
                            table.Rows.Add(dr);
                        }

                        using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                        {
                            copy.DestinationTableName = "KF_ChargeDataDts";  //指定服务器上目标表的名称  
                                                                             //设置数据表table和数据库中表的列对应关系
                            copy.ColumnMappings.Add("CDD_Name", "CDD_Name");
                            copy.ColumnMappings.Add("CDD_Phone", "CDD_Phone");
                            copy.ColumnMappings.Add("CDD_ActiveDate", "CDD_ActiveDate");
                            copy.ColumnMappings.Add("CDD_CD_Code", "CDD_CD_Code");
                            copy.ColumnMappings.Add("JoinMan", "JoinMan");
                            copy.WriteToServer(table);           //执行把DataTable中的数据写入DB  

                            #region 开始计费

                            foreach (var data in list)
                            {
                                cmd.CommandText = $@"INSERT INTO dbo.YX_SSBBillList
	                                        (
	                                        BL_Code,
	                                        BL_Cust_BillNumber,
	                                        BL_Date, 
	                                        JoinMan
	                                        )  
                                        SELECT c.Cust_Code
                                            , c.Cust_BillNumber
                                            , dt
                                            , '{userCode}'
                                        FROM SSB_Customer c
                                            LEFT JOIN (SELECT dateadd (day, number, '{data.activeDate}') AS dt
                                                       FROM master.dbo.spt_values
                                                       WHERE type = 'P' AND number <= DATEDIFF (day, '{data.activeDate}', getdate ())) a ON 1 = 1
                                            LEFT JOIN YX_SSBBillList x ON x.BL_Cust_BillNumber = c.Cust_BillNumber AND x.BL_Date = dt AND X.DataState = 0
                                        WHERE c.DataState = 0 AND c.Cust_BillNumber ='{data.billNo}' AND c.Cust_State = '已审' AND X.ID is null";
                                cmd.Transaction = tran;
                                cmd.ExecuteNonQuery();
                            }

                            #endregion

                            tran.Commit(); //提交事务  
                            flag = "true";
                        }
                    }
                }
            }
            catch
            {
                tran.Rollback();  //出错回滚 
                flag = "系统异常，请联系管理人员！";
            }

            return flag;
        }

        #endregion

        /// <summary>
        /// 获取历史计费列表
        /// </summary>
        /// <param name="state"></param>
        /// <param name="operateUserCode"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetSSBDealChargeData(string state, string operateUserCode, string BeginTime, string EndTime, string PageIndex, string PageNum)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT TOP {0} * FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sql.Append(@"SELECT  c.*,a.User_Name UpdateUserName, CASE c.CD_State WHEN 1 THEN '已执行' ELSE '已撤销' END StateName,ROW_NUMBER() OVER (ORDER BY c.JoinDate DESC) AS 'Num'
                            FROM dbo.KF_ChargeData c
                            JOIN AF_User a ON c.JoinMan = a.User_Code
                            WHERE c.DataState = 0");
            if (ValueHandler.GetStringValue(state) != "")
                sql.AppendFormat(" AND CD_State = {0}", ValueHandler.GetStringValue(state) == "已执行" ? "1" : "2");
            if (ValueHandler.GetStringValue(operateUserCode) != "")
            {
                string[] opMans = ValueHandler.GetStringValue(operateUserCode).Split(';');
                StringBuilder opMansStr = new StringBuilder();
                for (int i = 0; i < opMans.Length; i++)
                {
                    if (i == 0)
                        opMansStr.AppendFormat(" c.JoinMan = '{0}'", opMans[i]);
                    else
                        opMansStr.AppendFormat(" OR c.JoinMan = '{0}'", opMans[i]);
                }
                sql.AppendFormat(" AND ({0})", opMansStr.ToString());
            }

            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND c.JoinDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND c.JoinDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
            sql.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1}) order by Num  ", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sql.ToString());
        }

        /// <summary>
        /// 获取历史计费总数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="operateUserCode"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public string GetSSBDealChargeDataCount(string state, string operateUserCode, string BeginTime, string EndTime, string PageIndex, string PageNum)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT COUNT(CD_Code) AS 'DataCount' FROM KF_ChargeData WHERE DataState = 0");
            if (ValueHandler.GetStringValue(state) != "")
                sql.AppendFormat(" AND CD_State = {0}", ValueHandler.GetStringValue(state) == "已执行" ? "1" : "2");
            if (ValueHandler.GetStringValue(operateUserCode) != "")
            {
                string[] opMans = ValueHandler.GetStringValue(operateUserCode).Split(';');
                StringBuilder opMansStr = new StringBuilder();
                for (int i = 0; i < opMans.Length; i++)
                {
                    if (i == 0)
                        opMansStr.AppendFormat(" JoinMan = '{0}'", opMans[i]);
                    else
                        opMansStr.AppendFormat(" OR JoinMan = '{0}'", opMans[i]);
                }
                sql.AppendFormat(" AND ({0})", opMansStr.ToString());
            }
            if (ValueHandler.GetMarkStringDateValue(BeginTime) != "" && ValueHandler.GetMarkStringDateValue(BeginTime) != "null" && ValueHandler.GetMarkStringDateValue(BeginTime) != null)
                sql.AppendFormat(" AND JoinDate >= '{0}'", ValueHandler.GetStringValue(BeginTime));
            if (ValueHandler.GetMarkStringDateValue(EndTime) != "" && ValueHandler.GetMarkStringDateValue(EndTime) != "null" && ValueHandler.GetMarkStringDateValue(EndTime) != null)
                sql.AppendFormat(" AND JoinDate <= '{0}'", ValueHandler.GetStringValue(EndTime));
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

        public string CancelSSBChargeData(string odCode, string JoinMan)
        {
            string returnStr;
            try
            {
                if (hasExistChargeData(odCode, 1))
                {
                    returnStr = "false|当前任务不存在或已撤销！";
                }
                else if (hasExpireDate(odCode))
                {
                    returnStr = "false|仅能够撤销今天的相关操作！";
                }
                else
                {
                    bool flag = false;
                    int result;
                    flag = new SqlBase().ExcuteNonQuery_Sp("SP_CancelSSBChargeData", new SqlParameter[] {
                        new SqlParameter("@CDCode",odCode),
                        new SqlParameter("@JoinMan",JoinMan)
                          }, out result);
                    returnStr = flag ? "true|撤销成功！" : "false|提交失败，请联系管理员！";
                }
            }
            catch
            {
                returnStr = "false|提交失败，请联系管理员！";
            }

            return returnStr;
        }

        public bool isStateCustomer(string billno)
        {
            DataTable dt = SearchData($"Select 1 from SSB_Customer c WHERE DataState= 0 AND c.Cust_State='已审' AND c.Cust_BillNumber = '{billno}'");

            return dt == null || dt.Rows.Count == 0;
        }

        public bool isBillStateCustomer(string billno)
        {
            DataTable dt = SearchData($@" SELECT 
                                  1
                                FROM dbo.YX_SSBBillList
                                WHERE DataState = 0
                                AND BL_Cust_BillNumber = '{billno}'
                                AND BL_Date = '{DateTime.Now.ToShortDateString()}'");

            return dt != null && dt.Rows.Count > 0;
        }

        /// <summary>
        /// 删除计费批次数据
        /// </summary>
        /// <param name="odCode"></param>
        /// <returns></returns>
        public string DeleteSSBChargeData(string odCode)
        {
            string returnStr;
            try
            {
                if (hasExistChargeData(odCode, 2))
                {
                    returnStr = "false|当前任务不存在！";
                }
                else
                {
                    bool flag = UpdateData($"Update KF_ChargeData SET dataState = 1 WHERE CD_Code='{odCode}' AND DataState= 0 AND CD_State = 2");
                    returnStr = flag ? "true|已删除！" : "false|提交失败，请联系管理员！";
                }
            }
            catch
            {
                returnStr = "false|提交失败，请联系管理员！";
            }

            return returnStr;
        }

        /// <summary>
        /// 判断计费批次状态是否可操作
        /// </summary>
        /// <param name="odCode">批次编号</param>
        /// <param name="state">1 执行 2 撤销</param>
        /// <returns></returns>
        public bool hasExistChargeData(string odCode, int state)
        {
            DataTable dt = SearchData($"Select * from KF_ChargeData where CD_Code='{odCode}' And DataState= 0 And CD_State={state}");
            return dt == null || dt.Rows.Count == 0;
        }

        /// <summary>
        /// 判断是否导入7天了
        /// </summary>
        /// <param name="odCode"></param>
        /// <returns></returns>
        public bool hasExpireDate(string odCode)
        {
            DataTable dt = SearchData($"Select * from KF_ChargeData where CD_Code='{odCode}' And DataState= 0 And CD_State=1 AND DateDiff(dd,joindate,getdate())=0");
            return dt == null || dt.Rows.Count == 0;
        }

        /// <summary>
        /// 查看批次明细
        /// </summary>
        /// <param name="odCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetSSB_ViewChargeData(string odCode, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT *
                                        , ROW_NUMBER () OVER (ORDER BY JoinDate ASC) AS 'Num'
                                    FROM KF_ChargeDataDts
                                    WHERE CDD_CD_Code = '{1}'
                                      AND DataState = 0", PageNum, odCode);
            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取批次明细总数
        /// </summary>
        /// <param name="odCode"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public string GetSSB_ViewChargeDataCount(string odCode, string PageIndex, string PageNum)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($"SELECT COUNT(ID) AS 'DataCount' FROM KF_ChargeDataDts WHERE CDD_CD_Code = '{odCode}'");
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

        #endregion
    }
}
