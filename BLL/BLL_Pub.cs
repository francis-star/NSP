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
using DAL;
using HCWeb2016;

namespace BLL
{
    public class BLL_Pub
    {
        DAL_Pub dAL_Pub = new DAL_Pub();

        /// <summary>
        /// 获取按钮权限
        /// </summary>
        /// <param name="obj">用户ID</param>
        /// <returns></returns>
        public string GetMenuRight(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_Pub.GetMenuRight(BLL_User.User_Code, ValueHandler.GetStringValue(arr[0]));
            if (dt.Rows.Count > 0)
            {
                return ValueHandler.GetStringValue(dt.Rows[0]["UP_SM_FunIDs"]);
            }
            return "";
        }
    }
}
