using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AF_User
    {
        /// <summary>
        /// 编码
        /// </summary>		
        private string _user_Code;
        public string User_Code
        {
            get { return _user_Code; }
            set { _user_Code = value; }
        }

        /// <summary>
        /// 登录名
        /// </summary>		
        private string _user_LoginName;
        public string User_LoginName
        {
            get { return _user_LoginName; }
            set { _user_LoginName = value; }
        }

        /// <summary>
        /// 登录密码
        /// </summary>		
        private string _user_Password;
        public string User_Password
        {
            get { return _user_Password; }
            set { _user_Password = value; }
        }

        /// <summary>
        /// 姓名
        /// </summary>		
        private string _user_Name;
        public string User_Name
        {
            get { return _user_Name; }
            set { _user_Name = value; }
        }
        /// <summary>
        /// 性别
        /// </summary>		
        private string _user_Sex;
        public string User_Sex
        {
            get { return _user_Sex; }
            set { _user_Sex = value; }
        }
        /// <summary>
        /// 年龄
        /// </summary>		
        private int _user_Age;
        public int User_Age
        {
            get { return _user_Age; }
            set { _user_Age = value; }
        }
        /// <summary>
        /// 电话
        /// </summary>		
        private string _user_Phone;
        public string User_Phone
        {
            get { return _user_Phone; }
            set { _user_Phone = value; }
        }
        /// <summary>
        /// 职位
        /// </summary>		
        private string _user_Post;
        public string User_Post
        {
            get { return _user_Post; }
            set { _user_Post = value; }
        }
        /// <summary>
        /// 入职时间
        /// </summary>		
        private string _user_EntryDate;
        public string User_EntryDate
        {
            get { return _user_EntryDate; }
            set { _user_EntryDate = value; }
        }
        /// <summary>
        /// 工作地点
        /// </summary>		
        private string _user_Place;
        public string User_Place
        {
            get { return _user_Place; }
            set { _user_Place = value; }
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
        private string _joindate;
        public string JoinDate
        {
            get { return _joindate; }
            set { _joindate = value; }
        }
    }
     
    public class BatchChargeData
    {
        public string custName { set; get; }

        public string billNo { set; get; }

        public string activeDate { set; get; }
    }
}
