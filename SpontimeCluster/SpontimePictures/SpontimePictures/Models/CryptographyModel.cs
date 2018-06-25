using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SpontimePictures.Models
{
    public class CryptographyModel
    {
        public static string GetUserAvatarHash(string UserName, string UserId)
        {

            StringBuilder str = new StringBuilder(DateTime.UtcNow.ToShortTimeString());

            str.Append(UserName);
            str.Append("seemsLegit:D");
            str.Append("UserId");
            str.Append(DateTime.UtcNow.ToShortDateString());
            str.Append("jakiesZakonczenie12#%");
            return sha256(str.ToString());

        }

        public static string GetUserContainer(string UserName, string UserId)
        {


            StringBuilder str = new StringBuilder(UserId);
            str.Append("takichu@#jezebedzietosamo$@%#!");
            str.Append(UserId);
            str.Append("tutsasfj#$@$&($@#$");
            return sha256(str.ToString()).Substring(5, 40);

            //  return sha256(str.ToString()).Substring(5,40);

        }

        public static string sha256(string password)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();

            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static bool FileIsWebFriendlyImage(Stream stream)
        {
            try
            {
                //Read an image from the stream...
                var i = Image.FromStream(stream);

                //Move the pointer back to the beginning of the stream
                stream.Seek(0, SeekOrigin.Begin);

                if (ImageFormat.Jpeg.Equals(i.RawFormat))
                    return true;
                return ImageFormat.Png.Equals(i.RawFormat)
              || ImageFormat.Gif.Equals(i.RawFormat);

                

            }
            catch
            {
                stream.Seek(0, SeekOrigin.Begin);
                return false;
            }





        }


    }

    class PublicViewForFriends
    {

        
        public void  Method(string xaxax)
        {

            int x = 0;



        }


    }

}

