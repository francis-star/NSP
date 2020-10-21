using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Collections;
using HCWeb2016;
using System.Data;

namespace BLL
{
    public class BLL_UserSet
    {
        DAL_UserSet dAL_UserSet = new DAL_UserSet();

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetUser(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_UserSet.GetUser(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <returns></returns>
        public string GetUserList()
        {
            DataTable dt = dAL_UserSet.GetUserList();
            return JSON.DataTableToTreeList(dt);
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetNum(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_UserSet.GetNum(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[3]));
            return dt.Rows[0]["num"].ToString();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DelUser(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_UserSet.DelUser(ValueHandler.GetStringValue(arr[0]));
            if (dt)
                return "true";
            return "false";
        }
    }
}
