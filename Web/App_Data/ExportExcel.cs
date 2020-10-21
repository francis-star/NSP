using System.Collections;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using System.Text;
using System;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;

public class ExportExcel
{
    public static HSSFWorkbook Export(DataTable dt, string[] headerList, string[] headercode)
    {
        //创建Excel文件的对象
        NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
        //添加一个sheet
        NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
        ICellStyle style = book.CreateCellStyle();
        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.WrapText = true;
        style.FillPattern = FillPattern.SolidForeground;
        style.FillForegroundColor = HSSFColor.Grey25Percent.Index;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.BorderBottom = BorderStyle.Double;
        style.BorderLeft = BorderStyle.Double;
        style.BorderRight = BorderStyle.Double;
        style.BorderTop = BorderStyle.Double;
        style.BottomBorderColor = HSSFColor.Grey50Percent.Index;
        style.LeftBorderColor = HSSFColor.Grey50Percent.Index;
        style.RightBorderColor = HSSFColor.Grey50Percent.Index;
        style.TopBorderColor = HSSFColor.Grey50Percent.Index;
        IFont font = book.CreateFont();
        font.FontHeightInPoints = 14;
        font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
        font.FontName = "標楷體";
        font.Color = HSSFColor.Black.Index;
        style.SetFont(font);//HEAD 样式

        //貌似这里可以设置各种样式字体颜色背景等，但是不是很方便，这里就不设置了

        //给sheet1添加第一行的头部标题
        NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
        row1.Height = 80 * 5;
        ICellStyle cellstyle = book.CreateCellStyle();
        cellstyle.Alignment = HorizontalAlignment.Left;
        cellstyle.VerticalAlignment = VerticalAlignment.Top;
        cellstyle.WrapText = true;
        for (int i = 0; i < headerList.Length; i++)
        {
            row1.CreateCell(i).SetCellValue(headerList[i]);
            row1.GetCell(i).CellStyle = style;
            if (i == 0)
                sheet1.SetColumnWidth(i, 22 * 256);
            if (i > 0)
            {
                sheet1.SetColumnWidth(i, 30 * 256);
            }
        }
        //定义第二个工作簿防止内容溢出报错问题
        NPOI.SS.UserModel.ISheet sheet2 = null;
        if (dt.Rows.Count > 65535)
        {
            sheet2 = book.CreateSheet("Sheet2");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row11 = sheet2.CreateRow(0);
            row11.Height = 80 * 5;
            ICellStyle cellstyle1 = book.CreateCellStyle();
            cellstyle1.Alignment = HorizontalAlignment.Left;
            cellstyle1.VerticalAlignment = VerticalAlignment.Top;
            cellstyle1.WrapText = true;
            for (int i = 0; i < headerList.Length; i++)
            {
                row11.CreateCell(i).SetCellValue(headerList[i]);
                row11.GetCell(i).CellStyle = style;
                if (i == 0)
                    sheet2.SetColumnWidth(i, 22 * 256);
                if (i > 0)
                {
                    sheet2.SetColumnWidth(i, 30 * 256);
                }
            }
        }
        int k = 0;
        //将数据逐步写入sheet1各个行
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (i > 65534)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet2.CreateRow(k);
                rowtemp.HeightInPoints = 65;
                for (int j = 0; j < headerList.Length; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][headercode[j]].ToString());
                    rowtemp.GetCell(j).CellStyle = cellstyle;
                }
                k++;
            }
            else
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.HeightInPoints = 65;
                for (int j = 0; j < headerList.Length; j++)
                {

                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][headercode[j]].ToString());
                    rowtemp.GetCell(j).CellStyle = cellstyle;
                }
            }

        }
        return book;

    }


    public static HSSFWorkbook Export<T>(List<T> Objs, string[] headerList)
    {
        //创建Excel文件的对象
        NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
        //添加一个sheet
        NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

        ICellStyle style = book.CreateCellStyle();
        style.Alignment = HorizontalAlignment.Center;
        style.WrapText = true;
        style.FillPattern = FillPattern.SolidForeground;
        style.FillForegroundColor = HSSFColor.Grey25Percent.Index;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.BorderBottom = BorderStyle.Double;
        style.BorderLeft = BorderStyle.Double;
        style.BorderRight = BorderStyle.Double;
        style.BorderTop = BorderStyle.Double;
        style.BottomBorderColor = HSSFColor.Grey50Percent.Index;
        style.LeftBorderColor = HSSFColor.Grey50Percent.Index;
        style.RightBorderColor = HSSFColor.Grey50Percent.Index;
        style.TopBorderColor = HSSFColor.Grey50Percent.Index;
        IFont font = book.CreateFont();
        font.FontHeightInPoints = 14;
        font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
        font.FontName = "標楷體";
        font.Color = HSSFColor.White.Index;
        style.SetFont(font);//HEAD 样式
        //row.CreateCell(0).SetCellValue(getString(syscomment, "Company") + "\n" + getString(syscomment, "Report"));

        //貌似这里可以设置各种样式字体颜色背景等，但是不是很方便，这里就不设置了

        //给sheet1添加第一行的头部标题
        NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
        row1.Height = 80 * 5;
        for (int i = 0; i < headerList.Length; i++)
        {


            row1.CreateCell(i).SetCellValue(headerList[i]);
            row1.GetCell(i).CellStyle = style;
            if (i == 0)
                sheet1.SetColumnWidth(i, 22 * 256);
            if (i > 0)
                sheet1.SetColumnWidth(i, 30 * 256);
        }
        int m = 0;
        //将数据逐步写入sheet1各个行
        foreach (T model in Objs)
        {

            NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(m + 1);
            int j = 0;
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                rowtemp.CreateCell(j).SetCellValue(double.Parse(propertyInfo.GetValue(model, null).ToString()));
                j++;
            }
            m++;
        }
        return book;
    }

}

