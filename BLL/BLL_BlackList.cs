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
    public class BLL_BlackList
    {
        DAL_BlackList dAL_BlackList = new DAL_BlackList();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetBlackList(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_BlackList.GetBlackList(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetNum(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_BlackList.GetNum(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]));
            return dt.Rows[0]["num"].ToString();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DelBlackList(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_BlackList.DelBlackList(ValueHandler.GetStringValue(arr[0]));
            if (dt)
                return "true";
            return "false";
        }

        /// <summary>
        /// 是否是黑名单号码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string IsBlackPhone(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_BlackList.IsBlackPhone(ValueHandler.GetStringValue(arr[0]));
            if (dt != null && dt.Rows.Count > 0)
                return "true";
            return "false";
        }
    }
}

