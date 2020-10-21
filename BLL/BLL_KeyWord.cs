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
    public class BLL_KeyWord
    {
        DAL_KeyWord dAL_KeyWord = new DAL_KeyWord();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SearchWord(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_KeyWord.SearchWord(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]));
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
            DataTable dt = dAL_KeyWord.GetNum(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]));
            return dt.Rows[0]["num"].ToString();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DelKeyManager(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_KeyWord.DelKeyManager(ValueHandler.GetStringValue(arr[0]));
            if (dt)
                return "true";
            return "false";
        }
    }
}
