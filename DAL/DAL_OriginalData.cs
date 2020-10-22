////////////////////////////://///////////////////////////////////////////////
//模块名：维权通数据筛选
//开发者：赵虎
//开发时间：2016年11月24日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAL.InMolde;
using DAL.Service;
using HCWeb2016;

namespace DAL
{
    public class DAL_OriginalData : SqlBase
    {
        #region 新消费宝典

        #region 列表数据

        /// <summary>
        /// 查询原始数据
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <param name="PageIndex">第一页</param>
        /// <param name="PageNum">每页数量</param>
        /// <returns></returns>
        public DataTable GetXFBOriginalData(string Provider, string ProDateStart, string ProDateEnd, string Province, string City, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT OD_Code
                                          , OD_Provider
                                          , CONVERT(varchar(10), OD_ProviderTime, 23) OD_ProviderTime
                                          , isnull(OD_ProvinceName,'') + isnull(OD_CityName,'') AS AreaName
                                          , OD_TotalCount
                                          , OD_ValidCount
                                          , ISNULL(OD_NoValidCount,0) OD_NoValidCount
                                          , isnull(OD_AlreadyUse,0) OD_AlreadyUse
                                          , OD_State
                                          , ROW_NUMBER () OVER (ORDER BY JoinDate DESC) AS 'Num'
                                      FROM XFB_OriginalData
                                      WHERE DataState = 0", PageNum);

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取查询总数
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <returns></returns>
        public string GetXFBOriginalDataCount(string Provider, string ProDateStart, string ProDateEnd, string Province, string City)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT COUNT(*) AS DataCount FROM XFB_OriginalData WHERE DataState = 0");

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 删除原始数据

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteXFBDatas(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State FROM XFB_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                switch (dt.Rows[0]["OD_State"].ToString())
                {
                    case "已筛选":
                        sb.Append("DELETE  FROM XFB_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM XFB_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM XFB_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                    case "未筛选":
                        sb.Append("DELETE  FROM XFB_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM XFB_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 标记为已使用

        public bool MarkXFBAlreadyUse(string odCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE XFB_OriginalData SET OD_State='已使用' WHERE OD_Code='{0}'", odCode);

            return UpdateData(sb.ToString());
        }

        #endregion

        #region 导入

        /// <summary>
        /// 批量导入 只处理空 - , / 全角
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="AreaCode">地区编码</param>
        /// <param name="CityName">城市名称</param>
        /// <param name="JoinMan">录入人</param>
        /// <returns></returns>
        public bool PlImportXFBOrignData(string FileName, string Provider, string ProviderTime,
                                    string ProvinceCode, string CityCode, string BillMoney, string JoinMan)
        {
            bool flag = false;

            #region 旧代码
            /*
            int result;
            string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            //FileName = "2019/03/20190327061814517578.txt";
            abPath = "D:/hcUpfile/FileUp/";
            flag = new SqlBase().ExcuteNonQuery_Sp("SP_PLInsertXFBOrignData", new SqlParameter[] {
                        new SqlParameter("@FileName",abPath+FileName),
                        new SqlParameter("@Provider",ValueHandler.GetStringValue(Provider)),
                        new SqlParameter("@ProviderTime",Convert.ToDateTime(ProviderTime)),
                        new SqlParameter("@ProvinceCode",ValueHandler.GetStringValue(ProvinceCode)),
                        new SqlParameter("@CityCode",ValueHandler.GetStringValue(CityCode)),
                        new SqlParameter("@BillMoney",ValueHandler.GetStringValue(BillMoney)),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
              */
            #endregion

            #region 新导入数据库不读文件 使用程序sqlbulk导入

            string LastFileName = FileName.Substring(FileName.LastIndexOf('/') + 1);
            string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            try
            {
                if (!File.Exists(abPath + LastFileName))//判断文件存不存在 不存在重新下载
                {
                    using (var client = new System.Net.WebClient())
                    {
                        client.DownloadFile(FileName, HttpContext.Current.Request.PhysicalApplicationPath + "UpFile\\Files\\" + Path.GetFileName(LastFileName));
                    }
                }
                else
                {
                    using (DataTable table = new DataTable())
                    {
                        //为数据表创建相对应的数据列
                        table.Columns.Add("客户名称");
                        table.Columns.Add("座机号码");
                        table.Columns.Add("备注");
                        table.Columns.Add("是否计费");
                        table.Columns.Add("ODD_OD_Code");
                        table.Columns.Add("JoinMan");
                        table.Columns.Add("ODD_Code");
                        string ODD_OD_Code = GetCode();

                        using (StreamReader sr = new StreamReader(abPath + LastFileName, Encoding.GetEncoding("gb2312")))
                        {
                            string str;
                            sr.ReadLine();//第一行列头数据
                            while ((str = sr.ReadLine()) != null)
                            {
                                string[] temp = str.Replace("\"", "").Split((char)9);

                                //插入数据到Datatable供bulkinsert
                                //客户名称和号码都存在才导入 否则过滤不导入
                                if (!string.IsNullOrEmpty(temp[0]) && !string.IsNullOrEmpty(temp[1]))
                                {
                                    DataRow dr = table.NewRow();//创建数据行
                                    dr["客户名称"] = temp[0].Trim().Replace(" ", "");
                                    dr["座机号码"] = temp[1].Trim().Replace(" ", "").Replace(",", "").Replace("-", "").Replace("/", "");
                                    dr["备注"] = temp[3].Trim().Replace(" ", "");
                                    dr["是否计费"] = "是";
                                    dr["ODD_OD_Code"] = ODD_OD_Code;
                                    dr["JoinMan"] = JoinMan;
                                    dr["ODD_Code"] = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

                                    //将创建的数据行添加到table中
                                    table.Rows.Add(dr);
                                }
                            }
                        }
                        using (SqlConnection destinationConnection = new SqlConnection(ConfigurationManager.AppSettings["ConnStringSQL"]))
                        {
                            destinationConnection.Open();

                            using (SqlTransaction transaction = destinationConnection.BeginTransaction())
                            {
                                //先插入主表信息
                                SqlCommand cmd = new SqlCommand($@"INSERT INTO XFB_OriginalData (OD_Code
												, OD_Provider
												, OD_ProviderTime
												, OD_ProvinceCode
												, OD_ProvinceName
												, OD_CityCode
												, OD_CityName
												, OD_BillMoney
											, OD_TotalCount
											, OD_State
											, JoinMan)
										SELECT '{ODD_OD_Code}'
											, '{Provider}'
											, '{ProviderTime}'
											, '{ProvinceCode}'
											, (SELECT SA_Name
											   FROM SYS_Area
											   WHERE SA_Code = '{ProvinceCode}')
											, '{CityCode}'
											, (SELECT SA_Name
											   FROM SYS_Area
											   WHERE SA_Code = '{CityCode}')
											, {BillMoney}		    
											, {table.Rows.Count}
											, '未筛选'
											, '{JoinMan}'", destinationConnection);
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();

                                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.Default, transaction))
                                {
                                    bulkCopy.DestinationTableName = "XFB_OriginalDataDts";//设置数据库中对象的表名

                                    //设置数据表table和数据库中表的列对应关系
                                    bulkCopy.ColumnMappings.Add("客户名称", "ODD_Name");
                                    bulkCopy.ColumnMappings.Add("座机号码", "ODD_Phone");
                                    bulkCopy.ColumnMappings.Add("备注", "ODD_Remark");
                                    bulkCopy.ColumnMappings.Add("是否计费", "ODD_IsBill");
                                    bulkCopy.ColumnMappings.Add("ODD_OD_Code", "ODD_OD_Code");
                                    bulkCopy.ColumnMappings.Add("JoinMan", "JoinMan");
                                    bulkCopy.ColumnMappings.Add("ODD_Code", "ODD_Code");

                                    try
                                    {
                                        bulkCopy.WriteToServer(table);
                                        transaction.Commit();
                                        flag = true;
                                    }
                                    catch
                                    {
                                        transaction.Rollback();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                flag = false;
            }

            #endregion
            return flag;
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <param name="billMoney">计费金额</param>
        /// <param name="provinceCode">省Code</param>
        /// <param name="cityCode">市Code</param>
        /// <returns></returns>
        public bool UpdateXFBDatas(string OD_Code, string billMoney, string provinceCode, string cityCode)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State,OD_CityCode FROM XFB_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                sb.AppendFormat(@"UPDATE XFB_OriginalData
                                    SET OD_ProvinceName = (SELECT SA_Name
                                                           FROM SYS_Area
                                                           WHERE SA_Code = '{0}')
                                    , OD_ProvinceCode = '{0}'
                                    , OD_CityCode = '{1}'
                                    , OD_CityName = (SELECT SA_Name
                                                     FROM SYS_Area
                                                     WHERE SA_Code = '{1}')
                                    , OD_BillMoney = {2}
                                WHERE OD_Code = '{3}';", provinceCode, cityCode, billMoney, OD_Code);

                //地区变了 清空筛选类别 清空有效数据
                if (dt.Rows[0]["OD_CityCode"].ToString() != cityCode)
                {
                    switch (dt.Rows[0]["OD_State"].ToString())
                    {
                        case "已筛选":
                            sb.AppendFormat("UPDATE XFB_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}';", OD_Code);
                            sb.Append("DELETE FROM XFB_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                            UpdateData(sb.ToString());
                            break;
                        case "未筛选":
                            sb.AppendFormat("UPDATE XFB_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}'", OD_Code);
                            UpdateData(sb.ToString());
                            break;
                    }
                }
                else
                    UpdateData(sb.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <returns></returns>
        public DataTable GetXFBOriginData(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT *,CONVERT(varchar(10), OD_ProviderTime, 23) ProviderTime FROM XFB_OriginalData WHERE DataState = 0 AND OD_Code = '{0}'", OD_Code);

            return SearchData(sb.ToString());
        }

        #endregion

        #region 无效数据

        public bool XFBInvalidData(string FileName)
        {
            bool flag = false;
            int result;
            string abPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            //FileName = "2019/03/201903270618145175782.txt";
            abPath = "D:/hcUpfile/FileUp/";
            flag = new SqlBase().ExcuteNonQuery_Sp("SP_PLDeleteXFBInvalidData", new SqlParameter[] {
                        new SqlParameter("@FileName",abPath+FileName)
              }, out result);

            return flag;
        }

        #endregion

        #region 获取查看数据

        #region 明细数据

        /// <summary>
        /// 明细数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <returns></returns>
        public DataTable GetXFBOriginalDataDts(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT ODD_Name
                                          , ODD_Phone
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM XFB_OriginalDataDts
                                      WHERE DataState = 0 AND ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetXFBOriginalDataDtsCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM XFB_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 有效数据

        public DataTable GetXFBValidData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT *
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM XFB_OriginalDataValid
                                      WHERE ODD_OD_Code = '{1}' AND DataState = 0"
                                , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetXFBValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM XFB_OriginalDataValid WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 无效数据

        public DataTable GetXFBNOVaildData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT ODD_Name
                                    , ODD_Phone
                                    , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                FROM XFB_OriginalDataDts
                                WHERE ODD_OD_Code = '{1}' 
	                                AND DataState= 0
                                    AND ODD_Type >1"
                               , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetXFBNoValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT Count (*) AS DataCount
                                FROM XFB_OriginalDataDts
                                WHERE ODD_OD_Code = '{0}' AND DataState = 0 AND ODD_Type > 1", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }
        #endregion

        #endregion

        #endregion

        #region 实时保

        #region 列表数据

        /// <summary>
        /// 查询原始数据
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <param name="PageIndex">第一页</param>
        /// <param name="PageNum">每页数量</param>
        /// <returns></returns>
        public DataTable GetSSBOriginalData(string Provider, string ProDateStart, string ProDateEnd, string Province, string City, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT OD_Code
                                          , OD_Provider
                                          , CONVERT(varchar(10), OD_ProviderTime, 23) OD_ProviderTime
                                          , OD_ProvinceName + OD_CityName AS AreaName
                                          , OD_TotalCount
                                          , OD_ValidCount
                                          , ISNULL(OD_NoValidCount,0) OD_NoValidCount
                                          , isnull(OD_AlreadyUse,0) OD_AlreadyUse
                                          , OD_State
                                          , ROW_NUMBER () OVER (ORDER BY JoinDate DESC) AS 'Num'
                                      FROM SSB_OriginalData
                                      WHERE DataState = 0", PageNum);

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取查询总数
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <returns></returns>
        public string GetSSBOriginalDataCount(string Provider, string ProDateStart, string ProDateEnd, string Province, string City)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT COUNT(*) AS DataCount FROM SSB_OriginalData WHERE DataState = 0");

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 删除原始数据

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteSSBDatas(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State FROM SSB_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                switch (dt.Rows[0]["OD_State"].ToString())
                {
                    case "已筛选":
                        sb.Append("DELETE FROM SSB_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE FROM SSB_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE FROM SSB_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                    case "未筛选":
                        sb.Append("DELETE FROM SSB_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE FROM SSB_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 标记为已使用

        public bool MarkSSBAlreadyUse(string odCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE SSB_OriginalData SET OD_State='已使用' WHERE OD_Code='{0}'", odCode);

            return UpdateData(sb.ToString());
        }

        #endregion

        #region 导入

        /// <summary>
        /// 批量导入 只处理空 - , / 全角
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="AreaCode">地区编码</param>
        /// <param name="CityName">城市名称</param>
        /// <param name="JoinMan">录入人</param>
        /// <returns></returns>
        public bool PlImportSSBOrignData(string FileName, string Provider, string ProviderTime,
                                    string ProvinceCode, string CityCode, string @BillMoney, string JoinMan)
        {
            bool flag = false;
            int result;
            string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            abPath = "D:/hcUpfile/FileUp/";
            flag = new SqlBase().ExcuteNonQuery_Sp("SP_PLInsertSSBOrignData", new SqlParameter[] {
                        new SqlParameter("@FileName",abPath+FileName),
                        new SqlParameter("@Provider",ValueHandler.GetStringValue(Provider)),
                        new SqlParameter("@ProviderTime",Convert.ToDateTime(ProviderTime)),
                        new SqlParameter("@ProvinceCode",ValueHandler.GetStringValue(ProvinceCode)),
                        new SqlParameter("@CityCode",ValueHandler.GetStringValue(CityCode)),
                        new SqlParameter("@BillMoney",ValueHandler.GetStringValue(BillMoney)),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <param name="billMoney">计费金额</param>
        /// <param name="provinceCode">省Code</param>
        /// <param name="cityCode">市Code</param>
        /// <returns></returns>
        public bool UpdateSSBDatas(string OD_Code, string billMoney, string provinceCode, string cityCode)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State,OD_CityCode FROM SSB_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                sb.AppendFormat(@"UPDATE SSB_OriginalData
                                    SET OD_ProvinceName = (SELECT SA_Name
                                                           FROM SYS_Area
                                                           WHERE SA_Code = '{0}')
                                    , OD_ProvinceCode = '{0}'
                                    , OD_CityCode = '{1}'
                                    ,OD_State = '未筛选'
                                    , OD_CityName = (SELECT SA_Name
                                                     FROM SYS_Area
                                                     WHERE SA_Code = '{1}')
                                    , OD_BillMoney = {2}
                                WHERE OD_Code = '{3}';", provinceCode, cityCode, billMoney, OD_Code);

                //地区变了 清空筛选类别 清空有效数据
                if (dt.Rows[0]["OD_CityCode"].ToString() != cityCode)
                {
                    switch (dt.Rows[0]["OD_State"].ToString())
                    {
                        case "已筛选":
                            sb.AppendFormat("UPDATE SSB_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}';", OD_Code);
                            sb.Append("DELETE FROM SSB_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                            UpdateData(sb.ToString());
                            break;
                        case "未筛选":
                            sb.AppendFormat("UPDATE SSB_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}'", OD_Code);
                            UpdateData(sb.ToString());
                            break;
                    }
                }
                else
                    UpdateData(sb.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <returns></returns>
        public DataTable GetSSBOriginData(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT *,CONVERT(varchar(10), OD_ProviderTime, 23) ProviderTime FROM SSB_OriginalData WHERE DataState = 0 AND OD_Code = '{0}'", OD_Code);

            return SearchData(sb.ToString());
        }

        #endregion

        #region 获取查看数据

        #region 明细数据

        /// <summary>
        /// 明细数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <returns></returns>
        public DataTable GetSSBOriginalDataDts(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT ODD_Name
                                          , ODD_Phone
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM SSB_OriginalDataDts
                                      WHERE DataState = 0 AND ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetSSBOriginalDataDtsCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM SSB_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 有效数据

        public DataTable GetSSBValidData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT *
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM SSB_OriginalDataValid
                                      WHERE ODD_OD_Code = '{1}' AND DataState = 0"
                                , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetSSBValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM SSB_OriginalDataValid WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 无效数据

        public DataTable GetSSBNOVaildData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT ODD_Name
                                    , ODD_Phone
                                    , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                FROM SSB_OriginalDataDts
                                WHERE ODD_OD_Code = '{1}' 
	                                AND DataState= 0
                                    AND ODD_Type >1"
                               , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetSSBNoValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT Count (*) AS DataCount
                                FROM SSB_OriginalDataDts
                                WHERE ODD_OD_Code = '{0}' AND DataState = 0 AND ODD_Type > 1", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 营销成功

        public DataTable GetSSBAlreadyUseData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                            FROM (SELECT *
                                      , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                  FROM (SELECT DISTINCT *
                                        FROM (SELECT d.ODD_Code
                                                  , d.ODD_Name
                                                  , d.ODD_Phone
                                              FROM SSB_OriginalDataDts d
                                                  JOIN SSB_Customer c ON c.Cust_Phone = d.ODD_Phone
                                              WHERE d.ODD_OD_Code = '{1}' UNION
                                            SELECT d.ODD_Code
                                                , d.ODD_Name
                                                , d.ODD_Phone
                                            FROM SSB_OriginalDataDts d
                                                JOIN SSB_Customer c ON c.Cust_OldName = d.ODD_Name
                                            WHERE d.ODD_OD_Code = '{1}') t) r) r"
                               , PageNum, OD_Code);

            sb.AppendFormat(" WHERE r.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetSSBAlreadyUseDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(DISTINCT ODD_Code) AS DataCount
                                FROM (SELECT d.ODD_Code
                                          , d.ODD_Name
                                          , d.ODD_Phone
                                      FROM SSB_OriginalDataDts d
                                          JOIN SSB_Customer c ON c.Cust_Phone = d.ODD_Phone
                                      WHERE d.ODD_OD_Code = '{0}' UNION
                                    SELECT d.ODD_Code
                                        , d.ODD_Name
                                        , d.ODD_Phone
                                    FROM SSB_OriginalDataDts d
                                        JOIN SSB_Customer c ON c.Cust_OldName = d.ODD_Name
                                    WHERE d.ODD_OD_Code = '{0}') t", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion 

        #endregion

        #endregion

        #region 诚信通

        #region 列表数据

        /// <summary>
        /// 查询原始数据
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <param name="PageIndex">第一页</param>
        /// <param name="PageNum">每页数量</param>
        /// <returns></returns>
        public DataTable GetOriginalData(string Provider, string ProDateStart, string ProDateEnd, string Province, string City, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT OD_Code
                                          , OD_Provider
                                          , CONVERT(varchar(10), OD_ProviderTime, 23) OD_ProviderTime
                                          , OD_ProvinceName + OD_CityName AS AreaName
                                          , OD_TotalCount
                                          , OD_ValidCount
                                          , ISNULL(OD_NoValidCount,0) OD_NoValidCount
                                          , isnull(OD_AlreadyUse,0) OD_AlreadyUse
                                          , OD_State
                                          , ROW_NUMBER () OVER (ORDER BY JoinDate DESC) AS 'Num'
                                      FROM CXT_OriginalData
                                      WHERE DataState = 0", PageNum);

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取查询总数
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <returns></returns>
        public string GetOriginalDataCount(string Provider, string ProDateStart, string ProDateEnd, string Province, string City)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT COUNT(*) AS DataCount FROM CXT_OriginalData WHERE DataState = 0");

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 删除原始数据

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteDatas(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State FROM CXT_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                switch (dt.Rows[0]["OD_State"].ToString())
                {
                    case "已筛选":
                        sb.Append("DELETE  FROM CXT_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM CXT_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM CXT_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                    case "未筛选":
                        sb.Append("DELETE  FROM CXT_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM CXT_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 标记为已使用

        public bool MarkAlreadyUse(string odCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE CXT_OriginalData SET OD_State='已使用' WHERE OD_Code='{0}'", odCode);

            return UpdateData(sb.ToString());
        }

        #endregion

        #region 导入

        /// <summary>
        /// 批量导入 只处理空 - , / 全角
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="AreaCode">地区编码</param>
        /// <param name="CityName">城市名称</param>
        /// <param name="JoinMan">录入人</param>
        /// <returns></returns>
        public bool PlImportOrignData(string FileName, string Provider, string ProviderTime,
                                    string ProvinceCode, string CityCode, string @BillMoney, string JoinMan, string tablePre)
        {
            bool flag = false;

            #region  旧代码
            /*
            int result;
            string abPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            abPath = "D:/hcUpfile/FileUp/";
            flag = new SqlBase().ExcuteNonQuery_Sp("SP_PLInsertOrignData", new SqlParameter[] {
                        new SqlParameter("@FileName",abPath+FileName),
                        new SqlParameter("@Provider",ValueHandler.GetStringValue(Provider)),
                        new SqlParameter("@ProviderTime",Convert.ToDateTime(ProviderTime)),
                        new SqlParameter("@ProvinceCode",ValueHandler.GetStringValue(ProvinceCode)),
                        new SqlParameter("@CityCode",ValueHandler.GetStringValue(CityCode)),
                        new SqlParameter("@BillMoney",ValueHandler.GetStringValue(BillMoney)),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);

            */
            #endregion

            #region 新导入数据库不读文件 使用程序sqlbulk导入

            string LastFileName = FileName.Substring(FileName.LastIndexOf('/') + 1);
            string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            try
            {
                if (!File.Exists(abPath + LastFileName))//判断文件存不存在 不存在重新下载
                {
                    using (var client = new System.Net.WebClient())
                    {
                        client.DownloadFile(FileName, HttpContext.Current.Request.PhysicalApplicationPath + "UpFile\\Files\\" + Path.GetFileName(LastFileName));
                    }
                }
                else
                {
                    using (DataTable table = new DataTable())
                    {
                        //为数据表创建相对应的数据列
                        table.Columns.Add("客户名称");
                        table.Columns.Add("座机号码");
                        table.Columns.Add("地址");
                        table.Columns.Add("联系人");
                        table.Columns.Add("联系电话");
                        table.Columns.Add("是否计费");
                        table.Columns.Add("ODD_OD_Code");
                        table.Columns.Add("JoinMan");
                        table.Columns.Add("ODD_Code");
                        string ODD_OD_Code = GetCode();

                        using (StreamReader sr = new StreamReader(abPath + LastFileName, Encoding.GetEncoding("gb2312")))
                        {
                            string str;
                            sr.ReadLine();//第一行列头数据
                            while ((str = sr.ReadLine()) != null)
                            {
                                string[] temp = str.Replace("\"", "").Split((char)9);

                                //插入数据到Datatable供bulkinsert
                                //客户名称和号码都存在才导入 否则过滤不导入
                                if (!string.IsNullOrEmpty(temp[0]) && !string.IsNullOrEmpty(temp[1]))
                                {
                                    DataRow dr = table.NewRow();//创建数据行
                                    dr["客户名称"] = temp[0].Trim().Replace(" ", "");
                                    dr["座机号码"] = temp[1].Trim().Replace(" ", "").Replace(",", "").Replace("-", "").Replace("/", "");
                                    dr["地址"] = temp[2].Trim().Replace(" ", "");
                                    dr["联系人"] = temp[3].Trim().Replace(" ", "");
                                    dr["联系电话"] = temp[4].Trim().Replace(" ", "").Replace(",", "").Replace("-", "").Replace("/", "");
                                    dr["是否计费"] = "是";
                                    dr["ODD_OD_Code"] = ODD_OD_Code;
                                    dr["JoinMan"] = JoinMan;
                                    dr["ODD_Code"] = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

                                    //将创建的数据行添加到table中
                                    table.Rows.Add(dr);
                                }
                            }
                        }
                        using (SqlConnection destinationConnection = new SqlConnection(ConfigurationManager.AppSettings["ConnStringSQL"]))
                        {
                            destinationConnection.Open();

                            using (SqlTransaction transaction = destinationConnection.BeginTransaction())
                            {
                                //先插入主表信息
                                SqlCommand cmd = new SqlCommand($@"INSERT INTO {tablePre}_OriginalData (OD_Code
												, OD_Provider
												, OD_ProviderTime
												, OD_ProvinceCode
												, OD_ProvinceName
												, OD_CityCode
												, OD_CityName
												, OD_BillMoney
											, OD_TotalCount
											, OD_State
											, JoinMan)
										SELECT '{ODD_OD_Code}'
											, '{Provider}'
											, '{ProviderTime}'
											, '{ProvinceCode}'
											, (SELECT SA_Name
											   FROM SYS_Area
											   WHERE SA_Code = '{ProvinceCode}')
											, '{CityCode}'
											, (SELECT SA_Name
											   FROM SYS_Area
											   WHERE SA_Code = '{CityCode}')
											, {BillMoney}		    
											, {table.Rows.Count}
											, '未筛选'
											, '{JoinMan}'", destinationConnection);
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();

                                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.Default, transaction))
                                {
                                    bulkCopy.DestinationTableName = tablePre + "_OriginalDataDts";//设置数据库中对象的表名
                                    bulkCopy.BatchSize = 1000;
                                    bulkCopy.BulkCopyTimeout = 5000;
                                    //设置数据表table和数据库中表的列对应关系
                                    bulkCopy.ColumnMappings.Add("客户名称", "ODD_Name");
                                    bulkCopy.ColumnMappings.Add("座机号码", "ODD_Phone");
                                    bulkCopy.ColumnMappings.Add("地址", "ODD_Address");
                                    bulkCopy.ColumnMappings.Add("联系人", "ODD_LinkMan");
                                    bulkCopy.ColumnMappings.Add("联系电话", "ODD_LinkPhone");
                                    bulkCopy.ColumnMappings.Add("是否计费", "ODD_IsBill");
                                    bulkCopy.ColumnMappings.Add("ODD_OD_Code", "ODD_OD_Code");
                                    bulkCopy.ColumnMappings.Add("JoinMan", "JoinMan");
                                    bulkCopy.ColumnMappings.Add("ODD_Code", "ODD_Code");

                                    try
                                    {
                                        bulkCopy.WriteToServer(table);
                                        transaction.Commit();
                                        flag = true;
                                    }
                                    catch//(Exception ex)
                                    {
                                        transaction.Rollback();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                flag = false;
            }

            #endregion

            return flag;
        }

        /// <summary>
        /// 批量导入有效数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BatchImportValidData(string FileName, string JoinMan, string tablePre)
        {
            bool flag = false;
            #region 新导入数据库不读文件 使用程序sqlbulk导入
            string LastFileName = FileName.Substring(FileName.LastIndexOf('/') + 1);
            string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            try
            {
                if (!File.Exists(abPath + LastFileName))//判断文件存不存在 不存在重新下载
                {
                    using (var client = new System.Net.WebClient())
                    {
                        client.DownloadFile(FileName, HttpContext.Current.Request.PhysicalApplicationPath + "UpFile\\Files\\" + Path.GetFileName(LastFileName));
                    }
                }
                else
                {
                    using (DataTable table = new DataTable())
                    {
                        //为数据表创建相对应的数据列
                        table.Columns.Add("客户编码");
                        table.Columns.Add("客户名称");
                        table.Columns.Add("计费号码");
                        table.Columns.Add("地址");
                        table.Columns.Add("联系人");
                        table.Columns.Add("联系电话");
                        table.Columns.Add("是否计费");
                        table.Columns.Add("JoinMan");
                        table.Columns.Add("ODD_Code");
                        table.Columns.Add("城市编码");
                        //string ODD_OD_Code = GetCode();

                        string cityCode = string.Empty;

                        using (StreamReader sr = new StreamReader(abPath + LastFileName, Encoding.GetEncoding("gb2312")))
                        {
                            string str;
                            sr.ReadLine();//第一行列头数据
                            while ((str = sr.ReadLine()) != null)
                            {
                                string[] temp = str.Replace("\"", "").Split((char)9);

                                //插入数据到Datatable供bulkinsert
                                //客户名称和号码都存在才导入 否则过滤不导入
                                if (!string.IsNullOrEmpty(temp[0]) && !string.IsNullOrEmpty(temp[1]))
                                {
                                    if (string.IsNullOrEmpty(cityCode))
                                    {
                                        var tab = tablePre + "_OriginalData";
                                        var db = DbService.Instance;
                                        var str1 = $@"SELECT OD_CityCode FROM " + tab + " WHERE OD_Code= '" + temp[0].Trim().Replace(" ", "") + "'";
                                        cityCode = db.Ado.SqlQuery<string>(str1, new { })[0];
                                    }
                                    DataRow dr = table.NewRow();//创建数据行
                                    dr["客户编码"] = temp[0].Trim().Replace(" ", "");
                                    dr["客户名称"] = temp[1].Trim().Replace(" ", "");
                                    dr["计费号码"] = temp[2].Trim().Replace(" ", "").Replace(",", "").Replace("-", "").Replace("/", "");
                                    dr["地址"] = temp[3].Trim().Replace(" ", "");
                                    dr["联系人"] = temp[4].Trim().Replace(" ", "");
                                    dr["联系电话"] = temp[5].Trim().Replace(" ", "").Replace(",", "").Replace("-", "").Replace("/", "");
                                    dr["是否计费"] = temp[6].Trim() == null ? "是" : "否";
                                    dr["JoinMan"] = JoinMan;
                                    dr["ODD_Code"] = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
                                    dr["城市编码"] = cityCode;

                                    //将创建的数据行添加到table中
                                    table.Rows.Add(dr);
                                }
                            }
                        }
                        using (SqlConnection destinationConnection = new SqlConnection(ConfigurationManager.AppSettings["ConnStringSQL"]))
                        {
                            destinationConnection.Open();

                            using (SqlTransaction transaction = destinationConnection.BeginTransaction())
                            {
                                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection, SqlBulkCopyOptions.Default, transaction))
                                {
                                    bulkCopy.DestinationTableName = tablePre + "_OriginalDataValid"; //设置数据库中对象的表名
                                    bulkCopy.BatchSize = 1000;
                                    bulkCopy.BulkCopyTimeout = 5000;
                                    //设置数据表table和数据库中表的列对应关系
                                    bulkCopy.ColumnMappings.Add("ODD_Code", "ODD_Code");
                                    bulkCopy.ColumnMappings.Add("客户编码", "ODD_OD_Code");
                                    bulkCopy.ColumnMappings.Add("客户名称", "ODD_Name");
                                    bulkCopy.ColumnMappings.Add("计费号码", "ODD_Phone");
                                    bulkCopy.ColumnMappings.Add("地址", "ODD_Address");
                                    bulkCopy.ColumnMappings.Add("联系人", "ODD_LinkMan");
                                    bulkCopy.ColumnMappings.Add("联系电话", "ODD_LinkPhone");
                                    bulkCopy.ColumnMappings.Add("是否计费", "ODD_IsBill");
                                    bulkCopy.ColumnMappings.Add("JoinMan", "JoinMan");
                                    bulkCopy.ColumnMappings.Add("城市编码", "ODD_CityCode");

                                    try
                                    {
                                        bulkCopy.WriteToServer(table);
                                        transaction.Commit();
                                        flag = true;
                                    }
                                    catch//(Exception ex)
                                    {
                                        transaction.Rollback();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
            #endregion

        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <param name="billMoney">计费金额</param>
        /// <param name="provinceCode">省Code</param>
        /// <param name="cityCode">市Code</param>
        /// <returns></returns>
        public bool UpdateDatas(string OD_Code, string billMoney, string provinceCode, string cityCode)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State,OD_CityCode FROM CXT_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                sb.AppendFormat(@"UPDATE CXT_OriginalData
                                    SET OD_ProvinceName = (SELECT SA_Name
                                                           FROM SYS_Area
                                                           WHERE SA_Code = '{0}')
                                    , OD_ProvinceCode = '{0}'
                                    , OD_CityCode = '{1}'
                                    , OD_CityName = (SELECT SA_Name
                                                     FROM SYS_Area
                                                     WHERE SA_Code = '{1}')
                                    , OD_BillMoney = {2}
                                WHERE OD_Code = '{3}';", provinceCode, cityCode, billMoney, OD_Code);

                //地区变了 清空筛选类别 清空有效数据
                if (dt.Rows[0]["OD_CityCode"].ToString() != cityCode)
                {
                    switch (dt.Rows[0]["OD_State"].ToString())
                    {
                        case "已筛选":
                            sb.AppendFormat("UPDATE CXT_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}';", OD_Code);
                            sb.Append("DELETE  FROM CXT_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                            UpdateData(sb.ToString());
                            break;
                        case "未筛选":
                            sb.AppendFormat("UPDATE CXT_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}'", OD_Code);
                            UpdateData(sb.ToString());
                            break;
                    }
                }
                else
                    UpdateData(sb.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <returns></returns>
        public DataTable GetOriginData(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT *,CONVERT(varchar(10), OD_ProviderTime, 23) ProviderTime FROM CXT_OriginalData WHERE DataState = 0 AND OD_Code = '{0}'", OD_Code);

            return SearchData(sb.ToString());
        }

        #endregion

        #region 获取查看数据

        #region 明细数据

        /// <summary>
        /// 明细数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <returns></returns>
        public DataTable GetOriginalDataDts(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT ODD_Name
                                          , ODD_Phone
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM CXT_OriginalDataDts
                                      WHERE DataState = 0 AND ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetOriginalDataDtsCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM CXT_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 有效数据

        public DataTable GetValidData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT *
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM CXT_OriginalDataValid
                                      WHERE ODD_OD_Code = '{1}' AND DataState = 0"
                                , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM CXT_OriginalDataValid WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 无效数据

        public DataTable GetNOVaildData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT ODD_Name
                                    , ODD_Phone
                                    , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                FROM CXT_OriginalDataDts
                                WHERE ODD_OD_Code = '{1}' 
	                                AND DataState= 0
                                    AND ODD_Type >1"
                               , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }


        public string GetNoValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT Count (*) AS DataCount
                                FROM CXT_OriginalDataDts
                                WHERE ODD_OD_Code = '{0}' AND DataState = 0 AND ODD_Type > 1", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }
        #endregion

        #region 营销成功

        public DataTable GetAlreadyUseData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                            FROM (SELECT *
                                      , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                  FROM (SELECT DISTINCT *
                                        FROM (SELECT d.ODD_Code
                                                  , d.ODD_Name
                                                  , d.ODD_Phone
                                              FROM CXT_OriginalDataDts d
                                                  JOIN CXT_Customer c ON c.Cust_Phone = d.ODD_Phone
                                              WHERE d.ODD_OD_Code = '{1}' UNION
                                            SELECT d.ODD_Code
                                                , d.ODD_Name
                                                , d.ODD_Phone
                                            FROM CXT_OriginalDataDts d
                                                JOIN CXT_Customer c ON c.Cust_OldName = d.ODD_Name
                                            WHERE d.ODD_OD_Code = '{1}') t) r) r"
                               , PageNum, OD_Code);

            sb.AppendFormat(" WHERE r.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetAlreadyUseDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(DISTINCT ODD_Code) AS DataCount
                                FROM (SELECT d.ODD_Code
                                          , d.ODD_Name
                                          , d.ODD_Phone
                                      FROM CXT_OriginalDataDts d
                                          JOIN CXT_Customer c ON c.Cust_Phone = d.ODD_Phone
                                      WHERE d.ODD_OD_Code = '{0}' UNION
                                    SELECT d.ODD_Code
                                        , d.ODD_Name
                                        , d.ODD_Phone
                                    FROM CXT_OriginalDataDts d
                                        JOIN CXT_Customer c ON c.Cust_OldName = d.ODD_Name
                                    WHERE d.ODD_OD_Code = '{0}') t", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #endregion

        #region 无效数据

        public bool InvalidData(string FileName, string Type)
        {
            bool flag = false;

            #region 旧代码
            /*
            int result;
            string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            abPath = "D:/hcUpfile/FileUp/";
            flag = new SqlBase().ExcuteNonQuery_Sp("SP_PLDeleteInvalidData", new SqlParameter[] {
                        new SqlParameter("@FileName",abPath+FileName),
                        new SqlParameter("@Type",Type)
              }, out result);

            */
            #endregion

            #region 新逻辑代码

            string LastFileName = FileName.Substring(FileName.LastIndexOf('/') + 1);
            string abPath = AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            try
            {
                if (!File.Exists(abPath + LastFileName))//判断文件存不存在 不存在重新下载
                {
                    using (var client = new System.Net.WebClient())
                    {
                        client.DownloadFile(FileName, HttpContext.Current.Request.PhysicalApplicationPath + "UpFile\\Files\\" + Path.GetFileName(LastFileName));
                    }
                }
                else
                {
                    string custName = string.Empty;
                    string custPhone = string.Empty;
                    using (StreamReader sr = new StreamReader(abPath + LastFileName, Encoding.GetEncoding("gb2312")))
                    {
                        string str;
                        sr.ReadLine();//第一行列头数据
                        while ((str = sr.ReadLine()) != null)
                        {
                            string[] temp = str.Replace("\"", "").Split((char)9);

                            //插入数据到Datatable供bulkinsert
                            //客户名称和号码都存在才导入 否则过滤不导入
                            if (!string.IsNullOrEmpty(temp[0]) && !string.IsNullOrEmpty(temp[1]))
                            {
                                custName += $"'{temp[0]}',";
                                custPhone += $"'{temp[1]}',";
                            }
                        }
                        custName = custName.Length > 0 ? custName.TrimEnd(',') : custName;
                        custPhone = custPhone.Length > 0 ? custPhone.TrimEnd(',') : custPhone;
                    }

                    using (SqlConnection destinationConnection = new SqlConnection(ConfigurationManager.AppSettings["ConnStringSQL"]))
                    {
                        destinationConnection.Open();
                        int excuteResult;
                        using (SqlTransaction transaction = destinationConnection.BeginTransaction())
                        {
                            //先插入主表信息
                            SqlCommand cmd = new SqlCommand($@"DELETE FROM {Type}_OriginalDataDts WHERE ODD_Name IN ({custName});
                                                            DELETE FROM {Type}_OriginalDataDts WHERE ODD_Phone IN ({custPhone});
                                                            DELETE FROM {Type}_OriginalDataValid WHERE ODD_Name IN ({custName});
                                                            DELETE FROM {Type}_OriginalDataValid WHERE ODD_Phone IN ({custPhone});
                                                ", destinationConnection);
                            if (Type.ToLower() == "xfb")
                                cmd.CommandText = $@"DELETE FROM {Type}_OriginalDataDts WHERE ODD_Phone IN ({custPhone});
                                                     DELETE FROM {Type}_OriginalDataValid WHERE ODD_Phone IN ({custPhone});";
                            cmd.Transaction = transaction;
                            excuteResult = cmd.ExecuteNonQuery();
                            if (excuteResult > 0)
                            {
                                transaction.Commit();
                                flag = true;
                            }
                            else
                                transaction.Rollback();
                        }
                    }
                }
            }
            catch
            {
                flag = false;
            }

            #endregion

            return flag;

        }

        #endregion

        #endregion

        #region 民企云

        #region 列表数据

        /// <summary>
        /// 查询原始数据
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <param name="PageIndex">第一页</param>
        /// <param name="PageNum">每页数量</param>
        /// <returns></returns>
        public DataTable GetMQYOriginalData(string Provider, string ProDateStart, string ProDateEnd, string Province, string City, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT OD_Code
                                          , OD_Provider
                                          , CONVERT(varchar(10), OD_ProviderTime, 23) OD_ProviderTime
                                          , OD_ProvinceName + OD_CityName AS AreaName
                                          , OD_TotalCount
                                          , OD_ValidCount
                                          , ISNULL(OD_NoValidCount,0) OD_NoValidCount
                                          , isnull(OD_AlreadyUse,0) OD_AlreadyUse
                                          , OD_State
                                          , ROW_NUMBER () OVER (ORDER BY JoinDate DESC) AS 'Num'
                                      FROM MQY_OriginalData
                                      WHERE DataState = 0", PageNum);

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取查询总数
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <returns></returns>
        public string GetMQYOriginalDataCount(string Provider, string ProDateStart, string ProDateEnd, string Province, string City)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT COUNT(*) AS DataCount FROM MQY_OriginalData WHERE DataState = 0");

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 删除原始数据

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteMQYDatas(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State FROM MQY_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                switch (dt.Rows[0]["OD_State"].ToString())
                {
                    case "已筛选":
                        sb.Append("DELETE  FROM MQY_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM MQY_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM MQY_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                    case "未筛选":
                        sb.Append("DELETE  FROM MQY_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM MQY_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 标记为已使用

        public bool MarkMQYAlreadyUse(string odCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE MQY_OriginalData SET OD_State='已使用' WHERE OD_Code='{0}'", odCode);

            return UpdateData(sb.ToString());
        }

        #endregion

        #region 导入

        /// <summary>
        /// 批量导入 只处理空 - , / 全角
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="AreaCode">地区编码</param>
        /// <param name="CityName">城市名称</param>
        /// <param name="JoinMan">录入人</param>
        /// <returns></returns>
        public bool PlImportMQYOrignData(string FileName, string Provider, string ProviderTime,
                                    string ProvinceCode, string CityCode, string @BillMoney, string JoinMan)
        {
            bool flag = false;
            int result;
            string abPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            abPath = "D:/hcUpfile/FileUp/";
            flag = new SqlBase().ExcuteNonQuery_Sp("SP_PLInsertMQYOrignData", new SqlParameter[] {
                        new SqlParameter("@FileName",abPath+FileName),
                        new SqlParameter("@Provider",ValueHandler.GetStringValue(Provider)),
                        new SqlParameter("@ProviderTime",Convert.ToDateTime(ProviderTime)),
                        new SqlParameter("@ProvinceCode",ValueHandler.GetStringValue(ProvinceCode)),
                        new SqlParameter("@CityCode",ValueHandler.GetStringValue(CityCode)),
                        new SqlParameter("@BillMoney",ValueHandler.GetStringValue(BillMoney)),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <param name="billMoney">计费金额</param>
        /// <param name="provinceCode">省Code</param>
        /// <param name="cityCode">市Code</param>
        /// <returns></returns>
        public bool UpdateMQYDatas(string OD_Code, string billMoney, string provinceCode, string cityCode)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State,OD_CityCode FROM MQY_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                sb.AppendFormat(@"UPDATE MQY_OriginalData
                                    SET OD_ProvinceName = (SELECT SA_Name
                                                           FROM SYS_Area
                                                           WHERE SA_Code = '{0}')
                                    , OD_ProvinceCode = '{0}'
                                    , OD_CityCode = '{1}'
                                    , OD_CityName = (SELECT SA_Name
                                                     FROM SYS_Area
                                                     WHERE SA_Code = '{1}')
                                    , OD_BillMoney = {2}
                                WHERE OD_Code = '{3}';", provinceCode, cityCode, billMoney, OD_Code);

                //地区变了 清空筛选类别 清空有效数据
                if (dt.Rows[0]["OD_CityCode"].ToString() != cityCode)
                {
                    switch (dt.Rows[0]["OD_State"].ToString())
                    {
                        case "已筛选":
                            sb.AppendFormat("UPDATE MQY_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}';", OD_Code);
                            sb.Append("DELETE  FROM MQY_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                            UpdateData(sb.ToString());
                            break;
                        case "未筛选":
                            sb.AppendFormat("UPDATE MQY_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}'", OD_Code);
                            UpdateData(sb.ToString());
                            break;
                    }
                }
                else
                    UpdateData(sb.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <returns></returns>
        public DataTable GetMQYOriginData(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT *,CONVERT(varchar(10), OD_ProviderTime, 23) ProviderTime FROM MQY_OriginalData WHERE DataState = 0 AND OD_Code = '{0}'", OD_Code);

            return SearchData(sb.ToString());
        }

        #endregion

        #region 获取查看数据

        #region 明细数据

        /// <summary>
        /// 明细数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <returns></returns>
        public DataTable GetMQYOriginalDataDts(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT ODD_Name
                                          , ODD_Phone
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM MQY_OriginalDataDts
                                      WHERE DataState = 0 AND ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetMQYOriginalDataDtsCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM MQY_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 有效数据

        public DataTable GetMQYValidData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT *
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM MQY_OriginalDataValid
                                      WHERE ODD_OD_Code = '{1}' AND DataState = 0"
                                , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetMQYValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM MQY_OriginalDataValid WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 无效数据

        public DataTable GetMQYNOVaildData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT ODD_Name
                                    , ODD_Phone
                                    , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                FROM MQY_OriginalDataDts
                                WHERE ODD_OD_Code = '{1}' 
	                                AND DataState= 0
                                    AND ODD_Type >1"
                               , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetMQYNoValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT Count (*) AS DataCount
                                FROM MQY_OriginalDataDts
                                WHERE ODD_OD_Code = '{0}' AND DataState = 0 AND ODD_Type > 1", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #endregion

        #endregion

        #region 维权通

        #region 列表数据

        /// <summary>
        /// 查询原始数据
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <param name="PageIndex">第一页</param>
        /// <param name="PageNum">每页数量</param>
        /// <returns></returns>
        public DataTable GetWQTOriginalData(string Provider, string ProDateStart, string ProDateEnd, string Province, string City, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT OD_Code
                                          , OD_Provider
                                          , CONVERT(varchar(10), OD_ProviderTime, 23) OD_ProviderTime
                                          , OD_ProvinceName + OD_CityName AS AreaName
                                          , OD_TotalCount
                                          , OD_ValidCount
                                          , ISNULL(OD_NoValidCount,0) OD_NoValidCount
                                          , isnull(OD_AlreadyUse,0) OD_AlreadyUse
                                          , OD_State
                                          , ROW_NUMBER () OVER (ORDER BY JoinDate DESC) AS 'Num'
                                      FROM WQT_OriginalData
                                      WHERE DataState = 0", PageNum);

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 获取查询总数
        /// </summary>
        /// <param name="Provider">提供方</param>
        /// <param name="ProDateStart">提供时间起</param>
        /// <param name="ProDateEnd">提供时间止</param>
        /// <param name="Province">省CODE</param>
        /// <param name="City">市CODE</param>
        /// <returns></returns>
        public string GetWQTOriginalDataCount(string Provider, string ProDateStart, string ProDateEnd, string Province, string City)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT COUNT(*) AS DataCount FROM WQT_OriginalData WHERE DataState = 0");

            if (ValueHandler.GetStringValue(Provider) != "")
            {
                sb.Append(" AND OD_Provider LIKE '%" + ValueHandler.GetStringValue(Provider) + "%'");
            }
            if (ValueHandler.GetStringValue(Province) != "")
            {
                sb.Append(" AND OD_ProvinceCode='" + ValueHandler.GetStringValue(Province) + "'");
            }
            if (ValueHandler.GetStringValue(City) != "")
            {
                sb.Append(" AND OD_CityCode ='" + ValueHandler.GetStringValue(City) + "'");
            }
            if (ValueHandler.GetStringValue(ProDateStart) != "")
            {
                sb.Append(" AND OD_ProviderTime  >= " + ValueHandler.GetMarkStringDateValue(ProDateStart));
            }
            if (ValueHandler.GetStringValue(ProDateEnd) != "")
            {
                sb.Append(" AND OD_ProviderTime  <= " + ValueHandler.GetMarkStringDateValue(ProDateEnd));
            }

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 删除原始数据

        /// <summary>
        /// 删除原始数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteWQTDatas(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State FROM WQT_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                switch (dt.Rows[0]["OD_State"].ToString())
                {
                    case "已筛选":
                        sb.Append("DELETE  FROM WQT_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM WQT_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM WQT_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                    case "未筛选":
                        sb.Append("DELETE  FROM WQT_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                        sb.Append("DELETE  FROM WQT_OriginalDataDts WHERE ODD_OD_Code = '" + OD_Code + "'");
                        UpdateData(sb.ToString());
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 标记为已使用

        public bool MarkWQTAlreadyUse(string odCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE WQT_OriginalData SET OD_State='已使用' WHERE OD_Code='{0}'", odCode);

            return UpdateData(sb.ToString());
        }

        #endregion

        #region 导入

        /// <summary>
        /// 批量导入 只处理空 - , / 全角
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="AreaCode">地区编码</param>
        /// <param name="CityName">城市名称</param>
        /// <param name="JoinMan">录入人</param>
        /// <returns></returns>
        public bool PlImportWQTOrignData(string FileName, string Provider, string ProviderTime,
                                    string ProvinceCode, string CityCode, string @BillMoney, string JoinMan)
        {
            bool flag = false;
            int result;
            string abPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\UpFile\\Files\\";
            abPath = "D:/hcUpfile/FileUp/";
            flag = new SqlBase().ExcuteNonQuery_Sp("SP_PLInsertWQTOrignData", new SqlParameter[] {
                        new SqlParameter("@FileName",abPath+FileName),
                        new SqlParameter("@Provider",ValueHandler.GetStringValue(Provider)),
                        new SqlParameter("@ProviderTime",Convert.ToDateTime(ProviderTime)),
                        new SqlParameter("@ProvinceCode",ValueHandler.GetStringValue(ProvinceCode)),
                        new SqlParameter("@CityCode",ValueHandler.GetStringValue(CityCode)),
                        new SqlParameter("@BillMoney",ValueHandler.GetStringValue(BillMoney)),
                        new SqlParameter("@JoinMan",ValueHandler.GetStringValue(JoinMan))
              }, out result);
            return flag;
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <param name="billMoney">计费金额</param>
        /// <param name="provinceCode">省Code</param>
        /// <param name="cityCode">市Code</param>
        /// <returns></returns>
        public bool UpdateWQTDatas(string OD_Code, string billMoney, string provinceCode, string cityCode)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            try
            {
                sb.Append("SELECT OD_State,OD_CityCode FROM WQT_OriginalData WHERE OD_Code = '" + OD_Code + "'");
                dt = SearchData(sb.ToString());

                sb.Remove(0, sb.Length);
                sb.AppendFormat(@"UPDATE WQT_OriginalData
                                    SET OD_ProvinceName = (SELECT SA_Name
                                                           FROM SYS_Area
                                                           WHERE SA_Code = '{0}')
                                    , OD_ProvinceCode = '{0}'
                                    , OD_CityCode = '{1}'
                                    , OD_CityName = (SELECT SA_Name
                                                     FROM SYS_Area
                                                     WHERE SA_Code = '{1}')
                                    , OD_BillMoney = {2}
                                WHERE OD_Code = '{3}';", provinceCode, cityCode, billMoney, OD_Code);

                //地区变了 清空筛选类别 清空有效数据
                if (dt.Rows[0]["OD_CityCode"].ToString() != cityCode)
                {
                    switch (dt.Rows[0]["OD_State"].ToString())
                    {
                        case "已筛选":
                            sb.AppendFormat("UPDATE WQT_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}';", OD_Code);
                            sb.Append("DELETE  FROM WQT_OriginalDataValid WHERE ODD_OD_Code = '" + OD_Code + "'");
                            UpdateData(sb.ToString());
                            break;
                        case "未筛选":
                            sb.AppendFormat("UPDATE WQT_OriginalDataDts SET ODD_Type= NULL WHERE ODD_OD_Code = '{0}'", OD_Code);
                            UpdateData(sb.ToString());
                            break;
                    }
                }
                else
                    UpdateData(sb.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="OD_Code">编码</param>
        /// <returns></returns>
        public DataTable GetWQTOriginData(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT *,CONVERT(varchar(10), OD_ProviderTime, 23) ProviderTime FROM WQT_OriginalData WHERE DataState = 0 AND OD_Code = '{0}'", OD_Code);

            return SearchData(sb.ToString());
        }

        #endregion

        #region 获取查看数据

        #region 明细数据

        /// <summary>
        /// 明细数据
        /// </summary>
        /// <param name="OD_Code">原始数据Code</param>
        /// <returns></returns>
        public DataTable GetWQTOriginalDataDts(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} o. *
                                FROM (SELECT ODD_Name
                                          , ODD_Phone
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM WQT_OriginalDataDts
                                      WHERE DataState = 0 AND ODD_OD_Code = '{1}'", PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetWQTOriginalDataDtsCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM WQT_OriginalDataDts WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 有效数据

        public DataTable GetWQTValidData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT *
                                          , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                      FROM WQT_OriginalDataValid
                                      WHERE ODD_OD_Code = '{1}' AND DataState = 0"
                                , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetWQTValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT COUNT(*) AS DataCount FROM WQT_OriginalDataValid WHERE DataState = 0 AND ODD_OD_Code='{0}'", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region 无效数据

        public DataTable GetWQTNOVaildData(string OD_Code, string PageIndex, string PageNum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT TOP {0} *
                                FROM (SELECT ODD_Name
                                    , ODD_Phone
                                    , ROW_NUMBER () OVER (ORDER BY ODD_Name) AS 'Num'
                                FROM WQT_OriginalDataDts
                                WHERE ODD_OD_Code = '{1}' 
	                                AND DataState= 0
                                    AND ODD_Type >1"
                               , PageNum, OD_Code);

            sb.AppendFormat(" ) o WHERE o.Num > ({0}-1)*{1}", PageIndex, PageNum);
            return SearchData(sb.ToString());
        }

        public string GetWQTNoValidDataCount(string OD_Code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT Count (*) AS DataCount
                                FROM WQT_OriginalDataDts
                                WHERE ODD_OD_Code = '{0}' AND DataState = 0 AND ODD_Type > 1", OD_Code);

            DataTable dt = SearchData(sb.ToString());
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["DataCount"].ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #endregion

        #endregion

        #region 导出判断

        /// <summary>
        /// 检查导出是否有数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool CheckExportData(string sql)
        {
            DataTable dt = SearchData(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public string GetFileName(string code)
        {
            DataTable dt = SearchData($"SELECT CD_FileName from KF_ChargeData where CD_Code='{code}'");

            return dt != null ? dt.Rows[0][0].ToString() : "";
        }

        #endregion
    }
}
