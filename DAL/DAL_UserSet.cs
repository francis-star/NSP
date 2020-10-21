using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_UserSet : SqlBase
    {
        /// <summary>
        /// 根据姓名查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="indexNum"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public DataTable GetUser(string name, string PageIndex, string PageNum, string place)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP(" + ValueHandler.GetIntNumberValue(PageNum) + ")* FROM(SELECT *,ROW_NUMBER() OVER (ORDER BY JoinDate DESC) AS 'Num' FROM AF_User WHERE 1=1");
            if (name != "")
                sb.Append(" AND User_Name like '%" + ValueHandler.GetStringValue(name) + "%'");
            if (place != "" && place != "全部")
                sb.Append(" AND User_Place like '%" + ValueHandler.GetStringValue(place) + "%'");
            sb.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1})", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sb.ToString());
        }
        public DataTable GetUserList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT a.User_Code,a.User_Name FROM AF_User a WHERE DataState = 0 ORDER BY JoinDate ASC");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetNum(string name, string place)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COUNT(*) AS num FROM AF_User WHERE 1=1");
            if (name != "")
                sb.Append(" AND User_Name like '%" + ValueHandler.GetStringValue(name) + "%'");
            if (place != "" && place != "全部")
                sb.Append(" AND User_Place like '%" + ValueHandler.GetStringValue(place) + "%'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据code删除数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool DelUser(string code)
        {
            string str = "DELETE FROM AF_User WHERE User_Code='" + ValueHandler.GetStringValue(code) + "'";
            return UpdateData(str);
        }

    }
}
