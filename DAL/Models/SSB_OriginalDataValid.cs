using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class SSB_OriginalDataValid
    {

        public string ODD_Code { get; set; }

        public string ODD_OD_Code { get; set; }

        public string ODD_Name { get; set; }

        public string ODD_Phone { get; set; }

        public string ODD_Address { get; set; }

        public string ODD_LinkMan { get; set; }

        public string ODD_LinkPhone { get; set; }

        public string ODD_IsBill { get; set; }

        public string ODD_CityCode { get; set; }

        public int DataState { get; set; }

        public string JoinMan { get; set; }

        public DateTime JoinDate { get; set; }

        public int IsMoveValidData { get; set; }
    }
}
