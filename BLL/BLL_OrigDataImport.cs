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
                string[] arr = (obj as string).Split(',');
                string[] arrName = arr[0].Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                string yearStr;
                switch (ValueHandler.GetStringValue(arr[1]))
                {
                    case "CXT_OriginalDataDts":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.CXT_OriginalDataDts WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code ='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_ValidData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[姓名],ODD_LinkPhone[联系电话] FROM dbo.CXT_OriginalDataValid WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_NOValidData":
                        sb.AppendFormat(@"SELECT ODD_Name[客户名称]
                                    , ODD_Phone[座机号码]
                                FROM CXT_OriginalDataDts
                                WHERE ODD_OD_Code = '{0}' 
	                                AND DataState= 0
	                                AND ODD_Type >1
                                ORDER BY ODD_Name", arr[0]);
                        break;
                    case "CXT_AlreadyUse":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.CXT_OriginalDataDts WHERE 1=1 AND ODD_Type=2");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_UnsubscribeUser":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.CXT_OriginalDataDts WHERE 1=1 AND ODD_Type=3");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_AlreadyCallData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address [地址] FROM dbo.CXT_OriginalDataDts WHERE 1=1 AND ODD_Type=4");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_LoopData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.CXT_OriginalDataDts WHERE 1=1 AND ODD_Type=5");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_BlackUser":
                        sb.Append("SELECT a.ODD_Name[客户名称],a.ODD_Phone[座机号码] ,b.BL_Comment[提供来源] FROM CXT_OriginalDataDts a LEFT JOIN dbo.YX_BlackList b ON a.ODD_Phone=b.BL_Phone WHERE ODD_Type=6");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_KeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address [地址] FROM dbo.CXT_OriginalDataDts WHERE 1=1 AND ODD_Type=7");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_HighKeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.CXT_OriginalDataDts WHERE 1=1 AND ODD_Type=9");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "CXT_OtherData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Business[会员业务] FROM dbo.CXT_OriginalDataDts WHERE 1=1 AND ODD_Type=8");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_OriginalDataDts":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.MQY_OriginalDataDts WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code ='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_ValidData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[姓名],ODD_LinkPhone[联系电话] FROM dbo.MQY_OriginalDataValid WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_NOValidData":
                        sb.AppendFormat(@"SELECT ODD_Name[客户名称]
                                    , ODD_Phone[座机号码]
                                FROM MQY_OriginalDataDts
                                WHERE ODD_OD_Code = '{0}' 
	                                AND DataState= 0
	                                AND ODD_Type >1
                                ORDER BY ODD_Name", arr[0]);
                        break;
                    case "MQY_AlreadyUse":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.MQY_OriginalDataDts WHERE 1=1 AND ODD_Type=2");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_UnsubscribeUser":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.MQY_OriginalDataDts WHERE 1=1 AND ODD_Type=3");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_AlreadyCallData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address [地址] FROM dbo.MQY_OriginalDataDts WHERE 1=1 AND ODD_Type=4");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_LoopData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.MQY_OriginalDataDts WHERE 1=1 AND ODD_Type=5");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_BlackUser":
                        sb.Append("SELECT a.ODD_Name[客户名称],a.ODD_Phone[座机号码] ,b.BL_Comment[提供来源] FROM MQY_OriginalDataDts a LEFT JOIN dbo.YX_BlackList b ON a.ODD_Phone=b.BL_Phone WHERE ODD_Type=6");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_KeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address [地址] FROM dbo.MQY_OriginalDataDts WHERE 1=1 AND ODD_Type=7");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_HighKeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.MQY_OriginalDataDts WHERE 1=1 AND ODD_Type=9");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "MQY_OtherData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Business[会员业务] FROM dbo.MQY_OriginalDataDts WHERE 1=1 AND ODD_Type=8");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_OriginalDataDts":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.WQT_OriginalDataDts WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code ='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_ValidData":
                        sb.Append(@"SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address[地址],ODD_LinkMan[姓名]
                                    ,ODD_LinkPhone[联系电话] FROM dbo.WQT_OriginalDataValid WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_NOValidData":
                        sb.AppendFormat(@"SELECT ODD_Name[客户名称]
                                                , ODD_Phone[座机号码]
                                            FROM WQT_OriginalDataDts
                                            WHERE ODD_OD_Code = '{0}' 
	                                            AND DataState= 0
	                                            AND ODD_Type >1
                                            ORDER BY ODD_Name", arr[0]);
                        break;
                    case "WQT_AlreadyUse":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.WQT_OriginalDataDts WHERE 1=1 AND ODD_Type=2");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_UnsubscribeUser":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.WQT_OriginalDataDts WHERE 1=1 AND ODD_Type=3");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_AlreadyCallData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address [地址] FROM dbo.WQT_OriginalDataDts WHERE 1=1 AND ODD_Type=4");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_LoopData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.WQT_OriginalDataDts WHERE 1=1 AND ODD_Type=5");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_BlackUser":
                        sb.Append("SELECT a.ODD_Name[客户名称],a.ODD_Phone[座机号码] ,b.BL_Comment[提供来源] FROM WQT_OriginalDataDts a LEFT JOIN dbo.YX_BlackList b ON a.ODD_Phone=b.BL_Phone WHERE ODD_Type=6");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_KeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Address [地址] FROM dbo.WQT_OriginalDataDts WHERE 1=1 AND ODD_Type=7");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_HighKeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码] FROM dbo.WQT_OriginalDataDts WHERE 1=1 AND ODD_Type=9");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "WQT_OtherData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[座机号码],ODD_Business[会员业务] FROM dbo.WQT_OriginalDataDts WHERE 1=1 AND ODD_Type=8");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
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
                    case "XFB_NOValidData":
                        sb.AppendFormat(@"SELECT ODD_Name[客户名称]
                                                , ODD_Phone[联系电话]
                                            FROM XFB_OriginalDataDts
                                            WHERE ODD_OD_Code = '{0}' 
	                                            AND DataState= 0
	                                            AND ODD_Type >1
                                            ORDER BY ODD_Name", arr[0]);
                        break;
                    case "XFB_AlreadyUse":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[联系电话] FROM dbo.XFB_OriginalDataDts WHERE 1=1 AND ODD_Type=2");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "XFB_UnsubscribeUser":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[联系电话],ODD_Business[会员业务] FROM dbo.XFB_OriginalDataDts WHERE 1=1 AND ODD_Type=3");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "XFB_AlreadyCallData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[联系电话],ODD_Address [地址] FROM dbo.XFB_OriginalDataDts WHERE 1=1 AND ODD_Type=4");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "XFB_LoopData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[联系电话] FROM dbo.XFB_OriginalDataDts WHERE 1=1 AND ODD_Type=5");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "XFB_BlackUser":
                        sb.Append("SELECT a.ODD_Name[客户名称],a.ODD_Phone[联系电话] ,b.BL_Comment[提供来源] FROM XFB_OriginalDataDts a LEFT JOIN dbo.YX_BlackList b ON a.ODD_Phone=b.BL_Phone WHERE ODD_Type=6");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "XFB_KeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[联系电话],ODD_Address [地址] FROM dbo.XFB_OriginalDataDts WHERE 1=1 AND ODD_Type=7");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "XFB_HighKeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[联系电话] FROM dbo.XFB_OriginalDataDts WHERE 1=1 AND ODD_Type=9");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "XFB_OtherData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[联系电话],ODD_Business[会员业务] FROM dbo.XFB_OriginalDataDts WHERE 1=1 AND ODD_Type=8");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_OriginalDataDts":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）] FROM dbo.SSB_OriginalDataDts WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code ='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_ValidData":
                        sb.Append(@"SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）],ODD_Address[地址],ODD_LinkMan[姓名]
                                    ,ODD_LinkPhone[联系电话] FROM dbo.SSB_OriginalDataValid WHERE 1=1");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_NOValidData":
                        sb.AppendFormat(@"SELECT ODD_Name[客户名称]
                                                , ODD_Phone[计费号码（预）]
                                            FROM SSB_OriginalDataDts
                                            WHERE ODD_OD_Code = '{0}' 
	                                            AND DataState= 0
	                                            AND ODD_Type >1
                                            ORDER BY ODD_Name", arr[0]);
                        break;
                    case "SSB_AlreadyUse":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）] FROM dbo.SSB_OriginalDataDts WHERE 1=1 AND ODD_Type=2");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_UnsubscribeUser":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）] FROM dbo.SSB_OriginalDataDts WHERE 1=1 AND ODD_Type=3");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_AlreadyCallData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）],ODD_Address [地址] FROM dbo.SSB_OriginalDataDts WHERE 1=1 AND ODD_Type=4");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_LoopData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）] FROM dbo.SSB_OriginalDataDts WHERE 1=1 AND ODD_Type=5");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_BlackUser":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）] FROM SSB_OriginalDataDts WHERE ODD_Type=6");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_KeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）],ODD_Address [地址] FROM dbo.SSB_OriginalDataDts WHERE 1=1 AND ODD_Type=7");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_HighKeyWordData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）] FROM dbo.SSB_OriginalDataDts WHERE 1=1 AND ODD_Type=9");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_OtherDataTuiDing"://其他业务退订
                        yearStr = arr[2].ToString() != "" ? $" AND year([新增时间] )={arr[2].ToString()}" : "";
                        string yearTDStr = arr[3].ToString() != "" ? $" AND year([退订时间] )={arr[3].ToString()}" : "";
                        sb.Append($@"SELECT  [客户名称],[计费号码（预）],[新增时间],[退订时间],[业务名称] FROM (SELECT d.ODD_Name [客户名称]
                                            , d.ODD_Phone [计费号码（预）]
                                            , CASE d.ODD_Business WHEN '诚信通' THEN c2.JoinDate WHEN '维权通' THEN c3.JoinDate ELSE c4.JoinDate END [新增时间]
                                            , CASE d.ODD_Business WHEN '诚信通' THEN isnull(c2.Cust_OutDate, c2.Cust_UnOrder) WHEN '维权通' THEN isnull(c3.Cust_OutDate, c3.Cust_UnOrder) ELSE isnull(c4.Cust_OutDate, c4.Cust_UnOrder) END [退订时间]
                                            , d.ODD_Business [业务名称],IsMoveData,ODD_Name
                                        FROM SSB_OriginalDataDts d
                                            LEFT JOIN CXT_Customer c2 ON d.ODD_Phone = c2.Cust_BillNumber AND c2.DataState = 0 AND c2.Cust_State IN ('退订', '退费') AND d.ODD_Business = '诚信通'
                                            LEFT JOIN WQT_Customer c3 ON d.ODD_Phone = c3.Cust_BillNumber AND c3.DataState = 0 AND c3.Cust_State IN ('退订', '退费') AND d.ODD_Business = '维权通'
                                            LEFT JOIN MQY_Customer c4 ON d.ODD_Phone = c4.Cust_BillNumber AND c4.DataState = 0 AND c4.Cust_State IN ('退订', '退费') AND d.ODD_Business = '民企云'
                                        WHERE ODD_OD_Code = '{arrName[0]}' AND d.DataState = 0 AND ODD_Type = 10)T where 1=1{yearStr} {yearTDStr}
                                        ORDER BY IsMoveData DESC, ODD_Name");
                        break;
                    case "SSB_OtherData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）],ODD_Business[会员业务] FROM dbo.SSB_OriginalDataDts WHERE 1=1 AND ODD_Type=8");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_OtherNoValidData"://无效
                        yearStr = arr[2].ToString() != "" ? $" WHERE year([新增时间] )={arr[2].ToString()}" : "";
                        sb.Append($@"SELECT  [客户名称],[计费号码（预）],[新增时间],[业务名称] FROM (SELECT  d.ODD_Name [客户名称]
                                    , d.ODD_Phone  [计费号码（预）]
                                    , CASE d.ODD_Business WHEN '实时保' THEN c1.JoinDate WHEN '诚信通' THEN c2.JoinDate WHEN '维权通' THEN c3.JoinDate ELSE c4.JoinDate END [新增时间] 
                                    , d.ODD_Business [业务名称] ,IsMoveData,ODD_Name
                                FROM SSB_OriginalDataDts d
                                    LEFT JOIN SSB_Customer c1 ON d.ODD_Phone = c1.Cust_BillNumber AND c1.DataState = 0 AND c1.Cust_State = '无效' AND d.ODD_Business = '实时保'
                                    LEFT JOIN CXT_Customer c2 ON d.ODD_Phone = c2.Cust_BillNumber AND c2.DataState = 0 AND c2.Cust_State = '无效' AND d.ODD_Business = '诚信通'
                                    LEFT JOIN WQT_Customer c3 ON d.ODD_Phone = c3.Cust_BillNumber AND c3.DataState = 0 AND c3.Cust_State = '无效' AND d.ODD_Business = '维权通'
                                    LEFT JOIN MQY_Customer c4 ON d.ODD_Phone = c4.Cust_BillNumber AND c4.DataState = 0 AND c4.Cust_State = '无效' AND d.ODD_Business = '民企云'
                                WHERE ODD_OD_Code = '{arrName[0]}' AND d.DataState = 0 AND ODD_Type = 11)T {yearStr} 
                                ORDER BY IsMoveData DESC, ODD_Name");
                        break;
                    case "SSB_OtherNameCommonData":
                        sb.Append("SELECT ODD_Name[客户名称],ODD_Phone[计费号码（预）],ODD_Business[会员业务] FROM dbo.SSB_OriginalDataDts WHERE ODD_Type=12");
                        if (ValueHandler.GetStringValue(arrName[0]) != "")
                            sb.Append(" AND ODD_OD_Code='" + ValueHandler.GetStringValue(arrName[0]) + "'");
                        break;
                    case "SSB_ViewChargeData":
                        sb.Append($@"SELECT d.CDD_Name [客户名称],d.CDD_Phone [计费号码], CONVERT (VARCHAR (20), d.CDD_ActiveDate, 23) [计费生效时间] FROM KF_ChargeDataDts d
                                        WHERE d.CDD_CD_Code = '{ValueHandler.GetStringValue(arrName[0])}'
                                        ORDER BY d.JoinDate ASC ");
                        break;
                }

                DAL.DAL_OriginalData dAL_OriginalData = new DAL.DAL_OriginalData();
                if (!dAL_OriginalData.CheckExportData(sb.ToString()))
                    return "false";

                HttpContext.Current.Session.Remove("FileData");
                HttpContext.Current.Session.Add("FileData", sb.ToString());
                HttpContext.Current.Session.Remove("FileName");

                switch (ValueHandler.GetStringValue(arr[1]))
                {
                    case "CXT_OriginalDataDts":
                        HttpContext.Current.Session.Add("FileName", "诚信通-原始数据明细表");
                        break;
                    case "CXT_ValidData":
                        HttpContext.Current.Session.Add("FileName", "诚信通-有效数据表");
                        break;
                    case "CXT_NOValidData":
                        HttpContext.Current.Session.Add("FileName", "诚信通-无效数据表");
                        break;
                    case "CXT_AlreadyUse":
                        HttpContext.Current.Session.Add("FileName", "诚信通-已是会员表");
                        break;
                    case "CXT_UnsubscribeUser":
                        HttpContext.Current.Session.Add("FileName", "诚信通-退订会员表");
                        break;
                    case "CXT_AlreadyCallData":
                        HttpContext.Current.Session.Add("FileName", "诚信通-已呼过数据表");
                        break;
                    case "CXT_LoopData":
                        HttpContext.Current.Session.Add("FileName", "诚信通-重复数据表");
                        break;
                    case "CXT_BlackUser":
                        HttpContext.Current.Session.Add("FileName", "诚信通-黑名单表");
                        break;
                    case "CXT_KeyWordData":
                        HttpContext.Current.Session.Add("FileName", "诚信通-关键字数据表");
                        break;
                    case "CXT_HighKeyWordData":
                        HttpContext.Current.Session.Add("FileName", "诚信通-关键字(高危)数据表");
                        break;
                    case "CXT_OtherData":
                        HttpContext.Current.Session.Add("FileName", "诚信通-其他业务会员数据表");
                        break;
                    case "MQY_OriginalDataDts":
                        HttpContext.Current.Session.Add("FileName", "民企云-原始数据明细表");
                        break;
                    case "MQY_ValidData":
                        HttpContext.Current.Session.Add("FileName", "民企云-有效数据表");
                        break;
                    case "MQY_NOValidData":
                        HttpContext.Current.Session.Add("FileName", "民企云-无效数据表");
                        break;
                    case "MQY_AlreadyUse":
                        HttpContext.Current.Session.Add("FileName", "民企云-已是会员表");
                        break;
                    case "MQY_UnsubscribeUser":
                        HttpContext.Current.Session.Add("FileName", "民企云-退订会员表");
                        break;
                    case "MQY_AlreadyCallData":
                        HttpContext.Current.Session.Add("FileName", "民企云-已呼过数据表");
                        break;
                    case "MQY_LoopData":
                        HttpContext.Current.Session.Add("FileName", "民企云-重复数据表");
                        break;
                    case "MQY_BlackUser":
                        HttpContext.Current.Session.Add("FileName", "民企云-黑名单表");
                        break;
                    case "MQY_KeyWordData":
                        HttpContext.Current.Session.Add("FileName", "民企云-关键字数据表");
                        break;
                    case "MQY_HighKeyWordData":
                        HttpContext.Current.Session.Add("FileName", "民企云-关键字(高危)数据表");
                        break;
                    case "MQY_OtherData":
                        HttpContext.Current.Session.Add("FileName", "民企云-其他业务会员数据表");
                        break;
                    case "WQT_OriginalDataDts":
                        HttpContext.Current.Session.Add("FileName", "维权通-原始数据明细表");
                        break;
                    case "WQT_ValidData":
                        HttpContext.Current.Session.Add("FileName", "维权通-有效数据表");
                        break;
                    case "WQT_NOValidData":
                        HttpContext.Current.Session.Add("FileName", "维权通-无效数据表");
                        break;
                    case "WQT_AlreadyUse":
                        HttpContext.Current.Session.Add("FileName", "维权通-已是会员表");
                        break;
                    case "WQT_UnsubscribeUser":
                        HttpContext.Current.Session.Add("FileName", "维权通-退订会员表");
                        break;
                    case "WQT_AlreadyCallData":
                        HttpContext.Current.Session.Add("FileName", "维权通-已呼过数据表");
                        break;
                    case "WQT_LoopData":
                        HttpContext.Current.Session.Add("FileName", "维权通-重复数据表");
                        break;
                    case "WQT_BlackUser":
                        HttpContext.Current.Session.Add("FileName", "维权通-黑名单表");
                        break;
                    case "WQT_KeyWordData":
                        HttpContext.Current.Session.Add("FileName", "维权通-关键字数据表");
                        break;
                    case "WQT_HighKeyWordData":
                        HttpContext.Current.Session.Add("FileName", "维权通-关键字(高危)数据表");
                        break;
                    case "WQT_OtherData":
                        HttpContext.Current.Session.Add("FileName", "维权通-其他业务会员数据表");
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
                    case "SSB_ValidData":
                        HttpContext.Current.Session.Add("FileName", "实时保-有效数据表");
                        break;
                    case "SSB_NOValidData":
                        HttpContext.Current.Session.Add("FileName", "实时保-无效数据表");
                        break;
                    case "SSB_AlreadyUse":
                        HttpContext.Current.Session.Add("FileName", "实时保-本业务会员数据表");
                        break;
                    case "SSB_UnsubscribeUser":
                        HttpContext.Current.Session.Add("FileName", "实时保-本业务退订/退费数据表");
                        break;
                    case "SSB_AlreadyCallData":
                        HttpContext.Current.Session.Add("FileName", "实时保-已呼过数据表");
                        break;
                    case "SSB_LoopData":
                        HttpContext.Current.Session.Add("FileName", "实时保-重复数据表");
                        break;
                    case "SSB_BlackUser":
                        HttpContext.Current.Session.Add("FileName", "实时保-黑名单表");
                        break;
                    case "SSB_KeyWordData":
                        HttpContext.Current.Session.Add("FileName", "实时保-关键字(低危)数据表");
                        break;
                    case "SSB_HighKeyWordData":
                        HttpContext.Current.Session.Add("FileName", "实时保-关键字(高危)数据表");
                        break;
                    case "SSB_OtherData":
                        HttpContext.Current.Session.Add("FileName", "实时保-其他业务会员数据表");
                        break;
                    case "SSB_OtherDataTuiDing":
                        HttpContext.Current.Session.Add("FileName", "实时保-其他业务退订/退费数据表");
                        break;
                    case "SSB_OtherNoValidData":
                        HttpContext.Current.Session.Add("FileName", "实时保-无效(所有业务)数据表");
                        break;
                    case "SSB_OtherNameCommonData":
                        HttpContext.Current.Session.Add("FileName", "实时保-名称相同退订/退费数据表");
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