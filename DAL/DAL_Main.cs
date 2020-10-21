////////////////////////////://///////////////////////////////////////////////
//模块名：主页面
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
    public class DAL_Main : SqlBase
    {
        /// <summary>
        /// 得到主菜单
        /// </summary>
        /// <param name="UserCode">用户编码</param>
        /// <returns></returns>
        public DataTable GetMainMenu(string UserCode)
        {
            return SearchData(@"SELECT * FROM dbo.AF_SysModule WHERE SM_Code IN (SELECT  SM_PCode FROM dbo.AF_SysModule WHERE SM_Code IN 
                                (SELECT UP_SM_Code FROM dbo.AF_UserPopedom WHERE UP_User_Code='" + UserCode + "') AND DataState = 0 GROUP BY SM_PCode) ORDER BY SM_Index");
        }


        /// <summary>
        /// 返回菜单
        /// </summary>
        /// <param name="UserCode">用户编码</param>
        /// <param name="PCode">父级编码</param>
        /// <returns></returns>
        public DataTable GetMenu(string UserCode, string PCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT b.* FROM dbo.AF_UserPopedom a
            LEFT JOIN dbo.AF_SysModule b ON b.SM_Code = a.UP_SM_Code
            WHERE a.UP_User_Code='" + UserCode + "' AND b.SM_PCode='" + PCode + "'  ORDER by b.SM_Index");
            return SearchData(sb.ToString());
        }
    }
}
