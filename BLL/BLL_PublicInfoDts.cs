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
using DAL;
using Model;
using System.Collections;
using HCWeb2016;
using System.Data;

namespace BLL
{
    public class BLL_PublicInfoDts
    {
        DAL_PublicInfoDts dAL_PublicInfoDts = new DAL_PublicInfoDts();
        XXSD_PublicInfo model = new XXSD_PublicInfo();

        #region 查询资讯信息
        /// <summary>
        /// 查询资讯信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetPublicInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_PublicInfoDts.GetPublicInfo(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 新增、修改资讯信息
        /// <summary>
        /// 新增、修改资讯信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Update(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            model.Pub_LS_Code1 = ValueHandler.GetStringValue(arr[0]);
            model.Pub_LS_Code2 = ValueHandler.GetStringValue(arr[1]);
            model.Pub_LS_Code3 = ValueHandler.GetStringValue(arr[2]);
            model.Pub_LS_Code4 = ValueHandler.GetStringValue(arr[3]);
            model.Pub_LS_Code5 = ValueHandler.GetStringValue(arr[4]);
            model.Pub_SA_Code1 = ValueHandler.GetStringValue(arr[5]);
            model.Pub_SA_Code2 = ValueHandler.GetStringValue(arr[6]);
            model.Pub_SA_Code3 = ValueHandler.GetStringValue(arr[7]);
            model.Pub_Title = ValueHandler.GetStringValue(arr[8]);
            model.Pub_Pic1 = ValueHandler.GetStringValue(arr[9]);
            model.Pub_Pic2 = ValueHandler.GetStringValue(arr[10]);
            model.Pub_Pic3 = ValueHandler.GetStringValue(arr[11]);
            model.Pub_Content = ValueHandler.GetStringValue(arr[12]);
            model.Pub_ArticleSource = ValueHandler.GetStringValue(arr[13]);
            model.Pub_KeyWords = ValueHandler.GetStringValue(arr[14]);
            model.Pub_Code = ValueHandler.GetStringValue(arr[15]);
            model.Pub_SA_Name1 = ValueHandler.GetStringValue(arr[16]);
            model.Pub_SA_Name2 = ValueHandler.GetStringValue(arr[17]);
            model.Pub_SA_Name3 = ValueHandler.GetStringValue(arr[18]);
            model.JoinMan = ValueHandler.GetStringValue(BLL_User.User_Name);
            bool Data = dAL_PublicInfoDts.Update(model);
            return Data.ToString().ToLower();
        }
        #endregion
    }
}
