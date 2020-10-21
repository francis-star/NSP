/////////////////////////////////////////////////////////////////////////////
//模块名：明细数据
//开发者：赵虎
//开发时间：2016年11月28日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.InMolde;
using DAL.Models;
using DAL.Service;
using HCWeb2016;
using Newtonsoft.Json;
using SqlSugar;

namespace DAL
{
    public class DAL_OriginalDataDts : SqlBase
    {
        #region 新消费宝典

        #region 查看数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetXFBViewData(string OD_Code, string type, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT *
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM XFB_OriginalDataDts
                                    WHERE ODD_Type = {2} AND ODD_OD_Code = '{1}'
                                      AND DataState = 0", PageNum, OD_Code, type);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetXFBViewDataCount(string OD_Code, string type)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM XFB_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}' AND ODD_Type = {1}", OD_Code, type);

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

        #endregion

        #region 黑名单数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetXFBBlackData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT d. *
                                        , b.BL_Comment AS RB_Provider 
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM XFB_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetXFBBlackDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM XFB_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{0}'", OD_Code);

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

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <param name="JoinMan">操作人</param>
        /// <returns></returns>
        public bool GeneralXFBDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("SP_PLScreenXFBOrignData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        /// <summary>
        /// 保存有效数据并更新状态、统计值信息
        /// </summary>
        /// <param name="OD_Code"></param>
        /// <param name="JoinMan"></param>
        /// <returns></returns>
        public bool SaveXFBGeneralDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("PL_SaveXFBOrignValidData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        #endregion

        #region 移动数据

        public bool MoveXFBScreenData(string[] arr, string type, string OD_Code, string JoinMan, string isKeyMove)
        {
            StringBuilder sqlStr = new StringBuilder();

            sqlStr.AppendFormat("\r UPDATE XFB_OriginalDataDts SET ODD_Type = {0}  ", type);
            if (type == "1" && isKeyMove == "1")//关键字移动到有效数据的数据
                sqlStr.Append(",IsMoveValidData=1");
            else
                sqlStr.Append(",IsMoveValidData=0");

            sqlStr.Append(" WHERE  ODD_Code IN ( ");

            for (int i = 0; i < arr.Length; i++)
            {
                sqlStr.Append("\r '" + ValueHandler.GetStringValue(arr[i]) + "' ,");
            }
            sqlStr.Remove(sqlStr.Length - 1, 1);
            sqlStr.Append("\r )");
            bool b = UpdateData(sqlStr.ToString());
            if (!b)
                return false;
            else
            {
                b = SaveXFBGeneralDatas(OD_Code, JoinMan);
            }
            return b;
        }

        #endregion

        #endregion

        #region 实时表

        #region 查看数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字低危 8 其他业务会员 9 高危关键字 10 其他会员退订 11无效数据 12客户相同退订</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetSSBViewData(string OD_Code, string type, string year, string yearTD, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            string yearStr = (type == "11" || type == "10") && year != "" ? $" AND year(AddDate)={year}" : "";
            string yearTDStr = type == "10" && yearTD != "" ? $" AND year(TDDate)={yearTD}" : "";
            string sql;
            switch (type)
            {
                case "10":
                    sql = $@"SELECT * FROM (SELECT d.ODD_Code
                                , d.ODD_Name
                                , d.ODD_Phone
                                , d.ODD_Business
                                , CASE d.ODD_Business WHEN '诚信通' THEN c2.JoinDate WHEN '维权通' THEN c3.JoinDate ELSE c4.JoinDate END AddDate
                                , CASE d.ODD_Business WHEN '诚信通' THEN isnull(c2.Cust_OutDate, c2.Cust_UnOrder) WHEN '维权通' THEN isnull(c3.Cust_OutDate, c3.Cust_UnOrder) ELSE isnull(c4.Cust_OutDate, c4.Cust_UnOrder) END TDDate
                                , ROW_NUMBER () OVER (ORDER BY IsMoveData DESC, ODD_Name) AS 'Num'
                            FROM SSB_OriginalDataDts d
                                LEFT JOIN CXT_Customer c2 ON d.ODD_Phone = c2.Cust_BillNumber AND c2.DataState = 0 AND c2.Cust_State  IN ('退订','退费') AND d.ODD_Business = '诚信通'
                                LEFT JOIN WQT_Customer c3 ON d.ODD_Phone = c3.Cust_BillNumber AND c3.DataState = 0 AND c3.Cust_State  IN ('退订','退费') AND d.ODD_Business = '维权通'
                                LEFT JOIN MQY_Customer c4 ON d.ODD_Phone = c4.Cust_BillNumber AND c4.DataState = 0 AND c4.Cust_State  IN ('退订','退费') AND d.ODD_Business = '民企云'
                            WHERE ODD_OD_Code = '{OD_Code}' AND d.DataState = 0 AND ODD_Type = 10)t WHERE 1=1 {yearStr} {yearTDStr}";
                    break;
                case "11"://无效 时间
                    sql = $@"SELECT * FROM (SELECT d.ODD_Code
                                , d.ODD_Name
                                , d.ODD_Phone
                                , d.ODD_Business
                                , CASE d.ODD_Business WHEN '实时保' THEN c1.JoinDate WHEN '诚信通' THEN c2.JoinDate WHEN '维权通' THEN c3.JoinDate ELSE c4.JoinDate END AddDate
                                , ROW_NUMBER () OVER (ORDER BY IsMoveData DESC, ODD_Name) AS 'Num'
                            FROM SSB_OriginalDataDts d
                                LEFT JOIN SSB_Customer c1 ON d.ODD_Phone = c1.Cust_BillNumber AND c1.DataState = 0 AND c1.Cust_State = '无效' AND d.ODD_Business = '实时保'
                                LEFT JOIN CXT_Customer c2 ON d.ODD_Phone = c2.Cust_BillNumber AND c2.DataState = 0 AND c2.Cust_State = '无效' AND d.ODD_Business = '诚信通'
                                LEFT JOIN WQT_Customer c3 ON d.ODD_Phone = c3.Cust_BillNumber AND c3.DataState = 0 AND c3.Cust_State = '无效' AND d.ODD_Business = '维权通'
                                LEFT JOIN MQY_Customer c4 ON d.ODD_Phone = c4.Cust_BillNumber AND c4.DataState = 0 AND c4.Cust_State = '无效' AND d.ODD_Business = '民企云'
                            WHERE ODD_OD_Code = '{OD_Code}' AND d.DataState = 0 AND ODD_Type = 11)T WHERE 1=1 {yearStr}";
                    break;
                default:
                    sql = $@"SELECT d.ODD_Code
                            , d.ODD_Name
                            , d.ODD_Phone
                            , d.ODD_Business
                            , ROW_NUMBER () OVER (ORDER BY IsMoveData DESC, ODD_Name) AS 'Num'
                        FROM SSB_OriginalDataDts d
                        WHERE ODD_OD_Code = '{OD_Code}' AND d.DataState = 0 AND ODD_Type = {type}";
                    break;
            }

            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM ({1}", PageNum, sql);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        public string GetInvalidSSBViewData(InOriginViewModle model)
        {
            var db = DbService.Instance;
            int invalidTotalCount = 0;
            var invalidDataList = db.Queryable<SSB_OriginalDataDts>().Where(it => it.ODD_OD_Code == model.ODCode && it.ODD_Type != "1")
                .WhereIF(!string.IsNullOrEmpty(model.ApprovedUser), it => it.ApprovedUser == model.ApprovedUser)
                .WhereIF(!string.IsNullOrEmpty(model.BlackList), it => it.BlackList == model.BlackList)
                .WhereIF(!string.IsNullOrEmpty(model.BusinessName), it => it.ODD_Business == model.BusinessName)
                .WhereIF(!string.IsNullOrEmpty(model.ChargeNumber), it => it.ODD_Phone == model.ChargeNumber)
                .WhereIF(!string.IsNullOrEmpty(model.CustName), it => it.ODD_Name == model.CustName)
                .WhereIF(!string.IsNullOrEmpty(model.DuplicateData), it => it.DuplicateData == model.DuplicateData)
                .WhereIF(!string.IsNullOrEmpty(model.IsCharge), it => it.ODD_IsBill == model.IsCharge)
                .WhereIF(!string.IsNullOrEmpty(model.Keywords_high), it => it.Keywords_high == model.Keywords_high)
                .WhereIF(!string.IsNullOrEmpty(model.Keywords_low), it => it.Keywords_low == model.Keywords_low)
                .WhereIF(!string.IsNullOrEmpty(model.LinkPhone), it => !string.IsNullOrEmpty(it.ODD_LinkPhone))
                .WhereIF(!string.IsNullOrEmpty(model.OpenDate), it => it.OpenDate == SqlFunc.ToDate(model.OpenDate))
                .WhereIF(!string.IsNullOrEmpty(model.UnsubscribeTime), it => it.UnsubscribeTime == SqlFunc.ToDate(model.UnsubscribeTime))
                .WhereIF(!string.IsNullOrEmpty(model.RefundUser), it => it.RefundUser == model.RefundUser)
                .WhereIF(!string.IsNullOrEmpty(model.RepetitionType), it => it.RepetitionType == model.RepetitionType)
                .WhereIF(!string.IsNullOrEmpty(model.TSNature), it => it.TSNature == model.TSNature)
                .WhereIF(!string.IsNullOrEmpty(model.TSNumber), it => it.TSNumber == model.TSNumber)
                .WhereIF(!string.IsNullOrEmpty(model.TSSource), it => it.TSSource == model.TSSource)
                .WhereIF(!string.IsNullOrEmpty(model.UnsubscribedUser), it => it.UnsubscribedUser == model.UnsubscribedUser)
                .ToPageList(model.PageIndex, model.PageNum, ref invalidTotalCount);

            var validTotalCount = db.Queryable<SSB_OriginalDataDts>().Where(it => it.ODD_OD_Code == model.ODCode && it.ODD_Type == "1").Count();
            var totalCount = db.Queryable<SSB_OriginalDataDts>().Where(it => it.ODD_OD_Code == model.ODCode).Count();
            var result = new
            {
                invalidTotalCount = invalidTotalCount,
                invalidDataList = invalidDataList,
                validTotalCount = validTotalCount,
                totalCount = totalCount
            };
            return JsonConvert.SerializeObject(result);
        }

        public string GetValidSSBViewData(InOriginViewModle model)
        {
            var db = DbService.Instance;

            int validTotalCount = 0;
            var validDataList = db.Queryable<SSB_OriginalDataDts>().Where(it => it.ODD_OD_Code == model.ODCode && it.ODD_Type == "1")
                                .ToPageList(model.PageIndex, model.PageNum, ref validTotalCount);

            var totalCount = db.Queryable<SSB_OriginalDataDts>().Where(it => it.ODD_OD_Code == model.ODCode).Count();
            var result = new
            {
                validDataList = validDataList,
                validTotalCount = validTotalCount,
                totalCount = totalCount
            };
            return JsonConvert.SerializeObject(result);

        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字低危 8 其他业务会员 9 高危关键字 10 其他会员退订 11无效数据 12客户相同退订</param>
        /// <returns></returns>
        public string GetSSBViewDataCount(string OD_Code, string type, string year, string yearTD)
        {
            string yearStr = (type == "11" || type == "10") && year != "" ? $" AND year(AddDate)={year}" : "";
            string yearTDStr = type == "10" && yearTD != "" ? $" AND year(TDDate)={yearTD}" : "";
            string sql;
            switch (type)
            {
                case "10":
                    sql = $@"SELECT count(*) DataCount
                                    FROM (SELECT d.ODD_Code
                                              , CASE d.ODD_Business WHEN '诚信通' THEN c2.JoinDate WHEN '维权通' THEN c3.JoinDate ELSE c4.JoinDate END AddDate
                                , CASE d.ODD_Business WHEN '诚信通' THEN isnull(c2.Cust_OutDate, c2.Cust_UnOrder) WHEN '维权通' THEN isnull(c3.Cust_OutDate, c3.Cust_UnOrder) ELSE isnull(c4.Cust_OutDate, c4.Cust_UnOrder) END TDDate
                                          FROM SSB_OriginalDataDts d
                                              LEFT JOIN CXT_Customer c2 ON d.ODD_Phone = c2.Cust_BillNumber AND c2.DataState = 0 AND c2.Cust_State IN ('退订', '退费') AND d.ODD_Business = '诚信通'
                                              LEFT JOIN WQT_Customer c3 ON d.ODD_Phone = c3.Cust_BillNumber AND c3.DataState = 0 AND c3.Cust_State IN ('退订', '退费') AND d.ODD_Business = '维权通'
                                              LEFT JOIN MQY_Customer c4 ON d.ODD_Phone = c4.Cust_BillNumber AND c4.DataState = 0 AND c4.Cust_State IN ('退订', '退费') AND d.ODD_Business = '民企云'
                                          WHERE ODD_OD_Code = '{OD_Code}' AND d.DataState = 0 AND ODD_Type = 10) t
                                    WHERE 1= 1 {yearStr} {yearTDStr}";
                    break;
                case "11":
                    sql = $@"SELECT count(*) DataCount
                                    FROM (SELECT d.ODD_Code
                                              , CASE d.ODD_Business WHEN '实时保' THEN c1.JoinDate WHEN '诚信通' THEN c2.JoinDate WHEN '维权通' THEN c3.JoinDate ELSE c4.JoinDate END AddDate
                                          FROM SSB_OriginalDataDts d
                                              LEFT JOIN SSB_Customer c1 ON d.ODD_Phone = c1.Cust_BillNumber AND c1.DataState = 0 AND c1.Cust_State = '无效' AND d.ODD_Business = '实时保'
                                              LEFT JOIN CXT_Customer c2 ON d.ODD_Phone = c2.Cust_BillNumber AND c2.DataState = 0 AND c2.Cust_State = '无效' AND d.ODD_Business = '诚信通'
                                              LEFT JOIN WQT_Customer c3 ON d.ODD_Phone = c3.Cust_BillNumber AND c3.DataState = 0 AND c3.Cust_State = '无效' AND d.ODD_Business = '维权通'
                                              LEFT JOIN MQY_Customer c4 ON d.ODD_Phone = c4.Cust_BillNumber AND c4.DataState = 0 AND c4.Cust_State = '无效' AND d.ODD_Business = '民企云'
                                          WHERE ODD_OD_Code = '{OD_Code}' AND d.DataState = 0 AND ODD_Type = 11) t
                                    WHERE 1= 1 {yearStr}";
                    break;
                default:
                    sql = $@"SELECT count(*) DataCount
                        FROM SSB_OriginalDataDts d
                        WHERE ODD_OD_Code = '{OD_Code}' AND d.DataState = 0 AND ODD_Type = {type}";
                    break;
            }

            DataTable dt = SearchData(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        public DataTable GetTotalCount(string odCoode)
        {
            return SearchData($@"SELECT t. *
                                        , isnull (m.count, 0) count
                                        , (SELECT count ( *)
                                           FROM SSB_OriginalDataDts d
                                           WHERE d.ODD_OD_Code = '{odCoode}' AND d.DataState = 0) Total
                                    FROM (SELECT 1 Type UNION
                                        SELECT 2 UNION
                                        SELECT 3 UNION
                                        SELECT 4 UNION
                                        SELECT 5 UNION
                                        SELECT 6 UNION
                                        SELECT 7 UNION
                                        SELECT 8 UNION
                                        SELECT 9 UNION
                                        SELECT 10 UNION
                                        SELECT 11 UNION
                                        SELECT 12 UNION
                                        SELECT 21 UNION
                                        SELECT 22) t
                                        LEFT JOIN (SELECT count ( *) count
                                                       , d.ODD_Type
                                                   FROM SSB_OriginalDataDts d
                                                   WHERE d.ODD_OD_Code = '{odCoode}'
                                                   GROUP BY d.ODD_Type UNION ALL
                                        SELECT count ( *) count
                                            , 21 ODD_Type
                                        FROM SSB_OriginalDataDts d
                                        WHERE d.ODD_OD_Code = '{odCoode}' AND d.ODD_Type IN (4, 7, 10, 11, 12) UNION ALL
                                        SELECT count ( *) count
                                            , 22 ODD_Type
                                        FROM SSB_OriginalDataDts d
                                        WHERE d.ODD_OD_Code = '{odCoode}' AND d.ODD_Type IN (2, 3, 5, 6, 8, 9)) m ON m.ODD_Type = t.type
                                    ORDER BY t.type");
        }

        public DataTable GetNoValidYearData(string odCode)
        {
            return SearchData($@"Select * from (SELECT '全部' year
                                        , 1 type
                                    UNION SELECT DISTINCT CAST (year (AddDate) AS NVARCHAR),2
                                    FROM SSB_OriginalDataDts d
                                        JOIN (SELECT c.JoinDate AddDate
                                                    , Cust_BillNumber
                                                FROM SSB_Customer c
                                                WHERE c.DataState = 0 AND c.Cust_State = '无效' UNION
                                        SELECT c.JoinDate
                                            , Cust_BillNumber
                                        FROM WQT_Customer c
                                        WHERE c.DataState = 0 AND c.Cust_State = '无效' UNION
                                        SELECT c.JoinDate
                                            , Cust_BillNumber
                                        FROM MQY_Customer c
                                        WHERE c.DataState = 0 AND c.Cust_State = '无效' UNION
                                        SELECT c.JoinDate
                                            , Cust_BillNumber
                                        FROM CXT_Customer c
                                        WHERE c.DataState = 0 AND c.Cust_State = '无效') c ON d.ODD_Phone = c.Cust_BillNumber
                                    WHERE d.ODD_OD_Code = '{odCode}' AND DataState = 0 AND d.ODD_Type = 11) t where t.year IS NOT NULL ORDER BY type");
        }

        public DataTable GetOtherTDYearData(string odCode)
        {
            return SearchData($@"SELECT *
                                FROM (SELECT '全部' year
                                , 1 type
                                UNION SELECT DISTINCT CAST (year (AddDate) AS NVARCHAR),2
                                FROM SSB_OriginalDataDts d
                                    JOIN (SELECT c.JoinDate AddDate
                                        , Cust_BillNumber
                                    FROM WQT_Customer c
                                    WHERE c.DataState = 0 AND c.Cust_State IN ('退订','退费') UNION
                                    SELECT c.JoinDate
                                        , Cust_BillNumber
                                    FROM MQY_Customer c
                                    WHERE c.DataState = 0 AND c.Cust_State IN ('退订','退费') UNION
                                    SELECT c.JoinDate
                                        , Cust_BillNumber
                                    FROM CXT_Customer c
                                    WHERE c.DataState = 0 AND c.Cust_State IN ('退订','退费')) c ON d.ODD_Phone = c.Cust_BillNumber
                                WHERE d.ODD_OD_Code = '{odCode}' AND DataState = 0 AND d.ODD_Type = 10 )t where t.year IS NOT NULL ORDER BY type");
        }

        public DataTable GetOtherTDSJYearData(string odCode)
        {
            return SearchData($@"SELECT *
                                FROM (SELECT '全部' year
                                          , 1 type UNION
                                    SELECT DISTINCT CAST (year (AddDate) AS NVARCHAR)
                                        , 2
                                    FROM SSB_OriginalDataDts d
                                        JOIN (SELECT isnull (c.Cust_OutDate, Cust_UnOrder) AddDate
                                                  , Cust_BillNumber
                                              FROM WQT_Customer c
                                              WHERE c.DataState = 0 AND c.Cust_State IN ('退订', '退费') UNION
                                        SELECT isnull (c.Cust_OutDate, Cust_UnOrder)
                                            , Cust_BillNumber
                                        FROM MQY_Customer c
                                        WHERE c.DataState = 0 AND c.Cust_State IN ('退订', '退费') UNION
                                        SELECT isnull (c.Cust_OutDate, Cust_UnOrder)
                                            , Cust_BillNumber
                                        FROM CXT_Customer c
                                        WHERE c.DataState = 0 AND c.Cust_State IN ('退订', '退费')) c ON d.ODD_Phone = c.Cust_BillNumber
                                    WHERE d.ODD_OD_Code = '{odCode}' AND DataState = 0 AND d.ODD_Type = 10) t
                                WHERE t.year IS NOT NULL ORDER BY type");
        }

        #endregion

        #region 黑名单数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetSSBBlackData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT d. *
                                        , b.BL_Comment AS RB_Provider 
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM SSB_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetSSBBlackDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM SSB_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{0}'", OD_Code);

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

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <param name="JoinMan">操作人</param>
        /// <returns></returns>
        public bool GeneralSSBDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("SP_PLScreenSSBOrignData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        /// <summary>
        /// 保存有效数据并更新状态、统计值信息
        /// </summary>
        /// <param name="OD_Code"></param>
        /// <param name="JoinMan"></param>
        /// <returns></returns>
        public bool SaveSSBGeneralDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("PL_SaveSSBOrignValidData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        #endregion

        #region 移动数据

        public bool MoveSSBScreenData(string[] arr, string type, string OD_Code, string JoinMan, string isKeyMove)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("\r UPDATE SSB_OriginalDataDts SET ODD_Type = {0}  ", type);
            if (type == "1" && isKeyMove == "1")//关键字移动到有效数据的数据
                sqlStr.Append(",IsMoveValidData = 1");
            else
                sqlStr.Append(",IsMoveValidData = 0");

            sqlStr.Append(",IsMoveData = 1");
            sqlStr.Append(" WHERE ODD_Code IN ( ");

            for (int i = 0; i < arr.Length; i++)
            {
                sqlStr.Append("\r '" + ValueHandler.GetStringValue(arr[i]) + "' ,");
            }
            sqlStr.Remove(sqlStr.Length - 1, 1);
            sqlStr.Append("\r )");
            bool b = UpdateData(sqlStr.ToString());
            if (!b)
                return false;
            else
            {
                b = SaveSSBGeneralDatas(OD_Code, JoinMan);
            }
            return b;
        }

        #endregion

        #endregion

        #region 诚信通

        #region 查看数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetViewData(string OD_Code, string type, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT *
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM CXT_OriginalDataDts
                                    WHERE ODD_Type = {2} AND ODD_OD_Code = '{1}'
                                      AND DataState = 0", PageNum, OD_Code, type);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetViewDataCount(string OD_Code, string type)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM CXT_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}' AND ODD_Type = {1}", OD_Code, type);

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

        #endregion

        #region 黑名单数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetBlackData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT d. *
                                        , b.BL_Comment AS RB_Provider 
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM CXT_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetBlackDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM CXT_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{0}'", OD_Code);

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

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <param name="JoinMan">操作人</param>
        /// <returns></returns>
        public bool GeneralDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("SP_PLScreenOrignData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        /// <summary>
        /// 保存有效数据并更新状态、统计值信息
        /// </summary>
        /// <param name="OD_Code"></param>
        /// <param name="JoinMan"></param>
        /// <returns></returns>
        public bool SaveGeneralDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("PL_SaveOrignValidData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        #endregion

        #region 移动数据

        public bool MoveScreenData(string[] arr, string type, string OD_Code, string JoinMan, string isKeyMove)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("\r UPDATE CXT_OriginalDataDts SET ODD_Type = {0}  ", type);
            if (type == "1" && isKeyMove == "1")//关键字移动到有效数据的数据
                sqlStr.Append(",IsMoveValidData=1");
            else
                sqlStr.Append(",IsMoveValidData=0");

            sqlStr.Append(" WHERE  ODD_Code IN ( ");

            for (int i = 0; i < arr.Length; i++)
            {
                sqlStr.Append("\r '" + ValueHandler.GetStringValue(arr[i]) + "' ,");
            }
            sqlStr.Remove(sqlStr.Length - 1, 1);
            sqlStr.Append("\r )");
            bool b = UpdateData(sqlStr.ToString());
            if (!b)
                return false;
            else
            {
                b = SaveGeneralDatas(OD_Code, JoinMan);
            }
            return b;
        }

        #endregion

        #endregion

        #region 民企云

        #region 查看数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetMQYViewData(string OD_Code, string type, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT *
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM MQY_OriginalDataDts
                                    WHERE ODD_Type = {2} AND ODD_OD_Code = '{1}'
                                      AND DataState = 0", PageNum, OD_Code, type);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetMQYViewDataCount(string OD_Code, string type)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM MQY_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}' AND ODD_Type = {1}", OD_Code, type);

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

        #endregion

        #region 黑名单数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetMQYBlackData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT d. *
                                        , b.BL_Comment AS RB_Provider 
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM MQY_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetMQYBlackDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM MQY_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{0}'", OD_Code);

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

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <param name="JoinMan">操作人</param>
        /// <returns></returns>
        public bool GeneralMQYDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("SP_PLScreenMQYOrignData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        /// <summary>
        /// 保存有效数据并更新状态、统计值信息
        /// </summary>
        /// <param name="OD_Code"></param>
        /// <param name="JoinMan"></param>
        /// <returns></returns>
        public bool SaveMQYGeneralDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("PL_SaveMQYOrignValidData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        #endregion

        #region 移动数据

        public bool MoveMQYScreenData(string[] arr, string type, string OD_Code, string JoinMan, string isKeyMove)
        {
            StringBuilder sqlStr = new StringBuilder();

            sqlStr.AppendFormat("\r UPDATE MQY_OriginalDataDts SET ODD_Type = {0}  ", type);
            if (type == "1" && isKeyMove == "1")//关键字移动到有效数据的数据
                sqlStr.Append(",IsMoveValidData=1");
            else
                sqlStr.Append(",IsMoveValidData=0");

            sqlStr.Append(" WHERE  ODD_Code IN ( ");

            for (int i = 0; i < arr.Length; i++)
            {
                sqlStr.Append("\r '" + ValueHandler.GetStringValue(arr[i]) + "' ,");
            }
            sqlStr.Remove(sqlStr.Length - 1, 1);
            sqlStr.Append("\r )");
            bool b = UpdateData(sqlStr.ToString());
            if (!b)
                return false;
            else
            {
                b = SaveMQYGeneralDatas(OD_Code, JoinMan);
            }
            return b;
        }

        #endregion

        #endregion

        #region 维权通

        #region 查看数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetWQTViewData(string OD_Code, string type, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT *
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM WQT_OriginalDataDts
                                    WHERE ODD_Type = {2} AND ODD_OD_Code = '{1}'
                                      AND DataState = 0", PageNum, OD_Code, type);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetWQTViewDataCount(string OD_Code, string type)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM WQT_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}' AND ODD_Type = {1}", OD_Code, type);

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

        #endregion

        #region 黑名单数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public DataTable GetWQTBlackData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT d. *
                                        , b.BL_Comment AS RB_Provider 
                                        , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                    FROM WQT_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取数据总数
        /// </summary>
        /// <param name="OD_Code">原始数据code</param>
        /// <param name="type">1 有效数据 2 已是会员 3 退订用户 4 已呼过 5 重复数据 6 黑名单 7 关键字 8 其他业务会员</param>
        /// <returns></returns>
        public string GetWQTBlackDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM WQT_OriginalDataDts d
                                        JOIN YX_BlackList b ON d.ODD_Phone = b.BL_Phone AND b.DataState = 0
                                    WHERE d.DataState = 0 AND ODD_Type = 6 AND d.ODD_OD_Code = '{0}'", OD_Code);

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

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <param name="JoinMan">操作人</param>
        /// <returns></returns>
        public bool GeneralWQTDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("SP_PLScreenWQTOrignData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        /// <summary>
        /// 保存有效数据并更新状态、统计值信息
        /// </summary>
        /// <param name="OD_Code"></param>
        /// <param name="JoinMan"></param>
        /// <returns></returns>
        public bool SaveWQTGeneralDatas(string OD_Code, string JoinMan)
        {
            bool flag = false;
            int result;
            flag = DAL_SqlBase.ExcuteNonQuery_Sp("PL_SaveWQTOrignValidData", new SqlParameter[] {
                        new SqlParameter("@OD_Code",OD_Code),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        #endregion

        #region 移动数据

        public bool MoveWQTScreenData(string[] arr, string type, string OD_Code, string JoinMan, string isKeyMove)
        {
            StringBuilder sqlStr = new StringBuilder();

            sqlStr.AppendFormat("\r UPDATE WQT_OriginalDataDts SET ODD_Type = {0}  ", type);
            if (type == "1" && isKeyMove == "1")//关键字移动到有效数据的数据
                sqlStr.Append(",IsMoveValidData=1");
            else
                sqlStr.Append(",IsMoveValidData=0");

            sqlStr.Append(" WHERE  ODD_Code IN ( ");

            for (int i = 0; i < arr.Length; i++)
            {
                sqlStr.Append("\r '" + ValueHandler.GetStringValue(arr[i]) + "' ,");
            }
            sqlStr.Remove(sqlStr.Length - 1, 1);
            sqlStr.Append("\r )");
            bool b = UpdateData(sqlStr.ToString());
            if (!b)
                return false;
            else
            {
                b = SaveWQTGeneralDatas(OD_Code, JoinMan);
            }
            return b;
        }

        #endregion

        #endregion
    }
}
