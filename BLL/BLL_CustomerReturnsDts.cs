using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Collections;
using HCWeb2016;
using System.Data;
using Newtonsoft.Json;

namespace BLL
{
    public class BLL_CustomerReturnsDts
    {
        DAL_CustomerReturnsDts dAL_CustomerReturnsDts = new DAL_CustomerReturnsDts();

        #region 诚信通

        public string GetCustomerListByBillNumber(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetCustomerListByBillNumber(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetDetails(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetDetails(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetBilllist(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetBilllist(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ModState(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_CustomerReturnsDts.ModState(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetIntNumberValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                        ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), BLL_User.User_Name, arr[12].ToString(),BLL_User.User_Code);

            if (flag)
                return "true";
            return "false";
        }

        public string GetLastestCustState(object obj)
        {
            ArrayList arr = JSON.getPara(obj); 
            DataTable dt = dAL_CustomerReturnsDts.GetLastestStateBillHistory(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        #endregion

        #region 民企云

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYDetails(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetMQYDetails(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYBilllist(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetMQYBilllist(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ModMQYState(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_CustomerReturnsDts.ModMQYState(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetIntNumberValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                        ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), BLL_User.User_Name, arr[12].ToString(),BLL_User.User_Code);

            if (flag)
                return "true";
            return "false";
        }
        #endregion

        #region 维权通

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTDetails(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetWQTDetails(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTBilllist(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetWQTBilllist(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ModWQTState(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_CustomerReturnsDts.ModWQTState(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetIntNumberValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                        ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), BLL_User.User_Name, arr[12].ToString(), BLL_User.User_Code);

            if (flag)
                return "true";
            return "false";
        }

        #endregion

        #region 新消费宝典

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBDetails(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetXFBDetails(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBBilllist(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetXFBBilllist(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ModXFBState(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_CustomerReturnsDts.ModXFBState(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetIntNumberValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                        ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), BLL_User.User_Name, arr[12].ToString(), BLL_User.User_Code);

            if (flag)
                return "true";
            return "false";
        }

        /// <summary>
        /// 验证是否开户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBCustomerListByBillNumber(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetXFBCustomerListByBillNumber(ValueHandler.GetStringValue(arr[0]));
            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region 实时保
        public string GetSSBCustomerListByBillNumber(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetSSBCustomerListByBillNumber(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBDetails(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetSSBDetails(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBBilllist(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetSSBBilllist(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ModSSBState(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_CustomerReturnsDts.ModSSBState(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetIntNumberValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                        ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), BLL_User.User_Name, arr[12].ToString(), BLL_User.User_Code);

            if (flag)
                return "true";
            return "false";
        }

        public string GetBillInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetBillInfo(ValueHandler.GetStringValue(arr[0]));
            return JsonConvert.SerializeObject(dt);
        }

        public string GetBillDtsInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturnsDts.GetBillDtsInfo(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            return JsonConvert.SerializeObject(dt);
        }

        #endregion
    }
}
