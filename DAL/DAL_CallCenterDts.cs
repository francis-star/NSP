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
using HCWeb2016;
using System.Data;
using Model;

namespace DAL
{
    public class DAL_CallCenterDts : SqlBase
    {
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

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code"></param>
        /// <returns></returns>
        public DataTable GetCallCenterDts(string Cust_Code)
        {
            string platForm = "";
            string tablename = "CXT_Customer";
            if (platForm == "1")
                tablename = "WQT_Customer";
            if (platForm == "2")
                tablename = "MQY_Customer";
            string sql = string.Format("select * FROM dbo.{1} WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code), tablename);
            return SearchData(sql);
        }
        #endregion

        #region 得到座机号码
        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <returns></returns>
        public DataTable GetPhone(string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Phone FROM CXT_OriginalDataValid WHERE ODD_Phone LIKE '%" + phone + "%'");
            return SearchData(sb.ToString());
        }
        #endregion

        #region 得到名称
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="name">企业名称</param>
        /// <param name="iPlat"></param>
        /// <returns></returns>
        public DataTable GetName(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Name FROM CXT_OriginalDataValid WHERE ODD_Name LIKE '%" + name + "%'");
            return SearchData(sb.ToString());
        }
        #endregion

        #region 根据座机号码、名称获取客户信息
        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="Phone">座机号码</param>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public DataTable GetComData(string Phone, string Name)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT V.ODD_Code,V.ODD_Name,V.ODD_Phone,V.ODD_LinkMan,V.ODD_LinkPhone,D.OD_ProvinceCode,D.OD_ProvinceName,D.OD_CityCode,D.OD_CityName,V.ODD_Address,V.ODD_IsBill,D.OD_BillMoney,D.JoinMan,D.OD_Provider FROM CXT_OriginalDataValid V,CXT_OriginalData D WHERE V.ODD_OD_Code=D.OD_Code");
            if (ValueHandler.GetStringValue(Phone) != "")
                sql.AppendFormat(" AND V.ODD_Phone = '{0}'", ValueHandler.GetStringValue(Phone));
            if (ValueHandler.GetStringValue(Name) != "")
                sql.AppendFormat(" AND V.ODD_Name = '{0}'", ValueHandler.GetStringValue(Name));
            return SearchData(sql.ToString());
        }
        #endregion

        #region 新增、修改营销管理中心数据
        /// <summary>
        /// 新增、修改营销管理中心数据
        /// </summary>
        public string Update(CXT_Customer Model)
        {
            #region old
            //string msg = CheckExistLinkPhone(Model);
            //if (msg=="")
            //{
            //    StringBuilder strSql = new StringBuilder();

            //    strSql.Append("\r IF NOT EXISTS(SELECT * FROM CXT_Customer WHERE Cust_Code ='" + ValueHandler.GetStringValue(Model.Cust_Code) + "')");
            //    strSql.Append("\r BEGIN ");
            //    strSql.Append("\r INSERT INTO CXT_Customer(");
            //    strSql.Append("Cust_Code,Cust_Name,Cust_OldName,Cust_NameKey,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_ProvinceCode,Cust_ProvinceName,Cust_CityCode,Cust_CityName,Cust_CountyCode,Cust_CountyName,Cust_Address,Cust_IsBill,Cust_BillMoney,Cust_BillNumber,Cust_Nature,Cust_KFVoice,Cust_Source,Cust_OpenDate,Cust_WH_Remark,Cust_WH_UserName,Cust_State,Cust_OperateTime,Cust_IsView ");
            //    strSql.Append(") SELECT ");
            //    strSql.Append(" Cust_Code = '" + GetCode() + "', ");
            //    strSql.Append(GetCXT_Customer(Model));
            //    strSql.Append("\r END");
            //    strSql.Append("\r ELSE");
            //    strSql.Append("\r BEGIN ");
            //    strSql.Append("UPDATE CXT_Customer SET ");
            //    strSql.Append(GetCXT_Customer(Model));
            //    strSql.Append(" WHERE Cust_Code ='" + ValueHandler.GetStringValue(Model.Cust_Code) + "'");
            //    strSql.Append("\r END");

            //    if (strSql.Length > 0)
            //    {
            //        return UpdateData(strSql.ToString()).ToString().ToLower();
            //    }
            //    else
            //    {
            //        throw new Exception("无数据操作！");
            //    }
            //}
            //else
            //{
            //    return msg;
            //}
            #endregion

            string msg = "";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_Code) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_Name) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_OldName) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_NameKey) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_Phone) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_Linkman) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_LinkPhone) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_ProvinceCode) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_ProvinceName) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_CityCode) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_CityName) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_CountyCode) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_CountyName) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_Address) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_IsBill) + "', ");
            strSql.Append("'" + ValueHandler.GetIntNumberValue(Model.Cust_BillMoney) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_BillNumber) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_Nature) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_KFVoice) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_Source) + "', ");
            strSql.Append("'" + ValueHandler.GetMarkStringDateValue(Model.Cust_OpenDate) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_WH_Remark) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_WH_UserName) + "', ");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_State) + "', ");
            strSql.Append("'" + ValueHandler.GetIntNumberValue(Model.Cust_IsView) + "',");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.JoinMan) + "',");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.fromPage) + "',");
            strSql.Append("'" + ValueHandler.GetIntNumberValue(Model.platForm) + "',");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.isCheck) + "',");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_BelongProvinceCode) + "',");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_BelongProvinceName) + "',");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_BelongCityCode) + "',");
            strSql.Append("'" + ValueHandler.GetStringValue(Model.Cust_BelongCityName) + "'");

            string sql = string.Format(@"exec [dbo].[SP_CustomerMaitain] {0}", strSql);
            try {
                DataTable dt = SearchData(sql);
                msg = dt.Rows[0][0].ToString();
            }
            catch (Exception ex) {
                msg = ex.Message;
            }
            return msg;
        }
        #endregion

        #region 得到数据
        /// <summary>
        /// 得到数据
        /// </summary>
        public string GetCXT_Customer(CXT_Customer Model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Cust_Name = '" + ValueHandler.GetStringValue(Model.Cust_Name) + "', ");

            strSql.Append("Cust_OldName = '" + ValueHandler.GetStringValue(Model.Cust_OldName) + "', ");

            strSql.Append("Cust_NameKey = '" + ValueHandler.GetStringValue(Model.Cust_NameKey) + "', ");

            strSql.Append("Cust_Phone = '" + ValueHandler.GetStringValue(Model.Cust_Phone) + "', ");

            strSql.Append("Cust_Linkman = '" + ValueHandler.GetStringValue(Model.Cust_Linkman) + "', ");

            strSql.Append("Cust_LinkPhone = '" + ValueHandler.GetStringValue(Model.Cust_LinkPhone) + "', ");

            strSql.Append("Cust_ProvinceCode = '" + ValueHandler.GetStringValue(Model.Cust_ProvinceCode) + "', ");

            strSql.Append("Cust_ProvinceName = '" + ValueHandler.GetStringValue(Model.Cust_ProvinceName) + "', ");

            strSql.Append("Cust_CityCode = '" + ValueHandler.GetStringValue(Model.Cust_CityCode) + "', ");

            strSql.Append("Cust_CityName = '" + ValueHandler.GetStringValue(Model.Cust_CityName) + "', ");

            strSql.Append("Cust_CountyCode = '" + ValueHandler.GetStringValue(Model.Cust_CountyCode) + "', ");

            strSql.Append("Cust_CountyName = '" + ValueHandler.GetStringValue(Model.Cust_CountyName) + "', ");

            strSql.Append("Cust_Address = '" + ValueHandler.GetStringValue(Model.Cust_Address) + "', ");

            strSql.Append("Cust_IsBill = '" + ValueHandler.GetStringValue(Model.Cust_IsBill) + "', ");

            strSql.Append("Cust_BillMoney = " + ValueHandler.GetIntNumberValue(Model.Cust_BillMoney) + ", ");

            strSql.Append("Cust_BillNumber = '" + ValueHandler.GetStringValue(Model.Cust_BillNumber) + "', ");

            strSql.Append("Cust_Nature = '" + ValueHandler.GetStringValue(Model.Cust_Nature) + "', ");

            strSql.Append("Cust_KFVoice = '" + ValueHandler.GetStringValue(Model.Cust_KFVoice) + "', ");

            strSql.Append("Cust_Source = '" + ValueHandler.GetStringValue(Model.Cust_Source) + "', ");

            strSql.Append("Cust_OpenDate = " + ValueHandler.GetMarkStringDateValue(Model.Cust_OpenDate) + ", ");

            strSql.Append("Cust_WH_Remark = '" + ValueHandler.GetStringValue(Model.Cust_WH_Remark) + "', ");

            strSql.Append("Cust_WH_UserName = '" + ValueHandler.GetStringValue(Model.Cust_WH_UserName) + "', ");

            strSql.Append("Cust_State = '" + ValueHandler.GetStringValue(Model.Cust_State) + "', ");

            string NowTime = DateTime.Now.ToString();
            strSql.Append("Cust_OperateTime = '" + NowTime + "', ");

            strSql.Append("Cust_IsView = " + ValueHandler.GetIntNumberValue(Model.Cust_IsView) + "");

            return strSql.ToString();
        }
        #endregion

        #region  判断是否存在加盟店联系电话

        /// <summary>
        /// 判断是否存在加盟店联系电话 By zhaohu
        /// </summary>
        /// <param name="linkPhone">联系电话</param>
        /// <param name="Cust_IsBill">是否计费</param>
        /// <returns>bool值</returns>
        public string CheckExistLinkPhone(CXT_Customer model)
        {
            string msg = "";
            string linkPhone = model.Cust_LinkPhone;
            string Cust_IsBill = model.Cust_IsBill;
            string fromPage = model.fromPage;
            string cusCode = model.Cust_Code;
           
            string sql = string.Format(@"SELECT *
                                        FROM CXT_Customer
                                        WHERE Cust_LinkPhone = '{0}' AND Cust_State NOT IN( '待审','退回') AND DataState = 0", linkPhone);
            if (fromPage == "CustomerReturns_A")
                sql = string.Format(@"SELECT *
                                        FROM CXT_Customer
                                        WHERE Cust_Code!='{1}' and Cust_LinkPhone = '{0}' AND Cust_State NOT IN( '待审','退回') AND DataState = 0", linkPhone, cusCode);
            DataTable dt = SearchData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Cust_IsBill == "是")
                    return "联系电话已经存在，请将是否计费<br/>改成否!";
                else
                    return msg;
            }
            else
                return msg;
        }

        #endregion

        #region 民企云

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code"></param>
        /// <returns></returns>
        public DataTable GetMQYCallCenterDts(string Cust_Code)
        {
            string sql = string.Format("SELECT * FROM MQY_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            return SearchData(sql);
        }
        #endregion

        #region 得到座机号码
        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <returns></returns>
        public DataTable GetMQYPhone(string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Phone FROM MQY_OriginalDataValid WHERE ODD_Phone LIKE '%" + phone + "%'");
            return SearchData(sb.ToString());
        }
        #endregion

        #region 得到名称
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="name">企业名称</param>
        /// <param name="iPlat"></param>
        /// <returns></returns>
        public DataTable GetMQYName(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Name FROM MQY_OriginalDataValid WHERE ODD_Name LIKE '%" + name + "%'");
            return SearchData(sb.ToString());
        }
        #endregion

        #region 根据座机号码、名称获取客户信息
        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="Phone">座机号码</param>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public DataTable GetMQYComData(string Phone, string Name)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT V.ODD_Code,V.ODD_Name,V.ODD_Phone,V.ODD_LinkMan,V.ODD_LinkPhone,D.OD_ProvinceCode,D.OD_ProvinceName,D.OD_CityCode,D.OD_CityName,V.ODD_Address,V.ODD_IsBill,D.OD_BillMoney,D.JoinMan,D.OD_Provider FROM MQY_OriginalDataValid V,MQY_OriginalData D WHERE V.ODD_OD_Code=D.OD_Code");
            if (ValueHandler.GetStringValue(Phone) != "")
                sql.AppendFormat(" AND V.ODD_Phone = '{0}'", ValueHandler.GetStringValue(Phone));
            if (ValueHandler.GetStringValue(Name) != "")
                sql.AppendFormat(" AND V.ODD_Name = '{0}'", ValueHandler.GetStringValue(Name));
            return SearchData(sql.ToString());
        }
        #endregion

        #region 新增、修改营销管理中心数据
        /// <summary>
        /// 新增、修改营销管理中心数据
        /// </summary>
        public string UpMQYdate(CXT_Customer Model)
        {
            if (!CheckMQYExistLinkPhone(Model.Cust_LinkPhone, Model.Cust_IsBill))
            {

                StringBuilder strSql = new StringBuilder();

                strSql.Append("\r IF NOT EXISTS(SELECT * FROM MQY_Customer WHERE Cust_Code ='" + ValueHandler.GetStringValue(Model.Cust_Code) + "')");
                strSql.Append("\r BEGIN ");
                strSql.Append("\r INSERT INTO MQY_Customer(");
                strSql.Append("Cust_Code,Cust_Name,Cust_OldName,Cust_NameKey,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_ProvinceCode,Cust_ProvinceName,Cust_CityCode,Cust_CityName,Cust_CountyCode,Cust_CountyName,Cust_Address,Cust_IsBill,Cust_BillMoney,Cust_BillNumber,Cust_Nature,Cust_KFVoice,Cust_Source,Cust_OpenDate,Cust_WH_Remark,Cust_WH_UserName,Cust_State,Cust_OperateTime,Cust_IsView ");
                strSql.Append(") SELECT ");
                strSql.Append(" Cust_Code = '" + GetCode() + "', ");
                strSql.Append(GetCXT_Customer(Model));
                strSql.Append("\r END");
                strSql.Append("\r ELSE");
                strSql.Append("\r BEGIN ");
                strSql.Append("UPDATE MQY_Customer SET ");
                strSql.Append(GetCXT_Customer(Model));
                strSql.Append(" WHERE Cust_Code ='" + ValueHandler.GetStringValue(Model.Cust_Code) + "'");
                strSql.Append("\r END");

                if (strSql.Length > 0)
                {
                    return UpdateData(strSql.ToString()).ToString().ToLower();
                }
                else
                {
                    throw new Exception("无数据操作！");
                }
            }
            else
            {
                return "-1";
            }
        }
        #endregion

        #region  判断是否存在加盟店联系电话

        /// <summary>
        /// 判断是否存在加盟店联系电话 By zhaohu
        /// </summary>
        /// <param name="linkPhone">联系电话</param>
        /// <param name="Cust_IsBill">是否计费</param>
        /// <returns>bool值</returns>
        public bool CheckMQYExistLinkPhone(string linkPhone, string Cust_IsBill)
        {
            string sql = string.Format(@"SELECT *
                                        FROM MQY_Customer
                                        WHERE Cust_LinkPhone = '{0}' AND Cust_State NOT IN( '待审','退回') AND DataState = 0", linkPhone);

            DataTable dt = SearchData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Cust_IsBill == "是")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #endregion

        #region 维权通

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code"></param>
        /// <returns></returns>
        public DataTable GetWQTCallCenterDts(string Cust_Code)
        {
            string sql = string.Format("SELECT * FROM WQT_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            return SearchData(sql);
        }
        #endregion

        #region 得到座机号码
        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <returns></returns>
        public DataTable GetWQTPhone(string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Phone FROM WQT_OriginalDataValid WHERE ODD_Phone LIKE '%" + phone + "%'");
            return SearchData(sb.ToString());
        }
        #endregion

        #region 得到名称
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="name">企业名称</param>
        /// <param name="iPlat"></param>
        /// <returns></returns>
        public DataTable GetWQTName(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Name FROM WQT_OriginalDataValid WHERE ODD_Name LIKE '%" + name + "%'");
            return SearchData(sb.ToString());
        }
        #endregion

        #region 根据座机号码、名称获取客户信息
        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="Phone">座机号码</param>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public DataTable GetWQTComData(string Phone, string Name)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT V.ODD_Code,V.ODD_Name,V.ODD_Phone,V.ODD_LinkMan,V.ODD_LinkPhone,D.OD_ProvinceCode,D.OD_ProvinceName,D.OD_CityCode,D.OD_CityName,V.ODD_Address,V.ODD_IsBill,D.OD_BillMoney,D.JoinMan,D.OD_Provider FROM WQT_OriginalDataValid V,WQT_OriginalData D WHERE V.ODD_OD_Code=D.OD_Code");
            if (ValueHandler.GetStringValue(Phone) != "")
                sql.AppendFormat(" AND V.ODD_Phone = '{0}'", ValueHandler.GetStringValue(Phone));
            if (ValueHandler.GetStringValue(Name) != "")
                sql.AppendFormat(" AND V.ODD_Name = '{0}'", ValueHandler.GetStringValue(Name));
            return SearchData(sql.ToString());
        }
        #endregion

        #region 新增、修改营销管理中心数据
        /// <summary>
        /// 新增、修改营销管理中心数据
        /// </summary>
        public string UpWQTdate(CXT_Customer Model)
        {
            if (!CheckWQTExistLinkPhone(Model.Cust_LinkPhone, Model.Cust_IsBill))
            {

                StringBuilder strSql = new StringBuilder();

                strSql.Append("\r IF NOT EXISTS(SELECT * FROM WQT_Customer WHERE Cust_Code ='" + ValueHandler.GetStringValue(Model.Cust_Code) + "')");
                strSql.Append("\r BEGIN ");
                strSql.Append("\r INSERT INTO WQT_Customer(");
                strSql.Append("Cust_Code,Cust_Name,Cust_OldName,Cust_NameKey,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_ProvinceCode,Cust_ProvinceName,Cust_CityCode,Cust_CityName,Cust_CountyCode,Cust_CountyName,Cust_Address,Cust_IsBill,Cust_BillMoney,Cust_BillNumber,Cust_Nature,Cust_KFVoice,Cust_Source,Cust_OpenDate,Cust_WH_Remark,Cust_WH_UserName,Cust_State,Cust_OperateTime,Cust_IsView ");
                strSql.Append(") SELECT ");
                strSql.Append(" Cust_Code = '" + GetCode() + "', ");
                strSql.Append(GetCXT_Customer(Model));
                strSql.Append("\r END");
                strSql.Append("\r ELSE");
                strSql.Append("\r BEGIN ");
                strSql.Append("UPDATE WQT_Customer SET ");
                strSql.Append(GetCXT_Customer(Model));
                strSql.Append(" WHERE Cust_Code ='" + ValueHandler.GetStringValue(Model.Cust_Code) + "'");
                strSql.Append("\r END");

                if (strSql.Length > 0)
                {
                    return UpdateData(strSql.ToString()).ToString().ToLower();
                }
                else
                {
                    throw new Exception("无数据操作！");
                }
            }
            else
            {
                return "-1";
            }
        }
        #endregion

        #region  判断是否存在加盟店联系电话

        /// <summary>
        /// 判断是否存在加盟店联系电话 By zhaohu
        /// </summary>
        /// <param name="linkPhone">联系电话</param>
        /// <param name="Cust_IsBill">是否计费</param>
        /// <returns>bool值</returns>
        public bool CheckWQTExistLinkPhone(string linkPhone, string Cust_IsBill)
        {
            string sql = string.Format(@"SELECT *
                                        FROM WQT_Customer
                                        WHERE Cust_LinkPhone = '{0}' AND Cust_State NOT IN( '待审','退回') AND DataState = 0", linkPhone);

            DataTable dt = SearchData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Cust_IsBill == "是")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #endregion

        #region 新消费宝典

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code"></param>
        /// <returns></returns>
        public DataTable GetXFBCallCenterDts(string Cust_Code)
        {
            string sql = string.Format("SELECT * FROM XFB_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            return SearchData(sql);
        }
        #endregion

        #region 得到座机号码
        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <returns></returns>
        public DataTable GetXFBPhone(string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Phone FROM XFB_OriginalDataValid WHERE ODD_Phone LIKE '%" + phone + "%'");
            return SearchData(sb.ToString());
        }
        #endregion

        #region 得到名称
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="name">企业名称</param>
        /// <param name="iPlat"></param>
        /// <returns></returns>
        public DataTable GetXFBName(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Name FROM XFB_OriginalDataValid WHERE ODD_Name LIKE '%" + name + "%'");
            return SearchData(sb.ToString());
        }
        #endregion

        #region 根据座机号码、名称获取客户信息
        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="Phone">座机号码</param>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public DataTable GetXFBComData(string Phone, string Name)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT V.ODD_Code,V.ODD_Name,V.ODD_Phone,V.ODD_LinkMan,V.ODD_LinkPhone,D.OD_ProvinceCode,D.OD_ProvinceName,D.OD_CityCode,D.OD_CityName,V.ODD_Address,V.ODD_IsBill,V.ODD_Remark,D.OD_BillMoney,D.JoinMan,D.OD_Provider FROM XFB_OriginalDataValid V,XFB_OriginalData D WHERE V.ODD_OD_Code=D.OD_Code");
            if (ValueHandler.GetStringValue(Phone) != "")
                sql.AppendFormat(" AND V.ODD_Phone = '{0}'", ValueHandler.GetStringValue(Phone));
            if (ValueHandler.GetStringValue(Name) != "")
                sql.AppendFormat(" AND V.ODD_Name = '{0}'", ValueHandler.GetStringValue(Name));
            return SearchData(sql.ToString());
        }
        #endregion

        #region 新增、修改营销管理中心数据
        /// <summary>
        /// 新增、修改营销管理中心数据
        /// </summary>
        public string UpXFBdate(CXT_Customer Model)
        {
            if (!CheckXFBExistLinkPhone(Model.Cust_LinkPhone, Model.Cust_IsBill))
            {

                StringBuilder strSql = new StringBuilder();

                strSql.Append("\r IF NOT EXISTS(SELECT * FROM XFB_Customer WHERE Cust_Code ='" + ValueHandler.GetStringValue(Model.Cust_Code) + "')");
                strSql.Append("\r BEGIN ");
                strSql.Append("\r INSERT INTO XFB_Customer(");
                strSql.Append("Cust_Code,Cust_Name,Cust_OldName,Cust_NameKey,Cust_Phone,Cust_Linkman,Cust_LinkPhone,Cust_ProvinceCode,Cust_ProvinceName,Cust_CityCode,Cust_CityName,Cust_CountyCode,Cust_CountyName,Cust_Address,Cust_IsBill,Cust_BillMoney,Cust_BillNumber,Cust_Nature,Cust_KFVoice,Cust_Source,Cust_OpenDate,Cust_WH_Remark,Cust_WH_UserName,Cust_State,Cust_OperateTime,Cust_IsView ");
                strSql.Append(") SELECT ");
                strSql.Append(" Cust_Code = '" + GetCode() + "', ");
                strSql.Append(GetCXT_Customer(Model));
                strSql.Append("\r END");
                strSql.Append("\r ELSE");
                strSql.Append("\r BEGIN ");
                strSql.Append("UPDATE XFB_Customer SET ");
                strSql.Append(GetCXT_Customer(Model));
                strSql.Append(" WHERE Cust_Code ='" + ValueHandler.GetStringValue(Model.Cust_Code) + "'");
                strSql.Append("\r END");

                if (strSql.Length > 0)
                {
                    return UpdateData(strSql.ToString()).ToString().ToLower();
                }
                else
                {
                    throw new Exception("无数据操作！");
                }
            }
            else
            {
                return "-1";
            }
        }
        #endregion

        #region  判断是否存在加盟店联系电话

        /// <summary>
        /// 判断是否存在加盟店联系电话 By zhaohu
        /// </summary>
        /// <param name="linkPhone">联系电话</param>
        /// <param name="Cust_IsBill">是否计费</param>
        /// <returns>bool值</returns>
        public bool CheckXFBExistLinkPhone(string linkPhone, string Cust_IsBill)
        {
            string sql = string.Format(@"SELECT *
                                        FROM XFB_Customer
                                        WHERE Cust_LinkPhone = '{0}' AND Cust_State NOT IN( '待审','退回') AND DataState = 0", linkPhone);

            DataTable dt = SearchData(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Cust_IsBill == "是")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #endregion

        #region 实时保

        #region 获取营销管理中心数据

        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="Cust_Code"></param>
        /// <returns></returns>
        public DataTable GetSSBCallCenterDts(string Cust_Code)
        {
            string sql = string.Format("SELECT * FROM SSB_Customer WHERE Cust_Code = '{0}'", ValueHandler.GetStringValue(Cust_Code));
            return SearchData(sql);
        }

        #endregion

        #region 得到座机号码

        /// <summary>
        /// 得到座机号码
        /// </summary>
        /// <returns></returns>
        public DataTable GetSSBPhone(string phone)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Phone FROM SSB_OriginalDataValid WHERE ODD_Phone LIKE '%" + phone + "%'");
            return SearchData(sb.ToString());
        }

        #endregion

        #region 得到名称

        /// <summary>
        /// 得到名称
        /// </summary>
        /// <param name="name">企业名称</param>
        /// <param name="iPlat"></param>
        /// <returns></returns>
        public DataTable GetSSBName(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 10 ODD_Name FROM SSB_OriginalDataValid WHERE ODD_Name LIKE '%" + name + "%'");
            return SearchData(sb.ToString());
        }

        #endregion

        #region 根据座机号码、名称获取客户信息

        /// <summary>
        /// 根据座机号码、名称获取客户信息
        /// </summary>
        /// <param name="Phone">座机号码</param>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public DataTable GetSSBComData(string Phone, string Name)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT V.ODD_Code,V.ODD_Name,V.ODD_Phone,V.ODD_LinkMan,V.ODD_LinkPhone,D.OD_ProvinceCode,D.OD_ProvinceName,D.OD_CityCode,D.OD_CityName,V.ODD_Address,V.ODD_IsBill
                            ,D.OD_BillMoney,D.JoinMan,D.OD_Provider FROM SSB_OriginalDataValid V,SSB_OriginalData D WHERE V.ODD_OD_Code=D.OD_Code");
            if (ValueHandler.GetStringValue(Phone) != "")
                sql.AppendFormat(" AND V.ODD_Phone = '{0}'", ValueHandler.GetStringValue(Phone));
            if (ValueHandler.GetStringValue(Name) != "")
                sql.AppendFormat(" AND V.ODD_Name = '{0}'", ValueHandler.GetStringValue(Name));
            return SearchData(sql.ToString());
        }

        #endregion 

        #endregion
    }
}
