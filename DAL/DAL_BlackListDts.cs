using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_BlackListDts : SqlBase
    {

        public bool SaveBlackList(string Phone, string Comment, string bl_ProvinceCode, string bl_ProcinceName, string bl_CityCode, string bl_CItyName, string joinman)
        {
            StringBuilder sb = new StringBuilder();
            string strCode = GetCode();
            DataTable dt = SearchData("SELECT * FROM YX_BlackList WHERE BL_Phone='" + ValueHandler.GetStringValue(Phone) + "'");
            if (dt.Rows.Count > 0 && dt.Rows[0]["BL_Code"].ToString() != "")
                return false;
            sb.Append("INSERT INTO YX_BlackList (");
            sb.Append("BL_Code,BL_Phone,BL_Comment,BL_ProvinceCode,BL_ProvinceName,BL_CityCode,BL_CityName,JoinMan");
            sb.Append(")SELECT ");
            sb.Append("BL_Code='" + strCode + "',");
            sb.Append("BL_Phone='" + ValueHandler.GetStringValue(Phone) + "',");
            sb.Append("BL_Comment='" + ValueHandler.GetStringValue(Comment) + "',");
            sb.Append("BL_ProvinceCode='" + ValueHandler.GetStringValue(bl_ProvinceCode) + "',");
            sb.Append("BL_ProvinceName='" + ValueHandler.GetStringValue(bl_ProcinceName) + "',");
            sb.Append("BL_CityCode='" + ValueHandler.GetStringValue(bl_CityCode) + "',");
            sb.Append("BL_CityName='" + ValueHandler.GetStringValue(bl_CItyName) + "',");
            sb.Append("JoinMan='" + ValueHandler.GetStringValue(joinman) + "' ");

            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 批量导入黑名单
        /// </summary>
        /// <param name="blackList"></param>
        /// <param name="joinMan"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool PlInsertBlackPhone(string blackList, string joinMan, string comment)
        {
            bool flag = false;
            int result;
            flag = new SqlBase().ExcuteNonQuery_Sp("SP_PLInsertBlackList", new SqlParameter[] {
                        new SqlParameter("@BlackPhone",blackList),
                        new SqlParameter("@Comment",comment),
                        new SqlParameter("@JoinMan",joinMan)
              }, out result);
            return flag;
        }
    }
}
