using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CXT_Customer
    {
        /// <summary>
        /// 编码
        /// </summary>		
        private string _cust_code;
        public string Cust_Code
        {
            get { return _cust_code; }
            set { _cust_code = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>		
        private string _cust_name;
        public string Cust_Name
        {
            get { return _cust_name; }
            set { _cust_name = value; }
        }
        /// <summary>
        /// 旧名称
        /// </summary>		
        private string _cust_oldname;
        public string Cust_OldName
        {
            get { return _cust_oldname; }
            set { _cust_oldname = value; }
        }
        /// <summary>
        /// 关键字
        /// </summary>		
        private string _cust_namekey;
        public string Cust_NameKey
        {
            get { return _cust_namekey; }
            set { _cust_namekey = value; }
        }
        /// <summary>
        /// 电话
        /// </summary>		
        private string _cust_phone;
        public string Cust_Phone
        {
            get { return _cust_phone; }
            set { _cust_phone = value; }
        }
        /// <summary>
        /// 联系人
        /// </summary>		
        private string _cust_linkman;
        public string Cust_Linkman
        {
            get { return _cust_linkman; }
            set { _cust_linkman = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>		
        private string _cust_linkphone;
        public string Cust_LinkPhone
        {
            get { return _cust_linkphone; }
            set { _cust_linkphone = value; }
        }
        /// <summary>
        /// 省编码
        /// </summary>		
        private string _cust_provincecode;
        public string Cust_ProvinceCode
        {
            get { return _cust_provincecode; }
            set { _cust_provincecode = value; }
        }
        /// <summary>
        /// 省名称
        /// </summary>		
        private string _cust_provincename;
        public string Cust_ProvinceName
        {
            get { return _cust_provincename; }
            set { _cust_provincename = value; }
        }
        /// <summary>
        /// 市编码
        /// </summary>		
        private string _cust_citycode;
        public string Cust_CityCode
        {
            get { return _cust_citycode; }
            set { _cust_citycode = value; }
        }
        /// <summary>
        /// 市名称
        /// </summary>		
        private string _cust_cityname;
        public string Cust_CityName
        {
            get { return _cust_cityname; }
            set { _cust_cityname = value; }
        }
        /// <summary>
        /// 县区编码
        /// </summary>		
        private string _cust_countycode;
        public string Cust_CountyCode
        {
            get { return _cust_countycode; }
            set { _cust_countycode = value; }
        }
        /// <summary>
        /// 县区名称
        /// </summary>		
        private string _cust_countyname;
        public string Cust_CountyName
        {
            get { return _cust_countyname; }
            set { _cust_countyname = value; }
        }
        /// <summary>
        /// 地址
        /// </summary>		
        private string _cust_address;
        public string Cust_Address
        {
            get { return _cust_address; }
            set { _cust_address = value; }
        }
        /// <summary>
        /// 是否计费
        /// </summary>		
        private string _cust_isbill;
        public string Cust_IsBill
        {
            get { return _cust_isbill; }
            set { _cust_isbill = value; }
        }
        /// <summary>
        /// Cust_BillMoney
        /// </summary>		
        private int _cust_billmoney;
        public int Cust_BillMoney
        {
            get { return _cust_billmoney; }
            set { _cust_billmoney = value; }
        }
        /// <summary>
        /// Cust_BillNumber
        /// </summary>		
        private string _cust_billnumber;
        public string Cust_BillNumber
        {
            get { return _cust_billnumber; }
            set { _cust_billnumber = value; }
        }
        /// <summary>
        /// Cust_Nature
        /// </summary>		
        private string _cust_nature;
        public string Cust_Nature
        {
            get { return _cust_nature; }
            set { _cust_nature = value; }
        }
        /// <summary>
        /// Cust_KFVoice
        /// </summary>		
        private string _cust_kfvoice;
        public string Cust_KFVoice
        {
            get { return _cust_kfvoice; }
            set { _cust_kfvoice = value; }
        }
        /// <summary>
        /// Cust_Source
        /// </summary>		
        private string _cust_source;
        public string Cust_Source
        {
            get { return _cust_source; }
            set { _cust_source = value; }
        }
        /// <summary>
        /// Cust_OpenDate
        /// </summary>		
        private DateTime _cust_opendate;
        public DateTime Cust_OpenDate
        {
            get { return _cust_opendate; }
            set { _cust_opendate = value; }
        }
        /// <summary>
        /// Cust_WH_Remark
        /// </summary>		
        private string _cust_wh_remark;
        public string Cust_WH_Remark
        {
            get { return _cust_wh_remark; }
            set { _cust_wh_remark = value; }
        }
        /// <summary>
        /// Cust_WH_UserName
        /// </summary>		
        private string _cust_wh_username;
        public string Cust_WH_UserName
        {
            get { return _cust_wh_username; }
            set { _cust_wh_username = value; }
        }
        /// <summary>
        /// Cust_State
        /// </summary>		
        private string _cust_state;
        public string Cust_State
        {
            get { return _cust_state; }
            set { _cust_state = value; }
        }
        /// <summary>
        /// Cust_ReturnDate
        /// </summary>		
        private DateTime _cust_returndate;
        public DateTime Cust_ReturnDate
        {
            get { return _cust_returndate; }
            set { _cust_returndate = value; }
        }
        /// <summary>
        /// Cust_ReturnContent
        /// </summary>		
        private string _cust_returncontent;
        public string Cust_ReturnContent
        {
            get { return _cust_returncontent; }
            set { _cust_returncontent = value; }
        }
        /// <summary>
        /// Cust_OutDate
        /// </summary>		
        private DateTime _cust_outdate;
        public DateTime Cust_OutDate
        {
            get { return _cust_outdate; }
            set { _cust_outdate = value; }
        }
        /// <summary>
        /// Cust_UnOrder
        /// </summary>		
        private DateTime _cust_unorder;
        public DateTime Cust_UnOrder
        {
            get { return _cust_unorder; }
            set { _cust_unorder = value; }
        }
        /// <summary>
        /// Cust_OutMoney
        /// </summary>		
        private int _cust_outmoney;
        public int Cust_OutMoney
        {
            get { return _cust_outmoney; }
            set { _cust_outmoney = value; }
        }
        /// <summary>
        /// Cust_ReturnMan
        /// </summary>		
        private string _cust_returnman;
        public string Cust_ReturnMan
        {
            get { return _cust_returnman; }
            set { _cust_returnman = value; }
        }
        /// <summary>
        /// Cust_IsView
        /// </summary>		
        private int _cust_isview;
        public int Cust_IsView
        {
            get { return _cust_isview; }
            set { _cust_isview = value; }
        }
        /// <summary>
        /// Cust_IsLessMoney
        /// </summary>		
        private string _cust_islessmoney;
        public string Cust_IsLessMoney
        {
            get { return _cust_islessmoney; }
            set { _cust_islessmoney = value; }
        }
        /// <summary>
        /// Cust_OperateTime
        /// </summary>		
        private DateTime _cust_operatetime;
        public DateTime Cust_OperateTime
        {
            get { return _cust_operatetime; }
            set { _cust_operatetime = value; }
        }
        /// <summary>
        /// Cust_TSNatrue
        /// </summary>		
        private string _cust_tsnatrue;
        public string Cust_TSNatrue
        {
            get { return _cust_tsnatrue; }
            set { _cust_tsnatrue = value; }
        }
        /// <summary>
        /// Cust_TSSource
        /// </summary>		
        private string _cust_tssource;
        public string Cust_TSSource
        {
            get { return _cust_tssource; }
            set { _cust_tssource = value; }
        }
        /// <summary>
        /// Cust_KF_Remark
        /// </summary>		
        private string _cust_kf_remark;
        public string Cust_KF_Remark
        {
            get { return _cust_kf_remark; }
            set { _cust_kf_remark = value; }
        }
        /// <summary>
        /// Cust_KF_UserName
        /// </summary>		
        private string _cust_kf_username;
        public string Cust_KF_UserName
        {
            get { return _cust_kf_username; }
            set { _cust_kf_username = value; }
        }
        /// <summary>
        /// DataState
        /// </summary>		
        private int _datastate;
        public int DataState
        {
            get { return _datastate; }
            set { _datastate = value; }
        }
        /// <summary>
        /// JoinMan
        /// </summary>		
        private string _joinman;
        public string JoinMan
        {
            get { return _joinman; }
            set { _joinman = value; }
        }
        /// <summary>
        /// JoinDate
        /// </summary>		
        private DateTime _joindate;
        public DateTime JoinDate
        {
            get { return _joindate; }
            set { _joindate = value; }
        }
        /// <summary>
        /// fromPage 标识点击修改来源
        /// </summary>		
        private string _fromPage;
        public string fromPage
        {
            get { return _fromPage; }
            set { _fromPage = value; }
        }
        /// <summary>
        /// platForm 平台名称 0 诚信通 1维权通 2名企云 
        /// </summary>		
        private string _platForm;
        public string platForm
        {
            get { return _platForm; }
            set { _platForm = value; }
        }
        /// <summary>
        /// 是否验证资料
        /// </summary>		
        private string _isCheck;
        public string isCheck
        {
            get { return _isCheck; }
            set { _isCheck = value; }
        }
        //号码归属地 add 2020.4.9
        public string Cust_BelongProvinceName { set; get; }
        public string Cust_BelongProvinceCode { set; get; }

        public string Cust_BelongCityName { set; get; }

        public string Cust_BelongCityCode { set; get; }
    }
}

