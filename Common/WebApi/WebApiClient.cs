//*******************************************************************
//文件名（File Name）：           WebApiClient
//功能描述（Description）：       用于调用投诉管理平台API接口
//数据表（Table）：               IP:211.149.239.28下的[Sys_Company]、[[Sys_Users]]
//作者（Author）：                陈世盛
//日期（Create Date）：           2018.04.08
//*******************************************************************
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Web.WebApi
{
    public static class WebApiClient
    {
        /// <summary>
        /// 异步调用
        /// </summary>
        /// <param name="json">传递json字符串</param>
        public static async void dooPost(string json)
        {
            string url = ConfigurationManager.AppSettings["WebApiUrl"].ToString();
            //设置HttpClientHandler的AutomaticDecompression
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            //创建HttpClient（注意传入HttpClientHandler）
            using (var http = new HttpClient(handler))
            {
                //使用FormUrlEncodedContent做HttpContent
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                  {"json", json}//键名必须为空
                 });

                //await异步等待回应
                var response = await http.PostAsync(url, content);
                //确保HTTP成功状态值
                response.EnsureSuccessStatusCode();
                //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                string result = await response.Content.ReadAsStringAsync();
            }

        }
        /// <summary>
        /// 同步方法调用
        /// </summary>
        /// <param name="json">传递接送字符串</param>
        /// <returns></returns>
        public static bool RequestQGWQTZC(string json)
        {

            string url = ConfigurationManager.AppSettings["WebApiUrl"].ToString() + "?json=" + json;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            if (retString != "success")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}