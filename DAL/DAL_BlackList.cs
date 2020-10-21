using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_BlackList : SqlBase
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public DataTable GetBlackList(string procinceName, string cityName, string phone, string Comment, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP(" + ValueHandler.GetIntNumberValue(PageNum) + ")* FROM(SELECT *,ROW_NUMBER() OVER (ORDER BY JoinDate DESC) AS 'Num' FROM YX_BlackList WHERE 1=1");
            if (procinceName != "")
                sb.Append(" AND BL_ProvinceName='" + ValueHandler.GetStringValue(procinceName) + "'");
            if (cityName != "")
                sb.Append(" AND BL_CityName ='" + ValueHandler.GetStringValue(cityName) + "'");
            if (ValueHandler.GetStringValue(phone) != "")
                sb.Append(" AND BL_Phone LIKE '%" + ValueHandler.GetStringValue(phone) + "%'");
            if (ValueHandler.GetStringValue(Comment) != "")
                sb.Append(" AND BL_Comment LIKE '%" + ValueHandler.GetStringValue(Comment) + "%'");
            sb.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1}) order by Num asc", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public DataTable GetNum(string procinceName, string cityName, string phone, string Comment, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COUNT(*) AS num FROM YX_BlackList WHERE 1=1");
            if (procinceName != "")
                sb.Append(" AND BL_ProvinceName='" + ValueHandler.GetStringValue(procinceName) + "'");
            if (cityName != "")
                sb.Append(" AND BL_CityName ='" + ValueHandler.GetStringValue(cityName) + "'");
            if (ValueHandler.GetStringValue(phone) != "")
                sb.Append(" AND BL_Phone LIKE '%" + ValueHandler.GetStringValue(phone) + "%'");
            if (ValueHandler.GetStringValue(Comment) != "")
                sb.Append(" AND BL_Comment LIKE '%" + ValueHandler.GetStringValue(Comment) + "%'");

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据code删除数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool DelBlackList(string code)
        {
            string str = "DELETE FROM YX_BlackList WHERE BL_Code='" + ValueHandler.GetStringValue(code) + "'";
            return UpdateData(str);
        }

        ///// <summary>
        ///// 是否是黑名单
        ///// </summary>
        ///// <param name="phone"></param>
        ///// <returns></returns>
        //public DataTable IsXFBBlackPhone(string code, string type)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string tableName = "XFB";
        //    if (type == "1")
        //        tableName = "WQT";
        //    else if (type == "2")
        //        tableName = "MQY";
        //    else if (type == "3")
        //        tableName = "CXT";
        //    sb.AppendFormat($@"SELECT b.*
        //                    FROM {tableName}_Customer c
        //                        JOIN YX_BlackList b ON c.Cust_BillNumber = b.BL_Phone AND b.DataState = 0
        //                    WHERE c.DataState = 0 AND c.Cust_Code = '{code}'");

        //    return SearchData(sb.ToString());
        //}

        /// <summary>
        /// 是否是黑名单
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public DataTable IsBlackPhone(string phone)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat($"SELECT * FROM YX_BlackList WHERE BL_Phone = '{phone}' AND DataState = 0");

            return SearchData(sb.ToString());
        }
    }
}
