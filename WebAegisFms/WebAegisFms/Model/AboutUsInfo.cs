using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class AboutUsInfo
    {
        [Required(ErrorMessage="Please enter about us 1")]
        public string AboutUs1 { get; set; }
        [Required(ErrorMessage = "Please enter about us 1")]
        public string AboutUs2 { get; set; }
        public string AboutUs3 { get; set; }
        public int AboutUsId { get; set; }
        public string LoginUser { get; set; }
    }
}
