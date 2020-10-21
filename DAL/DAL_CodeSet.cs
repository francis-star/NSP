/////////////////////////////////////////////////////////////////////////////
//模块名：基础类型
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
    public class DAL_CodeSet : SqlBase
    {
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetCodeType()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM SYS_CodeType WHERE DataState=0");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据类型加载基础明细表数据
        /// </summary>
        /// <param name="CType_Code"></param>
        /// <returns></returns>
        public DataTable GetCodeSet(string CType_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT c.*,t.CType_Name
                        FROM SYS_CodeSet c
                            JOIN SYS_CodeType t ON c.CSet_CType_Code = t.CType_Code
                        WHERE c.DataState = 0 ");
            if (!string.IsNullOrEmpty(CType_Code))
            {
                sb.Append(" AND CSet_CType_Code='" + CType_Code + "'");
            }
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CSet_Code"></param>
        /// <returns></returns>
        public bool Delete(string CSet_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE SYS_CodeSet SET  DataState = 1 WHERE CSet_Code='" + CSet_Code + "'");
            return UpdateData(sb.ToString());
        }

        /// <summary>
        /// 增加 修改 一条数据
        /// </summary>
        public bool Update(string CSet_Code, string CSet_Name, string CSet_CType_Code, string userName)
        {
            string code = GetCode();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("\r IF NOT EXISTS(SELECT * FROM SYS_CodeSet WHERE CSet_Code ='" + ValueHandler.GetStringValue(CSet_Code) + "')");
            strSql.Append("\r BEGIN ");
            strSql.Append("\r INSERT INTO SYS_CodeSet(");
            strSql.Append("CSet_Code,CSet_Name,CSet_CType_Code,DataState,JoinMan ");
            strSql.Append(") SELECT ");
            strSql.Append("CSet_Code = '" + GetCode() + "',");
            strSql.Append("CSet_Name='" + ValueHandler.GetStringValue(CSet_Name) + "',");
            strSql.Append("CSet_CType_Code='" + ValueHandler.GetStringValue(CSet_CType_Code) + "',DataState = 0,");
            strSql.Append("JoinMan='" + ValueHandler.GetStringValue(userName) + "'");
            strSql.Append("\r END");
            strSql.Append("\r ELSE");
            strSql.Append("\r BEGIN ");
            strSql.Append("UPDATE SYS_CodeSet SET ");
            strSql.Append("CSet_Name='" + ValueHandler.GetStringValue(CSet_Name) + "',");
            strSql.Append("CSet_CType_Code='" + ValueHandler.GetStringValue(CSet_CType_Code) + "'");
            strSql.Append(" WHERE CSet_Code ='" + ValueHandler.GetStringValue(CSet_Code) + "'");
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
