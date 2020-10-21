/////////////////////////////////////////////////////////////////////////////
//模块名：登录
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
using Common;
using DAL;
using HCWeb2016;

namespace BLL
{
    public class BLL_Login
    {
        DAL_Login dAL_Login = new DAL_Login();
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Login(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_Login.Login(ValueHandler.GetStringValue(arr[0]), Security.EncryptDES(ValueHandler.GetStringValue(arr[1])));
            if (dt.Rows.Count == 0) return "用户名或密码错误！";
           
            BLL_User.SetUseInfo(dt);
            return "true";
        }
    }
}
