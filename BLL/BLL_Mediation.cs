/////////////////////////////////////////////////////////////////////////////
//模块名：易调解
//开发者：章敏
//开发时间：2017年06月13日
//////////////////////////////////////////////////////////////////////////////
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
    public class BLL_Mediation
    {
        DAL_Mediation dAL_Mediation = new DAL_Mediation();

        #region 投诉渠道，投诉类型，行业类型
        /// <summary>
        /// 投诉渠道，投诉类型，行业类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetParaType(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_Mediation.GetParaType(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 查询省、市、区
        /// <summary>
        /// 查询省、市、区
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetArea(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_Mediation.GetArea(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 易调解信息
        /// <summary>
        /// 获取易调解信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMediation(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string selectWhere = HttpUtility.UrlDecode(arr[0].ToString()).Replace(';', ',').Replace("'", "''");
            string  order=ValueHandler.GetStringValue(arr[1]);
            string  pageNum=ValueHandler.GetStringValue(arr[2]);
            string  pageSize=ValueHandler.GetStringValue(arr[3]);
            string  dataType=ValueHandler.GetStringValue(arr[4]);
            DataTable dt = dAL_Mediation.GetMediation(selectWhere, order, pageNum, pageSize, dataType);
            string json = "";
            json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetMediationDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string selectWhere = HttpUtility.UrlDecode(arr[0].ToString()).Replace(';', ',').Replace("'", "''");
            string  order=ValueHandler.GetStringValue(arr[1]);
            string  pageNum=ValueHandler.GetStringValue(arr[2]);
            string  pageSize=ValueHandler.GetStringValue(arr[3]);
            string  dataType=ValueHandler.GetStringValue(arr[4]);
            string json = dAL_Mediation.GetMediationDataCount(selectWhere, order, pageNum, pageSize, dataType);
            return json;
        }
        
        #endregion

        #region 新增，修改，删除易调解信息
        /// <summary>
        /// 新增，修改，删除易调解信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string OperateMediation(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string MD_Code = ValueHandler.GetStringValue(arr[0]);
            string MD_Complaintchannel = ValueHandler.GetStringValue(arr[1]);
            string MD_ComplaintType = ValueHandler.GetStringValue(arr[2]);
            string MD_ComplaintTime = ValueHandler.GetStringValue(arr[3]);
            string MD_IndustryType = ValueHandler.GetStringValue(arr[4]);
            string MD_Province = ValueHandler.GetStringValue(arr[5]);
            string MD_City = ValueHandler.GetStringValue(arr[6]);
            string MD_District = ValueHandler.GetStringValue(arr[7]);
            string MD_ProcessingUnit = ValueHandler.GetStringValue(arr[8]);
            string MD_Title = ValueHandler.GetStringValue(arr[9]);
            string MD_CitationClause = ValueHandler.GetStringValue(arr[10]);
            string MD_Content = HttpUtility.UrlDecode(ValueHandler.GetStringValue(arr[11]));
            string MD_ProcessingProcedure = HttpUtility.UrlDecode(ValueHandler.GetStringValue(arr[12]));
            string MD_CaseAnalysis = HttpUtility.UrlDecode(ValueHandler.GetStringValue(arr[13]));
            string UserCode = BLL_User.User_Code;
            string Type = ValueHandler.GetStringValue(arr[14]);

            DataTable dt = dAL_Mediation.OperateMediation(MD_Code, MD_Complaintchannel, MD_ComplaintType
                , MD_ComplaintTime, MD_IndustryType, MD_Province, MD_City, MD_District, MD_ProcessingUnit
                , MD_Title, MD_CitationClause, MD_Content,MD_ProcessingProcedure,MD_CaseAnalysis, UserCode, Type);
            string json = "";
            json = JSON.DataTableToArrayList(dt);
            return json;
        }
        #endregion
        #region 易调解置顶
        /// <summary>
        /// 获取易调解置顶
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DoTopMediation(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string id = ValueHandler.GetStringValue(arr[0]);
            string type = ValueHandler.GetStringValue(arr[1]);
            return dAL_Mediation.DoTopMediation(id, type);
        }
        #endregion
    }
}
