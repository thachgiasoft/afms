using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ContactUsInfo
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage="Please enter Company Name.")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Please enter Contact Number.")]
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        [Required(ErrorMessage = "Please enter Address Line 1.")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string Zip { get; set; }
        [Required(ErrorMessage = "Please enter Email Id.")]
        public string Email { get; set; }
        public string GoogleMapUrl { get; set; }
        public string LoginId { get; set; }        
    }
}
