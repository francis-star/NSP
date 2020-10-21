using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_CustomerReturns : SqlBase
    {
        #region 诚信通

        /// <summary>
        /// 获取当前登录用户的工作地点
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public string GetUserPlace(string usercode)
        {
            DataTable dt = SearchData("SELECT User_Place FROM dbo.AF_User WHERE User_Code='" + usercode + "'");
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return "";
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_LinkPhone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public DataTable GetCust(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string PageIndex, string PageNum, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT TOP {0} *,Cust_Area FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sb.Append(@"
                        SELECT  w.* ,
                                ( Cust_ProvinceName + Cust_CityName + ISNULL(Cust_CountyName, '') ) AS 'Cust_Area' ,
                                u.User_Place ,
                                ROW_NUMBER() OVER ( ORDER BY w.JoinDate DESC ) AS 'Num'
                        FROM    CXT_Customer w
                                LEFT JOIN AF_User u ON w.JoinMan = u.User_Name
                        WHERE   1 = 1 ");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(") T WHERE ");
            sb.AppendFormat(" T.Num >(0+({0}-1)*{1}) order by 'Num'", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 诚信通导出列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_LinkPhone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</pToExcelaram>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public string GetCustExcel(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                           SELECT  Cust_Name AS '客户名称' ,
                            Cust_Phone AS '座机号码' ,
                            Cust_BillNumber AS '计费号码' ,
                            Cust_IsBill AS '是否计费' ,
                            Cust_Linkman AS '联系人' ,
                            Cust_LinkPhone AS '联系电话' ,
                            Cust_WH_UserName AS '外呼人员' ,
		                    User_Place AS '工作地点',
                            Cust_State AS '客户状态' ,
                            Cust_KFVoice '录音编码',
                            Cust_OpenDate AS '开通时间' ,
                            Cust_UnOrder AS '退订时间' ,
                            Cust_DealPerson AS '退订人' ,
                            Cust_OutDate AS '退费时间' ,
                            Cust_ProvinceName AS '省' ,
                            Cust_CityName AS '市',
                            Cust_PassMan AS '审核人',
                            Cust_BelongProvinceName AS '计费号码归属地省',
                            Cust_BelongCityName AS '计费号码归属地市'
                    FROM    CXT_Customer w LEFT JOIN dbo.AF_User u ON w.Cust_WH_UserName = u.User_Name
                    WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");

            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(" ORDER BY w.JoinDate DESC ");
            return sb.ToString();
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="cust_Name"></param>
        /// <param name="cust_Linkman"></param>
        /// <param name="cust_LinkPhone"></param>
        /// <param name="address"></param>
        /// <param name="cust_State"></param>
        /// <param name="cust_BillNumber"></param>
        /// <param name="cust_IsBill"></param>
        /// <param name="cust_WH_UserName"></param>
        /// <param name="staJoinDate"></param>
        /// <param name="endJoinDate"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="endcust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="endcust_OutDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetNum(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                        SELECT  COUNT(*) AS num
                        FROM    ( SELECT    w.* ,
                                            u.User_Place ,
                                            ( Cust_ProvinceName + Cust_CityName + Cust_CountyName ) AS 'Cust_Area'
                                  FROM      CXT_Customer w
                                            LEFT JOIN dbo.AF_User u ON w.JoinMan = u.User_Name
                                ) AS Num
                        WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 诚信通客服异常退回
        /// </summary>
        /// <param name="custCode"></param>
        /// <returns></returns>
        public bool BackCXTCustomer(string custCode)
        {
            string sql = string.Format(@"UPDATE  CXT_Customer SET Cust_State='异常退回',Cust_ReturnDate=getdate(),Cust_OperateTime=getdate(),Cust_ReturnContent='计费号码有误,请修改计费号码'
                                    WHERE Cust_Code='{0}'", custCode);

            return UpdateData(sql);
        }

        #endregion

        #region 民企云

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_LinkPhone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public DataTable GetMQYCust(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string PageIndex, string PageNum, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT TOP {0} *,Cust_Area FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sb.Append(@"
                        SELECT  w.* ,
                                ( Cust_ProvinceName + Cust_CityName + ISNULL(Cust_CountyName, '') ) AS 'Cust_Area' ,
                                u.User_Place ,
                                ROW_NUMBER() OVER ( ORDER BY w.JoinDate DESC ) AS 'Num'
                        FROM    MQY_Customer w
                                LEFT JOIN AF_User u ON w.JoinMan = u.User_Name
                        WHERE   1 = 1 ");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(") T WHERE ");
            sb.AppendFormat(" T.Num >(0+({0}-1)*{1}) order by 'Num'", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 民企业导出列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_LinkPhone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public string GetMQYCustExcel(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                           SELECT  Cust_Name AS '客户名称' ,
                            Cust_Phone AS '座机号码' ,
                            Cust_BillNumber AS '计费号码' ,
                            Cust_IsBill AS '是否计费' ,
                            Cust_Linkman AS '联系人' ,
                            Cust_LinkPhone AS '联系电话' ,
                            Cust_WH_UserName AS '外呼人员' ,
		                    User_Place AS '工作地点',
                            Cust_State AS '客户状态',
                            Cust_KFVoice '录音编码',
                            Cust_OpenDate AS '开通时间' ,
                            Cust_UnOrder AS '退订时间' ,
                            Cust_DealPerson AS '退订人' ,
                            Cust_OutDate AS '退费时间' ,
                            Cust_ProvinceName AS '省' ,
                            Cust_CityName AS '市',
                            Cust_PassMan AS '审核人',
                            Cust_BelongProvinceName AS '计费号码归属地省',
                            Cust_BelongCityName AS '计费号码归属地市'
                    FROM    MQY_Customer w LEFT JOIN dbo.AF_User u ON w.Cust_WH_UserName = u.User_Name
                    WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");

            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(" ORDER BY w.JoinDate DESC ");
            return sb.ToString();
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="cust_Name"></param>
        /// <param name="cust_Linkman"></param>
        /// <param name="cust_LinkPhone"></param>
        /// <param name="address"></param>
        /// <param name="cust_State"></param>
        /// <param name="cust_BillNumber"></param>
        /// <param name="cust_IsBill"></param>
        /// <param name="cust_WH_UserName"></param>
        /// <param name="staJoinDate"></param>
        /// <param name="endJoinDate"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="endcust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="endcust_OutDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetMQYNum(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                        SELECT  COUNT(*) AS num
                        FROM    ( SELECT    w.* ,
                                            u.User_Place ,
                                            ( Cust_ProvinceName + Cust_CityName + Cust_CountyName ) AS 'Cust_Area'
                                  FROM      MQY_Customer w
                                            LEFT JOIN dbo.AF_User u ON w.JoinMan = u.User_Name
                                ) AS Num
                        WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 名企云客服异常退回
        /// </summary>
        /// <param name="custCode"></param>
        /// <returns></returns>
        public bool BackMQYCustomer(string custCode)
        {
            string sql = string.Format(@"UPDATE MQY_Customer SET Cust_State='异常退回',Cust_ReturnDate=getdate(),Cust_OperateTime=getdate(),Cust_ReturnContent='计费号码有误,请修改计费号码'
                                    WHERE Cust_Code='{0}'", custCode);

            return UpdateData(sql);
        }

        #endregion

        #region 维权通

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_LinkPhone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public DataTable GetWQTCust(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string PageIndex, string PageNum, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT TOP {0} *,Cust_Area FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sb.Append(@"
                        SELECT  w.* ,
                                ( Cust_ProvinceName + Cust_CityName + ISNULL(Cust_CountyName, '') ) AS 'Cust_Area' ,
                                u.User_Place ,
                                ROW_NUMBER() OVER ( ORDER BY w.JoinDate DESC ) AS 'Num'
                        FROM    WQT_Customer w
                                LEFT JOIN AF_User u ON w.JoinMan = u.User_Name
                        WHERE   1 = 1 ");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(") T WHERE ");
            sb.AppendFormat(" T.Num >(0+({0}-1)*{1}) order by 'Num'", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 维权通导出列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_LinkPhone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public string GetWQTCustExcel(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                           SELECT  Cust_Name AS '客户名称' ,
                            Cust_Phone AS '座机号码' ,
                            Cust_BillNumber AS '计费号码' ,
                            Cust_IsBill AS '是否计费' ,
                            Cust_Linkman AS '联系人' ,
                            Cust_LinkPhone AS '联系电话' ,
                            Cust_WH_UserName AS '外呼人员' ,
		                    User_Place AS '工作地点',
                            Cust_State AS '客户状态' ,
                            Cust_KFVoice '录音编码',
                            Cust_OpenDate AS '开通时间' ,
                            Cust_UnOrder AS '退订时间' ,
                            Cust_DealPerson AS '退订人' ,
                            Cust_OutDate AS '退费时间' ,
                            Cust_ProvinceName AS '省' ,
                            Cust_CityName AS '市',
                            Cust_PassMan AS '审核人',
                            Cust_BelongProvinceName AS '计费号码归属地省',
                            Cust_BelongCityName AS '计费号码归属地市'
                    FROM    WQT_Customer w LEFT JOIN dbo.AF_User u ON w.Cust_WH_UserName = u.User_Name
                    WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");

            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(" ORDER BY w.JoinDate DESC ");
            return sb.ToString();
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="cust_Name"></param>
        /// <param name="cust_Linkman"></param>
        /// <param name="cust_LinkPhone"></param>
        /// <param name="address"></param>
        /// <param name="cust_State"></param>
        /// <param name="cust_BillNumber"></param>
        /// <param name="cust_IsBill"></param>
        /// <param name="cust_WH_UserName"></param>
        /// <param name="staJoinDate"></param>
        /// <param name="endJoinDate"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="endcust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="endcust_OutDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetWQTNum(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                        SELECT  COUNT(*) AS num
                        FROM    ( SELECT    w.* ,
                                            u.User_Place ,
                                            ( Cust_ProvinceName + Cust_CityName + Cust_CountyName ) AS 'Cust_Area'
                                  FROM      WQT_Customer w
                                            LEFT JOIN dbo.AF_User u ON w.JoinMan = u.User_Name
                                ) AS Num
                        WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 维权通客服异常退回
        /// </summary>
        /// <param name="custCode"></param>
        /// <returns></returns>
        public bool BackWQTCustomer(string custCode)
        {
            string sql = string.Format(@"UPDATE WQT_Customer SET Cust_State='异常退回',Cust_ReturnDate=getdate(),Cust_OperateTime=getdate(),Cust_ReturnContent='计费号码有误,请修改计费号码'
                                    WHERE Cust_Code='{0}'", custCode);

            return UpdateData(sql);
        }

        #endregion

        #region 新消费宝典

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_Phone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public DataTable GetXFBCust(string cust_Name, string cust_Linkman, string cust_Phone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string PageIndex, string PageNum, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT TOP {0} *,Cust_Area FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sb.Append(@"
                        SELECT  w.* ,
                                (Cust_ProvinceName + Cust_CityName) AS 'Cust_Area' ,
                                u.User_Place ,
                                ROW_NUMBER() OVER ( ORDER BY w.JoinDate DESC ) AS 'Num'
                        FROM    XFB_Customer w
                                LEFT JOIN AF_User u ON w.Cust_WH_UserName = u.User_Name
                        WHERE   1 = 1 ");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            //if (ValueHandler.GetStringValue(cust_Linkman) != "")
            //    sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_Phone) != "")
                sb.Append(" AND Cust_Phone LIKE '%" + ValueHandler.GetStringValue(cust_Phone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(") T WHERE ");
            sb.AppendFormat(" T.Num >(0+({0}-1)*{1}) order by 'Num'", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 维权通导出列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_Phone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public string GetXFBCustExcel(string cust_Name, string cust_Linkman, string cust_Phone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@" SELECT  Cust_Name AS '客户名称' ,
                            Cust_Phone AS '联系电话' ,
                            Cust_BillNumber AS '计费号码' ,
                            Cust_IsBill AS '是否计费' , 
                            Cust_WH_UserName AS '外呼人员' ,
		                    User_Place AS '工作地点',
                            Cust_State AS '客户状态' ,
                            Cust_KFVoice '录音编码',
                            Cust_OpenDate AS '开通时间' ,
                            Cust_UnOrder AS '退订时间' ,
                            Cust_DealPerson AS '退订人' ,
                            Cust_OutDate AS '退费时间' ,
                            Cust_ProvinceName AS '省' ,
                            Cust_CityName AS '市',
                            Cust_PassMan AS '审核人',
                            Cust_BelongProvinceName AS '计费号码归属地省',
                            Cust_BelongCityName AS '计费号码归属地市'
                    FROM  XFB_Customer w LEFT JOIN dbo.AF_User u ON w.Cust_WH_UserName = u.User_Name
                    WHERE 1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_Phone) != "")
                sb.Append(" AND Cust_Phone LIKE '%" + ValueHandler.GetStringValue(cust_Phone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");

            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(" ORDER BY w.JoinDate DESC ");
            return sb.ToString();
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="cust_Name"></param>
        /// <param name="cust_Linkman"></param>
        /// <param name="cust_Phone"></param>
        /// <param name="address"></param>
        /// <param name="cust_State"></param>
        /// <param name="cust_BillNumber"></param>
        /// <param name="cust_IsBill"></param>
        /// <param name="cust_WH_UserName"></param>
        /// <param name="staJoinDate"></param>
        /// <param name="endJoinDate"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="endcust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="endcust_OutDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetXFBNum(string cust_Name, string cust_Linkman, string cust_Phone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                        SELECT  COUNT(*) AS num
                        FROM    ( SELECT    w.* ,
                                            u.User_Place ,
                                            (Cust_ProvinceName + Cust_CityName) AS 'Cust_Area'
                                  FROM      XFB_Customer w
                                            LEFT JOIN dbo.AF_User u ON w.Cust_WH_UserName = u.User_Name
                                ) AS Num
                        WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_Phone) != "")
                sb.Append(" AND Cust_Phone LIKE '%" + ValueHandler.GetStringValue(cust_Phone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 维权通客服异常退回
        /// </summary>
        /// <param name="custCode"></param>
        /// <returns></returns>
        public bool BackXFBCustomer(string custCode)
        {
            string sql = string.Format(@"UPDATE XFB_Customer SET Cust_State='异常退回',Cust_ReturnDate=getdate(),Cust_OperateTime=getdate(),Cust_ReturnContent='计费号码有误,请修改计费号码'
                                    WHERE Cust_Code='{0}'", custCode);

            return UpdateData(sql);
        }

        #endregion

        #region 实时保 

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_LinkPhone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public DataTable GetSSBCust(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string PageIndex, string PageNum, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT TOP {0} *,Cust_Area FROM (", ValueHandler.GetIntNumberValue(PageNum));
            sb.Append(@"
                        SELECT  w.* ,
                                ( Cust_ProvinceName + Cust_CityName + ISNULL(Cust_CountyName, '') ) AS 'Cust_Area' ,
                                u.User_Place ,
                                ROW_NUMBER() OVER ( ORDER BY w.JoinDate DESC ) AS 'Num'
                        FROM    SSB_Customer w
                                LEFT JOIN AF_User u ON w.JoinMan = u.User_Name
                        WHERE   1 = 1 ");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(") T WHERE ");
            sb.AppendFormat(" T.Num >(0+({0}-1)*{1}) order by 'Num'", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 实时保导出列表
        /// </summary>
        /// <param name="cust_Name">客户名称</param>
        /// <param name="cust_Linkman">联系人</param>
        /// <param name="cust_LinkPhone">联系电话</param>
        /// <param name="address">地址</param>
        /// <param name="cust_State">客户状态</param>
        /// <param name="cust_BillNumber">计费号码</param>
        /// <param name="cust_IsBill">是否计费</param>
        /// <param name="cust_WH_UserName">外呼人员</param>
        /// <param name="staJoinDate">开通时间(开始)</param>
        /// <param name="endJoinDate">开通时间(结束)</param>
        /// <param name="cust_UnOrder">退订时间(开始)</param>
        /// <param name="endcust_UnOrder">退订时间(结束)</param>
        /// <param name="cust_OutDate">退费时间(开始)</param>
        /// <param name="endcust_OutDate">退费时间(结束)</param>
        /// <returns></returns>
        public string GetSSBCustExcel(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT  Cust_Name AS '客户名称' ,
                            Cust_Phone AS '座机号码' ,
                            Cust_BillNumber AS '计费号码' ,
                            Cust_IsBill AS '是否计费' ,
                            Cust_Linkman AS '联系人' ,
                            Cust_LinkPhone AS '联系电话' ,
                            Cust_WH_UserName AS '外呼人员' ,
		                    User_Place AS '工作地点',
                            Cust_State AS '客户状态' ,
                            Cust_KFVoice '录音编码',
                            Cust_OpenDate AS '开通时间' ,
                            Cust_UnOrder AS '退订时间' ,
                            Cust_DealPerson AS '退订人' ,
                            Cust_OutDate AS '退费时间' ,
                            Cust_ProvinceName AS '省' ,
                            Cust_CityName AS '市',
                            Cust_PassMan AS '审核人',
                            Cust_BelongProvinceName AS '计费号码归属地省',
                            Cust_BelongCityName AS '计费号码归属地市'
                    FROM    SSB_Customer w LEFT JOIN dbo.AF_User u ON w.Cust_WH_UserName = u.User_Name
                    WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");

            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");
            sb.Append(" ORDER BY w.JoinDate DESC ");
            return sb.ToString();
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="cust_Name"></param>
        /// <param name="cust_Linkman"></param>
        /// <param name="cust_LinkPhone"></param>
        /// <param name="address"></param>
        /// <param name="cust_State"></param>
        /// <param name="cust_BillNumber"></param>
        /// <param name="cust_IsBill"></param>
        /// <param name="cust_WH_UserName"></param>
        /// <param name="staJoinDate"></param>
        /// <param name="endJoinDate"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="endcust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="endcust_OutDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetSSBNum(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string place, string userplace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                        SELECT  COUNT(*) AS num
                        FROM    ( SELECT    w.* ,
                                            u.User_Place ,
                                            ( Cust_ProvinceName + Cust_CityName + Cust_CountyName ) AS 'Cust_Area'
                                  FROM      SSB_Customer w
                                            LEFT JOIN dbo.AF_User u ON w.JoinMan = u.User_Name
                                ) AS Num
                        WHERE   1 = 1");
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Linkman) != "")
                sb.Append(" AND Cust_Linkman LIKE '%" + ValueHandler.GetStringValue(cust_Linkman) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            if (ValueHandler.GetStringValue(Province) != "")
                sb.Append(" AND Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Province) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(city) != "")
                sb.Append(" AND Cust_CityCode = '" + ValueHandler.GetStringValue(city) + "'");
            if (ValueHandler.GetStringValue(cust_State) != "")
            {
                string[] Cust_States = ValueHandler.GetStringValue(cust_State).Split(';');
                StringBuilder Cust_StateStr = new StringBuilder();
                for (int i = 0; i < Cust_States.Length; i++)
                {
                    if (i == 0)
                        Cust_StateStr.AppendFormat(" Cust_State = '{0}'", Cust_States[i]);
                    else
                        Cust_StateStr.AppendFormat(" OR Cust_State = '{0}'", Cust_States[i]);
                }
                sb.AppendFormat(" AND ({0})", Cust_StateStr.ToString());
            }
            else
            {
                sb.Append("AND Cust_State IN('已审','退订','退费')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
            {
                string[] cust_WH_UserNames = ValueHandler.GetStringValue(cust_WH_UserName).Split(';');
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < cust_WH_UserNames.Length; i++)
                {
                    if (i == 0)
                        str.AppendFormat(" Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                    else
                        str.AppendFormat(" OR Cust_WH_UserName LIKE '%{0}%'", cust_WH_UserNames[i]);
                }
                sb.AppendFormat(" AND ({0})", str.ToString());
            }
            if (ValueHandler.GetStringValue(place) != "" && ValueHandler.GetStringValue(place) != "全部")
                sb.Append(" AND User_Place='" + ValueHandler.GetStringValue(place) + "'");
            if (ValueHandler.GetStringValue(staJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(staJoinDate));
            if (ValueHandler.GetStringValue(endJoinDate) != "")
                sb.Append(" AND Cust_OpenDate  <= '" + endJoinDate + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  >= " + ValueHandler.GetMarkStringDateValue(cust_UnOrder));
            if (ValueHandler.GetStringValue(endcust_UnOrder) != "")
                sb.Append(" AND Cust_UnOrder  <= '" + endcust_UnOrder + " 23:59:59'");
            if (ValueHandler.GetStringValue(cust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  >= " + ValueHandler.GetMarkStringDateValue(cust_OutDate));
            if (ValueHandler.GetStringValue(endcust_OutDate) != "")
                sb.Append(" AND Cust_OutDate  <= '" + endcust_OutDate + " 23:59:59'");
            if (userplace != "全部" && !string.IsNullOrEmpty(userplace))
                sb.Append(" AND User_Place = '" + userplace + "'");

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 维权通客服异常退回
        /// </summary>
        /// <param name="custCode"></param>
        /// <returns></returns>
        public bool BackSSBCustomer(string custCode)
        {
            string sql = string.Format(@"UPDATE SSB_Customer SET Cust_State='异常退回',Cust_ReturnDate=getdate(),Cust_OperateTime=getdate(),Cust_ReturnContent='计费号码有误,请修改计费号码'
                                    WHERE Cust_Code='{0}'", custCode);

            return UpdateData(sql);
        } 

        #endregion

        /// <summary>
        /// 获取工作地点
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserPlace()
        {
            string sql = @"SELECT User_Place
                        , Type
                    FROM (SELECT DISTINCT User_Place
                              , CASE WHEN User_Place = '全部' THEN 1 ELSE 2 END Type
                          FROM AF_User
                          WHERE User_Place != '') t
                    ORDER BY Type";

            return SearchData(sql);
        }
    }
}
