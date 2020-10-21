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
    public class BLL_AllQuery
    {
        DAL_AllQuery dAL_AllQuery = new DAL_AllQuery();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_AllQuery.GetCustomer(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]),
                                                       ValueHandler.GetStringValue(arr[15]), ValueHandler.GetStringValue(arr[16]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }
        public string GetValidData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_AllQuery.GetValidData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }
        public string GetTSSheetData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_AllQuery.GetTSSheetData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }
        public string GetTSSheetDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string dt = dAL_AllQuery.GetTSSheetDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            return dt;
        }
        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCustomerCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string dt = dAL_AllQuery.GetCustomerCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]));

            return dt;
        }
        public string GetValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string dt = dAL_AllQuery.GetValidDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            return dt;
        }
    }
}
