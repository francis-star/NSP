////////////////////////////://///////////////////////////////////////////////
//模块名：分类信息设定
//开发者：田维华
//开发时间：2016年11月10日
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

namespace BLL
{
    public class BLL_LevelSetDts
    {
        DAL_LevelSetDts dAL_LevelSetDts = new DAL_LevelSetDts();

        #region 获取模块信息（左侧菜单：资讯信息维护、企业信息维护等）
        /// <summary>
        /// 获取模块信息（左侧菜单：资讯信息维护、企业信息维护等）
        /// </summary>
        /// <returns></returns>
        public string GetESysModule()
        {
            DataTable dt = dAL_LevelSetDts.GetESysModule();
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 获取节点信息
        /// <summary>
        /// 获取节点信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetLevelSet(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_LevelSetDts.GetLevelSet(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 新增节点信息
        /// <summary>
        /// 新增节点信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string InsertLevelSet(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_LevelSetDts.InsertLevelSet(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
        }
        #endregion

        #region 修改节点信息
        /// <summary>
        /// 修改节点信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpdateLevelSet(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_LevelSetDts.UpdateLevelSet(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]));
        }
        #endregion

        

        #region 获取用户的信息(E_User表)
        /// <summary>
        /// 获取用户的信息(E_User表)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetUsers(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_LevelSetDts.GetUsers(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion
    }
}
