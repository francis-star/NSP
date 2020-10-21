using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HCWeb2016;
using System.Data;
using BLL;
using System.Configuration;

namespace Web.Ashx
{
    /// <summary>
    /// GetPublicData 的摘要说明
    /// 获取资讯信息（真分页）
    /// </summary>
    public class GetPublicData : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        BLL_PublicInfo bLL_PublicInfo = new BLL_PublicInfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string postString = string.Empty;
            string Data = context.Request["Data"].ToString();
            postString = System.Web.HttpUtility.UrlDecode(Data);
            //替换部分SQL关键字，预防SQL注入
            string SQLReplace = ConfigurationManager.AppSettings["SqlReplace"].ToString();
            string[] SQLReplaces = SQLReplace.Split(',');
            foreach (string item in SQLReplaces)
            {
                postString = postString.Replace(item, "");
            }

            string[] strArr = postString.Split('臡');

            String methodName = strArr[0];//方法名
            String BLL = strArr[1];
            object Para = strArr[2];
            try
            {
                BeforeInvoke(BLL);
                string json = BLL_PubClass.PubMethod(methodName, BLL, Para);
                context.Response.Write(json);
            }
            catch (Exception ex)
            {
                String json = JSON.Encode(ex.Message);
                context.Response.Write(json);
            }
            finally
            {
                AfterInvoke();
            }
        }

        //权限管理
        protected void BeforeInvoke(string BLLName)
        {

        }
        //日志管理
        protected void AfterInvoke()
        {

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