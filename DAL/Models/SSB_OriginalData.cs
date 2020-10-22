using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class SSB_OriginalData
    {
        public string OD_Code { get; set; }

        public string OD_Provider { get; set; }

        public DateTime OD_ProviderTime { get; set; }

        public string OD_ProvinceCode { get; set; }

        public string OD_ProvinceName { get; set; }

        public string OD_CityCode { get; set; }

        public string OD_CityName { get; set; }

        public int OD_BillMoney { get; set; }

        public int OD_TotalCount { get; set; }

        public int OD_ValidCount { get; set; }

        public int OD_NoValidCount { get; set; }

        public int OD_AlreadyUse { get; set; }

        public string OD_State { get; set; }

        public int DataState { get; set; }

        public string JoinMan { get; set; }

        public DateTime JoinDate { get; set; }
    }
}
