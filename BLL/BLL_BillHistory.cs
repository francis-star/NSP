using DAL;
using HCWeb2016;
/////////////////////////////////////////////////////////////////////////////
//模块名：计费历史记录
//开发者：杨栋
//开发时间：2016年11月28日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_BillHistory
    {
        DAL_BillHistory dAL_BillHistory = new DAL_BillHistory();
        /// <summary>
        /// 获取修改记录
        /// </summary>
        /// <param name="obj"></param>
        public string GetBillHistoryInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_BillHistory.GetBillHistoryInfo(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetIntNumberValue(arr[1]), ValueHandler.GetIntNumberValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 获取修改记录条数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetBillHistoryCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_BillHistory.GetBillHistoryCount(ValueHandler.GetStringValue(arr[0]));
        }
    }
}
