/////////////////////////////////////////////////////////////////////////////
//模块名：资讯信息维护
//开发者：田维华
//开发时间：2016年11月7日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCWeb2016;
using System.Data;
using Model;

namespace DAL
{
    public class DAL_PublicInfoDts : SqlBase
    {
        #region 查询资讯信息
        /// <summary>
        /// 查询资讯信息
        /// </summary>
        /// <param name="Pub_Code">资讯编号</param>
        /// <returns>资讯信息</returns>
        public DataTable GetPublicInfo(string Pub_Code)
        {
            string sql = string.Format("SELECT Pub_Code,Pub_LS_Code1,Pub_LS_Code2,Pub_LS_Code3,Pub_LS_Code4,Pub_SA_Code1,Pub_SA_Code2,Pub_SA_Code3,Pub_Title,Pub_Pic1,Pub_Pic2,Pub_Pic3,Pub_Content,Pub_Content,Pub_ArticleSource,Pub_KeyWords,Pub_ReadCount,Pub_PraiseCount FROM XXSD_PublicInfo WHERE Pub_Code = '{0}'", ValueHandler.GetStringValue(Pub_Code));
            return SearchData(sql);
        }
        #endregion

        #region 新增、修改资讯信息
        /// <summary>
        /// 新增、修改资讯信息
        /// </summary>
        /// <param name="model">咨询信息实体类</param>
        /// <returns></returns>
        public bool Update(XXSD_PublicInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("\r IF NOT EXISTS(SELECT * FROM XXSD_PublicInfo WHERE Pub_Code ='" + ValueHandler.GetStringValue(model.Pub_Code) + "')");
            strSql.Append("\r BEGIN ");
            strSql.Append("\r INSERT INTO XXSD_PublicInfo(");
            strSql.Append("Pub_Code,Pub_LS_Code1,Pub_LS_Code2,Pub_LS_Code3,Pub_LS_Code4,Pub_LS_Code5,Pub_SA_Code1,Pub_SA_Code2,Pub_SA_Code3,Pub_SA_Name1,Pub_SA_Name2,Pub_SA_Name3,Pub_Title,Pub_Pic1,Pub_Pic2,Pub_Pic3,Pub_Content,Pub_ArticleSource,Pub_KeyWords,JoinMan ");
            strSql.Append(") SELECT ");
            strSql.Append(" Pub_Code = '" + GetCode() + "', ");
            strSql.AppendFormat(GetXXSD_PublicInfo(model) + ",JoinMan='{0}'",model.JoinMan);
            strSql.Append("\r END");
            strSql.Append("\r ELSE");
            strSql.Append("\r BEGIN ");
            strSql.Append("UPDATE XXSD_PublicInfo SET ");
            strSql.Append(GetXXSD_PublicInfo(model));
            strSql.Append(" WHERE Pub_Code ='" + ValueHandler.GetStringValue(model.Pub_Code) + "'");
            strSql.Append("\r END");
            if (strSql.Length > 0)
            {
                return UpdateData(strSql.ToString());
            }
            else
            {
                throw new Exception("无数据操作！");
            }
        }
        #endregion

        #region 得到数据
        /// <summary>
        /// 得到数据
        /// </summary>
        public string GetXXSD_PublicInfo(XXSD_PublicInfo Model)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("Pub_LS_Code1 = '" + ValueHandler.GetStringValue(Model.Pub_LS_Code1) + "', ");

            strSql.Append("Pub_LS_Code2 = '" + ValueHandler.GetStringValue(Model.Pub_LS_Code2) + "', ");

            strSql.Append("Pub_LS_Code3 = '" + ValueHandler.GetStringValue(Model.Pub_LS_Code3) + "', ");

            strSql.Append("Pub_LS_Code4 = '" + ValueHandler.GetStringValue(Model.Pub_LS_Code4) + "', ");

            strSql.Append("Pub_LS_Code5 = '" + ValueHandler.GetStringValue(Model.Pub_LS_Code5) + "', ");

            strSql.Append("Pub_SA_Code1 = '" + ValueHandler.GetStringValue(Model.Pub_SA_Code1) + "', ");

            strSql.Append("Pub_SA_Code2 = '" + ValueHandler.GetStringValue(Model.Pub_SA_Code2) + "', ");

            strSql.Append("Pub_SA_Code3 = '" + ValueHandler.GetStringValue(Model.Pub_SA_Code3) + "', ");

            strSql.Append("Pub_SA_Name1 = '" + ValueHandler.GetStringValue(Model.Pub_SA_Name1) + "', ");

            strSql.Append("Pub_SA_Name2 = '" + ValueHandler.GetStringValue(Model.Pub_SA_Name2) + "', ");

            strSql.Append("Pub_SA_Name3 = '" + ValueHandler.GetStringValue(Model.Pub_SA_Name3) + "', ");

            strSql.Append("Pub_Title = '" + ValueHandler.GetStringValue(Model.Pub_Title) + "', ");

            strSql.Append("Pub_Pic1 = '" + ValueHandler.GetStringValue(Model.Pub_Pic1) + "', ");

            strSql.Append("Pub_Pic2 = '" + ValueHandler.GetStringValue(Model.Pub_Pic2) + "', ");

            strSql.Append("Pub_Pic3 = '" + ValueHandler.GetStringValue(Model.Pub_Pic3) + "', ");

            strSql.Append("Pub_Content = '" + ValueHandler.GetStringValue(Model.Pub_Content) + "', ");

            strSql.Append("Pub_ArticleSource = '" + ValueHandler.GetStringValue(Model.Pub_ArticleSource) + "', ");

            strSql.Append("Pub_KeyWords = '" + ValueHandler.GetStringValue(Model.Pub_KeyWords) + "'");

            return strSql.ToString();
        }
        #endregion
    }
}
