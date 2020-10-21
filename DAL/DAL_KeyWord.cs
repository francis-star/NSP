using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_KeyWord:SqlBase
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="km_ProcinceName">省</param>
        /// <param name="km_CItyName">市</param>
        /// <param name="km_Name">关键字</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="PageNum">条数</param>
        /// <returns></returns>
        public DataTable SearchWord(string km_ProcinceName, string km_Name, string PageIndex, string PageNum, string PlatForm,string Danger)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP(" + ValueHandler.GetIntNumberValue(PageNum) + ")* FROM(SELECT *,ROW_NUMBER() OVER (ORDER BY JoinDate DESC) AS 'Num' FROM YX_KeyManager WHERE 1=1");
            if(km_ProcinceName!="")
                sb.Append(" AND KM_ProvinceName='" + ValueHandler.GetStringValue(km_ProcinceName) + "'");
            //if (km_CityName != "")
            //    sb.Append(" AND KM_CityName ='"+ValueHandler.GetStringValue(km_CityName)+"'");
            if(km_Name!="")
                sb.Append(" AND KM_Name LIKE '%"+ValueHandler.GetStringValue(km_Name)+"%'");
            if (PlatForm != "")
                sb.Append(" AND KM_PlatForm = '" + ValueHandler.GetIntNumberValue(PlatForm) + "'");
            if (Danger != "")
                sb.Append(" AND KM_DangerousDegree =" + Danger );

            sb.AppendFormat(") T WHERE T.Num >(0+({0}-1)*{1}) order by Num asc", ValueHandler.GetIntNumberValue(PageIndex), ValueHandler.GetIntNumberValue(PageNum));
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="km_ProcinceName"></param>
        /// <param name="km_CItyName"></param>
        /// <param name="km_Name"></param>
        /// <returns></returns>
        public DataTable GetNum(string km_ProcinceName, string km_Name, string PageIndex, string PageNum, string PlatForm, string Danger)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COUNT(*) AS num FROM YX_KeyManager WHERE 1=1");
            if (km_ProcinceName != "")
                sb.Append(" AND KM_ProvinceName='" + ValueHandler.GetStringValue(km_ProcinceName) + "'");
            //if (km_CItyName != "")
            //    sb.Append(" AND KM_CItyName ='" + ValueHandler.GetStringValue(km_CItyName) + "'");
            if (km_Name != "")
                sb.Append(" AND KM_Name LIKE '%" + ValueHandler.GetStringValue(km_Name) + "%'");
            if (PlatForm != "")
                sb.Append(" AND KM_PlatForm = '" + ValueHandler.GetIntNumberValue(PlatForm) + "'");
            if (Danger != "")
                sb.Append(" AND KM_DangerousDegree =" + Danger);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据code删除数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool DelKeyManager(string code)
        {
            string str = "DELETE FROM YX_KeyManager WHERE KM_Code='" + ValueHandler.GetStringValue(code) + "'";
            return UpdateData(str);
        }
    }
}