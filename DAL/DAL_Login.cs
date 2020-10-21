////////////////////////////://///////////////////////////////////////////////
//模块名：登录
//开发者：赵虎
//开发时间：2016年11月23日
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
    public class DAL_Login : SqlBase
    {
        /// <summary>
        /// 检测登录信息
        /// </summary>
        /// <param name="UserName">登录名</param>
        /// <param name="PassWord">密码</param>
        /// <returns></returns>
        public DataTable Login(string UserName, string PassWord)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM AF_User WHERE User_LoginName = '" + UserName + "' AND User_PassWord = '" + PassWord + "'");
            return SearchData(sb.ToString());
        }
    }
}
