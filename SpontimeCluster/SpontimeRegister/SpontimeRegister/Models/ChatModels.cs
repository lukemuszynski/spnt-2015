using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SpontimeRegister.Models
{
    public class ChatModel
    {

        public static string QBToken;
        public DateTime expireDate;
        const string AppSecret = "VhJ9XhP8nmKcOZV";



        public static async Task<int> CreateSessionWithUser()
        {



            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            // long unixTimeStampInSeconds1 = dateTimeOffset.ToUnixTimeMilliseconds();
            long unixTimeStampInSeconds = dateTimeOffset.ToUnixTimeSeconds();
            Random r = new Random();
            string rand = r.Next().ToString();

            //'application_id=38665&auth_key=bGUVnmzDMRtHUZ8&nonce=33432&timestamp=1326966962'

            StringBuilder signature = new StringBuilder("application_id=38727&auth_key=&");


            signature.Append("nonce=");
            signature.Append(rand);
            signature.Append("&timestamp=");
            signature.Append(unixTimeStampInSeconds.ToString());
            signature.Append("&user[login]=test&user[password]=JustFreakinWork!");
            //signature.Append("'");

            string signatureSha = CryptographyClass.hmacsha1(signature.ToString(), AppSecret);
            signatureSha = signatureSha.ToLower();
            var client = new RestClient("https://api.quickblox.com/session");
            var request = new RestRequest(Method.POST);


            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("quickblox-rest-api-version", "0.1.1");

            request.AddParameter("application/json", "{\"application_id\": \"38727\", \"auth_key\": \"\", \"timestamp\": \"" + unixTimeStampInSeconds.ToString() + "\", \"nonce\": \"" + rand + "\", \"signature\": \"" + signatureSha + "\", \"user\": {\"login\": \"test\", \"password\": \"JustFreakinWork!\"}}", ParameterType.RequestBody);

            request.RequestFormat = RestSharp.DataFormat.Json;
            IRestResponse response = client.Execute(request);

            int start = response.Content.IndexOf("<token>");
            int end = response.Content.IndexOf("</token>");

            QBToken = response.Content.Substring(start + 7, end - start - 7);



            return 0;

        }

        public static async Task<string> SignUpForChat(ApplicationUser user)
        {



            string chatPassword = CryptographyClass.chatPasswordHash(user);

            var client = new RestClient("http://api.quickblox.com/users");
            var request = new RestRequest(Method.POST);



            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("quickblox-rest-api-version", "0.1.1");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("QB-Token", QBToken);
            request.AddParameter("application/json", "{\"user\": { \"login\": \"" + user.Id + "\", \"password\": \"" + chatPassword + "\",    \"facebook_id\": \"" + user.UserName + "\",  \"full_name\": \"" + user.Id + "\"}}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);




            return chatPassword;

        }
    }
}