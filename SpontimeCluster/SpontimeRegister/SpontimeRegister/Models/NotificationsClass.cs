using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.NotificationHubs;
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



    }




}