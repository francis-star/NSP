////////////////////////////://///////////////////////////////////////////////
//模块名：修改密码
//开发者：赵虎
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCWeb2016;
using DAL;
using System.Data;
using System.Collections;
using Common;

namespace BLL
{
    public class BLL_UpdatePass
    {
        DAL_UpdatePass dAL_UpdatePass = new DAL_UpdatePass();

        /// <summary>
        /// 得到旧密码
        /// </summary>
        /// <returns></returns>
        public string GetPass(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_UpdatePass.GetPass(ValueHandler.GetStringValue(BLL_User.User_Code));

            if (dt.Rows[0][0].ToString() == Security.EncryptDES(arr[0].ToString()))
                return "true";
            return "false";
        }

        /// <summary>
        /// 保存密码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SavePass(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            if (dAL_UpdatePass.SavePass(BLL_User.User_Code, Security.EncryptDES(arr[0].ToString())))
                return "true";
            return "false";
        }
    }
}
