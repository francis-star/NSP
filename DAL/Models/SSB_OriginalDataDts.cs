using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class SSB_OriginalDataDts
    {
        public string ODD_Code { get; set; }

        public string ODD_OD_Code { get; set; }

        public string ODD_Name { get; set; }

        public string ODD_Phone { get; set; }

        public string ODD_Address { get; set; }

        public string ODD_LinkMan { get; set; }

        public string ODD_LinkPhone { get; set; }

        public string ODD_IsBill { get; set; }

        public string ODD_Type { get; set; }

        public string ODD_Business { get; set; }

        public string DataState { get; set; }

        public string JoinMan { get; set; }

        public DateTime JoinDate { get; set; }

        public string IsMoveValidData { get; set; }

        public string IsMoveData { get; set; }

        public string DuplicateData { get; set; }

        public string ApprovedUser { get; set; }

        public string UnsubscribedUser { get; set; }

        public string RefundUser { get; set; }

        public string BlackList { get; set; }

        public string TSNumber { get; set; }

        public string Keywords_high { get; set; }

        public string Keywords_low { get; set; }

        public string TSNature { get; set; }

        public string TSSource { get; set; }


        public DateTime OpenDate { get; set; }

        public DateTime UnsubscribeTime { get; set; }

        public string RepetitionType { get; set; }
    }
}
