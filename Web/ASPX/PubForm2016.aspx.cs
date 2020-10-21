using BLL;
using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.ASPX
{
    public partial class PubForm2016 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //接收消息指令
            string postString = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                    if (postString == "") { Response.End(); };
                }
            }
            postString = System.Web.HttpUtility.UrlDecode(postString);

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
                Response.Write(json);
            }
            catch (Exception ex)
            {
                String json = JSON.Encode(ex.Message);
                Response.Write(json);
            }
            finally
            {
                AfterInvoke();
            }
        }

        //权限管理
        protected void BeforeInvoke(string BLLName)
        {
            string state = ConfigurationManager.AppSettings["DebugState"];
            //发布状态
            if (state == "1")
            {

                //如果没有Session，则为非法路径，直接返回登陆页面
                if (BLL_User.User_Code == "" && BLLName != "BLL_Login")
                    throw new Exception("Error");
            }
        }
        //日志管理
        protected void AfterInvoke()
        {

        }
    }
}