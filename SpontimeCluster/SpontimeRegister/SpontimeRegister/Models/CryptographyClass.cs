using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SpontimeRegister.Models
{

        public static class CryptographyClass
        {

            public static string hmacsha1(string message, string key)
            {

                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(key);
                HMACSHA1 hmac = new HMACSHA1(keyByte);
                byte[] messageBytes = encoding.GetBytes(message);
                byte[] hashmessage = hmac.ComputeHash(messageBytes);

                return ByteToString(hashmessage);

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


            public static string ByteToString(byte[] buff)
            {
                string sbinary = "";
                for (int i = 0; i < buff.Length; i++)
                {
                    sbinary += buff[i].ToString("X2"); // hex format
                }
                return (sbinary);
            }


            public static string chatPasswordHash(ApplicationUser user)
            {

                StringBuilder str = new StringBuilder("");
                str.Append(user.UserName);
                str.Append("");

                return sha256(str.ToString()).Substring(0, 30);


            }


            public static string chatPasswordHash(string username)
            {

                StringBuilder str = new StringBuilder("");
                str.Append(username);
                str.Append("");

                return sha256(str.ToString()).Substring(0, 30);


            }



    }
}