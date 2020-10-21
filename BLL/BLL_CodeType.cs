using DAL;
using HCWeb2016;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_CodeType
    {
        DAL_CodeType dAL_CodeType = new DAL_CodeType();
        /// <summary>
        /// 根据类型加载基础类型数据
        /// </summary>
        /// <param name="CType_Code"></param>
        /// <returns></returns>
        public string GetCodeType(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CodeType.GetCodeType(ValueHandler.GetStringValue(arr[0]));
            String json = JSON.DataTableToArrayList(dt);
            return json;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CType_Code"></param>
        /// <returns></returns>
        public string DeleteType(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CodeType.DeleteType(ValueHandler.GetStringValue(arr[0])).ToString().ToLower();
        }

        /// <summary>
        /// 增加 修改 一条数据
        /// </summary>
        public string UpdateCodeType(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_CodeType.UpdateType(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), BLL_User.User_Name);

            if (flag)
                return "true";
            return "false";
        }
    }
}
