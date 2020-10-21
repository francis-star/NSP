/////////////////////////////////////////////////////////////////////////////
//模块名：易调解
//开发者：章敏
//开发时间：2016年11月7日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCWeb2016;
using System.Data;

namespace DAL
{
    public class DAL_Mediation : SqlBase
    {
        #region 投诉渠道，投诉类型，行业类型
        /// <summary>
        /// 投诉渠道，投诉类型，行业类型
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="type">父类，子类</param>
        /// <returns></returns>
        public DataTable GetParaType(string paras, string type, string datetype)
        {
            string sql = string.Format(@"exec [dbo].[SP_GetParas] '{0}','{1}','{2}'", paras, type, datetype);
            
            return SearchData(sql.ToString());
        }
        #endregion

        #region 查询省、市、区
        /// <summary>
        /// 查询省、市、区
        /// </summary>
        /// <param name="SA_Kind">类型</param>
        /// <param name="SA_PCode">父级编号</param>
        /// <returns></returns>
        public DataTable GetArea(string SA_Kind, string SA_PCode)
        {
            string sql = string.Format("SELECT SA_Code,SA_Name FROM SYS_Area WHERE SA_Kind='{0}' AND SA_PCode='{1}' AND DataState=0 ORDER BY SA_Index ASC", ValueHandler.GetStringValue(SA_Kind), ValueHandler.GetStringValue(SA_PCode));
            return SearchData(sql);
        }
        #endregion

        #region 获取易调解信息
        public DataTable GetMediation(string selectWhere,string order,string pageNum,string pageSize,string dataType)
        {
            string sql = string.Format(@" exec [dbo].[SP_GetPageData]  'Mediation', '{0}', '{1}', '{2}','{3}','{4}'",selectWhere,order,pageNum,pageSize,dataType);
            return SearchData(sql.ToString());
        }
        /// <summary>
        /// 得到原数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMediationDataCount(string selectWhere, string order, string pageNum, string pageSize, string dataType)
        {
            string sql = string.Format(@" exec [dbo].[SP_GetPageData]  'Mediation', '{0}', '{1}', '{2}','{3}','{4}'", selectWhere, order, pageNum, pageSize, dataType);
            return SearchData(sql.ToString()).Rows[0][0].ToString();
        }
        #endregion

        #region 操作易调解信息
        public DataTable OperateMediation(string MD_Code, string MD_Complaintchannel, string MD_ComplaintType
            , string MD_ComplaintTime, string MD_IndustryType, string MD_Province, string MD_City
            , string MD_District, string MD_ProcessingUnit, string MD_Title, string MD_CitationClause
            , string MD_Content, string MD_ProcessingProcedure, string MD_CaseAnalysis, string UserCode, string dataType)
        {
            string sql = string.Format(@" exec [dbo].[SP_OperateMediation] '{0}','{1}', '{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'", 
                MD_Code, MD_Complaintchannel, MD_ComplaintType
                , MD_ComplaintTime, MD_IndustryType, MD_Province, MD_City, MD_District, MD_ProcessingUnit
                , MD_Title, MD_CitationClause, (MD_Content+""+MD_ProcessingProcedure+""+MD_CaseAnalysis), MD_Content, MD_ProcessingProcedure, MD_CaseAnalysis, UserCode, dataType);
            return SearchData(sql.ToString());
        }
        public string DoTopMediation(string id, string type)
        {
            string sql = string.Format(@" update EC_Mediation set MD_TopDate=(case {1} when 1 then GETDATE() else NULL end) where MD_ID='{0}'", id, type);
            try { SearchData(sql.ToString()); }
            catch { }
            return "";
        }
        
        #endregion
    }
}
