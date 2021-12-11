using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterLoginAPIcode.Models
{
    public class Register
    {

        public string name { get; set; }
        public string mobile { get; set; }
        [Required(ErrorMessage = "otp is required")]
        public string otp { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string email { get; set; }
       
    }
    public class validOTP
    {
        public string mobile { get; set; }    
        public string otp { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string name { get; set; }



    }
    public class validOTP_login
    {
        public string mobile { get; set; }
        public string otp { get; set; }
       


    }
    public class sendOTP
    {
        public string mobile { get; set; }
       
    }
}
