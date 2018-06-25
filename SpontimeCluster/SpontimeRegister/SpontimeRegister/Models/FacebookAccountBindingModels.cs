using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpontimeRegister.Models
{
  

        public class RegisterFacebookModel
        {

            [Required]
            [Display(Name = "FacebookAccessToken")]
            [StringLength(500)]
            public string FacebookAccessToken { get; set; }

        }


        public class FacebookLoginModel
        {

            [Required]
            [Display(Name = "FacebookId")]
            [StringLength(50)]
            public string FacebookId { get; set; }


            [Required]
            [Display(Name = "FacebookAccessToken")]
            [StringLength(500)]
            public string FacebookAccessToken { get; set; }

        }


    
}