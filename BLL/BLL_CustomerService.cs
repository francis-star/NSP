/////////////////////////////////////////////////////////////////////////////
//模块名：审核管理中心
//开发者：杨栋
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using DAL;
using HCWeb2016;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_CustomerService
    {
        DAL_CustomerService dAL_CustomerService = new DAL_CustomerService();

        #region 诚信通

        /// <summary>
        /// 得到客户状态
        /// </summary>
        /// <returns></returns>
        public string GetState()
        {
            DataTable dt = dAL_CustomerService.GetState();
            String json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetCustomer(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到客户数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCustomerCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetCustomerCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));

        }

        #endregion

        #region 审核

        public class SearchResult
        {
            public string Key { get; set; }
            public DataTable Dtable { get; set; }
            public string TabStr { get; set; }
            public int RowCount { get; set; }
        }
        public class SimilarInfoCount
        {
            public string AllCount { get; set; }
            public string PhoneCount { get; set; }
            public string AddressCount { get; set; }
            public string BillnoCount { get; set; }
            public string KeyCount { get; set; }
            public string TsPhoneCount { set; get; }
        }

        public string GetMQYSimilarInfoCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string phone = ValueHandler.GetStringValue(arr[2]);
            string billno = ValueHandler.GetStringValue(arr[3]);
            string addressinfo = ValueHandler.GetStringValue(arr[4]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            string code = ValueHandler.GetStringValue(arr[0]);
            SimilarInfoCount sic = new SimilarInfoCount();
            string AllCount = dAL_CustomerService.GetMQYSimilarCount(code, cuskey);
            string PhoneCount = dAL_CustomerService.GetMQYPhoneSimilarCount(code, phone);
            string AddressCount = dAL_CustomerService.GetMQYAddressSimilarCount(code, addressinfo);
            string BillnoCount = dAL_CustomerService.GetMQYBillNoSimilarCount(code, billno);
            string KeyCount = string.Empty;
            if (string.IsNullOrEmpty(cuskey))
            {
                KeyCount = "0";
            }
            else
            {
                KeyCount = dAL_CustomerService.GetMQYKeySimilarCount(code, cuskey);
            }
            sic.AddressCount = AddressCount;
            sic.AllCount = AllCount;
            sic.PhoneCount = PhoneCount;
            sic.BillnoCount = BillnoCount;
            sic.KeyCount = KeyCount;
            return JsonConvert.SerializeObject(sic);
        }

        public string GetWQTSimilarInfoCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string phone = ValueHandler.GetStringValue(arr[2]);
            string billno = ValueHandler.GetStringValue(arr[3]);
            string addressinfo = ValueHandler.GetStringValue(arr[4]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            string code = ValueHandler.GetStringValue(arr[0]);
            SimilarInfoCount sic = new SimilarInfoCount();
            string AllCount = dAL_CustomerService.GetWQTSimilarCount(code, cuskey);
            string PhoneCount = dAL_CustomerService.GetWQTPhoneSimilarCount(code, phone);
            string AddressCount = dAL_CustomerService.GetWQTAddressSimilarCount(code, addressinfo);
            string BillnoCount = dAL_CustomerService.GetWQTBillNoSimilarCount(code, billno);
            string KeyCount = string.Empty;
            if (string.IsNullOrEmpty(cuskey))
            {
                KeyCount = "0";
            }
            else
            {
                KeyCount = dAL_CustomerService.GetWQTKeySimilarCount(code, cuskey);
            }
            sic.AddressCount = AddressCount;
            sic.AllCount = AllCount;
            sic.PhoneCount = PhoneCount;
            sic.BillnoCount = BillnoCount;
            sic.KeyCount = KeyCount;
            return JsonConvert.SerializeObject(sic);
        }

        public string GetSimilarInfoCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string phone = ValueHandler.GetStringValue(arr[2]);
            string billno = ValueHandler.GetStringValue(arr[3]);
            string addressinfo = ValueHandler.GetStringValue(arr[4]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            string code = ValueHandler.GetStringValue(arr[0]);
            SimilarInfoCount sic = new SimilarInfoCount();
            string AllCount = dAL_CustomerService.GetSimilarCount(code, cuskey);
            string PhoneCount = dAL_CustomerService.GetPhoneSimilarCount(code, phone);
            string AddressCount = dAL_CustomerService.GetAddressSimilarCount(code, addressinfo);
            string BillnoCount = dAL_CustomerService.GetBillNoSimilarCount(code, billno);
            string KeyCount = string.Empty;
            if (string.IsNullOrEmpty(cuskey))
            {
                KeyCount = "0";
            }
            else
            {
                KeyCount = dAL_CustomerService.GetKeySimilarCount(code, cuskey);
            }
            sic.AddressCount = AddressCount;
            sic.AllCount = AllCount;
            sic.PhoneCount = PhoneCount;
            sic.BillnoCount = BillnoCount;
            sic.KeyCount = KeyCount;
            return JsonConvert.SerializeObject(sic);
        }

        public string GetMQYCustPhoneSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetMQYCustPhoneSimilarInfo(code, phone, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetMQYCustAddressSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string address = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetMQYCustAddressSimilarInfo(code, address, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetMQYBillNoSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string billno = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetMQYBillNoSimilarInfo(code, billno, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetMQYNameContainsKeySimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string key = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetMQYNameContainsKeySimilarInfo(code, key, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetWQTCustPhoneSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetWQTCustPhoneSimilarInfo(code, phone, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetWQTCustAddressSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string address = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetWQTCustAddressSimilarInfo(code, address, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetWQTBillNoSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string billno = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetWQTBillNoSimilarInfo(code, billno, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetWQTNameContainsKeySimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string key = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetWQTNameContainsKeySimilarInfo(code, key, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetCustPhoneSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetCustPhoneSimilarInfo(code, phone, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetCustAddressSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string address = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetCustAddressSimilarInfo(code, address, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetBillNoSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string billno = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetBillNoSimilarInfo(code, billno, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetNameContainsKeySimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string key = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetNameContainsKeySimilarInfo(code, key, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="obj"></param>
        public string GetSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string phone = ValueHandler.GetStringValue(arr[4]);
            string billno = ValueHandler.GetStringValue(arr[5]);
            string addressinfo = ValueHandler.GetStringValue(arr[6]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            DataTable dt = dAL_CustomerService.GetSimilarInfo(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetIntNumberValue(arr[2]), ValueHandler.GetIntNumberValue(arr[3]));
            //return JSON.DataTableToArrayList(dt);
            List<SearchResult> srList = new List<SearchResult>();
            SearchResult sr_raw = new SearchResult();
            sr_raw.Key = "all";
            sr_raw.TabStr = JSON.DataTableToArrayList(dt);
            sr_raw.RowCount = dt.Rows.Count;
            srList.Add(sr_raw);
            DataTable dt_phone = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Cust_LinkPhone"].ToString() == phone)
                {
                    dt_phone.ImportRow(dt.Rows[i]);
                }
            }
            if (dt_phone.Rows.Count > 0)
            {
                SearchResult sr_phone = new SearchResult();
                sr_phone.Key = "Cust_LinkPhone";
                // sr_phone.Dtable = dt_phone;
                sr_phone.TabStr = JSON.DataTableToArrayList(dt_phone);
                sr_phone.RowCount = dt_phone.Rows.Count;
                srList.Add(sr_phone);
            }
            DataTable dt_billno = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Cust_BillNumber"].ToString() == billno)
                {
                    dt_billno.ImportRow(dt.Rows[i]);
                }
            }
            if (dt_billno.Rows.Count > 0)
            {
                SearchResult sr_no = new SearchResult();
                sr_no.Key = "Cust_BillNumber";
                // sr_no.Dtable = dt_billno;
                sr_no.RowCount = dt_billno.Rows.Count;
                sr_no.TabStr = JSON.DataTableToArrayList(dt_billno);
                srList.Add(sr_no);
            }
            DataTable dt_key = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Cust_Name"].ToString().IndexOf(cuskey) != -1)
                {
                    dt_key.ImportRow(dt.Rows[i]);
                }
            }
            if (dt_key.Rows.Count > 0)
            {
                SearchResult sr_key = new SearchResult();
                sr_key.Key = "Cust_Name";
                //sr_key.Dtable = dt_key;
                sr_key.RowCount = dt_key.Rows.Count;
                sr_key.TabStr = JSON.DataTableToArrayList(dt_key);
                srList.Add(sr_key);
            }
            DataTable dt_address = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Cust_ProvinceName"].ToString() + dt.Rows[i]["Cust_CityName"].ToString() + dt.Rows[i]["Cust_Address"].ToString() == addressinfo)
                {
                    dt_address.ImportRow(dt.Rows[i]);
                }
            }
            if (dt_address.Rows.Count > 0)
            {
                SearchResult sr_add = new SearchResult();
                sr_add.Key = "Cust_Address";
                //sr_key.Dtable = dt_key;
                sr_add.RowCount = dt_address.Rows.Count;
                sr_add.TabStr = JSON.DataTableToArrayList(dt_address);
                srList.Add(sr_add);
            }

            return JsonConvert.SerializeObject(srList);
        }

        /// <summary>
        /// 获取相似信息数目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSimilarCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetSimilarCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCustomerByCode(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetCustomerByCode(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="obj"></param>
        public string Pass(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.Pass(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        /// 审核成功后 向JSXFWQTDB库中的客户表添加信息
        /// </summary>
        /// <param name="obj"></param>
        public string AddOtherCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            PhoneWebService.PhoneWebService phoneWebService = new PhoneWebService.PhoneWebService();
            return phoneWebService.AddCustomer(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5])).ToString().ToLower();
        }

        #endregion

        #region 修改
        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpBillHistory(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpBillHistory(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name, "").ToString().ToLower();
        }

        /// <summary>
        ///  修改营销成功
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string UpAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpAlreadyUse(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])).ToString().ToLower();
        }
        #endregion

        #region 民企云
        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetMQYCustomer(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到客户数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYCustomerCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetMQYCustomerCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));

        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="obj"></param>
        public string GetMQYSimilarInfo(object obj)
        {

            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[4]);
            string billno = ValueHandler.GetStringValue(arr[5]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            string addressinfo = ValueHandler.GetStringValue(arr[6]);
            DataTable dt = dAL_CustomerService.GetMQYSimilarInfo(code, cuskey, ValueHandler.GetIntNumberValue(arr[2]), ValueHandler.GetIntNumberValue(arr[3]));
            return JSON.DataTableToArrayList(dt);

        }

        /// <summary>
        /// 获取相似信息数目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYSimilarCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetMQYSimilarCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYCustomerByCode(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetMQYCustomerByCode(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="obj"></param>
        public string PassMQY(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.PassMQY(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }


        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpMQYBillHistory(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpMQYBillHistory(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        ///  修改营销成功
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string UpMQYAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpMQYAlreadyUse(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])).ToString().ToLower();
        }



        #endregion

        #region 维权通

        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetWQTCustomer(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到客户数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTCustomerCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetWQTCustomerCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));

        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="obj"></param>
        public string GetWQTSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[4]);
            string billno = ValueHandler.GetStringValue(arr[5]);
            string addressinfo = ValueHandler.GetStringValue(arr[6]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            string pageindex = ValueHandler.GetStringValue(arr[2]);
            string pagesize = ValueHandler.GetStringValue(arr[3]);
            DataTable dt = dAL_CustomerService.GetWQTSimilarInfo(code, cuskey, int.Parse(pageindex), int.Parse(pagesize));

            return JSON.DataTableToArrayList(dt);
        }

        /// <summary>
        /// 获取相似信息数目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTSimilarCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetWQTSimilarCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTCustomerByCode(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetWQTCustomerByCode(ValueHandler.GetStringValue(arr[0]));
            dt.Columns.Add("SMSMsg");
            if (dt.Rows[0]["Cust_CityName"].ToString() == "南京市")
                dt.Rows[0]["SMSMsg"] = dt.Rows[0]["Cust_Linkman"].ToString() + "：您好！" + dt.Rows[0]["Cust_Name"].ToString() + "已开通市消协E通315投诉快速处理平台（www.et315.com），用户名：" + dt.Rows[0]["Cust_BillNumber"].ToString() + "，密码：123，请妥善保管，若有投诉单会另行短信通知。询：02582230315";
            else
                dt.Rows[0]["SMSMsg"] = dt.Rows[0]["Cust_Linkman"].ToString() + "：您好！" + dt.Rows[0]["Cust_Name"].ToString() + "已开通消协投诉快速和解通道" + GetWebSite(ValueHandler.GetStringValue(dt.Rows[0]["Cust_CityName"])) + "，用户名：" + dt.Rows[0]["Cust_BillNumber"].ToString() + "，密码：123，有投诉时消协会以短信形式及时通知企业。询：02582230315";
            dt.Columns.Add("SMSMsgTwo");
            dt.Rows[0]["SMSMsgTwo"] = GetMsgs(ValueHandler.GetStringValue(dt.Rows[0]["Cust_Linkman"]), ValueHandler.GetStringValue(dt.Rows[0]["Cust_CityName"]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="obj"></param>
        public string PassWQT(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.PassWQT(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpWQTBillHistory(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpWQTBillHistory(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        ///  修改营销成功
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string UpWQTAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpWQTAlreadyUse(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])).ToString().ToLower();
        }

        /// <summary>
        /// 获取网址
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        private string GetWebSite(string cityName)
        {
            switch (cityName)
            {
                case "江苏省":
                    return "(" + BLL_User.CityUrl.江苏省 + ")";
                case "南京市":
                    return "(" + BLL_User.CityUrl.南京市 + ")";
                case "无锡市":
                    return "(" + BLL_User.CityUrl.无锡市 + ")";
                case "徐州市":
                    return "(" + BLL_User.CityUrl.徐州市 + ")";
                case "常州市":
                    return "(" + BLL_User.CityUrl.常州市 + ")";
                case "苏州市":
                    return "(" + BLL_User.CityUrl.苏州市 + ")";
                case "南通市":
                    return "(" + BLL_User.CityUrl.南通市 + ")";
                case "连云港市":
                    return "(" + BLL_User.CityUrl.连云港市 + ")";
                case "淮安市":
                    return "(" + BLL_User.CityUrl.淮安市 + ")";
                case "盐城市":
                    return "(" + BLL_User.CityUrl.盐城市 + ")";
                case "扬州市":
                    return "(" + BLL_User.CityUrl.扬州市 + ")";
                case "镇江市":
                    return "(" + BLL_User.CityUrl.镇江市 + ")";
                case "泰州市":
                    return "(" + BLL_User.CityUrl.泰州市 + ")";
                case "宿迁市":
                    return "(" + BLL_User.CityUrl.宿迁市 + ")";
                case "苏州园区":
                    return "(" + BLL_User.CityUrl.苏州园区 + ")";
            }
            return "";
        }

        /// <summary>
        /// 得到短信二
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetMsgs(string cust, string name)
        {
            switch (name)
            {
                case "南京市":
                    return cust + "：您好！为避免投诉信息泄露，使企业与消费者快速和解，请打开微信“添加朋友”中搜索“njxx315”关注消协微信平台，点击“诚信消费”选择“投诉受理”，登录后更改初始密码，询：02582230315";
                case "无锡市":
                    return "";
                case "徐州市":
                    return cust + "：您好！为避免投诉信息泄露，使企业与消费者快速和解，请打开微信“添加朋友”中搜索“hhzs315”关注消协微信平台，点击“消费投诉”选择“投诉受理”，登录后更改初始密码，询：02582230315";
                case "常州市":
                    return "";
                case "苏州市":
                    return "";
                case "南通市":
                    return cust + "：您好！为避免投诉信息泄露，使企业与消费者快速和解，请打开微信“添加朋友”中搜索“ntzsxx”关注消协微信平台，点击“消费投诉”选择“投诉受理”，登录后更改初始密码，询：02582230315";
                case "连云港市":
                    return cust + "：您好！为避免投诉信息泄露，使企业与消费者快速和解，请打开微信“添加朋友”中搜索“lygxx315”关注消协微信平台，点击“消费投诉”选择“投诉受理”，登录后更改初始密码，询：02582230315";
                case "淮安市":
                    return cust + "：您好！为避免投诉信息泄露，使企业与消费者快速和解，请打开微信“添加朋友”中搜索“ha96315”关注消协微信平台，点击“放心消费”选择“投诉受理”，登录后更改初始密码，询：02582230315";
                case "盐城市":
                    return cust + "：您好！为避免投诉信息泄露，使企业与消费者快速和解，请打开微信“添加朋友”中搜索“yczsxx”关注消协微信平台，点击“消费投诉”选择“投诉受理”，登录后更改初始密码，询：02582230315";
                case "扬州市":
                    return "";
                case "镇江市":
                    return cust + "：您好！为避免投诉信息泄露，使企业与消费者快速和解，请打开微信“添加朋友”中搜索“zj-xiaofei”关注消协微信平台，点击“诚信消费”选择“投诉受理”，登录后更改初始密码，询：02582230315";
                case "泰州市":
                    return cust + "：您好！为避免投诉信息泄露，使企业与消费者快速和解，请打开微信“添加朋友”中搜索“jstz315”关注消协微信平台，点击“诚信消费”选择“投诉受理”，登录后更改初始密码，询：02582230315";
                case "宿迁市":
                    return "";
                case "苏州园区":
                    return "";
            }
            return "";
        }

        #endregion

        #region 新消费宝典

        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetXFBCustomer(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到客户数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBCustomerCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetXFBCustomerCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));

        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="obj"></param>
        public string GetXFBSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[4]);
            string billno = ValueHandler.GetStringValue(arr[5]);
            string addressinfo = ValueHandler.GetStringValue(arr[6]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            string pageindex = ValueHandler.GetStringValue(arr[2]);
            string pagesize = ValueHandler.GetStringValue(arr[3]);
            DataTable dt = dAL_CustomerService.GetXFBSimilarInfo(code, cuskey, int.Parse(pageindex), int.Parse(pagesize));
            return JSON.DataTableToArrayList(dt);
        }

        /// <summary>
        /// 获取相似信息数目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBSimilarCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetXFBSimilarCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBCustomerByCode(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetXFBCustomerByCode(ValueHandler.GetStringValue(arr[0]));
            dt.Columns.Add("SMSMsg");
            dt.Rows[0]["SMSMsg"] = $"{dt.Rows[0]["Cust_Name"].ToString()}，您好！{dt.Rows[0]["Cust_BillNumber"].ToString()}已成功定购“诚信通个人版”服务，微信搜索“品质消费”公众号，关注后获取更多会员权益。服务热线：02583729711";
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="obj"></param>
        public string PassXFB(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.PassXFB(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpXFBBillHistory(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpXFBBillHistory(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        ///  修改营销成功
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string UpXFBAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpXFBAlreadyUse(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])).ToString().ToLower();
        }

        #endregion

        #region 实时保

        /// <summary>
        /// 得到客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBCustomer(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetSSBCustomer(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到客户数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBCustomerCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetSSBCustomerCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]),
                                                           ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]),
                                                           ValueHandler.GetIntNumberValue(arr[8]), ValueHandler.GetIntNumberValue(arr[9]));

        }

        /// <summary>
        /// 获取相似信息
        /// </summary>
        /// <param name="obj"></param>
        public string GetSSBSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[4]);
            string billno = ValueHandler.GetStringValue(arr[5]);
            string addressinfo = ValueHandler.GetStringValue(arr[6]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            string pageindex = ValueHandler.GetStringValue(arr[2]);
            string pagesize = ValueHandler.GetStringValue(arr[3]);
            DataTable dt = dAL_CustomerService.GetSSBSimilarInfo(code, cuskey, billno, phone, int.Parse(pageindex), int.Parse(pagesize));

            return JSON.DataTableToArrayList(dt);
        }

        public string GetSSBSimilarInfoCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string phone = ValueHandler.GetStringValue(arr[2]);
            string billno = ValueHandler.GetStringValue(arr[3]);
            string addressinfo = ValueHandler.GetStringValue(arr[4]);
            string cuskey = ValueHandler.GetStringValue(arr[1]);
            string code = ValueHandler.GetStringValue(arr[0]);
            SimilarInfoCount sic = new SimilarInfoCount();
            string AllCount = dAL_CustomerService.GetSSBSimilarCount(code, cuskey, billno, phone);
            string PhoneCount = dAL_CustomerService.GetSSBPhoneSimilarCount(code, phone);
            string AddressCount = dAL_CustomerService.GetSSBAddressSimilarCount(code, addressinfo);
            string BillnoCount = dAL_CustomerService.GetSSBBillNoSimilarCount(code, billno);
            string TsPhoneCount = dAL_CustomerService.GetSSBCustTsPhoneSimilarInfoCount(code, billno, phone);
            string KeyCount = string.Empty;
            if (string.IsNullOrEmpty(cuskey))
            {
                KeyCount = "0";
            }
            else
            {
                KeyCount = dAL_CustomerService.GetSSBKeySimilarCount(code, cuskey);
            }
            sic.AddressCount = AddressCount;
            sic.AllCount = AllCount;
            sic.PhoneCount = PhoneCount;
            sic.BillnoCount = BillnoCount;
            sic.KeyCount = KeyCount;
            sic.TsPhoneCount = TsPhoneCount;
            return JsonConvert.SerializeObject(sic);
        }

        public string GetSSBPhoneSimilarCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetSSBCustPhoneSimilarInfo(code, phone, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetSSBAddressSimilarCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string address = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetSSBCustAddressSimilarInfo(code, address, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetSSBBillNoSimilarCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string billno = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetSSBBillNoSimilarInfo(code, billno, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        public string GetSSBNameContainsKeySimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string key = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetSSBNameContainsKeySimilarInfo(code, key, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        /// <summary>
        /// 获取相似信息数目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBSimilarCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.GetSSBSimilarCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[2]));
        }

        /// <summary>
        /// 得到一条客户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBCustomerByCode(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CustomerService.GetSSBCustomerByCode(ValueHandler.GetStringValue(arr[0]));
            //dt.Columns.Add("SMSMsg");
            //if (dt.Rows[0]["Cust_CityName"].ToString() == "南京市")
            //    dt.Rows[0]["SMSMsg"] = dt.Rows[0]["Cust_Linkman"].ToString() + "：您好！" + dt.Rows[0]["Cust_Name"].ToString() + "已开通市消协E通315投诉快速处理平台（www.et315.com），用户名：" + dt.Rows[0]["Cust_BillNumber"].ToString() + "，密码：123，请妥善保管，若有投诉单会另行短信通知。询：02582230315";
            //else
            //    dt.Rows[0]["SMSMsg"] = dt.Rows[0]["Cust_Linkman"].ToString() + "：您好！" + dt.Rows[0]["Cust_Name"].ToString() + "已开通消协投诉快速和解通道" + GetWebSite(ValueHandler.GetStringValue(dt.Rows[0]["Cust_CityName"])) + "，用户名：" + dt.Rows[0]["Cust_BillNumber"].ToString() + "，密码：123，有投诉时消协会以短信形式及时通知企业。询：02582230315";
            //dt.Columns.Add("SMSMsgTwo");
            //dt.Rows[0]["SMSMsgTwo"] = GetMsgs(ValueHandler.GetStringValue(dt.Rows[0]["Cust_Linkman"]), ValueHandler.GetStringValue(dt.Rows[0]["Cust_CityName"]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="obj"></param>
        public string PassSSB(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.PassSSB(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        /// 修改是否计费
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpSSBBillHistory(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpSSBBillHistory(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name).ToString().ToLower();
        }

        /// <summary>
        ///  修改营销成功
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string UpSSBAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CustomerService.UpSSBAlreadyUse(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])).ToString().ToLower();
        }

        #region 审核匹配信息展示

        /// <summary>
        /// 电话匹配
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBCustPhoneSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetSSBCustPhoneSimilarInfo(code, phone, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        /// <summary>
        /// 投诉号码
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBCustTsPhoneSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string phone = ValueHandler.GetStringValue(arr[1]);
            string billno = ValueHandler.GetStringValue(arr[2]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[3]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[4]));
            DataTable dt = dAL_CustomerService.GetSSBCustTsPhoneSimilarInfo(code, phone, billno, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        /// <summary>
        /// 地址匹配
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBCustAddressSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string address = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetSSBCustAddressSimilarInfo(code, address, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        /// <summary>
        /// 计费号码匹配
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBBillNoSimilarInfo(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string code = ValueHandler.GetStringValue(arr[0]);
            string billno = ValueHandler.GetStringValue(arr[1]);
            int pageindex = int.Parse(ValueHandler.GetStringValue(arr[2]));
            int pagesize = int.Parse(ValueHandler.GetStringValue(arr[3]));
            DataTable dt = dAL_CustomerService.GetSSBBillNoSimilarInfo(code, billno, pageindex, pagesize);
            return JSON.DataTableToArrayList(dt);
        }

        #endregion

        #endregion
    }
}
