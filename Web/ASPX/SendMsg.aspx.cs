using HCWeb2016;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.ASPX
{
    public partial class SendMsg : System.Web.UI.Page
    {
        ////签名 消法顾问
        //SendMessage.QGMessage sMSWebServiceSoapClient = new SendMessage.QGMessage();
        ////签名 E通315
        //SendMessageET.frm_SendMessageQG sMSWebServiceSoapClientET = new SendMessageET.frm_SendMessageQG();
        //签名 民企云
        //SendMessageMQY.MQYMessage sMSWebServiceSoapClientMQY = new SendMessageMQY.MQYMessage();

        NewSendMsg.NewSendMsg NewSendMsg = new NewSendMsg.NewSendMsg();
        string APPID = "4YPRDOCBR6";
        string APPSECRET = "92567e441c347d7896f0e158ca1d3502c593b73f";
        protected void Page_Load(object sender, EventArgs e)
        {
            String methodName = Request["method"];
            Type type = this.GetType();
            MethodInfo method = type.GetMethod(methodName); 

            if (method == null) throw new Exception("method is null"); 

            try
            {
                string json = "";
                json = (string)method.Invoke(this, null);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                Hashtable result = new Hashtable();
                result["error"] = -1;
                result["message"] = ex.Message;
                result["stackTrace"] = ex.StackTrace;
                String json = JSON.Encode(result);
                Response.Clear();
                Response.Write(json);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <returns></returns>
        public string SendMessages()
        {
            string Message = ValueHandler.GetStringValue(Request["Message"]);
            string Phone = ValueHandler.GetStringValue(Request["Phone"]);
            string timestamp = DateTimeToUnixTimestamp(DateTime.Now).ToString();
            string strAscII = AscIISort(APPID, APPSECRET, timestamp.ToString());
            string SIGNATURE = SHA1_Hash(strAscII);
            WriteLog(Phone + ":datatime:" + DateTime.Now + ":SendMessages:" + Message);
            string ReValue = NewSendMsg.SendMsg_Budiot_XFGWNormal(Phone, Message).ToLower();
            if (ReValue == "true") return "true";
            else return "false";
        }

        /// <summary>
        /// 直接发短信接口
        /// </summary>
        /// <returns></returns>
        public string SendMsgs()
        {
            string Message = ValueHandler.GetStringValue(Request["Message"]);
            string Phone = ValueHandler.GetStringValue(Request["Phone"]);
            string timestamp = DateTimeToUnixTimestamp(DateTime.Now).ToString();
            string strAscII = AscIISort(APPID, APPSECRET, timestamp.ToString());
            string SIGNATURE = SHA1_Hash(strAscII);
            WriteLog(Phone + ":datatime:" + DateTime.Now + ":SendMsgs:" + Message);
            string ReValue = NewSendMsg.SendMsg_Budiot_XFGWNormal(Phone, Message).ToLower(); 
            if (ReValue == "true") return "true";
            else return "false";
        }

        /// <summary>
        /// 直接发短信接口 E通
        /// </summary>
        /// <returns></returns>
        public string SendMsgsET()
        {
            string Message = ValueHandler.GetStringValue(Request["Message"]);
            string Phone = ValueHandler.GetStringValue(Request["Phone"]);
            string timestamp = DateTimeToUnixTimestamp(DateTime.Now).ToString();
            string strAscII = AscIISort(APPID, APPSECRET, timestamp.ToString());
            string SIGNATURE = SHA1_Hash(strAscII);
            WriteLog(Phone + ":datatime:" + DateTime.Now + ":SendMsgsET:" + Message);
            string ReValue = NewSendMsg.SendMsg_Budiot_ET315WNormal(Phone, Message).ToLower();
            if (ReValue == "true") return "true";
            else return "false";
        }

        /// <summary>
        /// 直接发短信接口 民企云
        /// </summary>
        /// <returns></returns>
        public string SendMsgsMQY()
        {
            string Message = ValueHandler.GetStringValue(Request["Message"]);
            string Phone = ValueHandler.GetStringValue(Request["Phone"]); 
            WriteLog(Phone + ":datatime:" + DateTime.Now + ":SendMsgsMQY:" + Message);
            string ReValue = NewSendMsg.SendMsg_Budiot_MQYNormal(Phone, Message).ToLower(); 
            if (ReValue == "true") return "true";
            else return "false";
        }

        /// <summary>
        /// 直接发短信接口 新消费宝典
        /// </summary>
        /// <returns></returns>
        public string SendMsgsXFB()
        {
            string Message = HttpUtility.UrlDecode(Request["Message"], System.Text.Encoding.UTF8);
            string Phone = ValueHandler.GetStringValue(Request["Phone"]); 
            WriteLog(Phone + ":datatime:" + DateTime.Now + ":SendMsgsXFB:" + Message);
            string ReValue = NewSendMsg.SendMsg_Budiot_CXTSNormal(Phone, Message).ToLower(); 
            if (ReValue == "true") return "true";
            else return "false";
        } 

        private string AscIISort(string str1, string str2, string str3)
        {
            string a = "";
            List<string> list = new List<string>();
            list.Add(str1);
            list.Add(str2);
            list.Add(str3);
            list.Sort();

            for (int i = 0; i < list.Count; i++)
            {
                a += list[i];
            }
            return a;
        } 

        //SHA1
        public string SHA1_Hash(string str_sha1_in)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(str_sha1_in);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }

        /// <summary>
        /// 日志记录（出错或者成功时）
        /// </summary>
        /// <param name="strMemo"></param>
        public  void WriteLog(string strMemo)
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + "log.txt", FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fs);
            streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            streamWriter.WriteLine(strMemo);
            streamWriter.Flush();
            fs.Close();
        }
    }
}