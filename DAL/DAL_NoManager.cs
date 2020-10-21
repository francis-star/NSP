using HCWeb2016;
/////////////////////////////////////////////////////////////////////////////
//模块名：号段维护
//开发者：杨栋
//开发时间：2016年12月5日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_NoManager : SqlBase
    {
        /// <summary>
        ///  获取号段信息
        /// </summary>
        /// <param name="provCode">省份编码</param>
        /// <param name="cityCode">地市编码</param>
        /// <param name="type">运营商类型 移动 电信 联通</param>
        /// <param name="phone">号段</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageNum">每页数量</param>
        /// <returns></returns>
        public DataTable GetNoMangerInfo(string provCode, string cityCode, string type, string phone, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} c.* FROM (
                        SELECT *,row_number() OVER (ORDER BY JoinDate DESC)AS rownumber,(NM_ProvinceName+NM_CityName) AreaName FROM YX_NoManager 
                        WHERE DataState=0 ", pageNum);
            if (!string.IsNullOrEmpty(provCode))
                sb.Append(" AND NM_ProvinceCode = '" + provCode + "'");

            if (!string.IsNullOrEmpty(cityCode))
                sb.Append(" AND NM_CityCode = '" + cityCode + "'");

            if (!string.IsNullOrEmpty(type))
                sb.Append(" AND NM_Type = '" + type + "'");

            if (!string.IsNullOrEmpty(phone))
                sb.Append(" AND NM_Phone like '%" + phone + "%'");
            sb.AppendFormat(" ) c  WHERE c.rownumber >(0+({0}-1)*{1})", pageIndex, pageNum);
            return SearchData(sb.ToString());
        }


        /// <summary>
        ///  获取号段信息数量
        /// </summary>
        /// <param name="provCode">省份编码</param>
        /// <param name="cityCode">地市编码</param>
        /// <param name="type">运营商类型 移动 电信 联通</param>
        /// <param name="phone">号段</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageNum">每页数量</param>
        /// <returns></returns>
        public string GetNoMangerCount(string provCode, string cityCode, string type, string phone, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT count(*) AS DataCount FROM YX_NoManager WHERE DataState=0 ");
            if (!string.IsNullOrEmpty(provCode))
                sb.Append(" AND NM_ProvinceCode = '" + provCode + "'");

            if (!string.IsNullOrEmpty(cityCode))
                sb.Append(" AND NM_CityCode = '" + cityCode + "'");

            if (!string.IsNullOrEmpty(type))
                sb.Append(" AND NM_Type = '" + type + "'");

            if (!string.IsNullOrEmpty(phone))
                sb.Append(" AND NM_Phone = '" + phone + "'");
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
        ///  添加号段信息
        /// </summary>
        /// <param name="provCode">省份编码</param>
        /// <param name="cityCode">城市编码</param>
        /// <param name="type">运营商类型 移动 电信 联通</param>
        /// <param name="phone">号段</param>
        /// <param name="provName">省份名称</param>
        /// <param name="cityName">城市编码</param>
        /// <param name="userName">账号</param>
        /// <returns></returns>
        public bool AddNoManager(string phone, string provCode, string provName, string cityCode, string cityName, string type, string userName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"INSERT INTO YX_NoManager(NM_Code,NM_ProvinceCode,NM_CityCode,NM_Type,NM_Phone,DataState,JoinMan,JoinDate,NM_ProvinceName,NM_CityName)
                        VALUES('{0}','{1}','{2}','{3}','{4}',0,'{5}', getdate(),'{6}','{7}')", GetCode(), provCode, cityCode, type, phone, userName, provName, cityName);
            return UpdateData(sb.ToString());
        }

        /// <summary>
        ///  删除号段
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public bool DeleteNoManager(string Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("DELETE FROM YX_NoManager WHERE NM_Code='{0}'", Code);
            return UpdateData(sb.ToString());
        }

    }
}
