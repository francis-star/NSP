using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Web.Ashx
{
    /// <summary>
    /// TreeData 的摘要说明
    /// </summary>
    public class TreeData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string code = context.Request["code"].ToString();
            DAL.DAL_OriginalDataDts dal = new DAL.DAL_OriginalDataDts();
            DataTable dt = dal.GetTotalCount(code);
            string json = string.Empty;
            json += "[{ text: '有效数据" + (int.Parse(dt.Rows[0]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[0]["count"].ToString() + ")" )+ "',id:1,pid;0},";
            json += "{ id:21,pid;0,text: '可移动' , children: [";
            json += "{ text: '已呼过" + (int.Parse(dt.Rows[3]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[3]["count"].ToString() + ")") + "',id:4 },";
            json += "{ text: '其他业务退订/退费" + (int.Parse(dt.Rows[9]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[9]["count"].ToString() + ")") + "',id:10 },";
            json += "{ text: '关键字：低危" + (int.Parse(dt.Rows[6]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[6]["count"].ToString() + ")") + "',id:7 },";
            json += "{ text: '无效" + (int.Parse(dt.Rows[10]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[10]["count"].ToString() + ")") + "',id:'11' },";
            json += "{ text: '名称相同退订/退费" + (int.Parse(dt.Rows[11]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[11]["count"].ToString() + ")") + "',id:12}";
            json += "]},";

            json += "{ id:22,pid;0,text: '不可移动' , children: [";
            json += "{ text: '重复数据" + (int.Parse(dt.Rows[4]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[4]["count"].ToString() + ")") + "',id:5 },";
            json += "{ text: '黑名单" + (int.Parse(dt.Rows[5]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[5]["count"].ToString() + ")") + "',id:6 }, ";
            json += "{ text: '关键字：高危" + (int.Parse(dt.Rows[8]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[8]["count"].ToString() + ")") + "',id:9},";
            json += "{ text: '本业务会员" + (int.Parse(dt.Rows[1]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[1]["count"].ToString() + ")") + "',id:2 },";
            json += "{ text: '其他业务会员" + (int.Parse(dt.Rows[7]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[7]["count"].ToString() + ")") + "',id:8 }, ";
            json += "{ text: '本业务退订/退费" + (int.Parse(dt.Rows[2]["count"].ToString()) > 0 ? "" : "(" + dt.Rows[2]["count"].ToString() + ")") + "',id:3 }";

            json += "]}]";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}