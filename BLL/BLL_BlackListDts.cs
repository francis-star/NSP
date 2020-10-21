using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Collections;
using HCWeb2016;
using System.Data;

namespace BLL
{
    public class BLL_BlackListDts
    {
        DAL_BlackListDts dAL_BlackListDts = new DAL_BlackListDts();

        public string SaveBlackList(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool result = dAL_BlackListDts.SaveBlackList(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]), ValueHandler.GetStringValue(arr[3]), ValueHandler.GetStringValue(arr[4]), ValueHandler.GetStringValue(arr[5]), BLL_User.User_Name);
            return result.ToString();
        }

        public string PlInsertBlackPhone(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool dt = dAL_BlackListDts.PlInsertBlackPhone(ValueHandler.GetStringValue(arr[0]), BLL_User.User_Name, arr[1].ToString());
            if (dt)
                return "true";
            return "false";

        }
    }
}
