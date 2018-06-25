using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Http.ModelBinding;
using System.Diagnostics;
using System.Collections.Generic;
using SpontimeRegister.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;

namespace SpontimeRegister.Controllers
{
    [Authorize]
    [RoutePrefix("Notifications")]
    public class NotificationsController : ApiController
    {

        private NotificationHubClient hub;

        public NotificationsController()
        {
            hub = NotificationsClass.Notifications.Instance.Hub;
        }



        [Route("RegisterDevice")]
        // This creates or updates a registration (with provided channelURI) at the specified id
        public async Task<NotificationRegisterReturn> RegisterDevice(NotificationRegisterInput deviceUpdate)
        {
            NotificationRegisterReturn ret = new NotificationRegisterReturn();

            ret.success = true;
            ret.actionRes = "";

            if (!ModelState.IsValid)
            {

                foreach (ModelState modelState in ModelState.Values)
                    foreach (ModelError err in modelState.Errors)
                        ret.actionRes += (err.ErrorMessage + " ");

                ret.success = false;
                return ret;
            }

            string currUserId = HttpContext.Current.User.Identity.GetUserId();
            RegistrationDescription registration = null;

            using (var db = new ApplicationDbContext())
            {

            var me =  await db.Users.FirstOrDefaultAsync(u => u.Id == currUserId);

                switch (deviceUpdate.DeviceSys)
                {
                    //case "mpns":
                    //    registration = new MpnsRegistrationDescription(deviceUpdate.ChannelURI);
                    //    break;
                    case "wns":
                        registration = new WindowsRegistrationDescription(deviceUpdate.ChannelURI);
                        me.Wns = true;
                        break;
                    case "apns":
                        registration = new AppleRegistrationDescription(deviceUpdate.ChannelURI);
                        me.Apns = true;
                        break;
                    case "gcm":
                        registration = new GcmRegistrationDescription(deviceUpdate.ChannelURI);
                        me.Gcm = true;
                        break;
                    default:
                        ret.success = false;
                        ret.actionRes += "DeviceSys: mpns/wns/apns/gcm";
                        return ret;
                }
                await db.SaveChangesAsync();
                registration.RegistrationId = deviceUpdate.RegistrationId;
            }




            // add check if user is allowed to add these tags

            System.Text.StringBuilder str = new System.Text.StringBuilder(currUserId);
            str.Append(deviceUpdate.DeviceSys);

            registration.Tags = new HashSet<string>();
            registration.Tags.Add(str.ToString());
            // registration.ExpirationTime

            try
            {
                await hub.CreateOrUpdateRegistrationAsync(registration);
            }
            catch (MessagingException e)
            {
                ReturnGoneIfHubResponseIsGone(e);
            }




            return ret;
        }


        // POST api/register
        // This creates a registration id
        [Route("GetRegisterId")]
        public async Task<DeviceRegistrationReturn> Post([FromBody]DeviceRegistration input)
        {

            DeviceRegistrationReturn ret = new DeviceRegistrationReturn();
            ret.success = true;
            ret.actionRes = "";
            ret.RegistrationId = null;
            // string newRegistrationId = null;

            // make sure there are no existing registrations for this push handle (used for iOS and Android)
            if (input != null)
            {
                var registrations = await hub.GetRegistrationsByChannelAsync(input.channelURI, 100);


                foreach (RegistrationDescription registration in registrations)
                {
                    if (ret.RegistrationId == null)
                    {
                        ret.RegistrationId = registration.RegistrationId;
                    }
                    else
                    {
                        await hub.DeleteRegistrationAsync(registration);
                    }
                }
            }

            if (ret.RegistrationId == null)
                ret.RegistrationId = await hub.CreateRegistrationIdAsync();

            if (ret.RegistrationId == "" || ret.RegistrationId == null)
            {
                ret.success = false;
                ret.actionRes = "Something went wrong";
                ret.RegistrationId = "";

            }

            return ret;
        }






        // DELETE api/register/5
        public async Task<HttpResponseMessage> Delete(string id)
        {
            await hub.DeleteRegistrationAsync(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private static void ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if (webex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webex.Response;
                if (response.StatusCode == HttpStatusCode.Gone)
                    throw new HttpRequestException(HttpStatusCode.Gone.ToString());
            }
        }


    }
}
