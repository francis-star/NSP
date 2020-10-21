using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

namespace JQueryUpLoad
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UploadHandler : IHttpHandler
    {
        Web.ServiceUpFile.UpFileWeb upFileWeb = new Web.ServiceUpFile.UpFileWeb();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            HttpPostedFile oFile = context.Request.Files["Filedata"];
            if (oFile != null)
            {

                //获得原文件名（含扩展名）
                string localFileName = System.IO.Path.GetFileName(oFile.FileName);
                //获取原文件扩展名
                string localFileExtension = getFileExtension(localFileName);

                byte[] bytes = null;
                using (var binaryReader = new BinaryReader(oFile.InputStream))
                {
                    bytes = binaryReader.ReadBytes(oFile.ContentLength);
                }
                string bytes64Str = Convert.ToBase64String(bytes);

                string Url = upFileWeb.upfilebyte(bytes64Str, localFileExtension);

                context.Response.Write(Url);
            }
            else
            {
                context.Response.Write("false");
            }
        }


        public string GetRootWebPath()
        {
            // 是否为SSL认证站点
            string secure = HttpContext.Current.Request.ServerVariables["HTTPS"];
            string httpProtocol = (secure == "on" ? "https://" : "http://");
            // 服务器名称
            string serverName = HttpContext.Current.Request.ServerVariables["Server_Name"];
            string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            // 应用服务名称
            string applicationName = HttpContext.Current.Request.ApplicationPath;
            return httpProtocol + serverName + (port.Length > 0 ? ":" + port : string.Empty) + applicationName;
        }

        #region getSaveFileFolderPath获得保存的文件夹的物理路径
        /// <summary>
        /// 获得保存的文件夹的物理路径
        /// 返回保存的文件夹的物理路径,若为null则表示出错
        /// </summary>
        /// <param name="format">保存的文件夹路径 或者 格式化方式创建保存文件的文件夹，如按日期"yyyy"+"MM"+"dd":20060511</param>
        /// <returns>保存的文件夹的物理路径,若为null则表示出错</returns>
        private string getSaveFileFolderPath(string format)
        {
            string mySaveFolder = null;
            try
            {
                string folderPath = null;
                //以当前时间创建文件夹,
                //!!!!!!!!!!!!以后用正则表达式替换下面的验证语句!!!!!!!!!!!!!!!!!!!
                if (format.IndexOf("yyyy") > -1 || format.IndexOf("MM") > -1 || format.IndexOf("dd") > -1 || format.IndexOf("hh") > -1 || format.IndexOf("mm") > -1 || format.IndexOf("ss") > -1 || format.IndexOf("ff") > -1)
                {
                    //以通用标准时间创建文件夹的名字
                    folderPath = DateTime.UtcNow.ToString(format);
                    mySaveFolder = @"" + folderPath + @"";
                }
                else
                {
                    mySaveFolder = format;
                }
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(mySaveFolder);
                //判断文件夹否存在,不存在则创建
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch
            {
            }
            return mySaveFolder;
        }

        #endregion

        #region getFileExtension(...):获取原文件的扩展名
        /// <summary>
        /// 获取原文件的扩展名,返回原文件的扩展名(localFileExtension),该函数用到外部变量fileType,即允许的文件扩展名.
        /// </summary>
        /// <param name="myFileName">原文件名</param>
        /// <returns>原文件的扩展名(localFileExtension);若返回为null,表明文件无后缀名;若返回为"",则表明扩展名为非法.</returns>
        private string getFileExtension(string myFileName)
        {
            string myFileExtension = null;
            //获得文件扩展名
            myFileExtension = System.IO.Path.GetExtension(myFileName);//若为null,表明文件无后缀名;
            //分解允许上传文件的格式
            if (myFileExtension != "")
            {
                myFileExtension = myFileExtension.ToLower();//转化为小写
            }
            return myFileExtension;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
