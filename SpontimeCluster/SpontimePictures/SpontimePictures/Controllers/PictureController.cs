using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SpontimePictures.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;

namespace SpontimePictures.Controllers
{
    [Authorize]
    public class PictureController : ApiController
    {
       // [AllowAnonymous]
        [Route("UploadAvatar")]
        public async Task<resultStatus> PostFormData()
        {

            resultStatus ret = new resultStatus();
            ret.success = true;


            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.

            string currentUserID = HttpContext.Current.User.Identity.GetUserId();
            string currentUserName = HttpContext.Current.User.Identity.GetUserName();

            string containerString = CryptographyModel.GetUserContainer(currentUserName, currentUserID);

            System.Text.StringBuilder str = new System.Text.StringBuilder(CryptographyModel.GetUserAvatarHash(currentUserName, currentUserID));
            str.Append(".png");
            string blobString = str.ToString();

            CloudBlobContainer container = blobClient.GetContainerReference(containerString);

            container.CreateIfNotExists();



            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                BlobContainerPublicAccessType.Blob
                });

            //  string root = HttpContext.Current.Server.MapPath("~/App_Data");


            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                ret.success = false;
                ret.actionRes = "jednak tu: " + System.Net.HttpStatusCode.UnsupportedMediaType.ToString();
                return ret;
                //throw new HttpResponseException(System.Net.HttpStatusCode.UnsupportedMediaType);
            }




            // await Request.Content.ReadAsMultipartAsync(provider);


            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);


                foreach (var file in provider.Contents)
                {
                    //var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();
                    //Do whatever you want with filename and its binaray data.

                    Stream stream = new MemoryStream(buffer);

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobString);
                    if (CryptographyModel.FileIsWebFriendlyImage(stream))
                        await blockBlob.UploadFromStreamAsync(stream);
                    else
                    {
                        ret.success = false;
                        ret.actionRes = "bad format";
                    }

                }


                //var User = System.Web.HttpContext.Current.User;

                str = new System.Text.StringBuilder("https://st-cdn-scus.azureedge.net/");
                str.Append(containerString);
                str.Append("/");
                str.Append(blobString);
                
               

                using (var db = new ApplicationDbContext()) {
                 
                    var userId = User.Identity.GetUserId();
                    var usr = db.Users.Find(userId);
                    usr.AvatarUrl = str.ToString();
                    await db.SaveChangesAsync();

                }
                //var manager = new Microsoft.AspNet.Identity.UserManager<User,>(store,);

                //using (var db = new join_dbEntities())
                //{
                //    AspNetUser user = await db.AspNetUsers.FindAsync(currentUserID);
                //    
                //}

                ret.actionRes= str.ToString();
                return ret;


            }

            catch (System.Exception e)
            {
                ret.success = false;
                ret.actionRes = "tu sie jeblo" + e.Message.ToString() + " " + e.InnerException.Message.ToString();
                return ret;
            }

        }


        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


    }
}
