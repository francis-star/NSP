using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_AllQuery : SqlBase
    {
        public DataTable GetCustomer(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();

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
                sb.Append("AND Cust_State IN('已审','退订','退费','无效')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
                sb.Append(" AND Cust_WH_UserName LIKE '%" + ValueHandler.GetStringValue(cust_WH_UserName) + "%'");
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

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT TOP {0} *
                                        , (isnull (Cust_ProvinceName, '') + isnull (Cust_CityName, '') + isnull (Cust_CountyName, '')) AS 'Cust_Area'
                                    FROM (SELECT *
                                              , ROW_NUMBER () OVER (ORDER BY JoinDate DESC) AS 'Num'
                                          FROM (SELECT Cust_Name,JoinDate,Cust_Phone,Cust_BillNumber,Cust_Linkman,Cust_LinkPhone,Cust_WH_UserName,Cust_State,Cust_OpenDate,Cust_UnOrder,Cust_DealPerson,Cust_OutDate,Cust_Address,Cust_ProvinceName,Cust_CityName,Cust_CountyName,Cust_Code
                                                    , '维权通' AS Name
                                                    , 1 AS Type
                                                FROM WQT_Customer
                                                WHERE 1 = 1 {1} UNION
                                              SELECT Cust_Name,JoinDate,Cust_Phone,Cust_BillNumber,Cust_Linkman,Cust_LinkPhone,Cust_WH_UserName,Cust_State,Cust_OpenDate,Cust_UnOrder,Cust_DealPerson,Cust_OutDate,Cust_Address,Cust_ProvinceName,Cust_CityName,Cust_CountyName,Cust_Code
                                                  , '民企云' AS Name
                                                  , 2 AS Type
                                              FROM MQY_Customer
                                              WHERE 1 = 1 {1} UNION
                                              SELECT Cust_Name,JoinDate,Cust_Phone,Cust_BillNumber,Cust_Linkman,Cust_LinkPhone,Cust_WH_UserName,Cust_State,Cust_OpenDate,Cust_UnOrder,Cust_DealPerson,Cust_OutDate,Cust_Address,Cust_ProvinceName,Cust_CityName,Cust_CountyName,Cust_Code
                                                  , '实时保' AS Name
                                                  , 5 AS Type
                                              FROM SSB_Customer
                                              WHERE 1 = 1 {1} UNION
                                              SELECT Cust_Name,JoinDate,Cust_Phone,Cust_BillNumber,Cust_Linkman,Cust_LinkPhone,Cust_WH_UserName,Cust_State,Cust_OpenDate,Cust_UnOrder,Cust_DealPerson,Cust_OutDate,Cust_Address,Cust_ProvinceName,Cust_CityName,Cust_CountyName,Cust_Code
                                                  , '诚信通' AS Name
                                                  , 3 AS Type
                                              FROM CXT_Customer
                                              WHERE 1 = 1 {1} ) t) t
                                    WHERE T.Num > (0+({2}-1)*{0})
                                    ORDER BY 'Num'", PageNum, sb.ToString(), PageIndex);
            /*XFB
              UNION  
                                                SELECT Cust_Name,JoinDate,Cust_Phone,Cust_BillNumber,Cust_Linkman,Cust_LinkPhone,Cust_WH_UserName,Cust_State,Cust_OpenDate,Cust_UnOrder,Cust_DealPerson,Cust_OutDate,Cust_Address,Cust_ProvinceName,Cust_CityName,Cust_CountyName,Cust_Code
                                                  , '新消费宝典' AS Name
                                                  , 4 AS Type
                                              FROM XFB_Customer
                                              WHERE 1 = 1 {1} */
            return SearchData(str.ToString());
        }
        public string GetValidDataCount(string cust_Name, string cust_Tel, string cust_Phone)
        {
            StringBuilder sb = new StringBuilder();
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND ODD_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Tel) != "")
                sb.Append(" AND ODD_Phone LIKE '%" + ValueHandler.GetStringValue(cust_Tel) + "%'");
            if (ValueHandler.GetStringValue(cust_Phone) != "")
                sb.Append(" AND ODD_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_Phone) + "%'");
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select sum(Name) from (
                                SELECT Count(1) AS Name 
                                FROM WQT_OriginalDataValid WHERE 1 = 1 {0}
                                UNION SELECT Count(1) AS Name 
                                FROM MQY_OriginalDataValid WHERE 1 = 1 {0}
                                UNION SELECT Count(1) AS Name 
                                FROM SSB_OriginalDataValid WHERE 1 = 1 {0}
                                UNION SELECT Count(1) AS Name 
                                FROM CXT_OriginalDataValid WHERE 1 = 1 {0})t where 1=1", sb.ToString());

            /*XFB
              UNION SELECT  Count(1) AS Name 
                                FROM    XFB_OriginalDataValid WHERE   1 = 1 {0}
             */
            DataTable dt = SearchData(str.ToString());
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return "0";
        }

        public DataTable GetTSSheetData(string cust_Name, string cust_Phone, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Phone) != "")
                sb.Append(" AND Cust_Phone LIKE '%" + ValueHandler.GetStringValue(cust_Phone) + "%'");

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT TOP {0} *
                                    FROM (SELECT *
                                              , ROW_NUMBER () OVER (ORDER BY ID DESC) AS 'Num'
                                          FROM (Select * from TS_Sheet
                                              WHERE 1 = 1 {1} ) t) t
                                    WHERE T.Num > (0+({2}-1)*{0})
                                    ORDER BY 'Num'", PageNum, sb.ToString(), PageIndex);
            return SearchData(str.ToString());
        }

        public DataTable GetValidData(string cust_Name, string cust_Tel, string cust_Phone, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND ODD_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_Tel) != "")
                sb.Append(" AND ODD_Phone LIKE '%" + ValueHandler.GetStringValue(cust_Tel) + "%'");
            if (ValueHandler.GetStringValue(cust_Phone) != "")
                sb.Append(" AND ODD_LinkPhone LIKE '%" + ValueHandler.GetStringValue(cust_Phone) + "%'");
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT TOP {0} *
                                    FROM (SELECT *
                                              , ROW_NUMBER () OVER (ORDER BY ODD_Code DESC) AS 'Num'
                                          FROM (SELECT ODD_Name,ODD_Phone,ODD_Address,ODD_LinkMan,ODD_LinkPhone,ODD_IsBill,ODD_Code
                                                    , '维权通' AS Name
                                                    , 1 AS Type
                                                FROM WQT_OriginalDataValid
                                                WHERE 1 = 1 {1} UNION
                                              SELECT ODD_Name,ODD_Phone,ODD_Address,ODD_LinkMan,ODD_LinkPhone,ODD_IsBill,ODD_Code
                                                  , '民企云' AS Name
                                                  , 2 AS Type
                                              FROM MQY_OriginalDataValid
                                              WHERE 1 = 1 {1} UNION
                                              SELECT ODD_Name,ODD_Phone,ODD_Address,ODD_LinkMan,ODD_LinkPhone,ODD_IsBill,ODD_Code
                                                  , '实时保' AS Name
                                                  , 5 AS Type
                                              FROM SSB_OriginalDataValid
                                              WHERE 1 = 1 {1} UNION
                                              SELECT ODD_Name,ODD_Phone,ODD_Address,ODD_LinkMan,ODD_LinkPhone,ODD_IsBill,ODD_Code
                                                  , '诚信通' AS Name
                                                  , 3 AS Type
                                              FROM CXT_OriginalDataValid
                                              WHERE 1 = 1 {1} ) t) t
                                    WHERE T.Num > (0+({2}-1)*{0})", PageNum, sb.ToString(), PageIndex);

            /*XFB
             * UNION
                                              SELECT ODD_Name,ODD_Phone,ODD_Address,ODD_LinkMan,ODD_LinkPhone,ODD_IsBill,ODD_Code
                                                  , '新消费宝典' AS Name
                                                  , 4 AS Type
                                              FROM XFB_OriginalDataValid
                                              WHERE 1 = 1 {1} 
             */
            return SearchData(str.ToString());
        }

        public string GetTSSheetDataCount(string cust_Name, string cust_LinkPhone)
        {
            StringBuilder sb = new StringBuilder();
            if (ValueHandler.GetStringValue(cust_Name) != "")
                sb.Append(" AND Cust_Name LIKE '%" + ValueHandler.GetStringValue(cust_Name) + "%'");
            if (ValueHandler.GetStringValue(cust_LinkPhone) != "")
                sb.Append(" AND Cust_Phone LIKE '%" + ValueHandler.GetStringValue(cust_LinkPhone) + "%'");
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT count (*) AS DataCount from TS_Sheet where 1=1 {0} ", sb.ToString());
            DataTable dt = SearchData(str.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetCustomerCount(string cust_Name, string cust_Linkman, string cust_LinkPhone, string Province, string city, string cust_State, string cust_BillNumber, string cust_IsBill, string cust_WH_UserName,
                                 string staJoinDate, string endJoinDate, string cust_UnOrder, string endcust_UnOrder, string cust_OutDate, string endcust_OutDate)
        {

            StringBuilder sb = new StringBuilder();

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
                sb.Append("AND Cust_State IN('已审','退订','退费','无效')");
            }
            if (ValueHandler.GetStringValue(cust_BillNumber) != "")
                sb.Append(" AND Cust_BillNumber LIKE '%" + ValueHandler.GetStringValue(cust_BillNumber) + "%'");
            if (ValueHandler.GetStringValue(cust_IsBill) != "")
                sb.Append(" AND Cust_IsBill='" + ValueHandler.GetStringValue(cust_IsBill) + "'");
            if (ValueHandler.GetStringValue(cust_WH_UserName) != "")
                sb.Append(" AND Cust_WH_UserName LIKE '%" + ValueHandler.GetStringValue(cust_WH_UserName) + "%'");
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

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT count (*) AS DataCount
                                          FROM (SELECT Cust_Code
                                                    , '维权通' AS Name
                                                    , 1 AS Type
                                                FROM WQT_Customer
                                                WHERE 1 = 1 {0} UNION
                                              SELECT Cust_Code
                                                  , '名企云' AS Name
                                                  , 2 AS Type
                                              FROM MQY_Customer
                                              WHERE 1 = 1 {0} UNION 
                                                SELECT Cust_Code
                                                  , '实时保' AS Name
                                                  , 5 AS Type
                                              FROM SSB_Customer
                                              WHERE 1 = 1 {0} UNION
                                              SELECT Cust_Code
                                                  , '诚信通' AS Name
                                                  , 3 AS Type
                                              FROM CXT_Customer
                                              WHERE 1 = 1 {0} ) t", sb.ToString());

            /*XFB
              UNION 
                                                SELECT Cust_Code
                                                  , '新消费宝典' AS Name
                                                  , 4 AS Type
                                              FROM XFB_Customer
                                              WHERE 1 = 1 {0}
                                              */
            DataTable dt = SearchData(str.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }
    }
}
