/////////////////////////////////////////////////////////////////////////////
//模块名：明细数据
//开发者：赵虎
//开发时间：2016年11月28日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.InMolde;
using HCWeb2016;

namespace BLL
{
    public class BLL_OriginalDataDts
    {
        DAL_OriginalDataDts dAL_OriginalDataDts = new DAL_OriginalDataDts();

        #region 新消费宝典

        #region 查看数据

        /// <summary>
        /// 得到查看数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBViewData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetXFBViewData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到查看数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBViewDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetXFBViewDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
        }

        #endregion

        #region 黑名单

        /// <summary>
        /// 得到黑名单数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBBlackData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetXFBBlackData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到黑名单数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBBlackDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetXFBBlackDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GeneralXFBDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.GeneralXFBDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SaveXFBGeneralDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.SaveXFBGeneralDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 移动筛选数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MoveXFBScreenData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string[] arrCode = (arr[0] as string).Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (dAL_OriginalDataDts.MoveXFBScreenData(arrCode, ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL.BLL_User.User_Name, arr[3].ToString()))
                return "true";
            return "false";
        }

        #endregion

        #endregion

        #region 实时保

        #region 查看数据

        /// <summary>
        /// 得到查看数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBViewData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetSSBViewData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]),
                                                           ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到查看数据信息 及数量(无效数据，数量，有效数量)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetInvalidSSBViewData(object obj)
        {
            var arr = obj as InOriginViewModle;

            //ArrayList arr = JSON.getPara(obj);
            var result = dAL_OriginalDataDts.GetInvalidSSBViewData(arr);

            return result;
        }

        /// <summary>
        ///  得到查看数据信息 及数量(有效数据，数量)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetValidSSBViewData(object obj)
        {
            var arr = obj as InOriginViewModle;

            //ArrayList arr = JSON.getPara(obj);
            var result = dAL_OriginalDataDts.GetValidSSBViewData(arr);

            return result;
        }


        /// <summary>
        /// 得到查看数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBViewDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetSSBViewDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
        }

        public string GetTotalCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetTotalCount(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetNoValidYearData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetNoValidYearData(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        public string GetOtherTDYearData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetOtherTDYearData(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        public string GetOtherTDSJYearData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetOtherTDSJYearData(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        } 

        #endregion

        #region 黑名单

        /// <summary>
        /// 得到黑名单数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBBlackData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetSSBBlackData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到黑名单数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBBlackDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetSSBBlackDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GeneralSSBDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.GeneralSSBDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SaveSSBGeneralDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.SaveSSBGeneralDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 移动筛选数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MoveSSBScreenData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string[] arrCode = (arr[0] as string).Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (dAL_OriginalDataDts.MoveSSBScreenData(arrCode, ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL.BLL_User.User_Name, arr[3].ToString()))
                return "true";
            return "false";
        }

        #endregion

        #endregion

        #region 诚信通

        #region 查看数据

        /// <summary>
        /// 得到查看数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetViewData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetViewData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到查看数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetViewDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetViewDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
        }

        #endregion

        #region 黑名单

        /// <summary>
        /// 得到黑名单数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetBlackData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetBlackData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到黑名单数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetBlackDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetBlackDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GeneralDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.GeneralDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SaveGeneralDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.SaveGeneralDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 移动筛选数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MoveScreenData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string[] arrCode = (arr[0] as string).Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (dAL_OriginalDataDts.MoveScreenData(arrCode, ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL.BLL_User.User_Name, arr[3].ToString()))
                return "true";
            return "false";
        }


        #endregion

        #endregion

        #region 民企云

        #region 查看数据

        /// <summary>
        /// 得到查看数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYViewData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetMQYViewData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到查看数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYViewDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetMQYViewDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
        }

        #endregion

        #region 黑名单

        /// <summary>
        /// 得到黑名单数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYBlackData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetMQYBlackData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到黑名单数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYBlackDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetMQYBlackDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GeneralMQYDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.GeneralMQYDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SaveMQYGeneralDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.SaveMQYGeneralDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 移动筛选数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MoveMQYScreenData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string[] arrCode = (arr[0] as string).Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (dAL_OriginalDataDts.MoveMQYScreenData(arrCode, ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL.BLL_User.User_Name, arr[3].ToString()))
                return "true";
            return "false";
        }

        #endregion

        #endregion

        #region 维权通

        #region 查看数据

        /// <summary>
        /// 得到查看数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTViewData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetWQTViewData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到查看数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTViewDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetWQTViewDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
        }

        #endregion

        #region 黑名单

        /// <summary>
        /// 得到黑名单数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTBlackData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalDataDts.GetWQTBlackData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到黑名单数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTBlackDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalDataDts.GetWQTBlackDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 筛选

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GeneralWQTDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.GeneralWQTDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SaveWQTGeneralDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalDataDts.SaveWQTGeneralDatas(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name))
                return "true";
            return "false";
        }

        /// <summary>
        /// 移动筛选数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MoveWQTScreenData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string[] arrCode = (arr[0] as string).Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (dAL_OriginalDataDts.MoveWQTScreenData(arrCode, ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL.BLL_User.User_Name, arr[3].ToString()))
                return "true";
            return "false";
        }

        #endregion

        #endregion
    }
}
