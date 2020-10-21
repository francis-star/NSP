/////////////////////////////////////////////////////////////////////////////
//模块名：审核管理中心
//开发者：杨栋
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_CustomerService : SqlBase
    {
        /// <summary>
        /// 得到客户状态
        /// </summary>
        /// <returns></returns>
        public DataTable GetState()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT c.*,t.CType_Name FROM SYS_CodeSet c
                        JOIN SYS_CodeType t ON c.CSet_CType_Code = t.CType_Code
                        WHERE c.DataState = 0 AND c.CSet_CType_Code='11'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custLinkPhone">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public DataTable GetCustomer(string custName, string custPhone, string custBillNumber, string custLinkman, string custLinkPhone, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} rownumber,Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_IsBill,Cust_State,Cust_OpenDate,Cust_KFVoice,Cust_ReturnContent,JoinDate,
                    (SELECT isnull(count(*),0) FROM YX_BillHistory WHERE BH_Cust_Code= c.Cust_Code AND DataState=0) AS XqCount 
                    FROM (SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber,* FROM CXT_Customer  WHERE DataState=0 ", pageNum);

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            if (!string.IsNullOrEmpty(custLinkman))
                sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custLinkPhone))
                sb.Append(" AND Cust_LinkPhone LIKE '%" + custLinkPhone + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }


            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            sb.AppendFormat(" ) c  WHERE c.rownumber >(0+({0}-1)*{1}) order by rownumber ", pageIndex, pageNum);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取客户信息数量
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custLinkPhone">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public string GetCustomerCount(string custName, string custPhone, string custBillNumber, string custLinkman, string custLinkPhone, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT count(*) AS DataCount FROM CXT_Customer WHERE DataState=0 ");

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            if (!string.IsNullOrEmpty(custLinkman))
                sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custLinkPhone))
                sb.Append(" AND Cust_LinkPhone LIKE '%" + custLinkPhone + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }

            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }

        }

        #region 审核

        public DataTable GetMQYCustPhoneSimilarInfo(string code, string phone, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                        AND c.Cust_LinkPhone='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT *  FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_LinkPhone='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                             AND c.Cust_LinkPhone='{3}'

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                             AND c.Cust_LinkPhone='{3}'
                                                
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, phone);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetMQYCustAddressSimilarInfo(string code, string address, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                      AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT *  FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                          AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}' 
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, address);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetMQYBillNoSimilarInfo(string code, string billno, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                         AND c.Cust_BillNumber='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT *  FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                              AND c.Cust_BillNumber='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                              AND c.Cust_BillNumber='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                              AND c.Cust_BillNumber='{3}'                                                
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, billno);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetMQYNameContainsKeySimilarInfo(string code, string key, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                       AND c.Cust_Name LIKE '%{3}%'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT *  FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                           AND c.Cust_Name LIKE '%{3}%'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_Name LIKE '%{3}%'

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_Name LIKE '%{3}%'
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, key);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetWQTCustPhoneSimilarInfo(string code, string phone, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                        AND c.Cust_LinkPhone='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_LinkPhone='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                             AND c.Cust_LinkPhone='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                             AND c.Cust_LinkPhone='{3}' 
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, phone);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetWQTCustAddressSimilarInfo(string code, string address, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                      AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                          AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, address);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetWQTBillNoSimilarInfo(string code, string billno, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                         AND c.Cust_BillNumber='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                              AND c.Cust_BillNumber='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                              AND c.Cust_BillNumber='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                              AND c.Cust_BillNumber='{3}'
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, billno);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetWQTNameContainsKeySimilarInfo(string code, string key, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                       AND c.Cust_Name LIKE '%{3}%'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                           AND c.Cust_Name LIKE '%{3}%'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_Name LIKE '%{3}%'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_Name LIKE '%{3}%'
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, key);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetCustPhoneSimilarInfo(string code, string phone, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                        AND c.Cust_LinkPhone='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                            AND c.Cust_LinkPhone='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                             AND c.Cust_LinkPhone='{3}'

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                             AND c.Cust_LinkPhone='{3}'
                                                
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, phone);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetCustAddressSimilarInfo(string code, string address, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                        AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                        AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                        AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                        AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, address);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetBillNoSimilarInfo(string code, string billno, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                                AND c.Cust_BillNumber='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                       AND c.Cust_BillNumber='{3}'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                        AND c.Cust_BillNumber='{3}'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                        AND c.Cust_BillNumber='{3}'
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, billno);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetNameContainsKeySimilarInfo(string code, string key, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
                                                AND c.Cust_Name LIKE '%{3}%'
                                                
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                        AND c.Cust_Name LIKE '%{3}%'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                        AND c.Cust_Name LIKE '%{3}%'

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
                                        AND c.Cust_Name LIKE '%{3}%'   
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, key);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public DataTable GetSimilarInfo(string code, string keyName, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT  c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                    c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber  OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3})
                                        UNION SELECT  c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                        c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber   OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3}) 
                                        UNION SELECT  c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                        c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber   OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                 {3}) 
                                        UNION SELECT  c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                        c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber   OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                 {3}) 
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pageNum, pageIndex, keyName);

            /*XFB
            UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_Phone,'新消费宝典' as TableName FROM XFB_Customer c
            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
            WHERE c.DataState = 0  
                    AND ( c.Cust_BillNumber = temp.Cust_LinkPhone) */
            return SearchData(sb.ToString());
        }

        public string GetKeySimilarCount(string code, string key)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM CXT_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_Name LIKE '%{1}%'

                                        UNION SELECT Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND c.Cust_Name LIKE '%{1}%'
                                         
                                        UNION SELECT Cust_Code FROM WQT_Customer c 
			                            WHERE c.DataState = 0  
					                           AND c.Cust_Name LIKE '%{1}%'
					                         
                                        UNION SELECT Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                           AND c.Cust_Name LIKE '%{1}%'
                                                )T
 		                 ", code, key);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetBillNoSimilarCount(string code, string billno)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM CXT_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_BillNumber = '{1}'

                                        UNION SELECT Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'
                                         
                                        UNION SELECT Cust_Code FROM WQT_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'
					                         
                                        UNION SELECT Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'
                                 )T
 		                 ", code, billno);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetAddressSimilarCount(string code, string address)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM CXT_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'

                                        UNION select Cust_Code  FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'
                                         
                                        UNION select Cust_Code FROM WQT_Customer c 
			                            WHERE c.DataState = 0  
					                          AND  c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'

					                    UNION select Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                          AND  c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'
                                                )T
 		                 ", code, address);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetPhoneSimilarCount(string code, string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM CXT_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_LinkPhone = '{1}'

                                        UNION SELECT Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'
                                         
                                        UNION SELECT Cust_Code FROM WQT_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'

					                    UNION SELECT Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'
                                                )T
 		                 ", code, phone);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetWQTKeySimilarCount(string code, string key)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM WQT_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_Name LIKE '%{1}%'
                                        UNION select Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND c.Cust_Name LIKE '%{1}%'
                                         
                                        UNION select Cust_Code FROM CXT_Customer c 
			                            WHERE c.DataState = 0  
					                           AND c.Cust_Name LIKE '%{1}%'
					                         
                                        UNION select Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                           AND c.Cust_Name LIKE '%{1}%'
                                   )T
 		                 ", code, key);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetWQTBillNoSimilarCount(string code, string billno)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM WQT_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_BillNumber = '{1}'

                                        UNION select Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'
                                         
                                        UNION select Cust_Code FROM CXT_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'
					                         
                                        UNION select Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'
                                )T
 		                 ", code, billno);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetWQTAddressSimilarCount(string code, string address)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM WQT_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'

                                        UNION select Cust_Code  FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'
                                         
                                        UNION select Cust_Code FROM CXT_Customer c 
			                            WHERE c.DataState = 0  
					                          AND  c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'

                                        UNION select Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                          AND  c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}' 
                                                )T
 		                 ", code, address);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetWQTPhoneSimilarCount(string code, string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM WQT_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_LinkPhone = '{1}'

                                        UNION select Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'
                                         
                                        UNION select Cust_Code FROM CXT_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'  

                                        UNION select Cust_Code FROM SSB_Customer c
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}' 
                                                )T
 		                 ", code, phone);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetMQYKeySimilarCount(string code, string key)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"     select count(T.Cust_Code)  as total from(   
                                        SELECT Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_Name LIKE '%{1}%'

                                        UNION select Cust_Code  FROM WQT_Customer c
			                            WHERE c.DataState = 0  
					                            AND c.Cust_Name LIKE '%{1}%'
                                         
                                        UNION select Cust_Code FROM CXT_Customer c 
			                            WHERE c.DataState = 0  
					                           AND c.Cust_Name LIKE '%{1}%'

                                        UNION select Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                           AND c.Cust_Name LIKE '%{1}%'
					                         
                                                )T
 		                 ", code, key);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetMQYBillNoSimilarCount(string code, string billno)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"     select count(T.Cust_Code)  as total from(   
                                        SELECT Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_BillNumber = '{1}'

                                        UNION select Cust_Code  FROM WQT_Customer c
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'
                                         
                                        UNION select Cust_Code FROM CXT_Customer c			                         
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'		 

                                        UNION select Cust_Code FROM SSB_Customer c			                         
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_BillNumber = '{1}'					                         
                                                )T
 		                 ", code, billno);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetMQYAddressSimilarCount(string code, string address)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'

                                        UNION select Cust_Code  FROM WQT_Customer c
			                            WHERE c.DataState = 0  
					                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'
                                         
                                        UNION select Cust_Code FROM CXT_Customer c 
			                            WHERE c.DataState = 0  
					                          AND  c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'

                                        UNION select Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                          AND  c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}' 
                                        )T
 		                 ", code, address);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetMQYPhoneSimilarCount(string code, string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code FROM MQY_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_LinkPhone = '{1}'

                                        UNION select Cust_Code FROM WQT_Customer c
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'
                                         
                                        UNION select Cust_Code FROM CXT_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'
					                         
                                       UNION select Cust_Code FROM SSB_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'
                                                )T
 		                 ", code, phone);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 获取相似信息的条数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetSimilarCount(string code, string keyName)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@"select sum(T._number)  as total from(   SELECT count(1) as _number,1 Type FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber  OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1})

                                        UNION select count(1) as _number,2  FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM CXT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber    OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1}) 

                                        UNION select count(1) as _number,3 FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber   OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1} )    

                                        UNION select count(1) as _number,4 FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber   OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1} )                                    
                                        )T
 		                 ", code, keyName);

            /*XFB 
            UNION select count(1) as _number,4 FROM XFB_Customer c
			LEFT JOIN (SELECT * FROM CXT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			WHERE c.DataState = 0  
					AND ( c.Cust_BillNumber = temp.Cust_LinkPhone)
             */
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable GetCustomerByCode(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM CXT_Customer WHERE Cust_Code='{0}'", code);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="isAudit">是否通过 0 通过 ；1 退回</param>
        /// <param name="para">是否显示/退回原因</param>
        public bool Pass(string code, string isAudit, string para, string userName)
        {
            StringBuilder sb = new StringBuilder();
            //通过
            if (isAudit == "0")
            {
                sb.AppendFormat(@"UPDATE CXT_Customer SET Cust_State='{1}',Cust_IsView={2},Cust_OpenDate=getdate(),Cust_OperateTime =getdate(),Cust_PassMan='{3}'
                        WHERE Cust_Code='{0}'", code, "已审", ValueHandler.GetIntNumberValue(para), userName);
            }
            else
            {
                sb.AppendFormat(@"UPDATE CXT_Customer SET Cust_State='{1}',Cust_ReturnContent='{2}',Cust_OperateTime =getdate(),Cust_ReturnDate=getdate(),Cust_ReturnBackMan='{3}'
                        WHERE Cust_Code='{0}'", code, "退回", para, userName);
            }
            return UpdateData(sb.ToString());
        }
        #endregion

        #region 修改

        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isBill"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool UpBillHistory(string code, string isBill, string Remark, string userName)
        {
            string isBills = isBill == "0" ? "是" : "否";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE CXT_Customer SET Cust_IsBill='{1}' WHERE Cust_Code='{0}';", code, isBills);
            sb.AppendFormat(@"INSERT INTO YX_BillHistory(BH_Code,BH_Cust_Code,BH_User_Name,BH_Time,BH_Content,BH_Remark,DataState,JoinMan,JoinDate)
                        VALUES('{0}','{1}','{2}',getdate(),'{3}','{4}',0,'{2}',getdate())", GetCode(), code, userName, isBills, Remark);
            return UpdateData(sb.ToString());
        }
        /// <summary>
        /// 记录修改历史
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isBill"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool UpBillHistory(string code, string content, string Remark, string userName, string empty)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"INSERT INTO YX_BillHistory(BH_Code,BH_Cust_Code,BH_User_Name,BH_Time,BH_Content,BH_Remark,DataState,JoinMan,JoinDate)
                        VALUES('{0}','{1}','{2}',getdate(),'{3}','{4}',0,'{2}',getdate())", GetCode(), code, userName, content, Remark);
            return UpdateData(sb.ToString());
        }
        /// <summary>
        /// 修改营销成功 ,并且修改有效数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="custName"></param>
        /// <param name="custPhone"></param>
        /// <returns></returns>
        public bool UpAlreadyUse(string code, string custName, string custPhone)
        {
            StringBuilder sb = new StringBuilder();
            //            sb.AppendFormat(@"UPDATE o SET OD_AlreadyUse=isnull(OD_AlreadyUse,0) +1
            //                        FROM CXT_OriginalData o 
            //                        JOIN CXT_OriginalDataValid  d ON o.OD_Code= d.ODD_OD_Code AND d.DataState=0
            //                        JOIN CXT_Customer c ON c.Cust_BillNumber = d.ODD_Phone AND c.Cust_Code = '{0}';", code);

            sb.AppendFormat(@"UPDATE o SET o.OD_AlreadyUse = isnull(o.OD_AlreadyUse,0) + 1,
                        OD_State = '已使用'
                        FROM CXT_OriginalData o 
                        JOIN  CXT_OriginalDataValid  d ON o.OD_Code = d.ODD_OD_Code
                        WHERE (ODD_Name = '{0}' OR ODD_Phone = '{1}') AND o.DataState = 0", custName, custPhone);
            return UpdateData(sb.ToString());
        }

        #endregion

        #region 民企云

        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custLinkPhone">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public DataTable GetMQYCustomer(string custName, string custPhone, string custBillNumber, string custLinkman, string custLinkPhone, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} rownumber,Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_IsBill,Cust_State,Cust_OpenDate,Cust_KFVoice,Cust_ReturnContent,JoinDate,
                    (SELECT isnull(count(*),0) FROM YX_BillHistory WHERE BH_Cust_Code= c.Cust_Code ) AS XqCount 
                    FROM (SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber,* FROM MQY_Customer  WHERE 1=1 ", pageNum);

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            if (!string.IsNullOrEmpty(custLinkman))
                sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custLinkPhone))
                sb.Append(" AND Cust_LinkPhone LIKE '%" + custLinkPhone + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }


            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            sb.AppendFormat(" ) c  WHERE c.rownumber >(0+({0}-1)*{1}) order by rownumber ", pageIndex, pageNum);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取客户信息数量
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custLinkPhone">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public string GetMQYCustomerCount(string custName, string custPhone, string custBillNumber, string custLinkman, string custLinkPhone, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT count(*) AS DataCount FROM MQY_Customer WHERE 1=1 ");

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            if (!string.IsNullOrEmpty(custLinkman))
                sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custLinkPhone))
                sb.Append(" AND Cust_LinkPhone LIKE '%" + custLinkPhone + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }

            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }

        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public DataTable GetMQYSimilarInfo(string code, string keyName, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@"SELECT TOP {1} a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
                                        LEFT JOIN (SELECT * FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
                                        WHERE c.DataState = 0  AND c.Cust_State != '待审' AND c.Cust_State != '退回'
		                                        AND c.Cust_Code != '{0}' 
                                                AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
                                                OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3}) 
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3}) 
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
                                        LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
                                        WHERE c.DataState = 0  
                                                AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
                                                OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3}) 
                                         UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
                                        LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
                                        WHERE c.DataState = 0  
                                                AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
                                                OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3}) 
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pageNum, pageIndex, keyName);

            /*XFB
              UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'新消费宝典' as TableName FROM XFB_Customer c
                                        LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
                                        WHERE c.DataState = 0  
                                                AND ( c.Cust_BillNumber = temp.Cust_LinkPhone) 
             */
            return SearchData(sb.ToString());

        }

        /// <summary>
        /// 获取相似信息的条数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetMQYSimilarCount(string code, string keyName)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@"select sum(T._number)  as total from(   SELECT count(1) as _number,1 Type FROM MQY_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1})
                                        UNION select count(1) as _number,2  FROM WQT_Customer c
			                            LEFT JOIN (SELECT *  FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1}) 
                                        UNION select count(1) as _number,3 FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1} )
                                        UNION select count(1) as _number,4 FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber   OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1} )
                                        )T
 		                 ", code, keyName);

            /*XFB
              UNION select count(1) as _number,4 FROM XFB_Customer c
			                            LEFT JOIN (SELECT * FROM MQY_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_BillNumber = temp.Cust_LinkPhone)
             */
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable GetMQYCustomerByCode(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM MQY_Customer WHERE Cust_Code='{0}'", code);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="isAudit">是否通过 0 通过 ；1 退回</param>
        /// <param name="para">是否显示/退回原因</param>
        public bool PassMQY(string code, string isAudit, string para, string userName)
        {
            StringBuilder sb = new StringBuilder();
            //通过
            if (isAudit == "0")
            {
                sb.AppendFormat(@"UPDATE MQY_Customer SET Cust_State='{1}',Cust_IsView={2},Cust_OpenDate=getdate(),Cust_OperateTime =getdate(),Cust_PassMan='{3}'
                        WHERE Cust_Code='{0}'", code, "已审", ValueHandler.GetIntNumberValue(para), userName);
            }
            else
            {
                sb.AppendFormat(@"UPDATE MQY_Customer SET Cust_State='{1}',Cust_ReturnContent='{2}',Cust_OperateTime =getdate(),Cust_ReturnDate=getdate(),Cust_ReturnBackMan='{3}'
                        WHERE Cust_Code='{0}'", code, "退回", para, userName);
            }
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isBill"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool UpMQYBillHistory(string code, string isBill, string BH_Remark, string userName)
        {
            string isBills = isBill == "0" ? "是" : "否";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE MQY_Customer SET Cust_IsBill='{1}' WHERE Cust_Code='{0}';", code, isBills);
            sb.AppendFormat(@"INSERT INTO YX_BillHistory(BH_Code,BH_Cust_Code,BH_User_Name,BH_Time,BH_Content,BH_Remark,DataState,JoinMan,JoinDate)
                        VALUES('{0}','{1}','{2}',getdate(),'{3}','{4}',0,'{2}',getdate())", GetCode(), code, userName, isBills, BH_Remark);
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 修改营销成功 ,并且修改有效数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="custName"></param>
        /// <param name="custPhone"></param>
        /// <returns></returns>
        public bool UpMQYAlreadyUse(string code, string custName, string custPhone)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"UPDATE o SET o.OD_AlreadyUse = isnull(o.OD_AlreadyUse,0) + 1,
                        OD_State = '已使用'
                        FROM MQY_OriginalData o 
                        JOIN  MQY_OriginalDataValid  d ON o.OD_Code = d.ODD_OD_Code
                        WHERE (ODD_Name = '{0}' OR ODD_Phone = '{1}') AND o.DataState = 0", custName, custPhone);
            return UpdateData(sb.ToString());
        }

        #endregion

        #region 维权通

        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custLinkPhone">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public DataTable GetWQTCustomer(string custName, string custPhone, string custBillNumber, string custLinkman, string custLinkPhone, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} rownumber,Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_IsBill,Cust_State,Cust_OpenDate,Cust_KFVoice,Cust_ReturnContent,JoinDate,
                    (SELECT isnull(count(*),0) FROM YX_BillHistory WHERE BH_Cust_Code= c.Cust_Code) AS XqCount 
                    FROM (SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber,* FROM WQT_Customer  WHERE 1=1 ", pageNum);

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            if (!string.IsNullOrEmpty(custLinkman))
                sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custLinkPhone))
                sb.Append(" AND Cust_LinkPhone LIKE '%" + custLinkPhone + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }


            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            sb.AppendFormat(" ) c  WHERE c.rownumber >(0+({0}-1)*{1}) order by rownumber ", pageIndex, pageNum);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取客户信息数量
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custLinkPhone">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public string GetWQTCustomerCount(string custName, string custPhone, string custBillNumber, string custLinkman, string custLinkPhone, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT count(*) AS DataCount FROM WQT_Customer WHERE 1=1 ");

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            if (!string.IsNullOrEmpty(custLinkman))
                sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custLinkPhone))
                sb.Append(" AND Cust_LinkPhone LIKE '%" + custLinkPhone + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }

            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }

        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public DataTable GetWQTSimilarInfo(string code, string keyName, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@"SELECT top {1}  a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  AND c.Cust_State != '待审' AND c.Cust_State != '退回'
												AND c.Cust_Code != '{0}' 
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3})  
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                            c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3})
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3} ) 
                                          UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT *  FROM WQT_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3} ) 
 		                            )b
	                            )a
                           where a.rownumber > (0+({2}-1)*{1})", code, pageNum, pageIndex, keyName);

            /*XFB
               UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,
                                            c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'新消费宝典' as TableName FROM XFB_Customer c
                                        LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
                                        WHERE c.DataState = 0  
                                                AND ( c.Cust_BillNumber = temp.Cust_LinkPhone) 
             */
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取相似信息的条数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetWQTSimilarCount(string code, string keyName)
        {

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@"SELECT sum(T._number) AS total from(SELECT count(1) AS _number,1 Type FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回'
					                            AND c.Cust_Code != '{0}' 
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1})

                                        UNION select count(1) AS _number,2  FROM CXT_Customer c
			                            LEFT JOIN (SELECT *  FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1}) 

                                        UNION select count(1) AS _number,3 FROM MQY_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1} )
                                        
                                        UNION select count(1) AS _number,4 FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {1} )
                                        )T
 		                 ", code, keyName);

            /*xfb 
            UNION select count(1) as _number,4 FROM XFB_Customer c
			LEFT JOIN (SELECT * FROM WQT_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			WHERE c.DataState = 0  
					AND ( c.Cust_BillNumber = temp.Cust_LinkPhone)
             */
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable GetWQTCustomerByCode(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM WQT_Customer WHERE Cust_Code='{0}'", code);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="isAudit">是否通过 0 通过 ；1 退回</param>
        /// <param name="para">是否显示/退回原因</param>
        public bool PassWQT(string code, string isAudit, string para, string userName)
        {
            StringBuilder sb = new StringBuilder();
            //通过
            if (isAudit == "0")
            {
                sb.AppendFormat(@"UPDATE WQT_Customer SET Cust_State='{1}',Cust_IsView={2},Cust_OpenDate=getdate(),Cust_OperateTime =getdate(),Cust_PassMan='{3}'
                        WHERE Cust_Code='{0}'", code, "已审", ValueHandler.GetIntNumberValue(para), userName);
            }
            else
            {
                sb.AppendFormat(@"UPDATE WQT_Customer SET Cust_State='{1}',Cust_ReturnContent='{2}',Cust_OperateTime =getdate(),Cust_ReturnDate=getdate(),Cust_ReturnBackMan='{3}'
                        WHERE Cust_Code='{0}'", code, "退回", para, userName);
            }
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isBill"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool UpWQTBillHistory(string code, string isBill, string BH_Remark, string userName)
        {
            string isBills = isBill == "0" ? "是" : "否";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE WQT_Customer SET Cust_IsBill='{1}' WHERE Cust_Code='{0}';", code, isBills);
            sb.AppendFormat(@"INSERT INTO YX_BillHistory(BH_Code,BH_Cust_Code,BH_User_Name,BH_Time,BH_Content,BH_Remark,DataState,JoinMan,JoinDate)
                        VALUES('{0}','{1}','{2}',getdate(),'{3}','{4}',0,'{2}',getdate())", GetCode(), code, userName, isBills, BH_Remark);
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 修改营销成功 ,并且修改有效数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="custName"></param>
        /// <param name="custPhone"></param>
        /// <returns></returns>
        public bool UpWQTAlreadyUse(string code, string custName, string custPhone)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"UPDATE o SET o.OD_AlreadyUse = isnull(o.OD_AlreadyUse,0) + 1,
                        OD_State = '已使用'
                        FROM WQT_OriginalData o 
                        JOIN  WQT_OriginalDataValid  d ON o.OD_Code = d.ODD_OD_Code
                        WHERE (ODD_Name = '{0}' OR ODD_Phone = '{1}') AND o.DataState = 0", custName, custPhone);
            return UpdateData(sb.ToString());
        }

        #endregion

        #region 新消费宝典
        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custWhUserName">外呼人员</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public DataTable GetXFBCustomer(string custName, string custPhone, string custBillNumber, string custLinkman, string custWhUserName, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} rownumber,Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_IsBill,Cust_State,Cust_OpenDate,Cust_KFVoice,Cust_ReturnContent,JoinDate,
                    (SELECT isnull(count(*),0) FROM YX_BillHistory WHERE BH_Cust_Code= c.Cust_Code) AS XqCount 
                    FROM (SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber,* FROM XFB_Customer  WHERE 1=1 ", pageNum);

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            //if (!string.IsNullOrEmpty(custLinkman))
            //    sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custWhUserName))
                sb.Append(" AND Cust_WH_UserName LIKE '%" + custWhUserName + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }

            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            sb.AppendFormat(" ) c  WHERE c.rownumber >(0+({0}-1)*{1}) order by rownumber ", pageIndex, pageNum);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取客户信息数量
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custWhUserName">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public string GetXFBCustomerCount(string custName, string custPhone, string custBillNumber, string custLinkman, string custWhUserName, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT count(*) AS DataCount FROM XFB_Customer WHERE 1=1 ");

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            //if (!string.IsNullOrEmpty(custLinkman))
            //    sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custWhUserName))
                sb.Append(" AND Cust_WH_UserName LIKE '%" + custWhUserName + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }

            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public DataTable GetXFBSimilarInfo(string code, string keyName, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@"SELECT top {1}  a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_OldName,c.Cust_Name,c.Cust_Phone,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_CountyName,c.Cust_IsBill,c.Cust_BillMoney,c.Cust_BillNumber,c.Cust_Nature,c.Cust_KFVoice,c.Cust_Source,c.Cust_WH_Remark,c.Cust_CityCode,c.Cust_LinkPhone,c.Cust_Address,c.Cust_State,'新消费宝典' as TableName,(c.Cust_ProvinceName+c.Cust_CityName+isnull(c.Cust_CountyName,'')) AS 'Cust_Area' FROM XFB_Customer c
			                            LEFT JOIN (SELECT * FROM XFB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  AND c.Cust_State != '待审' AND c.Cust_State != '退回'
												AND c.Cust_Code != '{0}' 
					                            AND (c.Cust_BillNumber = temp.Cust_BillNumber)    
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_OldName,c.Cust_Name,c.Cust_Phone,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_CountyName,c.Cust_IsBill,c.Cust_BillMoney,c.Cust_BillNumber,c.Cust_Nature,c.Cust_KFVoice,c.Cust_Source,c.Cust_WH_Remark,c.Cust_CityCode,c.Cust_LinkPhone,c.Cust_Address,c.Cust_State,'诚信通' as TableName,(c.Cust_ProvinceName+c.Cust_CityName+isnull(c.Cust_CountyName,'')) AS 'Cust_Area' FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM XFB_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND (c.Cust_LinkPhone = temp.Cust_BillNumber)
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_OldName,c.Cust_Name,c.Cust_Phone,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_CountyName,c.Cust_IsBill,c.Cust_BillMoney,c.Cust_BillNumber,c.Cust_Nature,c.Cust_KFVoice,c.Cust_Source,c.Cust_WH_Remark,c.Cust_CityCode,c.Cust_LinkPhone,c.Cust_Address,c.Cust_State,'维权通' as TableName,(c.Cust_ProvinceName+c.Cust_CityName+isnull(c.Cust_CountyName,'')) AS 'Cust_Area' FROM WQT_Customer c
			                            LEFT JOIN (SELECT * FROM XFB_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND (c.Cust_LinkPhone = temp.Cust_BillNumber)
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_OldName,c.Cust_Name,c.Cust_Phone,c.Cust_ProvinceName,c.Cust_CityName,c.Cust_CountyName,c.Cust_IsBill,c.Cust_BillMoney,c.Cust_BillNumber,c.Cust_Nature,c.Cust_KFVoice,c.Cust_Source,c.Cust_WH_Remark,c.Cust_CityCode,c.Cust_LinkPhone,c.Cust_Address,c.Cust_State,'民企云' as TableName,(c.Cust_ProvinceName+c.Cust_CityName+isnull(c.Cust_CountyName,'')) AS 'Cust_Area' FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM XFB_Customer  WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND (c.Cust_LinkPhone = temp.Cust_BillNumber)                                 
 		                            )b
	                            )a
                           where a.rownumber > (0+({2}-1)*{1})", code, pageNum, pageIndex);


            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取相似信息的条数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetXFBSimilarCount(string code, string keyName)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat($@"SELECT sum (T._number) AS total
                                FROM (SELECT count (1) AS _number,1 Type
                                      FROM XFB_Customer c
                                          LEFT JOIN (SELECT *
                                                     FROM XFB_Customer
                                                     WHERE DataState = 0 AND Cust_Code = '{code}') temp ON 1 = 1
                                      WHERE c.DataState = 0 AND c.Cust_State != '待审' AND c.Cust_State != '退回' AND c.Cust_Code != '{code}' AND (c.Cust_BillNumber = temp.Cust_BillNumber)
                                  UNION
                                  SELECT count (1) AS _number,2
                                  FROM CXT_Customer c
                                      LEFT JOIN (SELECT *
                                                 FROM XFB_Customer
                                                 WHERE DataState = 0 AND Cust_Code = '{code}') temp ON 1 = 1
                                  WHERE c.DataState = 0 AND (c.Cust_LinkPhone = temp.Cust_BillNumber) UNION
                                  SELECT count (1) AS _number,3
                                  FROM MQY_Customer c
                                      LEFT JOIN (SELECT *
                                                 FROM XFB_Customer
                                                 WHERE DataState = 0 AND Cust_Code = '{code}') temp ON 1 = 1
                                  WHERE c.DataState = 0 AND (c.Cust_LinkPhone = temp.Cust_BillNumber) UNION
                                  SELECT count (1) AS _number,4
                                  FROM WQT_Customer c
                                      LEFT JOIN (SELECT *
                                                 FROM XFB_Customer
                                                 WHERE DataState = 0 AND Cust_Code = '{code}') temp ON 1 = 1
                                  WHERE c.DataState = 0 AND (c.Cust_LinkPhone = temp.Cust_BillNumber)

                    ) T", code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable GetXFBCustomerByCode(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM XFB_Customer WHERE Cust_Code='{0}'", code);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="isAudit">是否通过 0 通过 ；1 退回</param>
        /// <param name="para">是否显示/退回原因</param>
        public bool PassXFB(string code, string isAudit, string para, string userName)
        {
            StringBuilder sb = new StringBuilder();
            //通过
            if (isAudit == "0")
            {
                sb.AppendFormat(@"UPDATE XFB_Customer SET Cust_State='{1}',Cust_IsView={2},Cust_OpenDate=getdate(),Cust_OperateTime =getdate(),Cust_PassMan='{3}'
                        WHERE Cust_Code='{0}'", code, "已审", ValueHandler.GetIntNumberValue(para), userName);
            }
            else
            {
                sb.AppendFormat(@"UPDATE XFB_Customer SET Cust_State='{1}',Cust_ReturnContent='{2}',Cust_OperateTime =getdate(),Cust_ReturnDate=getdate(),Cust_ReturnBackMan='{3}'
                        WHERE Cust_Code='{0}'", code, "退回", para, userName);
            }
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isBill"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool UpXFBBillHistory(string code, string isBill, string BH_Remark, string userName)
        {
            string isBills = isBill == "0" ? "是" : "否";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE XFB_Customer SET Cust_IsBill='{1}' WHERE Cust_Code='{0}';", code, isBills);
            sb.AppendFormat(@"INSERT INTO YX_BillHistory(BH_Code,BH_Cust_Code,BH_User_Name,BH_Time,BH_Content,BH_Remark,DataState,JoinMan,JoinDate)
                        VALUES('{0}','{1}','{2}',getdate(),'{3}','{4}',0,'{2}',getdate())", GetCode(), code, userName, isBills, BH_Remark);
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 修改营销成功 ,并且修改有效数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="custName"></param>
        /// <param name="custPhone"></param>
        /// <returns></returns>
        public bool UpXFBAlreadyUse(string code, string custName, string custPhone)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"UPDATE o SET o.OD_AlreadyUse = isnull(o.OD_AlreadyUse,0) + 1,
                        OD_State = '已使用'
                        FROM XFB_OriginalData o 
                        JOIN  XFB_OriginalDataValid  d ON o.OD_Code = d.ODD_OD_Code
                        WHERE (ODD_Phone = '{0}') AND o.DataState = 0", custPhone);
            return UpdateData(sb.ToString());
        }

        public DataTable GetXFBlackSimilarInfo(string code, string type)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = "XFB";
            if (type == "1")
                tableName = "WQT";
            else if (type == "2")
                tableName = "MQY";
            else if (type == "3")
                tableName = "CXT";
            sb.AppendFormat($@"SELECT b.*
                            FROM {tableName}_Customer c
                                JOIN YX_BlackList b ON c.Cust_BillNumber = b.BL_Phone AND b.DataState = 0
                            WHERE c.DataState = 0 AND c.Cust_Code = '{code}'");
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        #endregion

        #region 实时保

        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custLinkPhone">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public DataTable GetSSBCustomer(string custName, string custPhone, string custBillNumber, string custLinkman, string custLinkPhone, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} rownumber,Cust_Code,Cust_Name,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_BillNumber,Cust_IsBill,Cust_State,Cust_OpenDate,Cust_KFVoice,Cust_ReturnContent,JoinDate,
                    (SELECT isnull(count(*),0) FROM YX_BillHistory WHERE BH_Cust_Code= c.Cust_Code) AS XqCount 
                    FROM (SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber,* FROM SSB_Customer WHERE 1=1 ", pageNum);

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            if (!string.IsNullOrEmpty(custLinkman))
                sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custLinkPhone))
                sb.Append(" AND Cust_LinkPhone LIKE '%" + custLinkPhone + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }

            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            sb.AppendFormat(" ) c  WHERE c.rownumber >(0+({0}-1)*{1}) order by rownumber ", pageIndex, pageNum);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取客户信息数量
        /// </summary>
        /// <param name="custName">名称</param>
        /// <param name="custPhone">电话</param>
        /// <param name="custBillNumber">计费号码</param>
        /// <param name="custLinkman">联系人</param>
        /// <param name="custLinkPhone">联系电话</param>
        /// <param name="custState">客户状态</param>
        /// <param name="custOpenDate">开头时间</param>
        /// <param name="custEndDate">结束时间</param>
        /// <returns></returns>
        public string GetSSBCustomerCount(string custName, string custPhone, string custBillNumber, string custLinkman, string custLinkPhone, string custState, string custOpenDate, string custEndDate, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT count(*) AS DataCount FROM SSB_Customer WHERE 1=1 ");

            if (!string.IsNullOrEmpty(custName))
                sb.Append(" AND Cust_Name LIKE '%" + custName + "%'");

            if (!string.IsNullOrEmpty(custPhone))
                sb.Append(" AND Cust_Phone LIKE '%" + custPhone + "%'");

            if (!string.IsNullOrEmpty(custBillNumber))
                sb.Append(" AND Cust_BillNumber LIKE '%" + custBillNumber + "%'");

            if (!string.IsNullOrEmpty(custLinkman))
                sb.Append(" AND Cust_Linkman LIKE '%" + custLinkman + "%'");

            if (!string.IsNullOrEmpty(custLinkPhone))
                sb.Append(" AND Cust_LinkPhone LIKE '%" + custLinkPhone + "%'");

            if (!string.IsNullOrEmpty(custState))
            {
                string[] custStates = custState.Split(';');
                StringBuilder custStaSql = new StringBuilder();
                for (int i = 0; i < custStates.Length; i++)
                {
                    if (i == 0)
                        custStaSql.AppendFormat(" Cust_State = '{0}'", custStates[i]);
                    else
                        custStaSql.AppendFormat(" OR Cust_State = '{0}'", custStates[i]);
                }
                sb.Append(" AND (" + custStaSql.ToString() + ")");
            }

            if (!string.IsNullOrEmpty(custOpenDate))
                sb.Append(" AND Cust_OpenDate  >= " + ValueHandler.GetMarkStringDateValue(custOpenDate));

            if (!string.IsNullOrEmpty(custEndDate))
                sb.Append(" AND Cust_OpenDate <= '" + custEndDate + " 23:59:59'");
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public DataTable GetSSBSimilarInfo(string code, string keyName, string billNo, string phone, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@"SELECT top {1}  a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
												AND c.Cust_Code != '{0}' 
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber 
                                                OR c.Cust_TSPhone IN ('{4}','{5}')
                                                OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3})  
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                            c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c
			                            LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')  
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber 
                                                OR c.Cust_TSPhone IN ('{4}','{5}')
                                                OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3})
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber 
                                                OR c.Cust_TSPhone IN ('{4}','{5}')
                                                OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3} ) 
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                            c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c
                                        LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
                                        WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')  
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber 
                                                OR c.Cust_TSPhone IN ('{4}','{5}')
                                                OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                {3})
 		                            )b
	                            )a
                           where a.rownumber > (0+({2}-1)*{1})", code, pageNum, pageIndex, keyName, billNo, phone);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取相似信息的条数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetSSBSimilarCount(string code, string keyName, string billNo, string phone)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(keyName))
            {
                keyName = string.Format(" OR c.Cust_Name LIKE '%{0}%'", keyName);
            }
            sb.AppendFormat(@" select sum(T._number)  as total from(SELECT count(1) as _number,5 Type FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' 
					                            AND (c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber 
                                                OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                OR c.Cust_TSPhone IN ('{2}','{3}')
                                                {1}) 
                                UNION select count(1) as _number,1  FROM WQT_Customer c
			                            LEFT JOIN (SELECT *  FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber 
                                                OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                OR c.Cust_TSPhone IN ('{2}','{3}')
                                                {1}) 
                                UNION select count(1) as _number,3  FROM CXT_Customer c
			                            LEFT JOIN (SELECT *  FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber 
                                                OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                OR c.Cust_TSPhone IN ('{2}','{3}')
                                                {1}) 
                                UNION select count(1) as _number,2  FROM MQY_Customer c
			                            LEFT JOIN (SELECT *  FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0  
					                            AND ( c.Cust_LinkPhone = temp.Cust_LinkPhone
					                            OR c.Cust_BillNumber = temp.Cust_BillNumber 
                                                OR c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address=temp.Cust_ProvinceName+temp.Cust_CityName+temp.Cust_Address
                                                OR c.Cust_TSPhone IN ('{2}','{3}')
                                                {1}) 
                                        )T
 		                 ", code, keyName, billNo, phone);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable GetSSBCustomerByCode(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM SSB_Customer WHERE Cust_Code='{0}'", code);
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="isAudit">是否通过 0 通过 ；1 退回</param>
        /// <param name="para">是否显示/退回原因</param>
        public bool PassSSB(string code, string isAudit, string para, string userName)
        {
            StringBuilder sb = new StringBuilder();
            //通过
            if (isAudit == "0")
            {
                sb.AppendFormat(@"UPDATE SSB_Customer SET Cust_State='{1}',Cust_IsView={2},Cust_OpenDate=getdate(),Cust_OperateTime =getdate(),Cust_PassMan='{3}'
                        WHERE Cust_Code='{0}'", code, "已审", ValueHandler.GetIntNumberValue(para), userName);
            }
            else
            {
                sb.AppendFormat(@"UPDATE SSB_Customer SET Cust_State='{1}',Cust_ReturnContent='{2}',Cust_OperateTime =getdate(),Cust_ReturnDate=getdate(),Cust_ReturnBackMan='{3}'
                        WHERE Cust_Code='{0}'", code, "退回", para, userName);
            }

            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isBill"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool UpSSBBillHistory(string code, string isBill, string BH_Remark, string userName)
        {
            string isBills = isBill == "0" ? "是" : "否";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE SSB_Customer SET Cust_IsBill='{1}' WHERE Cust_Code='{0}';", code, isBills);
            sb.AppendFormat(@"INSERT INTO YX_BillHistory(BH_Code,BH_Cust_Code,BH_User_Name,BH_Time,BH_Content,BH_Remark,DataState,JoinMan,JoinDate)
                        VALUES('{0}','{1}','{2}',getdate(),'{3}','{4}',0,'{2}',getdate())", GetCode(), code, userName, isBills, BH_Remark);
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 修改营销成功 ,并且修改有效数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="custName"></param>
        /// <param name="custPhone"></param>
        /// <returns></returns>
        public bool UpSSBAlreadyUse(string code, string custName, string custPhone)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"UPDATE o SET o.OD_AlreadyUse = isnull(o.OD_AlreadyUse,0) + 1,
                        OD_State = '已使用'
                        FROM SSB_OriginalData o 
                        JOIN SSB_OriginalDataValid  d ON o.OD_Code = d.ODD_OD_Code
                        WHERE (ODD_Name = '{0}' OR ODD_Phone = '{1}') AND o.DataState = 0", custName, custPhone);
            return UpdateData(sb.ToString());
        }

        #region 审核匹配

        public DataTable GetSSBCustPhoneSimilarInfo(string code, string phone, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' AND c.Cust_LinkPhone='{3}' 

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_LinkPhone='{3}' 
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_LinkPhone='{3}'  

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM mqy_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_LinkPhone='{3}' 
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, phone);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetSSBCustTsPhoneSimilarInfo(string code, string phone, string billno, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' AND (c.Cust_TSPhone = '{3}' or c.Cust_TSPhone = '{4}') 

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c 
			                            WHERE c.DataState = 0 AND (c.Cust_TSPhone = '{3}' or c.Cust_TSPhone = '{4}')  
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c 
			                            WHERE c.DataState = 0 AND (c.Cust_TSPhone = '{3}' or c.Cust_TSPhone = '{4}')  

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM mqy_Customer c 
			                            WHERE c.DataState = 0 AND (c.Cust_TSPhone = '{3}' or c.Cust_TSPhone = '{4}') 
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, phone, billno);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetSSBCustAddressSimilarInfo(string code, string address, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}' 

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}' 
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}'  

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM mqy_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address='{3}' 
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, address);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetSSBBillNoSimilarInfo(string code, string billno, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' AND c.Cust_BillNumber='{3}' 

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_BillNumber='{3}' 
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_BillNumber='{3}' 

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM mqy_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_BillNumber='{3}' 
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, billno);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        public DataTable GetSSBNameContainsKeySimilarInfo(string code, string key, int pageindex, int pagesize)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"SELECT top {1}   a. *
                            FROM (
	                            SELECT row_number () OVER (ORDER BY JoinDate DESC) AS rownumber
	                                , b. *
	                            FROM (
			                            SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'实时保' as TableName FROM SSB_Customer c
			                            LEFT JOIN (SELECT * FROM SSB_Customer WHERE DataState = 0 AND Cust_Code = '{0}') temp ON 1 = 1
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' AND c.Cust_Name LIKE '%{3}%' 

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'维权通' as TableName FROM WQT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_Name LIKE '%{3}%'
                                             
                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'诚信通' as TableName FROM CXT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_Name LIKE '%{3}%'

                                        UNION SELECT c.DataState,c.JoinDate,c.Cust_Name,c.Cust_State,c.Cust_Code,c.Cust_NameKey,c.Cust_TSPhone,
                                                c.Cust_LinkPhone,c.Cust_BillNumber,c.Cust_Address,c.Cust_Phone,'民企云' as TableName FROM mqy_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_Name LIKE '%{3}%'
 		                            )b
	                            )a
                            WHERE a.rownumber > (0+({2}-1)*{1})", code, pagesize, pageindex, key);
            DataTable dts = SearchData(sb.ToString());
            return dts;
        }

        #endregion

        #region 审核匹配数量

        public string GetSSBKeySimilarCount(string code, string key)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code,1 Type FROM SSB_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_Name LIKE '%{1}%' 

                                        UNION select Cust_Code,2 Type FROM MQY_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_Name LIKE '%{1}%'  
                                         
                                        UNION select Cust_Code,3 Type FROM WQT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_Name LIKE '%{1}%'   

					                    UNION select Cust_Code,4 Type FROM cxt_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_Name LIKE '%{1}%'   
                                                )T
 		                 ", code, key);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetSSBBillNoSimilarCount(string code, string billno)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code,1 Type FROM SSB_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_BillNumber = '{1}' 

                                        UNION select Cust_Code,2 Type FROM MQY_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_BillNumber = '{1}'  
                                         
                                        UNION select Cust_Code,3 Type FROM WQT_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_BillNumber = '{1}'  

					                    UNION select Cust_Code,4 Type FROM cxt_Customer c 
			                            WHERE c.DataState = 0 AND c.Cust_BillNumber = '{1}'    
                                 )T
 		                 ", code, billno);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetSSBCustTsPhoneSimilarInfoCount(string code, string billno, string phoneNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code,1 Type FROM SSB_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' AND (c.Cust_TSPhone = '{1}' or c.Cust_TSPhone = '{2}')  

                                        UNION select Cust_Code,2 Type FROM MQY_Customer c
			                            WHERE c.DataState = 0  AND (c.Cust_TSPhone = '{1}' or c.Cust_TSPhone = '{2}')  
                                         
                                        UNION select Cust_Code,3 Type FROM WQT_Customer c 
			                            WHERE c.DataState = 0  AND (c.Cust_TSPhone = '{1}' or c.Cust_TSPhone = '{2}')  

					                    UNION select Cust_Code,4 Type FROM cxt_Customer c 
			                            WHERE c.DataState = 0  AND (c.Cust_TSPhone = '{1}' or c.Cust_TSPhone = '{2}')                                                   
                                 )T
 		                 ", code, billno, phoneNumber);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetSSBAddressSimilarCount(string code, string address)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code,1 Type FROM SSB_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'
                                        
                                        UNION select Cust_Code,2 Type FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'
                                         
                                        UNION select Cust_Code,3 Type FROM WQT_Customer c 
			                            WHERE c.DataState = 0  
					                          AND  c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'

					                    UNION select Cust_Code,4 Type FROM cxt_Customer c 
			                            WHERE c.DataState = 0  
					                          AND  c.Cust_ProvinceName+c.Cust_CityName+c.Cust_Address = '{1}'
                                                 )T
 		                 ", code, address);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetSSBPhoneSimilarCount(string code, string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT count(T.Cust_Code) AS total from(   
                                        SELECT Cust_Code,1 Type FROM SSB_Customer c
			                            WHERE c.DataState = 0 AND c.Cust_State IN ('已审','退订','退费')  
					                            AND c.Cust_Code != '{0}' 
					                            AND c.Cust_LinkPhone = '{1}'

                                        UNION SELECT Cust_Code,2 Type FROM MQY_Customer c
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'
                                         
                                        UNION SELECT Cust_Code,3 Type FROM WQT_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'

					                    UNION SELECT Cust_Code,4 Type FROM CXT_Customer c 
			                            WHERE c.DataState = 0  
					                            AND  c.Cust_LinkPhone = '{1}'
                                                )T
 		                 ", code, phone);
            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
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
