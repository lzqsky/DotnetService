using System;
using System.Collections.Generic;
using System.Text;

namespace RegistBusiness.Model
{
    public class RegisterModel
    {
        public string RID { get; set; }
        public string StatusStr { get; set; }
        public string HospitalName { get; set; }
        public string OfficeName { get; set; }
        public string TechnicianName { get; set; }
        public string AtionCode { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
