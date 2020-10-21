//////////////////////////////////////////////////////////////////////////////
//模块名：用户信息Session类
//开发者：柳青
//开发时间：2014年8月8日
//////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HCWeb2016;

namespace BLL
{
    public static class BLL_User
    {
        public static void SetUseInfo(DataTable dt)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                SessionHelper.SetSession(dt.Columns[i].ColumnName, ValueHandler.GetStringValue(dt.Rows[0][i]));
            }
        }

        /// <summary>
        /// 用户编码
        /// </summary>
        public static string User_Code
        {
            get { return ValueHandler.GetStringValue(SessionHelper.GetSession("User_Code")); }
        }

        /// <summary>
        /// 登录名
        /// </summary>
        public static string User_LoginName
        {
            get { return ValueHandler.GetStringValue(SessionHelper.GetSession("User_LoginName")); }
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public static string User_Name
        {
            get { return ValueHandler.GetStringValue(SessionHelper.GetSession("User_Name")); }
        }

        /// <summary>
        /// 性别
        /// </summary>
        public static string User_Sex
        {
            get { return ValueHandler.GetStringValue(SessionHelper.GetSession("User_Sex")); }
        }

        /// <summary>
        /// 移动电话
        /// </summary>
        public static string User_Phone
        {
            get { return ValueHandler.GetStringValue(SessionHelper.GetSession("User_Phone")); }
        }


        public struct CityUrl
        {
            /// <summary>
            /// 江苏省
            /// </summary>
            public static string 江苏省 = "www.js96315.com";
            /// <summary>
            /// 南京市
            /// </summary>
            public static string 南京市 = "nj.js96315.com";
            /// <summary>
            /// 无锡市
            /// </summary>
            public static string 无锡市 = "wx.js96315.com";
            /// <summary>
            /// 徐州市
            /// </summary>
            public static string 徐州市 = "www.wqkstd.com";
            /// <summary>
            /// 常州市
            /// </summary>
            public static string 常州市 = "cz.js96315.com";
            /// <summary>
            /// 苏州市
            /// </summary>
            public static string 苏州市 = "sz.js96315.com";
            /// <summary>
            /// 南通市
            /// </summary>
            public static string 南通市 = "nt.js96315.com";
            /// <summary>
            /// 连云港市
            /// </summary>
            public static string 连云港市 = "lyg.js96315.com";
            /// <summary>
            /// 淮安市
            /// </summary>
            public static string 淮安市 = "ha.js96315.com";
            /// <summary>
            /// 盐城市
            /// </summary>
            public static string 盐城市 = "yc.js96315.com";
            /// <summary>
            /// 扬州市
            /// </summary>
            public static string 扬州市 = "yz.js96315.com";
            /// <summary>
            /// 镇江市
            /// </summary>
            public static string 镇江市 = "zj.js96315.com";
            /// <summary>
            /// 泰州市
            /// </summary>
            public static string 泰州市 = "tz.js96315.com";
            /// <summary>
            /// 宿迁市
            /// </summary>
            public static string 宿迁市 = "sq.js96315.com";
            /// <summary>
            /// 苏州园区
            /// </summary>
            public static string 苏州园区 = "szyq.js96315.com";
        }
    }


}
