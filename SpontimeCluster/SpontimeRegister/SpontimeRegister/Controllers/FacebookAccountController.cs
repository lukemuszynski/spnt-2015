using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Configuration;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using Facebook;
using Newtonsoft.Json.Linq;
using SpontimeRegister.Models;
using SpontimeRegister;

namespace join_backend.Controllers
{
    [Authorize]
    [RoutePrefix("FacebookAccount")]
    public class FacebookAccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public FacebookAccountController()
        {
        }

        public FacebookAccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        [Route("RefreshFacebookAccessToken")]
        [AllowAnonymous]
        public async Task<HttpResult> ChangeFacebookAccessToken(RegisterFacebookModel model)
        {
           HttpResult ret = new HttpResult() { errors = new List<string>(), success = true };

            if (!ModelState.IsValid)
            {
                ret.errors.Add("ModelState Error");
                ret.success = false;
                //return BadRequest(ModelState);
                return ret;
            }


            FacebookClient newUserFacebookData = new FacebookClient(model.FacebookAccessToken);
            //IDictionary<string, object> result; //= (IDictionary<string, object>)e.GetResultData();
            string id = "";
          

            newUserFacebookData.GetCompleted +=
                    (o, e) =>
                    {

                        if (e.Cancelled)
                        {
                            // request was cancelled
                            return;
                        }

                        var ex = e.Error;
                        if (ex != null)
                        {
                            if (ex is FacebookOAuthException)
                            {
                                ret.errors.Add("FbOAuthException");
                                // oauth exception occurred
                            }
                            else if (ex is FacebookApiLimitException)
                            {
                                ret.errors.Add("FacebookApiLimitException");
                                // api limit exception occurred.
                            }
                            else if (ex is FacebookApiException)
                            {
                                ret.errors.Add("FacebookApiException");
                                // other general facebook api exception
                            }
                            else
                            {
                                // non-facebook exception such as no internet connection.
                            }

                            return;
                        }


                        var result = (IDictionary<string, object>)e.GetResultData();
                        id = (string)result["id"];

                    };


            await newUserFacebookData.GetTaskAsync("me");


            var user = await UserManager.FindByNameAsync(id);

            if (user == null)
            {
                ret.errors.Add("No user with this fb id");
                ret.success = false;
                return ret;
            }

            IdentityResult IdentityResult = await UserManager.RemovePasswordAsync(user.Id);

            //UserManager.GenerateUserTokenAsync("Bearer", "sdfsdfsd");



            if (!IdentityResult.Succeeded)
            {
                ret.success = false;
                // return GetErrorResult(result);
                foreach (var a in IdentityResult.Errors)
                {
                    ret.errors.Add(a);
                }
            }

            IdentityResult = await UserManager.AddPasswordAsync(user.Id, model.FacebookAccessToken);

            if (!IdentityResult.Succeeded)
            {
                ret.success = false;
                // return GetErrorResult(result);
                foreach (var a in IdentityResult.Errors)
                {
                    ret.errors.Add(a);
                }
            }


            return ret;
        }

        
        [AllowAnonymous]
        [Route("RegisterWithFacebook")]
        public async Task<HttpResult> RegisterWithFacebook(RegisterFacebookModel model)
        {
            HttpResult ret = new HttpResult();
            ret.success = true;
            ret.errors = new List<String>();


            if (!ModelState.IsValid)
            {
                ret.errors.Add("ModelState Error");
                ret.success = false;
                //return BadRequest(ModelState);
                return ret;
            }


            FacebookClient newUserFacebookData = new FacebookClient(model.FacebookAccessToken);
            //IDictionary<string, object> result; //= (IDictionary<string, object>)e.GetResultData();
            string id = "";
            string firstName = "";
            string lastName = "";
            

            string email = "";

            newUserFacebookData.GetCompleted +=
                    (o, e) =>
                    {

                        if (e.Cancelled)
                        {
                            // request was cancelled
                            return;
                        }

                        var ex = e.Error;
                        if (ex != null)
                        {
                            if (ex is FacebookOAuthException)
                            {
                                ret.errors.Add("FbOAuthException");
                                ret.success = false;
                                // oauth exception occurred
                            }
                            else if (ex is FacebookApiLimitException)
                            {
                                ret.errors.Add("FacebookApiLimitException");
                                ret.success = false;

                                // api limit exception occurred.
                            }
                            else if (ex is FacebookApiException)
                            {
                                ret.errors.Add("FacebookApiException");
                                ret.success = false;

                                // other general facebook api exception
                            }
                            else
                            {
                                ret.errors.Add("Something went wrong");
                                ret.success = false;
                                // non-facebook exception such as no internet connection.
                            }

                            return;
                        }

                        else {

                            var result = (IDictionary<string, object>)e.GetResultData();
                            id = (string)result["id"];
                            firstName = (string)result["first_name"];
                            lastName = (string)result["last_name"];
                            // link = (string)result["link"];
                            try {
                                email = (string)result["email"];
                            }
                            catch
                            {
                                email = "";
                            }
                        }
                    };

            if (ret.errors.Count > 0)
            {
                ret.success = false;
                return ret;
            }

            await newUserFacebookData.GetTaskAsync("me?fields=id,name,email,first_name,last_name");

            if (ret.errors.Count > 0)
            {
                ret.success = false;
                return ret;
            }


            System.Text.StringBuilder picLink = new System.Text.StringBuilder("https://graph.facebook.com/");
            picLink.Append(id);
            picLink.Append("/picture?type=large");



            var user = new ApplicationUser()
            {
                UserName = id,
                Email = email,
                NickName = lastName,
                FirstName = firstName,
                LastName = lastName,
                AvatarUrl = picLink.ToString()
            };

            IdentityResult IdentityResult = await UserManager.CreateAsync(user, model.FacebookAccessToken);

            
            if (!IdentityResult.Succeeded)
            {
                ret.success = false;
                // return GetErrorResult(result);
                foreach (var a in IdentityResult.Errors)
                {

                    ret.errors.Add(a);
                }
            }



            return ret;
        }



   

        [AllowAnonymous]
        [Route("RegisterOrRefresh")]
        public async Task<LoginReturn> FacebookLoginHub([FromBody]FacebookLoginModel model)
        {

            LoginReturn ret = new LoginReturn();
           
            

            if (!ModelState.IsValid)
            {
                ret.errors.Add("ModelState Error");
                ret.success = false;
                //return BadRequest(ModelState);
                return ret;
            }

            var user = await UserManager.FindByNameAsync(model.FacebookId);


            if (user == null)
            {
                RegisterFacebookModel m = new RegisterFacebookModel();
                m.FacebookAccessToken = model.FacebookAccessToken;
                var reg = await RegisterWithFacebook(m);

                if (reg.success == false)
                {

                    ret.success = false;
                    ret.errors = reg.errors;
                    return ret;

                }

                user = await UserManager.FindByNameAsync(model.FacebookId);

                await ChatModel.CreateSessionWithUser();
                await ChatModel.SignUpForChat(user);

              // await  ChatModel.SignUpForChat(user);

            }

            ///Must be registered by now



            ///Refresh Tokena

            if(! await UserManager.CheckPasswordAsync(user,model.FacebookAccessToken))
            {
                RegisterFacebookModel m = new RegisterFacebookModel();
                m.FacebookAccessToken = model.FacebookAccessToken;
                var reg = await ChangeFacebookAccessToken(m);

                if (reg.success == false)
                {

                    ret.success = false;
                    ret.errors = reg.errors;
                    return ret;

                }

            }

            
            return ret;

        }
        

        [HttpGet]
        [Route("ChatSession")]
        public async Task<string> CreateChatSession()
        {
            string userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            int ret = await ChatModel.CreateSessionWithUser();
          //  ret += await ChatModel.SingUp();
          string passwd =  await ChatModel.SignUpForChat(user);

            return passwd;

        }
        


    }
}
