using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Collections;
using HCWeb2016;
using System.Data;
using Common;
using Model;

namespace BLL
{
    public class BLL_UserSetDts
    {
        DAL_UserSetDts dAL_UserSetDts = new DAL_UserSetDts();
        AF_User aF_User = new AF_User();

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetUser(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dt = dAL_UserSetDts.GetUser(ValueHandler.GetStringValue(arr[0]));
            dt.Rows[0]["User_Password"] = Security.DecryptDES(dt.Rows[0]["User_Password"].ToString());
            string json = JSON.DataTableToTreeList(dt);
            return json;
        }

        #region 得到模块信息
        /// <summary>
        /// 得到模块信息
        /// </summary>
        public string GetMenu()
        {
            DataTable dt = dAL_UserSetDts.GetMenu();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //如果不为空，那么此模块含有功能按钮
                if (ValueHandler.GetStringValue(dt.Rows[i]["SM_FunIDs"]) != "")
                {
                    string[] ArrMenuId = ValueHandler.GetStringValue(dt.Rows[i]["SM_FunIDs"]).Split(',');
                    string[] ArrMenuName = ValueHandler.GetStringValue(dt.Rows[i]["SM_FunNames"]).Split(',');
                    for (int j = 0; j < ArrMenuId.Length; j++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["SM_Code"] = ValueHandler.GetStringValue(dt.Rows[i]["SM_Code"]) + "|" + ValueHandler.GetStringValue(ArrMenuId[j]);
                        dr["SM_Name"] = ValueHandler.GetStringValue(ArrMenuName[j]);
                        dr["SM_PCode"] = ValueHandler.GetStringValue(dt.Rows[i]["SM_Code"]);
                        dr["SM_Kind"] = "1";
                        dr["SM_Type"] = "1";
                        dt.Rows.Add(dr.ItemArray);
                    }
                }
            }
            String json = JSON.DataTableToTreeList(dt);
            return json;
        }
        #endregion

        #region 获取用户角色权限
        /// <summary>
        /// 获取用户角色权限
        /// </summary>
        public string GetUserModule(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            DataTable dtTemp = dAL_UserSetDts.GetUserModule(ValueHandler.GetStringValue(arr[0]));
            DataTable dt = dtTemp.Copy();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //取当前模块的按钮集合
                DataTable dt1 = new SqlBase().SearchData("SELECT SM_FunIDs FROM dbo.AF_SysModule WHERE SM_Code = '" + ValueHandler.GetStringValue(dt.Rows[i]["UP_SM_Code"]) + "'");
                string strFuns = ValueHandler.GetStringValue(dt1.Rows[0]["SM_FunIDs"]);

                if (ValueHandler.GetStringValue(dt.Rows[i]["UP_SM_FunIDs"]) != "" || (ValueHandler.GetStringValue(dt.Rows[i]["UP_SM_FunIDs"]) == "" && strFuns != ""))
                {
                    if (ValueHandler.GetStringValue(dt.Rows[i]["UP_SM_FunIDs"]) != "")
                    {
                        string[] strArr = ValueHandler.GetStringValue(dt.Rows[i]["UP_SM_FunIDs"]).Split(',');
                        for (int j = 0; j < strArr.Length; j++)
                        {
                            //从原模块按钮中替换掉未选择的按钮Code
                            strFuns = strFuns.Replace(ValueHandler.GetStringValue(strArr[j]), "");
                            //第一个按钮被替换
                            if (strFuns != "")
                            {
                                if (strFuns.Substring(0, 1) == ",")
                                    strFuns = strFuns.Substring(1, strFuns.Length - 1);
                            }
                            //中间替换时会产生2个逗号
                            strFuns = strFuns.Replace(",,", ",");
                            //最后一个按钮被替换
                            if (strFuns != "")
                            {
                                if (strFuns.Substring(strFuns.Length - 1, 1) == ",")
                                    strFuns = strFuns.Substring(0, strFuns.Length - 1);
                            }
                        }
                    }
                    string[] ArrFuns = strFuns.Split(',');
                    for (int k = 0; k < ArrFuns.Length; k++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["UP_SM_FunIDs"] = ArrFuns[k];
                        dr["UP_SM_Code"] = ValueHandler.GetStringValue(dt.Rows[i]["UP_SM_Code"]) + "|" + ArrFuns[k];
                        dr["SM_Type"] = "1";
                        dtTemp.Rows.Add(dr.ItemArray);
                    }

                }
            }
            String json = JSON.DataTableToTreeList(dtTemp);
            return json;
        }
        #endregion

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SaveUser(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            aF_User.User_Code = ValueHandler.GetStringValue(arr[0]);
            aF_User.User_LoginName = ValueHandler.GetStringValue(arr[1]);
            aF_User.User_Password = Security.EncryptDES(arr[2].ToString());
            aF_User.User_Name = ValueHandler.GetStringValue(arr[3]);
            aF_User.User_Sex = ValueHandler.GetStringValue(arr[4]);
            aF_User.User_Age = ValueHandler.GetIntNumberValue(arr[5]);
            aF_User.User_Phone = ValueHandler.GetStringValue(arr[6]);
            aF_User.User_Post = ValueHandler.GetStringValue(arr[7]);
            aF_User.User_EntryDate = ValueHandler.GetStringValue(arr[8]);
            aF_User.User_Place = ValueHandler.GetStringValue(arr[9]);
            aF_User.JoinMan = BLL_User.User_Name;
            return dAL_UserSetDts.SaveUser(aF_User);
        }

        /// <summary>
        ///保存用户角色的权限
        /// </summary>
        public string SaveUserModule(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            String json = dAL_UserSetDts.SaveUserModule(arr).ToString();
            if (json == "true")
                return "true";
            else
                return "false";
        }
    }
}
