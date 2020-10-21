using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HCWeb2016;

namespace Web.ASPX
{
    public partial class ImportHelper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void exportexcel()
        {
            string strSql = "";
            string FileName = "";
            //列表数据源
            if (Session["FileData"] != null)
                strSql = Session["FileData"].ToString();
            //文件名称
            if (Session["FileName"] != null)
                FileName = Session["FileName"].ToString();
            if (Session["SSBType"] == null) 
                FileName = FileName + "-" + DateTime.Now.ToString("yyyyMMdd");
            //根据sql读取数据源
            System.Data.DataTable dt = new HCWeb2016.SqlBase().SearchData(strSql);
            ExportDataToExcelByWeb(dt, FileName);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            exportexcel();
        }
        public void ExportDataToExcelByWeb(System.Data.DataTable dt, string FileName)
        {
            string[] headerList = new string[dt.Columns.Count];
            string[] headerCode = new string[dt.Columns.Count];//对应表头的字段
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                headerList.SetValue(dt.Columns[i].ColumnName, i);
                headerCode.SetValue(dt.Columns[i].ColumnName, i);
            }
            //如果是linq查询的列表，需要转datatable
            NPOI.HSSF.UserModel.HSSFWorkbook book = ExportExcel.Export(dt, headerList, headerCode);
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)//火狐浏览器
                FileName = HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);

            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileName));
            Response.ContentType = "application/vnd.ms-excel";

            Response.BinaryWrite(ms.ToArray());
            Response.End();
            book = null;
            ms.Close();
            ms.Dispose();

        }
        static void DGOutPut_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                    e.Row.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }

        public void ExportDataToCSV(System.Data.DataTable dt, string FileName, int[] indexs)
        {
            if (dt == null)
                return;
            if (dt.Rows.Count == 0)
            {
                return;
            }
            try
            {
                System.Web.UI.WebControls.GridView dgExport = new System.Web.UI.WebControls.GridView();
                dgExport.AllowPaging = false;
                //dgExport.CellPadding = 2;
                //dgExport.RowDataBound += new GridViewRowEventHandler(dgExport_RowDataBound);
                dgExport.DataSource = dt;
                dgExport.DataMember = dt.TableName;
                dgExport.DataBind();
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
                System.Web.HttpContext.Current.Response.Charset = "";
                //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName);
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName + ".csv"));
                string strText = "";
                string strRowDelim = System.Environment.NewLine;
                string strColDelim = ",";
                System.Web.HttpContext.Current.Response.ContentType = "text/txt";
                // System.Web.HttpContext.Current.Response.ContentType = "application/unkown";
                System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
                System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
                //System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
                System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
                dgExport.RenderControl(oHtmlTextWriter);
                strText = oStringWriter.ToString();
                strText = ParseToDelim(strText, strRowDelim, strColDelim);
                System.Web.HttpContext.Current.Response.Write(strText);
                System.Web.HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void dgExport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                    e.Row.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }

        public string ParseToDelim(string strText, string strRowDelim, string strColDelim)
        {
            Regex objReg = new Regex(@"(>\s+<)", RegexOptions.IgnoreCase);
            strText = objReg.Replace(strText, "><");
            strText = strText.Replace("&nbsp;", "");
            strText = strText.Replace(System.Environment.NewLine, "");
            //strText = strText.Replace("</th></tr><tr><th scope=\"col\">", strRowDelim);
            strText = strText.Replace("<th scope=\"col\">", "<td>");
            strText = strText.Replace("</th>", "</td>");
            strText = strText.Replace("</td></tr><tr><td>", strRowDelim);
            strText = strText.Replace("</td><td>", strColDelim);
            strText = strText.Replace("</th><th>", strColDelim);
            objReg = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            strText = objReg.Replace(strText, "");
            strText = System.Web.HttpUtility.HtmlDecode(strText);
            return strText;
        }
    }
}