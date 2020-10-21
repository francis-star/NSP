////////////////////////////://///////////////////////////////////////////////
//模块名：按钮权限
//开发者：赵虎
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCWeb2016;

namespace DAL
{
    public class DAL_Pub:SqlBase
    {
        /// <summary>
        /// 得到模块权限列表
        /// </summary>
        /// <param name="UserCode">用户编码</param>
        /// <param name="SMCode">模块Code</param>
        /// <returns></returns>
        public DataTable GetMenuRight(string UserCode, string SMCode)
        {
            return SearchData("SELECT * FROM dbo.AF_UserPopedom WHERE DataState=0 AND UP_User_Code = '" + UserCode + "' AND UP_SM_Code = '" + SMCode + "'");
        }
    }
}
