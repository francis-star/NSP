/////////////////////////////////////////////////////////////////////////////
//模块名：营销管理中心
//开发者：田维华
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using System.Collections;
using HCWeb2016;
using System.Data;
using System.IO;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Text.RegularExpressions;
using Model;

namespace BLL
{
    public class BLL_CallCenter
    {
        DAL_CallCenter dAL_CallCenter = new DAL_CallCenter();

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenter.GetCallCenterData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }
        #endregion

        #region 获取数据量
        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_CallCenter.GetDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            return Data;
        }
        #endregion

        #region 删除营销管理中心数据
        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string ResultStr = dAL_CallCenter.DeleteCallCenterData(ValueHandler.GetStringValue(arr[0]));
            return ResultStr;
        }
        #endregion

        #region 获取当前用户名(使用ligerUI真分页时，因请求目标不同，session无法获取用户信息)
        /// <summary>
        /// 获取当前用户名(使用ligerUI真分页时，因请求目标不同，session无法获取用户信息)
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return BLL_User.User_Name;
        }
        #endregion

        #region 民企云

        #region 获取营销管理中心数据
        /// <summary>
        /// 获取营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenter.GetMQYCallCenterData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }
        #endregion

        #region 获取数据量
        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetMQYDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_CallCenter.GetMQYDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            return Data;
        }
        #endregion

        #region 删除营销管理中心数据
        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteMQYCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string ResultStr = dAL_CallCenter.DeleteMQYCallCenterData(ValueHandler.GetStringValue(arr[0]));
            return ResultStr;
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
        public string GetWQTCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenter.GetWQTCallCenterData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        #endregion

        #region 获取数据量
        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetWQTDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_CallCenter.GetWQTDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            return Data;
        }
        #endregion

        #region 删除营销管理中心数据
        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteWQTCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string ResultStr = dAL_CallCenter.DeleteWQTCallCenterData(ValueHandler.GetStringValue(arr[0]));
            return ResultStr;
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
        public string GetXFBCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenter.GetXFBCallCenterData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }
        #endregion

        #region 获取数据量
        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetXFBDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_CallCenter.GetXFBDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            return Data;
        }
        #endregion

        #region 删除营销管理中心数据
        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteXFBCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string ResultStr = dAL_CallCenter.DeleteXFBCallCenterData(ValueHandler.GetStringValue(arr[0]));
            return ResultStr;
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
        public string GetSSBCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenter.GetSSBCallCenterData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        #endregion

        #region 获取数据量

        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_CallCenter.GetSSBDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), ValueHandler.GetStringValue(arr[6]), ValueHandler.GetStringValue(arr[7]), ValueHandler.GetStringValue(arr[8]), ValueHandler.GetStringValue(arr[9]), ValueHandler.GetStringValue(arr[10]));
            return Data;
        }

        #endregion

        #region 删除营销管理中心数据

        /// <summary>
        /// 删除营销管理中心数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteSSBCallCenterData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string ResultStr = dAL_CallCenter.DeleteSSBCallCenterData(ValueHandler.GetStringValue(arr[0]));
            return ResultStr;
        }

        #endregion

        #region 计费处理

        #region  导入相关

        /// <summary>
        /// 检查是否是6列
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CheckColumns(object obj)
        {
            string flag = "true";
            try
            {
                ArrayList arr = JSON.getPara(obj);
                string fileUrl = ValueHandler.GetStringValue(arr[0]);
                string fileName = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);
                string path = HttpContext.Current.Request.PhysicalApplicationPath + "UpFile\\Files\\";
                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile(fileUrl, path + Path.GetFileName(fileName));
                }
                //物理路径
                string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
                using (FileStream fs = new FileStream(abPath + fileName, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook sfWorkbook;
                    if (fileName.Last() == 's')
                        sfWorkbook = new HSSFWorkbook(fs);
                    else
                        sfWorkbook = new XSSFWorkbook(fs);
                    ISheet sheet = sfWorkbook.GetSheetAt(0);
                    int realCount = sheet.LastRowNum - sheet.FirstRowNum - 1;

                    if (sheet.PhysicalNumberOfRows == 0 || sheet.GetRow(1) == null || realCount < 1)
                    {
                        flag = "数据内容为空，请重新上传！";
                    }
                    else
                    {
                        //if (sheet.GetRow(1).GetCell(0).ToString() != "客户名称" ||
                        //       sheet.GetRow(1).GetCell(1).ToString() != "计费号码" ||
                        //       sheet.GetRow(1).GetCell(2).ToString() != "计费生效时间")
                        //{
                        //    flag = "模版错误，无法解析，请检查后重新导入！";
                        //} 
                        if (realCount > 10000)
                        {
                            flag = "导入内容过多，请分批次导入！";
                        }
                        else //处理错误1、格式错误；2、计费号码重复；3、计费号码非已审的未计费客户单
                             //格式错误：计费号码及计费生效时间格式存在问题或内容为空。
                        {
                            string errList = string.Empty;
                            string date = string.Empty;
                            List<BatchChargeData> list = new List<BatchChargeData>();
                            for (int i = 2; i < sheet.PhysicalNumberOfRows; i++)
                            {
                                if (sheet.GetRow(i).GetCell(2).CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(sheet.GetRow(i).GetCell(2)))
                                    date = sheet.GetRow(i).GetCell(2).DateCellValue.ToString("yyyy/MM/dd");
                                else
                                {
                                    date = sheet.GetRow(i).GetCell(2).ToString();
                                }
                                if (sheet.GetRow(i) != null)
                                {
                                    list.Add(new BatchChargeData()
                                    {
                                        billNo = sheet.GetRow(i).GetCell(1).ToString(),
                                        custName = sheet.GetRow(i).GetCell(0).ToString(),
                                        activeDate = date
                                    });
                                }
                            }

                            for (int i = 0; i < list.Count; i++)
                            {
                                var data = list[i];
                                if (FormarError(data.billNo))
                                {
                                    errList += $"行{i + 3} (计费号码格式错误)";
                                    break;
                                }
                                if (isRepeatBillNo(data.billNo, list))
                                {
                                    errList += $"行{i + 3} (计费号码重复)";
                                    break;
                                }
                                if (FormarDateError(data.activeDate))
                                {
                                    errList += $"行{i + 3} (计费生效时间格式错误)";
                                    break;
                                }
                                if (DateLimit(data.activeDate))
                                {
                                    errList += $"行{i + 3} (计费生效时间应小于等于当前时间)";
                                    break;
                                }
                                //非已审
                                if (dAL_CallCenter.isStateCustomer(data.billNo))
                                {
                                    errList += $"行{i + 3} (计费号码非已审的未计费客户单)";
                                    break;
                                }
                                //当天已计费
                                if (dAL_CallCenter.isBillStateCustomer(data.billNo))
                                {
                                    errList += $"行{i + 3} (计费号码非已审的未计费客户单)";
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(errList))
                                flag = "上传数据有误：" + errList;
                            else
                            {
                                flag = dAL_CallCenter.PlImportChargeData(ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
                                    , BLL_User.User_Code, list);
                            }
                        }
                    }
                }
            }
            catch
            {
                flag = "模版错误，无法解析，请检查后重新导入！";
            }

            return flag;
        }

        #region 验证方法

        public bool FormarError(string billNo)
        {
            if (string.IsNullOrEmpty(billNo))
                return true;
            else if (!Regex.IsMatch(billNo, @"^\d{11,12}$"))
                return true;
            else
                return false;
        }

        public bool FormarDateError(string date)
        {
            DateTime dtTime;
            if (string.IsNullOrEmpty(date))
                return true;
            else if (!DateTime.TryParse(date, out dtTime))
                return true;
            else
                return false;
        }

        public bool DateLimit(string date)
        {
            DateTime dtTime;
            DateTime.TryParse(date, out dtTime);
            if (dtTime > DateTime.Now)
                return true;
            else
                return false;
        }

        public bool isRepeatBillNo(string billNo, List<BatchChargeData> data)
        {
            var query = (from q in data where q.billNo == billNo select q).Count();
            return query > 1;
        }

        #endregion

        //public string PlImportChargeData(object obj)
        //{
        //    ArrayList arr = JSON.getPara(obj);
        //    bool flag = dAL_CallCenter.PlImportChargeData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
        //                ,BLL_User.User_Code); 

        //    if (flag)
        //        return "true";
        //    return "false";
        //}

        #endregion

        /// <summary>
        /// 根据条件获取历史计费列表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBDealChargeData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenter.GetSSBDealChargeData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
                , ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 获取历史计费总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSBDealChargeDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_CallCenter.GetSSBDealChargeDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2])
                , ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]));
            return Data;
        }

        /// <summary>
        /// 撤销批次
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string CancelSSBChargeData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CallCenter.CancelSSBChargeData(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Code);
        }

        /// <summary>
        /// 删除批次
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DeleteSSBChargeData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_CallCenter.DeleteSSBChargeData(ValueHandler.GetStringValue(arr[0]));
            return Data;
        }


        /// <summary>
        /// 查看批次明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSB_ViewChargeData(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CallCenter.GetSSB_ViewChargeData(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            string json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 获取批次明细总数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSSB_ViewChargeDataCount(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            string Data = dAL_CallCenter.GetSSB_ViewChargeDataCount(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]));
            return Data;
        }

        #endregion 

        #endregion
    }
}
