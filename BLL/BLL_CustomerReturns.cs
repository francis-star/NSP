using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Collections;
using HCWeb2016;
using System.Data;
using System.Web;

namespace BLL
{
    public class BLL_CustomerReturns
    {
        DAL_CustomerReturns dAL_CustomerReturns = new DAL_CustomerReturns();

        #region 诚信通

        /// <summary>
        /// 获取当前登录用户的工作地点
        /// </summary>
        /// <returns></returns>
        public string GetPlace()
        {
            return dAL_CustomerReturns.GetUserPlace(BLL_User.User_Code);
        }

        /// <summary>
        /// 获取用户工作地点
        /// </summary>
        /// <returns></returns>
        public string GetUserPlace()
        {
            DataTable dt = dAL_CustomerReturns.GetUserPlace();
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCust(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetCust(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]),
                                                       ValueHandler.GetStringValue(arr[15]), ValueHandler.GetStringValue(arr[16]), ValueHandler.GetStringValue(arr[17]), GetPlace());
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
            DataTable dt = dAL_CustomerReturns.GetNum(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[17]), GetPlace());

            return dt.Rows[0]["num"].ToString();
        }

        /// <summary>
        /// 诚信通客服异常退回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string BackCXTCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_CustomerReturns.BackCXTCustomer(ValueHandler.GetStringValue(arr[0]));
            if (dt)
                return "true";
            return "false";
        }

        /// <summary>
        /// 导出诚信通客户列表数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DtCTXToExcel(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string sb = dAL_CustomerReturns.GetCustExcel(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[15]), GetPlace());
            DAL.DAL_OriginalData dAL_OriginalData = new DAL.DAL_OriginalData();
            if (!dAL_OriginalData.CheckExportData(sb.ToString()))
                return "false";

            HttpContext.Current.Session.Remove("FileData");
            HttpContext.Current.Session.Add("FileData", sb.ToString());
            HttpContext.Current.Session.Remove("FileName");
            HttpContext.Current.Session.Add("FileName", "诚信通-客服管理中心明细表");
            return "true";
        }

#endregion

        #region 民企云

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYCust(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetMQYCust(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]),
                                                       ValueHandler.GetStringValue(arr[15]), ValueHandler.GetStringValue(arr[16]), ValueHandler.GetStringValue(arr[17]), GetPlace());
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYNum(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetMQYNum(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[17]), GetPlace());

            return dt.Rows[0]["num"].ToString();
        }

        /// <summary>
        /// 名企云客服异常退回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string BackMQYCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_CustomerReturns.BackMQYCustomer(ValueHandler.GetStringValue(arr[0]));
            if (dt)
                return "true";
            return "false";
        }

        /// <summary>
        /// 导出民企云户列表数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DtMQYToExcel(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string sb = dAL_CustomerReturns.GetMQYCustExcel(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[15]), GetPlace());
            DAL.DAL_OriginalData dAL_OriginalData = new DAL.DAL_OriginalData();
            if (!dAL_OriginalData.CheckExportData(sb.ToString()))
                return "false";

            HttpContext.Current.Session.Remove("FileData");
            HttpContext.Current.Session.Add("FileData", sb.ToString());
            HttpContext.Current.Session.Remove("FileName");
            HttpContext.Current.Session.Add("FileName", "民企云-客服管理中心明细表");
            return "true";
        }

        #endregion

        #region 维权通

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTCust(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetWQTCust(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]),
                                                       ValueHandler.GetStringValue(arr[15]), ValueHandler.GetStringValue(arr[16]), ValueHandler.GetStringValue(arr[17]), GetPlace());
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTNum(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetWQTNum(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[17]), GetPlace());

            return dt.Rows[0]["num"].ToString();
        }

        /// <summary>
        /// 维权通客服异常退回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string BackWQTCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_CustomerReturns.BackWQTCustomer(ValueHandler.GetStringValue(arr[0]));
            if (dt)
                return "true";
            return "false";
        }

        /// <summary>
        /// 导出维权通列表数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DtWQTToExcel(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string sb = dAL_CustomerReturns.GetWQTCustExcel(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[15]), GetPlace());
            DAL.DAL_OriginalData dAL_OriginalData = new DAL.DAL_OriginalData();
            if (!dAL_OriginalData.CheckExportData(sb.ToString()))
                return "false";

            HttpContext.Current.Session.Remove("FileData");
            HttpContext.Current.Session.Add("FileData", sb.ToString());
            HttpContext.Current.Session.Remove("FileName");
            HttpContext.Current.Session.Add("FileName", "维权通-客服管理中心明细表");
            return "true";
        }

        #endregion
        
        #region 新消费宝典

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBCust(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetXFBCust(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]),
                                                       ValueHandler.GetStringValue(arr[15]), ValueHandler.GetStringValue(arr[16]), ValueHandler.GetStringValue(arr[17]), GetPlace());
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBNum(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetXFBNum(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[17]), GetPlace());

            return dt.Rows[0]["num"].ToString();
        }

        /// <summary>
        /// 维权通客服异常退回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string BackXFBCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_CustomerReturns.BackXFBCustomer(ValueHandler.GetStringValue(arr[0]));
            if (dt)
                return "true";
            return "false";
        }

        /// <summary>
        /// 导出维权通列表数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DtXFBToExcel(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string sb = dAL_CustomerReturns.GetXFBCustExcel(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[15]), GetPlace());
            DAL.DAL_OriginalData dAL_OriginalData = new DAL.DAL_OriginalData();
            if (!dAL_OriginalData.CheckExportData(sb.ToString()))
                return "false";

            HttpContext.Current.Session.Remove("FileData");
            HttpContext.Current.Session.Add("FileData", sb.ToString());
            HttpContext.Current.Session.Remove("FileName");
            HttpContext.Current.Session.Add("FileName", "维权通-客服管理中心明细表");
            return "true";
        }

        #endregion

        #region 实时保
         
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBCust(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetSSBCust(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]),
                                                       ValueHandler.GetStringValue(arr[15]), ValueHandler.GetStringValue(arr[16]), ValueHandler.GetStringValue(arr[17]), GetPlace());
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBNum(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerReturns.GetSSBNum(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[17]), GetPlace());

            return dt.Rows[0]["num"].ToString();
        }

        /// <summary>
        /// 维权通客服异常退回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string BackSSBCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_CustomerReturns.BackSSBCustomer(ValueHandler.GetStringValue(arr[0]));
            if (dt)
                return "true";
            return "false";
        }

        /// <summary>
        /// 导出维权通列表数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DtSSBToExcel(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string sb = dAL_CustomerReturns.GetSSBCustExcel(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]),
                                                       ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]),
                                                       ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]), ValueHandler.GetStringValue(arr[13]), ValueHandler.GetStringValue(arr[14]), ValueHandler.GetStringValue(arr[15]), GetPlace());
            DAL.DAL_OriginalData dAL_OriginalData = new DAL.DAL_OriginalData();
            if (!dAL_OriginalData.CheckExportData(sb.ToString()))
                return "false";

            HttpContext.Current.Session.Remove("FileData");
            HttpContext.Current.Session.Add("FileData", sb.ToString());
            HttpContext.Current.Session.Remove("FileName");
            HttpContext.Current.Session.Add("FileName", "实时保-客服管理中心明细表");
            return "true";
        }

        #endregion
    }
}
