/////////////////////////////////////////////////////////////////////////////
//模块名：计费历史记录
//开发者：杨栋
//开发时间：2016年11月28日
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
    public class DAL_BillHistory : SqlBase
    {
        /// <summary>
        /// 获取修改记录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public DataTable GetBillHistoryInfo(string code, int pageIndex, int pageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {1} * FROM
                        (SELECT row_number() OVER (ORDER BY JoinDate DESC) AS rownumber, *  FROM YX_BillHistory WHERE BH_Cust_Code='{0}') a
                        WHERE a.rownumber > (0+({2}-1)*{1})", code, pageNum, pageIndex);
            return SearchData(sb.ToString());
        }

        /// <summary>
        ///  获取修改记录条数
        /// </summary>
        /// <param name="code"></param>
        public string GetBillHistoryCount(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT count(*) AS DataCount  FROM YX_BillHistory WHERE BH_Cust_Code='{0}'",code);
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
    }
}
