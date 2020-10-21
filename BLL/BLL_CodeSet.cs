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
    public class BLL_CodeSet
    {
        DAL_CodeSet dAL_CodeSet = new DAL_CodeSet();

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public string GetCodeType()
        {
            DataTable dt = dAL_CodeSet.GetCodeType();
            String json = JSON.DataTableToTreeList(dt);
            return json;
        }

        /// <summary>
        /// 根据类型加载基础明细表数据
        /// </summary>
        public string GetCodeSet(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_CodeSet.GetCodeSet(ValueHandler.GetStringValue(arr[0]));
            String json = JSON.DataTableToArrayList(dt);
            return json;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public string Delete(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            return dAL_CodeSet.Delete(ValueHandler.GetStringValue(arr[0])).ToString().ToLower(); 
        }

        /// <summary>
        /// 增加 修改 一条数据
        /// </summary>
        public string UpdateCodeSet(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool flag = dAL_CodeSet.Update(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), BLL_User.User_Name);

            if (flag)
                return "true";
            return "false";
        }
    }
}
