/////////////////////////////////////////////////////////////////////////////
//模块名：基础类型明细
//开发者：杨栋
//开发时间：2016年11月7日
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
    public class DAL_CodeType : SqlBase
    {
        /// <summary>
        /// 根据类型加载基础类型数据
        /// </summary>
        /// <param name="CType_Code"></param>
        /// <returns></returns>
        public DataTable GetCodeType(string CType_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT * FROM SYS_CodeType WHERE DataState= 0 ");
            if (!string.IsNullOrEmpty(CType_Code))
            {
                sb.AppendFormat(" and CType_Code='{0}'", CType_Code);
            }
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CType_Code"></param>
        /// <returns></returns>
        public bool DeleteType(string CType_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE SYS_CodeType SET  DataState = 1 WHERE CType_Code='" + CType_Code + "'");
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 增加 修改 一条数据
        /// </summary>
        public bool UpdateType(string CType_Code, string CType_Name, string userName)
        { 
            StringBuilder strSql = new StringBuilder();
            strSql.Append("\r IF NOT EXISTS(SELECT * FROM SYS_CodeType WHERE CType_Code ='" + ValueHandler.GetStringValue(CType_Code) + "')");
            strSql.Append("\r BEGIN ");
            strSql.Append("\r INSERT INTO SYS_CodeType(");
            strSql.Append("CType_Code,CType_Name,DataState,JoinMan ");
            strSql.Append(") SELECT ");
            strSql.Append("CType_Code = '" + GetCode() + "',");
            strSql.Append("CType_Name='" + ValueHandler.GetStringValue(CType_Name) + "',DataState = 0,"); 
            strSql.Append("JoinMan='" + ValueHandler.GetStringValue(userName) + "'");
            strSql.Append("\r END");
            strSql.Append("\r ELSE");
            strSql.Append("\r BEGIN ");
            strSql.Append("UPDATE SYS_CodeType SET ");
            strSql.Append("CType_Name='" + ValueHandler.GetStringValue(CType_Name) + "'");
            strSql.Append(" WHERE CType_Code ='" + ValueHandler.GetStringValue(CType_Code) + "'");
            strSql.Append("\r END");

            if (strSql.Length > 0)
            {
                return UpdateData(strSql.ToString());
            }
            else
            {
                throw new Exception("无数据操作！");
            }
        }
    }
}
