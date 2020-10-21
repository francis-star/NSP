/////////////////////////////////////////////////////////////////////////////
//模块名：导入导出数据
//开发者：赵虎
//开发时间：2016年11月25日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAL.InMolde;
using HCWeb2016;

namespace BLL
{
    public class BLL_OrigDataImport
    {
        public DataSet ImportData(string strname)
        {
            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile(strname, HttpContext.Current.Request.PhysicalApplicationPath + "NewUpFile\\Files\\" + System.IO.Path.GetFileName(strname.Substring(strname.LastIndexOf('/') + 1)));
            }

            DataSet ds = ExcelToDataSet(HttpContext.Current.Request.PhysicalApplicationPath + "NewUpFile\\Files\\" + strname.Substring(strname.LastIndexOf('/') + 1) + "");
            return ds;
        }

        /// <summary>
        /// excel数据导入DataSet
        /// </summary>
        /// <param name="strname">文件路径</param>
        /// <returns></returns>
        public DataSet ImportDataOLD(string strname)
        {
            DataSet ds = ExcelToDataSet(HttpContext.Current.Request.PhysicalApplicationPath + "UpFile\\Files\\" + strname + "");
            return ds;
        }

        /// <summary>
        /// excel导入Dataset
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static public DataSet ExcelToDataSet(string filename)
        {
            DataSet ds;
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                            "Extended Properties=Excel 8.0;" +
                            "data source=" + filename;
            OleDbConnection myConn = new OleDbConnection(strCon);

            try
            {
                myConn.Open();

                //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等
                DataTable dtSheetName = myConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

                //包含excel中表名的字符串数组
                string[] strTableNames = new string[dtSheetName.Rows.Count];

                for (int k = 0; k < dtSheetName.Rows.Count; k++)
                {
                    strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
                }

                string strCom = " SELECT * FROM [" + strTableNames[0].ToString() + "]";

                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                ds = new DataSet();

                myCommand.Fill(ds);
            }
            catch (Exception)
            {
                myConn.Close();
                throw;
            }
            myConn.Close();
            return ds;
        }

        /// <summary>
        /// 导出有效数据
        /// </summary>
        public string DtToExcel(object obj)
        {
            try
            {
                var model = obj as InOriginViewModle;
                string[] arr = (obj as string).Split(',');
                string[] arrName = arr[0].Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                string yearStr;
                switch (model.TableName.Trim())
                {
                    case "CXT_OriginalDataDts":
                        sb.Append("SELECT ODD_OD_Code[客户编码],ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[联系人],ODD_LinkPhone[联系电话],ODD_IsBill[是否计费]," +
                            "DuplicateData[重复数据],ApprovedUser[已审用户],UnsubscribedUser[退订用户],RefundUser[退费用户],BlackList[黑名单],ODD_LinkPhone[联系号码] " +
                            "TSNumber[投诉号码],Keywords_high[关键字(高)],Keywords_low[关键字(低)],RepetitionType[重复类型],ODD_Business[业务名称],ODD_IsBill[是否计费],TSNature[投诉性质],TSSource[投诉来源],OpenDate[开通时间],UnsubscribeTime[退订时间] FROM dbo.CXT_OriginalDataDts WHERE 1=1");
                        if (!string.IsNullOrEmpty(model.ODD_OD_Code))
                        {
                            sb.Append(" AND ODD_OD_Code ='" + model.ODD_OD_Code + "'");
                        }
                        if (model.IsExportByCondition == 0)
                        {
                            if (!string.IsNullOrEmpty(model.DuplicateData))
                            {
                                sb.Append(" AND DuplicateData ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.Keywords_high))
                            {
                                sb.Append(" AND Keywords_high ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.Keywords_low))
                            {
                                sb.Append(" AND Keywords_low ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.LinkPhone))
                            {
                                sb.Append(" AND ODD_LinkPhone  != ''");
                            }
                            if (!string.IsNullOrEmpty(model.OpenDate))
                            {
                                sb.Append(" AND  CONVERT(varchar(100), [OpenDate], 23) = '" + model.OpenDate + "'");

                            }
                            if (!string.IsNullOrEmpty(model.UnsubscribeTime))
                            {
                                sb.Append(" AND  CONVERT(varchar(100), [UnsubscribeTime], 23) = '" + model.UnsubscribeTime + "'");

                            }
                            if (!string.IsNullOrEmpty(model.ApprovedUser))
                            {
                                sb.Append(" AND ApprovedUser  = '1'");
                            }
                            if (!string.IsNullOrEmpty(model.BlackList))
                            {
                                sb.Append(" AND BlackList  = '1'");
                            }

                            if (!string.IsNullOrEmpty(model.BusinessName))
                            {
                                sb.Append(" AND ODD_Business  = '" + model.BusinessName + "'");
                            }

                            if (!string.IsNullOrEmpty(model.IsCharge))
                            {
                                sb.Append(" AND ODD_IsBill  = '" + model.IsCharge + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSNature))
                            {
                                sb.Append(" AND TSNature  = '" + model.TSNature + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSNumber))
                            {
                                sb.Append(" AND TSNumber  = '" + model.TSNumber + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSSource))
                            {
                                sb.Append(" AND TSSource  = '" + model.TSSource + "'");
                            }
                            if (!string.IsNullOrEmpty(model.UnsubscribedUser))
                            {
                                sb.Append(" AND UnsubscribedUser  = '" + model.UnsubscribedUser + "'");
                            }
                            if (!string.IsNullOrEmpty(model.RefundUser))
                            {
                                sb.Append(" AND RefundUser  = '1'");
                            }

                            if (!string.IsNullOrEmpty(model.RepetitionType))
                            {
                                sb.Append(" AND RepetitionType  = '" + model.RepetitionType + "'");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.ODCodeList))
                            {
                                var odList = model.ODCodeList.Split(',');
                                var ods = "   and (";
                                for (int i = 0; i < odList.Length; i++)
                                {
                                    if (i == ods.Length - 1)
                                    {
                                        ods += " Contains(ODD_Code,'" + odList[i] + "'))";
                                        // strkeywords += " Contains(title,'" + Rekeywords[i].keyword + "'))";
                                        break;
                                    }
                                    ods += " Contains(ODD_Code,'" + odList[i] + "') OR  ";
                                }

                                sb.Append("" + ods + "");
                            }
                        }

                        break;
                    case "CXT_OriginalDataValid":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[姓名],ODD_LinkPhone[联系电话] FROM dbo.CXT_OriginalDataValid WHERE 1=1");
                        if (!string.IsNullOrEmpty(model.ODD_OD_Code))
                        {
                            sb.Append(" AND ODD_OD_Code ='" + model.ODD_OD_Code + "'");
                        }
                        if (model.IsExportByCondition == 1)
                        {
                            if (!string.IsNullOrEmpty(model.ODCodeList))
                            {
                                var odList = model.ODCodeList.Split(',');
                                var ods = "   and (";
                                for (int i = 0; i < odList.Length; i++)
                                {
                                    if (i == ods.Length - 1)
                                    {
                                        ods += " Contains(ODD_Code,'" + odList[i] + "'))";
                                        // strkeywords += " Contains(title,'" + Rekeywords[i].keyword + "'))";
                                        break;
                                    }
                                    ods += " Contains(ODD_Code,'" + odList[i] + "') OR  ";
                                }

                                sb.Append("" + ods + "");
                            }
                        }
                        break;
                    case "MQY_OriginalDataDts":
                        sb.Append("SELECT ODD_OD_Code[客户编码],ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[联系人],ODD_LinkPhone[联系电话],ODD_IsBill[是否计费]," +
                           "DuplicateData[重复数据],ApprovedUser[已审用户],UnsubscribedUser[退订用户],RefundUser[退费用户],BlackList[黑名单],ODD_LinkPhone[联系号码] " +
                           "TSNumber[投诉号码],Keywords_high[关键字(高)],Keywords_low[关键字(低)],RepetitionType[重复类型],ODD_Business[业务名称],ODD_IsBill[是否计费],TSNature[投诉性质],TSSource[投诉来源],OpenDate[开通时间],UnsubscribeTime[退订时间] FROM dbo.MQY_OriginalDataDts WHERE 1=1");
                        if (!string.IsNullOrEmpty(model.ODD_OD_Code))
                        {
                            sb.Append(" AND ODD_OD_Code ='" + model.ODD_OD_Code + "'");
                        }
                        if (model.IsExportByCondition == 0)
                        {
                            if (!string.IsNullOrEmpty(model.DuplicateData))
                            {
                                sb.Append(" AND DuplicateData ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.Keywords_high))
                            {
                                sb.Append(" AND Keywords_high ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.Keywords_low))
                            {
                                sb.Append(" AND Keywords_low ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.LinkPhone))
                            {
                                sb.Append(" AND ODD_LinkPhone  != ''");
                            }
                            if (!string.IsNullOrEmpty(model.OpenDate))
                            {
                                sb.Append(" AND  CONVERT(varchar(100), [OpenDate], 23) = '" + model.OpenDate + "'");

                            }
                            if (!string.IsNullOrEmpty(model.UnsubscribeTime))
                            {
                                sb.Append(" AND  CONVERT(varchar(100), [UnsubscribeTime], 23) = '" + model.UnsubscribeTime + "'");

                            }
                            if (!string.IsNullOrEmpty(model.ApprovedUser))
                            {
                                sb.Append(" AND ApprovedUser  = '1'");
                            }
                            if (!string.IsNullOrEmpty(model.BlackList))
                            {
                                sb.Append(" AND BlackList  = '1'");
                            }

                            if (!string.IsNullOrEmpty(model.BusinessName))
                            {
                                sb.Append(" AND ODD_Business  = '" + model.BusinessName + "'");
                            }

                            if (!string.IsNullOrEmpty(model.IsCharge))
                            {
                                sb.Append(" AND ODD_IsBill  = '" + model.IsCharge + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSNature))
                            {
                                sb.Append(" AND TSNature  = '" + model.TSNature + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSNumber))
                            {
                                sb.Append(" AND TSNumber  = '" + model.TSNumber + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSSource))
                            {
                                sb.Append(" AND TSSource  = '" + model.TSSource + "'");
                            }
                            if (!string.IsNullOrEmpty(model.UnsubscribedUser))
                            {
                                sb.Append(" AND UnsubscribedUser  = '" + model.UnsubscribedUser + "'");
                            }
                            if (!string.IsNullOrEmpty(model.RefundUser))
                            {
                                sb.Append(" AND RefundUser  = '1'");
                            }

                            if (!string.IsNullOrEmpty(model.RepetitionType))
                            {
                                sb.Append(" AND RepetitionType  = '" + model.RepetitionType + "'");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.ODCodeList))
                            {
                                var odList = model.ODCodeList.Split(',');
                                var ods = "   and (";
                                for (int i = 0; i < odList.Length; i++)
                                {
                                    if (i == ods.Length - 1)
                                    {
                                        ods += " Contains(ODD_Code,'" + odList[i] + "'))";
                                        // strkeywords += " Contains(title,'" + Rekeywords[i].keyword + "'))";
                                        break;
                                    }
                                    ods += " Contains(ODD_Code,'" + odList[i] + "') OR  ";
                                }

                                sb.Append("" + ods + "");
                            }
                        }

                        break;
                    case "MQY_OriginalDataValid":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[姓名],ODD_LinkPhone[联系电话] FROM dbo.MQY_OriginalDataValid WHERE 1=1");
                        if (!string.IsNullOrEmpty(model.ODD_OD_Code))
                        {
                            sb.Append(" AND ODD_OD_Code ='" + model.ODD_OD_Code + "'");
                        }
                        if (model.IsExportByCondition == 1)
                        {
                            if (!string.IsNullOrEmpty(model.ODCodeList))
                            {
                                var odList = model.ODCodeList.Split(',');
                                var ods = "   and (";
                                for (int i = 0; i < odList.Length; i++)
                                {
                                    if (i == ods.Length - 1)
                                    {
                                        ods += " Contains(ODD_Code,'" + odList[i] + "'))";
                                        // strkeywords += " Contains(title,'" + Rekeywords[i].keyword + "'))";
                                        break;
                                    }
                                    ods += " Contains(ODD_Code,'" + odList[i] + "') OR  ";
                                }

                                sb.Append("" + ods + "");
                            }
                        }

                        break;
                    case "WQT_OriginalDataDts":
                        sb.Append("SELECT ODD_OD_Code[客户编码],ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[联系人],ODD_LinkPhone[联系电话],ODD_IsBill[是否计费]," +
                           "DuplicateData[重复数据],ApprovedUser[已审用户],UnsubscribedUser[退订用户],RefundUser[退费用户],BlackList[黑名单],ODD_LinkPhone[联系号码] " +
                           "TSNumber[投诉号码],Keywords_high[关键字(高)],Keywords_low[关键字(低)],RepetitionType[重复类型],ODD_Business[业务名称],ODD_IsBill[是否计费],TSNature[投诉性质],TSSource[投诉来源],OpenDate[开通时间],UnsubscribeTime[退订时间] FROM dbo.WQT_OriginalDataDts WHERE 1=1");
                        if (!string.IsNullOrEmpty(model.ODD_OD_Code))
                        {
                            sb.Append(" AND ODD_OD_Code ='" + model.ODD_OD_Code + "'");
                        }
                        if (model.IsExportByCondition == 0)
                        {
                            if (!string.IsNullOrEmpty(model.DuplicateData))
                            {
                                sb.Append(" AND DuplicateData ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.Keywords_high))
                            {
                                sb.Append(" AND Keywords_high ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.Keywords_low))
                            {
                                sb.Append(" AND Keywords_low ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.LinkPhone))
                            {
                                sb.Append(" AND ODD_LinkPhone  != ''");
                            }
                            if (!string.IsNullOrEmpty(model.OpenDate))
                            {
                                sb.Append(" AND  CONVERT(varchar(100), [OpenDate], 23) = '" + model.OpenDate + "'");

                            }
                            if (!string.IsNullOrEmpty(model.UnsubscribeTime))
                            {
                                sb.Append(" AND  CONVERT(varchar(100), [UnsubscribeTime], 23) = '" + model.UnsubscribeTime + "'");

                            }
                            if (!string.IsNullOrEmpty(model.ApprovedUser))
                            {
                                sb.Append(" AND ApprovedUser  = '1'");
                            }
                            if (!string.IsNullOrEmpty(model.BlackList))
                            {
                                sb.Append(" AND BlackList  = '1'");
                            }

                            if (!string.IsNullOrEmpty(model.BusinessName))
                            {
                                sb.Append(" AND ODD_Business  = '" + model.BusinessName + "'");
                            }

                            if (!string.IsNullOrEmpty(model.IsCharge))
                            {
                                sb.Append(" AND ODD_IsBill  = '" + model.IsCharge + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSNature))
                            {
                                sb.Append(" AND TSNature  = '" + model.TSNature + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSNumber))
                            {
                                sb.Append(" AND TSNumber  = '" + model.TSNumber + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSSource))
                            {
                                sb.Append(" AND TSSource  = '" + model.TSSource + "'");
                            }
                            if (!string.IsNullOrEmpty(model.UnsubscribedUser))
                            {
                                sb.Append(" AND UnsubscribedUser  = '" + model.UnsubscribedUser + "'");
                            }
                            if (!string.IsNullOrEmpty(model.RefundUser))
                            {
                                sb.Append(" AND RefundUser  = '1'");
                            }

                            if (!string.IsNullOrEmpty(model.RepetitionType))
                            {
                                sb.Append(" AND RepetitionType  = '" + model.RepetitionType + "'");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.ODCodeList))
                            {
                                var odList = model.ODCodeList.Split(',');
                                var ods = "   and (";
                                for (int i = 0; i < odList.Length; i++)
                                {
                                    if (i == ods.Length - 1)
                                    {
                                        ods += " Contains(ODD_Code,'" + odList[i] + "'))";
                                        // strkeywords += " Contains(title,'" + Rekeywords[i].keyword + "'))";
                                        break;
                                    }
                                    ods += " Contains(ODD_Code,'" + odList[i] + "') OR  ";
                                }

                                sb.Append("" + ods + "");
                            }
                        }

                        break;
                    case "WQT_OriginalDataValid":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.WQT_OriginalDataDts WHERE 1=1 AND ODD_Type=2");
                        if (!string.IsNullOrEmpty(model.ODD_OD_Code))
                        {
                            sb.Append(" AND ODD_OD_Code ='" + model.ODD_OD_Code + "'");
                        }
                        if (model.IsExportByCondition == 1)
                        {
                            if (!string.IsNullOrEmpty(model.ODCodeList))
                            {
                                var odList = model.ODCodeList.Split(',');
                                var ods = "   and (";
                                for (int i = 0; i < odList.Length; i++)
                                {
                                    if (i == ods.Length - 1)
                                    {
                                        ods += " Contains(ODD_Code,'" + odList[i] + "'))";
                                        // strkeywords += " Contains(title,'" + Rekeywords[i].keyword + "'))";
                                        break;
                                    }
                                    ods += " Contains(ODD_Code,'" + odList[i] + "') OR  ";
                                }

                                sb.Append("" + ods + "");
                            }
                        }

                        break;
                    //消费宝导出
                    case "XFB_OriginalDataDts":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[联系电话] FROM dbo.XFB_OriginalDataDts WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code ='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "XFB_ValidData":
                        sb.Append(@"SELECT ODD_Name[客户名称],ODD_Phone[联系电话],ODD_Remark[备注] FROM dbo.XFB_OriginalDataValid WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                        //实时保导出
                    case "SSB_OriginalDataDts":
                        sb.Append("SELECT ODD_OD_Code[客户编码],ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[联系人],ODD_LinkPhone[联系电话],ODD_IsBill[是否计费]," +
                       "DuplicateData[重复数据],ApprovedUser[已审用户],UnsubscribedUser[退订用户],RefundUser[退费用户],BlackList[黑名单],ODD_LinkPhone[联系号码] " +
                       "TSNumber[投诉号码],Keywords_high[关键字(高)],Keywords_low[关键字(低)],RepetitionType[重复类型],ODD_Business[业务名称],ODD_IsBill[是否计费],TSNature[投诉性质],TSSource[投诉来源],OpenDate[开通时间],UnsubscribeTime[退订时间] FROM dbo.SSB_OriginalDataDts WHERE 1=1");
                        if (!string.IsNullOrEmpty(model.ODD_OD_Code))
                        {
                            sb.Append(" AND ODD_OD_Code ='" + model.ODD_OD_Code + "'");
                        }
                        if (model.IsExportByCondition == 0)
                        {
                            if (!string.IsNullOrEmpty(model.DuplicateData))
                            {
                                sb.Append(" AND DuplicateData ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.Keywords_high))
                            {
                                sb.Append(" AND Keywords_high ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.Keywords_low))
                            {
                                sb.Append(" AND Keywords_low ='1'");
                            }
                            if (!string.IsNullOrEmpty(model.LinkPhone))
                            {
                                sb.Append(" AND ODD_LinkPhone  != ''");
                            }
                            if (!string.IsNullOrEmpty(model.OpenDate))
                            {
                                sb.Append(" AND  CONVERT(varchar(100), [OpenDate], 23) = '"+ model.OpenDate + "'");
                               
                            }
                            if (!string.IsNullOrEmpty(model.UnsubscribeTime))
                            {
                                sb.Append(" AND  CONVERT(varchar(100), [UnsubscribeTime], 23) = '" + model.UnsubscribeTime + "'");

                            }
                            if (!string.IsNullOrEmpty(model.ApprovedUser))
                            {
                                sb.Append(" AND ApprovedUser  = '1'");
                            }
                            if (!string.IsNullOrEmpty(model.BlackList))
                            {
                                sb.Append(" AND BlackList  = '1'");
                            }

                            if (!string.IsNullOrEmpty(model.BusinessName))
                            {
                                sb.Append(" AND ODD_Business  = '" + model.BusinessName + "'");
                            }

                            if (!string.IsNullOrEmpty(model.IsCharge))
                            {
                                sb.Append(" AND ODD_IsBill  = '" + model.IsCharge + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSNature))
                            {
                                sb.Append(" AND TSNature  = '" + model.TSNature + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSNumber))
                            {
                                sb.Append(" AND TSNumber  = '" + model.TSNumber + "'");
                            }
                            if (!string.IsNullOrEmpty(model.TSSource))
                            {
                                sb.Append(" AND TSSource  = '" + model.TSSource + "'");
                            }
                            if (!string.IsNullOrEmpty(model.UnsubscribedUser))
                            {
                                sb.Append(" AND UnsubscribedUser  = '" + model.UnsubscribedUser + "'");
                            }
                            if (!string.IsNullOrEmpty(model.RefundUser))
                            {
                                sb.Append(" AND RefundUser  = '1'");
                            }

                            if (!string.IsNullOrEmpty(model.RepetitionType))
                            {
                                sb.Append(" AND RepetitionType  = '" + model.RepetitionType + "'");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.ODCodeList))
                            {
                                var odList = model.ODCodeList.Split(',');
                                 var ods = "   and (";
                                for (int i = 0; i < odList.Length; i++)
                                {
                                    if (i == ods.Length - 1)
                                    {
                                        ods += " Contains(ODD_Code,'" + odList[i] + "'))";
                                        // strkeywords += " Contains(title,'" + Rekeywords[i].keyword + "'))";
                                        break;
                                    }
                                    ods += " Contains(ODD_Code,'" + odList[i] + "') OR  ";
                                }

                                sb.Append("" + ods + "");
                            }
                        }
                       
                        break;
                    case "SSB_OriginalDataValid":
                        sb.Append(@"SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）],ODD_Address[地址],ODD_LinkMan[姓名]
                                    ,ODD_LinkPhone[联系电话] FROM dbo.SSB_OriginalDataValid WHERE 1=1");
                        if (!string.IsNullOrEmpty(model.ODD_OD_Code))
                        {
                            sb.Append(" AND ODD_OD_Code ='" + model.ODD_OD_Code + "'");
                        }
                        if (model.IsExportByCondition == 1)
                        {
                            if (!string.IsNullOrEmpty(model.ODCodeList))
                            {
                                var odList = model.ODCodeList.Split(',');
                                var ods = "   and (";
                                for (int i = 0; i < odList.Length; i++)
                                {
                                    if (i == ods.Length - 1)
                                    {
                                        ods += " Contains(ODD_Code,'" + odList[i] + "'))";
                                        // strkeywords += " Contains(title,'" + Rekeywords[i].keyword + "'))";
                                        break;
                                    }
                                    ods += " Contains(ODD_Code,'" + odList[i] + "') OR  ";
                                }

                                sb.Append("" + ods + "");
                            }
                        }

                        break;
                   
                }

                DAL.DAL_OriginalData dAL_OriginalData = new DAL.DAL_OriginalData();
                if (!dAL_OriginalData.CheckExportData(sb.ToString()))
                    return "false";

                HttpContext.Current.Session.Remove("FileData");
                HttpContext.Current.Session.Add("FileData", sb.ToString());
                HttpContext.Current.Session.Remove("FileName");

                switch (model.TableName.Trim())
                {
                    case "CXT_OriginalDataDts":
                        HttpContext.Current.Session.Add("FileName", "诚信通-原始数据明细表");
                        break;
                    case "CXT_OriginalDataValid":
                        HttpContext.Current.Session.Add("FileName", "诚信通-有效数据表");
                        break;
                    case "MQY_OriginalDataDts":
                        HttpContext.Current.Session.Add("FileName", "民企云-原始数据明细表");
                        break;
                    case "MQY_OriginalDataValid":
                        HttpContext.Current.Session.Add("FileName", "民企云-有效数据表");
                        break;
                    case "WQT_OriginalDataDts":
                        HttpContext.Current.Session.Add("FileName", "维权通-原始数据明细表");
                        break;
                    case "WQT_OriginalDataValid":
                        HttpContext.Current.Session.Add("FileName", "维权通-有效数据表");
                        break;
                    case "XFB_OriginalDataDts":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-原始数据明细表");
                        break;
                    case "XFB_ValidData":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-有效数据表");
                        break;
                    case "XFB_NOValidData":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-无效数据表");
                        break;
                    case "XFB_AlreadyUse":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-已是会员表");
                        break;
                    case "XFB_UnsubscribeUser":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-退订会员表");
                        break;
                    case "XFB_AlreadyCallData":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-已呼过数据表");
                        break;
                    case "XFB_LoopData":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-重复数据表");
                        break;
                    case "XFB_BlackUser":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-黑名单表");
                        break;
                    case "XFB_KeyWordData":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-关键字数据表");
                        break;
                    case "XFB_HighKeyWordData":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-关键字(高危)数据表");
                        break;
                    case "XFB_OtherData":
                        HttpContext.Current.Session.Add("FileName", "新消费宝典-其他业务会员数据表");
                        break;
                    case "SSB_OriginalDataDts":
                        HttpContext.Current.Session.Add("FileName", "实时保-原始数据明细表");
                        break;
                    case "SSB_OriginalDataValid":
                        HttpContext.Current.Session.Add("FileName", "实时保-有效数据表");
                        break;
                    case "SSB_ViewChargeData":
                        HttpContext.Current.Session.Add("SSBType", "1");
                        HttpContext.Current.Session.Add("FileName", dAL_OriginalData.GetFileName(arr[0].ToString()).Replace(".xlsx", ""));
                        break;
                }

                return "true";
            }
            catch(Exception ex)
            {
                return "false";
            }
        }
    }
}