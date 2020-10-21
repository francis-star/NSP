////////////////////////////://///////////////////////////////////////////////
//模块名：修改密码
//开发者：赵虎
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCWeb2016;

namespace DAL
{
    public class DAL_UpdatePass : SqlBase
    {
        /// <summary>
        /// 得到旧密码
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public DataTable GetPass(string UserCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT User_PassWord FROM AF_User WHERE User_Code='" + UserCode + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 保存密码
        /// </summary>
        /// <param name="e_User"></param>
        /// <returns></returns>
        public bool SavePass(string userCode,string password)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\r IF EXISTS(SELECT User_Code FROM AF_User WHERE User_Code = '" + ValueHandler.GetStringValue(userCode) + "')");
            sb.Append("\r BEGIN ");
            sb.Append("UPDATE AF_User SET User_PassWord = '" + password + "'");
            sb.Append(" WHERE User_Code ='" + ValueHandler.GetStringValue(userCode) + "'");
            sb.Append("\r END");
            return UpdateData(sb.ToString());
        }
    }
}
