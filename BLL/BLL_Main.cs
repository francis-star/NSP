/////////////////////////////////////////////////////////////////////////////
//模块名：登录
//开发者：赵虎
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using HCWeb2016;

namespace BLL
{
    public class BLL_Main
    {
        DAL_Main dAL_Main = new DAL_Main();

        /// <summary>
        /// 得到登录人
        /// </summary>
        /// <returns></returns>
        public string GetLoginMan()
        {
            return BLL_User.User_Name;
        }

        /// <summary>
        /// 得到菜单权限
        /// </summary>
        /// <returns></returns>
        public string GetMenu()
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = dAL_Main.GetMainMenu(BLL_User.User_Code);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<div title='" + ValueHandler.GetStringValue(dt.Rows[i]["SM_Name"]) + "'>");
                sb.Append("<div id='Div" + i + "' class='navPanelMini'>");
                DataTable dtDts = dAL_Main.GetMenu(BLL_User.User_Code, ValueHandler.GetStringValue(dt.Rows[i]["SM_Code"]));
                for (int j = 0; j < dtDts.Rows.Count; j++)
                {
                    sb.Append("<li><div onclick=\"AddItem(" + ValueHandler.GetStringValue(dtDts.Rows[j]["SM_Code"]) + ",'" + ValueHandler.GetStringValue(dtDts.Rows[j]["SM_Name"]) + "','" + ValueHandler.GetStringValue(dtDts.Rows[j]["SM_Url"]) + "')\"><img src='Images/" + ValueHandler.GetStringValue(dtDts.Rows[j]["SM_Code"]) + ".png' width='28' height='28'>" + ValueHandler.GetStringValue(dtDts.Rows[j]["SM_Name"]) + "</div>");
                }
                sb.Append("</div>");
                sb.Append("</div>");
            }
            return sb.ToString();
        }
    }
}
