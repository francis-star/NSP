////////////////////////////
//模块名：数据链接核心类
//开发者：柳青
//开发时间：2016年6月16日
/////////////////////////////
using HCWeb2016;
using System; 
using System.IO; 
using System.Reflection; 

namespace BLL
{
    public static class BLL_PubClass
    {
        public static string PubMethod(String methodName, String BLLName, object Para)
        {
            string json = "";
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                object obj = assembly.CreateInstance("BLL." + BLLName); // 创建类的实例，返回为 object 类
                object[] args = new object[] { Para };
                // 根据类型名得到Type
                Type type = assembly.GetType("BLL." + BLLName);
                MethodInfo mi = type.GetMethod(methodName);

                if (ValueHandler.GetStringValue(args[0]) == "")
                    json = (string)mi.Invoke(obj, null);//无参数传值时触发
                else
                    json = (string)mi.Invoke(obj, args);//参数传值时触发

            }
            catch (TargetInvocationException targetEx)
            {
                if (targetEx.InnerException != null)
                {
                    //获取错误实例
                    WriteLog(targetEx.InnerException.ToString());
                    return "Error";
                }
            }
            return json;
        }

        /// <summary>
        /// 日志记录（出错或者成功时）
        /// </summary>
        /// <param name="strMemo"></param>
        public static void WriteLog(string strMemo)
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
