using System;
using System.Collections.Generic;
using System.Text;

namespace RegistBusiness.Model
{
    public  class HospitalModel
    {
        public string HID { get; set; }
        public string HName { get; set; }
        public string Description { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ContactGroupId { get; set; }
    }
}
