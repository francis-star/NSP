using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    //XXSD_PublicInfo
    public class XXSD_PublicInfo
    {

        /// <summary>
        /// Code
        /// </summary>		
        private string _pub_code;
        public string Pub_Code
        {
            get { return _pub_code; }
            set { _pub_code = value; }
        }
        /// <summary>
        /// 移动端Code001
        /// </summary>		
        private string _pub_ls_code1;
        public string Pub_LS_Code1
        {
            get { return _pub_ls_code1; }
            set { _pub_ls_code1 = value; }
        }
        /// <summary>
        /// 平台名称Code002
        /// </summary>		
        private string _pub_ls_code2;
        public string Pub_LS_Code2
        {
            get { return _pub_ls_code2; }
            set { _pub_ls_code2 = value; }
        }
        /// <summary>
        /// 信息类别Code003
        /// </summary>		
        private string _pub_ls_code3;
        public string Pub_LS_Code3
        {
            get { return _pub_ls_code3; }
            set { _pub_ls_code3 = value; }
        }
        /// <summary>
        /// 信息大类Code004	
        /// </summary>		
        private string _pub_ls_code4;
        public string Pub_LS_Code4
        {
            get { return _pub_ls_code4; }
            set { _pub_ls_code4 = value; }
        }
        /// <summary>
        /// 信息小类Code3
        /// </summary>		
        private string _pub_ls_code5;
        public string Pub_LS_Code5
        {
            get { return _pub_ls_code5; }
            set { _pub_ls_code5 = value; }
        }
        /// <summary>
        /// 所在省
        /// </summary>		
        private string _pub_sa_code1;
        public string Pub_SA_Code1
        {
            get { return _pub_sa_code1; }
            set { _pub_sa_code1 = value; }
        }
        /// <summary>
        /// 所在市
        /// </summary>		
        private string _pub_sa_code2;
        public string Pub_SA_Code2
        {
            get { return _pub_sa_code2; }
            set { _pub_sa_code2 = value; }
        }
        /// <summary>
        /// 所在乡镇
        /// </summary>		
        private string _pub_sa_code3;
        public string Pub_SA_Code3
        {
            get { return _pub_sa_code3; }
            set { _pub_sa_code3 = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>		
        private string _pub_title;
        public string Pub_Title
        {
            get { return _pub_title; }
            set { _pub_title = value; }
        }
        /// <summary>
        /// 封面图片1
        /// </summary>		
        private string _pub_pic1;
        public string Pub_Pic1
        {
            get { return _pub_pic1; }
            set { _pub_pic1 = value; }
        }
        /// <summary>
        /// 封面图片2
        /// </summary>		
        private string _pub_pic2;
        public string Pub_Pic2
        {
            get { return _pub_pic2; }
            set { _pub_pic2 = value; }
        }
        /// <summary>
        /// 封面图片3
        /// </summary>		
        private string _pub_pic3;
        public string Pub_Pic3
        {
            get { return _pub_pic3; }
            set { _pub_pic3 = value; }
        }
        /// <summary>
        /// 内容
        /// </summary>		
        private string _pub_content;
        public string Pub_Content
        {
            get { return _pub_content; }
            set { _pub_content = value; }
        }
        /// <summary>
        /// 文章出处
        /// </summary>		
        private string _pub_articlesource;
        public string Pub_ArticleSource
        {
            get { return _pub_articlesource; }
            set { _pub_articlesource = value; }
        }
        /// <summary>
        /// 关键字,以;分割
        /// </summary>		
        private string _pub_keywords;
        public string Pub_KeyWords
        {
            get { return _pub_keywords; }
            set { _pub_keywords = value; }
        }
        /// <summary>
        /// 阅读数
        /// </summary>		
        private int _pub_readcount;
        public int Pub_ReadCount
        {
            get { return _pub_readcount; }
            set { _pub_readcount = value; }
        }
        /// <summary>
        /// 赞数
        /// </summary>		
        private int _pub_praisecount;
        public int Pub_PraiseCount
        {
            get { return _pub_praisecount; }
            set { _pub_praisecount = value; }
        }
        /// <summary>
        /// 信息状态
        /// </summary>		
        private int _datastate;
        public int DataState
        {
            get { return _datastate; }
            set { _datastate = value; }
        }
        /// <summary>
        /// 信息录入时间
        /// </summary>		
        private DateTime _joindate;
        public DateTime JoinDate
        {
            get { return _joindate; }
            set { _joindate = value; }
        }
        /// <summary>
        /// 信息录入人
        /// </summary>		
        private string _joinman;
        public string JoinMan
        {
            get { return _joinman; }
            set { _joinman = value; }
        }

        /// <summary>
        /// 所在省名称
        /// </summary>		
        private string _pub_SA_Name1;
        public string Pub_SA_Name1
        {
            get { return _pub_SA_Name1; }
            set { _pub_SA_Name1 = value; }
        }

        /// <summary>
        /// 所在市名称
        /// </summary>		
        private string _pub_SA_Name2;
        public string Pub_SA_Name2
        {
            get { return _pub_SA_Name2; }
            set { _pub_SA_Name2 = value; }
        }

        /// <summary>
        /// 所在区名称
        /// </summary>		
        private string _pub_SA_Name3;
        public string Pub_SA_Name3
        {
            get { return _pub_SA_Name3; }
            set { _pub_SA_Name3 = value; }
        }
    }
}
