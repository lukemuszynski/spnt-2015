using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpontimeRegister.Models
{
    public class DeviceRegistration
    {
        [Required]
        public string channelURI { get; set; }
    }


    public class DeviceRegistrationReturn
    {

        public string RegistrationId { get; set; }


        public string actionRes { get; set; }
        public Boolean success { get; set; }


    }


    public class DeviceReg
    {
        [Required]
        [MaxLength(300)]
        public string channelURI { get; set; }

        [Required]
        [MaxLength(5)]
        public string DeviceSys { get; set; }

    }



    public class NotificationRegisterInput
    {
        [Required]
        [MaxLength(5)]
        public string DeviceSys { get; set; }

        /* id */
        [Required]
        [MaxLength(50)]
        [MinLength(30)]
        public string RegistrationId { get; set; }

        [Required]
        public string ChannelURI { get; set; }

    }



    public class NotificationRegisterReturn
    {


        public string actionRes;
        public Boolean success;


    }

    public class LoginReturn
    {

        public LoginReturn()
        {
            //  Token = "";
            success = true;
            errors = new List<string>();

        }

        // public string Token { get; set; }
        public bool success { get; set; }
        public List<string> errors { get; set; }

    }


    public class HttpResult
    {
        public List<String> errors;
        public Boolean success;
    }


}
