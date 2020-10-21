using HCWeb2016;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DAL
{
    public class DAL_CustomerReturnsDts : SqlBase
    {
        public DataTable GetCustomerListByBillNumber(string billno, string code)
        {
            StringBuilder sb = new StringBuilder();
            string sWhere = !string.IsNullOrEmpty(code) ? string.Format(" where Cust_Code !='{0}'", code) : "";
            sb.Append($@"select * from (SELECT Cust_Code
                        FROM dbo.WQT_Customer
                        WHERE Cust_BillNumber = '{ ValueHandler.GetStringValue(billno)}'
                        UNION
                        SELECT Cust_Code
                        FROM dbo.CXT_Customer
                        WHERE Cust_BillNumber = '{ ValueHandler.GetStringValue(billno)}'
                        UNION
                        SELECT Cust_Code
                        FROM dbo.MQY_Customer
                        WHERE Cust_BillNumber = '{ ValueHandler.GetStringValue(billno)}' 
                        UNION
                        SELECT Cust_Code
                        FROM dbo.SSB_Customer
                        WHERE Cust_BillNumber = '{ ValueHandler.GetStringValue(billno)}') t {sWhere}");

            return SearchData(sb.ToString());
        }

        public DataTable GetSSBCustomerListByBillNumber(string billno, string code)
        {
            StringBuilder sb = new StringBuilder();
            string sWhere = !string.IsNullOrEmpty(code) ? string.Format(" where Cust_Code !='{0}'", code) : "";
            sb.Append($@"select * from (SELECT Cust_Code
                        FROM dbo.SSB_Customer
                        WHERE Cust_BillNumber = '{ ValueHandler.GetStringValue(billno)}')t {sWhere}");

            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据code查询详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetDetails(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM CXT_Customer WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据计费号码连表查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetBilllist(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT BL_Code,BL_Year,BL_One,BL_Two,BL_Three,BL_Four,BL_Five,BL_Six,BL_Seven,BL_Eight,BL_Nine,BL_Ten,BL_Eleven,BL_Twelve, ");
            sb.Append("(ISNULL(BL_One,0)+ISNULL(BL_Two,0)+ISNULL(BL_Three,0)+ISNULL(BL_Four,0)+ISNULL(BL_Five,0)+ISNULL(BL_Six,0)+ISNULL(BL_Seven,0)+ISNULL(BL_Eight,0)+ISNULL(BL_Nine,0)+ISNULL(BL_Ten,0)+ISNULL(BL_Eleven,0)+ISNULL(BL_Twelve,0)) AS BL_TotalMoney ");
            sb.Append(" FROM YX_BillList X,CXT_Customer Y WHERE X.BL_Cust_BillNumber=Y.Cust_BillNumber AND X.BL_PlatForm='3' AND Y.Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        #region 修改状态

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="cust_OutMoney"></param>
        /// <param name="cust_ReturnMan"></param>
        /// <param name="cust_IsLessMoney"></param>
        /// <param name="cust_TSNatrue"></param>
        /// <param name="cust_TSSource"></param>
        /// <param name="cust_KF_Remark"></param>
        /// <param name="kfname"></param>
        /// <returns></returns>
        public bool ModState(string code, string cust_State, string cust_UnOrder, string cust_OutDate, int cust_OutMoney, string cust_ReturnMan, string cust_IsLessMoney, string cust_TSNatrue, string cust_TSSource, string cust_KF_Remark, string cust_IsKeep, string cust_TSPhone, string kfname, string isBlack, string userCode)
        {
            StringBuilder sb = new StringBuilder();

            bool flag = false, stateFlag = true;
            using (TransactionScope scope = new TransactionScope())
            {
                string now = DateTime.Now.ToString();
                DataTable dt = SearchData($"SELECT c.Cust_State FROM CXT_Customer c WHERE Cust_Code = '{code}'");
                string tempCustState = dt.Rows[0][0].ToString();
                if (tempCustState != cust_State)
                {
                    //处理状态变更信息
                    stateFlag = UpdateData($@"INSERT INTO dbo.YX_CustStateHistory (CSH_Code,CSH_Remark,JoinMan,CSH_UserCode,CSH_Time)
                        SELECT '{code}','由{dt.Rows[0][0]}改成{cust_State}','{kfname}','{userCode}','{now}'");
                }
                if (cust_UnOrder == "")
                    sb.Append("UPDATE CXT_Customer SET Cust_UnOrder=null,");
                else
                    sb.Append("UPDATE CXT_Customer SET Cust_UnOrder='" + cust_UnOrder + "',");

                sb.Append("Cust_State ='" + ValueHandler.GetStringValue(cust_State) + "',");
                if (cust_OutDate == "")
                    sb.Append("Cust_OutDate =null,");
                else
                    sb.Append("Cust_OutDate ='" + cust_OutDate + "',");

                sb.Append("Cust_OutMoney ='" + ValueHandler.GetIntNumberValue(cust_OutMoney) + "',");
                sb.Append("Cust_ReturnMan ='" + ValueHandler.GetStringValue(cust_ReturnMan) + "',");
                sb.Append("Cust_IsLessMoney ='" + ValueHandler.GetStringValue(cust_IsLessMoney) + "',");
                sb.Append("Cust_TSNatrue ='" + ValueHandler.GetStringValue(cust_TSNatrue) + "',");
                sb.Append("Cust_TSSource ='" + ValueHandler.GetStringValue(cust_TSSource) + "',");
                sb.Append("Cust_TSPhone ='" + ValueHandler.GetStringValue(cust_TSPhone) + "',");
                sb.Append("Cust_KF_Remark ='" + ValueHandler.GetStringValue(cust_KF_Remark) + "',");
                sb.Append("Cust_IsKeep ='" + ValueHandler.GetStringValue(cust_IsKeep) + "',");
                sb.Append("Cust_KF_UserName ='" + ValueHandler.GetStringValue(kfname) + "',");
                string tdr = cust_State == "已审" ? "" : ValueHandler.GetStringValue(kfname);
                sb.Append("Cust_DealPerson ='" + tdr + "',");
                sb.Append("Cust_OperateTime='" + now + "' ");
                sb.Append("WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
                flag = UpdateData(sb.ToString());

                if (isBlack == "1")//插入黑名单
                {
                    DAL_BlackListDts dalBLack = new DAL_BlackListDts();

                    flag = dalBLack.PlInsertBlackPhone(cust_TSPhone, kfname, $"{DateTime.Now.ToString("yyyyMMdd")} {cust_TSNatrue}用户");
                }

                if (flag && stateFlag)
                    scope.Complete();
                else
                    scope.Dispose();
            }
            return flag;
        }

        #endregion

        #region 民企云

        /// <summary>
        /// 根据code查询详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetMQYDetails(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM MQY_Customer WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据计费号码连表查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetMQYBilllist(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT BL_Code,BL_Year,BL_One,BL_Two,BL_Three,BL_Four,BL_Five,BL_Six,BL_Seven,BL_Eight,BL_Nine,BL_Ten,BL_Eleven,BL_Twelve, ");
            sb.Append("(ISNULL(BL_One,0)+ISNULL(BL_Two,0)+ISNULL(BL_Three,0)+ISNULL(BL_Four,0)+ISNULL(BL_Five,0)+ISNULL(BL_Six,0)+ISNULL(BL_Seven,0)+ISNULL(BL_Eight,0)+ISNULL(BL_Nine,0)+ISNULL(BL_Ten,0)+ISNULL(BL_Eleven,0)+ISNULL(BL_Twelve,0)) AS BL_TotalMoney ");
            sb.Append(" FROM YX_BillList X,MQY_Customer Y WHERE X.BL_Cust_BillNumber=Y.Cust_BillNumber AND X.BL_PlatForm='2' AND Y.Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="cust_OutMoney"></param>
        /// <param name="cust_ReturnMan"></param>
        /// <param name="cust_IsLessMoney"></param>
        /// <param name="cust_TSNatrue"></param>
        /// <param name="cust_TSSource"></param>
        /// <param name="cust_KF_Remark"></param>
        /// <param name="kfname"></param>
        /// <returns></returns>
        public bool ModMQYState(string code, string cust_State, string cust_UnOrder, string cust_OutDate, int cust_OutMoney, string cust_ReturnMan, string cust_IsLessMoney, string cust_TSNatrue, string cust_TSSource, string cust_KF_Remark, string cust_IsKeep, string cust_TSPhone, string kfname, string isBlack, string userCode)
        {
            StringBuilder sb = new StringBuilder();
            bool flag = false, stateFlag = true;
            using (TransactionScope scope = new TransactionScope())
            {
                string now = DateTime.Now.ToString();
                DataTable dt = SearchData($"SELECT c.Cust_State FROM MQY_Customer c WHERE Cust_Code = '{code}'");
                string tempCustState = dt.Rows[0][0].ToString();
                if (tempCustState != cust_State)
                {
                    //处理状态变更信息
                    stateFlag = UpdateData($@"INSERT INTO dbo.YX_CustStateHistory (CSH_Code,CSH_Remark,JoinMan,CSH_UserCode,CSH_Time)
                        SELECT '{code}','由{dt.Rows[0][0]}改成{cust_State}','{kfname}','{userCode}','{now}'");
                }
                if (cust_UnOrder == "")
                    sb.Append("UPDATE MQY_Customer SET Cust_UnOrder=null,");
                else
                    sb.Append("UPDATE MQY_Customer SET Cust_UnOrder='" + cust_UnOrder + "',");

                sb.Append("Cust_State ='" + ValueHandler.GetStringValue(cust_State) + "',");
                if (cust_OutDate == "")
                    sb.Append("Cust_OutDate =null,");
                else
                    sb.Append("Cust_OutDate ='" + cust_OutDate + "',");

                sb.Append("Cust_OutMoney ='" + ValueHandler.GetIntNumberValue(cust_OutMoney) + "',");
                sb.Append("Cust_ReturnMan ='" + ValueHandler.GetStringValue(cust_ReturnMan) + "',");
                sb.Append("Cust_IsLessMoney ='" + ValueHandler.GetStringValue(cust_IsLessMoney) + "',");
                sb.Append("Cust_TSNatrue ='" + ValueHandler.GetStringValue(cust_TSNatrue) + "',");
                sb.Append("Cust_TSPhone ='" + ValueHandler.GetStringValue(cust_TSPhone) + "',");
                sb.Append("Cust_TSSource ='" + ValueHandler.GetStringValue(cust_TSSource) + "',");
                sb.Append("Cust_KF_Remark ='" + ValueHandler.GetStringValue(cust_KF_Remark) + "',");
                sb.Append("Cust_IsKeep ='" + ValueHandler.GetStringValue(cust_IsKeep) + "',");
                sb.Append("Cust_KF_UserName ='" + ValueHandler.GetStringValue(kfname) + "',");
                string tdr = cust_State == "已审" ? "" : ValueHandler.GetStringValue(kfname);
                sb.Append("Cust_DealPerson ='" + tdr + "',");
                sb.Append("Cust_OperateTime='" + now + "' ");
                sb.Append("WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
                flag = UpdateData(sb.ToString());

                if (isBlack == "1")//插入黑名单
                {
                    DAL_BlackListDts dalBLack = new DAL_BlackListDts();

                    flag = dalBLack.PlInsertBlackPhone(cust_TSPhone, kfname, $"{DateTime.Now.ToString("yyyyMMdd")} {cust_TSNatrue}用户");
                }

                if (flag && stateFlag)
                    scope.Complete();
                else
                    scope.Dispose();
            }
            return flag;
        }
        #endregion

        #region 维权通

        /// <summary>
        /// 根据code查询详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetWQTDetails(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM WQT_Customer WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据计费号码连表查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetWQTBilllist(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT BL_Code,BL_Year,BL_One,BL_Two,BL_Three,BL_Four,BL_Five,BL_Six,BL_Seven,BL_Eight,BL_Nine,BL_Ten,BL_Eleven,BL_Twelve, ");
            sb.Append("(ISNULL(BL_One,0)+ISNULL(BL_Two,0)+ISNULL(BL_Three,0)+ISNULL(BL_Four,0)+ISNULL(BL_Five,0)+ISNULL(BL_Six,0)+ISNULL(BL_Seven,0)+ISNULL(BL_Eight,0)+ISNULL(BL_Nine,0)+ISNULL(BL_Ten,0)+ISNULL(BL_Eleven,0)+ISNULL(BL_Twelve,0)) AS BL_TotalMoney ");
            sb.Append(" FROM YX_BillList X,WQT_Customer Y WHERE X.BL_Cust_BillNumber=Y.Cust_BillNumber AND X.BL_PlatForm='1' AND Y.Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="cust_OutMoney"></param>
        /// <param name="cust_ReturnMan"></param>
        /// <param name="cust_IsLessMoney"></param>
        /// <param name="cust_TSNatrue"></param>
        /// <param name="cust_TSSource"></param>
        /// <param name="cust_KF_Remark"></param>
        /// <param name="kfname"></param>
        /// <returns></returns>
        public bool ModWQTState(string code, string cust_State, string cust_UnOrder, string cust_OutDate, int cust_OutMoney, string cust_ReturnMan, string cust_IsLessMoney, string cust_TSNatrue, string cust_TSSource, string cust_KF_Remark, string cust_IsKeep, string cust_TSPhone, string kfname, string isBlack,string userCode)
        {
            StringBuilder sb = new StringBuilder();
            bool flag = false, stateFlag = true;
            using (TransactionScope scope = new TransactionScope())
            {
                string now = DateTime.Now.ToString();
                DataTable dt = SearchData($"SELECT c.Cust_State FROM WQT_Customer c WHERE Cust_Code = '{code}'");
                string tempCustState = dt.Rows[0][0].ToString();
                if (tempCustState != cust_State)
                {
                    //处理状态变更信息
                    stateFlag = UpdateData($@"INSERT INTO dbo.YX_CustStateHistory (CSH_Code,CSH_Remark,JoinMan,CSH_UserCode,CSH_Time)
                        SELECT '{code}','由{dt.Rows[0][0]}改成{cust_State}','{kfname}','{userCode}','{now}'");
                }
                if (cust_UnOrder == "")
                    sb.Append("UPDATE WQT_Customer SET Cust_UnOrder=null,");
                else
                    sb.Append("UPDATE WQT_Customer SET Cust_UnOrder='" + cust_UnOrder + "',");

                sb.Append("Cust_State ='" + ValueHandler.GetStringValue(cust_State) + "',");
                if (cust_OutDate == "")
                    sb.Append("Cust_OutDate =null,");
                else
                    sb.Append("Cust_OutDate ='" + cust_OutDate + "',");

                sb.Append("Cust_OutMoney ='" + ValueHandler.GetIntNumberValue(cust_OutMoney) + "',");
                sb.Append("Cust_ReturnMan ='" + ValueHandler.GetStringValue(cust_ReturnMan) + "',");
                sb.Append("Cust_IsLessMoney ='" + ValueHandler.GetStringValue(cust_IsLessMoney) + "',");
                sb.Append("Cust_TSNatrue ='" + ValueHandler.GetStringValue(cust_TSNatrue) + "',");
                sb.Append("Cust_TSSource ='" + ValueHandler.GetStringValue(cust_TSSource) + "',");
                sb.Append("Cust_TSPhone ='" + ValueHandler.GetStringValue(cust_TSPhone) + "',");
                sb.Append("Cust_KF_Remark ='" + ValueHandler.GetStringValue(cust_KF_Remark) + "',");
                sb.Append("Cust_IsKeep ='" + ValueHandler.GetStringValue(cust_IsKeep) + "',");
                sb.Append("Cust_KF_UserName ='" + ValueHandler.GetStringValue(kfname) + "',");
                string tdr = cust_State == "已审" ? "" : ValueHandler.GetStringValue(kfname);
                sb.Append("Cust_DealPerson ='" + tdr + "',");
                sb.Append("Cust_OperateTime='" + now + "' ");
                sb.Append("WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
                flag = UpdateData(sb.ToString());

                if (isBlack == "1")//插入黑名单
                {
                    DAL_BlackListDts dalBLack = new DAL_BlackListDts();

                    flag = dalBLack.PlInsertBlackPhone(cust_TSPhone, kfname, $"{DateTime.Now.ToString("yyyyMMdd")} {cust_TSNatrue}用户");
                }

                if (flag && stateFlag)
                    scope.Complete();
                else
                    scope.Dispose();
            }
            return flag;
        }

        #endregion

        #region 新消费宝典

        /// <summary>
        /// 根据code查询详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetXFBDetails(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM XFB_Customer WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据计费号码连表查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetXFBBilllist(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT BL_Code,BL_Year,BL_One,BL_Two,BL_Three,BL_Four,BL_Five,BL_Six,BL_Seven,BL_Eight,BL_Nine,BL_Ten,BL_Eleven,BL_Twelve, ");
            sb.Append("(ISNULL(BL_One,0)+ISNULL(BL_Two,0)+ISNULL(BL_Three,0)+ISNULL(BL_Four,0)+ISNULL(BL_Five,0)+ISNULL(BL_Six,0)+ISNULL(BL_Seven,0)+ISNULL(BL_Eight,0)+ISNULL(BL_Nine,0)+ISNULL(BL_Ten,0)+ISNULL(BL_Eleven,0)+ISNULL(BL_Twelve,0)) AS BL_TotalMoney ");
            sb.Append(" FROM YX_BillList X,XFB_Customer Y WHERE X.BL_Cust_BillNumber=Y.Cust_BillNumber AND X.BL_PlatForm='4' AND Y.Cust_Code='" + ValueHandler.GetStringValue(code) + "' ORDER BY X.BL_Year ASC");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="cust_OutMoney"></param>
        /// <param name="cust_ReturnMan"></param>
        /// <param name="cust_IsLessMoney"></param>
        /// <param name="cust_TSNatrue"></param>
        /// <param name="cust_TSSource"></param>
        /// <param name="cust_KF_Remark"></param>
        /// <param name="cust_TSPhone"></param>
        /// <param name="kfname"></param>
        /// <returns></returns>
        public bool ModXFBState(string code, string cust_State, string cust_UnOrder, string cust_OutDate, int cust_OutMoney, string cust_ReturnMan, string cust_IsLessMoney, string cust_TSNatrue, string cust_TSSource, string cust_KF_Remark, string cust_IsKeep, string cust_TSPhone, string kfname, string isBlack,string userCode)
        {
            StringBuilder sb = new StringBuilder();
            bool flag = false, stateFlag = true;
            using (TransactionScope scope = new TransactionScope())
            {
                string now = DateTime.Now.ToString();
                DataTable dt = SearchData($"SELECT c.Cust_State FROM XFB_Customer c WHERE Cust_Code = '{code}'");
                string tempCustState = dt.Rows[0][0].ToString();
                if (tempCustState != cust_State)
                {
                    //处理状态变更信息
                    stateFlag = UpdateData($@"INSERT INTO dbo.YX_CustStateHistory (CSH_Code,CSH_Remark,JoinMan,CSH_UserCode,CSH_Time)
                        SELECT '{code}','由{dt.Rows[0][0]}改成{cust_State}','{kfname}','{userCode}','{now}'");
                }
                if (cust_UnOrder == "")
                    sb.Append("UPDATE XFB_Customer SET Cust_UnOrder=null,");
                else
                    sb.Append("UPDATE XFB_Customer SET Cust_UnOrder='" + cust_UnOrder + "',");

                sb.Append("Cust_State ='" + ValueHandler.GetStringValue(cust_State) + "',");
                if (cust_OutDate == "")
                    sb.Append("Cust_OutDate =null,");
                else
                    sb.Append("Cust_OutDate ='" + cust_OutDate + "',");

                sb.Append("Cust_OutMoney ='" + ValueHandler.GetIntNumberValue(cust_OutMoney) + "',");
                sb.Append("Cust_ReturnMan ='" + ValueHandler.GetStringValue(cust_ReturnMan) + "',");
                sb.Append("Cust_IsLessMoney ='" + ValueHandler.GetStringValue(cust_IsLessMoney) + "',");
                sb.Append("Cust_TSNatrue ='" + ValueHandler.GetStringValue(cust_TSNatrue) + "',");
                sb.Append("Cust_TSSource ='" + ValueHandler.GetStringValue(cust_TSSource) + "',");
                sb.Append("Cust_TSPhone ='" + ValueHandler.GetStringValue(cust_TSPhone) + "',");
                sb.Append("Cust_KF_Remark ='" + ValueHandler.GetStringValue(cust_KF_Remark) + "',");
                sb.Append("Cust_IsKeep ='" + ValueHandler.GetStringValue(cust_IsKeep) + "',");
                sb.Append("Cust_KF_UserName ='" + ValueHandler.GetStringValue(kfname) + "',");
                string tdr = cust_State == "已审" ? "" : ValueHandler.GetStringValue(kfname);
                sb.Append("Cust_DealPerson ='" + tdr + "',");
                sb.Append("Cust_OperateTime='" + now + "' ");
                sb.Append("WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
                flag = UpdateData(sb.ToString());

                if (isBlack == "1")//插入黑名单
                {
                    DAL_BlackListDts dalBLack = new DAL_BlackListDts();

                    flag = dalBLack.PlInsertBlackPhone(cust_TSPhone, kfname, $"{DateTime.Now.ToString("yyyyMMdd")} {cust_TSNatrue}用户");
                }

                if (flag && stateFlag)
                    scope.Complete();
                else
                    scope.Dispose();
            }
            return flag;
        }

        /// <summary>
        /// 验证是否开户
        /// </summary>
        /// <param name="billno"></param>
        /// <returns></returns>
        public DataTable GetXFBCustomerListByBillNumber(string billno)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"SELECT Cust_Code
                        FROM dbo.XFB_Customer
                        WHERE Cust_BillNumber = '{ ValueHandler.GetStringValue(billno)}'");

            return SearchData(sb.ToString());
        }

        #endregion

        #region 实时保

        /// <summary>
        /// 根据code查询详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetSSBDetails(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM SSB_Customer WHERE Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 根据计费号码连表查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetSSBBilllist(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT BL_Code,BL_Year,BL_One,BL_Two,BL_Three,BL_Four,BL_Five,BL_Six,BL_Seven,BL_Eight,BL_Nine,BL_Ten,BL_Eleven,BL_Twelve, ");
            sb.Append("(ISNULL(BL_One,0)+ISNULL(BL_Two,0)+ISNULL(BL_Three,0)+ISNULL(BL_Four,0)+ISNULL(BL_Five,0)+ISNULL(BL_Six,0)+ISNULL(BL_Seven,0)+ISNULL(BL_Eight,0)+ISNULL(BL_Nine,0)+ISNULL(BL_Ten,0)+ISNULL(BL_Eleven,0)+ISNULL(BL_Twelve,0)) AS BL_TotalMoney ");
            sb.Append(" FROM YX_BillList X,SSB_Customer Y WHERE X.BL_Cust_BillNumber=Y.Cust_BillNumber AND X.BL_PlatForm='5' AND Y.Cust_Code='" + ValueHandler.GetStringValue(code) + "'");
            return SearchData(sb.ToString());
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cust_UnOrder"></param>
        /// <param name="cust_OutDate"></param>
        /// <param name="cust_OutMoney"></param>
        /// <param name="cust_ReturnMan"></param>
        /// <param name="cust_IsLessMoney"></param>
        /// <param name="cust_TSNatrue"></param>
        /// <param name="cust_TSSource"></param>
        /// <param name="cust_KF_Remark"></param>
        /// <param name="kfname"></param>
        /// <returns></returns>
        public bool ModSSBState(string code, string cust_State, string cust_UnOrder, string cust_OutDate, int cust_OutMoney, string cust_ReturnMan, string cust_IsLessMoney, string cust_TSNatrue, string cust_TSSource, string cust_KF_Remark, string cust_IsKeep, string cust_TSPhone, string kfname, string isBlack,string userCode)
        {
            StringBuilder sb = new StringBuilder();
            bool flag = false, stateFlag = true;
            using (TransactionScope scope = new TransactionScope())
            {
                string now = DateTime.Now.ToString();
                DataTable dt = SearchData($"SELECT c.Cust_State FROM SSB_Customer c WHERE Cust_Code = '{code}'");
                string tempCustState = dt.Rows[0][0].ToString();
                if (tempCustState != cust_State)
                {
                    //处理状态变更信息
                    stateFlag = UpdateData($@"INSERT INTO dbo.YX_CustStateHistory (CSH_Code,CSH_Remark,JoinMan,CSH_UserCode,CSH_Time)
                        SELECT '{code}','由{dt.Rows[0][0]}改成{cust_State}','{kfname}','{userCode}','{now}'");
                }
                if (cust_UnOrder == "")
                    sb.Append("UPDATE SSB_Customer SET Cust_UnOrder = null,");
                else
                    sb.Append("UPDATE SSB_Customer SET Cust_UnOrder = '" + cust_UnOrder + "',");

                sb.Append("Cust_State ='" + ValueHandler.GetStringValue(cust_State) + "',");
                if (cust_OutDate == "")
                    sb.Append("Cust_OutDate = null,");
                else
                    sb.Append("Cust_OutDate = '" + cust_OutDate + "',");

                sb.Append("Cust_OutMoney = '" + ValueHandler.GetIntNumberValue(cust_OutMoney) + "',");
                sb.Append("Cust_ReturnMan = '" + ValueHandler.GetStringValue(cust_ReturnMan) + "',");
                sb.Append("Cust_IsLessMoney = '" + ValueHandler.GetStringValue(cust_IsLessMoney) + "',");
                sb.Append("Cust_TSNatrue = '" + ValueHandler.GetStringValue(cust_TSNatrue) + "',");
                sb.Append("Cust_TSSource = '" + ValueHandler.GetStringValue(cust_TSSource) + "',");
                sb.Append("Cust_TSPhone = '" + ValueHandler.GetStringValue(cust_TSPhone) + "',");
                sb.Append("Cust_KF_Remark = '" + ValueHandler.GetStringValue(cust_KF_Remark) + "',");
                sb.Append("Cust_IsKeep = '" + ValueHandler.GetStringValue(cust_IsKeep) + "',");
                sb.Append("Cust_KF_UserName = '" + ValueHandler.GetStringValue(kfname) + "',");
                string tdr = cust_State == "已审" ? "" : ValueHandler.GetStringValue(kfname);
                sb.Append("Cust_DealPerson = '" + tdr + "',");
                sb.Append("Cust_OperateTime = '" + now + "' ");
                sb.Append("WHERE Cust_Code = '" + ValueHandler.GetStringValue(code) + "'");
                flag = UpdateData(sb.ToString());

                if (isBlack == "1")//插入黑名单
                {
                    DAL_BlackListDts dalBLack = new DAL_BlackListDts();

                    flag = dalBLack.PlInsertBlackPhone(cust_TSPhone, kfname, $"{DateTime.Now.ToString("yyyyMMdd")} {cust_TSNatrue}用户");
                }

                if (flag && stateFlag)
                    scope.Complete();
                else
                    scope.Dispose();
            }

            return flag;
        }

        public DataTable GetBillInfo(string billNo)
        {
            var sql = $@"SELECT year (b.BL_Date) year
                            , sum (b.BL_FEE) YTotal
                            , sum (CASE MONTH (BL_Date) WHEN 1 THEN BL_FEE ELSE 0 END) AS January
                            , sum (CASE MONTH (BL_Date) WHEN 2 THEN BL_FEE ELSE 0 END) AS February
                            , sum (CASE MONTH (BL_Date) WHEN 3 THEN BL_FEE ELSE 0 END) AS March
                            , sum (CASE MONTH (BL_Date) WHEN 4 THEN BL_FEE ELSE 0 END) AS April
                            , sum (CASE MONTH (BL_Date) WHEN 5 THEN BL_FEE ELSE 0 END) AS May
                            , sum (CASE MONTH (BL_Date) WHEN 6 THEN BL_FEE ELSE 0 END) AS June
                            , sum (CASE MONTH (BL_Date) WHEN 7 THEN BL_FEE ELSE 0 END) AS July
                            , sum (CASE MONTH (BL_Date) WHEN 8 THEN BL_FEE ELSE 0 END) AS August
                            , sum (CASE MONTH (BL_Date) WHEN 9 THEN BL_FEE ELSE 0 END) AS September
                            , sum (CASE MONTH (BL_Date) WHEN 10 THEN BL_FEE ELSE 0 END) AS October
                            , sum (CASE MONTH (BL_Date) WHEN 11 THEN BL_FEE ELSE 0 END) AS November
                            , sum (CASE MONTH (BL_Date) WHEN 12 THEN BL_FEE ELSE 0 END) AS December
                        FROM YX_SSBBillList B
                        WHERE b.DataState = 0 AND b.BL_Code = '{billNo}'
                        GROUP BY year (b.BL_Date)
                        ORDER BY year (b.BL_Date) ASC";
            return SearchData(sql);
        }

        public DataTable GetBillDtsInfo(string billNo, string year, string month)
        {
            var sql = $@"SELECT day (dt) day
                            , CASE WHEN day IS NULL THEN '' ELSE '1' END Fee
                        FROM (SELECT dateadd (day, number, '{year}-{month}-1') AS dt
                              FROM master.dbo.spt_values
                              WHERE type = 'P' AND number <= DATEDIFF (day, '{year}-{month}-1',dateadd(m,1,'{year}-{month}-1')-1)) t
                            LEFT JOIN (SELECT day (BL_Date) day
                                       FROM YX_SSBBillList B
                                       WHERE b.DataState = 0 AND b.BL_Code = '{billNo}' AND month (BL_Date) = {month} AND year(BL_Date)={year}) a ON day (dt) = a.day  
                        ORDER BY dt ASC ";

            return SearchData(sql);
        }

        /// <summary>
        /// 获取客户状态变更表最新变更记录
        /// </summary>
        /// <param name="custCode"></param>
        /// <returns></returns>
        public DataTable GetLastestStateBillHistory(string custCode)
        {
            string sql = $@"SELECT TOP 1 u.User_Name,h.CSH_Time,h.CSH_Remark FROM YX_CustStateHistory h 
                             JOIN AF_User u ON h.CSH_UserCode = u.User_Code
                            WHERE h.CSH_Code = '{custCode}' 
                            ORDER BY h.CSH_Time DESC";

            return SearchData(sql);
        }

        #endregion
    }
}
