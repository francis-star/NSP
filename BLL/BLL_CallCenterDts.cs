/////////////////////////////////////////////////////////////////////////////
//模块名：营销管理中心
//开发者：田维华
//开发时间：2016年11月25日
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
using System.Net;

namespace BLL
{
    public class BLL_CallCenterDts
    {
        DAL_CallCenterDts dAL_CallCenterDts = new DAL_CallCenterDts();

        #region 查询省、市、区
        /// <summary>
        /// 查询省、市、区
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetArea(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetArea(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCallCenterDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetCallCenterDts(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 得到座机号码
        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetPhone(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetPhone(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 得到名称
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetName(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetName(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 根据座机号码、名称获取客户信息
        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetComData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetComData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 修改营销管理中心数据

        public string Update(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            CXT_Customer Model = new CXT_Customer();
            Model.Cust_Code = ValueHandler.GetStringValue(arr[0]);
            Model.Cust_Name = ValueHandler.GetStringValue(arr[1]);
            Model.Cust_OldName = ValueHandler.GetStringValue(arr[2]);
            Model.Cust_NameKey = ValueHandler.GetStringValue(arr[3]);
            Model.Cust_Phone = ValueHandler.GetStringValue(arr[4]);
            Model.Cust_Linkman = ValueHandler.GetStringValue(arr[5]);
            Model.Cust_LinkPhone = ValueHandler.GetStringValue(arr[6]);
            Model.Cust_ProvinceCode = ValueHandler.GetStringValue(arr[7]);
            Model.Cust_ProvinceName = ValueHandler.GetStringValue(arr[8]);
            Model.Cust_CityCode = ValueHandler.GetStringValue(arr[9]);
            Model.Cust_CityName = ValueHandler.GetStringValue(arr[10]);
            Model.Cust_CountyCode = ValueHandler.GetStringValue(arr[11]);
            Model.Cust_CountyName = ValueHandler.GetStringValue(arr[12]);
            Model.Cust_Address = ValueHandler.GetStringValue(arr[13]);
            Model.Cust_IsBill = ValueHandler.GetStringValue(arr[14]);
            Model.Cust_BillMoney = ValueHandler.GetIntNumberValue(arr[15]);
            Model.Cust_BillNumber = ValueHandler.GetStringValue(arr[16]);
            Model.Cust_Nature = ValueHandler.GetStringValue(arr[17]);
            Model.Cust_KFVoice = ValueHandler.GetStringValue(arr[18]);
            Model.Cust_Source = ValueHandler.GetStringValue(arr[19]);
            Model.Cust_WH_Remark = ValueHandler.GetStringValue(arr[20]);
            if (Model.Cust_Code == "")
                Model.Cust_State = ValueHandler.GetStringValue("待审");
            Model.Cust_IsView = ValueHandler.GetIntNumberValue(arr[21]);
            Model.fromPage = ValueHandler.GetStringValue(arr[22]);
            Model.platForm = ValueHandler.GetStringValue(arr[23]);
            Model.Cust_WH_UserName = ValueHandler.GetStringValue(arr[24]);
            Model.JoinMan = BLL_User.User_Name;
            Model.isCheck = ValueHandler.GetStringValue(arr[25]);
            Model.Cust_BelongProvinceName = ValueHandler.GetStringValue(arr[27]);
            Model.Cust_BelongProvinceCode = ValueHandler.GetStringValue(arr[26]);
            Model.Cust_BelongCityCode = ValueHandler.GetStringValue(arr[28]);
            Model.Cust_BelongCityName = ValueHandler.GetStringValue(arr[29]);
            string Data = dAL_CallCenterDts.Update(Model);
            return Data;
        }
        #endregion

        #region 获取当前登陆人姓名
        /// <summary>
        /// 获取当前登陆人姓名
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            string UserName = ValueHandler.GetStringValue(BLL_User.User_Name);
            return UserName;
        }
        #endregion

        #region 百度地图检索接口

        public string GetSearchForBaiduAPI(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Address = "http://api.map.baidu.com/place/v2/search?query=" + arr[0].ToString() + "&region=" + arr[1].ToString() + "&city_limit=true&output=json&ak=31hnqKEhOUOWFXXxvWYXiM1v";
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string str = client.DownloadString(Address);
            return str;
        }

        #endregion

        #region 民企云

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYCallCenterDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetMQYCallCenterDts(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 得到座机号码
        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYPhone(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetMQYPhone(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 得到名称
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYName(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetMQYName(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 根据座机号码、名称获取客户信息
        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYComData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetMQYComData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 修改营销管理中心数据

        public string UpMQYdate(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            CXT_Customer Model = new CXT_Customer();
            Model.Cust_Code = ValueHandler.GetStringValue(arr[0]);
            Model.Cust_Name = ValueHandler.GetStringValue(arr[1]);
            Model.Cust_OldName = ValueHandler.GetStringValue(arr[2]);
            Model.Cust_NameKey = ValueHandler.GetStringValue(arr[3]);
            Model.Cust_Phone = ValueHandler.GetStringValue(arr[4]);
            Model.Cust_Linkman = ValueHandler.GetStringValue(arr[5]);
            Model.Cust_LinkPhone = ValueHandler.GetStringValue(arr[6]);
            Model.Cust_ProvinceCode = ValueHandler.GetStringValue(arr[7]);
            Model.Cust_ProvinceName = ValueHandler.GetStringValue(arr[8]);
            Model.Cust_CityCode = ValueHandler.GetStringValue(arr[9]);
            Model.Cust_CityName = ValueHandler.GetStringValue(arr[10]);
            Model.Cust_CountyCode = ValueHandler.GetStringValue(arr[11]);
            Model.Cust_CountyName = ValueHandler.GetStringValue(arr[12]);
            Model.Cust_Address = ValueHandler.GetStringValue(arr[13]);
            Model.Cust_IsBill = ValueHandler.GetStringValue(arr[14]);
            Model.Cust_BillMoney = ValueHandler.GetIntNumberValue(arr[15]);
            Model.Cust_BillNumber = ValueHandler.GetStringValue(arr[16]);
            Model.Cust_Nature = ValueHandler.GetStringValue(arr[17]);
            Model.Cust_KFVoice = ValueHandler.GetStringValue(arr[18]);
            Model.Cust_Source = ValueHandler.GetStringValue(arr[19]);
            Model.Cust_WH_Remark = ValueHandler.GetStringValue(arr[20]);
            Model.Cust_WH_UserName = ValueHandler.GetStringValue(BLL_User.User_Name);
            Model.Cust_State = ValueHandler.GetStringValue("待审");
            Model.Cust_IsView = ValueHandler.GetIntNumberValue(arr[21]);
            Model.fromPage = ValueHandler.GetStringValue(arr[22]);
            string Data = dAL_CallCenterDts.UpMQYdate(Model);
            return Data;
        }
        #endregion

        #endregion

        #region 维权通

        #region 获取营销管理中心数据

        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTCallCenterDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetWQTCallCenterDts(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion
        
        #region 得到座机号码

        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTPhone(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetWQTPhone(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion

        #region 得到名称

        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTName(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetWQTName(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion

        #region 根据座机号码、名称获取客户信息

        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTComData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetWQTComData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion

        #region 修改营销管理中心数据

        public string UpWQTdate(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            CXT_Customer Model = new CXT_Customer();
            Model.Cust_Code = ValueHandler.GetStringValue(arr[0]);
            Model.Cust_Name = ValueHandler.GetStringValue(arr[1]);
            Model.Cust_OldName = ValueHandler.GetStringValue(arr[2]);
            Model.Cust_NameKey = ValueHandler.GetStringValue(arr[3]);
            Model.Cust_Phone = ValueHandler.GetStringValue(arr[4]);
            Model.Cust_Linkman = ValueHandler.GetStringValue(arr[5]);
            Model.Cust_LinkPhone = ValueHandler.GetStringValue(arr[6]);
            Model.Cust_ProvinceCode = ValueHandler.GetStringValue(arr[7]);
            Model.Cust_ProvinceName = ValueHandler.GetStringValue(arr[8]);
            Model.Cust_CityCode = ValueHandler.GetStringValue(arr[9]);
            Model.Cust_CityName = ValueHandler.GetStringValue(arr[10]);
            Model.Cust_CountyCode = ValueHandler.GetStringValue(arr[11]);
            Model.Cust_CountyName = ValueHandler.GetStringValue(arr[12]);
            Model.Cust_Address = ValueHandler.GetStringValue(arr[13]);
            Model.Cust_IsBill = ValueHandler.GetStringValue(arr[14]);
            Model.Cust_BillMoney = ValueHandler.GetIntNumberValue(arr[15]);
            Model.Cust_BillNumber = ValueHandler.GetStringValue(arr[16]);
            Model.Cust_Nature = ValueHandler.GetStringValue(arr[17]);
            Model.Cust_KFVoice = ValueHandler.GetStringValue(arr[18]);
            Model.Cust_Source = ValueHandler.GetStringValue(arr[19]);
            Model.Cust_WH_Remark = ValueHandler.GetStringValue(arr[20]);
            Model.Cust_WH_UserName = ValueHandler.GetStringValue(BLL_User.User_Name);
            Model.Cust_State = ValueHandler.GetStringValue("待审");
            Model.Cust_IsView = ValueHandler.GetIntNumberValue(arr[21]);
            Model.fromPage = ValueHandler.GetStringValue(arr[22]);
            string Data = dAL_CallCenterDts.UpWQTdate(Model);
            return Data;
        }

        #endregion

        #endregion

        #region 新消费宝典

        #region 获取营销管理中心数据

        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBCallCenterDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetXFBCallCenterDts(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion

        #region 得到座机号码
        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBPhone(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetXFBPhone(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 得到名称
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBName(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetXFBName(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 根据座机号码、名称获取客户信息
        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBComData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetXFBComData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 修改营销管理中心数据

        public string UpXFBdate(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            CXT_Customer Model = new CXT_Customer();
            Model.Cust_Code = ValueHandler.GetStringValue(arr[0]);
            Model.Cust_Name = ValueHandler.GetStringValue(arr[1]);
            Model.Cust_OldName = ValueHandler.GetStringValue(arr[2]);
            Model.Cust_NameKey = ValueHandler.GetStringValue(arr[3]);
            Model.Cust_Phone = ValueHandler.GetStringValue(arr[4]);
            Model.Cust_Linkman = ValueHandler.GetStringValue(arr[5]);
            Model.Cust_LinkPhone = ValueHandler.GetStringValue(arr[6]);
            Model.Cust_ProvinceCode = ValueHandler.GetStringValue(arr[7]);
            Model.Cust_ProvinceName = ValueHandler.GetStringValue(arr[8]);
            Model.Cust_CityCode = ValueHandler.GetStringValue(arr[9]);
            Model.Cust_CityName = ValueHandler.GetStringValue(arr[10]);
            Model.Cust_CountyCode = ValueHandler.GetStringValue(arr[11]);
            Model.Cust_CountyName = ValueHandler.GetStringValue(arr[12]);
            Model.Cust_Address = ValueHandler.GetStringValue(arr[13]);
            Model.Cust_IsBill = ValueHandler.GetStringValue(arr[14]);
            Model.Cust_BillMoney = ValueHandler.GetIntNumberValue(arr[15]);
            Model.Cust_BillNumber = ValueHandler.GetStringValue(arr[16]);
            Model.Cust_Nature = ValueHandler.GetStringValue(arr[17]);
            Model.Cust_KFVoice = ValueHandler.GetStringValue(arr[18]);
            Model.Cust_Source = ValueHandler.GetStringValue(arr[19]);
            Model.Cust_WH_Remark = ValueHandler.GetStringValue(arr[20]);
            Model.Cust_WH_UserName = ValueHandler.GetStringValue(BLL_User.User_Name);
            Model.Cust_State = ValueHandler.GetStringValue("待审");
            Model.Cust_IsView = ValueHandler.GetIntNumberValue(arr[21]);
            Model.fromPage = ValueHandler.GetStringValue(arr[22]);
            string Data = dAL_CallCenterDts.UpXFBdate(Model);
            return Data;
        }
        #endregion

        #endregion

        #region 实时保

        #region 获取营销管理中心数据

        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBCallCenterDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetSSBCallCenterDts(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion

        #region 得到座机号码

        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBPhone(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetSSBPhone(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion

        #region 得到名称

        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBName(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetSSBName(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion

        #region 根据座机号码、名称获取客户信息

        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBComData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenterDts.GetSSBComData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #endregion 

        #endregion
    }
}
