using DAL;
using HCWeb2016;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_NoManager
    {
        DAL_NoManager dAL_NoManager = new DAL_NoManager();
        /// <summary>
        /// 获取号段信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetNoMangerInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_NoManager.GetNoMangerInfo(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), 
                                                           ValueHandler.GetIntNumberValue(arr[4]), ValueHandler.GetIntNumberValue(arr[5]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 获取号段信息数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetNoMangerCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_NoManager.GetNoMangerCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetIntNumberValue(arr[4]), ValueHandler.GetIntNumberValue(arr[5]));

        }

        /// <summary>
        /// 添加号段
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string AddNoManager(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_NoManager.AddNoManager(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        ///  删除号段
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public string DeleteNoManager(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_NoManager.DeleteNoManager(ValueHandler.GetStringValue(arr[0])).ToString().ToLower();
        }
    }
}
