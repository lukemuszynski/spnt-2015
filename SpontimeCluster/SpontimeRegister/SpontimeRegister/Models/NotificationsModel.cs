using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.NotificationHubs;
using System.Text;
using Newtonsoft.Json;

namespace SpontimeRegister.Models
{
    public class NotificationsClass
    {

        public class Notifications
        {
            public static Notifications Instance = new Notifications();



            public NotificationHubClient Hub { get; set; }

            private Notifications()
            {
                Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://spontimenotifications.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey==",
                                                                             "spontimenotification");
            }
        }






        public async static Task<bool> RegisterPlatform(string UserId, string platform)
        {


            var user = await UserManager.FindByIdAsync(model.FacebookId);

            //  var userEntity = await db.Users.FindAsync(UserId);



            if (platform == "apns") userEntity.Apns = true;
                else if (platform == "gcm") userEntity.Gcm = true;
                else if (platform == "wns") userEntity.Wns = true;


                await db.SaveChangesAsync();



            return true;

        }







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

    }
}