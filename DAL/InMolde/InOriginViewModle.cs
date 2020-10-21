using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.InMolde
{
    public class InOriginViewModle
    {

        public string ODCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustName { get; set; }

        /// <summary>
        ///  计费号码
        /// </summary>
        public string ChargeNumber { get; set; }
        /// <summary>
        /// 重复数据 
        /// </summary>
        public string DuplicateData { get; set; }

        /// <summary>
        ///  已是会员
        /// </summary>
        public string ApprovedUser { get; set; }

        /// <summary>
        /// 退订用户 
        /// </summary>
        public string UnsubscribedUser { get; set; }

        /// <summary>
        /// 退费用户
        /// </summary>
        public string RefundUser { get; set; }

        /// <summary>
        /// 黑名单
        /// </summary>
        public string BlackList { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string LinkPhone { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        ///   是否计费
        /// </summary>
        public string IsCharge { get; set; }

        /// <summary>
        /// 投诉号码
        /// </summary>
        public string TSNumber { get; set; }

        /// <summary>
        /// 关键字(高)
        /// </summary>
        public string Keywords_high { get; set; }

        /// <summary>
        /// 关键字(低)
        /// </summary>
        public string Keywords_low { get; set; }

        /// <summary>
        /// 重复类型
        /// </summary>
        public string RepetitionType { get; set; }

        /// <summary>
        /// 投诉性质
        /// </summary>
        public string TSNature { get; set; }

        /// <summary>
        /// 投诉来源 
        /// </summary>
        public string TSSource { get; set; }

        /// <summary>
        /// 开通时间
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// 退订时间
        /// </summary>
        public DateTime? UnsubscribeTime { get; set; }
        public int PageIndex { get; set; }

        public int PageNum { get; set; }
    }
}
