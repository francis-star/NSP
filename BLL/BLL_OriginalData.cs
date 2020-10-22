////////////////////////////://///////////////////////////////////////////////
//模块名：维权通数据筛选
//开发者：赵虎
//开发时间：2016年11月25日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using DAL;
using DAL.InMolde;
using HCWeb2016;

namespace BLL
{
    public class BLL_OriginalData
    {
        DAL_OriginalData dAL_OriginalData = new DAL_OriginalData();

        #region 新消费宝典

        #region 列表查询

        /// <summary>
        /// 得到原数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBOriginalData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetXFBOriginalData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到原数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBOriginalDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetXFBOriginalDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]));
        }

        #endregion

        #region 导入

        /// <summary>
        /// 验证是否手机格式正确
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns>true正确</returns>
        public bool IsTelephone(string str_telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^[1]+\d{10}");
        }

        /// <summary>
        /// 新宝典导入验证
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string CheckXFBColumns(string obj)
        {
            string flag = "true";
            try
            {
                ArrayList arr = JSON.getPara(obj);

                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile(ValueHandler.GetStringValue(arr[0]), HttpContext.Current.Request.PhysicalApplicationPath + "UpFile\\Files\\" + System.IO.Path.GetFileName(ValueHandler.GetStringValue(arr[0]).Substring(ValueHandler.GetStringValue(arr[0]).LastIndexOf('/') + 1)));
                }
                string fileName = ValueHandler.GetStringValue(arr[0]).Substring(ValueHandler.GetStringValue(arr[0]).LastIndexOf('/') + 1);
                string abPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
                using (StreamReader sr = new StreamReader(abPath + fileName, System.Text.Encoding.GetEncoding("gb2312")))
                {
                    string str;
                    int i = 0;
                    while ((str = sr.ReadLine()) != null)
                    {
                        i++;
                        string[] temp = str.Replace("\"", "").Split((char)9);

                        if (str.Contains("\"") || str.Contains(","))
                        {
                            flag = "special|" + i + "|" + str;
                            break;
                        }
                        if (temp.Length < 4)
                        {
                            flag = "contentLen|" + i + "|" + str;
                            break;
                        }
                        if (!IsTelephone(temp[1]) && i != 1)
                        {
                            flag = "false|" + i + "|" + str;
                            break;
                        }
                        if (temp[3].Length > 500 && i != 1)
                        {
                            flag = "remark|" + i + "|" + str;
                            break;
                        }
                    }
                }
            }
            catch
            {
                flag = "false";
            }

            return flag;
        }

        /// <summary>
        /// 批量导入数据 只处理空 - , / 全角
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string PlImportXFBOrignData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_OriginalData.PlImportXFBOrignData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
                        , ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(BLL_User.User_Name));

            if (flag)
                return "true";
            return "false";
        }

        #endregion

        #region 修改

        /// <summary>
        /// 获取单个原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBOriginData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetXFBOriginData(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 修改原始数据地区、计费信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpdateXFBDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.UpdateXFBDatas(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                             ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3])))
                return "true";
            return "false";
        }

        #endregion

        #region 标记为已使用

        /// <summary>
        /// 标记为已使用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MarkXFBAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.MarkXFBAlreadyUse(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }

        #endregion

        #region 无效数据

        public string XFBInvalidData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_OriginalData.XFBInvalidData(ValueHandler.GetStringValue(arr[0]));

            if (flag)
                return "true";
            return "false";
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteXFBDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.DeleteXFBDatas(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }

        #endregion

        #region 获取查看数据

        #region 明细数据

        public string GetXFBOriginalDataDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetXFBOriginalDataDts(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetXFBOriginalDataDtsCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetXFBOriginalDataDtsCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 有效数据

        public string GetXFBValidData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetXFBValidData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetXFBValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetXFBValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 无效数据

        public string GetXFBNOVaildData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetXFBNOVaildData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetXFBNoValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetXFBNoValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #endregion

        #endregion

        #region 实时保

        #region 列表查询

        /// <summary>
        /// 得到原数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBOriginalData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetSSBOriginalData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到原数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBOriginalDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetSSBOriginalDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]));
        }

        #endregion

        #region 导入

        /// <summary>
        /// 批量导入数据 只处理空 - , / 全角
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string PlImportSSBOrignData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_OriginalData.PlImportSSBOrignData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
                        , ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(BLL_User.User_Name));

            if (flag)
                return "true";
            return "false";
        }

        #endregion

        #region 修改

        /// <summary>
        /// 获取单个原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBOriginData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetSSBOriginData(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 修改原始数据地区、计费信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpdateSSBDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.UpdateSSBDatas(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                             ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3])))
                return "true";
            return "false";
        }

        #endregion

        #region 标记为已使用

        /// <summary>
        /// 标记为已使用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MarkSSBAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.MarkSSBAlreadyUse(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteSSBDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.DeleteSSBDatas(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }
        #endregion

        #region 获取查看数据

        #region 明细数据

        public string GetSSBOriginalDataDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetSSBOriginalDataDts(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetSSBOriginalDataDtsCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetSSBOriginalDataDtsCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 有效数据

        public string GetSSBValidData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetSSBValidData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetSSBValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetSSBValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 无效数据

        public string GetSSBNOVaildData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetSSBNOVaildData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetSSBNoValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetSSBNoValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 营销成功

        public string GetSSBAlreadyUseData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetSSBAlreadyUseData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetSSBAlreadyUseDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetSSBAlreadyUseDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #endregion

        #endregion

        #region 诚信通

        #region 列表查询

        /// <summary>
        /// 得到原数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetOriginalData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetOriginalData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到原数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetOriginalDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetOriginalDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]));
        }

        #endregion

        #region 导入

        /// <summary>
        /// 检查是否是6列
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CheckColumns(string obj)
        {
            string flag = "true";
            try
            {
                ArrayList arr = JSON.getPara(obj);
                bool hasCheckQhdm = arr.Count > 2;
                string telNo = string.Empty;

                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile(ValueHandler.GetStringValue(arr[0]), HttpContext.Current.Request.PhysicalApplicationPath + "UpFile\\Files\\" + Path.GetFileName(ValueHandler.GetStringValue(arr[0]).Substring(ValueHandler.GetStringValue(arr[0]).LastIndexOf('/') + 1)));
                }
                if (hasCheckQhdm)
                {
                    DataTable dt = new SqlBase().SearchData("SELECT TelNo FROM dbo.SYS_Area WHERE SA_Code='" + arr[4] + "'");
                    if (dt.Rows.Count > 0)
                        telNo = dt.Rows[0][0].ToString();
                }
                string fileName = ValueHandler.GetStringValue(arr[0]).Substring(ValueHandler.GetStringValue(arr[0]).LastIndexOf('/') + 1);
                string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
                using (StreamReader sr = new StreamReader(abPath + fileName, Encoding.GetEncoding("gb2312")))
                {
                    string str;
                    int i = 0;
                    while ((str = sr.ReadLine()) != null)
                    {
                        i++;
                        string[] temp = str.Replace("\"", "").Split((char)9);

                        if (str.Contains("\"") || str.Contains(","))
                        {
                            flag = "special|" + i + "|" + str;
                            break;
                        }
                        if (temp.Length < 7)
                        {
                            flag = "contentLen|" + i + "|" + str;
                            break;
                        }
                        if (hasCheckQhdm)
                        {
                            if (temp[6] != telNo && i != 1)
                            {
                                flag = "false|" + i + "|" + str;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                flag = "false";
            }

            return flag;
        }

        /// <summary>
        /// 批量导入数据 只处理空 - , / 全角
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string PlImportOrignData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_OriginalData.PlImportOrignData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
                        , ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(BLL_User.User_Name), ValueHandler.GetStringValue(arr[6]));

            if (flag)
                return "true";
            return "false";
        }

        /// <summary>
        /// 批量导入有效数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string BatchImportValidData(object obj)
        {
            var arr = obj as InOriginViewModle;

            return "true";
        }

        #endregion

        #region 修改

        /// <summary>
        /// 获取单个原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetOriginData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetOriginData(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 修改原始数据地区、计费信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpdateDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.UpdateDatas(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                             ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3])))
                return "true";
            return "false";
        }

        #endregion

        #region 无效数据

        public string InvalidData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_OriginalData.InvalidData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]));

            if (flag)
                return "true";
            return "false";
        }

        #endregion

        #region 标记为已使用

        /// <summary>
        /// 标记为已使用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MarkAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.MarkAlreadyUse(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.DeleteDatas(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }
        #endregion

        #region 获取查看数据

        #region 明细数据

        public string GetOriginalDataDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetOriginalDataDts(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetOriginalDataDtsCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetOriginalDataDtsCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 有效数据

        public string GetValidData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetValidData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 无效数据

        public string GetNOVaildData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetNOVaildData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetNoValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetNoValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 营销成功

        public string GetAlreadyUseData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetAlreadyUseData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetAlreadyUseDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetAlreadyUseDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #endregion

        #endregion

        #region 维权通

        #region 列表查询

        /// <summary>
        /// 得到原数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTOriginalData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetWQTOriginalData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到原数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTOriginalDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetWQTOriginalDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]));
        }

        #endregion

        #region 导入

        /// <summary>
        /// 批量导入数据 只处理空 - , / 全角
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string PlImportWQTOrignData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_OriginalData.PlImportWQTOrignData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
                        , ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(BLL_User.User_Name));

            if (flag)
                return "true";
            return "false";
        }

        #endregion

        #region 修改

        /// <summary>
        /// 获取单个原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTOriginData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetWQTOriginData(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 修改原始数据地区、计费信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpdateWQTDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.UpdateWQTDatas(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                             ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3])))
                return "true";
            return "false";
        }

        #endregion

        #region 标记为已使用

        /// <summary>
        /// 标记为已使用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MarkWQTAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.MarkWQTAlreadyUse(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteWQTDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.DeleteWQTDatas(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }

        #endregion

        #region 获取查看数据

        #region 明细数据

        public string GetWQTOriginalDataDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetWQTOriginalDataDts(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetWQTOriginalDataDtsCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetWQTOriginalDataDtsCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 有效数据

        public string GetWQTValidData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetWQTValidData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetWQTValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetWQTValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 无效数据

        public string GetWQTNOVaildData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetWQTNOVaildData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetWQTNoValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetWQTNoValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #endregion

        #endregion

        #region 民企云

        #region 列表查询

        /// <summary>
        /// 得到原数据信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYOriginalData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetMQYOriginalData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 得到原数据数量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYOriginalDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetMQYOriginalDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                                           ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]),
                                                           ValueHandler.GetStringValue(arr[4]));
        }

        #endregion

        #region 导入

        /// <summary>
        /// 批量导入数据 只处理空 - , / 全角
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string PlImportMQYOrignData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_OriginalData.PlImportMQYOrignData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
                        , ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(BLL_User.User_Name));

            if (flag)
                return "true";
            return "false";
        }

        #endregion

        #region 修改

        /// <summary>
        /// 获取单个原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYOriginData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetMQYOriginData(ValueHandler.GetStringValue(arr[0]));
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 修改原始数据地区、计费信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string UpdateMQYDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.UpdateMQYDatas(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]),
                                             ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3])))
                return "true";
            return "false";
        }

        #endregion

        #region 标记为已使用

        /// <summary>
        /// 标记为已使用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string MarkMQYAlreadyUse(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.MarkMQYAlreadyUse(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteMQYDatas(object obj)
        {
            ArrayList arr = JSON.getPara(obj);

            if (dAL_OriginalData.DeleteMQYDatas(ValueHandler.GetStringValue(arr[0])))
                return "true";
            return "false";
        }

        #endregion

        #region 获取查看数据

        #region 明细数据

        public string GetMQYOriginalDataDts(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetMQYOriginalDataDts(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetMQYOriginalDataDtsCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetMQYOriginalDataDtsCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 有效数据

        public string GetMQYValidData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetMQYValidData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetMQYValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetMQYValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #region 无效数据

        public string GetMQYNOVaildData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_OriginalData.GetMQYNOVaildData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        public string GetMQYNoValidDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_OriginalData.GetMQYNoValidDataCount(ValueHandler.GetStringValue(arr[0]));
        }

        #endregion

        #endregion

        #endregion
    }
}
