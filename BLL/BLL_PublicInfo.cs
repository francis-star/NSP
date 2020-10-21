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
using System.Collections;
using HCWeb2016;
using System.Data;

namespace BLL
{
    public class BLL_PublicInfo
    {
        DAL_PublicInfo dAL_PublicInfo = new DAL_PublicInfo();

        #region 移动端、平台名称、信息类别、信息大类 四级联动
        /// <summary>
        /// 移动端、平台名称、信息类别、信息大类 四级联动
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSearchStrData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_PublicInfo.GetSearchStrData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), BLL_User.User_Code);
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        public string GetList()
        {
            DataTable dt = dAL_PublicInfo.GetList();
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        public string UpdateContent(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool ResultStr = dAL_PublicInfo.UpdateContent(arr[0].ToString(), arr[1].ToString());
            return ResultStr.ToString().ToLower();
        }

        /// <summary>
        /// 查询外呼人员
        /// </summary>
        /// <returns></returns>
        public string GetWHUser()
        {
            DataTable dt = dAL_PublicInfo.GetWHUser();
            return JSON.DataTableToTreeList(dt);
        }

        #region 查询省、市、区

        /// <summary>
        /// 查询省、市、区
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetArea(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_PublicInfo.GetArea(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        public string GetAreaTree(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            TreeArea treeArea = new TreeArea();
            treeArea.id = ValueHandler.GetStringValue(arr[1]);
            treeArea.text = "全国";
            DataTable dt = dAL_PublicInfo.GetArea(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            List<TreeArea> children = new List<TreeArea>();
            foreach (DataRow dr in dt.Rows)
                children.Add(new TreeArea() { id=dr["SA_Code"].ToString(),text = dr["SA_Name"].ToString() });
            treeArea.children = children;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(treeArea);
            return json.Replace(",\"children\":null", "");
        }

        #endregion

        #region 获取资讯信息
        /// <summary>
        /// 获取资讯信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetPublicInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_PublicInfo.GetPublicInfo(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]));
            string json = "";
            if (arr[13].ToString() == "0")
                json = JSON.DataTableToArrayList(dt);
            else
                json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 获取资讯信息数量
        /// <summary>
        /// 获取资讯信息数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetPublicInfoCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_PublicInfo.GetDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]), ValueHandler.GetStringValue(arr[11]), ValueHandler.GetStringValue(arr[12]));
            return Data;
        }
        #endregion

        #region 删除资讯信息
        /// <summary>
        /// 删除资讯信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeletePublicInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool ResultStr = dAL_PublicInfo.DeletePublicInfo(ValueHandler.GetStringValue(arr[0]));
            return ResultStr.ToString().ToLower();
        }
        #endregion
    }

    public class TreeArea
    {
        /// <summary>
        /// 节点
        /// </summary>
        public string id { set; get; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string text { set; get; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeArea> children { set; get; }
    }
}