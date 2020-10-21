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
    public class BLL_KeyWordDts
    {
        DAL_KeyWordDts dAL_KeyWordDts = new DAL_KeyWordDts();

        public string SaveWord(object obj)
        {
            ArrayList arr = JSON.getPara(obj);
            bool result = dAL_KeyWordDts.SaveWord(ValueHandler.GetStringValue(arr[0]), ValueHandler.GetStringValue(arr[1]), ValueHandler.GetStringValue(arr[2]),
                                                  ValueHandler.GetIntNumberValue(arr[3]), BLL_User.User_Name);
            return result.ToString();
        } 
    }
}
